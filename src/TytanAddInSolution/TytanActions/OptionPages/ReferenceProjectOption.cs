using System;
using System.Windows.Forms;
using Pretorianie.Tytan.Actions.Misc;
using Pretorianie.Tytan.Core.Data;
using Pretorianie.Tytan.Core.Helpers;

namespace Pretorianie.Tytan.OptionPages
{
    public partial class ReferenceProjectOption : Core.BaseForms.BaseOptionPage
    {
        private OpenFileDialog dlgOpen;
        private PersistentStorageData config;

        public ReferenceProjectOption()
        {
            InitializeComponent();
        }

        protected override Type ActionType
        {
            get { return typeof(ReferenceProjectAction); }
        }

        private void bttReload_Click(object sender, EventArgs e)
        {
            // update the menu:
            ExecuteAction(null);
        }

        private void bttFind_Click(object sender, EventArgs e)
        {
            if (dlgOpen == null)
            {
                dlgOpen = new OpenFileDialog();
                dlgOpen.Filter = SharedStrings.ReferenceProject_BrowseFilter;
                dlgOpen.FilterIndex = 0;
                dlgOpen.Title = SharedStrings.ReferenceProject_OptionDialogTitle;
                dlgOpen.CheckFileExists = true;
                dlgOpen.CheckPathExists = true;
                dlgOpen.DereferenceLinks = true;
                dlgOpen.Multiselect = true;
            }

            // and allow the user to select a file:
            if (dlgOpen.ShowDialog() == DialogResult.OK)
                // add all elements:
                Add(dlgOpen.FileNames);
        }

        protected override void ConfigurationPresent()
        {
            string[] assemblies;
            config = ObjectFactory.LoadConfiguration(ReferenceProjectAction.CofigurationName);

            // add assemblies to the screen:
            listReferences.Items.Clear();
            assemblies = config.GetMultiString(ReferenceProjectAction.Persistent_SystemAssemblies);
            if (assemblies != null)
                Add(assemblies);
        }

        protected override void ConfigurationUpdate(out PersistentStorageData actionConfig)
        {
            string[] assemblies = GetSerializedAssemblies();

            // set new set of system assemblies:
            if (assemblies != null)
                config.Add(ReferenceProjectAction.Persistent_SystemAssemblies, assemblies);
            else
                config.Remove(ReferenceProjectAction.Persistent_SystemAssemblies);

            actionConfig = config;
        }

        /// <summary>
        /// Get the list of assemblies from the list on the screen.
        /// </summary>
        private string[] GetSerializedAssemblies()
        {
            if (listReferences.Items.Count > 0)
            {
                ListBox.ObjectCollection items = listReferences.Items;
                int count = items.Count;
                string[] r = new string[count];

                for (int i = 0; i < count; i++)
                    r[i] = items[i].ToString();

                return r;
            }
            else
                return null;
        }

        private void listReferences_SelectedIndexChanged(object sender, EventArgs e)
        {
            bttRemove.Enabled = listReferences.Items.Count > 0 && listReferences.SelectedIndex > -1;
        }

        private void bttAdd_Click(object sender, EventArgs e)
        {
            string path = txtPath.Text.Trim();

            if (!string.IsNullOrEmpty(path))
                Add(path);
            txtPath.Text = string.Empty;
        }

        private void bttRemove_Click(object sender, EventArgs e)
        {
            int i = listReferences.SelectedIndex;
            if (i > -1)
                listReferences.Items.RemoveAt(i);
        }

        private void Add(string[] names)
        {
            foreach (string s in names)
                Add(s);
        }

        private void Add(string name)
        {
            if (!listReferences.Items.Contains(name))
                listReferences.Items.Add(name);
        }
    }
}

