namespace Pretorianie.Tytan.Core.Data
{
    /// <summary>
    /// Class to enumerate lines and navigate over <c>TextDocument</c> content.
    /// </summary>
    public class CodeLineNavigator
    {
        private readonly CodeExtractor codeExtractor;
        private readonly string firstLine;
        private int startLine;
        private int currentLine;

        /// <summary>
        /// Init constructor of CodeLineNavigator.
        /// </summary>
        public CodeLineNavigator(CodeExtractor codeExtractor, int startLine)
        {
            this.codeExtractor = codeExtractor;
            this.startLine = startLine;
            currentLine = startLine;
            firstLine = ReadLine();
        }

        /// <summary>
        /// Gets the first line of text from current document.
        /// </summary>
        public string FirstLine
        {
            get { return firstLine; }
        }

        /// <summary>
        /// Gets the current code extractor unit.
        /// </summary>
        public CodeExtractor CodeExtractor
        {
            get { return codeExtractor; }
        }

        /// <summary>
        /// Gets the text from next line.
        /// </summary>
        public string NextLine()
        {
            return GetLine(1);
        }

        /// <summary>
        /// Gets the text from previous line.
        /// </summary>
        public string PreviousLine()
        {
            return GetLine(-1);
        }

        /// <summary>
        /// Goes back to start line.
        /// </summary>
        public void Reset()
        {
            currentLine = startLine;
        }

        /// <summary>
        /// Gets the line of text from editor.
        /// </summary>
        private string GetLine(int nextStep)
        {
            string text;

            do
            {
                text = ReadLine();

                // no more text inside current document:
                if (text == null)
                    return null;

                // remove comments:
                text = ProcessLine(text);
                currentLine += nextStep;

                if (currentLine < 0)
                    return null;
            } while (string.IsNullOrEmpty(text));

            return text;
        }

        /// <summary>
        /// Reads line from current index.
        /// </summary>
        private string ReadLine()
        {
            try
            {
                return codeExtractor.EditPoint.GetLines(currentLine, currentLine + 1);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Removes unneeded chars from given string.
        /// </summary>
        private string ProcessLine(string text)
        {
            // remove any comment:
            int index;
            do
            {
                index = codeExtractor.GetCommentIndexStart(text);
                if (index >= 0)
                    text = text.Remove(index, codeExtractor.GetCommentIndexEnd(text) - index);
            } while (index >= 0);

            return text;
        }
    }
}
