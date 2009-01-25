using System.Collections.Generic;
using Pretorianie.Tytan.Parsers.Model;

namespace Pretorianie.Tytan.Parsers.Coff
{
    /// <summary>
    /// Section describing relocation section of Windows' COFF file.
    /// </summary>
    public class RelocationSection : BinarySection
    {
        /// <summary>
        /// Name of this section.
        /// </summary>
        public const string DefaultName = "Relocation";

        private IList<RelocationDescription> items = new List<RelocationDescription>();

        #region Type Definitions

        internal struct ImageBaseRelocation
        {
            public uint VirtualAddress;
            public uint SizeOfBlock;
        }

        #endregion

        #region Properties

        public IList<RelocationDescription> Items
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

        public void Add(RelocationDescription i)
        {
            items.Add(i);
        }
    }
}
