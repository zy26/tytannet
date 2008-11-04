using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.VisualStudio.DebuggerVisualizers;
using Pretorianie.Tytan.Forms;
using Pretorianie.Tytan.ObjectSources;
using Pretorianie.Tytan.Visualizers;

[assembly: DebuggerVisualizer(typeof(XmlVisualizer), typeof(VisualizerObjectSource), Target = typeof(System.String), Description = XmlVisualizer.Name)]
[assembly: DebuggerVisualizer(typeof(XmlVisualizer), typeof(XmlObjectSource), Target = typeof(System.Xml.XmlDocument), Description = XmlVisualizer.Name)]

namespace Pretorianie.Tytan.Visualizers
{
    /// <summary>
    /// Visualizer class for XmlDocument objects.
    /// </summary>
    public class XmlVisualizer : DialogDebuggerVisualizer
    {
        /// <summary>
        /// Public name of this visualizer.
        /// </summary>
        public const string Name = "TytanNET - XML Visualizer";

        protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
        {
            string data = objectProvider.GetObject() as string;

            if (data == null)
                return;

            using (Form displayForm = new XmlVisualizerForm(data))
            {
                windowService.ShowDialog(displayForm);
            }
        }
    }
}