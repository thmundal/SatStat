using Microsoft.VisualStudio.TestTools.UnitTesting;
using SatStat;
using System;
using System.Collections.Generic;
using System.Text;

namespace SatStatTests
{
    [TestClass]
    public class ObservableValueTests
    {
        [TestMethod]
        public void TestGeneric()
        {
            ObservableInt a = new ObservableInt { Value = 2, Min = 0, Max = 3 };
            ObservableFloat b = new ObservableFloat { Value = 2.0f, Min = 0.0f, Max = 3.0f };
            
            Console.WriteLine(a.Over());

            Assert.IsFalse(a.Over());
            Assert.IsFalse(a.Under());

            Assert.IsFalse(b.Over());
            Assert.IsFalse(b.Under());

            Assert.IsTrue(a.Stable());
            Assert.IsTrue(b.Stable());
        }

        [TestMethod]
        public void TestInterfaceArray()
        {
            IObservableNumericValue[] array = new IObservableNumericValue[5];

            array[0] = new ObservableInt { Value = 1, Min = -1, Max = 1 };
            array[1] = new ObservableFloat{ Value = 1.0f, Min = -1.0f, Max = 2.0f };
            array[2] = new ObservableDouble { Value = 1.5d, Min = -1.5d, Max = 2.0d };
            array[3] = new ObservableInt { Value = 1, Min = -1, Max = 1 };
            array[4] = new ObservableFloat { Value = 1.0f, Min = -1.0f, Max = 1.0f };

            foreach(IObservableNumericValue val in array)
            {
                Assert.IsFalse(val.Over());
                Assert.IsFalse(val.Under());
                Assert.IsTrue(val.Stable());
            }
        }

        [TestMethod]
        public void TestInterfaceList()
        {
            List<IObservableNumericValue> list = new List<IObservableNumericValue>();

            list.Add(new ObservableInt { Value = 1, Min = -1, Max = 1 });
            list.Add(new ObservableFloat { Value = 1.0f, Min = -1.0f, Max = 2.0f });
            list.Add(new ObservableDouble { Value = 1.5d, Min = -1.5d, Max = 2.0d });
            list.Add(new ObservableInt { Value = 1, Min = -1, Max = 1 });
            list.Add(new ObservableFloat { Value = 1.0f, Min = -1.0f, Max = 1.0f });

            foreach (IObservableNumericValue val in list)
            {
                Assert.IsFalse(val.Over());
                Assert.IsFalse(val.Under());
                Assert.IsTrue(val.Stable());
            }
        }

        [TestMethod]
        public void TestCollection()
        {
            ObservableNumericValueCollection collection = new ObservableNumericValueCollection();

            ObservableInt oInt = new ObservableInt { Value = 1, Min = -1, Max = 1 };

            collection.Add(oInt);

            Assert.AreEqual(1, collection.Count);
            Assert.AreEqual(1, collection.Get(0).Value);

            Console.WriteLine(collection.Get(0).Value);

            collection.Add(oInt);

            Assert.AreEqual(2, collection.Count);
            Assert.AreEqual(1, collection[1].Value);
            Console.WriteLine(collection[1].Value);
            Console.WriteLine(collection[1].Value.GetType());

            collection[0].Value = 0;

            foreach(IObservableNumericValue val in collection)
            {
                Console.WriteLine(val.Value);
            }
        }

        [TestMethod]
        public void TestCollectionInsert()
        {
            ObservableNumericValueCollection collection = new ObservableNumericValueCollection();

            ObservableInt oInt = new ObservableInt { Value = 1, Min = -1, Max = 1 };
            ObservableInt oInt2 = new ObservableInt { Value = 2, Min = -1, Max = 1 };
            ObservableInt oInt3 = new ObservableInt { Value = 3, Min = -1, Max = 1 };
            ObservableFloat oFloat = new ObservableFloat { Value = 4.0f, Min = -1.0f, Max = 1.0f };

            collection.Add(oInt);
            collection.Add(oInt2);
            collection.Add(oInt3);

            collection.Insert(1, oFloat);

            Assert.AreEqual(1, collection[0].Value);
            Assert.AreEqual(4.0f, collection[1].Value);
            Assert.AreEqual(2, collection[2].Value);
            Assert.AreEqual(3, collection[3].Value);
        }

        [TestMethod]
        public void TestContains()
        {
            ObservableNumericValueCollection collection = new ObservableNumericValueCollection();

            ObservableInt oInt = new ObservableInt { Value = 1, Min = -1, Max = 1 };
            ObservableInt oInt2 = new ObservableInt { Value = 2, Min = -1, Max = 1 };
            ObservableInt oInt3 = new ObservableInt { Value = 3, Min = -1, Max = 1 };

            collection.Add(oInt);
            collection.Add(oInt2);
            collection.Add(oInt3);

            Assert.IsTrue(collection.Contains(oInt));
            Assert.IsTrue(collection.Contains(oInt2));
            Assert.IsTrue(collection.Contains(oInt3));
        }

        [TestMethod]
        public void TestIndexOf()
        {
            ObservableNumericValueCollection collection = new ObservableNumericValueCollection();

            ObservableInt oInt = new ObservableInt { Label = "int1", Value = 1, Min = -1, Max = 1 };
            ObservableInt oInt2 = new ObservableInt { Value = 2, Min = -1, Max = 1 };
            ObservableInt dontContain = new ObservableInt { Value = 3, Min = -1, Max = 1 };

            collection.Add(oInt);
            collection.Add(oInt2);

            // This is a new observable int that is not added to the list, but has a label and value that exists
            ObservableInt checkExists = new ObservableInt { Label = "int1", Value = 1, Min = -1, Max = 1 };
            
            Assert.AreEqual(0, collection.IndexOf(oInt));
            Assert.AreEqual(0, collection.IndexOf(checkExists));
            Assert.AreEqual(1, collection.IndexOf(oInt2));
            Assert.AreEqual(-1, collection.IndexOf(dontContain));
        }

        [TestMethod]
        public void TestConainsLabel()
        {

            ObservableNumericValueCollection collection = new ObservableNumericValueCollection();

            ObservableInt oInt = new ObservableInt { Label = "int1", Value = 1, Min = -1, Max = 1 };

            collection.Add(oInt);

            Assert.IsTrue(collection.ContainsLabel("int1"));
            Assert.IsFalse(collection.ContainsLabel("iDoNotExist"));
        }

        [TestMethod]
        public void TestStringIndexAccess()
        {
            ObservableNumericValueCollection collection = new ObservableNumericValueCollection();

            ObservableInt oInt = new ObservableInt { Label = "int1", Value = 1, Min = -1, Max = 1 };
            ObservableInt oInt2 = new ObservableInt { Value = 1, Min = -1, Max = 1 };

            collection.Add(oInt);
            collection.Add(oInt2);

            Assert.AreEqual(oInt, collection["int1"]);
        }

        public delegate void EventHandler1();

        [TestMethod]
        public void TestEventHandler()
        {
            ObservableInt oInt = new ObservableInt { Label = "int1", Value = 1, Min = -1, Max = 1 };

            oInt.OnUpdate((v) =>
            {
                Console.WriteLine("Callback 1");
            });

            oInt.OnUpdate((v) =>
            {
                Console.WriteLine("Callback 2");
            });

            oInt.Value = 4;
        }
    }
}
