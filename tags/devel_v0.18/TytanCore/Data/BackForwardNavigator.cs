using System.Collections.Generic;

namespace Pretorianie.Tytan.Core.Data
{
    /// <summary>
    /// Abstract navigator that allows to browse in the similar way as inside text editors to undo/redo modifications.
    /// </summary>
    /// <typeparam name="T">Type of the information that will be navigated.</typeparam>
    public class BackForwardNavigator<T>
    {
        private readonly List<T> backwards = new List<T>();
        private readonly List<T> forwards = new List<T>();
        private readonly int maxItems;

        /// <summary>
        /// Init constructor.
        /// Remembers the maximum number of elements that can be added into both internal collections.
        /// </summary>
        /// <param name="maxItems"></param>
        public BackForwardNavigator(int maxItems)
        {
            this.maxItems = maxItems;
        }

        #region Properties

        /// <summary>
        /// Checks if it is possible to go back.
        /// </summary>
        public bool CanGoBack
        {
            get { return backwards.Count > 0; }
        }

        /// <summary>
        /// Checks if it is possible to go forward.
        /// </summary>
        public bool CanGoForward
        {
            get { return forwards.Count > 0; }
        }

        #endregion

        /// <summary>
        /// Remembers new item to enable later possibility of going back.
        /// </summary>
        /// <param name="item">Item that describes the current state.</param>
        public void Add(T item)
        {
            Add(item, true, true);
        }

        /// <summary>
        /// Remembers new item to enable later possibility of going back.
        /// </summary>
        /// <param name="item">Item that describes the current state.</param>
        /// <param name="clearForwards">'true' if it is impossible to move forward after adding current item</param>
        /// <param name="checkDuplicates">'true' if the same item can not be added twice in a row</param>
        public void Add(T item, bool clearForwards, bool checkDuplicates)
        {
            if (CanGoBack)
            {
                T lastAdded = backwards[0];

                // do not add duplicates:
                if (lastAdded.Equals(item))
                    return;
            }

            backwards.Insert(0, item);
            if (backwards.Count + forwards.Count > maxItems)
                backwards.RemoveAt(backwards.Count - 1);

            if (clearForwards)
                forwards.Clear();
        }

        /// <summary>
        /// Removes all stored items.
        /// </summary>
        public void Clear()
        {
            backwards.Clear();
            forwards.Clear();
        }

        /// <summary>
        /// Gets the last stored item.
        /// Returns 'true' if there is something valid set.
        /// </summary>
        public bool GoBack(out T item, T currentItem)
        {
            if (CanGoBack)
            {
                // get the last added item:
                item = backwards[0];
                backwards.RemoveAt(0);
                forwards.Insert(0, currentItem);
                return true;
            }

            item = default(T);

            return false;
        }

        /// <summary>
        /// Gets the last stored item.
        /// Returns 'true' if there is something valid set.
        /// </summary>
        public bool GoForward(out T item, T currentItem)
        {
            if (CanGoForward)
            {
                // get the last added item:
                item = forwards[0];
                forwards.RemoveAt(0);
                backwards.Insert(0, currentItem);
                return true;
            }

            item = default(T);

            return false;
        }
    }
}
