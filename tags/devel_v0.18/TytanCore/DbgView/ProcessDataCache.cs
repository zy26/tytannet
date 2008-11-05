using System.Collections.Generic;
using System.Diagnostics;

namespace Pretorianie.Tytan.Core.DbgView
{
    /// <summary>
    /// Class for managing the descriptions about running processes.
    /// </summary>
    public static class ProcessDataCache
    {
        /// <summary>
        /// Number of requests that will force to clear the caches.
        /// </summary>
        private const int MaxRefresh = 10;
        /// <summary>
        /// Name of the unspecified main module for process.
        /// </summary>
        private const string UnknownModuleFileName = "- - -";

        /// <summary>
        /// Default description for unknown process.
        /// </summary>
        private static readonly ProcessData UnknownProcess = new ProcessData(0, "unknown", UnknownModuleFileName);

        private static int request;
        private static readonly object syncLock = new object();
        private static readonly Dictionary<uint, ProcessData> infos = new Dictionary<uint, ProcessData>();
        private static ProcessData lastInfo = UnknownProcess;

        /// <summary>
        /// Get description for process with given PID.
        /// </summary>
        public static ProcessData GetByID(uint pid)
        {
            ProcessData result;

            lock (syncLock)
            {
                // check if last query wanted the same PID:
                if (pid == lastInfo.PID)
                {
                    result = lastInfo;
                }
                else
                {
                    // check internal collection:
                    if (infos.TryGetValue(pid, out result))
                    {
                        lastInfo = result;
                    }
                    else
                    {
                        // when all failed, ask the OS and add to caches:
                        try
                        {
                            Process process = Process.GetProcessById((int) pid);

                            if (process != null)
                            {
                                result = new ProcessData(pid, process.ProcessName,
                                                         (process.MainModule != null
                                                              ? process.MainModule.FileName
                                                              : UnknownModuleFileName));
                                lastInfo = result;
                                infos.Add(pid, result);
                            }
                            else
                                result = UnknownProcess;
                        }
                        catch
                        {
                            result = UnknownProcess;
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Request flushing caches.
        /// </summary>
        public static void RequestRefresh()
        {
            lock(syncLock)
            {
                request++;

                // this request should force refreshing data each few seconds:
                if(request >= MaxRefresh)
                {
                    request = 0;
                    infos.Clear();
                }
            }
        }
    }
}
