using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using SatStat;
using Newtonsoft.Json.Linq;

namespace SatStatTests
{
    [TestClass]
    public class OutputDataSerialization
    {


        public OutputDataSerialization()
        {
        }

        [TestMethod]
        public void DeliverFloat()
        {
            DataStream stream = new DataStream();
            DataProvider provider = new DataProvider(stream);

            stream.OnOutputReceived((string data) =>
            {
                Console.WriteLine(data);
                Assert.AreEqual("2.0", data);
            });

            provider.Deliver(2.0f);
        }

        [TestMethod]
        public void DeliverString()
        {
            DataStream stream = new DataStream();
            DataProvider provider = new DataProvider(stream);

            stream.OnOutputReceived((string data) =>
            {
                Console.WriteLine(data);
                Assert.AreEqual("\"This is a string\"", data);
            });

            provider.Deliver("This is a string");
        }


        [TestMethod]
        public void DeliverInt()
        {
            DataStream stream = new DataStream();
            DataProvider provider = new DataProvider(stream);

            stream.OnOutputReceived((string data) =>
            {
                Console.WriteLine(data);
                Assert.AreEqual("50", data);
            });

            provider.Deliver(50);
        }

        [TestMethod]
        public void DeliverLong()
        {
            DataStream stream = new DataStream();
            DataProvider provider = new DataProvider(stream);

            stream.OnOutputReceived((string data) =>
            {
                Console.WriteLine(data);
                Assert.AreEqual("50", data);
            });

            provider.Deliver(50L);
        }

        [TestMethod]
        public void DeliverDouble()
        {
            DataStream stream = new DataStream();
            DataProvider provider = new DataProvider(stream);

            stream.OnOutputReceived((string data) =>
            {
                Console.WriteLine(data);
                Assert.AreEqual("1.0", data);
            });

            provider.Deliver(1.0f);
        }
        
        public void DeliverKeyValPair()
        {
            DataStream stream = new DataStream();
            DataProvider provider = new DataProvider(stream);

            stream.OnOutputReceived((string data) =>
            {
                Console.WriteLine(data);
                Assert.AreEqual("{\"test\":1}", data);
            });

            KeyValuePair<string, int> kvp = new KeyValuePair<string, int>("test", 1);

            provider.Deliver(kvp);
        }

        [TestMethod]
        public void DeliverHashtable()
        {
            DataStream stream = new DataStream();
            DataProvider provider = new DataProvider(stream);

            stream.OnOutputReceived((string data) =>
            {
                Console.WriteLine(data);
                Assert.AreEqual("{\"test\":1}", data);
            });
            
            Hashtable hs = new Hashtable();
            hs.Add("test", 1);

            provider.Deliver(hs);
        }

        [TestMethod]
        public void DeliverJObject()
        {
            DataStream stream = new DataStream();
            DataProvider provider = new DataProvider(stream);

            stream.OnOutputReceived((string data) =>
            {
                Console.WriteLine(data);
                Assert.AreEqual("{\"test\":1}", data);
            });

            JObject obj = new JObject();
            obj.Add("test", 1);

            provider.Deliver(obj);
        }
    }
}
