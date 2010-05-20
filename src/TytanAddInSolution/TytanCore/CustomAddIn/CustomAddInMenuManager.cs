using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Resources;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.CommandBars;
using Pretorianie.Tytan.Core.Interfaces;

namespace Pretorianie.Tytan.Core.CustomAddIn
{
    /// <summary>
    /// Generic usage class for wrapping operations of add-in manager.
    /// </summary>
    public class CustomAddInMenuManager : IMenuCommandService, IMenuCreator
    {
        private readonly DTE2 appObject;
        private readonly AddIn addInInstance;
        private readonly ResourceManager resourceManager;
        private readonly CultureInfo cultureInfo;
        private readonly IMenuCustomizator menuCustomizator;
        private bool isSetupUI;

        private readonly DesignerVerbCollection verbs = new DesignerVerbCollection();
        private readonly Dictionary<CommandID, MenuCommand> commandItems = new Dictionary<CommandID, MenuCommand>();
        private readonly Dictionary<CommandID, string> commandNames = new Dictionary<CommandID, string>();
        private readonly Dictionary<CommandID, Command> commands = new Dictionary<CommandID, Command>();
        private readonly Dictionary<Command, string> captions = new Dictionary<Command, string>();
        private readonly Dictionary<string, MenuCommand> namedCommands = new Dictionary<string, MenuCommand>();
        private readonly Dictionary<CommandBarControl, MenuCommand> menuControls = new Dictionary<CommandBarControl, MenuCommand>();
        private readonly IList<CommandBarButton> buttons = new List<CommandBarButton>();
        private readonly IList<CommandBarPopup> popups = new List<CommandBarPopup>();
        private readonly IList<CommandBar> toolbars = new List<CommandBar>();
        private readonly CommandBars vsCommandBars;
        private readonly CommandBar mainMenu;
        private readonly CommandBar codeWindowMenu;
        private readonly CommandBar standardToolbar;

        /// <summary>
        /// Init constructor.
        /// </summary>
        public CustomAddInMenuManager(DTE2 appObject, AddIn addInInstance, IMenuCustomizator menuCustomizator)
        {
            this.appObject = appObject;
            this.addInInstance = addInInstance;
            this.menuCustomizator = menuCustomizator;
            isSetupUI = true;

            try
            {
                if (resourceManager == null)
                    resourceManager = new ResourceManager("Pretorianie.Tytan.Core.CustomAddIn.CommandBar", Assembly.GetExecutingAssembly());
                if (cultureInfo == null)
                    cultureInfo = new CultureInfo(this.appObject.LocaleID);

                // get instances of default command and tool bars:
                vsCommandBars = appObject.CommandBars as CommandBars;
                if (vsCommandBars != null)
                {
                    mainMenu = vsCommandBars["MenuBar"];
                    codeWindowMenu = vsCommandBars["Code Window"];
                    standardToolbar = vsCommandBars["Standard"];
                }
            }
            catch (Exception e)
            {
                Trace.TraceError(e.ToString());
                throw;
            }
        }

        #region IMenuCommandService Members

        ///<summary>
        ///Adds the specified standard menu command to the menu.
        ///</summary>
        ///<param name="command">The <see cref="T:System.ComponentModel.Design.MenuCommand"></see> to add. </param>
        ///<exception cref="T:System.InvalidOperationException">The <see cref="T:System.ComponentModel.Design.CommandID"></see> of the specified <see cref="T:System.ComponentModel.Design.MenuCommand"></see> is already present on a menu. </exception>
        public void AddCommand(MenuCommand command)
        {
            if (command == null)
                throw new ArgumentNullException("command");

            // store or overwrite the command:
            if (commandItems.ContainsKey(command.CommandID))
                commandItems[command.CommandID] = command;
            else
                commandItems.Add(command.CommandID, command);
        }

        ///<summary>
        ///Adds the specified designer verb to the set of global designer verbs.
        ///</summary>
        ///<param name="verb">The <see cref="T:System.ComponentModel.Design.DesignerVerb"></see> to add. </param>
        public void AddVerb(DesignerVerb verb)
        {
            verbs.Add(verb);
        }

        ///<summary>
        ///Searches for the specified command ID and returns the menu command associated with it.
        ///</summary>
        ///<returns>
        ///The <see cref="T:System.ComponentModel.Design.MenuCommand"></see> associated with the command ID, or null if no command is found.
        ///</returns>
        ///<param name="commandID">The <see cref="T:System.ComponentModel.Design.CommandID"></see> to search for. </param>
        public MenuCommand FindCommand(CommandID commandID)
        {
            return commandItems[commandID];
        }

        ///<summary>
        ///Invokes a menu or designer verb command matching the specified command ID.
        ///</summary>
        ///<returns>
        ///true if the command was found and invoked successfully; otherwise, false.
        ///</returns>
        ///<param name="commandID">The <see cref="T:System.ComponentModel.Design.CommandID"></see> of the command to search for and execute. </param>
        public bool GlobalInvoke(CommandID commandID)
        {
            MenuCommand c = FindCommand(commandID);

            if (c != null)
            {
                c.Invoke();
                return true;
            }

            return false;
        }

        ///<summary>
        ///Removes the specified standard menu command from the menu.
        ///</summary>
        ///<param name="command">The <see cref="T:System.ComponentModel.Design.MenuCommand"></see> to remove. </param>
        public void RemoveCommand(MenuCommand command)
        {
            if(command == null)
                throw new ArgumentNullException("command");

            commandItems.Remove(command.CommandID);
        }

        ///<summary>
        ///Removes the specified designer verb from the collection of global designer verbs.
        ///</summary>
        ///<param name="verb">The <see cref="T:System.ComponentModel.Design.DesignerVerb"></see> to remove. </param>
        public void RemoveVerb(DesignerVerb verb)
        {
            verbs.Add(verb);
        }

        ///<summary>
        ///Shows the specified shortcut menu at the specified location.
        ///</summary>
        ///<param name="y">The y-coordinate at which to display the menu, in screen coordinates. </param>
        ///<param name="menuID">The <see cref="T:System.ComponentModel.Design.CommandID"></see> for the shortcut menu to show. </param>
        ///<param name="x">The x-coordinate at which to display the menu, in screen coordinates. </param>
        public void ShowContextMenu(CommandID menuID, int x, int y)
        {
        }

        ///<summary>
        ///Gets or sets an array of the designer verbs that are currently available.
        ///</summary>
        ///<returns>
        ///An array of type <see cref="T:System.ComponentModel.Design.DesignerVerb"></see> that indicates the designer verbs that are currently available.
        ///</returns>
        public DesignerVerbCollection Verbs
        {
            get { return verbs; }
        }

        #endregion

        #region IMenuCreator Members

        /// <summary>
        /// Removes all occurrences of menus for specified command named.
        /// </summary>
        private static void RemoveMenuItem(CommandBar commandBar, string commandMenuName)
        {
            if (commandBar != null)
            {
                while (true)
                {
                    try
                    {
                        CommandBarControl ctl = commandBar.Controls[commandMenuName];
                        ctl.Delete(false);
                    }
                    catch (ArgumentException)
                    {
                        break;
                    }
                }
            }
        }

        private static int GetMenuIndex(int index, ICollection<string> names, CommandBar commandBar)
        {
            try
            {
                if (commandBar != null)
                {
                    if (names != null && names.Count > 0)
                    {
                        foreach (string n in names)
                        {
                            if (!string.IsNullOrEmpty(n))
                            {
                                try
                                {
                                    CommandBarControl button = commandBar.Controls[n];
                                    if (button != null)
                                        return button.Index + 1;
                                }
                                catch
                                {
                                }
                            }
                        }
                    }

                    if (index < 0)
                        return commandBar.Controls.Count + 1;
                }

                return index;
            }
            catch
            {
                if (commandBar != null)
                    return commandBar.Controls.Count + 1;

                return 1;
            }
        }

        /// <summary>
        /// Finds the localized menu name from English menu name.
        /// </summary>
        /// <param name="name">Name of the menu</param>
        /// <returns>Localized name</returns>
        /// <remarks>We have to catch all exceptions since GetString method can throw a very extensive
        /// list of exceptions</remarks>
        public string FindLocalizedName(string name)
        {
            try
            {
                return resourceManager.GetString(string.Concat(cultureInfo.TwoLetterISOLanguageName, name));
            }
            catch
            {
                // tried to find a localized version of the word, but one was not found,
                // so default to the en-US word, which may work for the current culture:
                return name;
            }
        }

        /// <summary>
        /// Checks if currently the SetupUI should be performed.
        /// This flag can be ignored when using Customizator.
        /// </summary>
        public bool IsSetupUI
        {
            get { return isSetupUI; }
            set { isSetupUI = value; }
        }

        /// <summary>
        /// Gets the handle to auxiliary menu-commands service manager.
        /// </summary>
        public IMenuCommandService CommandService
        {
            get { return this; }
        }

        /// <summary>
        /// Menu Customizator.
        /// </summary>
        public IMenuCustomizator Customizator
        {
            get { return menuCustomizator; }
        }

        /// <summary>
        /// Handle to Visual Studio IDE MainMenu.
        /// </summary>
        public CommandBar MainMenu
        {
            get { return mainMenu; }
        }

        /// <summary>
        /// Handle to Visual Studio IDE right-click menu for code window.
        /// </summary>
        public CommandBar CodeWindowMenu
        {
            get { return codeWindowMenu; }
        }

        /// <summary>
        /// Handle to Visual Studio IDE standard toolbar.
        /// </summary>
        public CommandBar StandardToolbar
        {
            get { return standardToolbar; }
        }

        /// <summary>
        /// Returns handle to any menu or toolbar of Visual Studio IDE.
        /// </summary>
        public CommandBar GetCommandBar(string name)
        {
            try
            {
                return vsCommandBars[name];
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Returns handle to any menu or toolbar of Visual Studio IDE with language version independent name.
        /// </summary>
        public CommandBar GetLocalizedCommandBar(string name)
        {
            try
            {
                return vsCommandBars[FindLocalizedName(name)];
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Returns handle to sub menu of Visual Studio MainMenu with given name.
        /// </summary>
        public CommandBar GetMainMenuCommandBar(string name)
        {
            try
            {
                return ((CommandBarPopup) mainMenu.Controls[FindLocalizedName(name)]).CommandBar;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Returns handle to sub menu of CodeWindow menu.
        /// </summary>
        public CommandBar GetCodeWindowCommandBar(string name)
        {
            try
            {
                return ((CommandBarPopup)codeWindowMenu.Controls[FindLocalizedName(name)]).CommandBar;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Creates new command that then can be captured by MenuCommand with the same CommandID.
        /// </summary>
        public Command AddCommand(MenuCommand menuCommand, string name, string caption, int iconIndex, string hotkeyString, string toolTip, bool imageOnly)
        {
            if (menuCommand == null || menuCommand.CommandID == null)
                throw new ArgumentNullException("menuCommand");

            CommandID commandID = menuCommand.CommandID;
            string commandName = (string.IsNullOrEmpty(name) ? commandNames[commandID] : name);
            Command vsCommand;

            // validate name's correctness:
            if (string.IsNullOrEmpty(commandName))
                commandName = commandID.Guid.ToString().Replace("{", "").Replace("}", "").Replace('-', '_') + "_ID" + commandID.ID;
            if (commandNames.ContainsKey(commandID))
                commandNames[commandID] = commandName;
            else commandNames.Add(commandID, commandName);
            
            if(commands.TryGetValue(commandID, out vsCommand) && vsCommand != null)
                return vsCommand;

            // register command:
            try
            {
                string fullCommandName = string.Format("{0}.{1}", addInInstance.ProgID, commandName);
                vsCommand = ((Commands2)appObject.Commands).Item(fullCommandName, -1);
            }
            catch (ArgumentException)
            {
                try
                {
                    object[] contextGuids = new object[] {};
                    vsCommand =
                        ((Commands2) appObject.Commands).AddNamedCommand2(addInInstance, commandName, caption, toolTip,
                                                                          (iconIndex < 0), Math.Abs(iconIndex),
                                                                          ref contextGuids,
                                                                          (int)
                                                                          (vsCommandStatus.vsCommandStatusEnabled |
                                                                           vsCommandStatus.vsCommandStatusSupported),
                                                                          (imageOnly
                                                                               ? (int) vsCommandStyle.vsCommandStylePict
                                                                               : (int)vsCommandStyle.vsCommandStylePictAndText),
                                                                          vsCommandControlType.vsCommandControlTypeButton);


                    if (!string.IsNullOrEmpty(hotkeyString))
                        vsCommand.Bindings = new object[] {hotkeyString};
                }
                catch
                {
                }
            }

            if (vsCommand != null)
            {
                // store for future usage:
                commands.Add(commandID, vsCommand);
                captions.Add(vsCommand, caption);
                AddCommand(menuCommand);
            }

            return vsCommand;
        }

        /// <summary>
        /// Creates new instance of toolbar with specified name.
        /// </summary>
        public CommandBar AddToolbar(string name)
        {
            CommandBar toolBar;

            try
            {
                toolBar = vsCommandBars[name];
            }
            catch
            {
                toolBar = null;
            }

            if (toolBar == null)
            {
                toolBar = vsCommandBars.Add(name, MsoBarPosition.msoBarTop, null, false);
                toolBar.Position = MsoBarPosition.msoBarTop;
                toolBar.RowIndex = 10;
                // toolBar.Visible = true;

                // store for future usage or release:
                toolbars.Add(toolBar);
            }
            else
                toolBar.Visible = true;

            return toolBar;
        }

        /// <summary>
        /// Creates new item with given description.
        /// </summary>
        public CommandBarButton AddButton(CommandBar parentMenu, MenuCommand menuCommand, bool beginGroup, int itemIndex, params string[] afterItemCaption)
        {
            if(parentMenu == null)
                throw new ArgumentNullException("parentMenu");
            if (menuCommand == null || menuCommand.CommandID == null)
                throw new ArgumentNullException("menuCommand");

            CommandID commandID = menuCommand.CommandID;
            Command vsCommand;

            if (!commands.TryGetValue(commandID, out vsCommand) || vsCommand == null)
                throw new ArgumentNullException("menuCommand",
                                                "Not registered VisualStudio IDE command for this menu. Please call AddCommand() first.");

            // remove old button:
            RemoveMenuItem(parentMenu, captions[vsCommand]);
            if(!namedCommands.ContainsKey(vsCommand.Name))
                namedCommands.Add(vsCommand.Name, menuCommand);

            // get the index inside the menu:
            int menuIndex = GetMenuIndex(itemIndex, afterItemCaption, parentMenu);

            // insert inside the tool-bars and menus:
            CommandBarButton vsButton = vsCommand.AddControl(parentMenu, menuIndex) as CommandBarButton;
            if (vsButton != null)
            {
                vsButton.BeginGroup = beginGroup;
                vsButton.Style = MsoButtonStyle.msoButtonIconAndCaption;

                // store for future usage or memory release:
                buttons.Add(vsButton);
                menuControls.Add(vsButton, menuCommand);
            }

            return vsButton;
        }

        /// <summary>
        /// Creates new popup item with given description.
        /// </summary>
        public CommandBarPopup AddPopup(CommandBar parentMenu, string caption, string toolTip, bool beginGroup, int itemIndex, params string[] afterItemCaption)
        {
            CommandBarPopup popupMenu;

            try
            {
                popupMenu = (CommandBarPopup)parentMenu.Controls[caption];
            }
            catch
            {
                popupMenu = null;
            }

            if (popupMenu == null)
            {
                popupMenu =
                    (CommandBarPopup)
                    (parentMenu.Controls.Add(MsoControlType.msoControlPopup, 1, string.Empty,
                                             GetMenuIndex(itemIndex, afterItemCaption, parentMenu), false));

                popupMenu.BeginGroup = beginGroup;
                popupMenu.TooltipText = toolTip;
                if (!string.IsNullOrEmpty(caption) && !string.IsNullOrEmpty(caption.Trim()))
                    popupMenu.Caption = caption;

                // store for future usage or memory release:
                popups.Add(popupMenu);
            }
            else
            {
                // move to given index:
                popupMenu.Move(popupMenu.Parent, GetMenuIndex(itemIndex, afterItemCaption, parentMenu));
                popupMenu.BeginGroup = beginGroup;
                popupMenu.TooltipText = toolTip;
            }

            return popupMenu;
        }

        /// <summary>
        /// Removes given MenuCommand and all the controls associated with it.
        /// </summary>
        public void DeleteCommand(MenuCommand menuCommand)
        {
            if(menuCommand == null || menuCommand.CommandID == null)
                throw new ArgumentNullException("menuCommand");

            CommandID commandID = menuCommand.CommandID;
            Command vsCommand;

            // release all associated controls:
            IList<CommandBarControl> toRemove = new List<CommandBarControl>();
            foreach (KeyValuePair<CommandBarControl, MenuCommand> item in menuControls)
            {
                if(item.Value == menuCommand)
                    toRemove.Add(item.Key);
            }
            foreach (CommandBarControl c in toRemove)
            {
                menuControls.Remove(c);
                if (c is CommandBarButton)
                    buttons.Remove(c as CommandBarButton);

                try
                {
                    c.Delete(null);
                }
                catch(Exception ex)
                {
                    Trace.Write(ex.Message);
                }
            }

            // release item from all dictionaries:
            commandNames.Remove(commandID);
            if (commands.TryGetValue(commandID, out vsCommand))
            {
                commands.Remove(commandID);
                captions.Remove(vsCommand);

                try
                {
                    // release command:
                    vsCommand.Delete();
                }
                catch(Exception ex)
                {
                    Trace.Write(ex.Message);
                }
            }

            // and remove the command from the menu-command service:
            RemoveCommand (menuCommand);
        }

        /// <summary>
        /// Removes given popup menu.
        /// All the child controls (MenuCommands) should be removed separately before call of this function.
        /// </summary>
        public void DeletePopup(CommandBarPopup popupMenu)
        {
            if (popupMenu == null)
                throw new ArgumentNullException("popupMenu");

            // remove from stored collection:
            if (!popups.Remove(popupMenu))
                throw new ArgumentException("popupMenu");

            try
            {
                // release the instance:
                popupMenu.Delete(null);
            }
            catch(Exception ex)
            {
                Trace.Write(ex.Message);
            }
        }

        #endregion

        /// <summary>
        /// Gets the MenuCommand associated with given command's full name.
        /// </summary>
        public MenuCommand GetByName(string name)
        {
            MenuCommand result;

            if (namedCommands.TryGetValue(name, out result))
                return result;
            return null;
        }

        /// <summary>
        /// Release all used VS IDE menu resources.
        /// </summary>
        public void Dispose(bool deleteVsIdeElements)
        {
            verbs.Clear();
            commandItems.Clear();
            commandNames.Clear();
            commands.Clear();
            captions.Clear();
            namedCommands.Clear();

            if (deleteVsIdeElements)
            {
                foreach (CommandBarButton b in buttons)
                {
                    try
                    {
                        b.Delete(null);
                    }
                    catch (Exception ex)
                    {
                        Trace.Write(ex.Message);
                    }
                }

                foreach (CommandBarPopup p in popups)
                {
                    try
                    {
                        p.Delete(null);
                    }
                    catch (Exception ex)
                    {
                        Trace.Write(ex.Message);
                    }
                }

                foreach (CommandBar t in toolbars)
                {
                    try
                    {
                        t.Delete();
                    }
                    catch (Exception ex)
                    {
                        Trace.Write(ex.Message);
                    }
                }
            }

            buttons.Clear();
            popups.Clear();
            toolbars.Clear();
        }
    }
}