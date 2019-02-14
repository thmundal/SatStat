using System;

namespace SatStat
{
    public class DataSubscription<T> :IDataSubscription
    {
        Type IDataSubscription.Type => typeof(T);

        public string subscriptionAttribute { get; set; }
        private DataReceiver<T> receiver;

        public DataSubscription(DataReceiver<T> r, string attr)
        {
            receiver = r;
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
    }

    public interface IDataSubscription
    {
        Type Type { get; }
        string subscriptionAttribute { get; set; }

        object receive(object data);
    }
}