using System.Text.RegularExpressions;

namespace Pretorianie.Tytan.Core.Data.Specialized
{
    /// <summary>
    /// Extractor class specialized to support C# syntax.
    /// </summary>
    public class CodeExtractorCSharp : CodeExtractor
    {
        private const string SingleLineComment = "//";
        private const string MultilineCommentStart = "/*";
        private const string MultilineCommentEnd = "*/";
        private const string SingleNamespaceSeparator = ".";

        private readonly Regex importNamespace = new Regex("using[ \\t]+((.)*);");

        /// <summary>
        /// Init constructor.
        /// </summary>
        public CodeExtractorCSharp(CodeEditPoint editPoint)
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
            int i = text.IndexOf(SingleLineComment);

            if (i >= 0)
                return i;

            return text.IndexOf(MultilineCommentStart);
        }

        /// <summary>
        /// Gets the index of comment in given line.
        /// </summary>
        public override int GetCommentIndexEnd(string text)
        {
            int i = text.IndexOf(SingleLineComment);

            if (i >= 0)
                return text.Length;

            // if this is multiline comment - then return
            // the end of it or end of line:
            i = text.IndexOf(MultilineCommentStart);
            if (i >= 0)
            {
                i = text.IndexOf(MultilineCommentEnd, i);
                if (i > 0)
                    return i + MultilineCommentEnd.Length;

                return text.Length;
            }

            return -1;
        }

        /// <summary>
        /// Checks if given separator is a terminal between instructions.
        /// </summary>
        public override bool IsTerminalInstructionSeparator(string separator)
        {
            return separator == ";";
        }

        #endregion
    }
}
