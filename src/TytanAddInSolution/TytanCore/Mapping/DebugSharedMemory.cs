using System;
using System.Runtime.InteropServices;

namespace Pretorianie.Tytan.Core.Mapping
{
    /// <summary>
    /// Shared memory to access system debug messages info.
    /// </summary>
    internal sealed class DebugSharedMemory : SharedMemory
    {
        private IntPtr message = IntPtr.Zero;

        /// <summary>
        /// Init constructor. Opens a shared memory with given name.
        /// </summary>
        public DebugSharedMemory(string name)
            : base(4096, name, 0, SectionTypes.SecNone, AccessTypes.ReadWrite)
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
            get
            {
                if (message == IntPtr.Zero)
                    return null;

                return Marshal.PtrToStringAnsi(message).Trim ();
            }
        }

        #region Protected Overrides

        /// <summary>
        /// Defines the pointer to message inside debugger-specific shared memory.
        /// </summary>
        protected override void OnCreateMapping(bool isCreated)
        {
            message = new IntPtr(Address.ToInt64() + Marshal.SizeOf(typeof(uint)));
        }

        #endregion
    }
}