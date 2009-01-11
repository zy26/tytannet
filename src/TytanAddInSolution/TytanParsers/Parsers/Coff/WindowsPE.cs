using System.Collections.Generic;
using Pretorianie.Tytan.Core.Mapping;
using Pretorianie.Tytan.Parsers.Model;

namespace Pretorianie.Tytan.Parsers.Coff
{
    /// <summary>
    /// Class encapsulating operations over standard Windows Portable Executable objects (Win PE).
    /// This can be used for analyzing files such as: .exe, .dll, .pdb, .obj, .lib.
    /// </summary>
    public class WindowsPE : BinaryFile,
                             IBinaryAppender<DataSectionDescription.ImageDataDirectory, DirectoryEntry>,
                             IBinaryAppender<ExportFunctionSection.ImageExportDirectory, ReaderWithOffsetArgs>
    {
        private Dictionary<DirectoryEntry, DataSectionDescription> knownSections =
            new Dictionary<DirectoryEntry, DataSectionDescription>();
        private IList<DataHeaderSection> dataSections = new List<DataHeaderSection>();
        private Dictionary<DirectoryEntry, BinarySection> specialSections =
            new Dictionary<DirectoryEntry, BinarySection>();

        #region Properties

        /// <summary>
        /// Gets the descriptions of known sections.
        /// </summary>
        public IDictionary<DirectoryEntry, BinarySection> SpecialSections
        {
            get { return specialSections; }
        }

        #endregion

        #region Overrides of BinaryFile

        /// <summary>
        /// Loads proper binary sections based on given source.
        /// </summary>
        protected override void Load(UnmanagedDataReader s, BinaryLoadArgs e)
        {
            DosHeaderSection dosSection;
            NtHeaderSection ntSection;
            NtOptionalHeaderSection ntOptionalSection;
            DataHeaderSection dataSection;

            // read MS-DOS compatible header:
            dosSection = Read<DosHeaderSection.ImageDosHeader, DosHeaderSection>(s);
            if (dosSection == null)
                return;

            if (dosSection.Name == "Object")
            {
                s.UndoRead();

                // read Win NT compatible header:
                ntSection = Read<NtHeaderSection.ImageFileHeader, NtHeaderSection>(s);
                if (ntSection == null)
                    return;
            }
            else
            {
                // jump to new NT header:
                if (s.Jump(dosSection.NtHeaderAddress) == uint.MaxValue)
                    return;

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
                        if (!Append<DataSectionDescription.ImageDataDirectory, WindowsPE, DirectoryEntry>
                                 (s, this, (DirectoryEntry) i))
                            return;
                }
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
                // add export information section:
                DataSectionDescription exportSection;

                if (knownSections.TryGetValue(DirectoryEntry.Export, out exportSection))
                    AppendExportSection(s, exportSection);
            }
        }

        private DataHeaderSection GetSection(uint virtualAddress)
        {
            foreach (DataHeaderSection d in dataSections)
                if (d.ContainsVirtual(virtualAddress))
                    return d;

            return null;
        }

        private void AppendExportSection(UnmanagedDataReader s, DataSectionDescription exportSection)
        {
            DataHeaderSection section = GetSection(exportSection.VirtualAddress);

            // analize data inside given section:
            if (section != null)
            {
                uint delta = section.VirtualAddress - section.DataAddress;
                uint offset = exportSection.VirtualAddress - delta;

                // read the info:
                s.Jump(offset);
                Append<ExportFunctionSection.ImageExportDirectory, WindowsPE, ReaderWithOffsetArgs>
                    (s, this, new ReaderWithOffsetArgs(s, offset, delta, exportSection));
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
    }
}