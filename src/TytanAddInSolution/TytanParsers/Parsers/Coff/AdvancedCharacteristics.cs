using System;

namespace Pretorianie.Tytan.Parsers.Coff
{
    /// <summary>
    /// Windows' COFF format characteristics.
    /// </summary>
    [Flags]
    public enum AdvancedCharacteristics
    {
        /// <summary>
        /// Unspecified value.
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// Reserved.
        /// </summary>
        ProcessInit = 0x0001,
        /// <summary>
        /// Reserved.
        /// </summary>
        ProcessTerminate = 0x0002,
        /// <summary>
        /// Reserved.
        /// </summary>
        ThreadInit = 0x0004,
        /// <summary>
        /// Reserved.
        /// </summary>
        ThreadTerminate = 0x0008,
        /// <summary>
        /// DLL can be relocated at load time.
        /// </summary>
        DynamicBase = 0x0040,
        /// <summary>
        /// Code Integrity checks are enforced.
        /// </summary>
        ForceIntegrity = 0x0080,
        /// <summary>
        /// Image is NX compatible.
        /// </summary>
        CompatibleNX = 0x0100,
        /// <summary>
        /// Image understands isolation and doesn't want it.
        /// </summary>
        NoIsolation = 0x0200,
        /// <summary>
        /// Image does not use SEH.  No SE handler may reside in this image.
        /// </summary>
        NoSEH = 0x0400,
        /// <summary>
        /// Do not bind this image.
        /// </summary>
        NoBind = 0x0800,
        /// <summary>
        /// Reserved.
        /// </summary>
        Reserved1 = 0x1000,
        /// <summary>
        /// Driver uses WDM model.
        /// </summary>
        WdmDriver = 0x2000,
        /// <summary>
        /// Reserved.
        /// </summary>
        Reserved2 = 0x4000,
        /// <summary>
        /// Image is aware of Windows Terminal Services.
        /// </summary>
        TerminalServerAware = 0x8000
    }
}