using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatStat
{
    class Instruction
    {
        private JObject paramtable;

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

        public JObject toJObject()
        {
            return paramtable;
        }

        public static JObject Create(string instr, params object[] arguments)
        {
            Instruction i = new Instruction(instr, arguments);
            return i.toJObject();
        }
    }
}
