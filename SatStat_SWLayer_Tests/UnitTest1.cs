using Microsoft.VisualStudio.TestTools.UnitTesting;
using SatStat;
using System;
using System.Collections.Generic;

namespace SatStatTests
{
    [TestClass]
    public class UnitTest1
    {
        string JSON = "{\"double\":\"2,0\", \"string\":\"this is a string\", \"float\":\"3,2\", \"int\":\"5\", \"long\":\"5\", \"genericlist\":[\"one\",\"two\",\"three\"]}";
        
        [TestMethod]
        public void ReceiveDouble()
        {
            DataStream stream = new DataStream();
            Receiver<double> receiver = new Receiver<double>();

            DataSubscription<double> sub = new DataSubscription<double>(receiver, "double");

            stream.AddSubscriber(sub);
            stream.Parse(JSON);
            stream.DeliverSubscriptions();

            Assert.AreEqual(2.0, receiver.receivedPayload);
        }

        [TestMethod]
        public void ReceiveString()
        {
            DataStream stream = new DataStream();
            Receiver<string> receiver = new Receiver<string>();

            DataSubscription<string> sub = new DataSubscription<string>(receiver, "string");

            stream.AddSubscriber(sub);
            stream.Parse(JSON);
            stream.DeliverSubscriptions();

            Assert.AreEqual("this is a string", receiver.receivedPayload);
        }

        [TestMethod]
        public void ReceiveFloat()
        {
            DataStream stream = new DataStream();
            Receiver<float> receiver = new Receiver<float>();

            DataSubscription<float> sub = new DataSubscription<float>(receiver, "float");

            stream.AddSubscriber(sub);
            stream.Parse(JSON);
            stream.DeliverSubscriptions();

            Assert.AreEqual(3.2f, receiver.receivedPayload);
        }

        [TestMethod]
        public void ReceiveInt()
        {
            DataStream stream = new DataStream();
            Receiver<int> receiver = new Receiver<int>();

            DataSubscription<int> sub = new DataSubscription<int>(receiver, "int");

            stream.AddSubscriber(sub);
            stream.Parse(JSON);
            stream.DeliverSubscriptions();

            Assert.AreEqual(5, receiver.receivedPayload);
        }

        [TestMethod]
        public void ReceiveLong()
        {
            DataStream stream = new DataStream();
            Receiver<long> receiver = new Receiver<long>();

            DataSubscription<long> sub = new DataSubscription<long>(receiver, "long");

            stream.AddSubscriber(sub);
            stream.Parse(JSON);
            stream.DeliverSubscriptions();

            Assert.AreEqual(5L, receiver.receivedPayload);
        }

        [TestMethod]
        public void ReceiveGenericList()
        {
            DataStream stream = new DataStream();
            Receiver<Newtonsoft.Json.Linq.JArray> receiver = new Receiver<Newtonsoft.Json.Linq.JArray>();

            DataSubscription<Newtonsoft.Json.Linq.JArray> sub = new DataSubscription<Newtonsoft.Json.Linq.JArray>(receiver, "genericlist");

            stream.AddSubscriber(sub);
            stream.Parse(JSON);
            stream.DeliverSubscriptions();

            Newtonsoft.Json.Linq.JArray actual = new Newtonsoft.Json.Linq.JArray { "one", "two", "three" };

            Assert.AreEqual(actual, receiver.receivedPayload);
        }

    }

    public class Receiver<T> : DataReceiver<T>
    {
        public T receivedPayload;
        
        public void ReceivePayload(T payload)
        {
            receivedPayload = payload;
            Console.WriteLine(payload);
        }
    }
}
