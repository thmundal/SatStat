using System;

namespace SatStat
{
    class DataSubscription<T> :IDataSubscription
    {
        Type IDataSubscription.Type => typeof(T);

        public string subscriptionAttribute { get; set; }
        private DataReceiver<T> receiver;

        public DataSubscription(DataReceiver<T> r, string attr)
        {
            receiver = r;
            subscriptionAttribute = attr;
        }

        public void receive(object data)
        {
            T payload = (T)Convert.ChangeType(data, typeof(T));
            receiver.ReceivePayload(payload);
        }
    }

    interface IDataSubscription
    {
        Type Type { get; }
        string subscriptionAttribute { get; set; }

        void receive(object data);
    }
}