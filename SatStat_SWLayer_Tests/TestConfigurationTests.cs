using Microsoft.VisualStudio.TestTools.UnitTesting;
using SatStat;
using System;
using System.Collections.Generic;
using System.Text;

namespace SatStatTests
{
    [TestClass]
    public class TestConfigurationTests
    {
        [TestMethod]
        public void RunTestConfiguration()
        {
            TestConfiguration testConfig = new TestConfiguration();

            StreamSimulator simulator = new StreamSimulator();
            DataReceiver receiver = new DataReceiver();


            simulator.Connect();
        }
    }
}
