﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Pretorianie.Tytan.Core.Mapping;
using Pretorianie.Tytan.Parsers.Model;

namespace Pretorianie.Tytan.Parsers.Coff
{
    /// <summary>
    /// Class encapsulating operations over standard Windows Portable Executable objects (Win PE).
    /// This can be used for analyzing files such as: .exe, .dll, .obj, .lib.
    /// </summary>
    public class WindowsPortableExecutable : BinaryFile,
                             IBinaryAppender<DataSectionDescription.ImageDataDirectory, DirectoryEntry>,
                             IBinaryAppender<ExportFunctionSection.ImageExportDirectory, ReaderWithOffsetArgs>,
                             IBinaryAppender<ImportFunctionModule.ImageImportDescriptor, ReaderWithOffsetArgs>,
                             IBinaryAppender<ImportFunctionModule.ImageImportData32, ReaderWithOffsetArgs>,
                             IBinaryAppender<ImportFunctionModule.ImageImportData64, ReaderWithOffsetArgs>,
                             IBinaryAppender<ImportFunctionModule.ImageBoundImportDescription, ReaderWithOffsetArgs>,
                             IBinaryAppender<RelocationSection.ImageBaseRelocation, ReaderWithOffsetArgs>

    {
        private Dictionary<DirectoryEntry, DataSectionDescription> knownSections =
            new Dictionary<DirectoryEntry, DataSectionDescription>();
        private IList<DataHeaderSection> dataSections = new List<DataHeaderSection>();

        #region Consts

        private const int LibHeaderNameLength = 8;
        private const string LibHeaderName = "!<arch>\n";

        #endregion

        #region Overrides of BinaryFile

        /// <summary>
        /// Loads proper binary sections based on given source.
        /// </summary>
        protected override void Load(UnmanagedDataReader s, BinaryLoadArgs e)
        {
            WindowsPortableExecutableLoadArgs a = e as WindowsPortableExecutableLoadArgs ??
                                                  new WindowsPortableExecutableLoadArgs();

            /////////////////////////////////////////
            // detect file type:

            // is it a .lib file?
            string text = s.ReadStringAnsiAt(0, LibHeaderNameLength);
            if (text == LibHeaderName)
                LoadLibBinary(s, a);

            // read MS-DOS compatible header and based on that
            // try to detect values of the fields correct for specific formats:
            DosHeaderSection dosSection = Read<DosHeaderSection.ImageDosHeader, DosHeaderSection>(s, false);

            if (dosSection == null)
                return;

            // load data as exe/dll binary:
            if (dosSection.IsExecutable)
            {
                Add(dosSection);

                // jump to new NT header:
                if (s.Jump(dosSection.NtHeaderAddress) == uint.MaxValue)
                    return;

                LoadExeBinary(s, a);
                return;
            }

            // load debug file:
            if (dosSection.IsDebugBinary)
            {
                s.UndoRead();
                LoadDbgBinary(s, a);
                return;
            }

            // load data as object binary:
            if (dosSection.IsObjectBinary)
            {
                s.UndoRead();
                LoadObjBinary(s, a);
                return;
            }
        }

        /// <summary>
        /// Loads binary as Visual Studio C++ .lib file.
        /// </summary>
        private void LoadLibBinary(UnmanagedDataReader s, WindowsPortableExecutableLoadArgs e)
        {
        }

        private void LoadObjBinary(UnmanagedDataReader s, WindowsPortableExecutableLoadArgs e)
        {
        }

        private void LoadDbgBinary(UnmanagedDataReader s, WindowsPortableExecutableLoadArgs e)
        {
        }

        /// <summary>
        /// Loads a binary data as a Windows COFF Portable Executable.
        /// </summary>
        private void LoadExeBinary(UnmanagedDataReader s, WindowsPortableExecutableLoadArgs e)
        {
            NtHeaderSection ntSection;
            NtOptionalHeaderSection ntOptionalSection;
            DataHeaderSection dataSection;

            // read Win NT compatible header:
            ntSection = Read<NtHeaderSection.ImageNtHeader, NtHeaderSection>(s);
            if (ntSection == null)
                return;

            // detect the type of optional header:
            ushort magicNumber = 0;

            if (!s.Read(ref magicNumber))
                return;

            // undo the last read:
            s.UndoRead();

            // now parse the corresponding sections:
            if (magicNumber == NtOptionalHeaderSection.OptionalHeader32Magic)
                ntOptionalSection = Read<NtOptionalHeaderSection.ImageOptionalHeader32, NtOptionalHeaderSection>(s);
            else if (magicNumber == NtOptionalHeaderSection.OptionalHeader64Magic)
                ntOptionalSection = Read<NtOptionalHeaderSection.ImageOptionalHeader64, NtOptionalHeaderSection>(s);
            else
                ntOptionalSection = Read<NtOptionalHeaderSection.ImageOptionalHeaderROM, NtOptionalHeaderSection>(s);

            // and read specified number of other section locations:
            if (ntOptionalSection != null)
            {
                for (uint i = 0; i < ntOptionalSection.DataDirectoryCount; i++)
                    if (!Append<DataSectionDescription.ImageDataDirectory, WindowsPortableExecutable, DirectoryEntry>
                             (s, this, (DirectoryEntry) i))
                        return;
            }

            // read data sections:
            for (uint i = 0; i < ntSection.DataSectionCount; i++)
            {
                dataSection = Read<DataHeaderSection.ImageSectionHeader, DataHeaderSection>(s);
                if (dataSection == null)
                    break;
                dataSections.Add(dataSection);
            }

            if (knownSections != null)
            {
                if (e.LoadExports)
                {
                    // add export information section:
                    DataSectionDescription exportSection;

                    if (knownSections.TryGetValue(DirectoryEntry.Export, out exportSection))
                        AppendExportSection(s, exportSection);
                }

                if (e.LoadImports)
                {
                    // add import information section:
                    DataSectionDescription importSection;
                    DataSectionDescription importBoundSection;

                    if (knownSections.TryGetValue(DirectoryEntry.Import, out importSection))
                    {
                        knownSections.TryGetValue(DirectoryEntry.BoundImport, out importBoundSection);
                        AppendImportSection(s, importSection, importBoundSection, ntSection);
                    }
                }

                if (e.LoadResources)
                {
                    // add resources information section:
                    DataSectionDescription resourceSection;

                    if (knownSections.TryGetValue(DirectoryEntry.Resource, out resourceSection))
                        AppendResourceSection(s, resourceSection);
                }

                if (e.LoadBaseRelocations)
                {
                    // add relocation information section:
                    DataSectionDescription relocationSection;

                    if (knownSections.TryGetValue(DirectoryEntry.BaseRelocationTable, out relocationSection))
                        AppendRelocationSection(s, relocationSection);
                }
            }

            // remove temporary data:
            knownSections = null;
            dataSections = null;
        }

        private DataHeaderSection GetSection(uint virtualAddress)
        {
            foreach (DataHeaderSection d in dataSections)
                if (d.ContainsVirtual(virtualAddress))
                    return d;

            return null;
        }

        private uint GetVirtualRelativeAddress(uint virtualAddress, out uint delta)
        {
            DataHeaderSection section = GetSection(virtualAddress);

            // analize data inside given section:
            if (section != null && section.IsValid)
            {
                delta = section.VirtualAddress - section.DataAddress;
                return virtualAddress - delta;
            }

            delta = 0;
            return 0;
        }

        /// <summary>
        /// Extracts info about Export section.
        /// </summary>
        private void AppendExportSection(UnmanagedDataReader s, DataSectionDescription exportSection)
        {
            uint delta;
            uint offset = GetVirtualRelativeAddress(exportSection.VirtualAddress, out delta);

            if (offset == 0)
                return;

            // read the info:
            s.Jump(offset);
            Append<ExportFunctionSection.ImageExportDirectory, WindowsPortableExecutable, ReaderWithOffsetArgs>
                (s, this, new ReaderWithOffsetArgs(s, offset, delta, exportSection));
        }

        /// <summary>
        /// Extracts info about Import section.
        /// </summary>
        private void AppendImportSection(UnmanagedDataReader s, DataSectionDescription importSection, DataSectionDescription importBoundSection, NtHeaderSection ntSection)
        {
            ImportFunctionSection ifs = new ImportFunctionSection();
            uint delta;
            uint offset = GetVirtualRelativeAddress(importSection.VirtualAddress, out delta);

            if (offset == 0 || ntSection == null)
                return;

            // analize data inside given section:
            ReaderWithOffsetArgs arg = new ReaderWithOffsetArgs(s, offset, delta, ifs);

            // read the info:
            s.Jump(offset);
            while (Append<ImportFunctionModule.ImageImportDescriptor, WindowsPortableExecutable, ReaderWithOffsetArgs>
                (s, this, arg))
            {
                // store the current location:
                offset = s.CurrentOffset;

                // read functions assigned to given module:
                if (ntSection.MachineType == MachineType.Intel_x86)
                    ReadImportData<ImportFunctionModule.ImageImportData32, WindowsPortableExecutable>(s, arg.Tag as ImportFunctionModule);
                else
                    if (ntSection.MachineType == MachineType.Intel_x64)
                        ReadImportData<ImportFunctionModule.ImageImportData64, WindowsPortableExecutable>(s, arg.Tag as ImportFunctionModule);

                // restore the offset:
                s.Jump(offset);
                arg.Tag = ifs;
            }

            // check if there is additional bound-import infor available and read it:
            if (importBoundSection != null)
            {
                ReaderWithOffsetArgs iArg = new ReaderWithOffsetArgs(s, importBoundSection.VirtualAddress, 0, ifs);

                s.Jump(importBoundSection.VirtualAddress);
                while(Append<ImportFunctionModule.ImageBoundImportDescription, WindowsPortableExecutable, ReaderWithOffsetArgs>(s, this, iArg))
                {
                }
            }

            // if import section was correctly filled by at least one element then store it:
            if (ifs.Count > 0)
            {
                ifs.UpdateFileInfo(ImportFunctionSection.DefaultName, offset, s.LastReadSize * (uint)ifs.Count);
                ifs.UpdateVirtualInfo(importSection.VirtualAddress, importSection.Size);
                Add(ifs);
            }
        }

        /// <summary>
        /// Reads detailed data of imported elements.
        /// </summary>
        private void ReadImportData<T, S>(UnmanagedDataReader s, ImportFunctionModule m)
            where T : struct
            where S : class, IBinaryAppender<T, ReaderWithOffsetArgs>
        {
            if (m != null)
            {
                uint xDelta;
                uint xOffset = GetVirtualRelativeAddress(m.NextModuleAddress, out xDelta);

                if (xOffset != 0)
                {
                    s.Jump(xOffset);
                    while (Append<T, S, ReaderWithOffsetArgs>
                        (s, this as S, new ReaderWithOffsetArgs(s, xOffset, xDelta, m)))
                    {
                        m.FirstThunk += s.LastReadSize;
                    }
                }
            }
        }

        /// <summary>
        /// Attaches 32-bit specific info about imported functions.
        /// </summary>
        private bool AttachImportData(ref ImportFunctionModule.ImageImportData32 r, UnmanagedDataReader s, uint delta, ImportFunctionModule m)
        {
            if (m == null)
                return false;

            // check if there is any data attached:
            if (r.OrdinalOrAddressOfData == 0)
                return false;

            if (r.IsOrdinal)
            {
                ImportFunctionModule.ImageImportData32 thunk = new ImportFunctionModule.ImageImportData32();

                if (s.ReadAt(ref thunk, m.FirstThunk - delta))
                    m.Add(new ImportFunctionDescription(r.Ordinal, m.IsBinded ? thunk.OrdinalOrAddressOfData : 0));
            }
            else
            {
                ImportFunctionModule.ImageImportByName funName = new ImportFunctionModule.ImageImportByName();
                ImportFunctionModule.ImageImportData32 thunk = new ImportFunctionModule.ImageImportData32();

                if (s.ReadAt(ref thunk, m.FirstThunk - delta))
                    if (s.ReadAt(ref funName, r.OrdinalOrAddressOfData - delta))
                        m.Add(new ImportFunctionDescription(
                                  s.ReadStringAnsiAt(r.OrdinalOrAddressOfData - delta + sizeof (short)),
                                  m.IsBinded ? thunk.OrdinalOrAddressOfData : 0,
                                  (uint) funName.Hint));
            }

            return true;
        }

        /// <summary>
        /// Attaches 64-bit specific info about imported functions.
        /// </summary>
        private bool AttachImportData(ref ImportFunctionModule.ImageImportData64 r, UnmanagedDataReader s, uint delta, ImportFunctionModule m)
        {
            if (m == null)
                return false;

            // check if there is any data attached:
            if (r.OrdinalOrAddressOfData == 0)
                return false;

            if (r.IsOrdinal)
            {
                ImportFunctionModule.ImageImportData64 thunk = new ImportFunctionModule.ImageImportData64();

                if (s.ReadAt(ref thunk, m.FirstThunk - delta))
                    m.Add(new ImportFunctionDescription(r.Ordinal,
                                                                  m.IsBinded ? thunk.OrdinalOrAddressOfData : 0));
            }
            else
            {
                ImportFunctionModule.ImageImportByName funName = new ImportFunctionModule.ImageImportByName();
                ImportFunctionModule.ImageImportData64 thunk = new ImportFunctionModule.ImageImportData64();

                if (s.ReadAt(ref thunk, m.FirstThunk - delta))
                    if (s.ReadAt(ref funName, (uint) r.OrdinalOrAddressOfData - delta))
                        m.Add(new ImportFunctionDescription(
                                  s.ReadStringAnsiAt((uint) r.OrdinalOrAddressOfData - delta + sizeof (short)),
                                  m.IsBinded ? thunk.OrdinalOrAddressOfData : 0,
                                  (uint) funName.Hint));
            }

            return true;
        }

        private void AppendResourceSection (UnmanagedDataReader s, DataSectionDescription resourceSection)
        {
            uint delta;
            uint offset = GetVirtualRelativeAddress(resourceSection.VirtualAddress, out delta);

            if (offset == 0)
                return;
        }

        private void AppendRelocationSection(UnmanagedDataReader s, DataSectionDescription relocationSection)
        {
            uint delta;
            uint offset = GetVirtualRelativeAddress(relocationSection.VirtualAddress, out delta);
            ReaderWithOffsetArgs arg;
            RelocationSection section;

            if (offset == 0)
                return;

            // read the info:
            s.Jump(offset);

            // add all sections:
            section = new RelocationSection();
            arg = new ReaderWithOffsetArgs(s, offset, delta, section);
            while (Append<RelocationSection.ImageBaseRelocation, WindowsPortableExecutable, ReaderWithOffsetArgs>
                (s, this, arg))
            {
            }

            // append relocation section when any data was read from file:
            if (section.Count > 0)
            {
                section.UpdateFileInfo(RelocationSection.DefaultName, offset, s.CurrentOffset - offset);
                section.UpdateVirtualInfo(offset, s.CurrentOffset - offset);

                Add(section);
            }
        }

        #endregion

        #region IBinaryAppender<ImageDataDirectory, DirectoryEntry> Members

        bool IBinaryAppender<DataSectionDescription.ImageDataDirectory, DirectoryEntry>.Attach(ref DataSectionDescription.ImageDataDirectory s, uint size, DirectoryEntry arg)
        {
            if (s.VirtualAddress != 0 && s.Size != 0)
                knownSections.Add(arg, new DataSectionDescription(s.VirtualAddress, s.Size));

            return true;
        }

        #endregion

        #region Implementation of IBinaryAppender<ImageExportDirectory,uint>

        /// <summary>
        /// Setup internal data based on given input read from native image.
        /// </summary>
        bool IBinaryAppender<ExportFunctionSection.ImageExportDirectory, ReaderWithOffsetArgs>.Attach(ref ExportFunctionSection.ImageExportDirectory s, uint size, ReaderWithOffsetArgs arg)
        {
            ExportFunctionSection x = new ExportFunctionSection();
            IBinaryConverter<ExportFunctionSection.ImageExportDirectory> c = x;
            DataSectionDescription e = arg.Tag as DataSectionDescription;

            if (!c.Convert(ref s, arg.Offset, size))
                return false;

            if (e != null)
                x.UpdateVirtualInfo(e.VirtualAddress, e.Size);

            if (x.Read(arg))
            {
                Add(x);
                return true;
            }

            return false;
        }

        #endregion

        #region IBinaryAppender<ImageImportDescriptor,ReaderWithOffsetArgs> Members

        bool IBinaryAppender<ImportFunctionModule.ImageImportDescriptor, ReaderWithOffsetArgs>.Attach(ref ImportFunctionModule.ImageImportDescriptor s, uint size, ReaderWithOffsetArgs arg)
        {
            ImportFunctionSection imports = arg.Tag as ImportFunctionSection;
            IBinaryAppender<ImportFunctionModule.ImageImportDescriptor, ReaderWithOffsetArgs> t = new ImportFunctionModule();

            // read the next import module description:
            if (t.Attach(ref s, size, arg))
                if (imports != null)
                {
                    imports.Add(t as ImportFunctionModule);
                    arg.Tag = t;
                    return true;
                }

            return false;
        }

        #endregion

        #region IBinaryAppender<ImageImportData32,ReaderWithOffsetArgs> Members

        bool IBinaryAppender<ImportFunctionModule.ImageImportData32, ReaderWithOffsetArgs>.Attach(ref ImportFunctionModule.ImageImportData32 s, uint size, ReaderWithOffsetArgs arg)
        {
            return AttachImportData(ref s, arg.Source, arg.Delta, arg.Tag as ImportFunctionModule);
        }

        #endregion

        #region IBinaryAppender<ImageImportData64,ReaderWithOffsetArgs> Members

        bool IBinaryAppender<ImportFunctionModule.ImageImportData64, ReaderWithOffsetArgs>.Attach(ref ImportFunctionModule.ImageImportData64 s, uint size, ReaderWithOffsetArgs arg)
        {
            return AttachImportData(ref s, arg.Source, arg.Delta, arg.Tag as ImportFunctionModule);
        }

        #endregion

        #region IBinaryAppender<ImageBoundImportDescription,ReaderWithOffsetArgs> Members

        bool IBinaryAppender<ImportFunctionModule.ImageBoundImportDescription, ReaderWithOffsetArgs>.Attach(ref ImportFunctionModule.ImageBoundImportDescription s, uint size, ReaderWithOffsetArgs arg)
        {
            if (s.TimeDateStamp == 0)
                return false;

            ImportFunctionSection ifs = arg.Tag as ImportFunctionSection;
            if (ifs == null)
                return false;

            string name = arg.Source.ReadStringAnsiAt(arg.Offset + s.OffsetModuleName);
            ImportFunctionModule ifm = ifs[name];

            // if there is a module to update:
            if (ifm != null)
            {
                ReaderWithOffsetArgs mArg = new ReaderWithOffsetArgs(arg.Source, arg.Offset, 0, null);
                ifm.BoundDate = new DateTime(1970, 1, 1).AddSeconds(s.TimeDateStamp);

                // append given number of forwarders, having the same size as the current structure
                // and lying just after:
                for (ushort i = 0; i < s.NumberOfModuleForwarderRefs; i++)
                {
                    Append
                        <ImportFunctionModule.ImageBoundImportForwarderRef, ImportFunctionModule, ReaderWithOffsetArgs>(
                        arg.Source, ifm, mArg);
                }
            }

            return true;
        }

        #endregion

        #region IBinaryAppender<ImageBaseRelocation,ReaderWithOffsetArgs> Members

        bool IBinaryAppender<RelocationSection.ImageBaseRelocation, ReaderWithOffsetArgs>.Attach(ref RelocationSection.ImageBaseRelocation s, uint size, ReaderWithOffsetArgs arg)
        {
            UnmanagedDataReader r = arg.Source;
            RelocationSection section = arg.Tag as RelocationSection;
            RelocationDescription desc;
            ushort data = 0;
            RelocationType type;
            uint count;
            uint offset;

            // validate input data:
            if (s.SizeOfBlock == 0 || s.VirtualAddress == 0 || section == null)
                return false;

            // append data to section:
            count = (s.SizeOfBlock - (uint) Marshal.SizeOf(typeof (RelocationSection.ImageBaseRelocation)))/
                                (uint) Marshal.SizeOf(typeof (ushort));

            if (count > 0)
            {
                desc = new RelocationDescription(s.VirtualAddress, s.SizeOfBlock);

                for (uint i = 0; i < count; i++)
                {
                    if (r.Read(ref data))
                    {
                        type = (RelocationType) ((data & 0xF000) >> 12);
                        offset = (uint) (data & 0x0FFF);

                        if (type == RelocationType.BasedHighAdjust)
                        {
                            if (r.Read(ref data))
                            {
                                offset += ((uint) data) << 12;
                                count--;
                            }
                        }

                        desc.Add(new RelocationItem(type, offset));
                    }
                    else
                        break;
                }

                // if data is defined inside the given description:
                if (desc.Count > 0)
                    section.Add(desc);
            }

            return true;
        }

        #endregion
    }
}