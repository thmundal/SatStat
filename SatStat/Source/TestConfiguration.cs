using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatStat
{
    /// <summary>
    /// An object of this class holds a queue of Instructions to run and a Parameter control template to use for observing values during a test-run.
    /// An instance of this class can also be saved and/or loaded from a LiteDatabase collection
    /// </summary>
    public class TestConfiguration
    {
        public ObjectId Id { get; set; }
        public List<Instruction> Instructions { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ParameterControlTemplate ParameterControlTemplate { get; set; }

        /// <summary>
        /// Holds the current instruction queue
        /// </summary>
        private Queue<Instruction> instructionQueue;

        /// <summary>
        /// An internal DataReceiver
        /// </summary>
        private DataReceiver internalReceiver;

        /// <summary>
        /// Holds the current instruction
        /// </summary>
        private Instruction currentInstruction;

        /// <summary>
        /// Keeps track of the current position in the queue
        /// </summary>
        private int queue_position = -1;

        /// <summary>
        /// An action to invoke when the queue advances
        /// </summary>
        private Action<Instruction> onQueueAdvanceCallback;

        /// <summary>
        /// A callback to invoke then the queue is finished
        /// </summary>
        private Action<Instruction> onQueueCompleteCallback;

        /// <summary>
        /// Constructor
        /// </summary>
        public TestConfiguration()
        {
            internalReceiver = new DataReceiver();
            instructionQueue = new Queue<Instruction>();
            Instructions = new List<Instruction>();
            ParameterControlTemplate = new ParameterControlTemplate();
        }

        /// <summary>
        /// Add an instruction to the instruction list
        /// </summary>
        /// <param name="instr">The instruction object</param>
        /// <param name="uindex">Optional UI index parameter to keep track of a position in a UI list</param>
        public void AddInstruction(Instruction instr, int uindex = -1)
        {
            instr.UI_Index = uindex;
            Instructions.Add(instr);

        }

        /// <summary>
        /// Start a test configuration run on a particular data stream
        /// </summary>
        /// <param name="stream">The data stream to run the test configuration on</param>
        public void Run(DataStream stream)
        {
            // Reset the queue position and clear the instruction queue. Also sort the instructions list by 
            // their UI index so that we execute the instructions in the correct order
            queue_position = -1;
            List<Instruction> sortedInstructions = Instructions.OrderBy(o => o.UI_Index).ToList();
            instructionQueue.Clear();

            foreach(Instruction instr in sortedInstructions)
            {
                instructionQueue.Enqueue(instr);
            }

            if(ParameterControlTemplate.Collection != null)
            {
                foreach(IObservableNumericValue value in ParameterControlTemplate.Collection)
                {
                    value.type.ToString();
                    internalReceiver.Subscribe(stream, value.Label, value.type.ToString());
                }
            }

            // Tell the internal receiver to observe the values it is getting
            // this will display an error for the attribute "instruction_complete" since
            // this attribute holds a string. Should be fixed in a future update
            internalReceiver.Observe = true;

            internalReceiver.OnPayloadReceived((object payload, string attribute) => {
                OnPayloadReceive(payload);

                if(attribute == "instruction_complete")
                {
                    RunQueuedInstruction(stream);
                }
            });

            internalReceiver.OnObservedvalueChanged((IObservableNumericValue val) =>
            {
                OnObservedValueChange(val);
            });

            if(!internalReceiver.HasSubscription("instruction_complete"))
            {
                internalReceiver.Subscribe(stream, "instruction_complete", "string");
            }

            // Start instruction queue execution
            RunQueuedInstruction(stream);
        }

        /// <summary>
        /// Run the next instruction in the queue if the queue is not empty. Also invokes the callback functions at the right time, if they are set
        /// </summary>
        /// <param name="stream">The stream the instruction should be sent to</param>
        public void RunQueuedInstruction(DataStream stream)
        {
            if(currentInstruction != null)
            {
                // Complete current instruction if there is one, and if there is something to do before executing next instruction
            }

            if(instructionQueue.Count > 0)
            {
                queue_position++;
                currentInstruction = instructionQueue.Dequeue();

                if(onQueueAdvanceCallback != null)
                {
                    onQueueAdvanceCallback.Invoke(currentInstruction);
                }

                stream.Output(currentInstruction.toJObject());

            } else
            {
                if(onQueueCompleteCallback != null)
                {
                    onQueueCompleteCallback.Invoke(currentInstruction);
                }
                queue_position = -1;
                Debug.Log("Instruction queue is empty");
                // Queue is empty
            }
        }

        /// <summary>
        /// A method to invoke once payload is received on the internal receiver
        /// </summary>
        /// <param name="payload">The payload data</param>
        public void OnPayloadReceive(object payload)
        {

        }

        /// <summary>
        /// A method to invoke once an observable value is changed
        /// </summary>
        /// <param name="val">The observed value</param>
        public void OnObservedValueChange(IObservableNumericValue val)
        {

        }

        /// <summary>
        /// Register a callback to invoke when queue is advanced
        /// </summary>
        /// <param name="callback">The action to invoke</param>
        public void OnQueueAdvance(Action<Instruction> callback)
        {
            onQueueAdvanceCallback = callback;
        }

        /// <summary>
        /// Register a callback to invoke when queue is complete
        /// </summary>
        /// <param name="callback">The action to invoke</param>
        public void OnQueueComplete(Action<Instruction> callback)
        {
            onQueueCompleteCallback = callback;
        }
    }
}
