using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Pretorianie.Tytan.Core.Mapping
{
    /// <summary>
    /// Class providing access to specified file via shared memory object.
    /// </summary>
    public class FileSharedMemory : SharedMemory
    {
        private readonly string viewName;
        private readonly string fileName;
        private IntPtr fileHandle;

        #region Imports

        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern IntPtr CreateFile(
            string lpFileName, uint dwDesiredAccess, uint dwShareMode,
            uint lpSecurityAttributes, uint dwCreationDisposition,
            uint dwFlagsAndAttributes, uint hTemplateFile);

        private const uint GenericRead = 0x80000000;
        private const uint GenericWrite = 0x40000000;
        private const uint OpenAlways = 4;
        private const uint AttributeNormal = 0x80;

        #endregion

        /// <summary>
        /// Init constructor for read/write access to given file.
        /// </summary>
        public FileSharedMemory(string fileName, string name, uint size, ulong offset)
        {
            this.fileName = Path.GetFullPath(fileName);
            viewName = name;

            // and try to creaet file mapping:
            Create(size, name, offset, SectionTypes.SecNone, AccessTypes.ReadWrite);
        }

        /// <summary>
        /// Creates view mapping for given file.
        /// </summary>
        protected override IntPtr Create(IntPtr handle, ref uint size, ref ulong offset, string name, SectionTypes section, AccessTypes access)
        {
            if (!File.Exists(fileName))
                throw new FileNotFoundException("File does not exist", fileName);

            FileInfo info = new FileInfo(fileName);

            if (info.Length == 0)
                throw new ArgumentException(string.Format("File '{0}' has zero length", fileName), "name");
            if (size == 0)
                size = Convert.ToUInt32(info.Length);

            // determine the file access parameters:
            uint fileDesiredAccess = GenericRead;
            if (access == AccessTypes.Full || access == AccessTypes.Copy || access == AccessTypes.ReadWrite)
                fileDesiredAccess |= GenericWrite;

            // open a file:
            fileHandle = CreateFile(fileName, fileDesiredAccess, 0, 0, OpenAlways, AttributeNormal, 0);
            if (fileHandle == InvalidHandleValue)
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());

            IntPtr memView = base.Create(fileHandle, ref size, ref offset, name, section, access);

            // close the file:
            CloseHandle(fileHandle);

            return memView;
        }

        #region Properties

        /// <summary>
        /// Gets the name of the file for which the view is set.
        /// </summary>
        public string FileName
        {
            get { return fileName; }
        }

        /// <summary>
        /// Gets the name of the view.
        /// </summary>
        public string Name
        {
            get { return viewName; }
        }

        /// <summary>
        /// Reads given structure from the memory.
        /// </summary>
        public T Read<T>(ref IntPtr source) where T : struct
        {
            IntPtr s = source;
            Type t = typeof(T);

            // move the pointer:
            source = new IntPtr(source.ToInt64() + Marshal.SizeOf(t));

            // read the structure:
            return (T)Marshal.PtrToStructure(s, t);
        }

        /// <summary>
        /// Gets the specified address within a file.
        /// </summary>
        public IntPtr GetAddressAt(ulong offset)
        {
            return new IntPtr(Address.ToInt64() + (long)offset);
        }

        #endregion
    }
}