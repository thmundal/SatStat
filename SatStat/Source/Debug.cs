using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatStat
{
    class Debug
    {
        public static void Log(object m)
        {
#if DEBUG
            Console.WriteLine(m);
#endif
        }
    }
}
