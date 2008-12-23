namespace Pretorianie.Tytan.Core.Data
{
    /// <summary>
    /// Class to enumerate words and navigate over <c>CodeLineNavigator</c>.
    /// </summary>
    public class CodeWordNavigator
    {
        private readonly CodeLineNavigator navigator;
        private readonly int startIndex;
        private int currentIndex;
        private string currentLine;
        private int length;

        /// <summary>
        /// Init constructor of CodeWordNavigator.
        /// </summary>
        public CodeWordNavigator(CodeLineNavigator navigator, int startIndex)
        {
            this.navigator = navigator;
            this.startIndex = startIndex;
            ReadNextLine();
            currentIndex = startIndex;
            NormalizePosition();
        }

        /// <summary>
        /// Reads next line.
        /// </summary>
        private int ReadNextLine()
        {
            currentLine = navigator.NextLine();
            currentIndex = 0;
            return length = currentLine != null ? currentLine.Length : 0;
        }

        /// <summary>
        /// Reads previous line.
        /// </summary>
        private int ReadPreviousLine()
        {
            currentLine = navigator.PreviousLine();
            currentIndex = currentLine != null ? currentLine.Length : 0;
            return length = currentIndex;
        }

        /// <summary>
        /// Goes back to start line and jumps to the beginning of the current word.
        /// </summary>
        public void ResetForNext()
        {
            navigator.Reset();
            ReadNextLine();
            currentIndex = startIndex;
            NormalizePosition();
        }

        /// <summary>
        /// Goes back to start line and jumps to the beginning of the current word.
        /// </summary>
        public void ResetForPrevious()
        {
            navigator.Reset();
            ReadPreviousLine();
            currentIndex = startIndex;
            NormalizePosition();
        }

        /// <summary>
        /// Jumps to the beginning of current word.
        /// </summary>
        private void NormalizePosition()
        {
            // move back the current index to the beginning of the word:
            if (!string.IsNullOrEmpty(currentLine))
            {
                int start = currentIndex;

                while (currentIndex > 0 && navigator.CodeExtractor.IsIdentifierChar(currentLine[currentIndex - 1]))
                    currentIndex--;

                // if not at the beginning of an identifier, so maybe in front of operator
                if (start == currentIndex)
                    while (currentIndex > 0 && navigator.CodeExtractor.IsOperatorChar(currentLine[currentIndex - 1]))
                        currentIndex--;
            }
        }

        /// <summary>
        /// Reads the next word on right.
        /// </summary>
        public string NextWord(out bool isOperator)
        {
            string word = null;

            isOperator = false;

            // can not read next word:
            if (string.IsNullOrEmpty(currentLine))
                return null;

            do
            {
                // if it is the end of line, read next:
                if (currentIndex >= length)
                    if (ReadNextLine() == 0 && currentLine == null)
                        return null;

                // try to read next word:
                int end = currentIndex;
                while (end < length && navigator.CodeExtractor.IsIdentifierChar(currentLine[end]))
                    end++;

                // try to read 'operator':
                if (end == currentIndex)
                {
                    while (end < length && navigator.CodeExtractor.IsOperatorChar(currentLine[end]))
                        end++;

                    if (end != currentIndex)
                        isOperator = true;
                }

                // was successfull reading ?
                if (end != currentIndex)
                {
                    word = currentLine.Substring(currentIndex, end - currentIndex);
                    currentIndex = end;
                }

                // if it is the end of line, read next:
                if (currentIndex >= length)
                    ReadNextLine();

                // skip white spaces:
                while (currentIndex < length && char.IsWhiteSpace(currentLine[currentIndex]))
                    currentIndex++;

            } while (currentIndex >= length);

            return word;
        }

        /// <summary>
        /// Reads the previous word on left.
        /// </summary>
        public string PreviousWord(out bool isOperator)
        {
            string word = null;

            isOperator = false;

            // can not read next word:
            if (string.IsNullOrEmpty(currentLine))
                return null;

            do
            {
                // skip white spaces:
                currentIndex--;
                while (currentIndex >= 0 && char.IsWhiteSpace(currentLine[currentIndex]))
                    currentIndex--;

                // if it is the end of line, read next:
                if (currentIndex < 0)
                {
                    if (ReadPreviousLine() == 0 && currentLine == null)
                        return null;
                }

            } while (currentIndex < 0 || currentIndex >= currentLine.Length);
            currentIndex++;

            // try to read next word:
            int start = currentIndex;
            while (start > 0 && navigator.CodeExtractor.IsIdentifierChar(currentLine[start - 1]))
                start--;

            // try to read 'operator':
            if (start == currentIndex)
            {
                while (start > 0 && navigator.CodeExtractor.IsOperatorChar(currentLine[start - 1]))
                    start--;

                if (start != currentIndex)
                    isOperator = true;
            }

            // was successfull reading ?
            if (start != currentIndex)
            {
                word = currentLine.Substring(start, currentIndex - start);
                currentIndex = start;
            }

            return word;
        }
    }
}
