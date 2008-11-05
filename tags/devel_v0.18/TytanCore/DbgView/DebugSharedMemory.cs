using System;
using System.Runtime.InteropServices;

namespace Pretorianie.Tytan.Core.DbgView
{
    internal class DebugSharedMemory : SharedMemory
    {
        public DebugSharedMemory(string name)
            : base(4096, name, ProtectionTypes.PageReadWrite, SectionTypes.SecNone, AccessTypes.Read)
        {
        }

        /// <summary>
        /// Gets the PID of process that inserted the message.
        /// </summary>
        public uint PID
        {
            get { return (uint)Marshal.ReadInt32(Address); }
        }

        /// <summary>
        /// Gets the message.
        /// </summary>
        public string Message
        {
            get { return Marshal.PtrToStringAnsi(new IntPtr(Address.ToInt32() + 4)).Trim (); }
        }
    }
}
