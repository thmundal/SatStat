using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SatStat
{
    static class Program
    {
        private static SerialReader sr;
        private static DataProvider<double[]> dataStream;
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
            dataStream = new DataProvider<double[]>(app);
            //SimulateDataStream();

            sr = new SerialReader();
            double counter = 0;
            sr.OnDataReceived((string input) =>
            {
                input = input.Replace(Environment.NewLine, "");
                Console.WriteLine("temperature is " + input);

                double temp = 0;
                try
                {
                    temp = double.Parse(input, System.Globalization.CultureInfo.InvariantCulture);
                } catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                double[] payload = { counter++, temp};
                
                dataStream.SetPayload(payload);
                dataStream.DeliverPayload();
            });

            Application.Run(app);
        }

        public static void StopReader()
        {
            sr.Stop();
        }

        public static void StartReader()
        {
            sr.Run();
        }

        [STAThread]
        static void SimulateDataStream()
        {
            Console.WriteLine("Starting datastream");
            double[] payload = { 0, 0 };

            Random rand = new Random();
            Task streamTask = Task.Run(async () =>
            {
                int i = 0;
                while (i < 100)
                {
                    payload[0] = i;
                    payload[1] = (rand.NextDouble() * 100);
                    
                    dataStream.SetPayload(payload);
                    dataStream.DeliverPayload();

                    await Task.Delay(500);
                    i++;
                }
            });
        }
    }
}
