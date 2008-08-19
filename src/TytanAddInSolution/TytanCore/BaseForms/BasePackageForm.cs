using System.Windows.Forms;
using System.Diagnostics;

namespace Pretorianie.Tytan.Core.BaseForms
{
    /// <summary>
    /// Base class for all the forms shown to the user.
    /// </summary>
    public partial class BasePackageForm : Form
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public BasePackageForm()
        {
            InitializeComponent();
        }

        private void linkFeedback_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                Process.Start("mailto:hofman@kn.pl?subject=Feedback about " + Text);
            }
            catch
            {
            }
        }
    }
}