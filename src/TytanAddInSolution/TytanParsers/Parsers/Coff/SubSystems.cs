namespace Pretorianie.Tytan.Parsers.Coff
{
    /// <summary>
    /// Subsystem supported by given COFF file.
    /// </summary>
    public enum SubSystems : ushort
    {
        /// <summary>
        /// Unknown subsystem.
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// Image doesn't require a subsystem.
        /// </summary>
        Native = 1,
        /// <summary>
        /// Image runs in the Windows GUI subsystem.
        /// </summary>
        WindowsGUI = 2,
        /// <summary>
        /// Image runs in the Windows character subsystem.
        /// </summary>
        WindowsCUI = 3,
        /// <summary>
        /// Image runs in the OS/2 character subsystem.
        /// </summary>
        OS2_CUI = 5,
        /// <summary>
        /// Image runs in the Posix character subsystem.
        /// </summary>
        POSIX_CUI = 7,
        /// <summary>
        /// Image is a native Win9x driver.
        /// </summary>
        NativeWindows = 8,
        /// <summary>
        /// Image runs in the Windows CE subsystem.
        /// </summary>
        WindowsCE_GUI = 9,
        EFI_Application = 10,
        EFI_BootServiceDriver = 11,
        EFI_RuntimeDriver = 12,
        EFI_ROM = 13,
        XBox = 14
    }
}