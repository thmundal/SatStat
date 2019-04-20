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

        public TestConfiguration()
        {
            internalReceiver = new DataReceiver();
            instructionQueue = new Queue<Instruction>();
        }

        public void AddInstruction(Instruction instr)
        {
            Instructions.Add(instr);
        }

        public void Run(DataStream stream)
        {
            instructionQueue.Clear();

            foreach(Instruction instr in Instructions)
            {
                instructionQueue.Enqueue(instr);
            }

            foreach(IObservableNumericValue value in ParameterControlTemplate.Collection)
            {
                value.type.ToString();
                internalReceiver.Subscribe(stream, value.Label, value.type.ToString());
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
                currentInstruction = instructionQueue.Dequeue();

                stream.Output(currentInstruction.toJObject());

            } else
            {
                // Queue is empty
            }
        }

        public void OnPayloadReceive(object payload)
        {

        }

        public void OnObservedValueChange(IObservableNumericValue val)
        {

        }
    }
}
