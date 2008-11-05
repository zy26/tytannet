using System;
using System.Diagnostics;
using System.Windows.Forms;
using Pretorianie.Tytan.Core.CustomAddIn;
using Pretorianie.Tytan.Core.EnvVarView;
using Pretorianie.Tytan.Core.Helpers;
using Pretorianie.Tytan.Forms;

namespace Pretorianie.Tytan.Tools
{
    public partial class EnvironmentVarsTool : UserControl
    {
        public const string DialogTitle = "Environment Variables";

        private readonly EnvironmentVariables[] varSets = new EnvironmentVariables[] { new EnvironmentVariables(EnvironmentVariableTarget.Machine), new EnvironmentVariables(EnvironmentVariableTarget.User), new EnvironmentVariables(EnvironmentVariableTarget.Process) };
        private EnvironmentVariables activeVars = null;
        private EnvironmentVariable selectedVar = null;

        public EnvironmentVarsTool()
        {
            InitializeComponent();

            toolStripViewType.SelectedIndex = 2;

            // store the reference of the created tool:
            CustomAddInManager.LastCreatedPackageTool = this;
        }

        private void ActivateVarSet(int index)
        {
            activeVars = varSets[index];
            PopulateVars();
        }

        private void RefreshVarSet()
        {
            // reread all the variables with history keep:
            foreach (EnvironmentVariables set in varSets)
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
            foreach (EnvironmentVariable v in activeVars)
                list.Items.Add(ToListViewItem(v));
        }

        private void list_SelectedIndexChanged(object sender, EventArgs e)
        {
            toolStripDelete.Enabled =
                toolStripEdit.Enabled =
                    deleteToolStripMenuItem.Enabled =
                        editToolStripMenuItem.Enabled = 
                                list.SelectedItems != null && list.SelectedItems.Count > 0;

            // select variable:
            if (toolStripDelete.Enabled && activeVars != null)
                selectedVar = list.SelectedItems[0].Tag as EnvironmentVariable;
            else
                selectedVar = null;
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
                    activeVars.SetVariable(dlgAdd.VarName, dlgAdd.VarValue);
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

        private void toolStripDelete_Click(object sender, EventArgs e)
        {
            if ( selectedVar == null )
            {
                MessageBox.Show ("No variable selected.", DialogTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // ask for confirmation:
            if (MessageBox.Show("Are you sure to delete '" + selectedVar.Name + "' ?", DialogTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    activeVars.Remove(selectedVar.Name);
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

        private void toolStripEdit_Click(object sender, EventArgs e)
        {
            EnvironmentVarForm dlgEdit = new EnvironmentVarForm("Edit variable", selectedVar);

            // edit:
            if (dlgEdit.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    activeVars.SetVariable(dlgEdit.VarName, dlgEdit.VarValue);
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
    }
}
