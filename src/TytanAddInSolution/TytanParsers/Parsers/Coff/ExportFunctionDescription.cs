using Pretorianie.Tytan.Parsers.Model;

namespace Pretorianie.Tytan.Parsers.Coff
{
    /// <summary>
    /// Function exported from given COFF file.
    /// </summary>
    public class ExportFunctionDescription : BaseFunctionDescription
    {
        private readonly string forwardedName;

        /// <summary>
        /// Init constructor of ExportFunctionDescription.
        /// </summary>
        public ExportFunctionDescription(string name, uint ordinal, ulong address)
            : base(name, ordinal, address)
        {
        }

        /// <summary>
        /// Init constructor of ExportFunctionDescription.
        /// </summary>
        public ExportFunctionDescription(string name, string forwardedName, uint ordinal, ulong address)
            : base(name, ordinal, address)
        {
            this.forwardedName = forwardedName;
        }

        #region Properties

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