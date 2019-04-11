﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatStat
{
    public class StreamSimulator : DataStream
    {
        private bool connected = false;
        private bool running = true;

        private Queue<JObject> output_buffer;
        private Queue<string> input_buffer;

        [STAThread]
        public void Connect()
        {
            Debug.Log("Starting datastream");

            output_buffer = new Queue<JObject>();
            input_buffer = new Queue<string>();

            Run();
        }

        private async Task Run()
        {
            Random rand = new Random();

            ClientInitializeHandshake();
            double count = 0;

            while (running)
            {
                JObject output = null;
                if(output_buffer.Count > 0)
                {
                    output = output_buffer.Dequeue();
                }

                string input = null;

                if(!connected)
                {
                    if (input_buffer.Count > 0)
                    {
                        input = input_buffer.Dequeue();
                    }

                    if (output != null && output.ContainsKey("serial_handshake"))
                    {
                        StartHandshake();
                    }
                    if(input != null)
                    {
                        JObject inputParsed = Parse(input);

                        if(inputParsed.ContainsKey("serial_handshake"))
                        {
                            // Set up "serial connection"
                            EndHandshake();
                            connected = true;
                        }
                    }
                } else
                {
                    double temp = (double) (rand.Next(0, 10));
                    int humidity = rand.Next(0, 10);

                    input_buffer.Enqueue("{\"temperature\": \"" + temp + "\", \"humidity\": " + humidity + ", \"sine\":"+ Math.Sin(count).ToString().Replace(",",".") +"}");

                    while(input_buffer.Count > 0)
                    {
                        input = input_buffer.Dequeue();

                        if (input != null)
                        {
                            Parse(input);
                            DeliverSubscriptions();
                        }
                    }
                    count += 0.1;
                }


                await Task.Delay(100);
            }
        }

        private void ClientInitializeHandshake()
        {
            Debug.Log("Initializing handshake from client");
            JObject o = new JObject() { { "serial_handshake", "init" } };
            output_buffer.Enqueue(o);
        }

        private void StartHandshake()
        {
            Debug.Log("Starting serial handshake");
            input_buffer.Enqueue("{\"serial_handshake\":{\"baud_rates\":[9600,4800,19200,115200], \"configs\":[\"8N1\", \"8Y1\"], \"newlines\":[\"\r\n\", \"\r\", \"\n\"]}}");
        }

        private void EndHandshake()
        {
            Debug.Log("Ending serial handshake");
            output_buffer.Enqueue(new JObject()
            {
                {"connection_request",
                    new JObject() {
                        { "baud_rate", 9600 }, {"config", "8N1" }, {"newline", "\r\n" }
                    }
                }
            });
            input_buffer.Enqueue("{\"available_data\":{\"temperature\":\"double\", \"humidity\":\"int\", \"sine\":\"double\"}}");
        }

        //private void OnDataReceived(string input)
        //{
        //    Dictionary<string, object> inputParsed = Parse(input);

        //    if (inputParsed.ContainsKey("serial_handshake"))
        //    {
        //        Debug.Log(inputParsed);
        //    }
        //    else
        //    {
        //        DeliverSubscriptions();
        //    }
        //}

        public void Output(JObject data)
        {
            output_buffer.Enqueue(data);
            Debug.Log("Sending data to output endpoint " + data);
        }
    }
}
