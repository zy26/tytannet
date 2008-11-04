using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.VisualStudio.DebuggerVisualizers;
using Pretorianie.Tytan.Forms;
using Pretorianie.Tytan.ObjectSources;
using Pretorianie.Tytan.Visualizers;

[assembly: System.Diagnostics.DebuggerVisualizer(typeof(SystemComVisualizer), typeof(SystemComObjectSource), TargetTypeName = "System.__ComObject, mscorlib", Description = SystemComVisualizer.Name)]

namespace Pretorianie.Tytan.Visualizers
{
    public class SystemComVisualizer : DialogDebuggerVisualizer
    {
        /// <summary>
        /// Public name of this visualizer.
        /// </summary>
        public const string Name = "TytanNET - COM Object Visualizer";

        ///<param name="objectProvider">An object of type <see cref="T:Microsoft.VisualStudio.DebuggerVisualizers.IVisualizerObjectProvider"></see>. This object provides communication from the debugger side of the visualizer to the object source (<see cref="T:Microsoft.VisualStudio.DebuggerVisualizers.VisualizerObjectSource"></see>) on the debuggee side.</param>
        ///<param name="windowService">An object of type <see cref="T:Microsoft.VisualStudio.DebuggerVisualizers.IDialogVisualizerService"></see>, which provides methods your visualizer can use to display Windows forms, controls, and dialogs.</param>
        protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
        {
            IList<Type> data = (IList<Type>)objectProvider.GetObject();

            using (Form displayForm = new SystemComVisualizerForm(data))
            {
                windowService.ShowDialog(displayForm);
            }
        }
    }
}
