using Newtonsoft.Json.Linq;
using System;

namespace SatStat
{
    public class DataSubscription<T> :IDataSubscription
    {
        Type IDataSubscription.Type => typeof(T);

        public string subscriptionAttribute { get; set; }
        private DataReceiver receiver;

        public DataSubscription(DataReceiver r, string attr)
        {
            receiver = r;
            subscriptionAttribute = attr;
        }

        public DataSubscription(string attr)
        {
            subscriptionAttribute = attr;
        }

        public object receive(object data)
        {   
            try
            {
                T payload = (T)Convert.ChangeType(data, typeof(T));
                receiver.ReceivePayload(payload);
                return payload;
            } catch(InvalidCastException)
            {
                Console.WriteLine("Cannot cast received "+ data.GetType() +" to type " + typeof(T));
            } catch(InvalidOperationException)
            {
                Console.WriteLine("Error when receiving data");
            }

            return default(T);
        }

        public static IDataSubscription CreateWithType(DataReceiver receiver, string attr, string type)
        {
            switch (type)
            {
                case "float":
                    return new DataSubscription<float>(receiver, attr);
                case "double":
                    return new DataSubscription<double>(receiver, attr);
                case "int":
                    return new DataSubscription<int>(receiver, attr);
                case "long":
                    return new DataSubscription<long>(receiver, attr);
                case "string":
                    return new DataSubscription<string>(receiver, attr);
                case "JObject":
                    return new DataSubscription<JObject>(receiver, attr);
                case "JArray":
                    return new DataSubscription<JArray>(receiver, attr);
            }

            return null;
        }
    }

    public interface IDataSubscription
    {
        Type Type { get; }
        string subscriptionAttribute { get; set; }

        object receive(object data);
    }
}