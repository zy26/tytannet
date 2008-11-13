using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using EnvDTE;
using EnvDTE80;
using Pretorianie.Tytan.Core.BaseForms;
using Pretorianie.Tytan.Core.Data;
using Pretorianie.Tytan.Core.DbgView;
using Pretorianie.Tytan.Core.Helpers;
using Pretorianie.Tytan.Core.Interfaces;
using Pretorianie.Tytan.Tools;

namespace Pretorianie.Tytan.Actions.Tools
{
    /// <summary>
    /// Custom tool that captures debug messages.
    /// </summary>
    public class DebugViewPackageTool : IPackageToolWindow
    {
        private IPackageEnvironment parent;
        private DebugViewTool control;
        private Window2 window;

        #region WindowPane

        [Guid(GuidList.guidToolWindow_DebugView)]
        public class DebugViewToolWindow : BaseToolWindowPane<DebugViewPackageTool>
        {
        }

        #endregion

        #region Code Jumps

        void ItemSelected(DebugViewTool sender, int selectedItemIndex, DebugViewData data)
        {
            parent.SelectAtProperties(window, data);
        }

        void ItemCodeJump(DebugViewTool sender, DebugViewData data, DbgViewCodeJumpStyle style)
        {
            bool found = false;
            DTE2 applicationObject = parent.DTE;

            if (applicationObject == null)
                throw new ArgumentNullException(null, "Invalid VisualStudio IDE application object.");

            // get the projects:
            int projectCount = applicationObject.Solution.Projects.Count;
            IList<Project> projects = new List<Project>();

            for (int p = 1; p <= projectCount; p++)
                projects.Add(applicationObject.Solution.Projects.Item(p));

            // search:
            if (style != DbgViewCodeJumpStyle.Automatic && style != DbgViewCodeJumpStyle.Autodetect)
            {
                found = SolutionHelper.Activate(projects, SolutionHelper.GetFinder(data.Message, style));
            }
            else
            {
                IList<DbgViewCodeJumpStyle> list = new List<DbgViewCodeJumpStyle>();

                list.Add(DbgViewCodeJumpStyle.Class_12);
                list.Add(DbgViewCodeJumpStyle.Class_23);
                list.Add(DbgViewCodeJumpStyle.Class_34);
                list.Add(DbgViewCodeJumpStyle.Class_45);
                list.Add(DbgViewCodeJumpStyle.Class_56);
                list.Add(DbgViewCodeJumpStyle.Class_1);
                list.Add(DbgViewCodeJumpStyle.Class_2);
                list.Add(DbgViewCodeJumpStyle.Class_3);
                list.Add(DbgViewCodeJumpStyle.Class_4);
                list.Add(DbgViewCodeJumpStyle.Class_5);

                // try guess the style:
                foreach (DbgViewCodeJumpStyle e in list)
                {
                    if (SolutionHelper.Activate(projects, SolutionHelper.GetFinder(data.Message, e)))
                    {
                        if (style == DbgViewCodeJumpStyle.Autodetect)
                        {
                            control.SetStyle(e);
                        }

                        found = true;
                        break;
                    }
                }
            }

            if (!found)
            {
                MessageBox.Show("Specific element doesn't contain reference to the code inside current solution.", DebugViewTool.DialogTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        #endregion

        #region IPackageToolWindow Members

        /// <summary>
        /// Gets the type of the Control that will be created dynamically and embedded inside IDE <c>ActiveX</c> host.
        /// </summary>
        public Type ControlType
        {
            get { return typeof(DebugViewTool); }
        }

        /// <summary>
        /// Gets the caption of this tool window.
        /// </summary>
        public string Caption
        {
            get { return SharedStrings.ToolWindowTitle_DebugView; }
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
            get { return typeof(DebugViewToolWindow); }
        }

        /// <summary>
        /// Gets or sets the instance of the control described by the type <c>ControlType</c>.
        /// </summary>
        public Control Control
        {
            get
            {
                if(control == null)
                {
                    control = new DebugViewTool();
                    control.VisibleCodeJump = true;
                    control.ItemCodeJump += ItemCodeJump;
                    control.ItemSelected += ItemSelected;
                }

                 return control;
            }
            set
            {
                if (control == null)
                {
                    control = value as DebugViewTool;
                    if(control != null)
                    {
                        control.VisibleCodeJump = true;
                        control.ItemCodeJump += ItemCodeJump;
                        control.ItemSelected += ItemSelected;
                    }
                }
            }
        }

        /// <summary>
        /// The unique description of this tool window.
        /// This GUID can be used for indexing the windows collection,
        /// for example: applicationObject.Windows.Item(GuidString).
        /// </summary>
        public string Guid
        {
            get { return GuidList.guidToolWindow_DebugView; }
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
            get { return 3; }
        }

        #endregion

        #region IPackageAction Members

        /// <summary>
        /// Gets the unique ID of the action.
        /// </summary>
        public int ID
        {
            get { return PackageCmdIDList.toolDebugView; }
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
            mc.AddCommand(menu, "DebugOutputView", "&Debug Output", BitmapIndex, "Global::Ctrl+W, V", null, true);
            mc.Customizator.AddToolWindow("View", menu, false, 8, "Output");
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
            if (control != null)
                control.SaveState();
        }

        #endregion
    }
}
