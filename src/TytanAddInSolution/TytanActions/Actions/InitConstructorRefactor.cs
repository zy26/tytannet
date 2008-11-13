using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Windows.Forms;
using EnvDTE;
using Pretorianie.Tytan.Core.Data;
using Pretorianie.Tytan.Core.Helpers;
using Pretorianie.Tytan.Core.Interfaces;
using Pretorianie.Tytan.Forms;

namespace Pretorianie.Tytan.Actions
{
    public sealed class InitConstructorRefactor : IPackageAction
    {
        private IPackageEnvironment parent;
        private InitConstructorRefactorForm cfgDialog;

        #region Refactor

        private bool Refactor(IList<CodeVariable> vars, IList<CodeVariable> disabledVars, string codeClassName, string languageGuid, CodeElements codeMembers, EditPoint insertLocation, out int gotoLine)
        {
            // set invalid goto-line:
            gotoLine = -1;

            if (vars != null)
            {
                // get the language and a list of currently available properties:
                CodeModelLanguages language = CodeHelper.GetCodeLanguage(languageGuid);
                IList<string> paramNames = NameHelper.GetParameterNames(vars, language);

                if (cfgDialog == null)
                {
                    cfgDialog = new InitConstructorRefactorForm();
                }

                cfgDialog.InitInterface(vars, disabledVars, paramNames);

                if (cfgDialog.ShowDialog() == DialogResult.OK && cfgDialog.ReadInterface(out vars, out paramNames))
                {
                    // generate code based on user modifications:
                    string code = GenerateSourceCodeOutput(codeClassName, vars, null, language);

                    // get all variables:
                    EditorHelper.GetList<CodeVariable>(codeMembers, vsCMElement.vsCMElementVariable);

                    // insert code to the editor:
                    if (!string.IsNullOrEmpty(code))
                    {
                        insertLocation.ReplaceText(insertLocation, code, (int)vsEPReplaceTextOptions.vsEPReplaceTextAutoformat);

                        // jump without selection to the insertion place:
                        gotoLine = insertLocation.Line + 2;
                        return true;
                    }
                }
            }

            return false;
        }

        private static string GenerateSourceCodeOutput(string codeClassName, IList<CodeVariable> vars, IList<string> paramNames, CodeModelLanguages language)
        {
            CodeTypeMemberCollection code = new CodeTypeMemberCollection();
            CodeTypeMember initConstructor = VariableHelper.GetInitConstructor(codeClassName, vars, paramNames, language);

            code.Add(initConstructor);
            return Environment.NewLine + CodeHelper.GenerateFromMember(language, code);
        }

        #endregion

        #region IPackageAction Members

        /// <summary>
        /// Gets the unique ID of the action.
        /// </summary>
        public int ID
        {
            get { return PackageCmdIDList.toolRefactor_InitConstructor; }
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
            MenuCommand menu = ObjectFactory.CreateCommand(GuidList.guidCmdSet, ID, Execute, BeforeQueryStatus);

            parent = env;

            // -------------------------------------------------------
            mc.AddCommand(menu, "InitConstructorRefactor", "Init &Constructor...", 9003, "Global::Ctrl+R, C", null, false);
            mc.Customizator.AddRefactoring(menu, false, -1, null);
        }

        void BeforeQueryStatus(object sender, EventArgs e)
        {
            MenuCommand menu = sender as MenuCommand;

            if (menu != null)
                menu.Enabled = parent.CurrentEditPoint.IsRefactorValid;
        }

        /// <summary>
        /// Invokes proper processing assigned to current action.
        /// </summary>
        public void Execute(object sender, EventArgs e)
        {
            CodeEditPoint editorEditPoint = parent.CurrentEditPoint;
            bool isRefactored;

            if (!editorEditPoint.IsRefactorValid)
                return;

            // get selected variables:
            CodeEditSelection editorSelection = EditorHelper.GetSelectedVariables(editorEditPoint);

            if (editorSelection != null && editorSelection.Variables != null)
            {
                IList<CodeVariable> vars = editorSelection.GetParentMembers<CodeVariable>(vsCMElement.vsCMElementVariable);
                EditPoint ep;

                if (vars != null)
                    ep = vars[vars.Count - 1].GetEndPoint(vsCMPart.vsCMPartWholeWithAttributes).CreateEditPoint();
                else ep = editorSelection.Variables[editorSelection.Variables.Count - 1].GetEndPoint(vsCMPart.vsCMPartWholeWithAttributes).CreateEditPoint();

                int gotoLine;
                try
                {
                    // open the undo-context to combine all the modifications of the source code into one:
                    parent.DTE.UndoContext.Open(SharedStrings.UndoContext_InitConstructorRefactor, true);

                    // move cursor to the end of the line, not to interrupt any comments:
                    ep.EndOfLine();

                    // update the source code:
                    isRefactored = Refactor(editorSelection.AllVariables, editorSelection.DisabledVariables, editorSelection.ParentName, editorSelection.ParentLanguage, editorSelection.CodeMembers, ep, out gotoLine);
                }
                finally
                {
                    // close the undo-context, so all the changes will be threated as one:
                    parent.DTE.UndoContext.Close();
                }

                // jump without selection to the insertion place:
                if (isRefactored && gotoLine >= 0)
                    editorEditPoint.GotoLine(gotoLine, 1);
            }
        }

        /// <summary>
        /// Executed on Visual Studio exit.
        /// All non-managed resources should be released here.
        /// </summary>
        public void Destroy()
        {
        }

        #endregion
    }
}