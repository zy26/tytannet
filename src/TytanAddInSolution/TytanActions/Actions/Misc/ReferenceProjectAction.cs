using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using EnvDTE;
using Microsoft.VisualStudio.CommandBars;
using Microsoft.VisualStudio.Shell;
using Pretorianie.Tytan.Core.Data;
using Pretorianie.Tytan.Core.Helpers;
using Pretorianie.Tytan.Core.Interfaces;
using VSLangProj;
using Pretorianie.Tytan.Core.Comparers;

namespace Pretorianie.Tytan.Actions.Misc
{
    public class ReferenceProjectAction : IPackageAction
    {
        private IPackageEnvironment parent;
        private SolutionEventsListener eventListener;
        private IMenuCommandService mcs;
        private IMenuCreator mc;
        private IList<CommandBarPopup> vsParentPopups;
        private readonly Dictionary<Project, MenuCommand> vsCommands = new Dictionary<Project, MenuCommand>();
        private readonly Dictionary<string, MenuCommand> vsNames = new Dictionary<string, MenuCommand>();
        private readonly IComparer<string> namespaceComparer = new NamespaceComparer('.');

        public const string Name = "ReferenceProjectAction";
        private const string Persistent_ForbiddenNames = "ForbiddenNames";
        private const string Persistent_SystemAssemblies = "SystemAssemblies";

        private string[] ForbiddenNames = 
            new string[] { "microsoft", "system", "mscorlib" };

        private string[] SystemAssemblies =
            new string[] {"System.Data.dll", "System.Drawing.dll", "System.Windows.Forms.dll", "System.Xml.dll"};

        private const string Property_ReferenceProject = "ReferenceProject";
        private const string Property_ReferencePath = "ReferenceAssembly";

        #region IPackageAction Members

        /// <summary>
        /// Gets the unique ID of the action.
        /// </summary>
        public int ID
        {
            get { return PackageCmdIDList.toolAction_ReferenceProject; }
        }

        /// <summary>
        /// Performs initialization of this action and
        /// also registers all the UI elements required by the action, e.g.: menus / menu groups / toolbars.
        /// </summary>
        public void Initialize(IPackageEnvironment env, IMenuCommandService mcs, IMenuCreator mc)
        {
            parent = env;
            this.mcs = mcs;
            this.mc = mc;

            eventListener = new SolutionEventsListener(env.DTE);
            eventListener.SolutionOpened += SolutionOpened;
            eventListener.SolutionClosed += SolutionClosed;
            eventListener.ProjectAdded += ProjectListChanged_Added;
            eventListener.ProjectRemoved += ProjectListChanged_Removed;
            eventListener.ProjectRenamed += ProjectListChanged_Renamed;

            // -------------------------------------------------------

            // load some configuration options from registry:
            PersistentStorageData data = new PersistentStorageData(Name);
            string forbiddenNames = data.GetString(Persistent_ForbiddenNames);
            string systemAssemblies = data.GetString(Persistent_SystemAssemblies);

            if (!string.IsNullOrEmpty(forbiddenNames))
                ForbiddenNames = forbiddenNames.Split('|', ',');
            if (!string.IsNullOrEmpty(systemAssemblies))
                SystemAssemblies = systemAssemblies.Split('|', ',');
            if (SystemAssemblies != null)
                Array.Sort(SystemAssemblies);

            // append all existing projects in current solution,
            // in case AddIn was loaded after opening solution:
            SolutionOpened(null, parent.DTE.Solution);
        }

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
        private void AddMultipleProjects(IList<Project> projects)
        {
            if (projects != null)
            {
                // add projects by reference inside solution:
                foreach (Project p in projects)
                    AddProject(p);

                if (projects.Count > 0)
                {
                    // add projects by path:
                    bool isFirst = true;
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
                                            string name = Path.GetFileNameWithoutExtension(r.Path);

                                            if (name.StartsWith("microsoft.", StringComparison.CurrentCultureIgnoreCase))
                                                name = name.Substring(10);

                                            if (!string.IsNullOrEmpty(name))
                                            {
                                                if (references.ContainsKey(name))
                                                    references[name] = r;
                                                else
                                                    references.Add(name, r);
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
            if(lowerName.StartsWith("microsoft."))
            {
                name = name.Substring(10);
                lowerName = lowerName.Substring(10);
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
        /// Register new project by Project object.
        /// </summary>
        private void AddProject(Project p)
        {
            MenuCommand menuCommand = AddProject(ID + vsCommands.Count, p.Name, vsCommands.Count + 1, 9009, Property_ReferenceProject, p, false);
            vsCommands.Add(p, menuCommand);
        }

        /// <summary>
        /// Register new project by unique ID, name and with specified parameters that will be used during invoke time.
        /// </summary>
        private MenuCommand AddProject(int id, string caption, int itemIndex, int iconIndex, string paramName, object paramValue, bool beginGroup)
        {
            CommandID commandID = new CommandID(GuidList.guidCmdSet, id);
            OleMenuCommand menuCommand = new OleMenuCommand(Execute, commandID);

            menuCommand.Properties.Add(paramName, paramValue);
            mcs.AddCommand(menuCommand);

            mc.AddCommand(menuCommand, GetUniqueName(caption), caption, iconIndex, null, GetToolTip(caption), false);
            mc.Customizator.AddReferenceProject(menuCommand, beginGroup, itemIndex);

            return menuCommand;
        }

        /// <summary>
        /// Generate unique name for named command of the IDE menu.
        /// </summary>
        private static string GetUniqueName(string name)
        {
            return "RefProject_" + name.Replace('.', '_').Replace(' ', '_').Replace('(', '_').Replace(')', '_');
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
            OleMenuCommand cmd = sender as OleMenuCommand;

            // get the project object to reference to:
            if (cmd != null)
            {
                try
                {
                    Project projectToAdd = cmd.Properties[Property_ReferenceProject] as Project;
                    string assemblyPathToAdd = cmd.Properties[Property_ReferencePath] as string;
                    Project cp = ProjectHelper.GetSelectedProject(parent.DTE);
                    VSProject currentProject = (cp != null ? (VSProject)cp.Object : null);

                    // try to add project:
                    if (projectToAdd != null && currentProject != null && cp != null && projectToAdd.Name != cp.Name)
                        currentProject.References.AddProject(projectToAdd);
                    else
                        // try to add any assembly by path:
                        if (currentProject != null && !string.IsNullOrEmpty(assemblyPathToAdd))
                            currentProject.References.Add(assemblyPathToAdd);
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

        #endregion

        #region IDisposable Members

        ///<summary>
        ///Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        ///</summary>
        public void Dispose()
        {
            if (eventListener != null)
            {
                eventListener.Dispose();
                eventListener = null;
            }

            vsCommands.Clear();
        }

        #endregion
    }
}
