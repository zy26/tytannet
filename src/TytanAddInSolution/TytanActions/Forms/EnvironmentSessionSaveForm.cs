using System.Collections.Generic;
using System.Windows.Forms;
using Pretorianie.Tytan.Core.EnvVarView.Tracking;

namespace Pretorianie.Tytan.Forms
{
    public partial class EnvironmentSessionSaveForm : Pretorianie.Tytan.Core.BaseForms.BasePackageForm
    {
        private EnvironmentSession _session;

        public EnvironmentSessionSaveForm()
        {
            InitializeComponent();
        }

        public EnvironmentSessionSaveForm (EnvironmentSession session)
        {
            InitializeComponent();
            LoadSession(session);
        }

        /// <summary>
        /// Setups UI to show given session.
        /// </summary>
        public void LoadSession(EnvironmentSession session)
        {
            _session = session;
            txtName.Text = session.Name;

            listItems.Items.Clear();
            foreach(EnvironmentSessionItem item in session.Items)
            {
                ListViewItem i = new ListViewItem(item.Name);
                i.Checked = true;
                i.SubItems.Add(item.Action);
                i.SubItems.Add(item.Value);
                i.Tag = item;

                listItems.Items.Add(i);
            }

            chkAll.Checked = true;
        }

        private void listItems_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            int check = 0;
            foreach (ListViewItem i in listItems.Items)
            {
                if (i.Checked)
                    check++;
            }

            if (check == 0)
                chkAll.CheckState = CheckState.Unchecked;
            else
            {
                if (check == listItems.Items.Count)
                    chkAll.CheckState = CheckState.Checked;
                else
                    chkAll.CheckState = CheckState.Indeterminate;
            }

            bttOK.Enabled = check != 0;
        }

        private void chkAll_Click(object sender, System.EventArgs e)
        {
            if (chkAll.CheckState != CheckState.Indeterminate)
            {
                bool check = chkAll.Checked;

                foreach(ListViewItem i in listItems.Items)
                {
                    i.Checked = check;
                }

                bttOK.Enabled = check;
            }
        }

        #region Properties

        /// <summary>
        /// Gets the session defined by the user.
        /// </summary>
        public EnvironmentSession Session
        {
            get
            {
                IList<EnvironmentSessionItem> items = new List<EnvironmentSessionItem>();

                foreach (ListViewItem i in listItems.Items)
                    if (i.Checked)
                        items.Add((EnvironmentSessionItem)i.Tag);

                return new EnvironmentSession(txtName.Text, items);
            }
        }

        /// <summary>
        /// Gets or sets the indication if current session should
        /// forget about changes stored into persistent storage.
        /// </summary>
        public bool ClearCurrentSession
        {
            get { return chkClear.Checked; }
            set { chkClear.Checked = value; }
        }

        #endregion
    }
}
