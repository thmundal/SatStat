using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace SatStat
{
    static class Program
    {
        public static SerialHandler serial;
        private static DataStream dataStream;
        public static AppSettings settings;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            settings = new AppSettings {
                selectedComPort = null
            };

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            SatStatMainForm app = new SatStatMainForm();
            
            serial = new SerialHandler();
            serial.AddSubscriber(new DataSubscription<double>(app, "temperature"));
            
            Application.Run(app);
        }

        [STAThread]
        static void SimulateDataStream()
        {
            Console.WriteLine("Starting datastream");
            string payload = "";

            Random rand = new Random();
            Task streamTask = Task.Run(async () =>
            {
                int i = 0;
                while (i < 100)
                {
                    double temp = (rand.NextDouble() * 100);

                    payload = "{\"temperature\": \"" + temp + "\"}";

                    dataStream.Parse(payload);
                    dataStream.DeliverSubscriptions();
                    //dataStream.SetPayload(payload);
                    //dataStream.DeliverPayload();

                    await Task.Delay(500);
                    i++;
                }
            });
        }
    }
}
