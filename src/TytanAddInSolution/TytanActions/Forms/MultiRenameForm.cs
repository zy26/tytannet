using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using EnvDTE;
using Pretorianie.Tytan.Core.BaseForms;
using Pretorianie.Tytan.Core.Helpers;

namespace Pretorianie.Tytan.Forms
{
    public partial class MultiRenameForm : BasePackageForm
    {
        private MultiRenameNameForm dlgSelection;
        private IList<CodeFunction> methods;

        StringHelper.IStringRenamer renamer;

        private readonly Color SelectedColor;
        private readonly Color NormalColor;

        public MultiRenameForm()
        {
            InitializeComponent();
            cmbLetter.SelectedIndex = 0;
            ActiveControl = textFormula;

            // setup colors:
            SelectedColor = Color.LimeGreen;
            NormalColor = listView.BackColor;
        }

        /// <summary>
        /// Sets the methods on the interface.
        /// </summary>
        public void InitInterface(IList<CodeFunction> methods, IList<CodeFunction> selectedMethods)
        {
            ListViewItem i;

            this.methods = methods;

            listView.Items.Clear();
            checkState.Checked = true;

            renamer = StringHelper.CreateCombineRenamer(textFormula.Text, RecognizeRenamer);
            
            if (this.methods != null)
            {

                foreach (CodeFunction m in this.methods)
                {
                    i = new ListViewItem(m.Name);
                    i.Checked = selectedMethods != null && selectedMethods.Contains(m);
                    i.Tag = m;

                    if (i.Checked)
                    {
                        i.SubItems.Add(ProcessName(m.Name, renamer));
                        i.BackColor = SelectedColor;
                    }
                    else
                    {
                        i.SubItems.Add(m.Name);
                        i.BackColor = NormalColor;
                    }

                    listView.Items.Add(i);
                }
            }

            ActiveControl = textFormula;
        }

        private void RecognizeRenamer(string formulaFragment, out StringHelper.IStringRenamer r)
        {
            if (string.IsNullOrEmpty(formulaFragment))
                r = null;
            else
            {
                if (formulaFragment == "[C]" || formulaFragment == "[c]")
                    r = new StringHelper.CounterRenamer(Counter);
                else
                    if (formulaFragment[0] == '[' && (formulaFragment[1] == 'N' || formulaFragment[1] == 'n')
                        && formulaFragment[formulaFragment.Length - 1] == ']')
                    {
                        string inner = formulaFragment.Substring(2, formulaFragment.Length - 3).Trim();
                        if (string.IsNullOrEmpty(inner))
                            r = new StringHelper.NameRenamer();
                        else
                        {
                            string[] numbers = inner.Split(new char[] { ' ', '-' }, StringSplitOptions.RemoveEmptyEntries);

                            if (numbers == null || numbers.Length == 0)
                                r = new StringHelper.NameRenamer();
                            else
                                r = new StringHelper.NameRenamer(numbers[0], (numbers.Length > 1 ? numbers[1] : null));
                        }
                    }
                    else
                        r = new StringHelper.StaticRenamer(formulaFragment);
            }
        }

        private void checkState_CheckedChanged(object sender, EventArgs e)
        {
            foreach (ListViewItem i in listView.Items)
                i.Checked = checkState.Checked;
        }

        public bool ReadInterface(out IList<CodeFunction> methods, out IList<string> names)
        {
            IList<CodeFunction> resMethods = new List<CodeFunction>();
            IList<string> resNames = new List<string>();
            int index = 0;

            StringHelper.IStringRenamer r = StringHelper.CreateCombineRenamer(textFormula.Text, RecognizeRenamer);

            // process only checked items:
            foreach (ListViewItem i in listView.Items)
            {
                if (i.Checked)
                {
                    resMethods.Add(this.methods[index]);
                    resNames.Add(ProcessName(this.methods[index].Name, r));
                }
                index++;
            }

            // evaluate result:
            if (resMethods.Count > 0)
            {
                methods = resMethods;
                names = resNames;
                return true;
            }
            else
            {
                methods = null;
                names = null;
                return false;
            }
        }

        #region Properties

        /// <summary>
        /// Gets or sets the renaming formula.
        /// </summary>
        public string Formula
        {
            get { return textFormula.Text; }
            set { textFormula.Text = value; }
        }

        /// <summary>
        /// Gets or sets the current counter description on the screen.
        /// </summary>
        public StringHelper.CounterDetails Counter
        {
            get { return new StringHelper.CounterDetails((uint)numericStart.Value, (uint)numericStep.Value, (int)numericDigits.Value); }
            set
            {
                numericStart.Value = value.Current;
                numericStep.Value = value.Step;
                numericDigits.Value = value.Digits;
            }
        }

        /// <summary>
        /// Gets or sets the index.
        /// </summary>
        public int FormatIndex
        {
            get { return cmbLetter.SelectedIndex; }
            set { if (value >= cmbLetter.Items.Count) cmbLetter.SelectedIndex = 0; else cmbLetter.SelectedIndex = value; }
        }

        #endregion

        #region Helper Functions

        private void InsertFormulaText(string text)
        {
            textFormula.SelectionLength = 0;
            textFormula.SelectedText = text;
            if (!string.IsNullOrEmpty(text))
                textFormula.SelectionStart += text.Length;
        }

        #endregion

        private string ProcessName(string inputText, StringHelper.IStringRenamer r)
        {
            // processing name:
            string newName = r.Rename(inputText);

            // process the first letter:
            if (!string.IsNullOrEmpty(newName))
                switch (cmbLetter.SelectedIndex)
                {
                    case 0: newName = char.ToUpper(newName[0]) + newName.Substring(1); break;
                    case 1: newName = char.ToLower(newName[0]) + newName.Substring(1); break;
                }

            // show to the user:
            return newName;
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            renamer = StringHelper.CreateCombineRenamer(textFormula.Text, RecognizeRenamer);
            
            if (methods != null)
            {
                foreach (ListViewItem i in listView.Items)
                {
                    RefreshItem(i);
                }
            }
        }


        private void listView_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            //RefreshItem(e.Item);
            buttonRefresh_Click(sender, e);
        }

        private void RefreshItem(ListViewItem i)
        {
            // processing name:
            if (i.Checked)
            {
                i.SubItems[1].Text = ProcessName(i.Text, renamer);
                i.BackColor = SelectedColor;
            }
            else
            {
                i.SubItems[1].Text = i.Text;
                i.BackColor = NormalColor;
            }
        }

        private void buttonNameFull_Click(object sender, EventArgs e)
        {
            InsertFormulaText("[N]");
        }

        private void buttonNameRange_Click(object sender, EventArgs e)
        {
            if (dlgSelection == null)
                dlgSelection = new MultiRenameNameForm();

            dlgSelection.SetUI(listView.Items[0].Text);

            if(dlgSelection.ShowDialog () == DialogResult.OK)
            {
                if (dlgSelection.LastLetter != -1)
                    InsertFormulaText(string.Format("[N{0}-{1}]", dlgSelection.FirstLetter, dlgSelection.LastLetter));
                else
                    InsertFormulaText(string.Format("[N{0}]", dlgSelection.FirstLetter));
            }
        }

        private void buttonCounter_Click(object sender, EventArgs e)
        {
            InsertFormulaText("[C]");
        }

        private void changeStateAboveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView.SelectedIndices != null && listView.SelectedIndices.Count > 0)
            {
                int index = listView.SelectedIndices[0];

                for (int i = 0; i <= index; i++)
                    listView.Items[i].Checked = !listView.Items[i].Checked;
            }
        }

        private void enableStateAboveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView.SelectedIndices != null && listView.SelectedIndices.Count > 0)
            {
                int index = listView.SelectedIndices[0];

                for (int i = 0; i <= index; i++)
                    listView.Items[i].Checked = true;
            }
        }

        private void disableStateAboveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView.SelectedIndices != null && listView.SelectedIndices.Count > 0)
            {
                int index = listView.SelectedIndices[0];

                for (int i = 0; i <= index; i++)
                    listView.Items[i].Checked = false;
            }
        }

        private void changeStateBelowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView.SelectedIndices != null && listView.SelectedIndices.Count > 0)
            {
                int index = listView.SelectedIndices[0];

                for (int i = index; i < listView.Items.Count; i++)
                    listView.Items[i].Checked = !listView.Items[i].Checked;
            }
        }

        private void enableStateBelowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView.SelectedIndices != null && listView.SelectedIndices.Count > 0)
            {
                int index = listView.SelectedIndices[0];

                for (int i = index; i < listView.Items.Count; i++)
                    listView.Items[i].Checked = true;
            }
        }

        private void disableStateBelowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView.SelectedIndices != null && listView.SelectedIndices.Count > 0)
            {
                int index = listView.SelectedIndices[0];

                for (int i = index; i < listView.Items.Count; i++)
                    listView.Items[i].Checked = false;
            }
        }

    }
}