using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatStat
{
    public class ParameterControlTemplate
    {
        public ObjectId Id { get; set; }
        public IObservableNumericValue[] Collection { get; set; }
        public string Name { get; set; }
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
    }
}
