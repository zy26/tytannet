using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Windows.Forms;
using EnvDTE;
using Pretorianie.Tytan.Core.Data;
using Pretorianie.Tytan.Core.Helpers;
using Pretorianie.Tytan.Core.Interfaces;
using Pretorianie.Tytan.Forms;
using System.Diagnostics;

namespace Pretorianie.Tytan.Actions
{
    /// <summary>
    /// Refactor action that enables renaming of multiple methods within a single class based on a given pattern.
    /// </summary>
    public sealed class MultiRenameRefactor : IPackageAction
    {
        private const string PersistantStorageName = "MultiRenameTool";

        private IPackageEnvironment parent;
        private MultiRenameForm dlgRename;
        
        #region State Management

        private static void RestoreState(MultiRenameForm dlg)
        {
            PersistentStorageData data = PersistentStorageHelper.Load(PersistantStorageName);

            if (data != null && data.Count > 0)
            {
                dlg.Formula = data.GetString("Formula");
                dlg.Counter = new StringHelper.CounterDetails(data.GetUInt("CounterStart", 1), data.GetUInt("CounterStep", 1), (int)data.GetUInt("CounterDigits", 3));
                dlg.FormatIndex = (int)data.GetUInt("FormatIndex");
            }
        }

        private static void StoreState(MultiRenameForm dlg)
        {
            if (dlg != null)
            {
                PersistentStorageData data = new PersistentStorageData(PersistantStorageName);
                StringHelper.CounterDetails counter = dlg.Counter;

                data.Add("Formula", dlg.Formula);
                data.Add("CounterStart", counter.Current);
                data.Add("CounterStep", counter.Step);
                data.Add("CounterDigits", (uint)counter.Digits);
                data.Add("FormatIndex", (uint)dlg.FormatIndex);


                // store:
                PersistentStorageHelper.Save(data);
            }
        }

        #endregion

        #region IPackageAction Members

        /// <summary>
        /// Gets the unique ID of the action.
        /// </summary>
        public int ID
        {
            get { return PackageCmdIDList.toolRefactor_MultiRename; }
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
            mc.AddCommand(menu, "MultiRenameRefactor", "Multi Method &Rename...", 9004, "Text Editor::Ctrl+R, L", null, false);
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
            CodeEditSelection selectionData;
            IList<CodeFunction> methods;
            IList<string> names;
            int i = 0;

            if (!editorEditPoint.IsRefactorValid)
                return;

            // get selected methods inside the code editor:
            selectionData = EditorHelper.GetSelectedMethods(editorEditPoint, true);
            if (selectionData != null)
            {
                if (dlgRename == null)
                {
                    dlgRename = new MultiRenameForm();
                    RestoreState(dlgRename);
                }

                // show confirmation dialog:
                dlgRename.InitInterface(EditorHelper.FilterMethods(selectionData.AllMethods, vsCMFunction.vsCMFunctionConstructor, vsCMFunction.vsCMFunctionDestructor),
                                        EditorHelper.FilterMethods(selectionData.Methods, vsCMFunction.vsCMFunctionConstructor, vsCMFunction.vsCMFunctionDestructor));
                if (dlgRename.ShowDialog() == DialogResult.OK && dlgRename.ReadInterface(out methods, out names))
                {
                    // open the undo-context to combine all the modifications of the source code into one:
                    parent.DTE.UndoContext.Open(SharedStrings.UndoContext_MultiRenameRefactor, true);

                    // update the source code:
                    if (methods != null && names != null)
                    {
                        foreach (CodeFunction m in methods)
                        {
                            try
                            {
                                // update the names:
                                if (m.Name != names[i])
                                    m.Name = names[i];
                            }
                            catch (Exception ex)
                            {
                                Trace.WriteLine(ex.Message);
                            }

                            i++;
                        }
                    }

                    // close the undo-context, so all the changes will be threated as one:
                    parent.DTE.UndoContext.Close();
                }
            }
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Release all used resources.
        /// </summary>
        public void Dispose()
        {
            if (dlgRename != null)
                StoreState(dlgRename);

            GC.SuppressFinalize(this);
        }

        #endregion
    }
}