using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatStat
{
    public struct AppSettings
    {
        public string portName;
        
        public string portDescription;
        public COMSettings comSettings;

        // DB Settings
        public string DatabasePath { get { return Directory.GetCurrentDirectory() + @"\Database.db"; } }
        public string COMSettingsDB { get { return "ComSettings"; } }
        public string PlotDatabase { get { return "ChartData"; } }
    }

    public struct COMSettings
    {
        public int baud_rate;
        public string config;
        public string newline;
    }
}
