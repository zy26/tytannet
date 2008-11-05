using System.Windows.Forms;
using Pretorianie.Tytan.Core.CustomAddIn;
using Pretorianie.Tytan.Core.Helpers;

namespace Pretorianie.Tytan.Tools
{
    /// <summary>
    /// Summary description for MyControl.
    /// </summary>
    public partial class NativeImagePreviewTool : UserControl
    {
        public NativeImagePreviewTool()
        {
            InitializeComponent();

            // store the reference of the created tool:
            CustomAddInManager.LastCreatedPackageTool = this;
        }

        /// <summary> 
        /// Let this control process the mnemonics.
        /// </summary>
        protected override bool ProcessDialogChar(char charCode)
        {
              // If we're the top-level form or control, we need to do the mnemonic handling
              if (charCode != ' ' && ProcessMnemonic(charCode))
              {
                    return true;
              }
              return base.ProcessDialogChar(charCode);
        }

        private void toolStripFeedback_Click(object sender, System.EventArgs e)
        {
            CallHelper.SendFeedback(this);
        }
    }
}
