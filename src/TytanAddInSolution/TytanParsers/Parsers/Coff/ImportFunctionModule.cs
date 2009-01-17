using System;
using System.Collections.Generic;
using Pretorianie.Tytan.Parsers.Model;

namespace Pretorianie.Tytan.Parsers.Coff
{
    /// <summary>
    /// Set of elements building one import section.
    /// </summary>
    public class ImportFunctionModule : IBinaryAppender<ImportFunctionModule.ImageImportDescriptor, ReaderWithOffsetArgs>,
                                        IBinaryAppender<ImportFunctionModule.ImageBoundImportForwarderRef, ReaderWithOffsetArgs>
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

        internal struct ImageBoundImportDescription
        {
            public uint TimeDateStamp;
            public ushort OffsetModuleName;
            public ushort NumberOfModuleForwarderRefs;
        }

        internal struct ImageBoundImportForwarderRef
        {
            public uint TimeDateStamp;
            public ushort OffsetModuleName;
            public ushort Reserved;
        }

        #endregion

        private IList<ImportFunctionDescription> functions = new List<ImportFunctionDescription>();
        private IList<ImportBoundForwarderDescription> forwarders = new List<ImportBoundForwarderDescription>();
        private bool isBinded;

        #region Properties

        public string Name { get; private set; }
        public DateTime BindDate { get; private set; }
        public DateTime BoundDate { get; internal set; }
        internal uint ForwarderChain { get; private set; }
        internal uint NextModuleAddress { get; private set; }
        internal uint FirstThunk { get; set; }

        public IList<ImportFunctionDescription> Functions
        {
            get { return functions; }
        }

        public IList<ImportBoundForwarderDescription> Forwarders
        {
            get { return forwarders; }
        }

        public int Count
        {
            get { return functions.Count; }
        }

        public bool IsBinded
        {
            get { return isBinded; }
        }

        #endregion

        /// <summary>
        /// Adds new function to the collection.
        /// </summary>
        public void Add(ImportFunctionDescription f)
        {
            functions.Add(f);
        }

        /// <summary>
        /// Adds new forwarder description to the collection.
        /// </summary>
        public void Add(ImportBoundForwarderDescription f)
        {
            forwarders.Add(f);
        }

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
                isBinded = true;
            }
            else
            {
                BindDate = new DateTime(1970, 1, 1).AddSeconds(s.TimeDateStamp);
                isBinded = s.TimeDateStamp != 0;
            }

            ForwarderChain = s.ForwarderChain;
            NextModuleAddress = s.CharacteristicsOrOriginalFirstThunk;
            if (NextModuleAddress == 0)
                NextModuleAddress = s.FirstThunk;
            FirstThunk = s.FirstThunk;

            return true;
        }

        #endregion

        #region IBinaryAppender<ImageBoundImportForwarderRef,ReaderWithOffsetArgs> Members

        bool IBinaryAppender<ImageBoundImportForwarderRef, ReaderWithOffsetArgs>.Attach(ref ImageBoundImportForwarderRef s, uint size, ReaderWithOffsetArgs arg)
        {
            ImportBoundForwarderDescription x =
                new ImportBoundForwarderDescription(arg.Source.ReadStringAnsiAt(arg.Offset + s.OffsetModuleName),
                                                    new DateTime(1970, 1, 1).AddSeconds(s.TimeDateStamp));

            Add(x);
            return true;
        }

        #endregion
    }
}
