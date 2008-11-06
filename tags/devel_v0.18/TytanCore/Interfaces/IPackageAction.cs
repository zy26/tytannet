using System;
using Pretorianie.Tytan.Core.Data;

namespace Pretorianie.Tytan.Core.Interfaces
{
    /// <summary>
    /// Interface describing a package action that can be executed by user from
    /// the Visual Studio menus/toolbars and is provided by given package.
    /// </summary>
    public interface IPackageAction
    {
        /// <summary>
        /// Gets the unique ID of the action.
        /// </summary>
        int ID
        { get; }

        /// <summary>
        /// Gets the current valid configuration for the action. In case of
        /// null-value no settings are actually needed at all.
        /// 
        /// Set is executed at runtime when the configuration for
        /// given action is updated via external module (i.e. Tools->Options).
        /// </summary>
        PersistentStorageData Configuration
        { get; set; }

        /// <summary>
        /// Performs initialization of this action and
        /// also registers all the UI elements required by the action, e.g.: menus / menu groups / toolbars.
        /// </summary>
        void Initialize(IPackageEnvironment env, IMenuCreator mc);

        /// <summary>
        /// Invokes proper processing assigned to current action.
        /// </summary>
        void Execute(object sender, EventArgs e);

        /// <summary>
        /// Executed on Visual Studio exit.
        /// All non-managed resources should be released here.
        /// </summary>
        void Destroy();
    }
}
