using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using Microsoft.VisualStudio.CommandBars;
using Pretorianie.Tytan.Core.CustomAddIn;
using Pretorianie.Tytan.Core.Data;
using Pretorianie.Tytan.Core.Helpers;
using Pretorianie.Tytan.Core.Interfaces;

namespace Pretorianie.Tytan
{
    /// <summary>
    /// TytanNET-specific class that is responsible for defining the GUI side of the add-in.
    /// </summary>
    class TytanCustomizator : IMenuCustomizator
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

        /// <summary>
        /// Name of the parameter that describes the status of the TytanToolbar visibility.
        /// </summary>
        private const string ShowTytanToolbarOption = "ShowTytanToolbar";

        #region IMenuCustomizator Members

        private CustomAddInManager manager;
        private PersistentStorageData config;
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


        /// <summary>
        /// Initialize customizator and associate with given manager.
        /// </summary>
        public void Initialize(CustomAddInManager m)
        {
            IMenuCreator mc = m.MenuCreator;

            // read the global settings for an add-in:
            config = ObjectFactory.LoadConfiguration(null);

            // configure the management objects:
            manager = m;
            tytanToolbar = mc.AddToolbar(ToolbarName);
            tytanAuxTools = mc.AddPopup(tytanToolbar, AuxiliaryToolsName, null, false, -1);

            tytanRefactors = mc.AddPopup(tytanToolbar, RefactorMenu, null, true, -1);
            refactors = new List<CommandBar>();
            if (tytanRefactors != null)
                refactors.Add(tytanRefactors.CommandBar);
            refactors.Add(mc.GetMainMenuCommandBar(RefactorMenu));
            refactors.Add(mc.GetCodeWindowCommandBar(RefactorMenu));
        }

        /// <summary>
        /// Initialize components when all actions has been already setup.
        /// </summary>
        public void AfterApplicationInit(bool setupUI)
        {
            if (tytanToolbar != null)
                tytanToolbar.Visible = config.GetUInt(ShowTytanToolbarOption, 1) > 0;
        }

        /// <summary>
        /// Destroy elements that belong to customizator's GUI.
        /// </summary>
        public void Destroy()
        {
            if (tytanToolbar != null)
            {
                // store the config option:
                config.Add(ShowTytanToolbarOption, (uint) (tytanToolbar.Visible ? 1 : 0));
                PersistentStorageHelper.Save(config);

                tytanToolbar.Visible = false;
            }
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

            if (menuCreator.IsSetupUI)
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
