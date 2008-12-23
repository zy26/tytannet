using System.Text.RegularExpressions;

namespace Pretorianie.Tytan.Core.Data.Specialized
{
    /// <summary>
    /// Extractor class specialized to support Visual Basic syntax.
    /// </summary>
    public class CodeExtractorVBasic : CodeExtractor
    {
        private const string SingleNamespaceSeparator = ".";
        private const string SingleLineComment = "'";

        private readonly Regex importNamespace = new Regex("Imports[ \\t]+((.)*)");

        /// <summary>
        /// Init constructor.
        /// </summary>
        public CodeExtractorVBasic(CodeEditPoint editPoint)
            : base(editPoint)
        {
        }

        #region Overrides of CodeExtractor

        /// <summary>
        /// Gets the regular expression to extract namespace's name from <c>CodeElement</c>.
        /// </summary>
        public override Regex ImportNamespace
        {
            get { return importNamespace; }
        }


        /// <summary>
        /// Gets the operator that separates elements of namespaces.
        /// </summary>
        public override string NamespaceSeparator
        {
            get { return SingleNamespaceSeparator; }
        }

        /// <summary>
        /// Gets the index of comment in given line.
        /// </summary>
        public override int GetCommentIndexStart(string text)
        {
            return text.IndexOf(SingleLineComment);
        }

        /// <summary>
        /// Gets the index of comment in given line.
        /// </summary>
        public override int GetCommentIndexEnd(string text)
        {
            int i = text.IndexOf(SingleLineComment);

            if (i >= 0)
                return text.Length;
            return -1;
        }

        /// <summary>
        /// Checks if given separator can be skipped while reading namespace of identifier.
        /// </summary>
        public override bool CanSkipNamespaceSeparator(string separator)
        {
            return separator == "_";
        }

        #endregion
    }
}
