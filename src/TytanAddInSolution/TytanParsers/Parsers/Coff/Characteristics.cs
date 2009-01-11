using System;

namespace Pretorianie.Tytan.Parsers.Coff
{
    /// <summary>
    /// Windows' COFF format characteristics.
    /// </summary>
    [Flags]
    public enum Characteristics : ushort
    {
        /// <summary>
        /// There are no relocations in this file.
        /// </summary>
        NoRelocations = 0x0001,
        /// <summary>
        /// File is an executable image (not an OBJ nor LIB).
        /// </summary>
        Executable = 0x0002,
        /// <summary>
        /// Line nunbers stripped from file.
        /// </summary>
        NoLineNumbers = 0x0004,
        /// <summary>
        /// Local symbols stripped from file.
        /// </summary>
        NoSymbols = 0x0008,
        /// <summary>
        /// Agressively trim working set.
        /// </summary>
        AggresivelyTrimWorkingSet = 0x0010,
        /// <summary>
        /// Application can handle >2GB addresses.
        /// </summary>
        LargeAddressAware = 0x0020,
        /// <summary>
        /// Bytes of machine word are reversed.
        /// </summary>
        ReservedBytesLo = 0x0080,
        /// <summary>
        /// 32-bit word machine.
        /// </summary>
        RunOn32bitMachine = 0x0100,
        /// <summary>
        /// Debugging info stripped from file in .DBG file.
        /// </summary>
        NoDebugInfo = 0x0200,
        /// <summary>
        /// If Image is on removable media, copy and run from the swap file.
        /// </summary>
        RemovableRunFromSwap = 0x0400,
        /// <summary>
        /// If Image is on Net, copy and run from the swap file.
        /// </summary>
        NetRunFromSwap = 0x0800,
        /// <summary>
        /// System file.
        /// </summary>
        SystemFile = 0x1000,
        /// <summary>
        /// File is a dynamic-link library (DLL), not an executable program.
        /// </summary>
        DynamicLibrary = 0x2000,
        /// <summary>
        /// File should only be run on an uniprocessor machine.
        /// </summary>
        UniprocessorSystemOnly = 0x4000,
        /// <summary>
        /// Bytes of machine word are reversed.
        /// </summary>
        ReservedBytesHi = 0x8000
    }
}