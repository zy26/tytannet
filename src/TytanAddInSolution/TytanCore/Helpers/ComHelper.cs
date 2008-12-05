using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Pretorianie.Tytan.Core.Helpers
{
    /// <summary>
    /// Helper class that returns existing COM ports.
    /// </summary>
    public static class ComHelper
    {
        [DllImport("kernel32")]
        private static extern int QueryDosDeviceW(string lpDeviceName, IntPtr lpTargetPath, uint ucchMax);

        /// <summary>
        /// Gets the names of existing COM ports that can be opened by the application.
        /// </summary>
        public static string[] GetNames()
        {
            List<string> result = new List<string>();
            int cb = 0xFFFF;
            IntPtr lpTargetPath = Marshal.AllocHGlobal(cb);

            try
            {
                cb = QueryDosDeviceW(null, lpTargetPath, (uint) cb);
                if (cb > 0)
                {
                    string[] strArray = Marshal.PtrToStringUni(lpTargetPath, cb).Split('\0');
                    for (int i = 0; i < strArray.Length; i++)
                    {
                        if (strArray[i].StartsWith("COM"))
                        {
                            result.Add(strArray[i]);
                        }
                    }
                }
            }
            catch
            {
            }
            finally
            {
                Marshal.FreeHGlobal(lpTargetPath);
            }
            return result.ToArray();
        }
    }
}