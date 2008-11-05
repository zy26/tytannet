using System;
using System.ComponentModel.Design;
using EnvDTE;
using Pretorianie.Tytan.Core.Data;
using Pretorianie.Tytan.Core.Helpers;
using Pretorianie.Tytan.Core.Interfaces;

namespace Pretorianie.Tytan.Actions
{
    /// <summary>
    /// Class defining insertion of Guid text or attribute at cursor.
    /// </summary>
    public class InsertionGuidRefactor : IPackageAction
    {
        private IPackageEnvironment parent;
        private Guid prevGuid = Guid.Empty;

        /// <summary>
        /// Gets the unique ID of the action.
        /// </summary>
        public int ID
        {
            get { return PackageCmdIDList.toolAction_Insertion; }
        }

        /// <summary>
        /// Performs initialization of this action and
        /// also registers all the UI elements required by the action, e.g.: menus / menu groups / toolbars.
        /// </summary>
        public void Initialize(IPackageEnvironment env, IMenuCommandService mcs, IMenuCreator mc)
        {
            MenuCommand guidMenu = ObjectFactory.CreateCommand(GuidList.guidCmdSet, PackageCmdIDList.toolAction_InsertGuid, ExecuteGuid, QueryExecuteGuid);
            MenuCommand prevGuidMenu = ObjectFactory.CreateCommand(GuidList.guidCmdSet, PackageCmdIDList.toolAction_InsertPrevGuid, ExecutePrevGuid, QueryExecutePrevGuid);

            parent = env;
            mcs.AddCommand(guidMenu);
            mcs.AddCommand(prevGuidMenu);

            // -------------------------------------------------------
            mc.AddCommand(guidMenu, "InsertGuid", "Insert New &Guid", 0, null, null, false);
            mc.AddCommand(prevGuidMenu, "InsertPrevGuid", "Insert &Previous Guid", 0, null, null, false);
            mc.Customizator.AddInsertionItem(guidMenu, false, 1, null);
            mc.Customizator.AddInsertionItem(prevGuidMenu, false, 2, null);
        }

        private void ExecuteGuid(object sender, EventArgs e)
        {
            prevGuid = Guid.NewGuid();

            InsertGuid(prevGuid);
        }

        private void ExecutePrevGuid(object sender, EventArgs e)
        {
            InsertGuid(prevGuid);
        }

        private void QueryExecuteGuid(object sender, EventArgs e)
        {
            MenuCommand m = sender as MenuCommand;

            if (m != null)
                m.Enabled = parent.CurrentEditPoint.IsActiveDocumentValid;
        }

        private void QueryExecutePrevGuid(object sender, EventArgs e)
        {
            MenuCommand m = sender as MenuCommand;

            // enable that option only when at least one GUID element has been inserted:
            if (m != null)
                m.Enabled = prevGuid != Guid.Empty && parent.CurrentEditPoint.IsActiveDocumentValid;
        }

        private void InsertGuid(Guid guid)
        {
            CodeEditPoint editorEditPoint = parent.CurrentEditPoint;

            try
            {
                // open the undo-context to combine all the modifications of the source code into one:
                parent.DTE.UndoContext.Open(SharedStrings.UndoContext_InsertGuid, true);

                // paste as Guid attribute of class or structure:
                if (!ProcessClassGuidAttribute(editorEditPoint, guid.ToString().ToUpper()))
                {
                    // otherwise type as normal text:
                    editorEditPoint.EditPoint.Insert(guid.ToString().ToUpper());
                }
            }
            finally
            {
                // close the undo-context, so all the changes will be threated as one:
                parent.DTE.UndoContext.Close();
            }
        }

        private static bool ProcessClassGuidAttribute(CodeEditPoint editorEditPoint, string guid)
        {
            CodeFunction currentFunction =
                editorEditPoint.GetCurrentCodeElement<CodeFunction>(vsCMElement.vsCMElementFunction);

            // if inside the function, then applying of an attribute to class/struct is prohibited:
            if (currentFunction != null)
                return false;

            CodeClass currentClass = editorEditPoint.GetCurrentCodeElement<CodeClass>(vsCMElement.vsCMElementClass);
            CodeStruct currentStruct = editorEditPoint.GetCurrentCodeElement<CodeStruct>(vsCMElement.vsCMElementStruct);
            EditPoint start = null;
            CodeModelLanguages language = CodeModelLanguages.Unknown;

            // find the start location of current class:
            if (currentClass != null)
            {
                start = currentClass.GetStartPoint(vsCMPart.vsCMPartHeader).CreateEditPoint();
                language = CodeHelper.GetCodeLanguage(currentClass.Language);
            }

            // find the start location of current structure:
            if (currentStruct != null)
            {
                start = currentStruct.GetStartPoint(vsCMPart.vsCMPartHeader).CreateEditPoint();
                language = CodeHelper.GetCodeLanguage(currentStruct.Language);
            }

            // append attributes at the 'start' location:
            if (start != null)
            {
                string sourceCodeSnippet = CodeHelper.GenerateFromAttribute(language,
                                                                            VariableHelper.GetGuidAttribute(guid));

                if (language == CodeModelLanguages.VisualBasic)
                    sourceCodeSnippet += Environment.NewLine;

                start.ReplaceText(start, sourceCodeSnippet, (int) vsEPReplaceTextOptions.vsEPReplaceTextAutoformat);
                return true;
            }

            // nothing special found... just insert the Guid string...
            return false;
        }

        /// <summary>
        /// Invokes proper processing assigned to current action.
        /// </summary>
        public void Execute(object sender, EventArgs e)
        {
        }

        #region Dispose

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
        }

        #endregion
    }
}