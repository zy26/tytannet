using System;
using System.Diagnostics;
using Microsoft.Win32;
using Pretorianie.Tytan.Core.Data;

namespace Pretorianie.Tytan.Core.Helpers
{
    /// <summary>
    /// Class that manages storing and loading settings of specified tool from the persistent storage area.
    /// </summary>
    public static class PersistentStorageHelper
    {
        /// <summary>
        /// Opens the persistent storage key for writing/reading settings.
        /// </summary>
        private static RegistryKey OpenKey(string toolName, bool writable)
        {
            try
            {
                RegistryKey key;
                string path = @"Software\Pretorianie\VisualStudio.8\" + toolName;

                if (writable)
                {
                    // open or create key for writing:
                    key = Registry.CurrentUser.OpenSubKey(path, true);

                    if (key == null)
                        key = Registry.CurrentUser.CreateSubKey(path);
                }
                else
                {
                    // open an existing key:
                    key = Registry.CurrentUser.OpenSubKey(path, false);
                }

                return key;
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
                Trace.WriteLine(ex.StackTrace);
                return null;
            }
        }

        /// <summary>
        /// Closes the opened persistent storage key.
        /// </summary>
        private static void CloseKey(RegistryKey openedKey)
        {
            try
            {
                if (openedKey != null)
                    openedKey.Close();
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
                Trace.WriteLine(ex.StackTrace);
            }
        }

        /// <summary>
        /// Writes the particular data to the persistent storage area.
        /// </summary>
        public static bool Save(PersistentStorageData data)
        {
            try
            {
                RegistryKey key = OpenKey(data.Name, true);

                // store the data:
                if (key != null)
                {
                    foreach (string k in data.KeysStrings)
                        key.SetValue(k, data.GetString(k), RegistryValueKind.String);

                    foreach (string k in data.KeysMultiString)
                        key.SetValue(k, data.GetMultiString(k), RegistryValueKind.MultiString);

                    foreach (string k in data.KeysBytes)
                        key.SetValue(k, data.GetByte(k), RegistryValueKind.Binary);

                    foreach (string k in data.KeysUInts)
                        key.SetValue(k, data.GetUInt(k), RegistryValueKind.DWord);
                }

                CloseKey(key);
                return true;
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
                Trace.WriteLine(ex.StackTrace);
                return false;
            }
        }

        /// <summary>
        /// Loads the persistent storage data for particular tool.
        /// </summary>
        public static PersistentStorageData Load(string toolName)
        {
            try
            {
                PersistentStorageData data = new PersistentStorageData(toolName);
                RegistryKey key = OpenKey(toolName, false);
                string[] names;

                if (key == null)
                    return data;

                names = key.GetValueNames();
                foreach (string k in names)
                {
                    object d = key.GetValue(k);
                    switch (key.GetValueKind(k))
                    {
                        case RegistryValueKind.MultiString:
                            data.Add(k, d as string[]);
                            break;
                        case RegistryValueKind.ExpandString:
                        case RegistryValueKind.String:
                            data.Add(k, d as string);
                            break;
                        case RegistryValueKind.Binary:
                            data.Add(k, d as byte[]);
                            break;
                        case RegistryValueKind.DWord:
                        case RegistryValueKind.QWord:
                            data.Add(k, Convert.ToUInt32(d));
                            break;
                    }
                }

                CloseKey(key);
                return data;
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
                Trace.WriteLine(ex.StackTrace);
                return null;
            }
        }
    }
}
