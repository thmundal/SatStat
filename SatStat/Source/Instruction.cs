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
        private JObject paramtable;

        /// <summary>
        /// Contructor
        /// </summary>
        /// <param name="instr">Instruction as a string</param>
        /// <param name="arguments">A variable parameter list containing the parameters for the instruction</param>
        public Instruction(string instr, params object[] arguments)
        {
            paramtable = new JObject();
            paramtable["request"] = instr;

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
        /// <returns></returns>
        public static JObject Create(string instr, params object[] arguments)
        {
            Instruction i = new Instruction(instr, arguments);
            return i.toJObject();
        }

        public static JObject Subscription(string type, params object[] subscriptions)
        {
            JArray list = new JArray();

            foreach(object n in subscriptions)
            {
                list.Add(n);
            }

            JObject instruction = new JObject();
            instruction["request"] = type;
            instruction["parameters"] = list;

            return instruction;
        }
    }
}
