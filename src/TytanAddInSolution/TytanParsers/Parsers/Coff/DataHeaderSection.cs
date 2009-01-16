using System.Text;
using Pretorianie.Tytan.Parsers.Model;

namespace Pretorianie.Tytan.Parsers.Coff
{
    public class DataHeaderSection : BinarySection, IBinaryConverter<DataHeaderSection.ImageSectionHeader>
    {
        #region Type Definitions

        internal struct ImageSectionHeader
        {
            public ulong Name;
            public uint PhysicalAddressOrVirtualSize;
            public uint VirtualAddress;
            public uint SizeOfRawData;
            public uint PointerToRawData;
            public uint PointerToRelocations;
            public uint PointerToLinenumbers;
            public ushort NumberOfRelocations;
            public ushort NumberOfLinenumbers;
            public uint Characteristics;
        }

        #endregion

        #region Properties

        public uint DataSize { get; private set; }
        public uint DataAddress { get; private set; }
        public uint RelocationAddress { get; private set; }
        public uint RelocationCount { get; private set; }
        public uint LineNumberAddress { get; private set; }
        public uint LineNumberCount { get; private set; }
        public DataSectionFlags Characteristics { get; private set; }

        #endregion

        #region Implementation of IBinaryConverter<ImageSectionHeader>

        /// <summary>
        /// Setup internal data based on given input read from native image.
        /// </summary>
        bool IBinaryConverter<ImageSectionHeader>.Convert(ref ImageSectionHeader s, uint startOffset, uint size)
        {
            DataSize = s.SizeOfRawData;
            DataAddress = s.PointerToRawData;
            RelocationAddress = s.PointerToRelocations;
            LineNumberAddress = s.PointerToLinenumbers;
            RelocationCount = s.NumberOfRelocations;
            LineNumberCount = s.NumberOfLinenumbers;
            Characteristics = (DataSectionFlags) s.Characteristics;

            UpdateFileInfo(GetName(s.Name), startOffset, size);
            UpdateVirtualInfo(s.VirtualAddress, s.PhysicalAddressOrVirtualSize);
            return true;
        }

        #endregion

        #region Auxiliary Functions

        /// <summary>
        /// Converts given 8-byte array into string.
        /// </summary>
        private static string GetName(ulong text)
        {
            char c;
            StringBuilder r = new StringBuilder(10);

            for (int i = 0; i < 8; i++)
            {
                c = (char) (text & 0xFF);
                if (c == '\0')
                    break;
                r.Append(c);
                text >>= 8;
            }

            return r.ToString();
        }

        #endregion
    }
}