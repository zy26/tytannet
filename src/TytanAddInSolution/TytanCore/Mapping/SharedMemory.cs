using System;
using System.Runtime.InteropServices;

namespace Pretorianie.Tytan.Core.Mapping
{
    /// <summary>
    /// Shared memory object.
    /// </summary>
    public class SharedMemory : IDisposable
    {
        #region Enums

        /// <summary>
        /// Way of accessing the physical memory.
        /// </summary>
        [Flags]
        protected enum ProtectionTypes : uint
        {
            /// <summary>
            /// Only read the memory.
            /// </summary>
            PageReadOnly = 0x2,
            /// <summary>
            /// Only write to memory.
            /// </summary>
            PageReadWrite = 0x4,
            /// <summary>
            /// Read and write to memory.
            /// </summary>
            PageWriteCopy = 0x8
        }

        /// <summary>
        /// Way of physical memory management.
        /// </summary>
        [Flags]
        public enum SectionTypes : uint
        {
            /// <summary>
            /// Do nothing with the memory.
            /// </summary>
            SecNone = 0,
            /// <summary>
            /// Commit all requested physical memory immediatelly after create.
            /// </summary>
            SecCommit = 0x8000000,
            /// <summary>
            /// Memory is a view of a EXE/DLL file.
            /// </summary>
            SecImage = 0x1000000,
            /// <summary>
            /// Don't cache the memory reads/writes.
            /// </summary>
            SecNoCache = 0x10000000,
            /// <summary>
            /// Reserved.
            /// </summary>
            SecReserve = 0x4000000
        }

        /// <summary>
        /// Way of accessing the file if mapped.
        /// </summary>
        [Flags]
        public enum AccessTypes : uint
        {
            /// <summary>
            /// Copy physical memory pages per process when writes performed.
            /// </summary>
            Copy = 0x1,
            /// <summary>
            /// Allow only read.
            /// </summary>
            ReadOnly = 0x4,
            /// <summary>
            /// Allow read and write operations.
            /// </summary>
            ReadWrite = ReadOnly | 0x2,
            /// <summary>
            /// Allow full control.
            /// </summary>
            Full = 0xF001F
        }

        #endregion

        #region Imports

        [StructLayout(LayoutKind.Sequential)]
        private struct SECURITY_ATTRIBUTES
        {
            uint nLength;
            uint lpSecurityDescriptor;
            bool bInheritHandle;
        }

        [DllImport("kernel32", EntryPoint = "CreateFileMappingW", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern IntPtr CreateFileMapping(IntPtr hFile, uint lpSecurityAttributes, uint flProtect, uint dwMaximumSizeHigh, uint dwMaximumSizeLow, string lpName);

        [DllImport("kernel32", EntryPoint = "OpenFileMappingW", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern IntPtr OpenFileMapping(AccessTypes dwDesiredAccess, bool bInheritHandle, string lpName);

        [DllImport("kernel32", SetLastError = true)]
        private static extern bool UnmapViewOfFile(IntPtr lpBaseAddress);

        [DllImport("kernel32", SetLastError = true)]
        protected static extern void CloseHandle(IntPtr handle);

        [DllImport("kernel32", SetLastError = true)]
        private static extern IntPtr MapViewOfFile(IntPtr hFileMappingObject, AccessTypes dwDesiredAccess, uint dwFileOffsetHigh, uint dwFileOffsetLow, uint dwNumberOfBytesToMap);

        /// <summary>
        /// Invalid handle value for Win32 objects.
        /// </summary>
        protected static readonly IntPtr InvalidHandleValue = new IntPtr(-1);

        #endregion

        private IntPtr hMappedFile = IntPtr.Zero;
        private IntPtr lpMemoryAddress = IntPtr.Zero;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public SharedMemory()
        {
        }

        /// <summary>
        /// Init constructor.
        /// </summary>
        public SharedMemory(uint size, string name, ulong offset, SectionTypes section, AccessTypes access)
        {
            Create(size, name, offset, section, access);
        }

        /// <summary>
        /// Creates named memory mapping object that then can be opened by the same call from another process.
        /// </summary>
        public void Create(uint size, string name, ulong offset, SectionTypes section, AccessTypes access)
        {
            hMappedFile = Create(InvalidHandleValue, ref size, ref offset, name, section, access);

            // in case of error try to open an existing shared memory object:
            if (hMappedFile == IntPtr.Zero && Marshal.GetLastWin32Error() != 0)
                hMappedFile = OpenFileMapping(access, false, name);

            if (hMappedFile != IntPtr.Zero)
                lpMemoryAddress = MapViewOfFile(hMappedFile, access, (uint)(offset >> 32) & 0xFFFFFFFF,
                                                (uint)(offset & 0xFFFFFFFF), size);

            if (lpMemoryAddress != IntPtr.Zero)
                OnCreateMapping(true);
        }

        /// <summary>
        /// Creates named memory object with given properties.
        /// </summary>
        protected virtual IntPtr Create(IntPtr handle, ref uint size, ref ulong offset, string name, SectionTypes section, AccessTypes access)
        {
            ProtectionTypes protection;

            switch (access)
            {
                case AccessTypes.Copy:
                    protection = ProtectionTypes.PageWriteCopy;
                    break;
                case AccessTypes.ReadOnly:
                    protection = ProtectionTypes.PageReadOnly;
                    break;

                default:
                    protection = ProtectionTypes.PageReadWrite;
                    break;
            }

            return CreateFileMapping(handle, 0, (uint) protection | (uint) section, 0, size, name);
        }

        #region Properties

        /// <summary>
        /// Gets the handle to the mapped-memory object.
        /// </summary>
        public IntPtr Handle
        {
            get { return hMappedFile; }
        }

        /// <summary>
        /// Gets the native address of the mapped-memory.
        /// </summary>
        public IntPtr Address
        {
            get { return lpMemoryAddress; }
        }

        #endregion

        #region Open / Close

        /// <summary>
        /// Opens specified memory mapped object for reading only.
        /// </summary>
        public void Open(uint size, string name)
        {
            Open(size, name, 0, AccessTypes.ReadOnly);
        }

        /// <summary>
        /// Opens specified memory mapped object.
        /// </summary>
        public void Open(uint size, string name, AccessTypes access)
        {
            Open(size, name, 0, access);
        }

        /// <summary>
        /// Opens specified memory mapped object.
        /// </summary>
        public void Open(uint size, string name, ulong offset)
        {
            Open(size, name, offset, AccessTypes.ReadOnly);
        }

        /// <summary>
        /// Opens specified memory mapped object.
        /// </summary>
        public void Open(uint size, string name, ulong offset, AccessTypes access)
        {
            hMappedFile = OpenFileMapping(access, false, name);

            if (hMappedFile != IntPtr.Zero)
                lpMemoryAddress = MapViewOfFile(hMappedFile, access, (uint) (offset >> 32) & 0xFFFFFFFF,
                                                (uint) (offset & 0xFFFFFFFF), size);

            if (lpMemoryAddress != IntPtr.Zero)
                OnCreateMapping(false);
        }

        /// <summary>
        /// Release resources.
        /// </summary>
        public virtual void Close()
        {
            if (lpMemoryAddress != IntPtr.Zero)
            {
                UnmapViewOfFile(lpMemoryAddress);
                lpMemoryAddress = IntPtr.Zero;
            }

            if (hMappedFile != IntPtr.Zero)
            {
                CloseHandle(hMappedFile);
                hMappedFile = IntPtr.Zero;
            }
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Release internal resources.
        /// </summary>
        public void Dispose()
        {
            Close();
        }

        #endregion

        #region Virtual Members

        /// <summary>
        /// Method invoked when new mapping object is created or opened.
        /// </summary>
        protected virtual void OnCreateMapping(bool isCreated)
        {
        }

        #endregion
    }
}