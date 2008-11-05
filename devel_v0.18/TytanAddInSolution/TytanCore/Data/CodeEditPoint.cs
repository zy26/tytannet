using EnvDTE;
using EnvDTE80;
using Pretorianie.Tytan.Core.Helpers;
using Pretorianie.Tytan.Core.Interfaces;

namespace Pretorianie.Tytan.Core.Data
{
    /// <summary>
    /// Parameters passed to the refactor that describe current editor state.
    /// </summary>
    public class CodeEditPoint
    {
        private readonly DTE2 application;
        private readonly TextSelection selection;
        private readonly VirtualPoint virtualStartEditPoint;
        private EditPoint startEditPoint;
        private readonly ProjectItem projectItem;

        /// <summary>
        /// Init constructor.
        /// </summary>
        public CodeEditPoint(DTE2 application, TextSelection selection, VirtualPoint virtualStartEditPoint, ProjectItem projectItem)
        {
            this.application = application;
            this.selection = selection;
            this.virtualStartEditPoint = virtualStartEditPoint;
            this.projectItem = projectItem;
        }

        #region Properties

        /// <summary>
        /// Gets the VS IDE application object.
        /// </summary>
        public DTE2 Application
        {
            get { return application; }
        }

        /// <summary>
        /// Gets the current text selection inside the editor.
        /// </summary>
        public TextSelection Selection
        {
            get { return selection; }
        }

        /// <summary>
        /// Gets the current edit point inside the editor.
        /// </summary>
        public EditPoint EditPoint
        {
            get
            {
                if (startEditPoint == null && virtualStartEditPoint != null)
                    startEditPoint = virtualStartEditPoint.CreateEditPoint();

                return startEditPoint;
            }
        }

        /// <summary>
        /// Gets the currently edited project item.
        /// </summary>
        public ProjectItem ProjectItem
        {
            get { return projectItem; }
        }

        /// <summary>
        /// Gets the language of the current project item.
        /// </summary>
        public FileCodeModel CodeModel
        {
            get { return projectItem.FileCodeModel; }
        }

        /// <summary>
        /// Gets the language of the current project item.
        /// </summary>
        public CodeModelLanguages CodeLanguage
        {
            get { return CodeHelper.GetCodeLanguage(projectItem.FileCodeModel.Language); }
        }

        /// <summary>
        /// Checks if all internal parameters are valid to process refactoring.
        /// </summary>
        public bool IsRefactorValid
        {
            get { return application != null && application.ActiveDocument != null && projectItem != null && projectItem.FileCodeModel != null && projectItem.FileCodeModel.Language != null; }
        }

        /// <summary>
        /// Checks if there is a file opened.
        /// </summary>
        public bool IsActiveDocumentValid
        {
            get { return application != null && application.ActiveDocument != null; }
        }

        /// <summary>
        /// Checks if any text is currently selected.
        /// </summary>
        public bool IsSelected
        {
            get { return IsActiveDocumentValid && selection != null && !selection.IsEmpty; }
        }

        #endregion

        #region Navigation

        /// <summary>
        /// Goes to specified line in the text editor and jumps to particular word in that line.
        /// </summary>
        public void GotoLine(int line, int word)
        {
            selection.GotoLine(line, false);
            if (word > 0)
                selection.WordRight(false, word);
        }

        /// <summary>
        /// Extends selection the the beginning and end of the lines.
        /// </summary>
        public void SelectFullLines(vsStartOfLineOptions startOptions, out EditPoint topPoint, out EditPoint bottomPoint)
        {
            topPoint = selection.TopPoint.CreateEditPoint();
            topPoint.StartOfLine();

            // move to the first word on the right:
            if (startOptions == vsStartOfLineOptions.vsStartOfLineOptionsFirstText)
                topPoint.WordRight(1);

            bottomPoint = selection.BottomPoint.CreateEditPoint();
            bottomPoint.EndOfLine();

            selection.MoveToPoint(topPoint, false);
            selection.MoveToPoint(bottomPoint, true);
        }

        #endregion

        #region Code Access Methods

        /// <summary>
        /// Gets the current code element available at cursor point or null if specified type does not exist.
        /// </summary>
        public T GetCurrentCodeElement<T>(vsCMElement elementType) where T : class
        {
            try
            {
                return projectItem.FileCodeModel.CodeElementFromPoint(EditPoint, elementType) as T;
            }
            catch
            {
                return null;
            }
        }

        #endregion

        #region Text Manipulation

        /// <summary>
        /// Inserts text at current cursor location or replaces the one that is currently selected.
        /// Undo will be used to name the action in Visual Studio's UndoRedo editor.
        /// </summary>
        public void InsertTextOrReplaceSelection(string undoContextName, string newText)
        {
            try
            {
                // open the undo-context to combine all the modifications of the source code into one:
                application.UndoContext.Open(undoContextName, true);

                if (IsSelected)
                {
                    // paste into selected text:
                    selection.Insert(newText, (int) vsInsertFlags.vsInsertFlagsContainNewText);
                }
                else
                {
                    // just insert text:
                    EditPoint.Insert(newText);
                }
            }
            finally
            {
                // close the undo-context, so all the changes will be threated as one:
                application.UndoContext.Close();
            }
        }

        #endregion
    }
}
