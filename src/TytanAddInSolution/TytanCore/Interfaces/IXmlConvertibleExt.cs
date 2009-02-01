using System.IO;

namespace Pretorianie.Tytan.Core.Interfaces
{
    /// <summary>
    /// Generic interface extending standard Xml conversion.
    /// It adds support for direct writing values into output stream.
    /// </summary>
    public interface IXmlConvertibleExt : IXmlConvertible
    {
        /// <summary>
        /// Writes given object into output text buffer.
        /// </summary>
        void ToXmlExt(TextWriter writer, object runtimeParams);
    }
}
