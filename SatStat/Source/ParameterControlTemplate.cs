﻿using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatStat
{
    /// <summary>
    /// An object of this class holds a collection of observable numeric values and can be saved and loaded from a LiteDatabase collection
    /// </summary>
    public class ParameterControlTemplate
    {
        public ObjectId Id { get; set; }
        
        public IObservableNumericValue[] Collection { get; set; }
        public string Name { get; set; }
        [BsonIgnore]
        public DateTime Date { get; set; }
        public string Description { get; set; }

        public ParameterControlTemplate()
        {
        }

        public ParameterControlTemplate(ObservableNumericValueCollection c)
        {
            SetCollection(c);
            Date = DateTime.Now;
        }

        public void SetCollection(ObservableNumericValueCollection c)
        {
            Collection = new IObservableNumericValue[c.Count];

            for (int i = 0; i < c.Count; i++)
            {
                Collection[i] = c[i];
            }
        }

        public ObservableNumericValueCollection GetCollection()
        {
            ObservableNumericValueCollection c = new ObservableNumericValueCollection();

            if (Collection.Length > 0)
            {
                foreach(IObservableNumericValue n in Collection)
                {
                    c.Add(n);
                }
            }

            return c;
        }

        public static List<ParameterControlTemplate> GetListFromDb()
        {
            List<ParameterControlTemplate> list = new List<ParameterControlTemplate>();

            using (LiteDatabase db = new LiteDatabase(Program.settings.DatabasePath))
            {
                LiteCollection<ParameterControlTemplate> collection = db.GetCollection<ParameterControlTemplate>(Program.settings.ParameterControlDatabase);

                IEnumerable<ParameterControlTemplate> d = collection.FindAll();

                list = d.ToList();
            }

            return list;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
