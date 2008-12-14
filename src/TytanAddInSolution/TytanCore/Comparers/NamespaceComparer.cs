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

            string[] namespacesX = x.Split(separator);
            string[] namespacesY = y.Split(separator);

            int min = Math.Min(namespacesX.Length, namespacesY.Length);

            for (int i = 0; i < min; i++)
            {
                int result = string.Compare(namespacesX[i], namespacesY[i]);
                if (result != 0)
                    return result;
            }

            if (namespacesX.Length < namespacesY.Length)
                return -2;
            
            return namespacesX.Length > namespacesY.Length ? 2 : 0;
        }

        #endregion
    }
}
