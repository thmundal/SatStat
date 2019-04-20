using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace SatStatTests
{
    [TestClass]
    public class QueueTests
    {
        [TestMethod]
        public void ReferenceTests()
        {
            Thing a = new Thing { name = "Jon", number = 1 };
            Thing b = new Thing { name = "Ming", number = 2 };
            Thing c = new Thing { name = "Rita", number = 3 };
            Thing d = new Thing { name = "Erica", number = 4 };

            List<Thing> thingList = new List<Thing>();
            thingList.Add(a);
            thingList.Add(b);
            thingList.Add(c);
            thingList.Add(d);

            Queue<Thing> thingQueue = new Queue<Thing>();

            foreach(Thing t in thingList)
            {
                thingQueue.Enqueue(t);
            }

            Thing u = thingQueue.Dequeue();

            int index = thingList.IndexOf(u);

            Assert.AreNotEqual(-1, index);
            Assert.AreEqual(thingList[0], u);
        }
    }

    public class Thing
    {
        public string name;
        public int number;
    }
}
