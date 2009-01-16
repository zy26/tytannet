using System;
using System.Runtime.InteropServices;
using Pretorianie.Tytan.Parsers.Model;

namespace Pretorianie.Tytan.Parsers.Coff
{
    /// <summary>
    /// Optional Windows' COFF file executable file.
    /// </summary>
    public class NtOptionalHeaderSection : BinarySection,
                                           IBinaryConverter<NtOptionalHeaderSection.ImageOptionalHeader32>,
                                           IBinaryConverter<NtOptionalHeaderSection.ImageOptionalHeader64>,
                                           IBinaryConverter<NtOptionalHeaderSection.ImageOptionalHeaderROM>
    {
        #region Consts

        private const uint DefaultDataDirectoryCount = 16;
        /// <summary>
        /// Name of this section.
        /// </summary>
        public const string DefaultName = "NtOptionalHeader";

        /// <summary>
        /// Magic number for PE optional header.
        /// </summary>
        internal const ushort OptionalHeader32Magic = 0x10b;
        /// <summary>
        /// Magic number for PE+ optional header.
        /// </summary>
        internal const ushort OptionalHeader64Magic = 0x20;
        /// <summary>
        /// Magic number for ROM files optional header.
        /// </summary>
        internal const ushort OptionalRomHeaderMagic = 0x107;

        #endregion

        #region Properties

        public uint DataDirectoryCount { get; private set; }
        public uint EntryPointAddress { get; private set; }
        public uint Checksum { get; private set; }
        public ulong ImageBaseAddress { get; private set; }
        public uint SectionAlignment { get; private set; }
        public uint RvaCount { get; private set; }
        public uint CodeBaseAddress { get; private set; }
        public uint DataBaseAddress { get; private set; }

        public Version LinkerVersion { get; private set; }
        public Version ImageVersion { get; private set; }
        public Version SystemVersion { get; private set; }
        public Version SubSystemVersion { get; private set; }
        public Version Win32Version { get; private set; }

        public uint CodeSize { get; private set; }
        public uint ImageSize { get; private set; }
        public uint HeaderSize { get; private set; }
        public ulong HeapCommitSize { get; private set; }
        public ulong HeapReserveSize { get; private set; }
        public ulong StackCommitSize { get; private set; }
        public ulong StackReserveSize { get; private set; }
        public uint InitializedDataSize { get; private set; }
        public uint UninitializedDataSize { get; private set; }

        public LoaderFlags LoaderFlags { get; private set; }
        public AdvancedCharacteristics Characteristics { get; private set; }
        public SubSystems SubSystem { get; private set; }

        #endregion

        #region Type Definitions

        /// <summary>
        /// Optional header format.
        /// </summary>
        internal struct ImageOptionalHeader32
        {
            //
            // Standard fields.
            //

            public ushort Magic;
            public byte MajorLinkerVersion;
            public byte MinorLinkerVersion;
            public uint SizeOfCode;
            public uint SizeOfInitializedData;
            public uint SizeOfUninitializedData;
            public uint AddressOfEntryPoint;
            public uint BaseOfCode;
            public uint BaseOfData;

            //
            // NT additional fields.
            //

            public uint ImageBase;
            public uint SectionAlignment;
            public uint FileAlignment;
            public ushort MajorOperatingSystemVersion;
            public ushort MinorOperatingSystemVersion;
            public ushort MajorImageVersion;
            public ushort MinorImageVersion;
            public ushort MajorSubsystemVersion;
            public ushort MinorSubsystemVersion;
            public uint Win32VersionValue;
            public uint SizeOfImage;
            public uint SizeOfHeaders;
            public uint CheckSum;
            public ushort Subsystem;
            public ushort DllCharacteristics;
            public uint SizeOfStackReserve;
            public uint SizeOfStackCommit;
            public uint SizeOfHeapReserve;
            public uint SizeOfHeapCommit;
            public uint LoaderFlags;
            public uint NumberOfRvaAndSizes;
            //fixed ImageDataDirectory DataDirectory[IMAGE_NUMBEROF_DIRECTORY_ENTRIES];
        }

        internal struct ImageOptionalHeaderROM
        {
            public ushort Magic;
            public byte MajorLinkerVersion;
            public byte MinorLinkerVersion;
            public uint SizeOfCode;
            public uint SizeOfInitializedData;
            public uint SizeOfUninitializedData;
            public uint AddressOfEntryPoint;
            public uint BaseOfCode;
            public uint BaseOfData;
            public uint BaseOfBss;
            public uint GprMask;
            public uint CprMask_0;
            public uint CprMask_1;
            public uint CprMask_2;
            public uint CprMask_3;
            public uint GpValue;
        }

        internal struct ImageOptionalHeader64
        {
            public ushort Magic;
            public byte MajorLinkerVersion;
            public byte MinorLinkerVersion;
            public uint SizeOfCode;
            public uint SizeOfInitializedData;
            public uint SizeOfUninitializedData;
            public uint AddressOfEntryPoint;
            public uint BaseOfCode;
            public ulong ImageBase;
            public uint SectionAlignment;
            public uint FileAlignment;
            public ushort MajorOperatingSystemVersion;
            public ushort MinorOperatingSystemVersion;
            public ushort MajorImageVersion;
            public ushort MinorImageVersion;
            public ushort MajorSubsystemVersion;
            public ushort MinorSubsystemVersion;
            public uint Win32VersionValue;
            public uint SizeOfImage;
            public uint SizeOfHeaders;
            public uint CheckSum;
            public ushort Subsystem;
            public ushort DllCharacteristics;
            public ulong SizeOfStackReserve;
            public ulong SizeOfStackCommit;
            public ulong SizeOfHeapReserve;
            public ulong SizeOfHeapCommit;
            public uint LoaderFlags;
            public uint NumberOfRvaAndSizes;
            //fixed ImageDataDirectory DataDirectory[IMAGE_NUMBEROF_DIRECTORY_ENTRIES];
        }

        #endregion

        #region IBinaryConverter<ImageOptionalHeader32> Members

        bool IBinaryConverter<ImageOptionalHeader32>.Convert(ref ImageOptionalHeader32 s, uint startOffset, uint size)
        {
            LinkerVersion = new Version(s.MajorLinkerVersion, s.MinorLinkerVersion);
            ImageVersion = new Version(s.MajorImageVersion, s.MinorImageVersion);
            SystemVersion = new Version(s.MajorOperatingSystemVersion, s.MinorOperatingSystemVersion);
            SubSystemVersion = new Version(s.MajorSubsystemVersion, s.MinorSubsystemVersion);
            Win32Version = new Version((int)s.Win32VersionValue, 0);

            DataDirectoryCount = DefaultDataDirectoryCount;
            EntryPointAddress = s.AddressOfEntryPoint;
            Checksum = s.CheckSum;
            ImageBaseAddress = s.ImageBase;
            SectionAlignment = s.SectionAlignment;
            RvaCount = s.NumberOfRvaAndSizes;
            CodeBaseAddress = s.BaseOfCode;
            DataBaseAddress = s.BaseOfData;

            CodeSize = s.SizeOfCode;
            ImageSize = s.SizeOfImage;
            HeaderSize = s.SizeOfHeaders;
            HeapCommitSize = s.SizeOfHeapCommit;
            HeapReserveSize = s.SizeOfHeapReserve;
            StackCommitSize = s.SizeOfStackCommit;
            StackReserveSize = s.SizeOfStackReserve;
            InitializedDataSize = s.SizeOfInitializedData;
            UninitializedDataSize = s.SizeOfUninitializedData;
            LoaderFlags = (LoaderFlags) s.LoaderFlags;
            Characteristics = (AdvancedCharacteristics) s.DllCharacteristics;
            SubSystem = (SubSystems) s.Subsystem;

            UpdateFileInfo(DefaultName, startOffset, size + DefaultDataDirectoryCount * (uint)Marshal.SizeOf(typeof(DataSectionDescription.ImageDataDirectory)));
            UpdateVirtualInfo(FileOffset, FileSize);
            return true;
        }

        #endregion

        #region IBinaryConverter<ImageOptionalHeaderROM> Members

        bool IBinaryConverter<ImageOptionalHeaderROM>.Convert(ref ImageOptionalHeaderROM s, uint startOffset, uint size)
        {
            LinkerVersion = new Version(s.MajorLinkerVersion, s.MinorLinkerVersion);
            ImageVersion = new Version(0, 0);
            SystemVersion = ImageVersion;
            SubSystemVersion = ImageVersion;
            Win32Version = ImageVersion;

            DataDirectoryCount = 0;
            EntryPointAddress = s.AddressOfEntryPoint;
            Checksum = 0;
            ImageBaseAddress = s.BaseOfBss;
            SectionAlignment = 0;
            RvaCount = 0;
            CodeBaseAddress = s.BaseOfCode;
            DataBaseAddress = s.BaseOfData;

            CodeSize = s.SizeOfCode;
            ImageSize = 0;
            HeaderSize = 0;
            HeapCommitSize = 0;
            HeapReserveSize = 0;
            StackCommitSize = 0;
            StackReserveSize = 0;
            InitializedDataSize = s.SizeOfInitializedData;
            UninitializedDataSize = s.SizeOfUninitializedData;
            LoaderFlags = LoaderFlags.Unknown;
            Characteristics = AdvancedCharacteristics.Unknown;
            SubSystem = SubSystems.WindowsCE_GUI;

            UpdateFileInfo(DefaultName, startOffset, size);
            UpdateVirtualInfo(FileOffset, FileSize);
            return false;
        }

        #endregion

        #region IBinaryConverter<ImageOptionalHeader64> Members

        bool IBinaryConverter<ImageOptionalHeader64>.Convert(ref ImageOptionalHeader64 s, uint startOffset, uint size)
        {
            LinkerVersion = new Version(s.MajorLinkerVersion, s.MinorLinkerVersion);
            ImageVersion = new Version(s.MajorImageVersion, s.MinorImageVersion);
            SystemVersion = new Version(s.MajorOperatingSystemVersion, s.MinorOperatingSystemVersion);
            SubSystemVersion = new Version(s.MajorSubsystemVersion, s.MinorSubsystemVersion);
            Win32Version = new Version((int)s.Win32VersionValue, 0);

            DataDirectoryCount = DefaultDataDirectoryCount;
            EntryPointAddress = s.AddressOfEntryPoint;
            Checksum = s.CheckSum;
            ImageBaseAddress = s.ImageBase;
            SectionAlignment = s.SectionAlignment;
            RvaCount = s.NumberOfRvaAndSizes;
            CodeBaseAddress = s.BaseOfCode;
            DataBaseAddress = 0;

            CodeSize = s.SizeOfCode;
            ImageSize = s.SizeOfImage;
            HeaderSize = s.SizeOfHeaders;
            HeapCommitSize = s.SizeOfHeapCommit;
            HeapReserveSize = s.SizeOfHeapReserve;
            StackCommitSize = s.SizeOfStackCommit;
            StackReserveSize = s.SizeOfStackReserve;
            InitializedDataSize = s.SizeOfInitializedData;
            UninitializedDataSize = s.SizeOfUninitializedData;
            LoaderFlags = (LoaderFlags)s.LoaderFlags;
            Characteristics = (AdvancedCharacteristics)s.DllCharacteristics;
            SubSystem = (SubSystems)s.Subsystem;

            UpdateFileInfo(DefaultName, startOffset, size + DefaultDataDirectoryCount * (uint)Marshal.SizeOf(typeof(DataSectionDescription.ImageDataDirectory)));
            UpdateVirtualInfo(FileOffset, FileSize);
            return false;
        }

        #endregion
    }
}