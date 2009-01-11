namespace Pretorianie.Tytan.Parsers.Coff
{
    /// <summary>
    /// Function exported from given COFF file.
    /// </summary>
    public class ExportFunctionDescription
    {
        private readonly string name;
        private readonly string forwardedName;
        private readonly uint ordinal;
        private readonly ulong address;

        /// <summary>
        /// Init constructor of ExportFunctionDescription.
        /// </summary>
        public ExportFunctionDescription(string name, uint ordinal, ulong address)
        {
            this.name = name;
            this.ordinal = ordinal;
            this.address = address;
        }

        /// <summary>
        /// Init constructor of ExportFunctionDescription.
        /// </summary>
        public ExportFunctionDescription(string name, string forwardedName, uint ordinal, ulong address)
        {
            this.name = name;
            this.forwardedName = forwardedName;
            this.ordinal = ordinal;
            this.address = address;
        }

        #region Properties

        /// <summary>
        /// Gets the value of Name.
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }
        }

        /// <summary>
        /// Gets the value of ForwardedName.
        /// </summary>
        public string ForwardedName
        {
            get
            {
                return forwardedName;
            }
        }

        /// <summary>
        /// Gets the value of Ordinal.
        /// </summary>
        public uint Ordinal
        {
            get
            {
                return ordinal;
            }
        }

        /// <summary>
        /// Gets the value of Address.
        /// </summary>
        public ulong VirtualAddress
        {
            get
            {
                return address;
            }
        }

        /// <summary>
        /// Gets an indication if given function is just a forwarder to another function.
        /// </summary>
        public bool IsForwarded
        {
            get
            {
                return !string.IsNullOrEmpty(forwardedName);
            }
        }

        #endregion
    }
}