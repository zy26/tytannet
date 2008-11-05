using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Pretorianie.Tytan.Core.Helpers
{
    /// <summary>
    /// Helper class for calling external processes.
    /// </summary>
    public static class CallHelper
    {
        #region Startup Functions

        /// <summary>
        /// Open given URL inside default web browser.
        /// </summary>
        public static void OpenBrowser(string url)
        {
            Process.Start(url);
        }

        #endregion

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

                while (parent != null && string.IsNullOrEmpty(parent.Text))
                    parent = parent.Parent;

                if (parent != null)
                    text = parent.Text;

                Process.Start("mailto:ankh.sangraal@gmail.com?subject=Feedback about \'" + text + "'" +
                                                 (text.EndsWith("tool", StringComparison.CurrentCultureIgnoreCase)
                                                      ? string.Empty
                                                      : " tool"));
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
        }

        #endregion
    }
}
