using LiteDB;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SatStat;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SatStatTests
{
    [TestClass]
    public class DatabaseTests
    {
        [TestMethod]
        public void SaveComSettings()
        {
            string path = Directory.GetCurrentDirectory() + @"\TestDatabase.db";
            using (LiteDatabase db = new LiteDatabase(@path))
            {
                LiteCollection<DB_ComSettingsItem> collection = db.GetCollection<DB_ComSettingsItem>("ComSettings");

                IEnumerable<DB_ComSettingsItem> results = collection.FindAll();

                DB_ComSettingsItem store = new DB_ComSettingsItem
                {
                    baud_rate = 9600,
                    Parity = 1,
                    DataBits = 16,
                    StopBits = 1,
                    NewLine = SerialHandler.default_settings.NewLine,
                    Config = "8N1"
                };

                if (results.Count() > 0)
                {
                    // Update existing item
                    DB_ComSettingsItem existing = results.First();
                    ObjectId id = existing.Id;
                    store.Id = id;
                    bool updateSucess = collection.Update(store);
                    Console.WriteLine("Settings document exists, updating values...");
                    Assert.IsTrue(updateSucess);
                }
                else
                {
                    // Add item
                    ObjectId insert_id = collection.Insert(store);
                    Console.WriteLine("Setting document does not exist, creating....");
                    Assert.AreNotEqual(null, insert_id);
                }

                Assert.AreEqual(1, results.Count());
            }
        }
    }
}
