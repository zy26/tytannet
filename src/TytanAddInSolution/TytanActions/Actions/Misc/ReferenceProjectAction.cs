using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using EnvDTE;
using VSLangProj;
using Microsoft.VisualStudio.CommandBars;
using Pretorianie.Tytan.Core.Data;
using Pretorianie.Tytan.Core.Helpers;
using Pretorianie.Tytan.Core.Interfaces;
using Pretorianie.Tytan.Core.Comparers;
using Pretorianie.Tytan.Core.Events;

namespace Pretorianie.Tytan.Actions.Misc
{
    /// <summary>
    /// Class defining action for Quick-Project-Referencing.
    /// </summary>
    public class ReferenceProjectAction : IPackageAction
    {
        private IPackageEnvironment parent;
        private SolutionEventsListener solutionListener;
        //private ShellEventsListener shellListener;
        private IMenuCreator mc;
        private IList<CommandBarPopup> vsParentPopups;
        private readonly Dictionary<Project, MenuCommand> vsCommands = new Dictionary<Project, MenuCommand>();
        private readonly Dictionary<string, MenuCommand> vsNames = new Dictionary<string, MenuCommand>();
        private readonly IComparer<string> namespaceComparer = new NamespaceComparer('.');
        private OpenFileDialog dlgBrowse;

        public const string CofigurationName = "ReferenceProjectAction";
        private const string Persistent_ForbiddenNames = "ForbiddenNames";
        public const string Persistent_SystemAssemblies = "SystemAssemblies";

        private string[] ForbiddenNames = 
            new string[] { "microsoft", "system", "mscorlib" };

        private string[] SystemAssemblies;
        private string[] DefaultSystemAssemblies =
            new string[] {"System.Data.dll", "System.Drawing.dll", "System.Windows.Forms.dll", "System.Xml.dll"};

        private const string Property_ReferenceProject = "ReferenceProject";
        private const string Property_ReferencePath = "ReferenceAssembly";

        private const string ForbiddenPrefix = "microsoft.";
        private const int ForbiddenPrefixLength = 10;

        #region Dummy Project Implementation

        private class DummyProject : Project
        {
            private string name;

            public DummyProject(string name)
            {
                this.name = name;
            }

            #region Project Implementation

            public void SaveAs(string newFileName)
            {
            }

            public void Save(string fileName)
            {
            }

            public void Delete()
            {
            }

            public string Name
            {
                get { return name; }
                set { name = value; }
            }

            public string FileName
            {
                get { return string.Empty; }
            }

            public bool IsDirty
            {
                get { return false; }
                set { }
            }

            public Projects Collection
            {
                get { return null; }
            }

            public DTE DTE
            {
                get { return null; }
            }

            public string Kind
            {
                get { return string.Empty; }
            }

            public ProjectItems ProjectItems
            {
                get { return null; }
            }

            public Properties Properties
            {
                get { return null; }
            }

            public string UniqueName
            {
                get { return string.Empty; }
            }

            public object Object
            {
                get { return null; }
            }

            public object ExtenderNames
            {
                get { return null; }
            }

            public string ExtenderCATID
            {
                get { return string.Empty; }
            }

            public string FullName
            {
                get { return string.Empty; }
            }

            public bool Saved
            {
                get { return true; }
                set { }
            }

            public ConfigurationManager ConfigurationManager
            {
                get { return null; }
            }

            public Globals Globals
            {
                get { return null; }
            }

            public ProjectItem ParentProjectItem
            {
                get { return null; }
            }

            public CodeModel CodeModel
            {
                get { return null; }
            }

            public object get_Extender(string ExtenderName)
            {
                return null;
            }

            #endregion
        }

        #endregion

        #region IPackageAction Members

        /// <summary>
        /// Gets the unique ID of the action.
        /// </summary>
        public int ID
        {
            get { return PackageCmdIDList.toolAction_ReferenceProject; }
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
            set
            {
                // update the menu each time something updates the configuration info object:
                Execute(this, null);
            }
        }

        /// <summary>
        /// Performs initialization of this action and
        /// also registers all the UI elements required by the action, e.g.: menus / menu groups / toolbars.
        /// </summary>
        public void Initialize(IPackageEnvironment env, IMenuCreator menuCreator)
        {
            parent = env;
            mc = menuCreator;

            solutionListener = new SolutionEventsListener(env.DTE);
            solutionListener.SolutionOpened += SolutionOpened;
            solutionListener.SolutionClosed += SolutionClosed;
            solutionListener.ProjectAdded += ProjectListChanged_Added;
            solutionListener.ProjectRemoved += ProjectListChanged_Removed;
            solutionListener.ProjectRenamed += ProjectListChanged_Renamed;

            //shellListener = new ShellEventsListener(env.DTE);
            //shellListener.ModeChanged += ShellModeChanged;
            // -------------------------------------------------------

            // append all existing projects in current solution,
            // in case AddIn was loaded after opening solution:
            LoadAndSortSystemAssemblies();
            SolutionOpened(null, parent.DTE.Solution);
        }

        /// <summary>
        /// Loads system assemblies and the list of forbidden names.
        /// </summary>
        private void LoadAndSortSystemAssemblies()
        {
            // load some configuration options from registry:
            PersistentStorageData data = ObjectFactory.LoadConfiguration(CofigurationName);
            string[] forbiddenNames = data.GetMultiString(Persistent_ForbiddenNames);
            string[] systemAssemblies = data.GetMultiString(Persistent_SystemAssemblies);

            if (forbiddenNames != null && forbiddenNames.Length > 0)
                ForbiddenNames = forbiddenNames;
            if (systemAssemblies != null && systemAssemblies.Length > 0)
                SystemAssemblies = systemAssemblies;
            else
                SystemAssemblies = DefaultSystemAssemblies;
            if (SystemAssemblies != null)
                Array.Sort(SystemAssemblies);
        }

        //void ShellModeChanged(object sender, EnvDTE80.DTE2 appObject, ShellModes currentMode, ShellModes oldMode)
        //{
        //    // update status of all popup menus:
        //    if (vsParentPopups != null)
        //        foreach (CommandBarPopup p in vsParentPopups)
        //            p.Enabled = currentMode != ShellModes.Debug;
        //}

        private void ProjectListChanged_Renamed(object sender, Project p)
        {
            RemoveAllProjects();
            vsParentPopups = mc.Customizator.AddReferenceProjectPopups();

            // refresh the projects 'Reference To' menus:
            AddMultipleProjects(ProjectHelper.GetAllProjects(parent.DTE));
        }

        void ProjectListChanged_Removed(object sender, Project p)
        {
            RemoveAllProjects();
            vsParentPopups = mc.Customizator.AddReferenceProjectPopups();

            // refresh the projects 'Reference To' menus:
            IList<Project> projects = ProjectHelper.GetAllProjects(parent.DTE);
            projects.Remove(p);

            AddMultipleProjects(projects);
        }

        private void ProjectListChanged_Added(object sender, Project p)
        {
            RemoveAllProjects();
            vsParentPopups = mc.Customizator.AddReferenceProjectPopups();

            // refresh the projects 'Reference To' menus:
            AddMultipleProjects(ProjectHelper.GetAllProjects(parent.DTE));
        }

        void SolutionOpened(object sender, Solution s)
        {
            if (s != null && s.Projects.Count > 0)
            {
                vsParentPopups = mc.Customizator.AddReferenceProjectPopups();
                AddMultipleProjects(ProjectHelper.GetAllProjects(parent.DTE));
            }
        }

        void SolutionClosed(object sender, Solution s)
        {
            RemoveAllProjects();
        }

        #region Add Multiple Projects

        /// <summary>
        /// Creates menu items for many projects.
        /// </summary>
        private void AddMultipleProjects(ICollection<Project> projects)
        {
            if (projects != null)
            {
                bool isFirst = true;

                // add pointing project from file:
                AddProjectBrowsing();

                // add projects by reference inside solution:
                foreach (Project p in projects)
                {
                    AddProject(p, isFirst);
                    isFirst = false;
                }

                if (projects.Count > 0)
                {
                    // add projects by path:
                    isFirst = true;
                    foreach (Project p in projects)
                    {
                        if (ProjectHelper.IsManaged(p))
                        {
                            try
                            {
                                string name = string.Format(SharedStrings.ReferenceProject_OutputText, p.Name);

                                AddProject(name, ProjectHelper.GetOutputPath(p), 9010, isFirst);
                                isFirst = false;
                            }
                            catch (Exception ex)
                            {
                                Trace.Write(ex.Message);
                            }
                        }
                    }

                    // add predefined set of assemblies:
                    isFirst = true;
                    foreach (string sys in SystemAssemblies)
                    {
                        AddProject(Path.GetFileNameWithoutExtension(sys), sys, 9011, isFirst);
                        isFirst = false;
                    }

                    // add all referenced assemblies:
                    isFirst = true;
                    SortedDictionary<string, Reference> references = new SortedDictionary<string, Reference>(namespaceComparer);
                    foreach (Project p in projects)
                    {
                        if (ProjectHelper.IsManaged(p))
                        {
                            VSProject vsProject = p.Object as VSProject;

                            if (vsProject != null && vsProject.References != null)
                            {
                                try
                                {
                                    foreach (Reference r in vsProject.References)
                                    {
                                        try
                                        {
                                            if (!projects.Contains(r.SourceProject))
                                            {
                                                string name = Path.GetFileNameWithoutExtension(r.Path);

                                                if (name.StartsWith(ForbiddenPrefix,
                                                                    StringComparison.CurrentCultureIgnoreCase))
                                                    name = name.Substring(ForbiddenPrefixLength);

                                                if (!string.IsNullOrEmpty(name))
                                                {
                                                    if (references.ContainsKey(name))
                                                        references[name] = r;
                                                    else
                                                        references.Add(name, r);
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            Trace.Write(ex.Message);
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Trace.Write(ex.Message);
                                }
                            }
                        }
                    }
                    foreach (string k in references.Keys)
                    {
                        if (AddProject(k, references[k].Path, 0, isFirst))
                            isFirst = false;
                    }

                    // show the popups:
                    bool isVisible = vsParentPopups[0].Controls.Count >= 1 + SystemAssemblies.Length;
                    foreach (CommandBarPopup p in vsParentPopups)
                        p.Visible = isVisible;
                }
            }
        }

        private void RemoveAllProjects()
        {
            if (vsParentPopups != null)
            {
                IMenuCustomizator customizator = mc.Customizator;

                // hide menus:
                foreach (CommandBarPopup p in vsParentPopups)
                    p.Visible = false;

                // remove all items:
                foreach (MenuCommand i in vsCommands.Values)
                    customizator.RemoveReferenceProject(i);
                foreach (MenuCommand j in vsNames.Values)
                    customizator.RemoveReferenceProject(j);

                // remove popup menus:
                vsParentPopups = null;
                vsCommands.Clear();
                vsNames.Clear();
                customizator.RemoveReferenceProjectPopups();
            }
        }

        #endregion

        #region Add Project

        /// <summary>
        /// Register new project by name and path to assembly.
        /// </summary>
        private bool AddProject(string name, string path, int iconIndex, bool beginGroup)
        {
            if(string.IsNullOrEmpty(name))
                return false;

            string lowerName = name.ToLower();

            // check if name is not forbidden, because we don't want to see some system DLLs...
            foreach(string f in ForbiddenNames)
                if(string.Compare(lowerName, f) == 0)
                    return false;

            // check if name starts with 'Microsoft'...
            if(lowerName.StartsWith(ForbiddenPrefix))
            {
                name = name.Substring(ForbiddenPrefixLength);
                lowerName = lowerName.Substring(ForbiddenPrefixLength);
            }

            if (!string.IsNullOrEmpty(name) && !vsNames.ContainsKey(lowerName))
            {
                int index = vsParentPopups[0].Controls.Count + 1;
                MenuCommand menuCommand = AddProject(ID + index, name, index, iconIndex, Property_ReferencePath, path, beginGroup);

                vsNames.Add(lowerName, menuCommand);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Add menu that browse for assembly to reference.
        /// </summary>
        private void AddProjectBrowsing ()
        {
            Project p = new DummyProject(SharedStrings.ReferenceProject_Browse);
            MenuCommand menuCommand = AddProject(ID - 1, p.Name, 1, 9012, Property_ReferenceProject, p, false);
            vsCommands.Add(p, menuCommand);
        }

        /// <summary>
        /// Register new project by Project object.
        /// </summary>
        private void AddProject(Project p, bool isFirstInGroup)
        {
            MenuCommand menuCommand = AddProject(ID + vsCommands.Count, p.Name, vsCommands.Count + 1, 9009, Property_ReferenceProject, p, isFirstInGroup);
            vsCommands.Add(p, menuCommand);
        }

        /// <summary>
        /// Register new project by unique ID, name and with specified parameters that will be used during invoke time.
        /// </summary>
        private MenuCommand AddProject(int id, string caption, int itemIndex, int iconIndex, string paramName, object paramValue, bool beginGroup)
        {
            MenuCommand menuCommand = ObjectFactory.CreateCommand(GuidList.guidCmdSet, id, ExecuteAddReference, AnyProjectBeforeQueryStatus);

            menuCommand.Properties.Add(paramName, paramValue);
            mc.AddCommand(menuCommand, GetUniqueName(caption), caption, iconIndex, null,
                          (paramValue is DummyProject ? ((DummyProject) paramValue).Name : GetToolTip(caption)), false);
            mc.Customizator.AddReferenceProject(menuCommand, beginGroup, itemIndex);

            return menuCommand;
        }

        void AnyProjectBeforeQueryStatus(object sender, EventArgs e)
        {
            MenuCommand x = sender as MenuCommand;

            if (x != null)
                x.Enabled = parent.Mode != ShellModes.Debug;
        }

        /// <summary>
        /// Generate unique name for named command of the IDE menu.
        /// </summary>
        private static string GetUniqueName(string name)
        {
            if (name.EndsWith("..."))
                name = name.Substring(0, name.Length - 3);

            //return "RefProject_" + name.Replace('.', '_').Replace(' ', '_').Replace('(', '_').Replace(')', '_');
            return "RefProject_" + name.Replace(".", string.Empty).Replace(" ", string.Empty).Replace("(", string.Empty).Replace(")", string.Empty);
        }

        /// <summary>
        /// Generate tool-tip text for given project.
        /// </summary>
        private static string GetToolTip(string name)
        {
            return string.Format(SharedStrings.ReferenceProject_ToolTip, name);
        }

        #endregion

        /// <summary>
        /// Invokes proper processing assigned to current action.
        /// </summary>
        public void Execute(object sender, EventArgs e)
        {
            // external update - update the whole menu list:
            RemoveAllProjects();
            LoadAndSortSystemAssemblies();
            SolutionOpened(this, parent.DTE.Solution);
        }

        /// <summary>
        /// Invokes proper processing assigned to current action.
        /// </summary>
        public void ExecuteAddReference(object sender, EventArgs e)
        {
            MenuCommand cmd = sender as MenuCommand;

            // get the project object to reference to:
            if (cmd != null)
            {

                try
                {
                    Project projectToAdd = cmd.Properties[Property_ReferenceProject] as Project;
                    string assemblyPathToAdd = cmd.Properties[Property_ReferencePath] as string;
                    Project cp = ProjectHelper.GetSelectedProject(parent.DTE);
                    VSProject currentProject = (cp != null ? (VSProject)cp.Object : null);

                    // check if there is where to add an item:
                    if (currentProject == null)
                    {
                        MessageBox.Show(SharedStrings.ReferenceProject_ActivateProject,
                                        SharedStrings.ActionDialogTitle_ReferenceProject,
                                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    if (projectToAdd != null && projectToAdd.Name == SharedStrings.ReferenceProject_Browse)
                    {
                        if (dlgBrowse == null)
                        {
                            dlgBrowse = new OpenFileDialog();
                            dlgBrowse.Filter = SharedStrings.ReferenceProject_BrowseFilter;
                            dlgBrowse.FilterIndex = 0;
                            dlgBrowse.CheckFileExists = true;
                            dlgBrowse.CheckPathExists = true;
                            dlgBrowse.Multiselect = true;
                        }
                        dlgBrowse.Title = string.Format(SharedStrings.ReferenceProject_BrowseTitle, cp.Name);

                        if (dlgBrowse.ShowDialog() == DialogResult.OK)
                        {
                            foreach (string name in dlgBrowse.FileNames)
                            {
                                try
                                {
                                    if (!string.IsNullOrEmpty(name))
                                        currentProject.References.Add(name);
                                }
                                catch (Exception ex)
                                {
                                    // display detailed error:
                                    MessageBox.Show(
                                        SharedStrings.ReferenceProject_NotValidProject + Environment.NewLine +
                                        ex.Message,
                                        SharedStrings.ActionDialogTitle_ReferenceProject,
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);

                                }
                            }
                        }
                    }
                    else
                    {
                        // try to add project:
                        if (projectToAdd != null && projectToAdd.Name != cp.Name)
                            currentProject.References.AddProject(projectToAdd);
                        else
                            // try to add any assembly by path:
                            if (!string.IsNullOrEmpty(assemblyPathToAdd))
                                currentProject.References.Add(assemblyPathToAdd);
                    }
                }
                catch (Exception ex)
                {
                    // display detailed error:
                    MessageBox.Show(SharedStrings.ReferenceProject_NotValidProject + Environment.NewLine + ex.Message,
                                    SharedStrings.ActionDialogTitle_ReferenceProject,
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Executed on Visual Studio exit.
        /// All non-managed resources should be released here.
        /// </summary>
        public void Destroy()
        {
            if (solutionListener != null)
            {
                solutionListener.Dispose();
                solutionListener = null;
            }

            vsCommands.Clear();
        }

        #endregion
    }
}
