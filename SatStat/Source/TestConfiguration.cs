using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatStat
{
    public class TestConfiguration
    {

        public ObjectId Id { get; set; }
        public List<Instruction> Instructions { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ParameterControlTemplate ParameterControlTemplate { get; set; }

        private Queue<Instruction> instructionQueue;
        private DataReceiver internalReceiver;
        private Instruction currentInstruction;

        private int queue_position = -1;
        private Action<Instruction> onQueueAdvanceCallback;
        private Action<Instruction> onQueueCompleteCallback;

        public TestConfiguration()
        {
            internalReceiver = new DataReceiver();
            instructionQueue = new Queue<Instruction>();
            Instructions = new List<Instruction>();
            ParameterControlTemplate = new ParameterControlTemplate();
        }

        public void AddInstruction(Instruction instr, int uindex = -1)
        {
            instr.UI_Index = uindex;
            Instructions.Add(instr);

        }

        public void Run(DataStream stream)
        {
            queue_position = -1;
            List<Instruction> sortedInstructions = Instructions.OrderBy(o => o.UI_Index).ToList();
            Debug.Log("Starting test run");
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

        public void OnPayloadReceive(object payload)
        {

        }

        public void OnObservedValueChange(IObservableNumericValue val)
        {

        }

        public void OnQueueAdvance(Action<Instruction> callback)
        {
            onQueueAdvanceCallback = callback;
        }

        public void OnQueueComplete(Action<Instruction> callback)
        {
            onQueueCompleteCallback = callback;
        }
    }
}
