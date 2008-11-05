using System.Collections.Generic;
using System;

namespace Pretorianie.Tytan.Core.Comparers
{
    /// <summary>
    /// Class that compares two string to achieve order of namespaces.
    /// </summary>
    public class NamespaceComparer : IComparer<string>
    {
        private readonly char separator;

        /// <summary>
        /// Init constructor.
        /// Sets the char that is a namespace separator inside the string.
        /// </summary>
        public NamespaceComparer(char namespaceSeparator)
        {
            separator = namespaceSeparator;
        }

        #region IComparer<string> Members

        ///<summary>
        ///Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
        ///</summary>
        ///
        ///<returns>
        ///Value Condition Less than zerox is less than y.Zerox equals y.Greater than zerox is greater than y.
        ///</returns>
        ///
        ///<param name="y">The second object to compare.</param>
        ///<param name="x">The first object to compare.</param>
        public int Compare(string x, string y)
        {
            if (string.IsNullOrEmpty(x))
                return -1;
            if (string.IsNullOrEmpty(y))
                return 1;

            string[] namespaces_x = x.Split(separator);
            string[] namespaces_y = y.Split(separator);

            int min = Math.Min(namespaces_x.Length, namespaces_y.Length);

            for (int i = 0; i < min; i++)
            {
                int result = string.Compare(namespaces_x[i], namespaces_y[i]);
                if (result != 0)
                    return result;
            }

            if (namespaces_x.Length < namespaces_y.Length)
                return -2;
            else if (namespaces_x.Length > namespaces_y.Length)
                return 2;

            return 0;
        }

        #endregion
    }
}
