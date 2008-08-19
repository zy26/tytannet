using System.Collections.Generic;

namespace Pretorianie.Tytan.Core.NativeImage
{
    public interface INativeImageDescriptor : IDescriptor
    {
        /// <summary>
        /// Gets the collection of all structures stored inside current NativeImage.
        /// </summary>
        IList<ISectionDescriptor> Structures
        { get; }

        /// <summary>
        /// Gets the collection of all other sources extracted from NativeImage that can be shown to the user.
        /// </summary>
        IList<IContentDescriptor> Contents
        { get; }

        /// <summary>
        /// Gets the full path to the NativeImage object.
        /// </summary>
        string FullPath
        { get; }

        /// <summary>
        /// Gets the extension of NativeImage object.
        /// </summary>
        string Extension
        { get; }
    }
}
