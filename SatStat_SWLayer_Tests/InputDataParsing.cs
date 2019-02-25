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
            DataReceiver receiver = new DataReceiver();

            receiver.OnPayloadReceived((object payload, string attribute) =>
            {
                Assert.AreEqual(2.0, payload);
                Assert.AreEqual(typeof(double), payload.GetType());
                Console.WriteLine(payload.GetType());
            });

            receiver.Subscribe(stream, "double", "double");

            stream.Parse(JSON);
            stream.DeliverSubscriptions();

        }

        [TestMethod]
        public void ReceiveString()
        {
            DataStream stream = new DataStream();
            DataReceiver receiver = new DataReceiver();

            receiver.OnPayloadReceived((object payload, string attribute) =>
            {
                Assert.AreEqual("this is a string", payload);
                Assert.AreEqual(typeof(string), payload.GetType());
                Console.WriteLine(payload.GetType());
            });

            receiver.Subscribe(stream, "string", "string");

            stream.Parse(JSON);
            stream.DeliverSubscriptions();

        }

        [TestMethod]
        public void ReceiveFloat()
        {
            DataStream stream = new DataStream();
            DataReceiver receiver = new DataReceiver();


            receiver.OnPayloadReceived((object payload, string attribute) =>
            {
                Assert.AreEqual(3.2f, payload);
                Assert.AreEqual(typeof(float), payload.GetType());
                Console.WriteLine(payload.GetType());
            });

            receiver.Subscribe(stream, "float", "float");

            stream.Parse(JSON);
            stream.DeliverSubscriptions();
        }

        [TestMethod]
        public void ReceiveInt()
        {
            DataStream stream = new DataStream();
            DataReceiver receiver = new DataReceiver();

            receiver.OnPayloadReceived((object payload, string attribute) =>
            {
                Assert.AreEqual(5, payload);
                Assert.AreEqual(typeof(int), payload.GetType());
                Console.WriteLine(payload.GetType());
            });

            receiver.Subscribe(stream, "int", "int");

            stream.Parse(JSON);
            stream.DeliverSubscriptions();

        }

        [TestMethod]
        public void ReceiveLong()
        {
            DataStream stream = new DataStream();
            DataReceiver receiver = new DataReceiver();

            receiver.OnPayloadReceived((object payload, string attribute) =>
            {
                Assert.AreEqual(5L, payload);
                Assert.AreEqual(typeof(long), payload.GetType());
                Console.WriteLine(payload.GetType());
            });

            receiver.Subscribe(stream, "long", "long");

            stream.Parse(JSON);
            stream.DeliverSubscriptions();

        }

        [TestMethod]
        public void ReceiveJArray()
        {
            DataStream stream = new DataStream();
            DataReceiver receiver = new DataReceiver();

            receiver.OnPayloadReceived((object payload, string attribute) => {

                JArray expected = new JArray { "one", "two", "three" };
                JArray actual = (JArray) payload;

                for (int i = 0; i < expected.Count; i++)
                {
                    Assert.AreEqual(expected[i], actual[i]);
                }
                Console.Write("Datatype: ");
                Console.WriteLine(payload.GetType());
            });

            receiver.Subscribe(stream, "genericlist", "JArray");

            stream.Parse(JSON);
            stream.DeliverSubscriptions();
        }

        [TestMethod]
        public void ReceiveJObject()
        {
            DataStream stream = new DataStream();
            DataReceiver receiver = new DataReceiver();

            receiver.OnPayloadReceived((object p, string attribute) =>
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

            receiver.Subscribe(stream, "hashtable", "JObject");

            stream.Parse(JSON);
            stream.DeliverSubscriptions();
        }

        [TestMethod]
        public void MultipleSubscriptionOnDifferentKeys()
        {
            DataStream stream = new DataStream();
            DataReceiver receiver = new DataReceiver();
            DataReceiver receiver2 = new DataReceiver();
            
            receiver.OnPayloadReceived((object p, string attribute) =>
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

            receiver2.OnPayloadReceived((object payload, string attribute) =>
            {
                Assert.AreEqual(5, payload);
                Assert.AreEqual(typeof(int), payload.GetType());
                Console.WriteLine(payload.GetType());
            });

            receiver.Subscribe(stream, "hashtable", "JObject");
            receiver2.Subscribe(stream, "int", "int");

            stream.Parse(JSON);
            stream.DeliverSubscriptions();
        }

        [TestMethod]
        public void MultipleSubscribersOnSameKey()
        {
            DataStream stream = new DataStream();
            DataReceiver receiver = new DataReceiver();
            DataReceiver receiver2 = new DataReceiver();
            
            receiver.OnPayloadReceived((object payload, string attribute) =>
            {
                Assert.AreEqual(5, payload);
                Assert.AreEqual(typeof(int), payload.GetType());
                Console.WriteLine(payload.GetType());
            });

            receiver2.OnPayloadReceived((object payload, string attribute) =>
            {
                Assert.AreEqual(5, payload);
                Assert.AreEqual(typeof(int), payload.GetType());
                Console.WriteLine(payload.GetType());
            });

            receiver.Subscribe(stream, "int", "int");
            receiver2.Subscribe(stream, "int", "int");

            stream.Parse(JSON);
            stream.DeliverSubscriptions();
        }

        [TestMethod]
        public void ReceiveSensorList()
        {
            string payload = "{\"available_sensors\":[{\"temperature\":\"double\"}, {\"humidity\":\"int\"}]}";
            DataStream stream = new DataStream();
            DataReceiver receiver = new DataReceiver();
            
            receiver.OnPayloadReceived((object received_payload, string attribute) =>
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

            receiver.Subscribe(stream, "available_sensors", "JArray");

            stream.Parse(payload);
            stream.DeliverSubscriptions();
        }

        [TestMethod]
        public void OneSubscriberMultipleKeys()
        {
            string payload = "{\"a\":1, \"b\":2}";
            DataStream stream = new DataStream();
            DataReceiver receiver = new DataReceiver();

            receiver.Subscribe(stream, "a", "int");
            receiver.Subscribe(stream, "b", "int");

            receiver.OnPayloadReceived((object actual, string attribute) =>
            {
                if(attribute == "a")
                {
                    Assert.AreEqual(1, actual);
                } else if(attribute == "b")
                {
                    Assert.AreEqual(2, actual);
                }
            });

            stream.Parse(payload);
            stream.DeliverSubscriptions();
        }
    }
}
