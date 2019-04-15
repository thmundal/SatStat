using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace SatStat
{
    /// <summary>
    /// A class to display Debug messages if we run Visual Studio in debug mode.
    /// </summary>
    class Debug
    {
        /// <summary>
        /// Log a message and display the filename and linenumber where this log was generated. This helps find the message and makes it easier to locate code that behaves bad. Essential for multi-threaded applications
        /// </summary>
        /// <param name="m">The object to be logged</param>
        /// <param name="ln">Line number where this Log method was invoked</param>
        /// <param name="file">File name where this Log method was invoked</param>
        public static void Log(object m, [CallerLineNumber] int ln = 0, [CallerFilePath] string file = "")
        {
#if DEBUG
            Console.WriteLine("\n{0}: {1}\n{2}", file, ln, m);
#endif
        }
    }
}
