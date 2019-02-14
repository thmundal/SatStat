using System.Collections.Generic;
using Newtonsoft.Json;
using System;
using Newtonsoft.Json.Serialization;

namespace SatStat
{
    /// <summary>
    /// A datastream that can be subscibed to trough a DataSubscription
    /// </summary>
    public class DataStream
    {
        private List<Dictionary<string, object>> inputBuffer;
        private DataProvider<int> provider;
        private List<IDataSubscription> subscriptions;

        private JsonSerializerSettings json_settings;
        private List<string> json_errors = new List<string>();
        private bool json_error = false;

        public DataStream()
        {
            subscriptions = new List<IDataSubscription>();
            inputBuffer = new List<Dictionary<string, object>>();

            json_settings = new JsonSerializerSettings
            {
                Error = (object sender, ErrorEventArgs args) =>
                {
                    json_errors.Add(args.ErrorContext.Error.Message);
                    args.ErrorContext.Handled = true;
                }
            };
        }

        /// <summary>
        /// Deliver data requested to all subscribers
        /// </summary>
        public void DeliverSubscriptions()
        {
            foreach(IDataSubscription subscriber in subscriptions)
            {
                string attribute = subscriber.subscriptionAttribute;

                Type type = subscriber.Type;
                
                //foreach(var input in inputBuffer)
                for(int i=0; i<inputBuffer.Count; i++)
                {
                    var input = inputBuffer[i];
                    if(input.TryGetValue(attribute, out var value))
                    {
                        // Convert data type to subscriber.subscriptionDataType ?????
                        Console.Write("Delivering to subscribers: ");
                        Console.WriteLine(value);
                        subscriber.receive(value);
                        inputBuffer.RemoveAt(i);
                    }
                }
            }
        }

        /// <summary>
        /// Return the latest error that has happened in relation to JSON parsing or serialization
        /// </summary>
        /// <returns></returns>
        private string GetLastJsonError()
        {
            return json_errors[json_errors.Count - 1];
        }

        /// <summary>
        /// Parse input string as json key/value pairs and put the parsed data in the input buffer
        /// </summary>
        /// <param name="input"></param>
        public void Parse(string input)
        {   
            if(input.Length > 0)
            {
                Console.WriteLine("Parsing " + input);
                Dictionary<string, object> inputParsed = JsonConvert.DeserializeObject<Dictionary<string, object>>(input);
            
                if (json_error)
                {
                    Console.WriteLine(GetLastJsonError());
                }
                inputBuffer.Add(inputParsed);
            } else
            {
                Console.WriteLine("Input is empty, aborting parse");
            }
        }

        /// <summary>
        /// Add a subscriber to this data stream
        /// </summary>
        /// <param name="subscription"></param>
        public void AddSubscriber(IDataSubscription subscription)
        {
            subscriptions.Add(subscription);
        }
    }
}