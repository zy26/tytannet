using System.Collections.Generic;
using Pretorianie.Tytan.Parsers.Model;

namespace Pretorianie.Tytan.Parsers.Coff
{
    /// <summary>
    /// Section describing exported functions of given COFF file.
    /// </summary>
    public class ExportFunctionSection : BinarySection, IBinaryConverter<ExportFunctionSection.ImageExportDirectory>
    {
        internal struct ImageExportDirectory
        {
            public uint Characteristics;
            public uint TimeDateStamp;
            public ushort MajorVersion;
            public ushort MinorVersion;
            public uint Name;
            public uint Base;
            public uint NumberOfFunctions;
            public uint NumberOfNames;
            public uint AddressOfFunctions;     // RVA from base of image
            public uint AddressOfNames;         // RVA from base of image
            public uint AddressOfNameOrdinals;  // RVA from base of image
        }

        public string ModuleName { get; private set; }
        protected uint ModuleNameAddress { get; private set; }
        protected uint NameAddress { get; private set; }
        protected uint FunctionAddress { get; private set; }
        protected uint OrdinalAddress { get; private set; }
        protected uint OrdinalBase { get; private set; }
        public uint Count { get; private set; }
        public IList<ExportFunctionDescription> Functions { get; private set; }


        #region IBinaryConverter<ImageExportDirectory> Members

        bool IBinaryConverter<ImageExportDirectory>.Convert(ref ImageExportDirectory s, uint startOffset, uint size)
        {
            NameAddress = s.AddressOfNames;
            FunctionAddress = s.AddressOfFunctions;
            OrdinalAddress = s.AddressOfNameOrdinals;
            OrdinalBase = s.Base;
            Count = s.NumberOfNames;
            ModuleNameAddress = s.Name;

            UpdateFileInfo("Export", startOffset, size);
            return true;
        }

        #endregion

        internal bool Read(ReaderWithOffsetArgs e)
        {
            const uint sizeOfUInt32 = 4;
            const uint sizeOfUInt16 = 2;
            string forwardedName;
            List<ExportFunctionDescription> r = new List<ExportFunctionDescription>();

            // update the pointers with offset:
            NameAddress -= e.Delta;
            FunctionAddress -= e.Delta;
            OrdinalAddress -= e.Delta;
            ModuleNameAddress -= e.Delta;

            // read the module name:
            ModuleName = e.Source.ReadStringAnsiAt(ModuleNameAddress);

            // read functions:
            for (uint i = 0; i < Count; i++)
            {
                uint entryPointRVA = e.Source.ReadUInt32At(FunctionAddress + sizeOfUInt32*i);
                uint ordinal = i + OrdinalBase;
                uint nameAddress;

                if (entryPointRVA != 0)
                {
                    // check if this function has an associated name exported:
                    for (uint j = 0; j < Count; j++)
                    {
                        if (e.Source.ReadUInt16At(OrdinalAddress + sizeOfUInt16 * j) == i)
                        {
                            nameAddress = e.Source.ReadUInt32At(NameAddress + sizeOfUInt32*j) - e.Delta;
                            forwardedName = ContainsVirtual(entryPointRVA) ? e.Source.ReadStringAnsiAt(entryPointRVA - e.Delta) : null;
                            r.Add(new ExportFunctionDescription(e.Source.ReadStringAnsiAt(nameAddress), forwardedName, ordinal,
                                                                entryPointRVA));
                        }
                    }
                }
            }

            Functions = r;
            return true;
        }
    }
}