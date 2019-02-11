using System.Collections.Generic;
using Newtonsoft.Json;
using System.Reflection;
using System;
using Newtonsoft.Json.Serialization;

namespace SatStat
{
    class DataStream
    {
        private DataProvider<int> provider;
        private List<IDataSubscription> subscriptions;
        private List<Dictionary<string, object>> inputBuffer;

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

        private string GetLastJsonError()
        {
            return json_errors[json_errors.Count - 1];
        }

        public virtual void Parse(string input)
        {   
            Console.WriteLine(input);
            Dictionary<string, object> inputParsed = JsonConvert.DeserializeObject<Dictionary<string, object>>(input);
            
            if (json_error)
            {
                Console.WriteLine(GetLastJsonError());
            }
            inputBuffer.Add(inputParsed);

        }

        public void AddSubscriber(IDataSubscription subscription)
        {
            subscriptions.Add(subscription);
        }
    }
}