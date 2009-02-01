using System;
using System.Collections;
using System.IO;
using System.Xml;
using Pretorianie.Tytan.Core.Interfaces;

namespace Pretorianie.Tytan.Core.Helpers
{
    /// <summary>
    /// Helper class for XML DOM objects.
    /// </summary>
    public static class XmlHelper
    {
        #region Public Constraints

        /// <summary>
        /// Constant value for 'boolean true'.
        /// </summary>
        public static readonly string True = "true";
        /// <summary>
        /// Constant value for 'boolean false'.
        /// </summary>
        public static readonly string False = "false";

        #endregion

        #region Append

        /// <summary>
        /// Appends all elements of enumeration into specified document. They need to implement at least <see cref="IXmlConvertible"/>.
        /// </summary>
        public static int Append(IXmlDocument doc, XmlNode node, IEnumerable items, string comment, bool printComment, object runtimeParams)
        {
            return items != null
                       ? Append(doc, node, items.GetEnumerator(), comment, printComment, runtimeParams)
                       : 0;
        }

        /// <summary>
        /// Appends all elements of enumeration into specified document. They need to implement at least <see cref="IXmlConvertible"/>.
        /// </summary>
        public static int Append(IXmlDocument doc, XmlNode node, IEnumerable items, string comment, bool printComment, string childNodeName, string childNodeAttribute)
        {
            return items != null
                       ? Append(doc, node, items.GetEnumerator(), comment, printComment, childNodeName, childNodeAttribute)
                       : 0;
        }

        /// <summary>
        /// Appends all elements of enumeration into specified document. They need to implement at least <see cref="IXmlConvertible"/>.
        /// </summary>
        public static int Append(IXmlDocument doc, XmlNode node, IEnumerator items, string comment, bool printComment, object runtimeParams)
        {
            int counter = 0;

            // add comment if needed:
            if (printComment)
                node.AppendChild(doc.CreateComment(comment));

            // go throu all the elements and serialize:
            if (items != null)
            {
                items.Reset();
                while (items.MoveNext())
                {
                    XmlNode elem = ((IXmlConvertible) items.Current).ToXmlNode(doc, runtimeParams);

                    // and if serialization was successful:
                    if (elem != null)
                    {
                        node.AppendChild(elem);
                        counter++;
                    }
                }
            }

            return counter;
        }

        /// <summary>
        /// Appends all elements of enumeration into specified document. They need to implement at least <see cref="IXmlConvertible"/>.
        /// </summary>
        public static int Append(IXmlDocument doc, XmlNode node, IEnumerator items, string comment, bool printComment, string childNodeName, string childNodeAttribute)
        {
            int counter = 0;

            // add comment:
            if (printComment)
                node.AppendChild(doc.CreateComment(comment));

            // go throu all the elements and serialize them:
            if (items != null)
            {
                items.Reset();
                while (items.MoveNext())
                {
                    XmlNode pElem = doc.CreateElement(childNodeName);
                    AddAttribute(doc, pElem, childNodeAttribute, items.Current.ToString());
                    node.AppendChild(pElem);
                    counter++;
                }
            }

            return counter;
        }

        #endregion

        #region Collect

        /// <summary>
        /// Reads values of given attribute inside specified child nodes.
        /// </summary>
        public static int Collect(XmlNode node, string childElementTagName, string childElementAttribute, out string[] attributeValues)
        {
            int counter = 0;
            int i = 0;

            // if input is correct:
            if (node != null && node.ChildNodes.Count > 0)
            {
                // visit all elements with specified name
                // and process them:
                foreach (XmlNode child in node.ChildNodes)
                    if (child.Name == childElementTagName) counter++;

                // generate results:
                attributeValues = new string[counter];
                foreach (XmlNode child in node.ChildNodes)
                    if (child.Name == childElementTagName)
                        ReadAttribute(child, childElementAttribute, ref attributeValues[i++]);
            }
            else
                attributeValues = null;

            return counter;
        }

        #endregion

        #region AddAttribute

        /// <summary>
        /// Adds new attribute to given element.
        /// </summary>
        public static bool AddAttribute(IXmlDocument doc, XmlNode node, string name, string value)
        {
            XmlAttribute attribute = doc.CreateAttribute(name);

            if (attribute != null)
            {
                attribute.Value = value;
                node.Attributes.Append(attribute);
                return true;
            }

            return false;
        }

        #endregion

        #region ReadAttribute

        /// <summary>
        /// Reads the value of given attribute are stores the result into output parameter.
        /// Returns 'true' when success.
        /// </summary>
        public static bool ReadAttribute(XmlNode node, string attributeName, ref double value)
        {
            XmlAttribute attribute = (XmlAttribute) node.Attributes.GetNamedItem(attributeName);

            if (attribute != null && !string.IsNullOrEmpty(attribute.Value))
            {
                try
                {
                    value = double.Parse(attribute.Value);
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            return false;
        }

        /// <summary>
        /// Reads the value of given attribute are stores the result into output parameter.
        /// Returns 'true' when success.
        /// </summary>
        public static bool ReadAttribute(XmlNode node, string attributeName, ref bool value)
        {
            XmlAttribute attribute = (XmlAttribute) node.Attributes.GetNamedItem(attributeName);

            if (attribute != null && !string.IsNullOrEmpty(attribute.Value))
            {
                try
                {
                    value = attribute.Value == "1" || bool.Parse(attribute.Value);
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            return false;
        }

        /// <summary>
        /// Reads the value of given attribute are stores the result into output parameter.
        /// Returns 'true' when success.
        /// </summary>
        public static bool ReadAttribute(XmlNode node, string attributeName, ref string value)
        {
            XmlAttribute attribute = (XmlAttribute) node.Attributes.GetNamedItem(attributeName);

            if (attribute != null && attribute.Value != null)
            {
                value = attribute.Value;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Reads the value of given attribute are stores the result into output parameter.
        /// Returns 'true' when success.
        /// </summary>
        public static bool ReadAttribute(XmlNode node, string attributeName, ref int value)
        {
            XmlAttribute attribute = (XmlAttribute) node.Attributes.GetNamedItem(attributeName);

            if (attribute != null && !string.IsNullOrEmpty(attribute.Value))
            {
                try
                {
                    value = Int32.Parse(attribute.Value);
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            return false;
        }

        /// <summary>
        /// Reads the value of given attribute are stores the result into output parameter.
        /// Returns 'true' when success.
        /// </summary>
        public static bool ReadAttribute(XmlNode node, string attributeName, ref long value)
        {
            XmlAttribute attribute = (XmlAttribute) node.Attributes.GetNamedItem(attributeName);

            if (attribute != null && !string.IsNullOrEmpty(attribute.Value))
            {
                try
                {
                    value = Int64.Parse(attribute.Value);
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            return false;
        }

        #endregion

        #region ToString

        /// <summary>
        /// Converts standard XML document into a string with proper formatting.
        /// </summary>
        public static string ToString(XmlDocument doc, bool useFormatting)
        {
            if (doc != null)
            {
                Stream stream = new MemoryStream();
                XmlTextWriter writer = new XmlTextWriter(stream, System.Text.Encoding.UTF8);

                // update writer if formatting should be enabled:
                if (useFormatting)
                {
                    writer.Formatting = Formatting.Indented;
                    writer.Indentation = 2;
                    writer.IndentChar = ' ';
                    writer.QuoteChar = '"';
                }

                doc.WriteContentTo(writer);
                writer.Flush();

                StreamReader reader = new StreamReader(stream);
                stream.Seek(0, SeekOrigin.Begin);
                string line = reader.ReadToEnd();
                reader.Close();
                writer.Close();
                return line;
            }

            return string.Empty;
        }

        #endregion
    }
}