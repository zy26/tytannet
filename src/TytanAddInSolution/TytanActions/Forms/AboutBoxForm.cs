using System.Windows.Forms;
using Pretorianie.Tytan.Core.Interfaces;
using Pretorianie.Tytan.Core.Helpers;

namespace Pretorianie.Tytan.Forms
{
    public partial class AboutBoxForm : Form
    {
        /// <summary>
        /// Init constructor.
        /// </summary>
        public AboutBoxForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Fill the UI with info about TytanNET.
        /// </summary>
        public void SetUI(IPackageEnvironment e)
        {
            if (e != null)
            {
                lblFriendly.Text = e.Info.FriendlyName;
                txtInfo.Text = e.Info.Info;
                txtDescription.Text = e.Info.Description;
                aboutIcon.BackgroundImage = e.Info.Icon;
            }
            else
            {
                lblFriendly.Text = null;
                txtInfo.Text = null;
                txtDescription.Text = null;
                aboutIcon.Image = null;
            }
        }

        private void LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            CallHelper.OpenBrowser(SharedStrings.About_Releases);
        }

        private void txtDescription_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            CallHelper.OpenBrowser(e.LinkText);
        }
    }
}