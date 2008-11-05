using System;
using System.Windows.Forms;
using EnvDTE80;

namespace Pretorianie.Tytan.Core.Interfaces
{
    /// <summary>
    /// Interface implemented by package actions, which delivers tool window.
    /// </summary>
    public interface IPackageToolWindow : IPackageAction
    {
        #region VisualStudio AddIn Required

        /// <summary>
        /// Gets the type of the Control that will be created dynamically and embedded inside IDE ActiveX host.
        /// </summary>
        Type ControlType
        { get; }

        /// <summary>
        /// Gets the caption of this tool window.
        /// </summary>
        string Caption
        { get; }

        /// <summary>
        /// Gets or stores the IDE window of this tool.
        /// </summary>
        Window2 Window
        { get; set; }

        /// <summary>
        /// Gets if after creation the tool-window should be floating or docked.
        /// </summary>
        bool IsFloating
        { get; }

        #endregion

        #region VisualStudio Package Required

        /// <summary>
        /// Type of the ToolWindowPane that can host the control.
        /// </summary>
        Type ToolWindowPaneType
        { get; }

        /// <summary>
        /// Gets or sets the instance of the control described by the type ControlType.
        /// </summary>
        Control Control
        { get; set; }

        /// <summary>
        /// The unique description of this tool window.
        /// This guid can be used for indexing the windows collection,
        /// for example: applicationObject.Windows.Item(guidstr).
        /// </summary>
        string Guid
        { get; }

        /// <summary>
        /// Resource ID of the bitmap with all the images for tool-windows.
        /// </summary>
        int BitmapResourceID
        { get; }

        /// <summary>
        /// 0-based index of the 16x16 pixels bitmap within BitmapResourceID that will be used as TabImage.
        /// </summary>
        int BitmapIndex
        { get; }

        #endregion
    }
}
