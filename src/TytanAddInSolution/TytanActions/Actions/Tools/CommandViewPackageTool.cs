using System;
using System.ComponentModel.Design;
using System.Windows.Forms;
using EnvDTE80;
using Pretorianie.Tytan.Core.Helpers;
using Pretorianie.Tytan.Core.Interfaces;
using Pretorianie.Tytan.Tools;
using Pretorianie.Tytan.Windows;

namespace Pretorianie.Tytan.Actions.Tools
{
    /// <summary>
    /// 
    /// </summary>
    public class CommandViewPackageTool : IPackageToolWindow
    {
        private IPackageEnvironment parent;
        private CommandViewTool control;
        private Window2 window;

        #region IPackageToolWindow Members

        /// <summary>
        /// Gets the type of the Control that will be created dynamically and embedded inside IDE ActiveX host.
        /// </summary>
        public Type ControlType
        {
            get { return typeof(CommandViewTool); }
        }

        /// <summary>
        /// Gets the caption of this tool window.
        /// </summary>
        public string Caption
        {
            get { return SharedStrings.ToolWindowTitle_CommandView; }
        }

        /// <summary>
        /// Gets or stores the IDE window of this tool.
        /// </summary>
        public Window2 Window
        {
            get { return window; }
            set { window = value; }
        }

        /// <summary>
        /// Gets if after creation the tool-window should be floating or docked.
        /// </summary>
        public bool IsFloating
        {
            get { return false; }
        }

        /// <summary>
        /// Type of the ToolWindowPane that can host the control.
        /// </summary>
        public Type ToolWindowPaneType
        {
            get { return typeof(CommandViewToolWindow); }
        }

        /// <summary>
        /// Gets or sets the instance of the control described by the type ControlType.
        /// </summary>
        public Control Control
        {
            get
            {
                if (control == null)
                {
                    control = new CommandViewTool();
                    control.CommandSelected += CommandSelected;
                }

                return control;
            }
            set
            {
                if (control == null)
                {
                    control = value as CommandViewTool;
                    if (control != null)
                        control.CommandSelected += CommandSelected;
                }
            }
        }

        /// <summary>
        /// The unique description of this tool window.
        /// This GUID can be used for indexing the windows collection,
        /// for example: applicationObject.Windows.Item(guidstr).
        /// </summary>
        public string Guid
        {
            get { return GuidList.guidToolWindow_CommandView; }
        }

        /// <summary>
        /// Resource ID of the bitmap with all the images for tool-windows.
        /// </summary>
        public int BitmapResourceID
        {
            get { return 301; }
        }

        /// <summary>
        /// 0-based index of the 16x16 pixels bitmap within BitmapResourceID that will be used as TabImage.
        /// </summary>
        public int BitmapIndex
        {
            get { return 5; }
        }

        #endregion

        #region IPackageAction Members

        /// <summary>
        /// Gets the unique ID of the action.
        /// </summary>
        public int ID
        {
            get { return PackageCmdIDList.toolCommandView; }
        }

        /// <summary>
        /// Performs initialization of this action and
        /// also registers all the UI elements required by the action, e.g.: menus / menu groups / toolbars.
        /// </summary>
        public void Initialize(IPackageEnvironment env, IMenuCommandService mcs, IMenuCreator mc)
        {
            MenuCommand menu = ObjectFactory.CreateCommand(GuidList.guidCmdSet, ID, Execute);

            parent = env;
            mcs.AddCommand(menu);

            // -------------------------------------------------------
            mc.AddCommand(menu, "CommandView", "&Command Browser", BitmapIndex, "Global::Ctrl+W, B", null, true);
            mc.Customizator.AddAuxToolWindow("View", menu, false, 9, "Properties Window");
        }

        /// <summary>
        /// Invokes proper processing assigned to current action.
        /// </summary>
        public void Execute(object sender, EventArgs e)
        {
            if (parent.ShowToolWindow(this))
                control.RefreshInfos(parent.DTE);
        }
        
        void CommandSelected(CommandViewTool sender, Core.Data.CommandInfo item)
        {
            // select given command inside Property Window:
            parent.SelectAtProperties(window, item);
        }

        #endregion

        #region IDisposable Members

        ///<summary>
        ///Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        ///</summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
