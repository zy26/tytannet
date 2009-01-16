using Pretorianie.Tytan.Parsers.Model;

namespace Pretorianie.Tytan.Parsers.Coff
{
    /// <summary>
    /// Standard MS-DOS 2.0 header of Windows' Portable Executable.
    /// </summary>
    public class DosHeaderSection : BinarySection, IBinaryConverter<DosHeaderSection.ImageDosHeader>
    {
        #region Consts

        private const ushort ExecutableSignature = 0x5A4D;
        private const ushort ObjectSignature = 0x014C;
        private const ushort Object64Signature = 0x0184;
        private const ushort DebugSignature = 0x4944;

        #endregion

        #region Type Definitions

        internal struct ImageDosHeader
        {
            public ushort e_magic;                      // Magic number
            public ushort e_cblp;                       // Bytes on last page of file
            public ushort e_cp;                         // Pages in file
            public ushort e_crlc;                       // Relocations
            public ushort e_cparhdr;                    // Size of header in paragraphs
            public ushort e_minalloc;                   // Minimum extra paragraphs needed
            public ushort e_maxalloc;                   // Maximum extra paragraphs needed
            public ushort e_ss;                         // Initial (relative) SS value
            public ushort e_sp;                         // Initial SP value
            public ushort e_csum;                       // Checksum
            public ushort e_ip;                         // Initial IP value
            public ushort e_cs;                         // Initial (relative) CS value
            public ushort e_lfarlc;                     // File address of relocation table
            public ushort e_ovno;                       // Overlay number
            public ushort e_res_0;                      // Reserved uints
            public ushort e_res_1;                      // Reserved uints
            public ushort e_res_2;                      // Reserved uints
            public ushort e_res_3;                      // Reserved uints
            public ushort e_oemid;                      // OEM identifier (for e_oeminfo)
            public ushort e_oeminfo;                    // OEM information; e_oemid specific
            public ushort e_res2_0;                     // Reserved uints
            public ushort e_res2_1;                     // Reserved uints
            public ushort e_res2_2;                     // Reserved uints
            public ushort e_res2_3;                     // Reserved uints
            public ushort e_res2_4;                     // Reserved uints
            public ushort e_res2_5;                     // Reserved uints
            public ushort e_res2_6;                     // Reserved uints
            public ushort e_res2_7;                     // Reserved uints
            public ushort e_res2_8;                     // Reserved uints
            public ushort e_res2_9;                     // Reserved uints
            public uint e_lfanew;                       // File address of new exe header
        }

        #endregion

        #region Properties

        public ushort Checksum { get; private set; }
        public ushort Type { get; private set; }
        public uint NtHeaderAddress { get; private set; }
        public ushort OemID { get; private set; }
        public ushort OemInfo { get; private set; }
        public ushort PagesCount { get; private set; }
        public ushort ReallocationCount { get; private set; }
        public uint ReallocationHeaderAddress { get; private set; }
        public uint MsDosStubProgram { get; private set; }
        private ushort StackPointer { get; set; }

        #endregion

        #region IBinaryConverter<ImageDosHeader> Members

        bool IBinaryConverter<ImageDosHeader>.Convert(ref ImageDosHeader s, uint startOffset, uint size)
        {
            Checksum = s.e_csum;
            Type = s.e_magic;
            NtHeaderAddress = s.e_lfanew;
            OemID = s.e_oemid;
            OemInfo = s.e_oeminfo;
            PagesCount = s.e_cp;
            StackPointer = s.e_sp;
            ReallocationCount = s.e_crlc;
            ReallocationHeaderAddress = s.e_lfarlc;
            MsDosStubProgram = startOffset + size;

            UpdateVirtualInfo(startOffset, size);
            if (IsExecutable)
                UpdateFileInfo(GetSignature(s.e_magic), startOffset, size);

            return true;
        }

        /// <summary>
        /// Get MS-DOS header signature - 'MZ'
        /// </summary>
        private string GetSignature(ushort m)
        {
            return "" + (char)(m & 0xFF) + (char)((m >> 8) & 0xFF);
        }

        /// <summary>
        /// Returns true when given section describes .EXE or .DLL file.
        /// </summary>
        public bool IsExecutable
        {
            get { return Type == ExecutableSignature; }
        }

        /// <summary>
        /// Returns true when given section describes .DBG file.
        /// </summary>
        public bool IsDebugBinary
        {
            get { return Type == DebugSignature; }
        }

        /// <summary>
        /// Returns true when given section describes .OBJ file.
        /// </summary>
        public bool IsObjectBinary
        {
            get
            {
                return (Type == ObjectSignature || Type == Object64Signature)
                            && StackPointer == 0;
            }
        }

        #endregion
    }
}