using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SatStat
{
    class JSON
    {
        public static object parse(string input)
        {
            return JsonConvert.DeserializeObject(input);
        }

        public static string serialize(object input)
        {
            return JsonConvert.SerializeObject(input);
        }
    }
}
