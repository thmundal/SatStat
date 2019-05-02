using LiteDB;
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
        public DateTime Date { get; set; }
        public string Description { get; set; }

        private Dictionary<string, int> observableValueArrayKeyMap;

        public ParameterControlTemplate()
        {
            observableValueArrayKeyMap = new Dictionary<string, int>();
        }

        public ParameterControlTemplate(ObservableNumericValueCollection c)
        {
            observableValueArrayKeyMap = new Dictionary<string, int>();
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
            foreach(IObservableNumericValue n in Collection)
            {
                c.Add(n);
            }
            return c;
        }

        public IObservableNumericValue getValue(string label)
        {
            if(observableValueArrayKeyMap.Count != Collection.Length)
            {
                observableValueArrayKeyMap.Clear();
                for(int i=0; i<Collection.Length; i++)
                {
                    IObservableNumericValue n = Collection[i];

                    if(n.Label == label)
                    {
                        observableValueArrayKeyMap.Add(label, i);
                        return n;
                    }
                }
            } else
            {
                if(observableValueArrayKeyMap.ContainsKey(label))
                {
                    return Collection[observableValueArrayKeyMap[label]];
                }
            }

            return null;
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
    }
}
