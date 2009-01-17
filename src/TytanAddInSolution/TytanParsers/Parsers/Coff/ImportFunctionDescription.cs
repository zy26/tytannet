using Pretorianie.Tytan.Parsers.Model;

namespace Pretorianie.Tytan.Parsers.Coff
{
    /// <summary>
    /// Imported function by given COFF file.
    /// </summary>
    public class ImportFunctionDescription : BaseFunctionDescription
    {
        /// <summary>
        /// Init constructor of ImportFunctionDescription.
        /// </summary>
        public ImportFunctionDescription(string name, ulong address, ulong hint)
            : base(name, 0, address, hint)
        {
        }

        /// <summary>
        /// Init constructor of ImportFunctionDescription.
        /// </summary>
        public ImportFunctionDescription(uint ordinal, ulong address)
            : base(string.Empty, ordinal, address, 0)
        {
        }
    }
}
