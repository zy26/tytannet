namespace Pretorianie.Tytan.Parsers.Coff
{
    /// <summary>
    /// Windows' COFF file destination machine type characteristics.
    /// </summary>
    public enum MachineType : ushort
    {
        /// <summary>
        /// Unknown type of machine.
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// Intel i860
        /// </summary>
        Intel_i860 = 0x014d,
        /// <summary>
        /// Intel I386 (same ID used for 486 and 586)
        /// </summary>
        Intel_i386 = 0x014c,
        /// <summary>
        /// MIPS R3000, MIPS little-endian, 0x160 big-endian
        /// </summary>
        MipsR3000 = 0x0162,
        /// <summary>
        /// MIPS R4000, MIPS little-endian
        /// </summary>
        MipsR4000 = 0x0166,
        /// <summary>
        /// MIPS R10000, MIPS little-endian
        /// </summary>
        MipsR10000 = 0x0168,
        /// <summary>
        /// MIPS little-endian WCE v2
        /// </summary>
        MipsWinCeMipsV2 = 0x0169,
        /// <summary>
        /// DEC Alpha AXP
        /// </summary>
        DecAlphaAxp = 0x0183,
        /// <summary>
        /// Alpha_AXP
        /// </summary>
        AlphaAxp = 0x0184,
        /// <summary>
        /// Alpha64
        /// </summary>
        AlphaAxp64 = 0x0284,
        /// <summary>
        /// SH3 little-endian
        /// </summary>
        SH3 = 0x01a2,
        SH3Dsp = 0x01a3,
        /// <summary>
        /// SH3E little-endian
        /// </summary>
        SH3E = 0x01a4,
        /// <summary>
        /// SH4 little-endian
        /// </summary>
        SH4 = 0x01a6,
        /// <summary>
        /// SH5
        /// </summary>
        SH5 = 0x01a8,
        /// <summary>
        /// ARM Little-Endian
        /// </summary>
        Arm = 0x01c0,
        Thumb = 0x01c2,
        AM33 = 0x01d3,
        /// <summary>
        /// IBM PowerPC Little-Endian
        /// </summary>
        PowerPC = 0x01F0,
        /// <summary>
        /// IBM PowerPC
        /// </summary>
        PowerPCFP = 0x01f1,
        /// <summary>
        /// Intel x64
        /// </summary>
        IA64 = 0x0200,
        /// <summary>
        /// MIPS
        /// </summary>
        Mips16 = 0x0266,
        /// <summary>
        /// MIPS
        /// </summary>
        MipsFpu = 0x0366,
        /// <summary>
        /// MIPS
        /// </summary>
        MipsFpu16 = 0x0466,
        /// <summary>
        /// Infineon
        /// </summary>
        TriCore = 0x0520,
        CEF = 0x0CEF,
        /// <summary>
        /// EFI Byte Code
        /// </summary>
        EBC = 0x0EBC,
        /// <summary>
        /// AMD64 (K8)
        /// </summary>
        AMD64 = 0x8664,
        /// <summary>
        /// M32R little-endian
        /// </summary>
        M32R = 0x9041,
        CEE = 0xC0EE
    }
}