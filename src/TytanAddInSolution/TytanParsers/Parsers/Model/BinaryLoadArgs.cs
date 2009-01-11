namespace Pretorianie.Tytan.Parsers.Model
{
    /// <summary>
    /// Class passing additional parameters according to parsing operation.
    /// </summary>
    public class BinaryLoadArgs
    {
        private readonly bool useMapping;
        private readonly bool isReadOnly;

        /// <summary>
        /// Init constructor of BinaryLoadArgs.
        /// </summary>
        public BinaryLoadArgs(bool useMapping, bool isReadOnlyMode)
        {
            this.useMapping = useMapping;
            isReadOnly = isReadOnlyMode;
        }

        #region Properties

        /// <summary>
        /// Gets the indication if memory mapping will be used.
        /// </summary>
        public bool UseMapping
        {
            get { return useMapping; }
        }

        /// <summary>
        /// Gets the indication if current file should be opened in read-only mode.
        /// </summary>
        public bool IsReadOnlyMode
        {
            get { return isReadOnly; }
        }

        #endregion
    }
}