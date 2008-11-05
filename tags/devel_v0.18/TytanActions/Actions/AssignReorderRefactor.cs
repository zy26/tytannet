using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;
using EnvDTE;
using Pretorianie.Tytan.Core.Data;
using Pretorianie.Tytan.Core.Interfaces;
using Pretorianie.Tytan.Core.Helpers;

namespace Pretorianie.Tytan.Actions
{
    public sealed class AssignReorderRefactor : IPackageAction
    {
        private IPackageEnvironment parent;
        
        #region ProcessText

        private static bool ProcessText(ICollection<string> selectedLines, out string[] resultLines, CodeModelLanguages language)
        {
            resultLines = new string[selectedLines.Count];
            int index = 0;
            string assign;

            foreach (string l in selectedLines)
            {
                int position = GetPosition(l, '=');
                if (position >= 0)
                {
                    assign = l.Substring(position + 1).Trim ();
                    bool isSemicolon;
                    if (assign.EndsWith(";"))
                    {
                        isSemicolon = true;
                        assign = assign.Substring(0, assign.Length - 1).Trim();
                    }
                    else isSemicolon = false;

                    resultLines[index] = string.Format("{0} = {1}{2}", assign, l.Substring(0, position - 1), (isSemicolon ? ";" : string.Empty));
                }
                else
                    resultLines[index] = l;

                index++;
            }

            return true;
        }

        private static int GetPosition(string text, char data)
        {
            int counter1 = 0;
            int counter2 = 0;

            if ( string.IsNullOrEmpty(text))
                return -1;

            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == '\'')
                    counter1 = (counter1 + 1) % 2;
                if (text[i] == '"')
                    counter2 = (counter2 + 1) % 2;

                if (counter1 == 0 && counter2 == 0 && text[i] == data)
                    return i;
            }

            // not found:
            return -1;
        }

        #endregion

        #region IPackageAction Members

        /// <summary>
        /// Gets the unique ID of the action.
        /// </summary>
        public int ID
        {
            get { return PackageCmdIDList.toolRefactor_Assignment; }
        }

        /// <summary>
        /// Performs initialization of this action and
        /// also registers all the UI elements required by the action, e.g.: menus / menu groups / toolbars.
        /// </summary>
        public void Initialize(IPackageEnvironment env, IMenuCommandService mcs, IMenuCreator mc)
        {
            MenuCommand menu = ObjectFactory.CreateCommand(GuidList.guidCmdSet, ID, Execute, BeforeQueryStatus);

            parent = env;
            mcs.AddCommand(menu);

            // -------------------------------------------------------
            mc.AddCommand(menu, "AssignReorderRefactor", "Reorder &Assignments...", 9002, "Text Editor::Ctrl+R, A", null, false);
            mc.Customizator.AddRefactoring(menu, true, -1, null);
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
            string[] selectedLines;
            string[] resultLines;
            StringBuilder result;
            EditPoint top, bottom;

            if (!editorEditPoint.IsRefactorValid)
                return;

            // get selected lines or current one:
            editorEditPoint.SelectFullLines(vsStartOfLineOptions.vsStartOfLineOptionsFirstText, out top, out bottom);

            selectedLines = editorEditPoint.Selection.Text.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            // get text and process it:
            if (ProcessText(selectedLines, out resultLines, editorEditPoint.CodeLanguage))
            {
                result = new StringBuilder();

                // convert to text
                if (resultLines != null && resultLines.Length > 0)
                    foreach (string r in resultLines)
                        result.AppendLine(r);

                try
                {
                    // open the undo-context to combine all the modifications of the source code into one:
                    parent.DTE.UndoContext.Open(SharedStrings.UndoContext_AssignReorderRefactor, true);

                    // paste processed text:
                    top.ReplaceText(bottom, result.ToString().Trim(), (int)vsEPReplaceTextOptions.vsEPReplaceTextAutoformat);

                    // remove the selection:
                    editorEditPoint.Selection.MoveToPoint(bottom, false);
                }
                finally
                {
                    // close the undo-context, so all the changes will be threated as one:
                    parent.DTE.UndoContext.Close();
                }
            }
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Release used resources.
        /// </summary>
        public void Dispose()
        {
        }

        #endregion
    }
}