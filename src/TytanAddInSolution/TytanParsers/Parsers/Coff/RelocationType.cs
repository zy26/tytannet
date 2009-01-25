namespace Pretorianie.Tytan.Parsers.Coff
{
    /// <summary>
    /// Type of relocation item.
    /// </summary>
    public enum RelocationType : ushort
    {
        BasedAbsolute = 0,
        BasedHigh = 1,
        BasedLow = 2,
        BasedHighLow = 3,
        BasedHighAdjust = 4,
        BasedMipsJumpAddress = 5,
        BasedMipsJumpAddress16 = 9,
        BasedIA64IMM64 = 9,
        BasedDirectory64 = 10
    }
}
