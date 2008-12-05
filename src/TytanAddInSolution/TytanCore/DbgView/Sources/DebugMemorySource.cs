using System;
using System.Threading;
using Pretorianie.Tytan.Core.Data;
using Pretorianie.Tytan.Core.Mapping;
using Pretorianie.Tytan.Core.Helpers;

namespace Pretorianie.Tytan.Core.DbgView.Sources
{
    /// <summary>
    /// Class providing elementary access to shared debug memory and firing 
    /// </summary>
    public class DebugMemorySource : IDbgSource
    {
        private static EventWaitHandle eventBufferReady;
        private static EventWaitHandle eventDataReady;
        private static DebugSharedMemory sharedMemory;

        private static Thread threadProcessing;
        private static volatile bool isRunning;

        private const string BufferReadyName = @"Global\DBWIN_BUFFER_READY";
        private const string DataReadyName = @"Global\DBWIN_DATA_READY";
        private const string SharedMemoryName = @"Global\DBWIN_BUFFER";

        #region Implementation of IDbgSource

        /// <summary>
        /// Event fired each time new pack of raw data has been received.
        /// No synchronization and multi-line support guaranteed.
        /// </summary>
        public event DbgDataEventHandler DataReceived;

        /// <summary>
        /// Provides info if given source can be disabled at runtime by the user.
        /// </summary>
        public bool CanConfigureAtRuntime
        {
            get { return false; }
        }

        /// <summary>
        /// Reinitialize and start processing of messages.
        /// </summary>
        public void Start()
        {
            Close();
            eventBufferReady = SysEventHelper.CreateOrOpen(BufferReadyName, EventResetMode.AutoReset, false);
            eventDataReady = SysEventHelper.CreateOrOpen(DataReadyName, EventResetMode.AutoReset, false);
            sharedMemory = new DebugSharedMemory(SharedMemoryName);

            // check if opening handles failed:
            if (eventBufferReady == null || eventDataReady == null || sharedMemory.Address == IntPtr.Zero)
            {
                Close();
                return;
            }

            // create processing units:
            if (threadProcessing == null)
            {
                // main thread listening for messages:
                threadProcessing = new Thread(ThreadMonitor);
                threadProcessing.IsBackground = true;
                threadProcessing.Start();
            }
        }

        private void ThreadMonitor()
        {
            isRunning = true;
            eventBufferReady.Set();

            while (isRunning)
            {
                // wait for the debug data:
                if (eventDataReady.WaitOne())
                {
                    if (isRunning)
                    {
                        InternalReceive(sharedMemory.PID, sharedMemory.Message);
                        eventBufferReady.Set();
                    }
                    else
                        break;
                }
            }
        }

        private void InternalReceive(uint pid, string message)
        {
            // and notify all listeners that new message has arrived:
            if (DataReceived != null)
                DataReceived(pid, null, null, message);
        }

        /// <summary>
        /// Stops receiving of the data.
        /// </summary>
        public void Close()
        {
            if (threadProcessing != null)
            {
                isRunning = false;
                eventDataReady.Set();

                // wait for thread to finish:
                if (threadProcessing != null)
                    threadProcessing.Join(3000);

                threadProcessing = null;
            }

            if (eventBufferReady != null)
            {
                eventBufferReady.Close();
                eventBufferReady = null;
            }

            if (eventDataReady != null)
            {
                eventDataReady.Close();
                eventDataReady = null;
            }

            if (sharedMemory != null)
            {
                sharedMemory.Close();
                sharedMemory = null;
            }
        }

        #endregion
    }
}
