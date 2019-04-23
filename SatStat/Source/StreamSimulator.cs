using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SatStat.Utils;

namespace SatStat
{
    public class StreamSimulator : DataStream
    {
        private bool connected = false;
        private bool running = true;

        private Queue<JObject> output_buffer;
        private Queue<string> input_buffer;

        private double position = 0;
        private object _position_lock = new object();

        private int taskdelay = 100;

        [STAThread]
        public async void Connect()
        {
            Debug.Log("Starting datastream");

            output_buffer = new Queue<JObject>();
            input_buffer = new Queue<string>();

            OnOutputReceived((data) =>
            {
                JObject json_data = JSON.parse<JObject>(data);
                output_buffer.Enqueue(json_data);
                Debug.Log("Sending data to output endpoint " + data);
            });

            await Run();
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
                    if (output != null)
                    {
                        if (output.ContainsKey("request"))
                        {
                            string instruction_name = output["request"].Value<string>();
                            double deg_val = 0;
                            double steps_val = 0;

                            try
                            {
                                deg_val = output["degrees"].Value<double>();
                            }
                            catch (Exception) { }

                            try
                            {
                                steps_val = output["steps"].Value<double>();
                            }
                            catch (Exception) { }

                            if(instruction_name == "rotate")
                            {
                                RotateInstruction(deg_val, steps_val);
                            }
                        }
                    }

                    double temp = (double)(rand.Next(0, 10));
                    int humidity = rand.Next(0, 10);

                    JObject data = new JObject()
                    {
                        { "temperature", temp },
                        { "humidity", humidity },
                        { "sine", Math.Sin(count) },
                        { "position", position }
                    };

                    input_buffer.Enqueue(JSON.serialize(data));

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


                await Task.Delay(taskdelay);
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

            JObject available_data = new JObject()
            {
                {"available_data", new JObject()
                    {
                        { "temperature", "double" },
                        { "humidity", "int" },
                        { "sine", "double" },
                        { "position", "double" }
                    }
                }
            };

            JObject available_instructions = new JObject()
            {
                { "available_instructions", new JObject()
                    {
                        { "rotate", new JObject() { { "degrees", "int" }, { "steps", "int" } } }
                    }
                }
            };
            input_buffer.Enqueue(JSON.serialize(available_data));
            input_buffer.Enqueue(JSON.serialize(available_instructions));
        }
        
        public new void Output(object data)
        {
            output_buffer.Enqueue((JObject) data);
            Debug.Log("Sending data to output endpoint " + data);
        }

        public void RotateInstruction(double degrees = 0, double steps = 0)
        {
            Debug.Log("Start rotating");
            Task.Run(async () =>
            {
                for(int i=0; i< Math.Abs(degrees) + Math.Abs(steps); i++)
                {
                    lock(_position_lock)
                    {
                        if(degrees != 0)
                        {
                            position += 1 * Math.Sign(degrees);
                        }

                        if(steps != 0)
                        {
                            position += 1 * Math.Sign(steps);
                        }
                    }
                    Console.WriteLine(position);
                    await Task.Delay(taskdelay);
                }

                input_buffer.Enqueue(JSON.serialize(new JObject() { { "instruction_complete", "rotate" } }));
            });
        }
    }
}
