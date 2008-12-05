using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Pretorianie.Tytan.Core.Mapping
{
    /// <summary>
    /// Class for reading and converting memory from native blocks.
    /// </summary>
    public sealed class UnmanagedDataReader
    {
        private readonly IntPtr source;
        private readonly uint size;
        private IntPtr reader;
        private uint sizeLeft;
        private uint lastRead;
        private IList<uint> lastReads;

        /// <summary>
        /// Init constructor.
        /// </summary>
        public UnmanagedDataReader(IntPtr source, uint size)
        {
            this.source = source;
            this.size = size;
            Reset();
        }

        /// <summary>
        /// Resets reading so the next try will start from the beginning.
        /// </summary>
        public void Reset()
        {
            reader = source;
            sizeLeft = size;
            lastRead = 0;
            lastReads = new List<uint>();
        }

        /// <summary>
        /// Reads data and place it into object.
        /// It will also keep track of reading, so it is possible later to undo specified operation.
        /// </summary>
        public bool Read<T>(ref T item) where T : struct
        {
            Type type = typeof(T);

            // check if there is enough data to read:
            int itemSize = Marshal.SizeOf(type);
            if (itemSize > sizeLeft)
                return false;

            // read the data:
            item = (T)Marshal.PtrToStructure(reader, type);

            // move the pointers and remember size of read:
            sizeLeft -= (uint)itemSize;
            lastRead = (uint)itemSize;
            reader = new IntPtr(reader.ToInt64() + itemSize);
            lastReads.Add(lastRead);

            return true;
        }

        /// <summary>
        /// Reads element with given type from given address without any implications.
        /// </summary>
        public bool ReadAt<T>(ref T item, uint offset) where T : struct
        {
            Type type = typeof (T);

            // check if there is enough data to read:
            int itemSize = Marshal.SizeOf(type);
            if (offset + itemSize > size)
                return false;

            item = (T) Marshal.PtrToStructure(new IntPtr(source.ToInt64() + offset), typeof (T));
            return true;
        }

        /// <summary>
        /// Reads string from given address without any implications.
        /// </summary>
        public string ReadStringAnsiAt(uint offset)
        {
            return ReadStringAnsiAt(offset, 0);
        }

        /// <summary>
        /// Reads string from given address without any implications.
        /// </summary>
        public string ReadStringAnsiAt(uint offset, int length)
        {
            if (offset + length > size)
                return null;

            // read the string:
            if (length == 0)
                return Marshal.PtrToStringAnsi(new IntPtr(source.ToInt64() + offset));

            return Marshal.PtrToStringAnsi(new IntPtr(source.ToInt64() + offset), length);
        }

        /// <summary>
        /// Reads string from given address without any implications.
        /// </summary>
        public string ReadStringUniAt(uint offset)
        {
            return ReadStringUniAt(offset, 0);
        }

        /// <summary>
        /// Reads string from given address without any implications.
        /// </summary>
        public string ReadStringUniAt(uint offset, int length)
        {
            if (offset + 2 * length > size)
                return null;

            // read the string:
            if (length == 0)
                return Marshal.PtrToStringUni(new IntPtr(source.ToInt64() + offset));

            return Marshal.PtrToStringUni(new IntPtr(source.ToInt64() + offset), length);
        }

        /// <summary>
        /// Reads UInt32 from given address without any implications.
        /// </summary>
        public uint ReadUInt32At(uint offset)
        {
            if (offset + 4 > size)
                return 0;

            return (uint) Marshal.ReadInt32(new IntPtr(source.ToInt64() + offset));
        }

        /// <summary>
        /// Reads UInt16 from given address without any implications.
        /// </summary>
        public ushort ReadUInt16At(uint offset)
        {
            if (offset + 2 > size)
                return 0;

            return (ushort)Marshal.ReadInt16(new IntPtr(source.ToInt64() + offset));
        }

        /// <summary>
        /// Reads Byte from given address without any implications.
        /// </summary>
        public byte ReadByteAt(uint offset)
        {
            if (offset + 1 > size)
                return 0;

            return Marshal.ReadByte(new IntPtr(source.ToInt64() + offset));
        }

        /// <summary>
        /// Reads the ANSI string from current cursor location.
        /// </summary>
        public bool ReadStringAnsi(out string text, int length)
        {
            // check if there any data left to read:
            if (length > 0 && sizeLeft > 0)
            {
                text = null;
                return false;
            }

            // read the string:
            if (length == 0)
                text = Marshal.PtrToStringAnsi(reader);
            else
                text = Marshal.PtrToStringAnsi(reader, (int)Math.Min((uint)length, sizeLeft));

            // move the pointers and remember size of read:
            uint itemSize = (text == null ? 0 : (uint) text.Length + 1);

            sizeLeft -= itemSize;
            lastRead = itemSize;
            reader = new IntPtr(reader.ToInt64() + itemSize);
            lastReads.Add(lastRead);
            return true;
        }

        /// <summary>
        /// Undoes last read operation.
        /// </summary>
        public bool UndoRead()
        {
            return UndoRead(1);
        }

        /// <summary>
        /// Undoes the given number of read operations.
        /// </summary>
        public bool UndoRead(int count)
        {
            long moveReader = 0;

            for (int i = 0; i < count && lastReads.Count > 0; i++)
            {
                lastRead = lastReads[lastReads.Count - 1];
                lastReads.RemoveAt(lastReads.Count - 1);
                moveReader += lastRead;
            }

            if (lastReads.Count > 0)
                lastRead = lastReads[lastReads.Count - 1];
            else lastRead = 0;

            // move the pointer to first element:
            reader = new IntPtr(reader.ToInt64() - moveReader);
            return moveReader != 0;
        }

        /// <summary>
        /// Moves the reading cursor to given offset.
        /// </summary>
        public uint Jump(uint offset)
        {
            if (offset > size)
                return uint.MaxValue;

            uint lastStartOffset = LastReadStartOffset;

            // jump to given location and reset all read-related variables:
            reader = new IntPtr(source.ToInt64() + offset);
            lastRead = 0;
            sizeLeft = size - offset;

            return lastStartOffset;
        }

        /// <summary>
        /// Jumps from current location back or forward with given offset.
        /// </summary>
        public uint JumpOffset(int offset)
        {
            IntPtr t = new IntPtr(reader.ToInt64() + offset);
            uint s;

            if (offset < 0)
            {
                // can't it be moved to new location smaller than the source:
                if (t.ToInt64() < source.ToInt64())
                    return uint.MaxValue;
                s = sizeLeft - (uint) -offset;
            }
            else
            {
                // can't it be moved to new location greater than end of the file:
                if (offset > sizeLeft)
                    return uint.MaxValue;
                s = sizeLeft + (uint) offset;
            }

            uint lastStartOffset = LastReadStartOffset;

            // jump to given location:
            reader = t;
            lastRead = 0;
            sizeLeft = s;

            return lastStartOffset;
        }

        #region Properties

        /// <summary>
        /// Gets the pointer to source memory.
        /// </summary>
        public IntPtr Source
        {
            get { return source; }
        }

        /// <summary>
        /// Gets the size of memory buffer.
        /// </summary>
        public uint Size
        {
            get { return size; }
        }

        /// <summary>
        /// Gets the number of bytes to read to the end of the buffer.
        /// </summary>
        public uint SizeLeft
        {
            get { return sizeLeft; }
        }

        /// <summary>
        /// Gets the number of read operations performed.
        /// </summary>
        public int PerformedReadsCount
        {
            get { return lastReads.Count; }
        }

        /// <summary>
        /// Gets the size of last read operation.
        /// </summary>
        public uint LastReadSize
        {
            get { return lastRead; }
        }

        /// <summary>
        /// Gets the offset from the beginning of the memory buffer,
        /// where the last read operation took place.
        /// </summary>
        public uint LastReadStartOffset
        {
            get { return size - sizeLeft - lastRead; }
        }

        /// <summary>
        /// Gets the current read location withing the buffer.
        /// </summary>
        public uint CurrentOffset
        {
            get { return size - sizeLeft; }
        }

        #endregion
    }
}
