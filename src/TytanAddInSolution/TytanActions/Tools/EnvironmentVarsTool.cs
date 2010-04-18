using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Pretorianie.Tytan.Core.CustomAddIn;
using Pretorianie.Tytan.Core.EnvVarView;
using Pretorianie.Tytan.Core.EnvVarView.Tracking;
using Pretorianie.Tytan.Core.Helpers;
using Pretorianie.Tytan.Forms;

namespace Pretorianie.Tytan.Tools
{
    public partial class EnvironmentVarsTool : UserControl
    {
        public const string DialogTitle = "Environment Variables";

        private readonly EnvironmentVariables[] _varSets = new[] { new EnvironmentVariables(EnvironmentVariableTarget.Machine), new EnvironmentVariables(EnvironmentVariableTarget.User), new EnvironmentVariables(EnvironmentVariableTarget.Process) };
        private EnvironmentVariables _activeVars;
        private EnvironmentVariable _selectedVar;
        private StringHelper.IStringFilter _activeFilter;

        private readonly EnvironmentSession _session = new EnvironmentSession("Current session");

        public EnvironmentVarsTool()
        {
            InitializeComponent();

            toolStripViewType.SelectedIndex = 2;
            _session.Changed += CurrentEnvironmentSession_Changed;

            // store the reference of the created tool:
            CustomAddInManager.LastCreatedPackageTool = this;
        }

        void CurrentEnvironmentSession_Changed(object sender, EnvironmentSessionEventArgs e)
        {
            toolStripSaveSession.Enabled =
            toolStripUndoSession.Enabled = e.Count > 0;
        }

        private void ActivateVarSet(int index)
        {
            _activeVars = _varSets[index];
            PopulateVars();
        }

        private void ActivateVarSet(EnvironmentVariableTarget target)
        {
            if (target == EnvironmentVariableTarget.Machine)
            {
                ActivateVarSet(0);
                return;
            }

            if (target == EnvironmentVariableTarget.User)
            {
                ActivateVarSet(1);
                return;
            }

            ActivateVarSet(2);
        }

        private void RefreshVarSet()
        {
            // reread all the variables with history keep:
            foreach (EnvironmentVariables set in _varSets)
                set.Refresh();

            PopulateVars();
        }

        private static ListViewItem ToListViewItem(EnvironmentVariable v)
        {
            ListViewItem i = new ListViewItem();

            i.Text = v.Name;
            i.SubItems.Add(v.Value);
            i.Tag = v;

            return i;
        }

        /// <summary>
        /// Refreshes the list of variables on the screen.
        /// </summary>
        private void PopulateVars()
        {
            list.Items.Clear ();
            if (_activeFilter == null || _activeFilter.IsAlwaysMatch)
            {
                foreach (EnvironmentVariable v in _activeVars)
                    list.Items.Add(ToListViewItem(v));
            }
            else
            {
                foreach (EnvironmentVariable v in _activeVars)
                    if (_activeFilter.Match(v.Name))
                        list.Items.Add(ToListViewItem(v));
            }
        }

        private void PopulateVars(string filterText)
        {
            // update the filter:
            if (string.IsNullOrEmpty(filterText))
                _activeFilter = null;
            else
                _activeFilter = new StringHelper.RegexFilter(filterText, RegexOptions.IgnoreCase);

            if (filterText != null && !toolStripFilter.Items.Contains(filterText))
                toolStripFilter.Items.Add(filterText);

            // refresh the view:
            PopulateVars();
        }

        private void list_SelectedIndexChanged(object sender, EventArgs e)
        {
            toolStripDelete.Enabled =
                toolStripEdit.Enabled =
                    deleteToolStripMenuItem.Enabled =
                        editToolStripMenuItem.Enabled = 
                                list.SelectedItems != null && list.SelectedItems.Count > 0;

            // select variable:
            if (toolStripDelete.Enabled && _activeVars != null && list.SelectedItems != null
                && list.SelectedItems.Count > 0)
                _selectedVar = list.SelectedItems[0].Tag as EnvironmentVariable;
            else
                _selectedVar = null;
        }

        private void toolStripViewType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ActivateVarSet(toolStripViewType.SelectedIndex);
            list_SelectedIndexChanged(null, EventArgs.Empty);
        }

        private void toolStripAdd_Click(object sender, EventArgs e)
        {
            EnvironmentVarForm dlgAdd = new EnvironmentVarForm("Add new variable", null);

            // add:
            if (dlgAdd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    _activeVars.SetVariable(dlgAdd.VarName, dlgAdd.VarValue);
                    _session.Update(_activeVars.Target, dlgAdd.VarName, dlgAdd.VarValue);
                    RefreshVarSet();
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex.Message);
                    Trace.WriteLine(ex.StackTrace);
                    MessageBox.Show(ex.Message + Environment.NewLine, DialogTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void toolStripDelete_Click(object sender, EventArgs e)
        {
            if (_selectedVar == null)
            {
                MessageBox.Show("No variable selected.", DialogTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // ask for confirmation:
            if (MessageBox.Show("Are you sure to delete '" + _selectedVar.Name + "' ?", DialogTitle,
                                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    _activeVars.Remove(_selectedVar.Name);
                    _session.Update(_activeVars.Target, _selectedVar.Name, null, _selectedVar.Value);
                    RefreshVarSet();
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex.Message);
                    Trace.WriteLine(ex.StackTrace);
                    MessageBox.Show(ex.Message + Environment.NewLine, DialogTitle, MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                }
            }
        }

        private void toolStripEdit_Click(object sender, EventArgs e)
        {
            EnvironmentVarForm dlgEdit = new EnvironmentVarForm("Edit variable", _selectedVar);

            // edit:
            if (dlgEdit.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    _activeVars.SetVariable(dlgEdit.VarName, dlgEdit.VarValue);
                    _session.Update(_activeVars.Target, dlgEdit.VarName, dlgEdit.VarValue, string.Compare(dlgEdit.VarName, _selectedVar.Name, true) == 0 ? _selectedVar.Value: null);
                    RefreshVarSet();
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex.Message);
                    Trace.WriteLine(ex.StackTrace);
                    MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace, DialogTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void toolStripCmd_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(Environment.GetEnvironmentVariable("ComSpec"));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Can not start ConsoleWindow." + Environment.NewLine + ex.Message, DialogTitle,
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void toolStripFeedback_Click(object sender, EventArgs e)
        {
            CallHelper.SendFeedback(this);
        }

        private void list_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Insert)
            {
                // add new variable:
                toolStripAdd_Click(sender, EventArgs.Empty);
                return;
            }

            if (e.KeyCode == Keys.Enter)
            {
                // edit existing variable:
                toolStripEdit_Click(sender, EventArgs.Empty);
                return;
            }

            if (e.KeyCode == Keys.Delete)
            {
                // delete variable:
                toolStripDelete_Click(sender, EventArgs.Empty);
                return;
            }

            if (e.KeyCode == Keys.F && e.Modifiers == Keys.Control)
            {
                toolStripFilter.Focus();
            }
        }

        private void toolStripUndoSession_Click(object sender, EventArgs e)
        {
            EnvironmentSessionItemsForm dlgSession = new EnvironmentSessionItemsForm(_session);

            DialogResult result = dlgSession.ShowDialog();
            if (result == DialogResult.OK)
            {
                try
                {
                    _session.Revert();
                    RefreshVarSet();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        "Error while reverting variables." + Environment.NewLine + ex.Message,
                        DialogTitle,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                _activeVars.Refresh();
                PopulateVars();
            }

            if (result == DialogResult.Ignore)
            {
                _session.Clear();
            }
        }

        private void toolStripFilter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && toolStripFilter != null)
            {
                PopulateVars(toolStripFilter.Text);
            }
        }

        private void toolStripSaveSession_Click(object sender, EventArgs e)
        {
            EnvironmentSessionSaveForm dlgSave = new EnvironmentSessionSaveForm(_session);

            if (dlgSave.ShowDialog() == DialogResult.OK)
            {
                EnvironmentSession sessionToStore = dlgSave.Session;

                EnvironmentSessionProvider.SaveSession(sessionToStore.Name, sessionToStore);

                // remove all stored items from current session, if requested:
                if (dlgSave.ClearCurrentSession)
                {
                    foreach (EnvironmentSessionItem item in sessionToStore.Items)
                    {
                        _session.Remove(item.Name);
                    }
                }
            }
        }

        private void toolStripLoadSession_Click(object sender, EventArgs e)
        {
            EnvironmentSessionListForm dlgSessions = new EnvironmentSessionListForm(EnvironmentSessionProvider.LoadSessions());

            if (dlgSessions.ShowDialog() == DialogResult.OK)
            {
                ActivateVarSet(dlgSessions.SelectedTarget);

                foreach(EnvironmentSessionItem item in dlgSessions.SelectedSession.Items)
                {
                    EnvironmentVariable variable = _activeVars.GetVariable(item.Name);

                    _session.Update(dlgSessions.SelectedTarget, item, variable);
                    _activeVars.SetVariable(item.Name, item.Value);
                }

                RefreshVarSet();
            }
        }

        private void toolStripFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateVars(toolStripFilter.Text);
        }

        private void toolStripClearHistory_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(SharedStrings.EnvironmentView_ClearHistoryQuestion, DialogTitle,
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                foreach(EnvironmentVariables vars in _varSets)
                    vars.ClearHistory();
            }
        }

        public void SaveHistory()
        {
            foreach (EnvironmentVariables vars in _varSets)
                vars.SaveHistory();
        }
    }
}
