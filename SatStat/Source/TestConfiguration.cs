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
        public string Name { get; set; }
        public string Description { get; set; }
        public ParameterControlTemplate ParameterControlTemplate { get; set; }
        public List<InstructionEntry> instructionEntries { get; set; }

        /// <summary>
        /// Holds the current instruction queue
        /// </summary>
        //private Queue<Instruction> instructionQueue;

        /// <summary>
        /// An internal DataReceiver
        /// </summary>
        private DataReceiver internalReceiver;

        /// <summary>
        /// Holds the current instruction
        /// </summary>
        //private Instruction currentInstruction;

        /// <summary>
        /// Keeps track of the current position in the queue
        /// </summary>
        private int queue_position = -1;

        /// <summary>
        /// An action to invoke when the queue advances
        /// </summary>
        private Action<InstructionEntry> onQueueAdvanceCallback;

        /// <summary>
        /// A callback to invoke then the queue is finished
        /// </summary>
        private Action<InstructionEntry> onQueueCompleteCallback;

        private Action onQueueAbortedCallback;

        private bool is_running = false;

        public bool IsRunning
        {
            get
            {
                return is_running;
            }
        }

        private Queue<InstructionEntry> instructionEntryQueue;
        private InstructionEntry currentInstructionEntry = new InstructionEntry();

        /// <summary>
        /// Constructor
        /// </summary>
        public TestConfiguration()
        {
            internalReceiver = new DataReceiver();
            ParameterControlTemplate = new ParameterControlTemplate();
            instructionEntries = new List<InstructionEntry>();
            instructionEntryQueue = new Queue<InstructionEntry>();

            // Possible copy all instructions into instructionEntries if loaded from DB
        }

        public void AddInstructionEntry(InstructionEntry entry)
        {
            instructionEntries.Add(entry);
        }

        public InstructionEntry AddInstructionEntry(Instruction instr, List<string> observedLabels, int uindex = -1)
        {
            instr._ui_index = uindex;

            InstructionEntry entry = new InstructionEntry
            {
                instruction = instr,
                observedValueLabels = observedLabels
            };
            instructionEntries.Add(entry);

            return entry;
        }

        public void RemoveInstructionEntry(InstructionEntry entry)
        {
            if(instructionEntries.Contains(entry))
            {
                instructionEntries.Remove(entry);
            }
        }

        public void RemoveInstructionEntry(Instruction instr)
        {
            foreach(InstructionEntry entry in instructionEntries)
            {
                if(entry.instruction == instr)
                {
                    RemoveInstructionEntry(entry);
                }
            }
        }

        public int InstructionEntryIndex(InstructionEntry entry)
        {
            return instructionEntries.IndexOf(entry);
        }

        public bool HasInstructionEntries()
        {
            return instructionEntries.Count > 0;
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
            List<InstructionEntry> sortedInstructionEntries = instructionEntries.OrderBy(o => o.instruction._ui_index).ToList();

            instructionEntryQueue.Clear();
            foreach(InstructionEntry entry in sortedInstructionEntries)
            {
                instructionEntryQueue.Enqueue(entry);
            }

            // Tell the internal receiver to observe the values it is getting
            // this will display an error for the attribute "instruction_complete" since
            // this attribute holds a string. Should be fixed in a future update

            if(ParameterControlTemplate != null)
            {
                internalReceiver.Observe = true;
                loadParameterControlTemplate(ParameterControlTemplate, stream);
            }

            internalReceiver.OnPayloadReceived((object payload, string attribute, string label) => {
                OnPayloadReceive(payload);

                if(attribute == "instruction_complete" || attribute == "instruction_done")
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
                internalReceiver.Subscribe(stream, "instruction_done", "string");
            }

            // Start instruction queue execution
            is_running = true;
            RunQueuedInstruction(stream);
        }

        public void Abort()
        {
            is_running = false;
            instructionEntryQueue.Clear();
            queue_position = 0;

            if(onQueueAbortedCallback != null)
            {
                onQueueAbortedCallback.Invoke();
            }
        }

        public void setParameterControlTemplate(ParameterControlTemplate template)
        {
            ParameterControlTemplate = template;
        }

        public void loadParameterControlTemplate(ParameterControlTemplate template, DataStream stream)
        {
            Debug.Log("Load parameter control template");
            ParameterControlTemplate = template;

            if(template.Collection != null)
            {
                internalReceiver.ObservedValues.Clear();

                // Clear subscriptions?
                internalReceiver.UnsubscribeAll(stream);

                foreach(IObservableNumericValue n in ParameterControlTemplate.Collection)
                {
                    internalReceiver.ObservedValues.Add(n);
                    internalReceiver.Subscribe(stream, n.Label, n.type.ToString());
                }

                internalReceiver.Observe = true;
            }
        }
        
        /// <summary>
        /// Run the next instruction in the queue if the queue is not empty. Also invokes the callback functions at the right time, if they are set
        /// </summary>
        /// <param name="stream">The stream the instruction should be sent to</param>
        public void RunQueuedInstruction(DataStream stream)
        {
            if (currentInstructionEntry.instruction != null)
            {
                // Complete current instruction if there is one, and if there is something to do before executing next instruction
            }

            if (instructionEntryQueue.Count > 0)
            {
                queue_position++;
                currentInstructionEntry = instructionEntryQueue.Dequeue();
                
                if(onQueueAdvanceCallback != null)
                {
                    if (currentInstructionEntry.instruction.feedbackStatus == ObservableNumericValueStatus.Unknown)
                    {
                        currentInstructionEntry.instruction.feedbackStatus = ObservableNumericValueStatus.Stable;
                    }
                    onQueueAdvanceCallback.Invoke(currentInstructionEntry);
                }

                stream.Output(currentInstructionEntry.instruction.toJObject());

            } else
            {
                queue_position = -1;
                internalReceiver.Observe = false;
                is_running = false;

                if(onQueueCompleteCallback != null)
                {
                    onQueueCompleteCallback.Invoke(currentInstructionEntry);
                }
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
            if(currentInstructionEntry.instruction != null && currentInstructionEntry.observedValueLabels != null)
            {
                if(currentInstructionEntry.observedValueLabels.Contains(val.Label))
                {
                    currentInstructionEntry.instruction.feedbackStatus = val.Status();

                    Debug.Log("===============================");
                    Debug.Log("Observed " + val.Label + " as " + val.Status());
                    Debug.Log(val.Value + "," + val.Min + "," + val.Max);
                    Debug.Log("===============================");
                }
            }
        }

        /// <summary>
        /// Register a callback to invoke when queue is advanced
        /// </summary>
        /// <param name="callback">The action to invoke</param>
        public void OnQueueAdvance(Action<InstructionEntry> callback)
        {
            onQueueAdvanceCallback = callback;
        }

        /// <summary>
        /// Register a callback to invoke when queue is complete
        /// </summary>
        /// <param name="callback">The action to invoke</param>
        public void OnQueueComplete(Action<InstructionEntry> callback)
        {
            onQueueCompleteCallback = callback;
        }

        public void OnQueueAbort(Action callback)
        {
            onQueueAbortedCallback = callback;
        }

        public void Save(TestConfiguration existing, Action<TestConfiguration> cb = null)
        {
            if(Name == String.Empty || Name == null)
            {
                // Must have a name
                return;
            }

            using (LiteDatabase db = new LiteDatabase(Program.settings.DatabasePath))
            {
                LiteCollection<TestConfiguration> collection = db.GetCollection<TestConfiguration>(Program.settings.TestConfigDatabase);

                if(existing == null)
                {
                    this.Id = null;
                    collection.Insert(this);
                } else
                {
                    this.Id = existing.Id;
                    collection.Update(this);
                }

                if(cb != null)
                {
                    cb.Invoke(this);
                }
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
