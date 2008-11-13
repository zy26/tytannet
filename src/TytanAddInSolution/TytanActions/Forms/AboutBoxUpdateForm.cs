using System.Windows.Forms;
using System;
using Pretorianie.Tytan.Core.Helpers;
using System.Drawing;
using System.Diagnostics;

namespace Pretorianie.Tytan.Forms
{
    public partial class AboutBoxUpdateForm : Form
    {
        private Version currentVersion;
        private Version newVersion;
        private string navigationURL;
        private string advice;
        private bool showButton;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public AboutBoxUpdateForm()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            SetupUI();
        }

        /// <summary>
        /// Asks remote server for version info and compares go them to give proper advice to the user.
        /// </summary>
        public void SetupUI(Bitmap logo)
        {
            try
            {
                advice = SharedStrings.AboutUpdate_DefaultAdvice;
                lblAdvice.Text = advice;
                lblLocalVersion.Text = VersionHelper.CurrentVersion.ToString(2);
                lblRemoteVersion.Text = SharedStrings.AboutUpdate_Checking;
                pictureBox.BackgroundImage = logo;

                VersionHelper.CheckVersion(VersionInfoAvailable);
            }
            catch
            {
                lblRemoteVersion.Text = SharedStrings.AboutUpdate_UnknownVersion;
            }
        }

        private void VersionInfoAvailable(Version currentVersion, Version newVersion, string navigationURL)
        {
            this.currentVersion = currentVersion;
            this.newVersion = newVersion;
            this.navigationURL = navigationURL;

            // and update the UI from proper thread:
            if (Visible && InvokeRequired)
                Invoke(new EventHandler(RemoteVersionReceived));
            else
                RemoteVersionReceived(this, EventArgs.Empty);
        }

        /// <summary>
        /// Refreshes info on the GUI about the version.
        /// </summary>
        protected void SetupUI()
        {
            try
            {
                if (newVersion != null)
                {
                    lblAdvice.Text = advice;
                    lblRemoteVersion.Text = newVersion.ToString(2);
                    if (currentVersion != null)
                        lblLocalVersion.Text = currentVersion.ToString(2);
                }
                bttHomepage.Left = lblAdvice.Left + lblAdvice.Width + 8;
                bttHomepage.Visible = showButton;
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Parse for new version information, update the dialog-box and fire proper notification event.
        /// </summary>
        private void RemoteVersionReceived(object sender, EventArgs e)
        {
            try
            {
                if (newVersion == VersionHelper.InvalidVersion || newVersion == null)
                {
                    advice = SharedStrings.AboutUpdate_UnknownVersion;
                }
                else if (currentVersion == newVersion)
                {
                    advice = SharedStrings.AboutUpdate_VersionLatest;
                }
                else if (currentVersion > newVersion)
                {
                    advice = SharedStrings.AboutUpdate_VersionTooNew;
                }
                else
                {
                    advice = SharedStrings.AboutUpdate_VersionUpdate;
                    showButton = true;
                }

                SetupUI();
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
        }

        private void bttHomepage_Click(object sender, EventArgs e)
        {
            CallHelper.OpenBrowser(navigationURL);
            Close();
        }
    }
}