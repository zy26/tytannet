using System;

namespace Pretorianie.Tytan.Core.NativeImage.Common
{
    /// <summary>
    /// File extension description class.
    /// </summary>
    public class EngineFileExtension
    {
        private readonly string extension;
        private readonly string description;

        /// <summary>
        /// Init constructor.
        /// Stores all the info about file extension.
        /// </summary>
        public EngineFileExtension(string extension, string description)
        {
            if(extension == null || extension[0] != '.')
                throw new ArgumentException("Extension must start with a '.' character.", "extension");

            this.extension = extension;
            this.description = description;
        }

        #region Properties

        /// <summary>
        /// Gets the extension with '.' at the front.
        /// </summary>
        public string Extension
        {
            get { return extension; }
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        public string Description
        {
            get { return description; }
        }

        #endregion
    }
}
