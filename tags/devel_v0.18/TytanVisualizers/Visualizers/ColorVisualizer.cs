using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.VisualStudio.DebuggerVisualizers;
using Pretorianie.Tytan.Forms;
using Pretorianie.Tytan.Visualizers;

[assembly: DebuggerVisualizer(typeof(ColorVisualizer), typeof(VisualizerObjectSource), Target = typeof(Color), Description = ColorVisualizer.Name)]

namespace Pretorianie.Tytan.Visualizers
{
    /// <summary>
    /// Visualizer class for Color struct instances.
    /// </summary>
    public class ColorVisualizer : DialogDebuggerVisualizer
    {
        /// <summary>
        /// Public name of this visualizer.
        /// </summary>
        public const string Name = "TytanNET - Color Visualizer";

        ///<param name="objectProvider">An object of type <see cref="T:Microsoft.VisualStudio.DebuggerVisualizers.IVisualizerObjectProvider"></see>. This object provides communication from the debugger side of the visualizer to the object source (<see cref="T:Microsoft.VisualStudio.DebuggerVisualizers.VisualizerObjectSource"></see>) on the debuggee side.</param>
        ///<param name="windowService">An object of type <see cref="T:Microsoft.VisualStudio.DebuggerVisualizers.IDialogVisualizerService"></see>, which provides methods your visualizer can use to display Windows forms, controls, and dialogs.</param>
        protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
        {
            Color data = (Color)objectProvider.GetObject();

            using (Form displayForm = new ColorVisualizerForm(data))
            {
                windowService.ShowDialog(displayForm);
            }
        }
    }
}
