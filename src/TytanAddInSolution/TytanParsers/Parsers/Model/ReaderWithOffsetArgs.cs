using Pretorianie.Tytan.Core.Mapping;

namespace Pretorianie.Tytan.Parsers.Model
{
    /// <summary>
    /// Class describing reader and start location.
    /// </summary>
    public class ReaderWithOffsetArgs
    {
        private readonly UnmanagedDataReader source;
        private readonly uint offset;
        private readonly uint delta;
        private object tag;

        /// <summary>
        /// Init constructor of ReaderWithOffset.
        /// </summary>
        public ReaderWithOffsetArgs(UnmanagedDataReader source, uint offset, uint delta, object tag)
        {
            this.source = source;
            this.offset = offset;
            this.delta = delta;
            this.tag = tag;
        }

        #region Properties

        /// <summary>
        /// Gets the value of Source.
        /// </summary>
        public UnmanagedDataReader Source
        {
            get
            {
                return source;
            }
        }

        /// <summary>
        /// Gets the value of Offset.
        /// </summary>
        public uint Offset
        {
            get
            {
                return offset;
            }
        }

        /// <summary>
        /// Gets the value of Delta.
        /// </summary>
        public uint Delta
        {
            get
            {
                return delta;
            }
        }

        /// <summary>
        /// Gets the size of Tag.
        /// </summary>
        public object Tag
        {
            get
            {
                return tag;
            }
            set
            {
                tag = value;
            }
        }

        #endregion
    }
}