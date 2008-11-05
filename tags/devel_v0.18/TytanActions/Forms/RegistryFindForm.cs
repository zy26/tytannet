using Pretorianie.Tytan.Core.BaseForms;

namespace Pretorianie.Tytan.Forms
{
    public partial class RegistryFindForm : BasePackageForm
    {
        private bool isConfirmed;

        public RegistryFindForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Stores historical entry for possible future usage.
        /// </summary>
        public void AddHistory(string value)
        {
            if (!cmbSearchText.Items.Contains(value))
                cmbSearchText.Items.Add(value);
        }

        /// <summary>
        /// Reinitializes the UI.
        /// </summary>
        public void SetUI()
        {
            ActiveControl = cmbSearchText;
            cmbSearchText.Text = string.Empty;
            isConfirmed = false;
        }

        #region Properties

        /// <summary>
        /// Gets or sets the search text.
        /// </summary>
        public string SearchText
        {
            get { return cmbSearchText.Text; }
            set
            {
                cmbSearchText.Text = value;
                AddHistory(value);
            }
        }

        /// <summary>
        /// Gets or sets searching for the keys.
        /// </summary>
        public bool SearchKeys
        {
            get { return chbKeys.Checked; }
            set { chbKeys.Checked = value; }
        }

        /// <summary>
        /// Gets or sets indication if search among values.
        /// </summary>
        public bool SearchValues
        {
            get { return chbValues.Checked; }
            set { chbValues.Checked = value; }
        }

        /// <summary>
        /// Gets or sets searching inside data content.
        /// </summary>
        public bool SearchContent
        {
            get { return checkBox1.Checked; }
            set { checkBox1.Checked = value; }
        }

        /// <summary>
        /// Gets or sets an indication if user closed this dialog with result OK.
        /// </summary>
        public bool IsConfirmed
        {
            get { return isConfirmed; }
            set { isConfirmed = value; }
        }

        #endregion
    }
}