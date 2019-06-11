using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatStat
{
    /// <summary>
    /// A collection of observable numeric values
    /// </summary>
    public class ObservableNumericValueCollection : IEnumerable, IList<IObservableNumericValue>
    {
        private IObservableNumericValue[] valueCollection;
        public bool IsReadOnly => false;
        public bool IsFixedSize => false;
        public int Count => valueCollection.Length;
        public object SyncRoot => this;
        public bool IsSynchronized => false;

        public ObservableNumericValueCollection()
        {
            valueCollection = new IObservableNumericValue[0];
        }

        public IObservableNumericValue this[int index]
        {
            get
            {
                return valueCollection[index];
            }
            set
            {
                valueCollection[index] = (IObservableNumericValue)value;
            }
        }

        public IObservableNumericValue this[string label]
        {
            get
            {
                return Get(label);
            }
            set
            {
                Set(label, value);
            }
        }

        public IObservableNumericValue Get(int index)
        {
            return valueCollection[index];
        }

        public IObservableNumericValue Get(string label)
        {
            foreach (IObservableNumericValue o in this)
            {
                if (o.Label == label)
                {
                    return o;
                }
            }

            throw new ArgumentOutOfRangeException();
        }

        public void Set(string label, IObservableNumericValue value)
        {
            foreach (IObservableNumericValue o in this)
            {
                if (o.Label == label)
                {
                    o.Value = value;
                    return;
                }
            }

            throw new ArgumentOutOfRangeException();
        }

        public void Add(IObservableNumericValue val)
        {
            int oldLength = valueCollection.Length;
            int newLength = oldLength + 1;

            IObservableNumericValue[] oldList = valueCollection;
            IObservableNumericValue[] newList = new IObservableNumericValue[newLength];

            int i;
            for (i = 0; i < oldLength; i++)
            {
                newList[i] = oldList[i];
            }

            newList[i] = val;
            valueCollection = newList;

        }

        public IObservableNumericValue AddWithType(string label, string type)
        {
            IObservableNumericValue observedAttribute = new ObservableInt();
            bool observe_invalid = false;
            string observe_error = "";

            switch (type)
            {
                case "System.Single":
                case "float":
                    observedAttribute = new ObservableFloat();
                    break;
                case "System.Double":
                case "double":
                    observedAttribute = new ObservableDouble();
                    break;
                case "System.Int32":
                case "int":
                    observedAttribute = new ObservableInt();
                    break;
                case "System.Int64":
                case "long":
                    observedAttribute = new ObservableLong();
                    break;
                case "string":
                    observe_invalid = true;
                    observe_error = "A string value cannot be observed";
                    break;
                case "JObject":
                    observe_invalid = true;
                    observe_error = "A JObject cannot be observed";
                    break;
                case "JArray":
                    observe_invalid = true;
                    observe_error = "A JArray cannot be observed: ";
                    break;
            }

            if(observe_invalid)
            {
                Debug.Log(observe_error);
                return null;
            } else
            {
                observedAttribute.Label = label;
                Add(observedAttribute);
            }

            return observedAttribute;
        }

        public IEnumerator GetEnumerator()
        {
            return new ObservableNumericValueCollectionEnum(valueCollection);
        }

        public bool Contains(IObservableNumericValue value)
        {
            return IndexOf(value) > -1;
        }

        public void Clear()
        {
            valueCollection = new IObservableNumericValue[0];
        }

        public int IndexOf(IObservableNumericValue value)
        {
            for (int i = 0; i < valueCollection.Length; i++)
            {
                IObservableNumericValue o = valueCollection[i];

                if (o == null)
                {
                    continue;
                }

                bool a = value.Value.Equals(o.Value);
                bool b = value.Label == o.Label;
                bool c = value.Value.GetType() == o.Value.GetType();
                bool d = value == o;

                if ((a && b && c) || d)
                {
                    return i;
                }
            }

            return -1;
        }

        public bool ContainsLabel(string label)
        {
            foreach (IObservableNumericValue n in this)
            {
                if (n.Label == label)
                {
                    return true;
                }
            }

            return false;
        }

        public void Insert(int index, IObservableNumericValue value)
        {
            IObservableNumericValue[] newList = new IObservableNumericValue[valueCollection.Length + 1];

            for (int i = 0; i < newList.Length; i++)
            {
                if (i < index)
                {
                    newList[i] = valueCollection[i];
                }

                if (i == index)
                {
                    newList[i] = value;
                }

                if (i > index)
                {
                    newList[i] = valueCollection[i - 1];
                }
            }

            valueCollection = newList;
        }

        public bool Remove(IObservableNumericValue value)
        {
            for(int i=0; i < valueCollection.Length; i++)
            {
                IObservableNumericValue v = valueCollection[i];
                if (v == value)
                {
                    valueCollection[i] = null;
                    return true;
                }
            }

            return false;
        }

        public void RemoveAt(int index)
        {
            valueCollection[index] = null;
        }

        public void CopyTo(IObservableNumericValue[] array, int index)
        {
            throw new NotImplementedException();
        }

        public IObservableNumericValue[] ToArray()
        {
            return valueCollection;
        }

        IEnumerator<IObservableNumericValue> IEnumerable<IObservableNumericValue>.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }

    public class ObservableNumericValueCollectionEnum : IEnumerator
    {
        private int position = -1;
        private IObservableNumericValue[] valueCollection;

        public ObservableNumericValueCollectionEnum(IObservableNumericValue[] values)
        {
            valueCollection = values;
        }

        public object Current
        {
            get
            {
                return valueCollection[position];
            }
        }

        public bool MoveNext()
        {
            position++;
            if (position < valueCollection.Length)
            {
                if (valueCollection[position] == null)
                {
                    return MoveNext();
                }
            }

            return (position < valueCollection.Length);
        }

        public void Reset()
        {
            position = -1;
        }
    }
}
