using System;
using System.Collections.Generic;
using System.Text;

namespace Pretorianie.Tytan.Core.NativeImage
{

    public interface ISourceProvider
    {
        /// <summary>
        /// Gets or sets the location inside current data buffer.
        /// </summary>
        uint Position
        { get; set; }
    }
}
