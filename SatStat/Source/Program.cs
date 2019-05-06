using System;
using System.Windows.Forms;

namespace SatStat
{
    static class Program
    {
        public static SerialHandler serial;
        public static AppSettings settings;
        public static StreamSimulator streamSimulator;
        public static SocketHandler socketHandler;
        public static SatStatMainForm app;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            settings = new AppSettings {
                portName = null,
                comSettings =
                {
                    baud_rate = 9600,
                    config = "8N1"
                }
            };
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

#if !DEBUG
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += new UnhandledExceptionEventHandler(ExceptionHandler);
#endif
            app = new SatStatMainForm();
            
            Application.Run(app);

        }

        private static void ExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = (Exception)e.ExceptionObject;
            Debug.Log("An unhandled exception was thrown");
            Debug.Log(ex);
        }
    }
}
