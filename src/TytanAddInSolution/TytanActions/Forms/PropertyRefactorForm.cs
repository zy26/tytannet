using System;
using System.Collections.Generic;
using System.Windows.Forms;
using EnvDTE;
using Pretorianie.Tytan.Core.BaseForms;
using Pretorianie.Tytan.Core.Data;
using Pretorianie.Tytan.Core.Helpers;
using Pretorianie.Tytan.Core.Interfaces;

namespace Pretorianie.Tytan.Forms
{
    public partial class PropertyRefactorForm : BasePackageForm
    {
        private IList<CodeVariable> storedVars;
        private bool[] modifiedVars;
        private CodeModelLanguages storedLanguage;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public PropertyRefactorForm()
        {
            InitializeComponent();
            dataVars.CellValueChanged += CellValueChanged;
        }

        /// <summary>
        /// Sets the info on the GUI.
        /// Some of this parameters can be edited by user so use <c>ReadInterface</c> method
        /// to gather all changed names or disabled variables.
        /// </summary>
        public void InitializeInterface(IList<CodeVariable> vars, IList<CodeVariable> toDisable, CodeModelLanguages language)
        {
            int i = 0;
            IList<string> varNames;
            IList<string> propNames;

            // generate output names:
            storedLanguage = language;
            NameHelper.GetVariableNames(vars, out varNames, out propNames, checkUpdateNames.Checked, language);

            dataVars.Rows.Clear();
            if (vars != null)
            {
                storedVars = vars;

                // create array to store 2 states for each row:
                modifiedVars = new bool[2 * vars.Count];

                foreach (CodeVariable v in vars)
                {
                    dataVars.Rows.Add( (toDisable != null && toDisable.Contains(v)? false: true), v.Name, varNames[i], propNames[i]);
                    i++;
                }
            }

            ActiveControl = bttOK;
        }

        /// <summary>
        /// Gets the input set by the user on the interface.
        /// </summary>
        public bool ReadInterface(out IList<CodeVariable> vars, out IList<string> varNames, out IList<string> propNames)
        {
            int i = 0;

            vars = new List<CodeVariable>();
            varNames = new List<string>();
            propNames = new List<string>();

            foreach (DataGridViewRow r in dataVars.Rows)
            {
               if ((bool)r.Cells[0].Value)
                {
                    vars.Add(storedVars[i]);
                    varNames.Add(r.Cells[2].Value as string);
                    propNames.Add(r.Cells[3].Value as string);
                }
                i++;
            }
            
            // deletes empty collections:
            if (vars.Count == 0)
            {
                vars = null;
                varNames = null;
                propNames = null;
                return false;
            }

            return true;
        }

        /// <summary>
        /// Gets or sets the generator options.
        /// </summary>
        public PropertyGeneratorOptions GeneratorOptions
        {
            get
            {
                PropertyGeneratorOptions options = PropertyGeneratorOptions.Nothing;

                // get the output method options:
                switch (cmbMethod.SelectedIndex)
                {
                    case 0: options |= PropertyGeneratorOptions.GetterAndSetter; break;
                    case 1: options |= PropertyGeneratorOptions.Getter; break;
                    case 2: options |= PropertyGeneratorOptions.Setter; break;
                }

                // assess protection levels:
                switch (cmbVar.SelectedIndex)
                {
                    case 1: options |= PropertyGeneratorOptions.ForceVarDontChange; break;
                    case 2: options |= PropertyGeneratorOptions.ForceVarPublic; break;
                    case 3: options |= PropertyGeneratorOptions.ForceVarInternal; break;
                    case 4: options |= PropertyGeneratorOptions.ForceVarProtected; break;
                    case 5: options |= PropertyGeneratorOptions.ForceVarProtectedInternal; break;
                    case 6: options |= PropertyGeneratorOptions.ForceVarPrivate; break;
                }

                // assess protection levels:
                switch (cmbProp.SelectedIndex)
                {
                    case 1: options |= PropertyGeneratorOptions.ForcePropertyAsVar; break;
                    case 2: options |= PropertyGeneratorOptions.ForcePropertyPublic; break;
                    case 3: options |= PropertyGeneratorOptions.ForcePropertyInternal; break;
                    case 4: options |= PropertyGeneratorOptions.ForcePropertyProtected; break;
                    case 5: options |= PropertyGeneratorOptions.ForcePropertyProtectedInternal; break;
                    case 6: options |= PropertyGeneratorOptions.ForcePropertyPrivate; break;
                }

                // region & comment:
                if (!checkComments.Checked)
                    options |= PropertyGeneratorOptions.SuppressComment;
                if (!checkRegion.Checked)
                    options |= PropertyGeneratorOptions.SuppressRegion;


                return options;
            }
            set
            {
                // set method opitions:
                cmbMethod.SelectedIndex = 0;
                if ((value & PropertyGeneratorOptions.Getter) == PropertyGeneratorOptions.GetterAndSetter)
                    cmbMethod.SelectedIndex = 1;
                else
                    if ((value & PropertyGeneratorOptions.Setter) == PropertyGeneratorOptions.GetterAndSetter)
                        cmbMethod.SelectedIndex = 2;

                // access level:
                cmbVar.SelectedIndex = 0;
                cmbProp.SelectedIndex = 0;

                if ((value & PropertyGeneratorOptions.ForceVarDontChange) == PropertyGeneratorOptions.ForceVarDontChange)
                    cmbVar.SelectedIndex = 1;
                else
                    if ((value & PropertyGeneratorOptions.ForceVarPublic) == PropertyGeneratorOptions.ForceVarPublic)
                        cmbVar.SelectedIndex = 2;
                    else
                        if ((value & PropertyGeneratorOptions.ForceVarInternal) == PropertyGeneratorOptions.ForceVarInternal)
                            cmbVar.SelectedIndex = 3;
                        else
                            if ((value & PropertyGeneratorOptions.ForceVarProtected) == PropertyGeneratorOptions.ForceVarProtected)
                                cmbVar.SelectedIndex = 4;
                            else
                                if ((value & PropertyGeneratorOptions.ForceVarProtectedInternal) == PropertyGeneratorOptions.ForceVarProtectedInternal)
                                    cmbVar.SelectedIndex = 5;
                                else
                                    if ((value & PropertyGeneratorOptions.ForceVarPrivate) == PropertyGeneratorOptions.ForceVarPrivate)
                                        cmbVar.SelectedIndex = 6;

                if ((value & PropertyGeneratorOptions.ForcePropertyAsVar) == PropertyGeneratorOptions.ForcePropertyAsVar)
                    cmbProp.SelectedIndex = 1;
                else
                    if ((value & PropertyGeneratorOptions.ForcePropertyPublic) == PropertyGeneratorOptions.ForcePropertyPublic)
                        cmbProp.SelectedIndex = 2;
                    else
                        if ((value & PropertyGeneratorOptions.ForcePropertyInternal) == PropertyGeneratorOptions.ForcePropertyInternal)
                            cmbProp.SelectedIndex = 3;
                        else
                            if ((value & PropertyGeneratorOptions.ForcePropertyProtected) == PropertyGeneratorOptions.ForcePropertyProtected)
                                cmbProp.SelectedIndex = 4;
                            else
                                if ((value & PropertyGeneratorOptions.ForcePropertyProtectedInternal) == PropertyGeneratorOptions.ForcePropertyProtectedInternal)
                                    cmbProp.SelectedIndex = 5;
                                else
                                    if ((value & PropertyGeneratorOptions.ForcePropertyPrivate) == PropertyGeneratorOptions.ForcePropertyPrivate)
                                        cmbProp.SelectedIndex = 6;

                checkComments.Checked = ((value & PropertyGeneratorOptions.SuppressComment) != PropertyGeneratorOptions.SuppressComment);
                checkRegion.Checked = ((value & PropertyGeneratorOptions.SuppressRegion) != PropertyGeneratorOptions.SuppressRegion);
                textRegionName.Enabled = checkRegion.Checked;
            }
        }

        /// <summary>
        /// Gets or sets the name of the generated region.
        /// </summary>
        public string RegionName
        {
            get { return textRegionName.Text; }
            set { textRegionName.Text = value; }
        }

        void CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if ((e.ColumnIndex == 2 || e.ColumnIndex == 3) && e.RowIndex >= 0 && e.RowIndex < modifiedVars.Length)
                modifiedVars[2*e.RowIndex + e.ColumnIndex - 2] = true;
        }

        private void checkRegion_CheckedChanged(object sender, EventArgs e)
        {
            textRegionName.Enabled = checkRegion.Checked;
        }

        private void checkUpdateNames_CheckedChanged(object sender, EventArgs e)
        {
            int i = 0;
            IList<string> varNames;
            IList<string> propNames;

            // reset the values on the interface:
            NameHelper.GetVariableNames(storedVars, out varNames, out propNames, checkUpdateNames.Checked,
                                        storedLanguage);

            // visit all unmodified rows and update the variable and property names:
            if (varNames != null && varNames.Count == dataVars.Rows.Count)
            {
                // disable value change notifications:
                dataVars.CellValueChanged -= CellValueChanged;

                // update values if needed, based on stored state for each row:
                foreach (DataGridViewRow r in dataVars.Rows)
                {
                    if (!modifiedVars[2*i])
                        r.Cells[2].Value = varNames[i];
                    if (!modifiedVars[2*i + 1])
                        r.Cells[3].Value = propNames[i];

                    i++;
                }

                // enable value change notifications again:
                dataVars.CellValueChanged += CellValueChanged;
            }
        }
    }
}

