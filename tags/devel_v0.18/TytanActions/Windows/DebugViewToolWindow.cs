using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.VisualStudio.Shell;
using Pretorianie.Tytan.Actions.Tools;
using Pretorianie.Tytan.Core.Interfaces;

namespace Pretorianie.Tytan.Windows
{
    /// <summary>
    /// This class implements the tool window exposed by this package and hosts a user control.
    ///
    /// In Visual Studio tool windows are composed of a frame (implemented by the shell) and a pane, 
    /// usually implemented by the package implementer.
    ///
    /// This class derives from the ToolWindowPane class provided from the MPF in order to use its 
    /// implementation of the IVsWindowPane interface.
    /// </summary>
    [Guid(GuidList.guidToolWindow_DebugView)]
    public class DebugViewToolWindow : ToolWindowPane
    {
        private IPackageToolWindow tool;

        /// <summary>
        /// Standard constructor for the tool window.
        /// </summary>
        public DebugViewToolWindow()
            : base(null)
        {
            tool = new DebugViewPackageTool();
            Caption = tool.Caption;
            BitmapResourceID = tool.BitmapResourceID;
            BitmapIndex = tool.BitmapIndex;
        }

        /// <summary>
        /// This property returns the handle to the user control that should
        /// be hosted in the Tool Window.
        /// </summary>
        override public IWin32Window Window
        {
            get
            {
                return tool.Control;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (tool != null)
            {
                tool.Dispose();
                tool = null;
            }

            base.Dispose(disposing);
        }
    }
}
