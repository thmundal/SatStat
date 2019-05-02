using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatStat
{
    /// <summary>
    /// A struct that contains the application settings
    /// </summary>
    public struct AppSettings
    {
        /// <summary>
        /// The name of the COM port that a COM connection should be made on
        /// </summary>
        public string portName;
        
        /// <summary>
        /// A text describing the unit connected to the selected COM port
        /// </summary>
        public string portDescription;

        /// <summary>
        /// Contains the collective COM port settings for a COM connection such as baud rate etc
        /// </summary>
        public COMSettings comSettings;

        /// <summary>
        /// Defines the path to the database file
        /// </summary>
        public string DatabasePath { get { return Directory.GetCurrentDirectory() + @"\Database.db"; } }

        /// <summary>
        /// Defines the name of the database collection to hold saved COM settings
        /// </summary>
        public string COMSettingsDB { get { return "ComSettings"; } }

        /// <summary>
        /// Defines the name of the database collection to hold saved plotted data
        /// </summary>
        public string PlotDatabase { get { return "ChartData"; } }

        /// <summary>
        /// Defines the name of the database collection to hold saved parameter control templates
        /// </summary>
        public string ParameterControlDatabase { get { return "ParamControlDB"; } }

        /// <summary>
        /// Defines the name of the database collection to hold save test configurations
        /// </summary>
        public string TestConfigDatabase { get { return "TestConfigDB"; } }
    }

    /// <summary>
    /// A struct that contains different parameters that a COM connection needs
    /// </summary>
    public struct COMSettings
    {
        /// <summary>
        /// Defines the baud rate to use on a COM connection
        /// </summary>
        public int baud_rate;

        /// <summary>
        /// Defines the config string to use on a connection towards an Arduino such as "8N1".
        /// Number of data bits can be anything from 5 through 8.
        /// Parity must be defined as either "N" (no parity), "O" (Odd parity) or "E" (Even parity).
        /// Number of stop bits can be set to either 1 or 2.
        /// </summary>
        public string config;

        /// <summary>
        /// Defines the character(s) that denote the end of the data message send over COM connection
        /// </summary>
        public string newline;

        /// <summary>
        /// A text describing the unit connected to the selected COM port
        /// </summary>
        public string portDescription;
    }
}
