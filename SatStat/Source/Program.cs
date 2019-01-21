using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SatStat
{
    static class Program
    {
        private static DataProvider<double[]> dataStream;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            SatStatMainForm app = new SatStatMainForm();
            dataStream = new DataProvider<double[]>(app);
            SimulateDataStream();

            Application.Run(app);
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
