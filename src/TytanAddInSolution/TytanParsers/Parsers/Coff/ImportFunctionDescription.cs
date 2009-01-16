using Pretorianie.Tytan.Parsers.Model;

namespace Pretorianie.Tytan.Parsers.Coff
{
    /// <summary>
    /// Imported function by given COFF file.
    /// </summary>
    public class ImportFunctionDescription : BaseFunctionDescription
    {
        private readonly ulong hint;

        /// <summary>
        /// Init constructor of ImportFunctionDescription.
        /// </summary>
        public ImportFunctionDescription(string name, ulong address, ulong hint)
            : base(name, 0, address)
        {
            this.hint = hint;
        }

        /// <summary>
        /// Init constructor of ImportFunctionDescription.
        /// </summary>
        public ImportFunctionDescription(uint ordinal, ulong address)
            : base(string.Empty, ordinal, address)
        {
        }

        #region Properties

        /// <summary>
        /// Gets the value of Hint.
        /// </summary>
        public ulong Hint
        {
            get
            {
                return hint;
            }
        }

        #endregion
    }
}
