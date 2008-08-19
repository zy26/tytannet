using System;
using System.ComponentModel.Design;
using Microsoft.VisualStudio.Shell;
using Pretorianie.Tytan.Core.Interfaces;

namespace Pretorianie.Tytan.Actions
{
    /// <summary>
    /// Refactoring AddIn that generates resources based on selected text or other data embedded directly inside code files.
    /// </summary>
    public sealed class ExtractResourceRefactor : IPackageAction
    {
        private IPackageEnvironment parent;

        #region IPackageAction Members
        
        /// <summary>
        /// Gets the unique ID of the action.
        /// </summary>
        public int ID
        {
            get { return PackageCmdIDList.toolRefactor_ExtractResource; }
        }

        /// <summary>
        /// Performs initialization of this action and
        /// also registers all the UI elements required by the action, e.g.: menus / menu groups / toolbars.
        /// </summary>
        public void Initialize(IPackageEnvironment env, IMenuCommandService mcs, IMenuCreator mc)
        {
            CommandID cmdID = new CommandID(GuidList.guidCmdSet, ID);
            OleMenuCommand menu = new OleMenuCommand(Execute, cmdID);

            menu.BeforeQueryStatus += BeforeQueryStatus;

            parent = env;
            mcs.AddCommand(menu);

            // -------------------------------------------------------
            mc.AddCommand(menu, "ExtractResourceRefactor", "Extract &To Resource...", 9006, "Text Editor::Ctrl+R, T", null, false);
            mc.Customizator.AddRefactoring(menu, false, -1, null);
        }

        void BeforeQueryStatus(object sender, EventArgs e)
        {
            OleMenuCommand menu = sender as OleMenuCommand;

            if (menu != null)
                menu.Enabled = parent.CurrentEditPoint.IsRefactorValid;
        }

        /// <summary>
        /// Invokes proper processing assigned to current action.
        /// </summary>
        public void Execute(object sender, EventArgs e)
        {
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
        }

        #endregion
    }
}