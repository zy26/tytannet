namespace Pretorianie.Tytan.Parsers.Coff
{
    /// <summary>
    /// Description of relocation item.
    /// </summary>
    public class RelocationItem
    {
        /// <summary>
        /// Init constructor of RelocationItem.
        /// </summary>
        public RelocationItem(RelocationType type, uint offset)
        {
            Type = type;
            Offset = offset;
        }

        #region Properties

        public RelocationType Type
        { get; private set; }

        public uint Offset
        { get; private set; }

        #endregion
    }
}
