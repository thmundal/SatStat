using LiteDB;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatStat
{
    public enum ObservableNumericValueStatus
    {
        Unknown,
        Under,
        Over,
        Stable,
        Unstable
    }

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

        public ObservableNumericValueStatus Status()
        {
            ObservableNumericValueStatus status = ObservableNumericValueStatus.Unknown;
            if (Over())
            {
                status = ObservableNumericValueStatus.Over;
            }

            if (Under())
            {
                status = ObservableNumericValueStatus.Under;
            }

            if (Stable())
            {
                status = ObservableNumericValueStatus.Stable;
            }

            return status;
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

        ObservableNumericValueStatus Status();
    }
}
