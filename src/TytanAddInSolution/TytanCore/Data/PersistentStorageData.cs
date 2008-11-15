using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Pretorianie.Tytan.Core.Data
{
    /// <summary>
    /// Dictionary of settings that are stored, received from persistent storage.
    /// </summary>
    public class PersistentStorageData
    {
        private readonly string internalName;
        private readonly Dictionary<string, string> valStrings;
        private readonly Dictionary<string, string[]> valMultiStrings;
        private readonly Dictionary<string, byte[]> valBytes;
        private readonly Dictionary<string, uint> valUInts;
        private readonly IList<string> namesToRemove;
        private bool isDirty; // checks if current configuration should be stored

        /// <summary>
        /// Init constructor of PersistentStorageData.
        /// </summary>
        public PersistentStorageData(string storageName)
        {
            internalName = storageName;
            valStrings = new Dictionary<string, string>();
            valMultiStrings = new Dictionary<string, string[]>();
            valBytes = new Dictionary<string, byte[]>();
            valUInts = new Dictionary<string, uint>();
            namesToRemove = new List<string>();
            isDirty = true;
        }

        #region Add / Get

        /// <summary>
        /// Adds or replaces the value.
        /// </summary>
        public void Add(string name, string value)
        {
            if (valBytes.ContainsKey(name))
                valBytes.Remove(name);
            if (valUInts.ContainsKey(name))
                valUInts.Remove(name);
            if (valMultiStrings.ContainsKey(name))
                valMultiStrings.Remove(name);
            if (namesToRemove.Contains(name))
                namesToRemove.Remove(name);

            if (valStrings.ContainsKey(name))
                valStrings[name] = value;
            else
                valStrings.Add(name, value);
        }

        /// <summary>
        /// Adds or replaces the value.
        /// </summary>
        public void Add(string name, byte[] value)
        {
            if (valStrings.ContainsKey(name))
                valStrings.Remove(name);
            if (valUInts.ContainsKey(name))
                valUInts.Remove(name);
            if (valMultiStrings.ContainsKey(name))
                valMultiStrings.Remove(name);
            if (namesToRemove.Contains(name))
                namesToRemove.Remove(name);

            if (valBytes.ContainsKey(name))
                valBytes[name] = value;
            else
                valBytes.Add(name, value);
        }

        /// <summary>
        /// Adds or replaces the value.
        /// </summary>
        public void Add(string name, uint value)
        {
            if (valStrings.ContainsKey(name))
                valStrings.Remove(name);
            if (valBytes.ContainsKey(name))
                valBytes.Remove(name);
            if (valMultiStrings.ContainsKey(name))
                valMultiStrings.Remove(name);
            if (namesToRemove.Contains(name))
                namesToRemove.Remove(name);

            if (valUInts.ContainsKey(name))
                valUInts[name] = value;
            else
                valUInts.Add(name, value);
        }

        /// <summary>
        /// Adds or replaces the value.
        /// </summary>
        public void Add(string name, string[] value)
        {
            if (valStrings.ContainsKey(name))
                valStrings.Remove(name);
            if (valBytes.ContainsKey(name))
                valBytes.Remove(name);
            if (valUInts.ContainsKey(name))
                valUInts.Remove(name);
            if (namesToRemove.Contains(name))
                namesToRemove.Remove(name);

            if (valMultiStrings.ContainsKey(name))
                valMultiStrings[name] = value;
            else
                valMultiStrings.Add(name, value);
        }

        /// <summary>
        /// Gets the value with specified name.
        /// </summary>
        public object Get(string name)
        {
            object result = GetString(name);

            if (result != null)
                return result;

            result = GetMultiString(name);
            if (result != null)
                return result;

            result=  GetByte(name);
            if (result != null)
                return result;

            return GetUInt(name);
        }

        /// <summary>
        /// Gets the <see cref="DateTime"/> value with specified name.
        /// </summary>
        public DateTime GetDateTime(string name)
        {
            string text = GetString(name);
            try
            {
                return DateTime.Parse(text);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(string.Format("Invalid date to parse from persistent storage: '{0}'.", text));
                Trace.WriteLine(ex.Message);
                return DateTime.MinValue;
            }
        }

        /// <summary>
        /// Gets the string value with specified name.
        /// </summary>
        public string GetString(string name)
        {
            return GetString(name, null);
        }

        /// <summary>
        /// Gets the string value with specified name.
        /// </summary>
        public string GetString(string name, string defaultValue)
        {
            string value;

            if (valStrings.TryGetValue(name, out value))
                return value;

            return defaultValue;
        }

        /// <summary>
        /// Gets the multi-string value with specified name.
        /// </summary>
        public string[] GetMultiString(string name)
        {
            return GetMultiString(name, null);
        }

        /// <summary>
        /// Gets the multi-string value with specified name.
        /// </summary>
        public string[] GetMultiString(string name, string[] defaultValue)
        {
            string[] value;

            if (valMultiStrings.TryGetValue(name, out value))
                return value;

            return defaultValue;
        }

        /// <summary>
        /// Gets the byte[] value with specified name.
        /// </summary>
        public byte[] GetByte(string name)
        {
            return GetByte(name, null);
        }

        /// <summary>
        /// Gets the byte[] value with specified name.
        /// </summary>
        public byte[] GetByte(string name, byte[] defaultValue)
        {
            byte[] value;

            if (valBytes.TryGetValue(name, out value))
                return value;

            return defaultValue;
        }

        /// <summary>
        /// Gets the <c>uint</c> value with specified name.
        /// </summary>
        public uint GetUInt(string name)
        {
            return GetUInt(name, 0);
        }

        /// <summary>
        /// Gets the <c>uint</c> value with specified name.
        /// </summary>
        public uint GetUInt(string name, uint defaultValue)
        {
            uint value;

            if (valUInts.TryGetValue(name, out value))
                return value;

            return defaultValue;
        }

        /// <summary>
        /// Checks if value with specified name exists inside any of the internal collections.
        /// </summary>
        public bool Contains(string name)
        {
            return valStrings.ContainsKey(name) || valMultiStrings.ContainsKey(name) || valBytes.ContainsKey(name) || valUInts.ContainsKey(name);
        }

        /// <summary>
        /// Removes element at given name.
        /// </summary>
        public bool Remove(string name)
        {
            if (PerformInternalRemove(name))
            {
                if (!namesToRemove.Contains(name))
                    namesToRemove.Add(name);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Removes given element from internal collections.
        /// </summary>
        private bool PerformInternalRemove(string name)
        {
            if (valStrings.ContainsKey(name))
            {
                valStrings.Remove(name);

                return true;
            }

            if (valMultiStrings.ContainsKey(name))
            {
                valMultiStrings.Remove(name);
                return true;
            }

            if (valBytes.ContainsKey(name))
            {
                valBytes.Remove(name);
                return true;
            }

            if (valUInts.ContainsKey(name))
            {
                valUInts.Remove(name);
                return true;
            }

            return false;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the number of items.
        /// </summary>
        public int Count
        {
            get { return valStrings.Count + valMultiStrings.Count + valBytes.Count + valUInts.Count; }
        }

        /// <summary>
        /// Gets the name of the tool that can manage the persistent storage data.
        /// </summary>
        public string Name
        {
            get
            {
                return internalName;
            }
        }

        /// <summary>
        /// Gets or sets an indication if current data has been modified.
        /// </summary>
        public bool IsDirty
        {
            get { return isDirty; }
            set { isDirty = value; }
        }

        /// <summary>
        /// Gets the stored string values.
        /// </summary>
        public ICollection<string> Strings
        {
            get
            {
                return valStrings.Values;
            }
        }

        /// <summary>
        /// Gets the stored multi-string values.
        /// </summary>
        public ICollection<string[]> MultiStrings
        {
            get { return valMultiStrings.Values; }
        }

        /// <summary>
        /// Gets the stored byte values.
        /// </summary>
        public ICollection<byte[]> Bytes
        {
            get
            {
                return valBytes.Values;
            }
        }

        /// <summary>
        /// Gets the stored <c>uint</c> values.
        /// </summary>
        public ICollection<uint> UInts
        {
            get
            {
                return valUInts.Values;
            }
        }

        /// <summary>
        /// Gets all the key names.
        /// </summary>
        public IList<string> Keys
        {
            get
            {
                ICollection<string> keyString = valStrings.Keys;
                ICollection<string> keyMultiStrings = valMultiStrings.Keys;
                ICollection<string> keyByte = valBytes.Keys;
                ICollection<string> keyUInt = valUInts.Keys;
                int count = keyString.Count + keyMultiStrings.Count + keyByte.Count + keyUInt.Count;
                string[] result = null;

                if (count > 0)
                {
                    result = new string[count];

                    keyString.CopyTo(result, 0);
                    keyMultiStrings.CopyTo(result, keyString.Count);
                    keyByte.CopyTo(result, keyString.Count + keyMultiStrings.Count);
                    keyUInt.CopyTo(result, keyString.Count + keyMultiStrings.Count + keyByte.Count);
                }

                return result;
            }
        }

        /// <summary>
        /// Gets the names of string values.
        /// </summary>
        public ICollection<string> KeysStrings
        {
            get
            {
                return valStrings.Keys;
            }
        }

        /// <summary>
        /// Gets the names of multi-string values.
        /// </summary>
        public ICollection<string> KeysMultiString
        {
            get
            {
                return valMultiStrings.Keys;
            }
        }

        /// <summary>
        /// Gets the names of byte[] values.
        /// </summary>
        public ICollection<string> KeysBytes
        {
            get
            {
                return valBytes.Keys;
            }
        }

        /// <summary>
        /// Gets the names of <c>uint</c> values.
        /// </summary>
        public ICollection<string> KeysUInts
        {
            get
            {
                return valUInts.Keys;
            }
        }

        /// <summary>
        /// Gets the names of elements that should be removed.
        /// </summary>
        public ICollection<string> Removed
        {
            get
            {
                return namesToRemove;
            }
        }

        #endregion
    }
}
