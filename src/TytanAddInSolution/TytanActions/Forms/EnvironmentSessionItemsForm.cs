using System;
using System.Windows.Forms;
using Pretorianie.Tytan.Core.EnvVarView.Tracking;

namespace Pretorianie.Tytan.Forms
{
    public partial class EnvironmentSessionItemsForm : Pretorianie.Tytan.Core.BaseForms.BasePackageForm
    {
        public EnvironmentSessionItemsForm()
        {
            InitializeComponent();
        }

        public EnvironmentSessionItemsForm(EnvironmentSession session)
        {
            InitializeComponent();
            SetSession(session);
        }

        public void SetSession(EnvironmentSession session)
        {
            if (session == null)
                throw new ArgumentNullException("session");

            // update the view:
            listVariables.Items.Clear();
            foreach (EnvironmentSessionItem i in session.Items)
            {
                ListViewItem listItem = new ListViewItem(i.Target.ToString());
                listItem.SubItems.Add(i.Name);
                listItem.SubItems.Add(i.Action);
                listItem.SubItems.Add(i.Value);
                listItem.SubItems.Add(i.PrimaryValue);
                listVariables.Items.Add(listItem);
            }
        }
    }
}
