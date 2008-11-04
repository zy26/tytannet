using System;

namespace Pretorianie.Tytan.Core.Interfaces
{
    /// <summary>
    /// Interface describing a package action that can be executed by user from
    /// the VisualStudio menus/toolbars and is provided by given package.
    /// </summary>
    public interface IPackageAction : IDisposable
    {
        /// <summary>
        /// Gets the unique ID of the action.
        /// </summary>
        int ID
        { get; }

        /// <summary>
        /// Performs initialization of this action and
        /// also registers all the UI elements required by the action, e.g.: menus / menu groups / toolbars.
        /// </summary>
        void Initialize(IPackageEnvironment env, System.ComponentModel.Design.IMenuCommandService mcs, IMenuCreator mc);

        /// <summary>
        /// Invokes proper processing assigned to current action.
        /// </summary>
        void Execute(object sender, EventArgs e);
    }
}
