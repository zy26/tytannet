using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Pretorianie.Tytan.Core.EnvVarView.Tracking;
using Pretorianie.Tytan.Core.Helpers;

namespace Pretorianie.Tytan.Forms
{
    public partial class EnvironmentSessionListForm : Pretorianie.Tytan.Core.BaseForms.BasePackageForm
    {
        private EnvironmentSession _selectedSession;

        public EnvironmentSessionListForm()
        {
            InitializeComponent();
        }

        public EnvironmentSessionListForm(IEnumerable<EnvironmentSession> sessions)
        {
            InitializeComponent();

            LoadSessions(sessions);
        }

        public void LoadSessions(IEnumerable<EnvironmentSession> sessions)
        {
            cmbTarget.SelectedIndex = 2;
            _selectedSession = null;
            listSessions.Items.Clear();

            if (sessions != null)
            {
                foreach (EnvironmentSession s in sessions)
                {
                    ListViewItem v = new ListViewItem(s.Name);
                    v.Tag = s;
                    v.ToolTipText = s.FileName;

                    listSessions.Items.Add(v);
                }
            }
        }

        private void listSessions_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            bool hasItemSelected = listSessions.SelectedIndices != null
                                   && listSessions.SelectedIndices.Count > 0;

            bttOK.Enabled = listSessions.Items.Count > 0 && hasItemSelected;

            // and update items on the right side of the form:
            listItems.Items.Clear();
            if (hasItemSelected)
            {
                _selectedSession = listSessions.SelectedItems[0].Tag as EnvironmentSession;

                if (_selectedSession != null)
                    foreach (EnvironmentSessionItem item in _selectedSession.Items)
                    {
                        ListViewItem i = new ListViewItem(item.Name);
                        i.SubItems.Add(item.Action);
                        i.SubItems.Add(item.Value);
                        listItems.Items.Add(i);
                    }
            }
            else
            {
                _selectedSession = null;
            }

            bttDelete.Enabled = _selectedSession != null && !string.IsNullOrEmpty(_selectedSession.FileName);
        }

        #region Properties

        /// <summary>
        /// Gets the selected environment session available from the persistent storage.
        /// </summary>
        public EnvironmentSession SelectedSession
        {
            get { return _selectedSession; }
        }

        /// <summary>
        /// Gets the currently selected target for environment variables session to apply to.
        /// </summary>
        public EnvironmentVariableTarget SelectedTarget
        {
            get
            {
                if (cmbTarget.SelectedIndex == 0)
                    return EnvironmentVariableTarget.Machine;

                if (cmbTarget.SelectedIndex == 1)
                    return EnvironmentVariableTarget.User;

                return EnvironmentVariableTarget.Process;
            }
        }

        #endregion

        private void listSessions_DoubleClick(object sender, EventArgs e)
        {
            if (_selectedSession != null)
            {
                FileHelper.OpenFolder(_selectedSession.FileName);
            }
        }

        private void bttDelete_Click(object sender, EventArgs e)
        {
            if (_selectedSession != null)
            {
                if (MessageBox.Show(string.Format(SharedStrings.EnvironmentView_ConfirmSessionDelete, _selectedSession.Name), SharedStrings.SolutionClose_DialogTitle,
                                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    // delete the session if confirmed:
                    try
                    {
                        File.Delete(_selectedSession.FileName);
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine(ex.Message);
                        Trace.WriteLine(ex.StackTrace);
                    }
                }
            }
        }
    }
}
