using System.ComponentModel.Design;
using Microsoft.VisualStudio.CommandBars;
using EnvDTE;

namespace Pretorianie.Tytan.Core.Interfaces
{
    /// <summary>
    /// This interface defines an easy way of creation VisualStudio IDE menus.
    /// </summary>
    public interface IMenuCreator
    {
        /// <summary>
        /// Checks if currently the SetupUI should be performed.
        /// This flag can be ignored when using Customizator.
        /// </summary>
        bool IsSetupUI
        { get; }

        /// <summary>
        /// Menu customizator.
        /// </summary>
        IMenuCustomizator Customizator
        { get; }

        /// <summary>
        /// Handle to VisualStudio IDE MainMenu.
        /// </summary>
        CommandBar MainMenu
        { get; }

        /// <summary>
        /// Handle to VisualStudio IDE right-click menu for code window.
        /// </summary>
        CommandBar CodeWindowMenu
        { get; }

        /// <summary>
        /// Handle to VisualStudio IDE standard toolbar.
        /// </summary>
        CommandBar StandardToolbar
        { get; }

        /// <summary>
        /// Returns handle to any menu or toolbar of VisualStudio IDE.
        /// </summary>
        CommandBar GetCommandBar(string name);

        /// <summary>
        /// Returns handle to any menu or toolbar of VisualStudio IDE with language version independent name.
        /// </summary>
        CommandBar GetLocalizedCommandBar(string name);

        /// <summary>
        /// Returns handle to sub menu of VisualStudio MainMenu with given name.
        /// </summary>
        CommandBar GetMainMenuCommandBar(string name);

        /// <summary>
        /// Returns handle to sub menu of CodeWindow menu.
        /// </summary>
        CommandBar GetCodeWindowCommandBar(string name);

        /// <summary>
        /// Creates new command that then can be captured by MenuCommand with the same CommandID.
        /// </summary>
        Command AddCommand(MenuCommand menuCommand, string name, string caption, int iconIndex, string hotkeyString, string toolTip, bool imageOnly);

        /// <summary>
        /// Creates new instance of toolbar with specified name.
        /// </summary>
        CommandBar AddToolbar(string name);

        /// <summary>
        /// Creates new item with given description.
        /// </summary>
        CommandBarButton AddButton(CommandBar parentMenu, MenuCommand menuCommand, bool beginGroup, int itemIndex, params string[] afterItemCaption);

        /// <summary>
        /// Creates new popup item with given description.
        /// </summary>
        CommandBarPopup AddPopup(CommandBar parentMenu, string caption, string toolTip, bool beginGroup, int itemIndex, params string[] afterItemCaption);

        /// <summary>
        /// Removes given MenuCommand and all the controls associated with it.
        /// </summary>
        void DeleteCommand(MenuCommand menuCommand);

        /// <summary>
        /// Removes given popup menu.
        /// All the child controls (MenuCommands) should be removed separately before call of this function.
        /// </summary>
        void DeletePopup(CommandBarPopup popupMenu);
    }
}
