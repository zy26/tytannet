using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Windows.Forms;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell.Interop;
using Pretorianie.Tytan.Core.DbgView;
using Pretorianie.Tytan.Core.Helpers;
using Pretorianie.Tytan.Core.Interfaces;
using Pretorianie.Tytan.Tools;
using Pretorianie.Tytan.Windows;

namespace Pretorianie.Tytan.Actions.Tools
{
    public class DebugViewPackageTool : IPackageToolWindow
    {
        private IPackageEnvironment parent;
        private DebugViewTool control;
        private Window window;

        private readonly Microsoft.VisualStudio.Shell.SelectionContainer selectionContainer;
        private readonly System.Collections.ArrayList selectedItems;
        private object[] selectedElements;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public DebugViewPackageTool()
        {
            selectionContainer = new Microsoft.VisualStudio.Shell.SelectionContainer();
            selectedItems = new System.Collections.ArrayList();
            selectedElements = new object[1];
        }

        #region Code Jumps

        void ItemSelected(DebugViewTool sender, int selectedItemIndex, DebugViewData data)
        {
            //IVsUIShell shell = parent.GetService(typeof(SVsUIShell)) as IVsUIShell;
            //Guid guidPropertyBrowser = new Guid(ToolWindowGuids.PropertyBrowser);
            //IVsWindowFrame frame = null;

            //if (shell != null)
            //    shell.FindToolWindow((uint)__VSFINDTOOLWIN.FTW_fForceCreate, ref guidPropertyBrowser, out frame);

            //if (frame != null)
            //    frame.ShowNoActivate();

            ITrackSelection trackSelection = parent.GetService(typeof(STrackSelection)) as ITrackSelection;
            if (trackSelection != null)
            {
                selectedItems.Clear();
                if (data != null)
                    selectedItems.Add(data);

                selectionContainer.SelectedObjects = selectedItems;
                trackSelection.OnSelectChange(selectionContainer);
            }
            else
            {
                if (data != null)
                {
                    selectedElements[0] = data;
                    window.SetSelectionContainer(ref selectedElements);
                    window.Activate();
                }
            }
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
        /// Gets the type of the Control that will be created dynamically and embedded inside IDE ActiveX host.
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
        public Window Window
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
            get { return typeof(DebugViewToolWindow); }
        }

        /// <summary>
        /// Gets or sets the instance of the control described by the type ControlType.
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
        /// This guid can be used for indexing the windows collection,
        /// for example: applicationObject.Windows.Item(guidstr).
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
        /// 0-based index of the 16x16 pixels bitmap within BitmapResourceID that will be used as TabImage.
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
        /// Performs initialization of this action and
        /// also registers all the UI elements required by the action, e.g.: menus / menu groups / toolbars.
        /// </summary>
        public void Initialize(IPackageEnvironment env, IMenuCommandService mcs, IMenuCreator mc)
        {
            CommandID cmdID = new CommandID(GuidList.guidCmdSet, ID);
            MenuCommand menu = new MenuCommand(Execute, cmdID);

            parent = env;
            mcs.AddCommand(menu);

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
