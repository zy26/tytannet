﻿using Pretorianie.Tytan.Parsers.Model;

namespace Pretorianie.Tytan.Parsers.Coff
{
    /// <summary>
    /// Description of custom data/code sections from Windows' COFF binary.
    /// </summary>
    public class DataSectionDescription : IBinaryConverter<DataSectionDescription.ImageDataDirectory>
    {
        /// <summary>
        /// Init constructor of DataSectionDescription.
        /// </summary>
        public DataSectionDescription(uint virtualAddress, uint size)
        {
            VirtualAddress = virtualAddress;
            Size = size;
        }

        #region Properties

        public uint VirtualAddress { get; private set; }
        public uint Size { get; private set; }
        public uint VirtualAddressEnd { get { return VirtualAddress + Size; } }

        #endregion

        #region Type Definitions

        internal struct ImageDataDirectory
        {
            public uint VirtualAddress;
            public uint Size;
        }

        #endregion

        #region IBinaryConverter<ImageDataDirectory> Members

        bool IBinaryConverter<ImageDataDirectory>.Convert(ref ImageDataDirectory s, uint startOffset, uint size)
        {
            VirtualAddress = s.VirtualAddress;
            Size = s.Size;
            return true;
        }

        #endregion
    }
}