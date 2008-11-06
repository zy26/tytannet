using System.Windows.Forms;
using Microsoft.VisualStudio.Shell;
using Pretorianie.Tytan.Core.Interfaces;

namespace Pretorianie.Tytan.Core.BaseForms
{
    /// <summary>
    /// This class implements the tool window exposed by this package and hosts a user control.
    ///
    /// In Visual Studio tool windows are composed of a frame (implemented by the shell) and a pane, 
    /// usually implemented by the package implementer.
    ///
    /// This class derives from the <c>ToolWindowPane</c> class provided from the MPF in order to use its 
    /// implementation of the <c>IVsWindowPane</c> interface.
    /// </summary>
    public class BaseToolWindowPane<T> : ToolWindowPane where T : class, IPackageToolWindow, new()
    {
        private IPackageToolWindow tool;

        /// <summary>
        /// Standard constructor for the tool window.
        /// </summary>
        public BaseToolWindowPane()
            : base(null)
        {
            tool = new T();
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
                tool.Destroy();
                tool = null;
            }

            base.Dispose(disposing);
        }
    }
}
