using System.Collections.Generic;

namespace Pretorianie.Tytan.Parsers.Coff
{
    /// <summary>
    /// Description of relocation items.
    /// </summary>
    public class RelocationDescription
    {
        private IList<RelocationItem> items = new List<RelocationItem>();

        /// <summary>
        /// Init constructor of RelocationDescription.
        /// </summary>
        public RelocationDescription(uint virtualAddress, uint size)
        {
            VirtualAddress = virtualAddress;
            Size = size;
        }

        #region Properties

        public uint VirtualAddress
        { get; private set; }

        public uint Size
        { get; private set; }

        public IList<RelocationItem> Items
        {
            get { return items; }
        }

        /// <summary>
        /// Get the number of items.
        /// </summary>
        public int Count
        {
            get { return items.Count; }
        }

        #endregion

        public void Add(RelocationItem i)
        {
            items.Add(i);
        }
    }
}
