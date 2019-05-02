using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace SatStat
{
    class Request
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

        public Request() { }

        /// <summary>
        /// Contructor
        /// </summary>
        /// <param name="instr">Instruction as a string</param>
        /// <param name="arguments">A variable parameter list containing the parameters for the instruction</param>
        public Request(string instr, params object[] arguments)
        {
            paramtable = new JObject();
            paramtable["request"] = instr;

            string param = "";

            for (int i = 0; i < arguments.Length; i++)
            {
                if (i % 2 == 0)
                {
                    param = (string)arguments[i];

                }
                else
                {
                    paramtable[param] = new JValue(arguments[i]);
                }
            }

            Debug.Log(JSON.serialize(paramtable));
        }

        public Request(string instr)
        {
            paramtable = new JObject();
            paramtable["request"] = instr;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="instr">Instruction as a string</param>
        /// <param name="paramTable">A JSON Object containing the parameters for the instruction as key/value pairs</param>
        public Request(string instr, JObject paramTable)
        {
            paramtable = paramTable;
            paramtable["request"] = instr;
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
            Request i = new Request(instr, arguments);
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
            Request i = new Request(instr, paramTable);
            return i.toJObject();
        }

        public static JObject Create(string instr)
        {
            Request i = new Request(instr);
            return i.toJObject();
        }

        /// <summary>
        /// Create a special instruction that is compatible with subscribe and unsubscribe functionality on the hardware layer
        /// </summary>
        /// <param name="type">Type of subscription, can be "subscribe" or "unsubscribe"</param>
        /// <param name="subscriptions">A list of attributes to subscribe to</param>
        /// <returns>A JSON object that can be sent on a datastream</returns>
        public static JObject Subscription(string type, params object[] subscriptions)
        {
            JArray list = new JArray();

            foreach (object n in subscriptions)
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
