using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace SatStat
{
    static class Program
    {
        public static SerialHandler serial;
        public static AppSettings settings;
        public static StreamSimulator streamSimulator;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            settings = new AppSettings {
                selectedComPort = null
            };

            serial = new SerialHandler();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            SatStatMainForm app = new SatStatMainForm();
            
            Application.Run(app);
        }
    }
}
