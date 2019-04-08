using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace SatStat
{
    class Debug
    {
        public static void Log(object m, [CallerLineNumber] int ln = 0, [CallerFilePath] string file = "")
        {
#if DEBUG
            Console.WriteLine("\n{0}: {1}\n{2}", file, ln, m);
#endif
        }
    }
}
