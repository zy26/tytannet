using System.Collections.Generic;
using System.ComponentModel.Design;
using EnvDTE;
using Microsoft.VisualStudio.CommandBars;
using Pretorianie.Tytan.Core.Interfaces;
namespace Pretorianie.Tytan.Core.CustomPackage
{
    /// <summary>
    /// This class provides empty implementation of IMenuCreator interface.
    /// </summary>
    public class EmptyMenuCreator : IMenuCreator
    {
        private readonly EmptyMenuCustomizator customizator = new EmptyMenuCustomizator();

        #region Private Class

        private class EmptyMenuCustomizator : IMenuCustomizator
        {
            #region IMenuCustomizator Members

            /// <summary>
            /// Registers new refactoring method.
            /// </summary>
            public void AddRefactoring(MenuCommand menuCommand, bool beginGroup, int itemIndex, string afterItemCaption)
            {
            }

            /// <summary>
            /// Registers new tool window.
            /// </summary>
            public void AddToolWindow(string parentMenuName, MenuCommand menuCommand, bool beginGroup, int itemIndex,
                                      string afterItemCaption)
            {
            }

            /// <summary>
            /// Registers new item for solution-explorer.
            /// </summary>
            public void AddSolutionExplorerItem(MenuCommand menuCommand, bool alsoMdiMenu, bool beginGroup,
                                                int itemIndex, params string[] afterItemCaption)
            {
            }

            /// <summary>
            /// Creates the list of popup menus that will contain the list of projects to reference.
            /// </summary>
            public IList<CommandBarPopup> AddReferenceProjectPopups()
            {
                return null;
            }

            /// <summary>
            /// Removes all the list of popup menus for referenced projects.
            /// </summary>
            public void RemoveReferenceProjectPopups()
            {
            }

            /// <summary>
            /// Registers new popup menu for Reference-Project group.
            /// </summary>
            public void AddReferenceProject(MenuCommand menuCommand, bool beginGroup, int itemIndex,
                                                       params string[] afterItemCaption)
            {
            }

            /// <summary>
            /// Removes all buttons and named commands assigned to given menu command.
            /// </summary>
            public void RemoveReferenceProject(MenuCommand menuCommand)
            {
            }

            #endregion
        }

        #endregion

        #region IMenuCreator Members

        /// <summary>
        /// Checks if currently the SetupUI should be performed.
        /// This flag can be ignored when using Customizator.
        /// </summary>
        public bool IsSetupUI
        {
            get { return false; }
        }

        /// <summary>
        /// Menu customizator.
        /// </summary>
        public IMenuCustomizator Customizator
        {
            get { return customizator; }
        }

        /// <summary>
        /// Handle to VisualStudio IDE MainMenu.
        /// </summary>
        public CommandBar MainMenu
        {
            get { return null; }
        }

        /// <summary>
        /// Handle to VisualStudio IDE right-click menu for code window.
        /// </summary>
        public CommandBar CodeWindowMenu
        {
            get { return null; }
        }

        /// <summary>
        /// Handle to VisualStudio IDE standard toolbar.
        /// </summary>
        public CommandBar StandardToolbar
        {
            get { return null; }
        }

        /// <summary>
        /// Returns handle to any menu or toolbar of VisualStudio IDE.
        /// </summary>
        public CommandBar GetCommandBar(string name)
        {
            return null;
        }

        /// <summary>
        /// Returns handle to any menu or toolbar of VisualStudio IDE with language version independent name.
        /// </summary>
        public CommandBar GetLocalizedCommandBar(string name)
        {
            return null;
        }

        /// <summary>
        /// Returns handle to sub menu of VisualStudio MainMenu with given name.
        /// </summary>
        public CommandBar GetMainMenuCommandBar(string name)
        {
            return null;
        }

        /// <summary>
        /// Returns handle to sub menu of CodeWindow menu.
        /// </summary>
        public CommandBar GetCodeWindowCommandBar(string name)
        {
            return null;
        }

        /// <summary>
        /// Creates new command that then can be captured by MenuCommand with the same CommandID.
        /// </summary>
        public Command AddCommand(MenuCommand menuCommand, string name, string caption, int iconIndex,
                                  string hotkeyString, string toolTip, bool imageOnly)
        {
            return null;
        }

        /// <summary>
        /// Creates new instance of toolbar with specified name.
        /// </summary>
        public CommandBar AddToolbar(string name)
        {
            return null;
        }

        /// <summary>
        /// Creates new item with given description.
        /// </summary>
        public CommandBarButton AddButton(CommandBar parentMenu, MenuCommand menuCommand, bool beginGroup, int itemIndex,
                                          params string[] afterItemCaption)
        {
            return null;
        }

        /// <summary>
        /// Creates new popup item with given description.
        /// </summary>
        public CommandBarPopup AddPopup(CommandBar parentMenu, string caption, string toolTip, bool beginGroup,
                                        int itemIndex, params string[] afterItemCaption)
        {
            return null;
        }

        /// <summary>
        /// Removes given MenuCommand and all the controls associated with it.
        /// </summary>
        public void DeleteCommand(MenuCommand menuCommand)
        {
        }

        /// <summary>
        /// Removes given popup menu.
        /// All the child controls (MenuCommands) should be removed separately before call of this function.
        /// </summary>
        public void DeletePopup(CommandBarPopup vsPopup)
        {
        }

        #endregion
    }
}
