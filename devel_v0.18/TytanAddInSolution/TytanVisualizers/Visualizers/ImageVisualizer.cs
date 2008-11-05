using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.VisualStudio.DebuggerVisualizers;
using Pretorianie.Tytan.Forms;
using Pretorianie.Tytan.Visualizers;
using Pretorianie.Tytan.Helpers;

[assembly: DebuggerVisualizer(typeof(ImageVisualizer),typeof(VisualizerObjectSource), Target=typeof(Image), Description=ImageVisualizer.Name)]

namespace Pretorianie.Tytan.Visualizers
{
    /// <summary>
    /// Visualizer class for all types of images.
    /// </summary>
    public class ImageVisualizer : DialogDebuggerVisualizer
    {
        /// <summary>
        /// Public name of this visualizer.
        /// </summary>
        public const string Name = "TytanNET - Image Visualizer";

        protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
        {
            Image data = (Image)objectProvider.GetObject();
            
            if (data == null)
                return;

            using (Form displayForm = new ImageVisualizerForm(data, ImageHelper.AdjustSize(data.Width)))
            {
                windowService.ShowDialog(displayForm);
            }
        }
    }
}