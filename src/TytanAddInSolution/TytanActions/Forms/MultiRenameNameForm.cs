using Pretorianie.Tytan.Core.BaseForms;

namespace Pretorianie.Tytan.Forms
{
    public partial class MultiRenameNameForm : BasePackageForm
    {
        public MultiRenameNameForm()
        {
            InitializeComponent();
        }

        public void SetUI(string text)
        {
            textName.Text = text;
            ActiveControl = textName;
        }

        #region Properties

        public int FirstLetter
        {
            get { return textName.SelectionStart + 1; }
        }

        public int LastLetter
        {
            get { return (textName.SelectionLength == 0 || textName.SelectionLength + textName.SelectionStart >= textName.Text.Length? -1 : textName.SelectionStart + textName.SelectionLength); }
        }

        #endregion
    }
}