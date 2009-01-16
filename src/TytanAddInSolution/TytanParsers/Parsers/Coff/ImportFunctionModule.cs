using System;
using System.Collections.Generic;
using Pretorianie.Tytan.Parsers.Model;

namespace Pretorianie.Tytan.Parsers.Coff
{
    /// <summary>
    /// Set of elements building one import section.
    /// </summary>
    public class ImportFunctionModule : IBinaryAppender<ImportFunctionModule.ImageImportDescriptor, ReaderWithOffsetArgs>
    {
        #region Type Definitions

        internal struct ImageImportDescriptor
        {
            public uint CharacteristicsOrOriginalFirstThunk;    // 0 for terminating null import descriptor; RVA to original unbound IAT (PIMAGE_THUNK_DATA)
            public uint TimeDateStamp;                          // 0 if not bound, -1 if bound, and real date\time stamp in IMAGE_DIRECTORY_ENTRY_BOUND_IMPORT (new BIND); O.W. date/time stamp of DLL bound to (Old BIND)
            public uint ForwarderChain;                         // -1 if no forwarders
            public uint Name;
            public uint FirstThunk;                             // RVA to IAT (if bound this IAT has actual addresses)
        }

        internal struct ImageImportData64
        {
            public ulong OrdinalOrAddressOfData;

            public bool IsOrdinal
            {
                get { return (OrdinalOrAddressOfData & 0x8000000000000000) != 0; }
            }

            public uint Ordinal
            {
                get { return (uint)(OrdinalOrAddressOfData & 0xFFFF); }
            }
        }

        internal struct ImageImportData32
        {
            public uint OrdinalOrAddressOfData;

            public bool IsOrdinal
            {
                get { return (OrdinalOrAddressOfData & 0x80000000) != 0; }
            }

            public uint Ordinal
            {
                get { return OrdinalOrAddressOfData & 0xFFFF; }
            }
        }

        internal struct ImageImportByName
        {
            public short Hint;
            public uint Name;
        }

        #endregion

        private IList<ImportFunctionDescription> functions = new List<ImportFunctionDescription>();
        private bool isBound;

        #region Properties

        public string Name { get; private set; }
        public DateTime BindDate { get; private set; }
        internal uint ForwarderChain { get; private set; }
        internal uint NextModuleAddress { get; private set; }
        internal uint FirstThunk { get; set; }

        public IList<ImportFunctionDescription> Functions
        {
            get { return functions; }
        }

        public int Count
        {
            get { return functions.Count; }
        }

        public bool IsBound
        {
            get { return isBound; }
        }

        #endregion

        #region IBinaryAppender<ImageImportDescriptor,ReaderWithOffsetArgs> Members

        bool IBinaryAppender<ImageImportDescriptor, ReaderWithOffsetArgs>.Attach(ref ImageImportDescriptor s, uint size, ReaderWithOffsetArgs arg)
        {
            // check if data not empty:
            if (s.TimeDateStamp == 0 && s.Name == 0)
                return false;

            // read proper data:
            Name = arg.Source.ReadStringAnsiAt(s.Name - arg.Delta);
            if (s.TimeDateStamp == uint.MaxValue)
            {
                BindDate = DateTime.Now;
                isBound = true;
            }
            else
            {
                BindDate = new DateTime(1970, 1, 1).AddSeconds(s.TimeDateStamp);
                isBound = s.TimeDateStamp != 0;
            }

            ForwarderChain = s.ForwarderChain;
            NextModuleAddress = s.CharacteristicsOrOriginalFirstThunk;
            if (NextModuleAddress == 0)
                NextModuleAddress = s.FirstThunk;
            FirstThunk = s.FirstThunk;

            return true;
        }

        #endregion
    }
}
