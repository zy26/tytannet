using System;
using System.ComponentModel.Design;
using System.Windows.Forms;
using Pretorianie.Tytan.Core.Data;
using Pretorianie.Tytan.Core.Helpers;
using Pretorianie.Tytan.Core.Interfaces;
using System.Diagnostics;

namespace Pretorianie.Tytan.Actions
{
    /// <summary>
    /// Class providing access to external file paths/contents and inserting its data into active code window.
    /// </summary>
    public class InsertionPathRefactor : IPackageAction
    {
        private const string DialogTitle = "Insertion Action";

        private OpenFileDialog dlgFile;
        private SaveFileDialog dlgExport;
        private FolderBrowserDialog dlgFolder;
        private IPackageEnvironment parent;

        #region IPackageAction Members

        /// <summary>
        /// Gets the unique ID of the action.
        /// </summary>
        public int ID
        {
            get { return PackageCmdIDList.toolAction_Insertion; }
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
            MenuCommand fileMenu = ObjectFactory.CreateCommand(GuidList.guidCmdSet, PackageCmdIDList.toolAction_InsertFilePath, ExecuteFilePath, QueryExecute);
            MenuCommand contentMenu = ObjectFactory.CreateCommand(GuidList.guidCmdSet, PackageCmdIDList.toolAction_InsertContentPath, ExecuteFileContent, QueryExecute);
            MenuCommand exportContentMenu = ObjectFactory.CreateCommand(GuidList.guidCmdSet, PackageCmdIDList.toolAction_InsertExportContentPath, ExecuteFileExportContent, QueryExportExecute);
            MenuCommand folderMenu = ObjectFactory.CreateCommand(GuidList.guidCmdSet, PackageCmdIDList.toolAction_InsertFolderPath, ExecuteFolderPath, QueryExecute);

            parent = env;

            // -------------------------------------------------------
            mc.AddCommand(fileMenu, "InsertFilePath", "Insert &File Path...", 9019, null, null, false);
            mc.AddCommand(contentMenu, "InsertFileContent", "Insert File Content As &Binary...", 9020, null, null, false);
            mc.AddCommand(exportContentMenu, "InsertFileExportContent", "Export File C&ontent...", 0, null, null, false);
            mc.AddCommand(folderMenu, "InsertFolderPath", "Insert Folder &Location...", 9018, null, null, false);
            mc.Customizator.AddInsertionItem(folderMenu, false, -1, null);
            mc.Customizator.AddInsertionItem(fileMenu, false, -1, null);
            mc.Customizator.AddInsertionItem(contentMenu, false, -1, null);
            mc.Customizator.AddInsertionItem(exportContentMenu, false, -1, null);
        }

        private void QueryExecute(object sender, EventArgs e)
        {
            MenuCommand m = sender as MenuCommand;

            if (m != null)
                m.Enabled = parent.CurrentEditPoint.IsActiveDocumentValid;
        }

        private void QueryExportExecute(object sender, EventArgs e)
        {
            MenuCommand m = sender as MenuCommand;

            if (m != null)
                m.Enabled = parent.CurrentEditPoint.IsActiveDocumentValid && parent.CurrentEditPoint.IsSelected;
        }

        /// <summary>
        /// Invokes proper processing assigned to current action.
        /// </summary>
        public void Execute(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Executed on Visual Studio exit.
        /// All non-managed resources should be released here.
        /// </summary>
        public void Destroy()
        {
        }

        /// <summary>
        /// Creates secure path - acceptable inside the programming language source-code.
        /// </summary>
        private static string SecurePath(string path)
        {
            return path;
        }

        private static string UnsecurePath(string path)
        {
            return path;
        }

        /// <summary>
        /// Gets the currently selected text.
        /// </summary>
        private string GetSelection()
        {
            CodeEditPoint editPoint = parent.CurrentEditPoint;

            if (editPoint != null && editPoint.IsSelected)
                return editPoint.Selection.Text;

            return null;
        }

        private void CreateFileDialog(bool forPath)
        {
            // configure dialog-box:
            if (dlgFile != null)
                return;

            dlgFile = new OpenFileDialog();
            dlgFile.Title = forPath ? SharedStrings.InsertionPath_FileTitle : SharedStrings.InsertionPath_ContentTitle;
            dlgFile.Filter = "All files|*.*";
            dlgFile.FilterIndex = 0;
            dlgFile.CheckFileExists = false;
            dlgFile.CheckPathExists = false;
            dlgFile.Multiselect = false;
        }

        private void CreateExportFileDialog()
        {
            // configre export dialog-box:
            if (dlgExport != null)
                return;

            dlgExport = new SaveFileDialog();
            dlgExport.Title = SharedStrings.InsertionPath_ContentExportTitle;
            dlgExport.Filter = "All files|*.*";
            dlgExport.FilterIndex = 0;
            dlgExport.AutoUpgradeEnabled = true;
            dlgExport.CheckFileExists = false;
            dlgExport.CheckPathExists = true;
            dlgExport.OverwritePrompt = true;
            dlgExport.SupportMultiDottedExtensions = true;
        }

        /// <summary>
        /// Invokes proper processing assigned to current action.
        /// </summary>
        public void ExecuteFilePath(object sender, EventArgs e)
        {
            string path = GetSelection();

            // configure dialog-box:
            CreateFileDialog(true);

            if (!string.IsNullOrEmpty(path))
                dlgFile.FileName = UnsecurePath(path);

            // and insert selected file's path into currently active document:
            if (dlgFile.ShowDialog() == DialogResult.OK)
                parent.CurrentEditPoint.InsertTextOrReplaceSelection(SharedStrings.UndoContext_InsertFilePath,
                                                                     SecurePath(dlgFile.FileName), true);
        }

        /// <summary>
        /// Invokes proper processing assigned to current action.
        /// </summary>
        public void ExecuteFileContent(object sender, EventArgs e)
        {
            string path = GetSelection();

            // configure dialog-box:
            CreateFileDialog(false);

            if (!string.IsNullOrEmpty(path))
                dlgFile.FileName = UnsecurePath(path);

            // and insert selected file into currently active document:
            if (dlgFile.ShowDialog() == DialogResult.OK)
            {
                string data = ProcessImportFileContent(dlgFile.FileName);

                if (!string.IsNullOrEmpty(data))
                    parent.CurrentEditPoint.InsertTextOrReplaceSelection(SharedStrings.UndoContext_InsertFileContent,
                                                                         data, true);
            }
        }

        /// <summary>
        /// Invokes proper processing assigned to current action.
        /// </summary>
        public void ExecuteFileExportContent(object sender, EventArgs e)
        {
            string selection = GetSelection();

            if (string.IsNullOrEmpty(selection))
                return;

            // configure dialog-box:
            CreateExportFileDialog();

            // and insert currently selected content into specified file:
            if (dlgExport.ShowDialog() == DialogResult.OK)
                ProcessExportFileContent(dlgExport.FileName, selection);
        }

        /// <summary>
        /// Convert the content of given file into a hex array.
        /// </summary>
        private static string ProcessImportFileContent(string fileName)
        {
            try
            {
                return FileHelper.Import(fileName);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
                Trace.WriteLine(ex.StackTrace);

                // show error message to the user:
                MessageBox.Show(ex.Message, DialogTitle);
                return null;
            }
        }

        /// <summary>
        /// Convert the string into binary file.
        /// </summary>
        private static void ProcessExportFileContent(string fileName, string hexContent)
        {
            try
            {
                FileHelper.Export(fileName, hexContent);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
                Trace.WriteLine(ex.StackTrace);

                // show error message to the user:
                MessageBox.Show(ex.Message, DialogTitle);
            }
        }


        /// <summary>
        /// Invokes proper processing assigned to current action.
        /// </summary>
        public void ExecuteFolderPath(object sender, EventArgs e)
        {
            string path = GetSelection();

            if (dlgFolder == null)
            {
                dlgFolder = new FolderBrowserDialog();
                dlgFolder.ShowNewFolderButton = true;
                dlgFolder.Description = SharedStrings.InsertionPath_FolderPath;
            }

            // configure dialog-box:
            if (!string.IsNullOrEmpty(path))
                dlgFolder.SelectedPath = path;

            // and insert selected folder's location into currently active document:
            if (dlgFolder.ShowDialog() == DialogResult.OK)
                parent.CurrentEditPoint.InsertTextOrReplaceSelection(SharedStrings.UndoContext_InsertFolderPath,
                                                                     SecurePath(dlgFolder.SelectedPath), true);
        }

        #endregion
    }
}
