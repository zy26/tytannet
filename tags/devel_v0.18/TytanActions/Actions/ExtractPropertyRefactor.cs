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
    /// <summary>
    /// Refactoring AddIn that generates properties from the C#/VBasic/J# class variables code.
    /// </summary>
    public sealed class ExtractPropertyRefactor : IPackageAction
    {
        /// <summary>
        /// Default name of the region.
        /// </summary>
        public const string RegionName = "Properties";

        private IPackageEnvironment parent;
        private PropertyRefactorForm cfgDialog;

        #region Refactor

        private static void FindExistingProperties(IList<CodeVariable> vars, IList<CodeProperty> props, CodeModelLanguages language, out IList<CodeVariable> toRemove, bool remove)
        {
            // remove redundant properties:
            if (props != null && vars != null)
            {
                toRemove = new List<CodeVariable>();

                foreach (CodeVariable v in vars)
                    if (EditorHelper.Contains(props, NameHelper.GetPropertyName(v.Name, language)))
                        toRemove.Add(v);

                if (remove)
                    foreach (CodeVariable v in toRemove)
                        vars.Remove(v);
            }
            else
                toRemove = null;
        }

        private static string GenerateSourceCodeOutput(string codeClassName, IEnumerable<CodeVariable> vars, IList<string> varNames, IList<string> propNames, CodeModelLanguages language, PropertyGeneratorOptions options, string regionName)
        {
            // generate output:
            if (vars != null)
            {
                CodeTypeMemberCollection code = new CodeTypeMemberCollection();
                int index = 0;

                // serialize variables:
                foreach (CodeVariable var in vars)
                {
                    code.Add(VariableHelper.GetProperty(codeClassName, propNames[index], var, varNames[index], options));

                    // replace the variable:
                    var.Name = varNames[index];

                    try
                    {
                        vsCMAccess access;

                        if (VariableHelper.GetVariableAttributes(var, options, out access))
                            var.Access = access;
                    }
                    catch
                    {
                    }

                    index++;
                }

                if ((options & PropertyGeneratorOptions.SuppressRegion) != PropertyGeneratorOptions.SuppressRegion)
                {
                    code[0].StartDirectives.Add(new CodeRegionDirective(CodeRegionMode.Start, regionName));
                    code[code.Count - 1].EndDirectives.Add(new CodeRegionDirective(CodeRegionMode.End, regionName));
                }

                return CodeHelper.GenerateFromMember(language, code);
            }

            return null;
        }

        private bool Refactor(IList<CodeVariable> vars, IList<CodeVariable> disabledVars, string codeClassName, string languageGUID, CodeElements codeMembers, EditPoint insertLocation, out int gotoLine)
        {
            IList<CodeVariable> toDisable;
            IList<CodeProperty> props;
            IList<string> varNames;
            IList<string> propNames;

            // set invalid goto-line:
            gotoLine = -1;

            if (vars != null)
            {
                // get the language and a list of currently available properties:
                CodeModelLanguages language = CodeHelper.GetCodeLanguage(languageGUID);
                props = EditorHelper.GetList<CodeProperty>(codeMembers, vsCMElement.vsCMElementProperty);

                // remove the variables from the list for which the properties exist:
                FindExistingProperties(vars, props, language, out toDisable, false);
                // generate output names:
                NameHelper.GetVariableNames(vars, out varNames, out propNames, language);

                // add additional disabled variables:
                if (disabledVars != null)
                {
                    if (toDisable == null)
                        toDisable = disabledVars;
                    else
                    {
                        foreach (CodeVariable v in disabledVars)
                        {
                            if (!toDisable.Contains(v))
                                toDisable.Add(v);
                        }
                    }
                }

                if (cfgDialog == null)
                {
                    cfgDialog = new PropertyRefactorForm();
                    cfgDialog.GeneratorOptions = PropertyGeneratorOptions.GetterAndSetter | PropertyGeneratorOptions.ForcePropertyPublic | PropertyGeneratorOptions.ForceVarDontChange | PropertyGeneratorOptions.SuppressComment;
                    cfgDialog.RegionName = RegionName;
                }

                // show dialog with possibility to change:
                cfgDialog.InitializeInterface(vars, varNames, toDisable, propNames);
                if (cfgDialog.ShowDialog() == DialogResult.OK && cfgDialog.ReadInterface(out vars, out varNames, out propNames))
                {
                    // generate code based on user modifications:
                    string code = GenerateSourceCodeOutput(codeClassName, vars, varNames, propNames, language, cfgDialog.GeneratorOptions, cfgDialog.RegionName);

                    // insert code to the editor:
                    if (!string.IsNullOrEmpty(code))
                    {
                        insertLocation.ReplaceText(insertLocation, code, (int)vsEPReplaceTextOptions.vsEPReplaceTextAutoformat);

                        // jump without selection to the insertion place:
                        gotoLine = insertLocation.Line;
                        return true;
                    }
                }
            }

            return false;
        }

        #endregion

        #region IPackageAction Members

        /// <summary>
        /// Gets the unique ID of the action.
        /// </summary>
        public int ID
        {
            get { return PackageCmdIDList.toolRefactor_ExtractProperty; }
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
            mc.AddCommand(menu, "ExtractPropertyRefactor", "E&xtract Property...", 9001, "Text Editor::Ctrl+R, R", null, false);
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

            if (!editorEditPoint.IsRefactorValid)
                return;

            CodeEditSelection editorSelection = EditorHelper.GetSelectedVariables(editorEditPoint);

            if (editorSelection != null)
            {
                int gotoLine;
                bool isRefactored;
                try
                {
                    // open the undo-context to combine all the modifications of the source code into one:
                    parent.DTE.UndoContext.Open(SharedStrings.UndoContext_ExtractPropertyRefactor, true);

                    isRefactored = Refactor(editorSelection.AllVariables, editorSelection.DisabledVariables, editorSelection.ParentName, editorSelection.ParentLanguage, editorSelection.CodeMembers, editorSelection.GetEndPoint(vsCMPart.vsCMPartBody).CreateEditPoint(), out gotoLine);
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

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
        }

        #endregion
    }
}