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
            dataStream = new DataStream();
            dataStream.AddSubscriber(new DataSubscription<double> (app, "temperature"));
            SimulateDataStream();

            //serial = new SerialHandler();
            //serial.AddSubscriber(new DataSubscription<double>(app, "temperature"));


            //SerialHandler.GetPortListInformation();
            //double counter = 0;
            //serial.OnDataReceived((object data) =>
            //{
            //    string input = data.temp;
            //    input = input.Replace(Environment.NewLine, "");
            //    Console.WriteLine("temperature is " + input);

            //    double temp = 0;
            //    try
            //    {
            //        temp = double.Parse(input, System.Globalization.CultureInfo.InvariantCulture);
            //    }
            //    catch (Exception e)
            //    {
            //        Console.WriteLine(e.Message);
            //    }
            //    double[] payload = { counter++, temp };

            //    dataStream.SetPayload(payload);
            //    dataStream.DeliverPayload();
            //});

            Application.Run(app);
        }

        public static void StopReader()
        {
            if(serial != null)
            {
                serial.Stop();
            }
        }

        public static void StartReader()
        {
            if(serial != null)
            {
                serial.Connect();
            }
        }

        public static void WriteSerialData(string data)
        {
            serial.WriteData(data);
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
