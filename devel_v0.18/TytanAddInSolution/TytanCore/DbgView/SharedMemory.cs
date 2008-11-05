using System;
using System.Runtime.InteropServices;

namespace Pretorianie.Tytan.Core.DbgView
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
        public enum ProtectionTypes : uint
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
            Read = 0x4,
            /// <summary>
            /// Allow only write.
            /// </summary>
            Write = 0x2,
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

        [DllImport("kernel32", EntryPoint = "CreateFileMappingW", CharSet = CharSet.Unicode)]
        private static extern IntPtr CreateFileMapping(IntPtr hFile, uint lpSecurityAttributes, uint flProtect, uint dwMaximumSizeHigh, uint dwMaximumSizeLow, string lpName);

        [DllImport("kernel32", EntryPoint = "OpenFileMappingW", CharSet = CharSet.Unicode)]
        private static extern IntPtr OpenFileMapping(AccessTypes dwDesiredAccess, bool bInheritHandle, string lpName);

        [DllImport("kernel32")]
        private static extern bool UnmapViewOfFile(IntPtr lpBaseAddress);

        [DllImport("kernel32")]
        private static extern void CloseHandle(IntPtr handle);

        [DllImport("kernel32")]
        private static extern IntPtr MapViewOfFile(IntPtr hFileMappingObject, AccessTypes dwDesiredAccess, uint dwFileOffsetHigh, uint dwFileOffsetLow, uint dwNumberOfBytesToMap);

        private static readonly IntPtr InvalidHandleValue = new IntPtr(-1);

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
        public SharedMemory(uint size, string name, ProtectionTypes protection, SectionTypes section, AccessTypes access)
        {
            Create(size, name, protection, section, access);
        }

        /// <summary>
        /// Creates named memory mapping object that then can be opened by the same call from another process.
        /// </summary>
        public void Create(uint size, string name, ProtectionTypes protection, SectionTypes section, AccessTypes access)
        {
            hMappedFile = CreateFileMapping(InvalidHandleValue, 0, (uint)protection | (uint)section, 0, size, name);
            if (hMappedFile != IntPtr.Zero)
                lpMemoryAddress = MapViewOfFile(hMappedFile, access, 0, 0, size);
        }

        #region Properties

        /// <summary>
        /// Gets the handle to the mapped-memory object.
        /// </summary>
        public IntPtr Handle
        {
            get
            {
                return hMappedFile;
            }
        }

        /// <summary>
        /// Gets the native address of the mapped-memory.
        /// </summary>
        public IntPtr Address
        {
            get
            {
                return lpMemoryAddress;
            }
        }

        #endregion

        #region Open / Close

        /// <summary>
        /// Opens specified memory mapped object for reading only.
        /// </summary>
        public void Open(uint size, string name)
        {
            Open(size, name, 0, AccessTypes.Read);
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
        public void Open(uint size, string name, uint offset)
        {
            Open(size, name, offset, AccessTypes.Read);
        }

        /// <summary>
        /// Opens specified memory mapped object.
        /// </summary>
        public void Open(uint size, string name, uint offset, AccessTypes access)
        {
            hMappedFile = OpenFileMapping(access, false, name);
            if (hMappedFile != IntPtr.Zero)
                lpMemoryAddress = MapViewOfFile(hMappedFile, access, 0, offset, size);
        }

        /// <summary>
        /// Release resources.
        /// </summary>
        public void Close()
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

                // don't need to invoke Dispose() by GC
                GC.SuppressFinalize(this);
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
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
