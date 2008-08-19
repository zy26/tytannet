using Pretorianie.Tytan.Core.BaseForms;

namespace Pretorianie.Tytan.Forms
{
    public partial class RegistryNewKeyForm : BasePackageForm
    {
        public RegistryNewKeyForm(string title)
        {
            Text = title;
            InitializeComponent();
        }

        public RegistryNewKeyForm(string title, string initialRegistryKeyName)
            : this(title)
        {
            RegistryKeyName = initialRegistryKeyName;
        }

        /// <summary>
        /// Gets or sets the name of the registry key.
        /// </summary>
        public string RegistryKeyName
        {
            get { return textBox1.Text; }
            set { textBox1.Text = value; }
        }
    }
}