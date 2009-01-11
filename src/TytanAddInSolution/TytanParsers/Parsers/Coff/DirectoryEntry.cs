namespace Pretorianie.Tytan.Parsers.Coff
{
    /// <summary>
    /// Types of entries for sections directory of COFF format.
    /// </summary>
    public enum DirectoryEntry
    {
        /// <summary>
        /// Export Directory.
        /// </summary>
        Export = 0,
        /// <summary>
        /// Import Directory.
        /// </summary>
        Import = 1,
        /// <summary>
        /// Resource Directory.
        /// </summary>
        Resource = 2,
        /// <summary>
        /// Exception Directory.
        /// </summary>
        Exception = 3,
        /// <summary>
        /// Security Directory.
        /// </summary>
        Security = 4,
        /// <summary>
        /// Base Relocation Table Directory.
        /// </summary>
        BaseRelocationTable = 5,
        /// <summary>
        /// Debug Directory.
        /// </summary>
        Debug = 6,
        /// <summary>
        /// (X86 usage)
        /// </summary>
        Copyright = ArchitectureSpecific,
        /// <summary>
        /// Architecture Specific Data.
        /// </summary>
        ArchitectureSpecific = 7,
        /// <summary>
        /// RVA of GP.
        /// </summary>
        GlobalDataRVA = 8,
        /// <summary>
        /// TLS Directory.
        /// </summary>
        TLS = 9,
        /// <summary>
        /// Load Configuration Directory.
        /// </summary>
        LoadConfig = 10,
        /// <summary>
        /// Bound Import Directory in headers.
        /// </summary>
        BoundImport = 11,
        /// <summary>
        /// Import Address Table.
        /// </summary>
        IAT = 12,
        /// <summary>
        /// Delay Load Import Descriptors.
        /// </summary>
        DelayImport = 13,
        /// <summary>
        /// COM Runtime descriptor.
        /// </summary>
        ComDescriptor = 14
    }
}