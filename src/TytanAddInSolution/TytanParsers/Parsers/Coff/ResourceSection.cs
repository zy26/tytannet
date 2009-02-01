using Pretorianie.Tytan.Parsers.Model;

namespace Pretorianie.Tytan.Parsers.Coff
{
    /// <summary>
    /// Section description containing info about all resources exposed by given COFF file.
    /// </summary>
    public class ResourceSection : BinarySection
    {
        /// <summary>
        /// Name of this section.
        /// </summary>
        public const string DefaultName = "Resources";

        #region Type Definitions

        internal struct ImageResourceDirectory
        {
            public uint Characteristics;
            public uint TimeDateStamp;
            public ushort MajorVersion;
            public ushort MinorVersion;
            public ushort NumberOfNamedEntries;
            public ushort NumberOfIdEntries;
        }

        internal struct ImageResourceDataEntry
        {
            public uint OffsetToData;
            public uint Size;
            public uint CodePage;
            public uint Reserved;
        }

        #endregion
    }
}
