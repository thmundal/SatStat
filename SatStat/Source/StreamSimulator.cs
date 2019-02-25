﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatStat
{
    public class StreamSimulator : DataStream
    {
        [STAThread]
        public void Connect()
        {
            Console.WriteLine("Starting datastream");
            Run();
        }

        private async Task Run()
        {
            Random rand = new Random();
            string payload = "{\"available_sensors\":[{\"temperature\":\"double\"}, {\"humidity\":\"int\"}]}";

            Parse(payload);
            DeliverSubscriptions();

            int i = 0;
            while (i < 100)
            {
                double temp = (rand.NextDouble() * 100);
                double humidity = rand.Next(0, 100);

                payload = "{\"temperature\": \"" + temp + "\", \"humidity\": " + humidity + "}";

                Parse(payload);
                DeliverSubscriptions();

                await Task.Delay(500);
                i++;
            }
        }
    }
}
