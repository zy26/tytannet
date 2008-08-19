using System.Collections.Generic;
using System.ComponentModel.Design;
using Microsoft.VisualStudio.CommandBars;

namespace Pretorianie.Tytan.Core.Interfaces
{
    /// <summary>
    /// Interface implemented by current AddIn menu customizator.
    /// </summary>
    public interface IMenuCustomizator
    {
        /// <summary>
        /// Registers new refactoring method.
        /// </summary>
        void AddRefactoring(MenuCommand menuCommand, bool beginGroup, int itemIndex, string afterItemCaption);

        /// <summary>
        /// Registers new tool window.
        /// </summary>
        void AddToolWindow(string parentMenuName, MenuCommand menuCommand, bool beginGroup, int itemIndex, string afterItemCaption);

        /// <summary>
        /// Registers new item for solution-explorer.
        /// </summary>
        void AddSolutionExplorerItem(MenuCommand menuCommand, bool alsoMdiMenu, bool beginGroup, int itemIndex, params string[] afterItemCaption);

        /// <summary>
        /// Creates the list of popup menus that will contain the list of projects to reference.
        /// </summary>
        IList<CommandBarPopup> AddReferenceProjectPopups();

        /// <summary>
        /// Removes all the list of popup menus for referenced projects.
        /// </summary>
        void RemoveReferenceProjectPopups();

        /// <summary>
        /// Registers new popup menu for Reference-Project group.
        /// </summary>
        void AddReferenceProject(MenuCommand menuCommand, bool beginGroup, int itemIndex, params string[] afterItemCaption);

        /// <summary>
        /// Removes all buttons and named commands assigned to given menu command.
        /// </summary>
        void RemoveReferenceProject(MenuCommand menuCommand);
    }
}
