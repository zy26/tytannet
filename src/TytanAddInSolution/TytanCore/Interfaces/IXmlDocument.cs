using System.Xml;

namespace Pretorianie.Tytan.Core.Interfaces
{
    /// <summary>
    /// Generic interface providing only creation functions over <see cref="XmlDocument"/> object.
    /// </summary>
    public interface IXmlDocument
    {
        /// <summary>
        /// Creates an System.Xml.XmlAttribute with the specified System.Xml.XmlNode.Prefix, System.Xml.XmlDocument.LocalName, and System.Xml.XmlNode.NamespaceURI.
        /// </summary>
        /// <param name="prefix">The prefix of the attribute (if any). String.Empty and null are equivalent.</param>
        /// <param name="localName">The local name of the attribute.</param>
        /// <param name="namespaceURI">The namespace URI of the attribute (if any). String.Empty and null are equivalent. If prefix is xmlns, then this parameter must be http://www.w3.org/2000/xmlns/; otherwise an exception is thrown.</param>
        /// <returns>The new XmlAttribute.</returns>
        XmlAttribute CreateAttribute(System.String prefix, System.String localName, System.String namespaceURI);

        /// <summary>
        /// Creates an System.Xml.XmlAttribute with the specified System.Xml.XmlNode.Prefix, System.Xml.XmlDocument.LocalName, and System.Xml.XmlNode.NamespaceURI.
        /// </summary>
        /// <param name="qualifiedName">The qualified name of the attribute. If the name contains a colon then the System.Xml.XmlNode.Prefix property will reflect the part of the name preceding the colon and the System.Xml.XmlDocument.LocalName property will reflect the part of the name after the colon.</param>
        /// <param name="namespaceURI">The namespaceURI of the attribute. If the qualified name includes a prefix of xmlns, then this parameter must be http://www.w3.org/2000/xmlns/. </param>
        /// <returns>The new XmlAttribute.</returns>
        XmlAttribute CreateAttribute(System.String qualifiedName, System.String namespaceURI);

        /// <summary>
        /// Creates an System.Xml.XmlAttribute with the specified System.Xml.XmlDocument.Name.
        /// </summary>
        /// <param name="name">The qualified name of the attribute. If the name contains a colon, the System.Xml.XmlNode.Prefix property reflects the part of the name preceding the first colon and the System.Xml.XmlDocument.LocalName property reflects the part of the name following the first colon. The System.Xml.XmlNode.NamespaceURI remains empty unless the prefix is a recognized built-in prefix such as xmlns. In this case NamespaceURI has a value of http://www.w3.org/2000/xmlns/. </param>
        /// <returns>The new XmlAttribute.</returns>
        XmlAttribute CreateAttribute(System.String name);

        /// <summary>
        /// Creates an System.Xml.XmlCDataSection containing the specified data.
        /// </summary>
        /// <param name="data">The content of the new XmlCDataSection.</param>
        /// <returns>The new XmlCDataSection.</returns>
        XmlCDataSection CreateCDataSection(System.String data);

        /// <summary>
        /// Creates an System.Xml.XmlComment containing the specified data.
        /// </summary>
        /// <param name="data">The content of the new XmlComment.</param>
        /// <returns>The new XmlComment.</returns>
        XmlComment CreateComment(System.String data);

        /// <summary>
        /// Creates an element with the specified System.Xml.XmlNode.Prefix, System.Xml.XmlDocument.LocalName, and System.Xml.XmlNode.NamespaceURI.
        /// </summary>
        /// <param name="prefix">The prefix of the new element (if any). String.Empty and null are equivalent.</param>
        /// <param name="localName">The local name of the new element.</param>
        /// <param name="namespaceURI">The namespace URI of the new element (if any). String.Empty and null are equivalent.</param>
        /// <returns>The new System.Xml.XmlElement.</returns>
        XmlElement CreateElement(System.String prefix, System.String localName, System.String namespaceURI);

        /// <summary>
        /// Creates an element with the specified System.Xml.XmlNode.Prefix, System.Xml.XmlDocument.LocalName, and System.Xml.XmlNode.NamespaceURI.
        /// </summary>
        /// <param name="qualifiedName">The qualified name of the element. If the name contains a colon then the System.Xml.XmlNode.Prefix property will reflect the part of the name preceding the colon and the System.Xml.XmlDocument.LocalName property will reflect the part of the name after the colon. The qualified name cannot include a prefix of'xmlns'.</param>
        /// <param name="namespaceURI">The namespace URI of the element.</param>
        /// <returns>The new XmlElement.</returns>
        XmlElement CreateElement(System.String qualifiedName, System.String namespaceURI);

        /// <summary>
        /// Creates an element with the specified System.Xml.XmlNode.Prefix, System.Xml.XmlDocument.LocalName, and System.Xml.XmlNode.NamespaceURI.
        /// </summary>
        /// <param name="name">The qualified name of the element. If the name contains a colon then the System.Xml.XmlNode.Prefix property reflects the part of the name preceding the colon and the System.Xml.XmlDocument.LocalName property reflects the part of the name after the colon. The qualified name cannot include a prefix of'xmlns'.</param>
        /// <returns>The new XmlElement.</returns>
        XmlElement CreateElement(System.String name);

        /// <summary>
        /// Creates an element with the specified System.Xml.XmlNode.Prefix, System.Xml.XmlDocument.LocalName, and System.Xml.XmlNode.NamespaceURI.
        /// </summary>
        /// <param name="name">The qualified name of the element. If the name contains a colon then the System.Xml.XmlNode.Prefix property reflects the part of the name preceding the colon and the System.Xml.XmlDocument.LocalName property reflects the part of the name after the colon. The qualified name cannot include a prefix of'xmlns'.</param>
        /// <param name="innerText">Inner text to set with the element.</param>
        /// <returns>The new XmlElement.</returns>
        XmlElement CreateElementWithText(System.String name, System.String innerText);

        /// <summary>
        /// Creates an System.Xml.XmlEntityReference with the specified name.
        /// </summary>
        /// <param name="name">The name of the entity reference.</param>
        /// <returns>The new XmlEntityReference.</returns>
        XmlEntityReference CreateEntityReference(System.String name);

        /// <summary>
        /// Creates an System.Xml.XmlNode with the specified System.Xml.XmlNodeType, System.Xml.XmlDocument.Name, and System.Xml.XmlNode.NamespaceURI.
        /// </summary>
        /// <param name="type">The XmlNodeType of the new node.</param>
        /// <param name="name">The qualified name of the new node. If the name contains a colon then it is parsed into System.Xml.XmlNode.Prefix and System.Xml.XmlDocument.LocalName components.</param>
        /// <param name="namespaceURI">The namespace URI of the new node.</param>
        /// <returns>The new XmlNode.</returns>
        XmlNode CreateNode(XmlNodeType type, System.String name, System.String namespaceURI);

        /// <summary>
        /// Creates an System.Xml.XmlNode with the specified System.Xml.XmlNodeType, System.Xml.XmlDocument.Name, and System.Xml.XmlNode.NamespaceURI.
        /// </summary>
        /// <param name="nodeTypeString">String version of the System.Xml.XmlNodeType of the new node. This parameter must be one of the values listed in the table below.</param>
        /// <param name="name">The qualified name of the new node. If the name contains a colon, it is parsed into System.Xml.XmlNode.Prefix and System.Xml.XmlDocument.LocalName components.</param>
        /// <param name="namespaceURI">The namespace URI of the new node.</param>
        /// <returns>The new XmlNode.</returns>
        XmlNode CreateNode(System.String nodeTypeString, System.String name, System.String namespaceURI);

        /// <summary>
        /// Creates a System.Xml.XmlNode with the specified System.Xml.XmlNodeType, System.Xml.XmlNode.Prefix, System.Xml.XmlDocument.Name, and System.Xml.XmlNode.NamespaceURI.
        /// </summary>
        /// <param name="type">The XmlNodeType of the new node.</param>
        /// <param name="prefix">The prefix of the new node.</param>
        /// <param name="name">The local name of the new node.</param>
        /// <param name="namespaceURI">The namespace URI of the new node.</param>
        /// <returns>The new XmlNode.</returns>
        XmlNode CreateNode(XmlNodeType type, System.String prefix, System.String name, System.String namespaceURI);

        /// <summary>
        /// Creates an System.Xml.XmlProcessingInstruction with the specified name and data.
        /// </summary>
        /// <param name="target">The name of the processing instruction.</param>
        /// <param name="data">The data for the processing instruction.</param>
        /// <returns>The new XmlProcessingInstruction.</returns>
        XmlProcessingInstruction CreateProcessingInstruction(System.String target, System.String data);

        /// <summary>
        /// Creates an System.Xml.XmlSignificantWhitespace node.
        /// </summary>
        /// <param name="text">The string must contain only the following characters #20; #10; #13; and #9;.</param>
        /// <returns>A new XmlSignificantWhitespace node.</returns>
        XmlSignificantWhitespace CreateSignificantWhitespace(System.String text);

        /// <summary>
        /// Creates an System.Xml.XmlText with the specified text.
        /// </summary>
        /// <param name="text">The text for the Text node.</param>
        /// <returns>The new XmlText node.</returns>
        XmlText CreateTextNode(System.String text);

        /// <summary>
        /// Creates an System.Xml.XmlWhitespace node.
        /// </summary>
        /// <param name="text">The string must contain only the following characters #20; #10; #13; and #9;.</param>
        /// <returns>A new XmlWhitespace node.</returns>
        XmlWhitespace CreateWhitespace(System.String text);
    }
}
