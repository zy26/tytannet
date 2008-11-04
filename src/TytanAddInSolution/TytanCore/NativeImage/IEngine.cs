using System;
using System.Collections.Generic;
using Pretorianie.Tytan.Core.NativeImage.Common;

namespace Pretorianie.Tytan.Core.NativeImage
{
    /// <summary>
    /// Base interface implemented by all native image processor engines to deliver information
    /// about specified binary files.
    /// </summary>
    public interface IEngine : IServiceProvider, IDescriptor
    {
        /// <summary>
        /// Gets the list of file extensions supported by this engine.
        /// </summary>
        IList<EngineFileExtension> FileExtensions
        { get; }
    }
}
