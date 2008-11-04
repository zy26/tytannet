using System;
using System.Collections.Generic;
using System.Threading;
using Pretorianie.Tytan.Core.Helpers;

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

        private static EventWaitHandle eventBufferReady;
        private static EventWaitHandle eventDataReady;
        private static DebugSharedMemory sharedMemory;

        private static Queue<DebugViewData> storedItems;
        private static Thread threadProcessing;
        private static Timer refreshTimer;
        private static object syncItems;
        private static volatile bool isRunning;
        private static bool isRefreshing;


        private const string BufferReadyName = "DBWIN_BUFFER_READY";
        private const string DataReadyName = "DBWIN_DATA_READY";
        private const string SharedMemoryName = "DBWIN_BUFFER";
        private const string TabReplace = "    ";

        #endregion

        /// <summary>
        /// Creates internal data structures to receive debug messages.
        /// </summary>
        public static bool Start()
        {
            if (eventBufferReady == null)
                eventBufferReady = SysEventHelper.CreateOrOpen(BufferReadyName, EventResetMode.AutoReset, false);

            if (eventDataReady == null)
                eventDataReady = SysEventHelper.CreateOrOpen(DataReadyName, EventResetMode.AutoReset, false);

            sharedMemory = new DebugSharedMemory(SharedMemoryName);

            // check if opening handles failed:
            if (eventBufferReady == null || eventDataReady == null || sharedMemory.Handle == IntPtr.Zero)
            {
                Stop();
                return false;
            }

            // create processing units:
            if (threadProcessing == null)
            {
                storedItems = new Queue<DebugViewData>();
                syncItems = new object();
                refreshTimer = new Timer(InternalDataRefresh);
                isRefreshing = false;

                // main thread listening for messages:
                threadProcessing = new Thread(delegate()
                                                  {
                                                      isRunning = true;
                                                      while (isRunning)
                                                      {
                                                          // wait for the debug data:
                                                          if (WaitHandle.SignalAndWait (eventBufferReady, eventDataReady))
                                                          {
                                                              if (sharedMemory != null && sharedMemory.Address != IntPtr.Zero)
                                                                  InternalReceive(sharedMemory.PID, sharedMemory.Message);
                                                              else
                                                                  break;
                                                          }
                                                          else
                                                          {
                                                              break;
                                                          }
                                                      }
                                                  });
                threadProcessing.IsBackground = true;
                threadProcessing.Start();
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

        private static void InternalReceive(uint pid, string message)
        {
            lock (syncItems)
            {
                DateTime creation = DateTime.Now;
                string[] msgs;

                if (string.IsNullOrEmpty(message))
                {
                    msgs = new string[] {string.Empty};
                }
                else
                {
                    message = message.Replace("\t", TabReplace);
                    msgs = message.Replace("\r\n", "\r").Replace("\n", "\r").Split('\r');
                }

                ProcessData dbgProcess = ProcessDataCache.GetByID(pid);
                if (dbgProcess != null)
                    for (int i = 0; i < msgs.Length; i++)
                        storedItems.Enqueue(new DebugViewData(pid, dbgProcess.Name,
                                                              dbgProcess.MainModuleFileName, creation,
                                                              msgs[i].TrimEnd(null)));

                // avoid data flooding, by adding 1-sec delays
                // when sending to the receiver:
                if (!isRefreshing)
                {
                    isRefreshing = true;
                    refreshTimer.Change(1000, Timeout.Infinite);
                }
            }
        }

        /// <summary>
        /// Releases internal data structures to receive debug messages.
        /// </summary>
        public static void Stop()
        {
            if (threadProcessing != null)
            {
                isRunning = false;

                sharedMemory.Close();
                eventDataReady.Set();

                // wait for thread to finish:
                if (threadProcessing != null)
                    threadProcessing.Join(10 * 1000);
                threadProcessing = null;
            }

            if (eventBufferReady != null)
            {
                eventBufferReady.Close();
                eventBufferReady = null;
            }
         
            if (sharedMemory.Address != IntPtr.Zero)
            {
                sharedMemory.Close();
                sharedMemory = null;
            }
        }
    }
}
