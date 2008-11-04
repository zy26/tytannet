using System.IO;
using Microsoft.VisualStudio.DebuggerVisualizers;
using System.Windows.Forms;
using Pretorianie.Tytan.Helpers;

namespace Pretorianie.Tytan.ObjectSources
{
    /// <summary>
    /// Class that coordinates data transfer.
    /// </summary>
    public class ImageListObjectSource : VisualizerObjectSource
    {
        /// <summary>
        /// Serialize data from the debugee process into the debugger.
        /// </summary>
        public override void GetData(object target, Stream outgoingData)
        {
            ImageList list = target as ImageList;
            ImageListStreamer imageStream = (list != null ? list.ImageStream : null);

            // serialize:
            SerializationHelper.WriteAsBinary(outgoingData, imageStream);
        }
    }
}
