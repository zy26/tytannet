using System;

namespace Pretorianie.Tytan.Parsers.Coff
{
    /// <summary>
    /// Windows' COFF loader specific flags.
    /// </summary>
    [Flags]
    public enum LoaderFlags : ushort
    {
        Unknown = 0
    }
}