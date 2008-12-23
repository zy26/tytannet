using System.Text.RegularExpressions;

namespace Pretorianie.Tytan.Core.Data.Specialized
{
    /// <summary>
    /// Extractor class specialized to support no language.
    /// </summary>
    public class CodeExtractorDummy : CodeExtractor
    {
        /// <summary>
        /// Init constructor.
        /// </summary>
        public CodeExtractorDummy(CodeEditPoint editPoint)
            : base(editPoint)
        {
        }

        #region Overrides of CodeExtractor

        /// <summary>
        /// Gets the regular expression to extract namespace's name from <c>CodeElement</c>.
        /// </summary>
        public override Regex ImportNamespace
        {
            get { return null; }
        }

        #endregion
    }
}
