using System;
using System.Collections.Generic;

namespace Pretorianie.Tytan.Core.Data
{
    /// <summary>
    /// Collection that splits given name into items separated by given character
    /// (e.g. '.' or ':', '/') and then creates a set of collection that will store the
    /// items and all child items with separate names. It is similar to an image of local
    /// disk structure.
    /// </summary>
    public class NamedItemTreeCollection<T>
    {
        /// <summary>
        /// List of characters that separates different folder/items inside the name.
        /// </summary>
        public readonly static char[] SplitChars = new char[] { '.', ',', ':', '/', '\\', '_' };

        private readonly string name;
        private readonly char[] splitChars;
        private readonly SortedDictionary<string, NamedItemTreeCollection<T>> folders = new SortedDictionary<string, NamedItemTreeCollection<T>>();
        private readonly SortedDictionary<string, T> items = new SortedDictionary<string, T>();

        /// <summary>
        /// Default constructor.
        /// </summary>
        public NamedItemTreeCollection()
            : this(string.Empty, SplitChars)
        {
        }

        /// <summary>
        /// Init constructor.
        /// </summary>
        public NamedItemTreeCollection(params char[] splitNameChars)
            : this(string.Empty, splitNameChars)
        {
        }

        /// <summary>
        /// Init constructor.
        /// </summary>
        protected NamedItemTreeCollection(string currentName, params char[] splitNameChars)
        {
            name = currentName;
            splitChars = splitNameChars;
        }

        #region Properties

        /// <summary>
        /// Gets the name of current collection.
        /// </summary>
        public string Name
        {
            get { return name; }
        }

        /// <summary>
        /// Gets the local items.
        /// </summary>
        public IDictionary<string, T> Items
        {
            get { return items; }
        }

        /// <summary>
        /// Gets the existing folders.
        /// </summary>
        public IDictionary<string, NamedItemTreeCollection<T>> Folders
        {
            get { return folders; }
        }

        #endregion

        /// <summary>
        /// Removes all items from the collection.
        /// </summary>
        public void Clear ()
        {
            folders.Clear();
            items.Clear();
        }

        /// <summary>
        /// Adds new item to internal collection using name splitting.
        /// </summary>
        public void Add (string itemName, T item)
        {
            if (itemName == null)
                throw new ArgumentNullException("itemName");

            string[] names = itemName.Split(splitChars);
            string shortName = names[0];
            NamedItemTreeCollection<T> destination = this;
            NamedItemTreeCollection<T> subFolder;

            // find proper collection:
            if (names.Length > 1)
            {
                // find proper parent item or create such one:
                for ( int i = 0; i < names.Length - 1; i++ )
                {
                    if(destination.Folders.TryGetValue(names[i], out subFolder))
                    {
                        destination = subFolder;
                    }
                    else
                    {
                        // create all required subfolders:
                        for ( int j = i; j < names.Length - 1; j++ )
                        {
                            subFolder = new NamedItemTreeCollection<T>(names[j], splitChars);
                            destination.Add(subFolder);
                            destination = subFolder;
                        }

                        break;
                    }
                }
                shortName = names[names.Length - 1];
            }

            // add or overwrite item:
            destination.AddOrOverwrite(shortName, item);
        }

        /// <summary>
        /// Adds new folder or overwrites existing one.
        /// </summary>
        public void Add(NamedItemTreeCollection<T> folder)
        {
            if (folder == null)
                throw new ArgumentNullException("folder");

            NamedItemTreeCollection<T> f;

            if (folders.TryGetValue(folder.name, out f))
                folders[folder.name] = folder;
            else
                folders.Add(folder.Name, folder);
        }

        private void AddOrOverwrite(string itemName, T item)
        {
            T existingItem;

            if (items.TryGetValue(itemName, out existingItem))
                items[itemName] = item;
            else
                items.Add(itemName, item);
        }
    }
}
