using LiteDB;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatStat
{
    /// <summary>
    /// Describes a value that can be observed. When the value is changed, a callback method is invoked passing the Observed value as parameter
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ObservableNumericValue<T> : IObservableNumericValue where T : IComparable
    {
        protected T value;
        private T minVal;
        private T maxVal;
        private string label;

        [BsonIgnore]
        public Type type { get{ return typeof(T); } }

        private Action<IObservableNumericValue> onUpdate_callback;

        public object Value {
            get {
                return value;
            }
            set {
                bool updated = !this.Equal((T) value);

                this.value = (T) value;

                if(updated && onUpdate_callback != null)
                {
                    onUpdate_callback.Invoke(this);
                }
            }
        }
        public object Max { get { return maxVal; } set { this.maxVal = (T) value; } }
        public object Min { get { return minVal; } set { this.minVal = (T) value; } }
        public string Label { get { return label; } set { this.label = value; } }
        
    
        public bool Over()
        {
            return value.CompareTo(maxVal) > 0;
        }

        public bool Under()
        {
            return value.CompareTo(minVal) < 0;
        }

        public bool EqualMin()
        {
            return value.CompareTo(minVal) == 0;
        }

        public bool EqualMax()
        {
            return value.CompareTo(maxVal) == 0;
        }

        public bool Equal(T testVal)
        {
            return value.CompareTo(testVal) == 0;
        }

        public bool Stable()
        {
            return (!Over() || EqualMax()) && (!Under() || EqualMin());
        }

        public void OnUpdate(Action<IObservableNumericValue> cb)
        {
            onUpdate_callback = cb;
        }

        public abstract object Diff();
    }

    public class ObservableInt : ObservableNumericValue<int>
    {
        public override object Diff()
        {
            if(Over())
            {
                return (int)value - (int)Max;
            } else if(Under())
            {
                return (int)value - (int)Min;
            }

            return 0;
        }
    }

    public class ObservableFloat : ObservableNumericValue<float>
    {
        public override object Diff()
        {
            if (Over())
            {
                return (float)value - (float)Max;
            }
            else if (Under())
            {
                return (float)value - (float)Min;
            }

            return 0;
        }
    }

    public class ObservableDouble : ObservableNumericValue<double>
    {
        public override object Diff()
        {
            if (Over())
            {
                return (double)value - (double)Max;
            }
            else if (Under())
            {
                return (double)value - (double)Min;
            }

            return 0;
        }
    }

    public class ObservableLong: ObservableNumericValue<long>
    {
        public override object Diff()
        {
            if (Over())
            {
                return (long)value - (long)Max;
            }
            else if (Under())
            {
                return (long)value - (long)Min;
            }

            return 0;
        }
    }

    /// <summary>
    /// An interface for ObservableNumericValue so that we can create lists without worrying about different generics in the same list
    /// </summary>
    public interface IObservableNumericValue
    {
        /// <summary>
        /// Check if the value is above the defined max value
        /// </summary>
        /// <returns>Returns true if the value is above max value, false otherwise</returns>
        bool Over();

        /// <summary>
        /// Check if the value is below the defined min value
        /// </summary>
        /// <returns>Returns true if the value is below min value, false otherwise</returns>
        bool Under();

        /// <summary>
        /// Check if the value is between or equal to min or max
        /// </summary>
        /// <returns>Returns true if the value is equal or between min or max, false otherwise</returns>
        bool Stable();

        /// <summary>
        /// Check if the value is equal to min
        /// </summary>
        /// <returns>True is equal to min, false otherwise</returns>
        bool EqualMin();

        /// <summary>
        /// Check if the value is equal to max
        /// </summary>
        /// <returns>True if equal to max, false otherwise</returns>
        bool EqualMax();

        /// <summary>
        /// The value contained in the object
        /// </summary>
        object Value { get; set; }

        /// <summary>
        /// A minimum value accepted for this object
        /// </summary>
        object Min { get; set; }

        /// <summary>
        /// A maximum value accepted for this object
        /// </summary>
        object Max { get; set; }

        /// <summary>
        /// Register a callback function to invoke when the value changes
        /// </summary>
        /// <param name="cb">An action passing an IObservableNumericvalue as parameter</param>
        void OnUpdate(Action<IObservableNumericValue> cb);

        /// <summary>
        /// A label for this object
        /// </summary>
        string Label { get; set; }

        /// <summary>
        /// Should return the generic type that this object was initialized with
        /// </summary>
        Type type { get; }

        /// <summary>
        /// Returns the offset between the current value and the min or max value, which ever is violated
        /// </summary>
        /// <returns>The different between the current violated max or min</returns>
        object Diff();
    }

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

        public IObservableNumericValue this[int index] {
            get {
                return valueCollection[index];
            }
            set {
                valueCollection[index] = (IObservableNumericValue) value;
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
            IObservableNumericValue val;
            foreach(IObservableNumericValue o in this)
            {
                if(o.Label == label)
                {
                    return o;
                }
            }

            throw new ArgumentOutOfRangeException();
        }

        public void Set(string label, IObservableNumericValue value)
        {
            IObservableNumericValue val;
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
            for(i=0; i<oldLength; i++)
            {
                newList[i] = oldList[i];
            }

            newList[i] = val;
            valueCollection = newList;

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
            for(int i=0; i<valueCollection.Length; i++)
            {
                IObservableNumericValue o = valueCollection[i];

                bool a = value.Value.Equals(o.Value);
                bool b = value.Label == o.Label;
                bool c = value.Value.GetType() == o.Value.GetType();
                bool d = value == o;

                if((a && b && c) || d)
                {
                    return i;
                }
            }

            return -1;
        }

        public bool ContainsLabel(string label)
        {
            foreach(IObservableNumericValue n in this)
            {
                if(n.Label == label)
                {
                    return true;
                }
            }

            return false;
        }

        public void Insert(int index, IObservableNumericValue value)
        {
            IObservableNumericValue[] newList = new IObservableNumericValue[valueCollection.Length + 1];

            for(int i=0; i<newList.Length; i++)
            {
                if(i < index)
                {
                    newList[i] = valueCollection[i];
                }

                if(i == index)
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
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(IObservableNumericValue[] array, int index)
        {
            throw new NotImplementedException();
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
            return (position < valueCollection.Length);
        }

        public void Reset()
        {
            position = -1;
        }
    }
}
