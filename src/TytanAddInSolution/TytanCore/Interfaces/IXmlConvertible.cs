using System.Xml;

namespace Pretorianie.Tytan.Core.Interfaces
{
    /// <summary>
    /// Generic interface implemented by all objects supporting own and non-invasive serialization to XML.
    /// </summary>
    public interface IXmlConvertible
    {
        /// <summary>
        /// Converts element to <see cref="XmlNode"/>.
        /// </summary>
        XmlNode ToXmlNode(IXmlDocument xmlDocument, object runtimeParams);

        /// <summary>
        /// Reads info from <see cref="XmlNode"/> and fills the object.
        /// </summary>
        bool FromXmlNode(XmlNode node, object runtimeParams);
    }
}