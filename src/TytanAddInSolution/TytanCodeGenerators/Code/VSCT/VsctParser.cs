using System.Collections.Generic;
using System.Xml;

namespace Pretorianie.Tytan.Code.VSCT
{
    /// <summary>
    /// Class that parses the given XML content to retrieve item IDs and GUIDs.
    /// </summary>
    class VsctParser
    {
        /// <summary>
        /// Extract GUIDs and IDs descriptions from given XML content.
        /// </summary>
        public static void Parse(string vsctContentFile, out IList<NamedValue> guids, out IList<NamedValue> ids)
        {
            XmlDocument xml = new XmlDocument();
            XmlElement symbols = null;

            guids = null;
            ids = null;

            try
            {
                xml.LoadXml(vsctContentFile);

                // having XML loaded go throu and find:
                // CommandTable / Symbols / GuidSymbol* / IDSymbol*
                if (xml.DocumentElement.Name == "CommandTable")
                    symbols = xml.DocumentElement["Symbols"];
            }
            catch
            {
                return;
            }

            if (symbols != null)
            {
                XmlNodeList guidSymbols = symbols.GetElementsByTagName("GuidSymbol");

                guids = new List<NamedValue>();
                ids = new List<NamedValue>();

                foreach (XmlElement g in guidSymbols)
                {
                    NamedValue gValue;
                    try
                    {
                        // go throu all GuidSymbol elements...
                        guids.Add(gValue = new NamedValue(g.Attributes["name"].Value, g.Attributes["value"].Value));
                    }
                    catch
                    {
                        gValue = null;
                    }

                    XmlNodeList idSymbols = g.GetElementsByTagName("IDSymbol");
                    if (idSymbols != null)
                    {
                        foreach (XmlElement i in idSymbols)
                        {
                            try
                            {
                                // go throu all IDSymbol elements...
                                ids.Add(new NamedValue(i.Attributes["name"].Value, i.Attributes["value"].Value, gValue));
                            }
                            catch
                            {
                            }
                        }
                    }
                }
            }
        }
    }
}
