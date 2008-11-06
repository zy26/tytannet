using System;
using System.ComponentModel.Design;
using System.IO;
using EnvDTE;
using Pretorianie.Tytan.Core.Data;
using Pretorianie.Tytan.Core.Helpers;
using Pretorianie.Tytan.Core.Interfaces;

namespace Pretorianie.Tytan.Actions.Misc
{
    /// <summary>
    /// Action that allows to open Windows Explorer with selected element from Solution Explorer.
    /// </summary>
    public sealed class OpenWindowsExplorer : IPackageAction
    {
        private IPackageEnvironment parent;

        #region OpenFolder

        private static void OpenFolder(FileInfo folder)
        {
            if (folder != null && folder.Directory != null
                && !string.IsNullOrEmpty(folder.Directory.FullName))
            {
                string args;
                if (string.IsNullOrEmpty(folder.Name))
                    args = "/n,/e,\"" + folder.Directory.FullName + "\"";
                else
                    args = "/n,/e,/select,\"" + folder.FullName + "\"";

                System.Diagnostics.Process.Start("explorer.exe", args);
            }
        }

        private static FileInfo GetFolderPath(UIHierarchyItem s)
        {
            string path = GetFilePath(s.Object);

            if (string.IsNullOrEmpty(path))
                return null;
            return new FileInfo(path);
        }

        private static FileInfo GetFolderPath(ProjectItem i)
        {
            string path = GetFilePath(i);

            if(string.IsNullOrEmpty(path))
                return null;
            return new FileInfo(path);
        }

        private static string GetFilePath(object o)
        {
            if (o is Solution)
            {
                return (o as Solution).FullName;
            }
            if (o is Project)
            {
                Project project = o as Project;

                if (IsWebProject(project))
                    return project.Properties.Item("FullPath").Value + "\\";
                return project.FullName;
            }

            if (o is ProjectItem)
            {
                ProjectItem item = o as ProjectItem;

                if (item.SubProject != null)
                    return GetFilePath(item.SubProject);
                return item.Properties.Item("FullPath").Value.ToString(); // item.get_FileNames(0); 
            }

            return null;
        }

        private static bool IsWebProject(Project project)
        {
            foreach (Property p in project.Properties)
            {
                if (p.Name == "OpenedURL")
                    return true;
            }

            return false;
        }

        #endregion

        #region IPackageAction Members

        /// <summary>
        /// Gets the unique ID of the action.
        /// </summary>
        public int ID
        {
            get { return PackageCmdIDList.toolOpenFolder; }
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
            mc.AddCommand(menu, "OpenWindowsExplorer", "Open &Folder...", 9008, null, null, false);
            mc.Customizator.AddSolutionExplorerItem(menu, true, false, 1, "Exclude From Project",
                                                    "Set as StartUp Project", "Add", "Copy Full Path");
        }

        /// <summary>
        /// Invokes proper processing assigned to current action.
        /// </summary>
        public void Execute(object sender, EventArgs e)
        {
            // check if opening from active document window:
            if (parent.DTE.ActiveWindow.ProjectItem != null)
            {
                OpenFolder(GetFolderPath(parent.DTE.ActiveWindow.ProjectItem));
            }
            else
            {
                // or from SolutionExplorer:
                UIHierarchy hierarchy = parent.DTE.ToolWindows.SolutionExplorer;

                foreach (UIHierarchyItem s in (object[]) hierarchy.SelectedItems)
                    OpenFolder(GetFolderPath(s));
            }
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