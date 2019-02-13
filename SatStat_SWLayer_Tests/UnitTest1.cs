using Microsoft.VisualStudio.TestTools.UnitTesting;
using SatStat;
using System;
using System.Collections.Generic;

namespace SatStatTests
{
    [TestClass]
    public class UnitTest1 : DataReceiver<string>
    {
        string JSON = "{\"test\":\"value\"}";
        string value = "value";
        string received_payload;

        public void ReceivePayload(string payload)
        {
            received_payload = payload;
        }

        [TestMethod]
        public void TestMethod1()
        {
            DataSubscription<string> sub = new DataSubscription<string>(this, "test");


            Dictionary<string, string> json_parsed = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(JSON);

            received_payload = (string) sub.receive(json_parsed);
            Assert.AreEqual(received_payload, value);
        }


    }
}
