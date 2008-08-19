using System.Windows.Forms;
using System;

namespace Pretorianie.Tytan.Core.Helpers
{
    /// <summary>
    /// Class that helps in common config operations across the AddIns.
    /// </summary>
    public static class ConfigHelper
    {
        #region Feedback Functions

        /// <summary>
        /// Generates new process that will send user feedback to the author.
        /// </summary>
        public static void SendFeedback(UserControl ctrl)
        {
            try
            {
                Control parent = ctrl;
                string text = "unknown";

                while(parent != null && string.IsNullOrEmpty(parent.Text))
                    parent = parent.Parent;

                if (parent != null)
                    text = parent.Text;

                System.Diagnostics.Process.Start("mailto:hofman@kn.pl?subject=Feedback about \"" + text + "\"" +
                                                 (text.EndsWith("tool", StringComparison.CurrentCultureIgnoreCase)
                                                      ? string.Empty
                                                      : " tool"));
            }
            catch
            {
            }
        }

        #endregion
    }
}
