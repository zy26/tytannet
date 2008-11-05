using System.Collections.Generic;
using System.Drawing;
using Pretorianie.Tytan.Core.BaseForms;
using Pretorianie.Tytan.Core.Data;
using System.Windows.Forms;

namespace Pretorianie.Tytan.Forms
{
    public partial class RegistryRemoveFavorite : BasePackageForm
    {
        private readonly Color SelectedColor;
        private readonly Color NormalColor;

        public RegistryRemoveFavorite()
        {
            InitializeComponent();

            // setup colors:
            SelectedColor = Color.DarkOrange;
            NormalColor = listView.BackColor;
        }

        /// <summary>
        /// Gets the serialized list of available favorite items.
        /// </summary>
        public string[] Favorites
        {
            get
            {
                if(listView.Items.Count == 0)
                 return null;

                string[] result = new string[listView.Items.Count*2];
                int i = 0;

                foreach(ListViewItem item in listView.Items)
                {
                    result[i++] = item.SubItems[0].Text;
                    result[i++] = item.SubItems[1].Text;
                }

                return result;
            }
        }

        /// <summary>
        /// Gets the number of currently checked items.
        /// </summary>
        private int NumberOfChecked
        {
            get
            {
                int result = 0;

                foreach (ListViewItem i in listView.Items)
                    if (i.Checked)
                        result++;

                return result;
            }
        }

        /// <summary>
        /// Sets the items that will be shown on the GUI.
        /// </summary>
        public void Initialize(NamedValueCollection<string> favorites)
        {
            ActiveControl = listView;
            listView.Items.Clear();

            if (favorites != null)
                foreach (string name in favorites.Names)
                {
                    ListViewItem item = new ListViewItem(new string[] {name, favorites[name]});

                    listView.Items.Add(item);
                }
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            if (NumberOfChecked == 0)
            {
                foreach (ListViewItem i in listView.SelectedItems)
                    listView.Items.Remove(i);
            }
            else
            {
                List<ListViewItem> toRemove = new List<ListViewItem>();

                foreach(ListViewItem i in listView.Items)
                    if(i.Checked)
                        toRemove.Add(i);

                // remove items that are checked:
                foreach(ListViewItem i in toRemove)
                    listView.Items.Remove(i);
            }
        }

        private void listView_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            RefreshItem(e.Item);
        }

        private void RefreshItem(ListViewItem i)
        {
            // processing name:
            if (i.Checked)
            {
                i.BackColor = SelectedColor;
            }
            else
            {
                i.BackColor = NormalColor;
            }
        }
    }
}

