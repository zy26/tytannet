using System;
using Pretorianie.Tytan.Parsers.Model;

namespace Pretorianie.Tytan.Parsers.Coff
{
    /// <summary>
    /// Standard Windows' NT header of each COFF executable.
    /// </summary>
    public class NtHeaderSection : BinarySection,
                                   IBinaryConverter<NtHeaderSection.ImageNtHeader>,
                                   IBinaryConverter<NtHeaderSection.ImageFileHeader>
    {
        #region Consts

        private const ushort NtSignature = 0x4550;
        /// <summary>
        /// Name of this section.
        /// </summary>
        public const string DefaultName = "NtHeader";

        #endregion

        #region Type Definitions

        internal struct ImageNtHeader
        {
            public uint Signature;
            public ImageFileHeader FileHeader;
        }

        internal struct ImageFileHeader
        {
            public ushort Machine;
            public ushort NumberOfSections;
            public uint TimeDateStamp;
            public uint PointerToSymbolTable;
            public uint NumberOfSymbols;
            public ushort SizeOfOptionalHeader;
            public ushort Characteristics;
        }

        #endregion

        #region Properties

        public MachineType MachineType { get; private set; }
        public ushort DataSectionCount { get; private set; }
        public DateTime CreationTime { get; private set; }
        public uint SymbolTableAddress { get; private set; }
        public uint SymbolCount { get; private set; }
        public uint SizeOfOptionalHeader { get; private set; }
        public Characteristics Characteristics { get; private set; }

        #endregion

        #region Implementation of IDataConverter<ImageNtHeader>

        /// <summary>
        /// Setup internal data based on given input read from native image.
        /// </summary>
        bool IBinaryConverter<ImageNtHeader>.Convert(ref ImageNtHeader s, uint startOffset, uint size)
        {
            if (s.Signature != NtSignature)
                return false;

            bool result = ((IBinaryConverter<ImageFileHeader>) this).Convert(ref s.FileHeader, startOffset + 4, size - 4);

            UpdateFileInfo(DefaultName, startOffset, size);
            UpdateVirtualInfo(startOffset, size);
            return result;
        }

        #endregion

        #region Implementation of IBinaryConverter<ImageFileHeader>

        /// <summary>
        /// Setup internal data based on given input read from native image.
        /// </summary>
        bool IBinaryConverter<ImageFileHeader>.Convert(ref ImageFileHeader s, uint startOffset, uint size)
        {
            MachineType = (MachineType)s.Machine;
            DataSectionCount = s.NumberOfSections;
            CreationTime = new DateTime(1970, 1, 1).AddSeconds(s.TimeDateStamp);
            SymbolTableAddress = s.PointerToSymbolTable;
            SizeOfOptionalHeader = s.SizeOfOptionalHeader;
            SymbolCount = s.NumberOfSymbols;
            Characteristics = (Characteristics)s.Characteristics;

            UpdateFileInfo("Object", startOffset, size);
            return true;
        }

        #endregion
    }
}