using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.VisualStudio.DebuggerVisualizers;
using Pretorianie.Tytan.Forms;
using Pretorianie.Tytan.Visualizers;
using Pretorianie.Tytan.Utils;
using Pretorianie.Tytan.ObjectSources;

[assembly: DebuggerVisualizer(typeof(ImageListVisualizer), typeof(VisualizerObjectSource), Target = typeof(ImageListStreamer), Description = ImageListVisualizer.Name)]
[assembly: DebuggerVisualizer(typeof(ImageListVisualizer), typeof(ImageListObjectSource), Target = typeof(ImageList), Description = ImageListVisualizer.Name)]

namespace Pretorianie.Tytan.Visualizers
{
    /// <summary>
    /// Visualizer class for Stream property of ImageList control.
    /// It is like this because Stream property is the only one serializable element of that control.
    /// </summary>
    public class ImageListVisualizer : DialogDebuggerVisualizer
    {
        /// <summary>
        /// Public name of this visualizer.
        /// </summary>
        public const string Name = "TytanNET - ImageList Visualizer";

        protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
        {
            ImageListStreamer data = objectProvider.GetObject() as ImageListStreamer;
            ImageList list;

            if (data == null)
                return;
            else
            {
                list = new ImageList();
                list.ImageStream = data;
            }

            using (Form displayForm = new ImageVisualizerForm(list.Images, list.ImageSize, ImageHelper.AdjustSize(list.ImageSize.Width)))
            {
                windowService.ShowDialog(displayForm);
            }
        }
    }
}