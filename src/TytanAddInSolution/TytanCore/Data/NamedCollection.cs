using System.Collections.Generic;

namespace Pretorianie.Tytan.Core.Data
{
    /// <summary>
    /// Generic class that stores collection of named values.
    /// </summary>
    public class NamedValueCollection<T>
    {
        private readonly SortedDictionary<string, T> items = new SortedDictionary<string, T>();

        /// <summary>
        /// Gets the number of stored items.
        /// </summary>
        public int Count
        {
            get { return items.Count; }
        }

        /// <summary>
        /// Gets the names of stored values.
        /// </summary>
        public IEnumerable<string> Names
        {
            get { return items.Keys; }
        }

        /// <summary>
        /// Gets the list of stored values.
        /// </summary>
        public IEnumerable<T> Values
        {
            get { return items.Values; }
        }

        /// <summary>
        /// Gets the value with given name.
        /// </summary>
        public T this[string name]
        {
            get { return items[name]; }
        }

        /// <summary>
        /// Remembers new item inside collection.
        /// </summary>
        public bool Add(string name, T value)
        {
            if (items.ContainsKey(name))
            {
                items[name] = value;
                return false;
            }
            else
            {
                items.Add(name, value);
                return true;
            }
        }

        /// <summary>
        /// Gets the value of given name.
        /// </summary>
        public bool TryGetValue(string name, out T item)
        {
            return items.TryGetValue(name, out item);
        }

        /// <summary>
        /// Removes all the stored items.
        /// </summary>
        public void Clear()
        {
            items.Clear();
        }
    }
}
