using System;
using System.ComponentModel.Design;
using Pretorianie.Tytan.Core.Data;
using Pretorianie.Tytan.Core.Helpers;
using Pretorianie.Tytan.Core.Interfaces;
using Pretorianie.Tytan.Database.Helpers;

namespace Pretorianie.Tytan.Actions
{
    /// <summary>
    /// Action class that helps in edition/insertion of database connection string (settings).
    /// </summary>
    public class InsertionDatabaseRefactor : IPackageAction
    {
        private IPackageEnvironment parent;

        /// <summary>
        /// Gets the unique ID of the action.
        /// </summary>
        public int ID
        {
            get { return PackageCmdIDList.toolAction_InsertDatabaseConnection; }
        }

        /// <summary>
        /// Gets the current valid configuration for the action. In case of
        /// null-value no settings are actually needed at all.
        /// 
        /// Set is executed at runtime when the configuration for
        /// given action is updated via external module (i.e. Tools->Options).
        /// </summary>
        public PersistentStorageData Configuration
        {
            get { return null; }
            set { }
        }

        /// <summary>
        /// Performs initialization of this action and
        /// also registers all the UI elements required by the action, e.g.: menus / menu groups / toolbars.
        /// </summary>
        public void Initialize(IPackageEnvironment env, IMenuCreator mc)
        {
            MenuCommand databaseMenu = ObjectFactory.CreateCommand(GuidList.guidCmdSet, ID, Execute, BeforeExecute);

            parent = env;

            // -------------------------------------------------------
            mc.AddCommand(databaseMenu, "InsertConnectionString", "Insert &Connection String...", 9016, "Global::Ctrl+R, S", null, false);
            mc.Customizator.AddInsertionItem(databaseMenu, true, -1, null);
        }

        private void BeforeExecute(object sender, EventArgs e)
        {
            MenuCommand m = sender as MenuCommand;

            if (m != null)
                m.Enabled = parent.CurrentEditPoint.IsActiveDocumentValid;
        }

        /// <summary>
        /// Invokes proper processing assigned to current action.
        /// </summary>
        public void Execute(object sender, EventArgs e)
        {
            CodeEditPoint editorEditPoint = parent.CurrentEditPoint;
            string cs = null;
            bool isSelected = editorEditPoint.IsSelected;

            if (isSelected)
                cs = editorEditPoint.Selection.Text;

            // ask user for connection string:
            if (ConnectionHelper.PromptConnectionString(cs, out cs))
                InsertDatabaseConnection(editorEditPoint, isSelected, cs);
        }

        /// <summary>
        /// Executed on Visual Studio exit.
        /// All non-managed resources should be released here.
        /// </summary>
        public void Destroy()
        {
        }

        private static void InsertDatabaseConnection(CodeEditPoint editorEditPoint, bool isSelection, string connectionString)
        {
            // if selected text is equal to the one accepted by user - there is no point in editor update:
            if (isSelection && string.Compare(editorEditPoint.Selection.Text, connectionString, true) == 0)
                return;

            editorEditPoint.InsertTextOrReplaceSelection(SharedStrings.UndoContext_InsertConnectionString,
                                                         connectionString, true);
        }
    }
}
