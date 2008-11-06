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
        /// <summary>
        /// Name of the configuration settings group.
        /// </summary>
        private const string ConfigurationName = "MultiRenameTool";

        private IPackageEnvironment parent;
        private PersistentStorageData config;
        private MultiRenameForm dlgRename;
        
        #region State Management

        private void PopulateConfig(MultiRenameForm dlg)
        {
            if (config != null && config.Count > 0)
            {
                dlg.Formula = config.GetString("Formula");
                dlg.Counter = new StringHelper.CounterDetails(config.GetUInt("CounterStart", 1),
                                                              config.GetUInt("CounterStep", 1),
                                                              (int) config.GetUInt("CounterDigits", 3));
                dlg.FormatIndex = (int) config.GetUInt("FormatIndex");
            }
        }

        private void StoreConfig(MultiRenameForm dlg)
        {
            if (dlg != null)
            {
                if(config == null)
                    config = ObjectFactory.LoadConfiguration(ConfigurationName);

                StringHelper.CounterDetails counter = dlg.Counter;

                config.Add("Formula", dlg.Formula);
                config.Add("CounterStart", counter.Current);
                config.Add("CounterStep", counter.Step);
                config.Add("CounterDigits", (uint)counter.Digits);
                config.Add("FormatIndex", (uint)dlg.FormatIndex);
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
        /// Gets the current valid configuration for the action. In case of
        /// null-value no settings are actually needed at all.
        /// 
        /// Set is executed at runtime when the configuration for
        /// given action is updated via external module (i.e. Tools->Options).
        /// </summary>
        public PersistentStorageData Configuration
        {
            get { return config; }
            set { config = value; }
        }

        /// <summary>
        /// Performs initialization of this action and
        /// also registers all the UI elements required by the action, e.g.: menus / menu groups / toolbars.
        /// </summary>
        public void Initialize(IPackageEnvironment env, IMenuCreator mc)
        {
            MenuCommand menu = ObjectFactory.CreateCommand(GuidList.guidCmdSet, ID, Execute, BeforeQueryStatus);

            parent = env;

            // load configuration:
            config = ObjectFactory.LoadConfiguration(ConfigurationName);

            // -------------------------------------------------------
            mc.AddCommand(menu, "MultiRenameRefactor", "Multi Method &Rename...", 9004, "Global::Ctrl+R, L", null, false);
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
                    PopulateConfig(dlgRename);
                }

                // show confirmation dialog:
                dlgRename.InitInterface(EditorHelper.FilterMethods(selectionData.AllMethods, vsCMFunction.vsCMFunctionConstructor, vsCMFunction.vsCMFunctionDestructor),
                                        EditorHelper.FilterMethods(selectionData.Methods, vsCMFunction.vsCMFunctionConstructor, vsCMFunction.vsCMFunctionDestructor));
                if (dlgRename.ShowDialog() == DialogResult.OK && dlgRename.ReadInterface(out methods, out names))
                {
                    // remember the latest settings:
                    StoreConfig(dlgRename);

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

        /// <summary>
        /// Executed on Visual Studio exit.
        /// All non-managed resources should be released here.
        /// </summary>
        public void Destroy()
        {
            if (dlgRename != null)
                StoreConfig(dlgRename);
        }

        #endregion
    }
}