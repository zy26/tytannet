using System;
using System.ComponentModel.Design;
using Microsoft.VisualStudio.Shell;
using Pretorianie.Tytan.Core.Data;

namespace Pretorianie.Tytan.Core.Helpers
{
    /// <summary>
    /// Creator of objects.
    /// </summary>
    public static class ObjectFactory
    {
        /// <summary>
        /// Creates new <see cref="MenuCommand"/> object associated with given command GUID and ID.
        /// Specified handler will be invoked when command is selected.
        /// </summary>
        /// <param name="menuGroup"><see cref="Guid"/> of the menu group.</param>
        /// <param name="commandID">ID of the command.</param>
        /// <param name="executeHandler">Method to execute when menu is selected.</param>
        public static MenuCommand CreateCommand(Guid menuGroup, int commandID, EventHandler executeHandler)
        {
            return new MenuCommand(executeHandler, new CommandID(menuGroup, commandID));
        }

        /// <summary>
        /// Creates new <see cref="MenuCommand"/> object associated with given command GUID and ID.
        /// Specified handlers will be invoked when command is selected and before state validation on the screen.
        /// </summary>
        /// <param name="menuGroup"><see cref="Guid"/> of the menu group.</param>
        /// <param name="commandID">ID of the command.</param>
        /// <param name="executeHandler">Method to execute when menu is selected.</param>
        /// <param name="beforeQueryStatusHandler">Method to execute to validate status of the menu item.</param>
        public static MenuCommand CreateCommand(Guid menuGroup, int commandID, EventHandler executeHandler, EventHandler beforeQueryStatusHandler)
        {
            return new OleMenuCommand(executeHandler, null, beforeQueryStatusHandler, new CommandID(menuGroup, commandID));
        }

        /// <summary>
        /// Loads the configuration with specified name.
        /// </summary>
        /// <param name="name">Unique name of the configuration to load.</param>
        public static PersistentStorageData LoadConfiguration(string name)
        {
            return PersistentStorageHelper.Load(name);
        }
    }
}
