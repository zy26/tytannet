using System;
using System.Windows.Forms;
using Pretorianie.Tytan.Core.Helpers;

namespace Pretorianie.Tytan.Forms
{
    public partial class AboutBoxNewVersionForm : Form
    {
        private string releasePageURL;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public AboutBoxNewVersionForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Sets new content on the GUI.
        /// </summary>
        public void SetUI(string versionString, string url)
        {
            lblText.Text = string.Format(SharedStrings.AboutUpdate_NewRelease, versionString);
            releasePageURL = url;
        }

        private void bttDownload_Click(object sender, EventArgs e)
        {
            CallHelper.OpenBrowser(releasePageURL);
        }
    }
}