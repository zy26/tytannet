using System;
using System.Diagnostics;
using System.Windows.Forms;
using Pretorianie.Tytan.Core.Helpers;
using Pretorianie.Tytan.Data;

namespace Pretorianie.Tytan.Forms
{
    public partial class AboutBoxTipsForm : Form
    {
        private readonly TipsProvider tips;

        public AboutBoxTipsForm(TipsProvider tips)
        {
            this.tips = tips;
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            UpdateTip();
        }

        private void richTextBox1_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            CallHelper.OpenBrowser(e.LinkText);
        }

        private void bttTip_Click(object sender, System.EventArgs e)
        {
            UpdateTip();
        }

        private void UpdateTip()
        {
            try
            {
                int i;
                bool isRtf;
                string tip = tips.GetRandomTip(out i, out isRtf);

                // set the tip text on the screen:
                if (isRtf)
                    txtTipText.Rtf = tip;
                else
                    txtTipText.Text = tip;

                // and update the displayed number:
                lblTipNumber.Text = string.Format("{0} of {1}", i + 1, tips.Count);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
                Trace.WriteLine(ex.StackTrace);
            }
        }
    }
}