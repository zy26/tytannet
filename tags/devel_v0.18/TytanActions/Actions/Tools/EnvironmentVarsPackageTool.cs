using System;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using EnvDTE80;
using Pretorianie.Tytan.Core.BaseForms;
using Pretorianie.Tytan.Core.Data;
using Pretorianie.Tytan.Core.Helpers;
using Pretorianie.Tytan.Core.Interfaces;
using Pretorianie.Tytan.Tools;

namespace Pretorianie.Tytan.Actions.Tools
{
    /// <summary>
    /// Custom tool window that edits environment variables for: current process, user and whole system.
    /// </summary>
    public class EnvironmentVarsPackageTool : IPackageToolWindow
    {
        private IPackageEnvironment parent;
        private Window2 window;
        private EnvironmentVarsTool control;

        #region WindowPane

        [Guid(GuidList.guidToolWindow_EnvironmentVarView)]
        public class EnvironmentVarsToolWindow : BaseToolWindowPane<EnvironmentVarsPackageTool>
        {
        }

        #endregion

        #region IPackageToolWindow Members

        /// <summary>
        /// Gets the type of the Control that will be created dynamically and embedded inside IDE <c>ActiveX</c> host.
        /// </summary>
        public Type ControlType
        {
            get { return typeof(EnvironmentVarsTool); }
        }

        /// <summary>
        /// Gets the caption of this tool window.
        /// </summary>
        public string Caption
        {
            get { return SharedStrings.ToolWindowTitle_EnvironmentVars; }
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
        /// Type of the <c>ToolWindowPane</c> that can host the control.
        /// </summary>
        public Type ToolWindowPaneType
        {
            get { return typeof(EnvironmentVarsToolWindow); }
        }

        /// <summary>
        /// Gets or sets the instance of the control described by the type <c>ControlType</c>.
        /// </summary>
        public Control Control
        {
            get
            {
                if (control == null)
                    control = new EnvironmentVarsTool();

                return control;
            }
            set
            {
                if (control == null)
                    control = value as EnvironmentVarsTool;
            }
        }

        /// <summary>
        /// The unique description of this tool window.
        /// This guid can be used for indexing the windows collection,
        /// for example: applicationObject.Windows.Item(GuidString).
        /// </summary>
        public string Guid
        {
            get { return GuidList.guidToolWindow_EnvironmentVarView; }
        }

        /// <summary>
        /// Resource ID of the bitmap with all the images for tool-windows.
        /// </summary>
        public int BitmapResourceID
        {
            get { return 301; }
        }

        /// <summary>
        /// 0-based index of the 16x16 pixels bitmap within <c>BitmapResourceID</c> that will be used as TabImage.
        /// </summary>
        public int BitmapIndex
        {
            get { return 2; }
        }

        #endregion

        #region IPackageAction Members

        /// <summary>
        /// Gets the unique ID of the action.
        /// </summary>
        public int ID
        {
            get { return PackageCmdIDList.toolEnvironmentVarView; }
        }

        /// <summary>
        /// Gets the current valid configuration for the action. In case of
        /// null-value no settings are actually needed at all.
        /// 
        /// Set is executed at runtime when the configuration for
        /// given action is updated via external module (i.e. Tools->Options).
        /// </summary>
        public PersistentStorageData Configuration
        {
            get { return null; }
            set { }
        }

        /// <summary>
        /// Performs initialization of this action and
        /// also registers all the UI elements required by the action, e.g.: menus / menu groups / toolbars.
        /// </summary>
        public void Initialize(IPackageEnvironment env, IMenuCreator mc)
        {
            MenuCommand menu = ObjectFactory.CreateCommand(GuidList.guidCmdSet, ID, Execute);

            parent = env;

            // -------------------------------------------------------
            mc.AddCommand(menu, "EnvironmentVariablesView", "Variabl&es View", BitmapIndex, "Global::Ctrl+W, I", null, true);
            mc.Customizator.AddToolWindow("View", menu, false, 3, "Class View");
        }

        /// <summary>
        /// Invokes proper processing assigned to current action.
        /// </summary>
        public void Execute(object sender, EventArgs e)
        {
            parent.ShowToolWindow(this);
        }

        /// <summary>
        /// Executed on Visual Studio exit.
        /// All non-managed resources should be released here.
        /// </summary>
        public void Destroy()
        {
        }

        #endregion
    }
}
