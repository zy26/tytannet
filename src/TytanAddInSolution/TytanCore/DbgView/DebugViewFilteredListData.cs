using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Pretorianie.Tytan.Core.Helpers;

namespace Pretorianie.Tytan.Core.DbgView
{
    /// <summary>
    /// Class for managing list of DebugViewData items.
    /// </summary>
    public class DebugViewFilteredListData
    {
        /// <summary>
        /// DebugViewData Export styles.
        /// </summary>
        public enum ExportFormat
        {
            /// <summary>
            /// Export only time and message.
            /// </summary>
            TimeMessage = 0,
            /// <summary>
            /// Export time, creator PID and message.
            /// </summary>
            TimePidMessage = 1,
            /// <summary>
            /// Export time, creator name and message.
            /// </summary>
            TimeProcessMessage = 2,
            /// <summary>
            /// Export message only.
            /// </summary>
            MessageOnly = 3
        }

        private readonly IList<ListViewItem> storedItems = new List<ListViewItem>();
        private IList<ListViewItem> filteredItems = new List<ListViewItem>();
        private readonly object syncItems = new object();
        private static uint number = 1;

        private static readonly string[] outputFormats = new string[] {  "{0}|{5}",
                                                                "{1:dd-MM-yyyy HH:mm:ss}.{2}|0x{3:X}|{5}",
                                                                "{0}|{4}|{5}", "{5}" };

        #region Properties

        /// <summary>
        /// Gets the number of stored items.
        /// </summary>
        public int Count
        {
            get { return storedItems.Count; }
        }

        /// <summary>
        /// Gets the number of filtered items.
        /// </summary>
        public int FilteredCount
        {
            get { return filteredItems.Count; }
        }

        /// <summary>
        /// Gets the list of all stored items.
        /// </summary>
        public IList<ListViewItem> Items
        {
            get { return storedItems; }
        }

        /// <summary>
        /// Gets the list of filtered items.
        /// </summary>
        public IList<ListViewItem> Filtered
        {
            get { return filteredItems; }
        }

        #endregion

        #region Filtering

        /// <summary>
        /// Converts DebugViewData to ListViewItem.
        /// </summary>
        private static ListViewItem ToListViewItem(DebugViewData i)
        {
            ListViewItem d = new ListViewItem((number++).ToString());

            d.SubItems.Add(i.CreationTime);
            if ((int)i.PID < 0)
                d.SubItems.Add("[ -- ]");
            else
                d.SubItems.Add("0x" + i.PID.ToString("X"));
            d.SubItems.Add(i.ProcessName);
            d.SubItems.Add(i.Message);

            d.Tag = i;

            return d;
        }
        
        #endregion

        /// <summary>
        /// Stores new items and updates the filtered list as well.
        /// </summary>
        public void Add(IList<DebugViewData> items, int filterPID, StringHelper.IStringFilter filter)
        {
            lock (syncItems)
            {
                ListViewItem x;
                if (filterPID == -1)
                {
                    if (filter == null || filter.IsAlwaysMatch)
                    {
                        foreach (DebugViewData i in items)
                        {
                            x = ToListViewItem(i);
                            storedItems.Add(x);

                            // using special filter, may set the filteredItems
                            // to the same collection as storedItems, to avoid
                            // adding the same element twice, check if different:
                            if (storedItems != filteredItems)
                                filteredItems.Add(x);
                        }
                    }
                    else
                    {
                        foreach (DebugViewData i in items)
                        {
                            x = ToListViewItem(i);
                            storedItems.Add(x);

                            // using special filter, may set the filteredItems
                            // to the same collection as storedItems, to avoid
                            // adding the same element twice, check if different:
                            if (storedItems != filteredItems && filter.Match(i.Message))
                                filteredItems.Add(x);
                        }
                    }
                }
                else
                {
                    if (filter == null || filter.IsAlwaysMatch)
                    {
                        foreach (DebugViewData i in items)
                        {
                            if ((int)i.PID == filterPID)
                            {
                                x = ToListViewItem(i);
                                storedItems.Add(x);

                                // using special filter, may set the filteredItems
                                // to the same collection as storedItems, to avoid
                                // adding the same element twice, check if different:
                                if (storedItems != filteredItems)
                                    filteredItems.Add(x);
                            }
                        }
                    }
                    else
                    {
                        foreach (DebugViewData i in items)
                        {
                            if ((int)i.PID == filterPID)
                            {
                                x = ToListViewItem(i);
                                storedItems.Add(x);

                                // using special filter, may set the filteredItems
                                // to the same collection as storedItems, to avoid
                                // adding the same element twice, check if different:
                                if (storedItems != filteredItems && filter.Match(i.Message))
                                    filteredItems.Add(x);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Removes all the items from the internal collection.
        /// </summary>
        public void Clear()
        {
            lock (syncItems)
            {
                storedItems.Clear();
                filteredItems.Clear();
            }
        }

        /// <summary>
        /// Rebuilds the filtered items array based on given citerias.
        /// </summary>
        public void ApplyFilter(StringHelper.IStringFilter filter)
        {
            if (filter == null || filter.IsAlwaysMatch)
            {
                filteredItems = storedItems;
            }
            else
            {
                IList<ListViewItem> tmp = new List<ListViewItem>();

                lock (syncItems)
                {
                    foreach (ListViewItem i in storedItems)
                    {
                        DebugViewData d = i.Tag as DebugViewData;
                        if (d != null && filter.Match(d.Message))
                            tmp.Add(i);
                    }
                }

                filteredItems = tmp;
            }
        }

        /// <summary>
        /// Stores filtered items to a specified file.
        /// </summary>
        public string ExportAsTextFile(string fileName, ExportFormat mode)
        {
            if (filteredItems != null && filteredItems.Count > 0)
            {
                TextWriter output = null;

                try
                {
                    DebugViewData d;
                    output = File.CreateText(fileName);
                    string format = outputFormats[(int)mode];

                    lock (syncItems)
                    {
                        foreach (ListViewItem item in filteredItems)
                        {
                            d = item.Tag as DebugViewData;

                            if (d != null)
                                output.WriteLine(format, d.CreationTime, d.CreationDate, d.CreationDate.Millisecond,
                                                 d.PID, d.ProcessName, d.Message);
                        }
                    }

                    output.Flush();
                    output.Close();
                    return null;
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
                finally
                {
                    if (output != null)
                        output.Close();
                }
            }

            return "No data to export.";
        }

        /// <summary>
        /// Imports entries from the text file.
        /// </summary>
        public int ImportTextFile(string fileName, int filterPID, StringHelper.IStringFilter filter)
        {
            StreamReader input = null;
            IList<DebugViewData> items = new List<DebugViewData>();
            string line;
            DateTime creation = DateTime.Now;
            string processName = "-- disk --";
            int pid = -2;

            try
            {
                input = File.OpenText(fileName);

                // read each entry:
                while ((line = input.ReadLine()) != null)
                {
                    line = line.Trim();

                    // interpret as DebugViewData item:
                    if (!string.IsNullOrEmpty(line))
                        items.Add(new DebugViewData((uint) pid, processName, null, creation, line));
                }

                // add all items to the internal collections:
                Add(items, filterPID, filter);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
            finally
            {
                if (input != null)
                    input.Close();
            }

            return items.Count;
        }

        /// <summary>
        /// Returns index of given element in filtered list or the nearest one.
        /// </summary>
        public bool GetIndexFromFilter(ListViewItem item, out int filteredIndex)
        {
            if (item == null || filteredItems == null || filteredItems.Count == 0)
            {
                filteredIndex = -1;
                return false;
            }

            // find on the filtered list:
            filteredIndex = filteredItems.IndexOf(item);
            if (filteredIndex >= 0)
                return false;
            

            // find on the normal list and search for something that is nearest:
            int storedIndex = storedItems.IndexOf(item);

            for (int i = 1; i < storedItems.Count; i++)
            {
                int indexUp = storedIndex + i;
                int indexDown = storedIndex - i;

                if (indexUp < storedItems.Count
                    && filteredItems.Contains(storedItems[indexUp]))
                {
                    filteredIndex = filteredItems.IndexOf(storedItems[indexUp]);
                    return true;
                }

                if (indexDown >= 0
                    && filteredItems.Contains(storedItems[indexDown]))
                {
                    filteredIndex = filteredItems.IndexOf(storedItems[indexDown]);
                    return true;
                }
            }

            // has not been found at all:
            filteredIndex = -1;
            return true;
        }
    }
}
