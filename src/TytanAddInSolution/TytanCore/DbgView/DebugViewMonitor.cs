using System;
using System.Collections.Generic;
using System.Threading;
using Pretorianie.Tytan.Core.DbgView.Sources;
using System.Diagnostics;

namespace Pretorianie.Tytan.Core.DbgView
{
    /// <summary>
    /// Class that captures the OutputDebugString text.
    /// </summary>
    public static class DebugViewMonitor
    {
        /// <summary>
        /// Definition of the callback function that will receive debug messages.
        /// </summary>
        public delegate void ReceiveHandler(IList<DebugViewData> items);

        /// <summary>
        /// Event fired when new text has been received via OutputDebugMessage call.
        /// </summary>
        public static event ReceiveHandler ReceivedMessage;

        #region Sync Events

        private static Queue<DebugViewData> storedItems;
        private static List<IDbgSource> dataSources;
        private static IList<IDbgSource> cachedDataSources;
        private static Timer refreshTimer;
        private static object syncItems;
        private static object syncSources;
        private static bool isRefreshing;
        private static bool isStarted;

        private const string TabReplace = "    ";

        #endregion

        /// <summary>
        /// Creates internal data structures to receive debug messages.
        /// </summary>
        public static bool Start()
        {
            // create processing units:
            if (storedItems == null)
            {
                storedItems = new Queue<DebugViewData>();
                dataSources = new List<IDbgSource>();
                syncItems = new object();
                syncSources = new object();
                refreshTimer = new Timer(InternalDataRefresh);
                isRefreshing = false;

                AddDefaultSources();
            }

            // and force all data providers to start:
            lock (syncSources)
            {
                isStarted = true;
                foreach (IDbgSource s in dataSources)
                {
                    try
                    {
                        s.Start();
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine(ex.Message);
                        Trace.WriteLine(ex.StackTrace);
                    }
                }
            }

            return true;
        }

        private static void InternalDataRefresh(object sender)
        {
            lock (syncItems)
            {
                DebugViewData[] items = storedItems.ToArray();
                storedItems.Clear();
                ProcessDataCache.RequestRefresh();

                // process elements:
                if (ReceivedMessage != null)
                    ReceivedMessage(items);

                isRefreshing = false;
            }
        }

        /// <summary>
        /// Releases internal data structures to receive debug messages.
        /// </summary>
        public static void Stop()
        {
            lock (syncSources)
            {
                isStarted = false;

                foreach (IDbgSource s in dataSources)
                {
                    try
                    {
                        s.Close();
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine(ex.Message);
                        Trace.WriteLine(ex.StackTrace);
                    }
                }

                // kill the timer:
                lock (syncItems)
                {
                    if (isRefreshing)
                    {
                        refreshTimer.Change(Timeout.Infinite, Timeout.Infinite);
                        if (storedItems != null && storedItems.Count < 0)
                            InternalDataRefresh(null);
                    }
                }
            }
        }

        /// <summary>
        /// Adds reference to new source of debug messages.
        /// </summary>
        public static bool AddSource(IDbgSource s, bool overrideExisting, bool autoStart)
        {
            lock (syncSources)
            {
                IDbgSource existing;
                bool canAdd = false;
                bool overriden = false;

                // check if there is already an item with the same name:
                if (s != null)
                {
                    existing = Find(s);

                    // if there is one with the same name:
                    if (existing != null)
                    {
                        // override?
                        if (overrideExisting)
                        {
                            RemoveSource(existing);
                            canAdd = true;
                            overriden = true;
                        }
                    }
                    else
                    {
                        canAdd = true;
                    }

                    // add new data-source element if possible:
                    if (canAdd)
                    {
                        s.DataReceived -= SourceDataReceived;
                        s.DataReceived += SourceDataReceived;
                        dataSources.Add(s);
                        cachedDataSources = null;

                        if (isStarted && autoStart)
                            s.Start();
                    }
                }

                return overriden;
            }
        }

        /// <summary>
        /// Adds the default set of sources.
        /// </summary>
        private static void AddDefaultSources()
        {
            // by default add listener to standard debug messages:
            AddSource(new DebugMemorySource(), false, false);
        }

        /// <summary>
        /// Checks if given debug data-source is already attached.
        /// </summary>
        public static bool ContainsSource(IDbgSource s)
        {
            lock (syncSources)
            {
                return Find(s) != null;
            }
        }

        /// <summary>
        /// Finds debug source with the same name.
        /// </summary>
        private static IDbgSource Find(IDbgSource s)
        {
            if (string.IsNullOrEmpty(s.Name))
                return null;

            foreach (IDbgSource x in dataSources)
                if (string.Compare(s.Name, x.Name, true) == 0)
                    return x;

            return null;
        }

        /// <summary>
        /// Removes given debug messages source.
        /// </summary>
        public static void RemoveSource(IDbgSource s)
        {
            lock (syncSources)
            {
                if (s != null && dataSources.Contains(s))
                {
                    s.Close();
                    s.DataReceived -= SourceDataReceived;
                    dataSources.Remove(s);
                    cachedDataSources = null;
                }
            }
        }

        /// <summary>
        /// Removes all the debug messages sources.
        /// </summary>
        public static void RemoveSources()
        {
            lock(syncSources)
            {
                List<IDbgSource> result = new List<IDbgSource>();

                foreach(IDbgSource s in dataSources)
                {
                    if (s.CanConfigureAtRuntime)
                    {
                        s.Close();
                        s.DataReceived -= SourceDataReceived;
                    }
                    else
                    {
                        result.Add(s);
                    }
                }

                // and now remove all unwanted sources from monitoring collection:
                dataSources = result;
                cachedDataSources = null;
            }
        }

        /// <summary>
        /// Gets the list of debug data sources.
        /// </summary>
        public static IList<IDbgSource> Sources
        {
            get
            {
                if (cachedDataSources == null)
                {
                    cachedDataSources = new List<IDbgSource>();

                    // add proper sources that can be manipulated at run-time:
                    lock (syncItems)
                    {
                        foreach (IDbgSource s in dataSources)
                            if (s.CanConfigureAtRuntime)
                                cachedDataSources.Add(s);
                    }
                }

                return cachedDataSources;
            }
        }

        /// <summary>
        /// Process received message.
        /// </summary>
        static void SourceDataReceived(IDbgSource source, uint pid, string message)
        {
            lock (syncItems)
            {
                DateTime creation = DateTime.Now;
                string[] msgs;

                if (string.IsNullOrEmpty(message))
                {
                    msgs = new string[] { string.Empty };
                }
                else
                {
                    message = message.Replace("\t", TabReplace);
                    msgs = message.Replace("\r\n", "\r").Replace("\n", "\r").Split('\r');
                }

                // check if this element has already the name:
                if (pid == 0 && (!string.IsNullOrEmpty(source.Name) || !string.IsNullOrEmpty(source.Module)))
                {
                    foreach (string m in msgs)
                        storedItems.Enqueue(new DebugViewData(0, source.Name, source.Module, creation, m.TrimEnd(null)));
                }
                else
                {
                    ProcessData dbgProcess = ProcessDataCache.GetByID(pid);
                    if (dbgProcess != null)
                        foreach (string m in msgs)
                            storedItems.Enqueue(new DebugViewData(pid, dbgProcess.Name,
                                                                  dbgProcess.MainModuleFileName, creation,
                                                                  m.TrimEnd(null)));
                }

                // avoid data flooding, by adding 1-sec delays
                // when sending to the receiver:
                if (!isRefreshing)
                {
                    isRefreshing = true;
                    refreshTimer.Change(1000, Timeout.Infinite);
                }
            }
        }
    }
}
