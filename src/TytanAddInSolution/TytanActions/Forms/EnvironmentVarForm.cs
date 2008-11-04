using Pretorianie.Tytan.Core.BaseForms;
using Pretorianie.Tytan.Core.EnvVarView;

namespace Pretorianie.Tytan.Forms
{
    public partial class EnvironmentVarForm : BasePackageForm
    {
        public EnvironmentVarForm(string title, EnvironmentVariable var)
        {
            InitializeComponent();

            // initialize GUI with specified variable:
            Text = title;

            if (var != null)
            {
                textName.Text = var.Name;
                comboValue.Text = var.Value;

                foreach (string v in var.HistoryValues)
                    comboValue.Items.Add(v);
            }
        }

        #region Properties

        /// <summary>
        /// Gets the name of the variable.
        /// </summary>
        public string VarName
        {
            get { return textName.Text; }
        }

        /// <summary>
        /// Gets the value of the variable.
        /// </summary>
        public string VarValue
        {
            get { return comboValue.Text; }
        }

        /// <summary>
        /// Checks if any field of the variable has been updated.
        /// </summary>
        public bool IsUpdated(EnvironmentVariable v)
        {
            if (v == null)
                return true;

            return string.Compare(VarName, v.Name, true) != 0
                        || string.Compare(VarValue, v.Value, true) != 0;
        }

        #endregion
    }
}