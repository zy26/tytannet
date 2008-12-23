using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using EnvDTE;
using Pretorianie.Tytan.Core.Data;
using Pretorianie.Tytan.Core.Helpers;
using Pretorianie.Tytan.Core.Interfaces;
using Pretorianie.Tytan.Forms;

namespace Pretorianie.Tytan.Actions
{
    /// <summary>
    /// Refactoring expanding enumeration based on selected instruction (variable definition) type.
    /// </summary>
    public class ExpandEnumRefactor : IPackageAction
    {
        private IPackageEnvironment parent;
        private ExpandEnumForm dlgExpand;

        #region Implementation of IPackageAction

        /// <summary>
        /// Gets the unique ID of the action.
        /// </summary>
        public int ID
        {
            get { return PackageCmdIDList.toolRefactor_ExpandEnum; }
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
            mc.AddCommand(menu, "ExpandEnumRefactor", "Ex&pand Switch From Enum...", 9021, "Global::Ctrl+R, J", null, false);
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
            CodeEditPoint point = parent.CurrentEditPoint;
            CodeTypeRef type = null;
            IList<string> names = null;

            if (point != null)
            {
                // check if there is variable defined:
                CodeVariable var = point.GetCurrentCodeElement<CodeVariable>(vsCMElement.vsCMElementVariable);
                type = (var != null ? var.Type : null);

                // or maybe clicked on parameter:
                if (type == null)
                {
                    CodeParameter param = point.GetCurrentCodeElement<CodeParameter>(vsCMElement.vsCMElementParameter);
                    type = (param != null ? param.Type : null);
                }
            }

            // extract enum values:
            if (type != null && type.CodeType as CodeEnum != null)
            {
                names = new List<string>();
                foreach (CodeElement f in type.CodeType.Members)
                    names.Add(f.Name);
            }

            if (type == null)
            {
                CodeElement elem = point.GetCurrentCodeElement<CodeElement>(vsCMElement.vsCMElementFunctionInvokeStmt);
                if (elem != null)
                {
                    names = new List<string>();
                    names.Add(elem.Name);
                }
            }

            if (type == null && names == null)
            {
                string identifier = point.CodeExtractor.CurrentIdentifier;

                System.Windows.Forms.MessageBox.Show(identifier);
                CodeType t = point.CodeExtractor.GetTypeInfo(identifier);
                names = new List<string>();
                if (point.CodeExtractor.Namespaces != null)
                    foreach (string n in point.CodeExtractor.Namespaces)
                        names.Add(n);
                names.Add("-----------");
                names.Add(identifier);
                if (t != null)
                    foreach (CodeElement f in t.Members)
                        names.Add(f.Name);

                //CodeFunction elem = point.GetCurrentCodeElement<CodeFunction>(vsCMElement.vsCMElementFunction);
                //if (elem != null)
                //{
                //    EditPoint body = elem.StartPoint.CreateEditPoint();
                //    string content = body.GetText(elem.EndPoint);
                //    names = new List<string>();

                //    names.Add(content);

                //    string[] xNames = new string[] { "System.GCCollectionMode", "GCCollectionMode", point.Selection.Text};
                //    foreach (string n in xNames)
                //    {
                //        CodeType t = point.GetTypeInfo(n);
                //        names.Add("-----------");
                //        names.Add(n);
                //        if (t != null)
                //            foreach (CodeElement f in t.Members)
                //                names.Add(f.Name);
                //    }
                //}
            }

            // and populate them into dialog
            if (names != null)
            {
                if (dlgExpand == null)
                    dlgExpand = new ExpandEnumForm();

                dlgExpand.SetUI(names);
                dlgExpand.ShowDialog();
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
