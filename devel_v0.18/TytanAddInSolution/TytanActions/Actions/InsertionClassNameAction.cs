using System;
using System.ComponentModel.Design;
using EnvDTE;
using Pretorianie.Tytan.Core.Data;
using Pretorianie.Tytan.Core.Helpers;
using Pretorianie.Tytan.Core.Interfaces;
using Pretorianie.Tytan.Data;

namespace Pretorianie.Tytan.Actions
{
    /// <summary>
    /// Action that inserts generated class-name into current document.
    /// </summary>
    public class InsertionClassNameAction : IPackageAction
    {
        private IPackageEnvironment parent;
        private NameProvider nameGenerator;

        /// <summary>
        /// Gets the unique ID of the action.
        /// </summary>
        public int ID
        {
            get { return PackageCmdIDList.toolAction_InsertClassName; }
        }

        /// <summary>
        /// Performs initialization of this action and
        /// also registers all the UI elements required by the action, e.g.: menus / menu groups / toolbars.
        /// </summary>
        public void Initialize(IPackageEnvironment env, IMenuCommandService mcs, IMenuCreator mc)
        {
            MenuCommand nameMenu = ObjectFactory.CreateCommand(GuidList.guidCmdSet, ID, Execute, BeforeExecute);

            parent = env;
            mcs.AddCommand(nameMenu);

            // -------------------------------------------------------
            mc.AddCommand(nameMenu, "InsertClassName", "Insert Class &Name...", 0, null, null, false);
            mc.Customizator.AddInsertionItem(nameMenu, true, -1, null);

            // -------------------------------------------------------
            string[] pre = new string[]
                               {
                                   "Byte", "String", "Object", "Thread", "Int", "Custom", "Type", "Editor",
                                   "Environment", "Code", "File", "Socket", "Handle", "Image", "Picture", "Icon", "Bitmap", "Color",
                                   "Smart", "Stupid", "Secure", "Custom", "Client", "Server", "Remote", "Random",
                                   "MultiByte", "Shared", "Box", "Static", "Dynamic"
                               };
            string[] body = new string[]
                                {
                                    "Provider", "Executor", "Factory", "Locator", "Helper", "Util", "Record",
                                    "Randomizer", "Modeler", "Listener", "Serializer", "Generator", "Consumer"
                                };
            string[] post = new string[]
                                {
                                    "Info", "Ext", "Extension", "Param", "Event", "Args", "Contract", "Annotation",
                                    "Attribute", "Action", "Array", "Mode", "Comparer", "Metadata", "Data",
                                };

            // and create random-names-generator:
            nameGenerator = new NameProvider(pre, 0.8, body, 1, post, 0.5);
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
            bool isSelection = editorEditPoint.IsSelected;
            string newName = nameGenerator.NextName();

            try
            {
                // open the undo-context to combine all the modifications of the source code into one:
                parent.DTE.UndoContext.Open(SharedStrings.UndoContext_ExtractPropertyRefactor, true);

                if (isSelection)
                {
                    // paste into selected text:
                    editorEditPoint.Selection.Insert(newName, (int)vsInsertFlags.vsInsertFlagsContainNewText);
                }
                else
                {
                    // just insert text:
                    editorEditPoint.EditPoint.Insert(newName);
                }
            }
            finally
            {
                // close the undo-context, so all the changes will be threated as one:
                parent.DTE.UndoContext.Close();
            }
        }

        #region Dispose

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
        }

        #endregion
    }
}
