using Microsoft.VisualStudio.TestTools.UnitTesting;
using SatStat;
using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace SatStatTests
{
    [TestClass]
    public class InputDataParsing
    {
        string JSON = "{\"double\":2.0, \"string\":\"this is a string\", \"float\":3.2, \"int\":\"5\", \"long\":\"5\", \"genericlist\":[\"one\",\"two\",\"three\"], \"hashtable\":{\"int\":2,\"string\":\"value\"}}";
        
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
        public void ReceiveJArray()
        {
            DataStream stream = new DataStream();
            Receiver<JArray> receiver = new Receiver<JArray>();

            DataSubscription<JArray> sub = new DataSubscription<JArray>(receiver, "genericlist");

            stream.AddSubscriber(sub);
            stream.Parse(JSON);
            stream.DeliverSubscriptions();

            JArray expected = new JArray { "one", "two", "three" };
            JArray actual = receiver.receivedPayload;

            for(int i=0; i<expected.Count; i++)
            {
                Assert.AreEqual(expected[i], actual[i]);
            }
        }

        
        public void ReceiveHashtable()
        {
            DataStream stream = new DataStream();
            Receiver<Hashtable> receiver = new Receiver<Hashtable>();

            DataSubscription<Hashtable> sub = new DataSubscription<Hashtable>(receiver, "hashtable");

            stream.AddSubscriber(sub);
            stream.Parse(JSON);
            stream.DeliverSubscriptions();
            

            Hashtable actual = new Hashtable();
            actual.Add("int", 2);
            actual.Add("string", "value");

            Assert.AreEqual(actual, receiver.receivedPayload);
        }

        [TestMethod]
        public void ReceiveJObject()
        {
            DataStream stream = new DataStream();
            Receiver<JObject> receiver = new Receiver<JObject>();

            DataSubscription<JObject> sub = new DataSubscription<JObject>(receiver, "hashtable");

            stream.AddSubscriber(sub);
            stream.Parse(JSON);
            stream.DeliverSubscriptions();

            JObject actual = new JObject();
            actual.Add("int", 2);
            actual.Add("string", "value");

            foreach (var item in actual)
            {
                Assert.IsTrue(receiver.receivedPayload.ContainsKey(item.Key));
                Assert.AreEqual(receiver.receivedPayload.GetValue(item.Key), item.Value);
            }
        }

        [TestMethod]
        public void MultipleSubscriptionOnDifferentKeys()
        {
            DataStream stream = new DataStream();
            Receiver<JObject> receiver = new Receiver<JObject>();
            Receiver<int> receiver2 = new Receiver<int>();

            DataSubscription<JObject> sub = new DataSubscription<JObject>(receiver, "hashtable");
            DataSubscription<int> sub2 = new DataSubscription<int>(receiver2, "int");

            stream.AddSubscriber(sub);
            stream.AddSubscriber(sub2);
            stream.Parse(JSON);
            stream.DeliverSubscriptions();

            JObject actual = new JObject();
            actual.Add("int", 2);
            actual.Add("string", "value");

            foreach (var item in actual)
            {
                Assert.IsTrue(receiver.receivedPayload.ContainsKey(item.Key));
                Assert.AreEqual(receiver.receivedPayload.GetValue(item.Key), item.Value);
            }

            int actual2 = 5;
            Assert.AreEqual(actual2, receiver2.receivedPayload);
        }

        [TestMethod]
        public void MultipleSubscribersOnSameKey()
        {
            DataStream stream = new DataStream();
            Receiver<int> receiver = new Receiver<int>();
            Receiver<int> receiver2 = new Receiver<int>();

            DataSubscription<int> sub = new DataSubscription<int>(receiver, "int");
            DataSubscription<int> sub2 = new DataSubscription<int>(receiver2, "int");

            stream.AddSubscriber(sub);
            stream.AddSubscriber(sub2);
            stream.Parse(JSON);
            stream.DeliverSubscriptions();
            
            Assert.AreEqual(5, receiver.receivedPayload);
            Assert.AreEqual(5, receiver2.receivedPayload);
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
