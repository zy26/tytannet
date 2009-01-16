namespace Pretorianie.Tytan.Parsers.Model
{
    /// <summary>
    /// Class describing abstract section of binary file.
    /// </summary>
    public class BinarySection
    {
        private string sectionName;
        private uint fileOffset;
        private uint fileSize;
        private uint virtualAddress;
        private uint virtualSize;

        /// <summary>
        /// Default constructor.
        /// </summary>
        protected BinarySection()
        {
        }

        /// <summary>
        /// Init constructor of BinarySection.
        /// </summary>
        public BinarySection(string name, uint fileOffset, uint fileSize)
        {
            sectionName = name;
            this.fileOffset = fileOffset;
            this.fileSize = fileSize;
        }

        /// <summary>
        /// Update internal infos about section name, file section's offset and size.
        /// </summary>
        public void UpdateFileInfo(string name, uint offset, uint size)
        {
            sectionName = name;
            fileOffset = offset;
            fileSize = size;
        }

        /// <summary>
        /// Updates internal infos about virtual allocation of this function when mapped into memory.
        /// </summary>
        public void UpdateVirtualInfo(uint address, uint size)
        {
            virtualAddress = address;
            virtualSize = size;
        }

        #region Properties

        /// <summary>
        /// Gets name of this section.
        /// </summary>
        public string Name
        {
            get
            {
                return sectionName;
            }
        }

        /// <summary>
        /// Gets the offset from the beginning of binary file.
        /// </summary>
        public uint FileOffset
        {
            get
            {
                return fileOffset;
            }
        }

        /// <summary>
        /// Gets the size of this section.
        /// </summary>
        public uint FileSize
        {
            get
            {
                return fileSize;
            }
        }

        /// <summary>
        /// Gets the offset of this section inside memory.
        /// </summary>
        public uint VirtualAddress
        {
            get
            {
                return virtualAddress;
            }
        }

        /// <summary>
        /// Gets the size of this section inside memory.
        /// </summary>
        public uint VirtualSize
        {
            get
            {
                return virtualSize;
            }
        }

        /// <summary>
        /// Gets the indication if given section is valid.
        /// </summary>
        public bool IsValid
        {
            get { return virtualAddress != 0 || virtualSize != 0; }
        }

        #endregion

        /// <summary>
        /// Checks if current section contains given address.
        /// </summary>
        public bool ContainsVirtual (uint address)
        {
            return address >= virtualAddress && address <= virtualAddress + virtualSize;
        }

        /// <summary>
        /// Checks if current section contains given address.
        /// </summary>
        public bool ContainsFile(uint offset)
        {
            return offset >= fileOffset && offset <= fileOffset + fileSize;
        }
    }
}