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
            DataReceiver receiver = new DataReceiver("double");

            IDataSubscription sub = DataSubscription<object>.CreateWithType(receiver, "double", "double");

            receiver.OnPayloadReceived((object payload) =>
            {
                Assert.AreEqual(2.0, payload);
                Assert.AreEqual(typeof(double), payload.GetType());
                Console.WriteLine(payload.GetType());
            });

            stream.AddSubscriber(sub);
            stream.Parse(JSON);
            stream.DeliverSubscriptions();

        }

        [TestMethod]
        public void ReceiveString()
        {
            DataStream stream = new DataStream();
            DataReceiver receiver = new DataReceiver("string");

            IDataSubscription sub = DataSubscription<object>.CreateWithType(receiver, "string", "string");

            receiver.OnPayloadReceived((object payload) =>
            {
                Assert.AreEqual("this is a string", payload);
                Assert.AreEqual(typeof(string), payload.GetType());
                Console.WriteLine(payload.GetType());
            });

            stream.AddSubscriber(sub);
            stream.Parse(JSON);
            stream.DeliverSubscriptions();

        }

        [TestMethod]
        public void ReceiveFloat()
        {
            DataStream stream = new DataStream();
            DataReceiver receiver = new DataReceiver("float");

            IDataSubscription sub = DataSubscription<object>.CreateWithType(receiver, "float", "float");

            receiver.OnPayloadReceived((object payload) =>
            {
                Assert.AreEqual(3.2f, payload);
                Assert.AreEqual(typeof(float), payload.GetType());
                Console.WriteLine(payload.GetType());
            });

            stream.AddSubscriber(sub);
            stream.Parse(JSON);
            stream.DeliverSubscriptions();

        }

        [TestMethod]
        public void ReceiveInt()
        {
            DataStream stream = new DataStream();
            DataReceiver receiver = new DataReceiver("int");

            IDataSubscription sub = DataSubscription<object>.CreateWithType(receiver, "int", "int");

            receiver.OnPayloadReceived((object payload) =>
            {
                Assert.AreEqual(5, payload);
                Assert.AreEqual(typeof(int), payload.GetType());
                Console.WriteLine(payload.GetType());
            });

            stream.AddSubscriber(sub);
            stream.Parse(JSON);
            stream.DeliverSubscriptions();

        }

        [TestMethod]
        public void ReceiveLong()
        {
            DataStream stream = new DataStream();
            DataReceiver receiver = new DataReceiver("long");

            IDataSubscription sub = DataSubscription<object>.CreateWithType(receiver, "long", "long");

            receiver.OnPayloadReceived((object payload) =>
            {
                Assert.AreEqual(5L, payload);
                Assert.AreEqual(typeof(long), payload.GetType());
                Console.WriteLine(payload.GetType());
            });

            stream.AddSubscriber(sub);
            stream.Parse(JSON);
            stream.DeliverSubscriptions();

        }

        [TestMethod]
        public void ReceiveJArray()
        {
            DataStream stream = new DataStream();
            DataReceiver receiver = new DataReceiver("JArray");

            IDataSubscription sub = DataSubscription<object>.CreateWithType(receiver, "genericlist", "JArray");

            receiver.OnPayloadReceived((object payload) => {

                JArray expected = new JArray { "one", "two", "three" };
                JArray actual = (JArray) payload;

                for (int i = 0; i < expected.Count; i++)
                {
                    Assert.AreEqual(expected[i], actual[i]);
                }
                Console.Write("Datatype: ");
                Console.WriteLine(payload.GetType());
            });

            stream.AddSubscriber(sub);
            stream.Parse(JSON);
            stream.DeliverSubscriptions();
        }

        [TestMethod]
        public void ReceiveJObject()
        {
            DataStream stream = new DataStream();
            DataReceiver receiver = new DataReceiver("JObject");

            IDataSubscription sub = DataSubscription<object>.CreateWithType(receiver, "hashtable", "JObject");

            receiver.OnPayloadReceived((object p) =>
            {
                JObject payload = (JObject)p;
                JObject actual = new JObject
                {
                    { "int", 2 },
                    { "string", "value" }
                };

                foreach (var item in actual)
                {
                    Assert.IsTrue(payload.ContainsKey(item.Key));
                    Assert.AreEqual(payload.GetValue(item.Key), item.Value);
                }
                Console.WriteLine(payload.GetType());
            });

            stream.AddSubscriber(sub);
            stream.Parse(JSON);
            stream.DeliverSubscriptions();
        }

        [TestMethod]
        public void MultipleSubscriptionOnDifferentKeys()
        {
            DataStream stream = new DataStream();
            DataReceiver receiver = new DataReceiver("JObject");
            DataReceiver receiver2 = new DataReceiver("int");
            
            IDataSubscription sub = DataSubscription<object>.CreateWithType(receiver, "hashtable", "JObject");
            IDataSubscription sub2 = DataSubscription<object>.CreateWithType(receiver2, "int", "int");

            receiver.OnPayloadReceived((object p) =>
            {
                JObject payload = (JObject)p;
                JObject actual = new JObject();
                actual.Add("int", 2);
                actual.Add("string", "value");

                foreach (var item in actual)
                {
                    Assert.IsTrue(payload.ContainsKey(item.Key));
                    Assert.AreEqual(payload.GetValue(item.Key), item.Value);
                }
                Console.WriteLine(payload.GetType());
            });

            receiver2.OnPayloadReceived((object payload) =>
            {
                Assert.AreEqual(5, payload);
                Assert.AreEqual(typeof(int), payload.GetType());
                Console.WriteLine(payload.GetType());
            });

            stream.AddSubscriber(sub);
            stream.AddSubscriber(sub2);
            stream.Parse(JSON);
            stream.DeliverSubscriptions();
        }

        [TestMethod]
        public void MultipleSubscribersOnSameKey()
        {
            DataStream stream = new DataStream();
            DataReceiver receiver = new DataReceiver("int");
            DataReceiver receiver2 = new DataReceiver("int");
            
            IDataSubscription sub = DataSubscription<object>.CreateWithType(receiver, "int", "int");
            IDataSubscription sub2 = DataSubscription<object>.CreateWithType(receiver2, "int", "int");

            receiver.OnPayloadReceived((object payload) =>
            {
                Assert.AreEqual(5, payload);
                Assert.AreEqual(typeof(int), payload.GetType());
                Console.WriteLine(payload.GetType());
            });

            receiver2.OnPayloadReceived((object payload) =>
            {
                Assert.AreEqual(5, payload);
                Assert.AreEqual(typeof(int), payload.GetType());
                Console.WriteLine(payload.GetType());
            });

            stream.AddSubscriber(sub);
            stream.AddSubscriber(sub2);
            stream.Parse(JSON);
            stream.DeliverSubscriptions();
        }

        [TestMethod]
        public void ReceiveSensorList()
        {
            string payload = "{\"available_sensors\":[{\"temperature\":\"double\"}, {\"humidity\":\"int\"}]}";
            DataStream stream = new DataStream();
            DataReceiver receiver = new DataReceiver("JArray");

            IDataSubscription sub = DataSubscription<object>.CreateWithType(receiver, "available_sensors", "JArray");
            
            receiver.OnPayloadReceived((object received_payload) =>
            {
                JArray received = (JArray)received_payload;
                foreach(JObject obj in received)
                {
                    Console.WriteLine(obj);
                    Console.WriteLine(obj.GetType());

                    //Assert.IsTrue(obj.ContainsKey("temperature"));
                    //Assert.IsTrue(obj.ContainsKey("humidity"));

                    //Assert.AreEqual(obj["temperature"], "double");
                    //Assert.AreEqual(obj["humidity"], "int");

                    foreach (var elem in obj)
                    {
                        Console.WriteLine(elem.Key);
                        Console.WriteLine(elem.Value);
                    }
                }
            });

            stream.AddSubscriber(sub);
            stream.Parse(payload);
            stream.DeliverSubscriptions();
        }
    }

    public class Receiver<T> : DataReceiver
    {
        public T receivedPayload;


        public Receiver(string dataType = "int") : base(dataType)
        {
        }
        

        public void ReceivePayload(T payload)
        {
            receivedPayload = payload;
            Console.WriteLine(payload);
        }
    }
}
