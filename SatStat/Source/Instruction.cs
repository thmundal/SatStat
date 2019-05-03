using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatStat
{
    /// <summary>
    /// Representing an instruction that can be run on the Hardware Layer
    /// </summary>
    public class Instruction
    {
        /// <summary>
        /// An object containing the parameters for the instruction
        /// </summary>
        protected JObject paramtable;

        /// <summary>
        /// Refers to the index in the list of queued instructions this instruction is placed at in the test planning UI
        /// </summary>
        public int UI_Index = -1;

        public ObservableNumericValueStatus feedbackStatus = ObservableNumericValueStatus.Unknown;

        public Instruction() { }

        /// <summary>
        /// Contructor
        /// </summary>
        /// <param name="instr">Instruction as a string</param>
        /// <param name="arguments">A variable parameter list containing the parameters for the instruction</param>
        public Instruction(string instr, params object[] arguments)
        {
            paramtable = new JObject();
            paramtable["instruction"] = instr;

            string param = "";

            for(int i=0; i<arguments.Length; i++)
            {
                if(i % 2 == 0)
                {
                    param = (string) arguments[i];

                } else
                {
                    paramtable[param] = new JValue(arguments[i]);
                }
            }

            Debug.Log(JSON.serialize(paramtable));
        }

        public Instruction(string instr)
        {
            paramtable = new JObject();
            paramtable["instruction"] = instr;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="instr">Instruction as a string</param>
        /// <param name="paramTable">A JSON Object containing the parameters for the instruction as key/value pairs</param>
        public Instruction(string instr, JObject paramTable)
        {
            paramtable = paramTable;
            paramtable["instruction"] = instr;
        }

        /// <summary>
        /// Return the parameter table
        /// </summary>
        /// <returns>The parameter table</returns>
        public JObject toJObject()
        {
            return paramtable;
        }

        /// <summary>
        /// A static function for creating and returning a new instance of an instruction. Can be used directly with SerialHandler.Output()
        /// </summary>
        /// <param name="instr">Instruction as string</param>
        /// <param name="arguments">Variable parameter list containing the parameters for the instruction</param>
        /// <returns>A JSON object that can be sent on a datastream</returns>
        public static JObject Create(string instr, params object[] arguments)
        {
            Instruction i = new Instruction(instr, arguments);
            return i.toJObject();
        }

        /// <summary>
        /// Static function for creating and returning JSON compatible instruction data for sending trough DataStream
        /// </summary>
        /// <param name="instr">Name of the instruction</param>
        /// <param name="paramTable">JSON Object containing the parameter names and values</param>
        /// <returns>A JSON object that can be sent on a datastream</returns>
        public static JObject Create(string instr, JObject paramTable)
        {
            Instruction i = new Instruction(instr, paramTable);
            return i.toJObject();
        }

        public static JObject Create(string instr)
        {
            Instruction i = new Instruction(instr);
            return i.toJObject();
        }
    }

    public struct InstructionUIEntry
    {
        public string label;
        public string attribute;
        public override string ToString()
        {
            return label;
        }
    }
}
