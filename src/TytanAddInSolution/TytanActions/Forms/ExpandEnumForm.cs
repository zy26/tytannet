using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Pretorianie.Tytan.Forms
{
    public partial class ExpandEnumForm : Pretorianie.Tytan.Core.BaseForms.BasePackageForm
    {
        public ExpandEnumForm()
        {
            InitializeComponent();
        }

        public void SetUI(IList<string> names)
        {
            listView.Items.Clear();

            // add given names:
            if (names != null)
                foreach (string s in names)
                    listView.Items.Add(s);

            // and then check them all:
            foreach(ListViewItem i in listView.Items)
                i.Checked = true;
        }
    }
}
