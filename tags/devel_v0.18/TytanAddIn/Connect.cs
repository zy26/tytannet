using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using EnvDTE;
using EnvDTE80;
using Extensibility;
using Microsoft.VisualStudio.CommandBars;
using Pretorianie.Tytan.Actions;
using Pretorianie.Tytan.Actions.Misc;
using Pretorianie.Tytan.Actions.Tools;
using Pretorianie.Tytan.Core.CustomAddIn;
using Pretorianie.Tytan.Core.Data;
using Pretorianie.Tytan.Core.Interfaces;
using System.Reflection;
using System.Diagnostics;

namespace Pretorianie.Tytan
{
    /// <summary>The object for implementing an Add-in.</summary>
    /// <seealso class='IDTExtensibility2' />
    [ComVisible(true)]
    public class Connect : IDTExtensibility2, IDTCommandTarget, IMenuCustomizator
    {
        /// <summary>
        /// Name of the Tytan-Toolbar.
        /// </summary>
        public const string ToolbarName = "Tytan Toolbar";
        /// <summary>
        /// Name of the menu for auxiliary windows.
        /// </summary>
        public const string AuxiliaryToolsName = " ";
        /// <summary>
        /// Name of the Refactor menu.
        /// </summary>
        public const string RefactorMenu = "Refactor";
        /// <summary>
        /// Name of the About menu.
        /// </summary>
        public const string AboutMenu = "Info";
        /// <summary>
        /// Name of the Insert menu.
        /// </summary>
        public const string InsertMenu = "Quick &Insert";

        private CustomAddInManager manager;

        /// <summary>Implements the OnConnection method of the IDTExtensibility2 interface. Receives notification that the Add-in is being loaded.</summary>
        /// <param term='application'>Root object of the host application.</param>
        /// <param term='connectMode'>Describes how the Add-in is being loaded.</param>
        /// <param term='addInInst'>Object representing this Add-in.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnConnection(object application, ext_ConnectMode connectMode, object addInInst, ref Array custom)
        {
            if (manager == null)
            {
                manager = new CustomAddInManager((DTE2) application, (AddIn) addInInst, this,
                                                 Assembly.GetExecutingAssembly());
                //if (connectMode != ext_ConnectMode.ext_cm_UISetup)
                InitializeCustomizator();

                // register all known refactoring IPackageActions:
                manager.Add(new InitConstructorRefactor());
                manager.Add(new ExtractPropertyRefactor());
                // manager.Add(new ExtractResourceRefactor());
                manager.Add(new AssignReorderRefactor());
                manager.Add(new MultiRenameRefactor());
                manager.Add(new InsertionGuidRefactor());
                manager.Add(new InsertionClassNameAction());
                manager.Add(new InsertionPathRefactor());
                manager.Add(new InsertionDatabaseRefactor());
                if (manager.Version == ShellVersions.VS2005)
                    manager.Add(new OpenWindowsExplorer());
                manager.Add(new ReferenceProjectAction());
                manager.Add(new VisualStudioCloseQuestion());

                // manager.Add(new NativeImagePreviewPackageTool());
                manager.Add(new CommandViewPackageTool());
                manager.Add(new RegistryViewPackageTool());
                manager.Add(new EnvironmentVarsPackageTool());
                manager.Add(new DebugViewPackageTool());

                manager.Add(new AboutBoxAction());
            }

            // initialize VS IDE commands:
            manager.ApplicationInit(true /* connectMode == ext_ConnectMode.ext_cm_UISetup */);
            AfterApplicationInit();
        }

        private void AfterApplicationInit()
        {
            if (tytanToolbar != null)
                tytanToolbar.Visible = true;
        }

        /// <summary>Implements the OnDisconnection method of the IDTExtensibility2 interface. Receives notification that the Add-in is being unloaded.</summary>
        /// <param term='disconnectMode'>Describes how the Add-in is being unloaded.</param>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnDisconnection(ext_DisconnectMode disconnectMode, ref Array custom)
        {
            manager.ApplicationExit(false /* true */);
            DestroyCustomizator();
            manager = null;
        }

        /// <summary>Implements the OnAddInsUpdate method of the IDTExtensibility2 interface. Receives notification when the collection of Add-ins has changed.</summary>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnAddInsUpdate(ref Array custom)
        {
        }

        /// <summary>Implements the OnStartupComplete method of the IDTExtensibility2 interface. Receives notification that the host application has completed loading.</summary>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnStartupComplete(ref Array custom)
        {
        }

        /// <summary>Implements the OnBeginShutdown method of the IDTExtensibility2 interface. Receives notification that the host application is being unloaded.</summary>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnBeginShutdown(ref Array custom)
        {
        }

        #region IDTCommandTarget Members

        /// <summary>
        /// Execute command.
        /// </summary>
        public void Exec(string cmdName, vsCommandExecOption executeOption, ref object variantIn, ref object variantOut, ref bool handled)
        {
            manager.Execute(cmdName, ref handled);
        }

        /// <summary>
        /// Query status.
        /// </summary>
        public void QueryStatus(string cmdName, vsCommandStatusTextWanted neededText, ref vsCommandStatus statusOption, ref object commandText)
        {
            manager.QueryStatus(cmdName, neededText, ref statusOption, ref commandText);
        }

        #endregion

        #region IMenuCustomizator Members

        private CommandBar tytanToolbar;
        private CommandBarPopup tytanAuxTools;
        private CommandBarPopup tytanRefactors;
        private CommandBarPopup aboutItems;
        private IList<CommandBar> refactors;
        private IList<CommandBarPopup> referenceProjects;
        private IList<CommandBarPopup> insertionItems;

        private readonly string[] SolutionTreeMenu =
            new string[] { "Solution", "Project", "Folder", "Item", "Web Project Folder", "Web Folder", "Web Item" };
        private readonly string[] SolutionTreeMdiContextMenu =
            new string[] { "Solution", "Project", "Folder", "Item", "Web Project Folder", "Web Folder", "Web Item", "Easy MDI Document Window" };

        private readonly string[] ReferenceProjectMenu =
            new string[] { "Tytan Toolbar", "Class View Project", "Class View Project References Folder", "Project", "Reference Root" };

        private readonly string[] ReferenceProjectAfter =
            new string[] { "Add Reference...", "Add Web Reference...", RefactorMenu };

        private void InitializeCustomizator()
        {
            IMenuCreator mc = manager.MenuCreator;

            tytanToolbar = mc.AddToolbar(ToolbarName);
            tytanAuxTools = mc.AddPopup(tytanToolbar, AuxiliaryToolsName, null, false, -1);

            tytanRefactors = mc.AddPopup(tytanToolbar, RefactorMenu, null, true, -1);
            refactors = new List<CommandBar>();
            if (tytanRefactors != null)
                refactors.Add(tytanRefactors.CommandBar);
            refactors.Add(mc.GetMainMenuCommandBar(RefactorMenu));
            refactors.Add(mc.GetCodeWindowCommandBar(RefactorMenu));
        }

        private void DestroyCustomizator()
        {
            if (tytanToolbar != null)
                tytanToolbar.Visible = false;
        }

        /// <summary>
        /// Create a set of identical popup menus attached to given parents.
        /// </summary>
        private IList<CommandBarPopup> InitializeReferenceProjectsPopups(string[] vsParentMenus, string caption, string captionNo1, string toolTip, params string[] afterItemCaption)
        {
            CommandBarPopup[] vsPopups = new CommandBarPopup[vsParentMenus.Length];
            IMenuCreator menuCreator = manager.MenuCreator;

            // register number of popup-menus:
            for (int i = 0; i < vsParentMenus.Length; i++)
            {
                vsPopups[i] = menuCreator.AddPopup(manager.MenuCreator.GetCommandBar(vsParentMenus[i]),
                                                   (i == 0 ? captionNo1 : caption), toolTip,
                                                   false, -1, afterItemCaption);

                vsPopups[i].Visible = false;
            }

            return vsPopups;
        }

        /// <summary>
        /// Registers new refactoring method.
        /// </summary>
        void IMenuCustomizator.AddRefactoring(MenuCommand menuCommand, bool beginGroup, int itemIndex, string afterItemCaption)
        {
            IMenuCreator menuCreator = manager.MenuCreator;

            if (menuCreator.IsSetupUI && tytanRefactors != null)
                foreach (CommandBar r in refactors)
                    menuCreator.AddButton(r, menuCommand, beginGroup, itemIndex, afterItemCaption);
        }

        /// <summary>
        /// Registers new tool window.
        /// </summary>
        void IMenuCustomizator.AddToolWindow(string parentMenuName, MenuCommand menuCommand, bool beginGroup, int itemIndex, string afterItemCaption)
        {
            IMenuCreator menuCreator = manager.MenuCreator;

            if (menuCreator.IsSetupUI)
            {
                if (!string.IsNullOrEmpty(parentMenuName))
                {
                    CommandBar menu = menuCreator.GetMainMenuCommandBar(parentMenuName);
                    menuCreator.AddButton(menu, menuCommand, false, itemIndex, afterItemCaption);
                }

                // add only icon on the toolbar:
                if (tytanToolbar != null)
                {
                    CommandBarButton b = menuCreator.AddButton(tytanToolbar, menuCommand, beginGroup, 1);
                    b.Style = MsoButtonStyle.msoButtonIcon;
                }
            }
        }

        /// <summary>
        /// Registers new tool window as an auxiliary item.
        /// </summary>
        public void AddAuxToolWindow(string parentMenuName, MenuCommand menuCommand, bool beginGroup, int itemIndex, string afterItemCaption)
        {
            IMenuCreator menuCreator = manager.MenuCreator;

            if (menuCreator.IsSetupUI)
            {
                if (!string.IsNullOrEmpty(parentMenuName))
                {
                    CommandBar menu = menuCreator.GetMainMenuCommandBar(parentMenuName);
                    menuCreator.AddButton(menu, menuCommand, false, itemIndex, afterItemCaption);
                }

                // add icons with text on the toolbar:
                if (tytanAuxTools != null)
                {
                    CommandBarButton b = menuCreator.AddButton(tytanAuxTools.CommandBar, menuCommand, beginGroup, 1);
                    b.Style = MsoButtonStyle.msoButtonIconAndCaption;
                    tytanAuxTools.BeginGroup = false;
                }
            }
        }

        /// <summary>
        /// Registers new item for solution-explorer.
        /// </summary>
        public void AddSolutionExplorerItem(MenuCommand menuCommand, bool alsoMdiMenu, bool beginGroup, int itemIndex, params string[] afterItemCaption)
        {
            IMenuCreator menuCreator = manager.MenuCreator;

            if (menuCreator.IsSetupUI)
            {
                string[] commandBarNames = (alsoMdiMenu ? SolutionTreeMdiContextMenu : SolutionTreeMenu);

                foreach (string n in commandBarNames)
                {
                    // find proper command-bar:
                    CommandBar cmdBar = menuCreator.GetCommandBar(n);

                    // try to insert new element:
                    if (cmdBar != null)
                        menuCreator.AddButton(cmdBar, menuCommand, beginGroup, itemIndex, afterItemCaption);
                }
            }
        }

        /// <summary>
        /// Registers new item inside 'About' popup.
        /// </summary>
        public void AddAboutItem(MenuCommand menuCommand, bool beginGroup, int itemIndex, string afterItemCaption)
        {
            IMenuCreator menuCreator = manager.MenuCreator;

            if (menuCreator.IsSetupUI)
            {
                if (aboutItems == null)
                    aboutItems = menuCreator.AddPopup(tytanToolbar, AboutMenu, null, true, -1);

                menuCreator.AddButton(aboutItems.CommandBar, menuCommand, beginGroup, itemIndex, afterItemCaption);
            }
        }

        /// <summary>
        /// Registers new item inside 'Insert' popup.
        /// </summary>
        public void AddInsertionItem(MenuCommand menuCommand, bool beginGroup, int itemIndex, string afterItemCaption)
        {
            IMenuCreator menuCreator = manager.MenuCreator;

            if(menuCreator.IsSetupUI)
            {
                if (insertionItems == null)
                {
                    insertionItems = new List<CommandBarPopup>();
                    foreach (CommandBar r in refactors)
                        insertionItems.Add(menuCreator.AddPopup(r, InsertMenu, null, false, -1));
                }

                // and insert the button inside each popup menu:
                foreach (CommandBarPopup p in insertionItems)
                    menuCreator.AddButton(p.CommandBar, menuCommand, beginGroup, itemIndex, afterItemCaption);
            }
        }

        /// <summary>
        /// Creates the list of popup menus that will contain the list of projects to reference.
        /// </summary>
        public IList<CommandBarPopup> AddReferenceProjectPopups()
        {
            if (referenceProjects == null)
            {
                referenceProjects =
                    InitializeReferenceProjectsPopups(ReferenceProjectMenu, "Add Reference &To", "Re&ference", null, ReferenceProjectAfter);
            }

            return referenceProjects;
        }

        /// <summary>
        /// Removes all the list of popup menus for referenced projects.
        /// </summary>
        public void RemoveReferenceProjectPopups()
        {
            if (referenceProjects != null)
            {
                IMenuCreator menuCreator = manager.MenuCreator;

                foreach (CommandBarPopup vsPopup in referenceProjects)
                    menuCreator.DeletePopup(vsPopup);

                referenceProjects = null;
            }
        }

        /// <summary>
        /// Registers new popup menu for Reference-Project group.
        /// </summary>
        public void AddReferenceProject(MenuCommand menuCommand, bool beginGroup, int itemIndex,
                                                   params string[] afterItemCaption)
        {
            if (referenceProjects != null)
            {
                try
                {
                    IMenuCreator menuCreator = manager.MenuCreator;

                    foreach (CommandBarPopup vsPopup in referenceProjects)
                    {
                        menuCreator.AddButton(vsPopup.CommandBar, menuCommand, beginGroup, itemIndex,
                                                      afterItemCaption);
                    }
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex.Message);
                }
            }
        }

        /// <summary>
        /// Removes all buttons and named commands assigned to given menu command.
        /// </summary>
        public void RemoveReferenceProject(MenuCommand menuCommand)
        {
            manager.MenuCreator.DeleteCommand(menuCommand);
        }

        #endregion
    }
}
