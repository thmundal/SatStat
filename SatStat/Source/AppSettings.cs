using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatStat
{
    public struct AppSettings
    {
        public string portName;
        public int baud_rate;
        public string config;

        public COMSettings comSettings;
    }

    public struct COMSettings
    {
        public int baud_rate;
        public string config;
        public string newline;
    }
}
