using System.IO;
using System.Xml;
using Microsoft.VisualStudio.DebuggerVisualizers;
using Pretorianie.Tytan.Helpers;

namespace Pretorianie.Tytan.ObjectSources
{
    /// <summary>
    /// Class that coordinates data transfer.
    /// </summary>
    public class XmlObjectSource : VisualizerObjectSource
    {
        /// <summary>
        /// Serialize data from the debugee process into the debugger.
        /// </summary>
        public override void GetData(object target, Stream outgoingData)
        {
            XmlDocument xml = target as XmlDocument;
            string innerXML = (xml != null ? xml.InnerXml : null);

            // serialize:
            SerializationHelper.WriteAsBinary(outgoingData, innerXML);
        }
    }
}