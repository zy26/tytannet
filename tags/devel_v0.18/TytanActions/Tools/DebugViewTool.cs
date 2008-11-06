using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using Pretorianie.Tytan.Core.CustomAddIn;
using Pretorianie.Tytan.Core.Data;
using Pretorianie.Tytan.Core.DbgView;
using Pretorianie.Tytan.Core.Helpers;

namespace Pretorianie.Tytan.Tools
{
    public partial class DebugViewTool : UserControl
    {
        #region Internal Classes

        private class ProcessItem
        {
            private readonly int pid;
            private readonly string name;

            public ProcessItem(int pid, string name)
            {
                this.pid = pid;
                this.name = name;
            }

            #region Properties

            public int PID
            {
                get { return pid; }
            }

            public string Name
            {
                get { return name; }
            }

            #endregion

            public override string  ToString()
            {
 	             return name;
            }
        }

        private class InternalItemEventArgs : EventArgs
        {
            private readonly int count;
            private readonly int filteredCount;

            public InternalItemEventArgs(int count, int filteredCount)
            {
                this.count = count;
                this.filteredCount = filteredCount;
            }

            #region Properties

            public int Count
            {
                get { return count; }
            }

            public int FilteredCount
            {
                get { return filteredCount; }
            }

            #endregion
        }

        #endregion

        public const string DialogTitle = "Debug View";
        public const string ToolName = "DebugViewTool";

        public const string ClearFilterMessage = "-- Clear Filter --";

        public delegate void ItemCodeJumpHandler(DebugViewTool sender, DebugViewData data, DbgViewCodeJumpStyle style);
        public delegate void ItemSelectedHandler(DebugViewTool sender, int selectedItemIndex, DebugViewData data);

        /// <summary>
        /// Occures when jump to the code is requested for particular item.
        /// </summary>
        public event ItemCodeJumpHandler ItemCodeJump;
        /// <summary>
        /// Occures when the item is selected inside the view by the user.
        /// </summary>
        public event ItemSelectedHandler ItemSelected;

        private SaveFileDialog dlgExport;
        private OpenFileDialog dlgImport;
        private ListViewItem selectedItem;
        private ProcessItem selectedProcess;

        private readonly DebugViewFilteredListData items = new DebugViewFilteredListData();
        private readonly StringHelper.IStringFilter filter;
        private int selectedItemIndex = -1;

        public DebugViewTool()
        {
            InitializeComponent();
            RefreshProcessCombo();

            list.RetrieveVirtualItem += RetrieveVirtualItem;
            DebugViewMonitor.ReceivedMessage += ReceivedMessage;
            ServiceEnabled = true;

            toolStripCustomColumns.SelectedIndex = 2;

            toolStripSaveAs.Enabled = false;
            toolStripClear.Enabled = false;
            clearToolStripMenuItem.Enabled = false;
            filter = StringHelper.CreateStarFilter(null);

            SetItemSelected(-1, null, false);
            FilterMessages = null;

            // store the reference of the created tool:
            CustomAddInManager.LastCreatedPackageTool = this;
        }

        private void DebugViewTool_Load(object sender, EventArgs e)
        {
            RestoreState();
        }

        #region State Management

        public void RestoreState()
        {
            PersistentStorageData data = PersistentStorageHelper.Load(ToolName);

            if (data != null && data.Count > 0)
            {
                string[] storedFilter = data.GetMultiString("Filter");
                if (storedFilter == null)
                {
                    string f = data.GetString("Filter");

                    if (f != null)
                        storedFilter = new string[] {f};
                }

                FilterMessages = storedFilter;
            }
        }

        public void SaveState()
        {
            string[] storedFilter = FilterMessages;

            if (storedFilter != null && storedFilter.Length > 0)
            {
                PersistentStorageData data = new PersistentStorageData(ToolName);

                data.Add("Filter", storedFilter);

                PersistentStorageHelper.Save(data);
            }
        }

        #endregion

        /// <summary>
        /// Shows or hides the code jump options.
        /// </summary>
        public bool VisibleCodeJump
        {
            get
            {
                return toolStripSeparator4.Visible;
            }
            set
            {
                toolStripMenuItem1.Visible = value;
                jumpToFunctionToolStripMenuItem.Visible = value;

                toolStripSeparator2.Visible = value;
                toolStripJump.Visible = value;
                toolStripSeparator4.Visible = value;
                toolStripLabel3.Visible = value;
                toolStripCustomColumns.Visible = value;
            }
        }

        /// <summary>
        /// Gets or sets the filter messages.
        /// </summary>
        public string[] FilterMessages
        {
            get
            {
                string[] result = null;

                try
                {
                    if (toolStripMessage.Items.Count > 1)
                    {
                        result = new string[toolStripMessage.Items.Count - 1];

                        for (int i = 1; i < toolStripMessage.Items.Count; i++)
                            result[i - 1] = (toolStripMessage.Items[i] as string);
                    }
                }
                catch (Exception ex)
                {
                    Trace.Write(ex.Message);
                }

                return result;
            }
            set
            {
                toolStripMessage.Items.Clear();
                toolStripMessage.Items.Add(ClearFilterMessage);

                if (value != null && value.Length > 0)
                    foreach (string f in value)
                        InsertFilterMessage(f);
            }
        }

        /// <summary>
        /// Gets or sets the index of selected item on the list.
        /// </summary>
        public int SelectedItemIndex
        {
            get { return selectedItemIndex; }
            set { if (list.Items.Count > value && value >= 0) list.Items[value].Selected = true; }
        }

        private void InsertFilterMessage(string newFilterMessage)
        {
            if (!toolStripMessage.Items.Contains(newFilterMessage))
                toolStripMessage.Items.Add(newFilterMessage);
        }

        void RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            e.Item = items.Filtered[e.ItemIndex];
        }

        void ReceivedMessage(IList<DebugViewData> newItems)
        {
            int count = items.Count;
            int filteredCount = items.FilteredCount;

            items.Add(newItems, FilterPID, filter);

            if (InvokeRequired)
                Invoke(new EventHandler(EnableMoreItems), this, new InternalItemEventArgs(count, filteredCount));
            else
                EnableMoreItems(this, new InternalItemEventArgs(count, filteredCount));
        }

        private void EnableMoreItems(object sender, EventArgs e)
        {
            InternalItemEventArgs x = e as InternalItemEventArgs;

            if (x != null && !toolStripClear.Enabled && items.Count != x.Count)
            {
                toolStripClear.Enabled = true;
                clearToolStripMenuItem.Enabled = true;
                toolStripSaveAs.Enabled = true;
            }

            if (x != null && x.FilteredCount != items.FilteredCount)
            {
                list.BeginUpdate();
                list.VirtualListSize = items.FilteredCount;
                if (AutoScrollDown)
                    list.EnsureVisible(items.FilteredCount - 1);
                list.EndUpdate();
            }
        }

        private void toolStripFeedback_Click(object sender, EventArgs e)
        {
            CallHelper.SendFeedback(this);
        }

        #region Properties

        /// <summary>
        /// Gets or sets the auto-scroll option.
        /// </summary>
        public bool AutoScrollDown
        {
            get
            {
                return toolStripScrollDown.Enabled;
            }
            set
            {
                toolStripScrollDown.Visible =
                    toolStripScrollDown.Enabled = value;
                toolStripScrollNo.Visible =
                    toolStripScrollNo.Enabled = !value;
            }
        }

        /// <summary>
        /// Gets or sets the state of monitoring.
        /// </summary>
        public bool ServiceEnabled
        {
            get
            {
                return toolStripClear.Enabled && !labelError.Visible;
            }
            set
            {
                if (value)
                {
                    bool result = DebugViewMonitor.Start();

                    list.Visible = result;
                    labelError.Visible = !result;
                    if (!result)
                        ServiceEnabled = false;

                    toolStripStop.Enabled =
                        toolStripStop.Visible = true;
                    toolStripStart.Enabled =
                        toolStripStart.Visible = false;
                }
                else
                {
                    DebugViewMonitor.Stop();

                    toolStripStop.Enabled =
                        toolStripStop.Visible = false;
                    toolStripStart.Enabled =
                        toolStripStart.Visible = true;
                }
            }
        }

        /// <summary>
        /// Gets the PID of the process that should only display the messages.
        /// </summary>
        public int FilterPID
        {
            get
            {
                if (selectedProcess != null)
                    return selectedProcess.PID;

                return -1;
            }
        }

        private void toolStripProcesses_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedProcess = toolStripProcesses.SelectedItem as ProcessItem;
        }

        private void toolStripProcesses_DropDownClosed(object sender, EventArgs e)
        {
            ActiveControl = list;
        }

        #endregion

        #region Filtering

        private void RefreshProcessCombo()
        {
            if (toolStripProcesses == null)
                return;

            ProcessItem i = new ProcessItem(-1, "-- all --");
            ProcessItem j = new ProcessItem(-2, "-- disk --");
            SortedList<string, Process> processes = new SortedList<string, Process>();
            string selected = (toolStripProcesses.SelectedItem != null
                               && (toolStripProcesses.SelectedItem) is ProcessItem
                                   ? ((ProcessItem) (toolStripProcesses.SelectedItem)).Name
                                   : null);
            int selectedIndex = 0;
            int counter = 0;
            
            toolStripProcesses.Items.Clear();
            toolStripProcesses.Items.Add(i);
            toolStripProcesses.Items.Add(j);
            if (selected == i.Name)
                selectedIndex = 0;

            foreach (Process p in Process.GetProcesses())
            {
                processes.Add(p.ProcessName + "_" + counter++, p);
            }

            foreach (KeyValuePair<string, Process> p in processes)
            {
                i = new ProcessItem(p.Value.Id, string.Format("(0x{1:X4}) {0}", p.Value.ProcessName, (uint) p.Value.Id));
                toolStripProcesses.Items.Add(i);
                if (selected == i.Name)
                    selectedIndex = toolStripProcesses.Items.Count - 1;
            }

            // select the same process as before:
            toolStripProcesses.SelectedIndex = selectedIndex;
        }

        #endregion

        private void toolStripScrollDown_Click(object sender, EventArgs e)
        {
            AutoScrollDown = false;
        }

        private void toolStripScrollNo_Click(object sender, EventArgs e)
        {
            AutoScrollDown = true;
        }

        private void toolStripClose_Click(object sender, EventArgs e)
        {
            list.VirtualListSize = 0;
            list.Refresh();
            items.Clear();
            toolStripClear.Enabled = false;
            clearToolStripMenuItem.Enabled = false;
            toolStripSaveAs.Enabled = false;
            SetItemSelected(-1, null, true);
        }

        private void toolStripStop_Click(object sender, EventArgs e)
        {
            ServiceEnabled = false;
        }

        private void toolStripStart_Click(object sender, EventArgs e)
        {
            ServiceEnabled = true;
        }

        private void toolStripProcesses_DropDown(object sender, EventArgs e)
        {
            RefreshProcessCombo();
        }

        private void toolStripSaveAs_Click(object sender, EventArgs e)
        {
            if (dlgExport == null)
            {
                dlgExport = new SaveFileDialog();
                dlgExport.Title = "Exporting debug messages";
                dlgExport.AddExtension = true;
                dlgExport.DefaultExt = ".txt";
                dlgExport.RestoreDirectory = true;
                dlgExport.OverwritePrompt = true;
                dlgExport.Filter = "Time & Message|*.*|Time & PID & Message|*.*|Time & Process & Message|*.*|Message Only|*.*";
                dlgExport.FilterIndex = 0;
                dlgExport.InitialDirectory = "C:";
            }

            if (dlgExport.ShowDialog() == DialogResult.OK)
            {
                string errorMessage =
                       items.ExportAsTextFile(dlgExport.FileName, (DebugViewFilteredListData.ExportFormat)(dlgExport.FilterIndex-1));

                if (!string.IsNullOrEmpty(errorMessage))
                    MessageBox.Show(errorMessage, DialogTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void toolStripOpen_Click(object sender, EventArgs e)
        {
            if (dlgImport == null)
            {
                dlgImport = new OpenFileDialog();
                dlgImport.Title = "Importing debug messages";
                dlgImport.DefaultExt = ".txt";
                dlgImport.CheckFileExists = true;
                dlgImport.CheckPathExists = true;
                dlgImport.InitialDirectory = "C:";
                dlgImport.Filter = "Log files|*.txt;*.csv|All files|*.*";
                dlgImport.FilterIndex = 0;
            }

            if (dlgImport.ShowDialog() == DialogResult.OK)
            {
                int count = items.Count;
                int filteredCount = items.FilteredCount;
                int addedCount = items.ImportTextFile(dlgImport.FileName, FilterPID, filter);

                EnableMoreItems(this, new InternalItemEventArgs(count, filteredCount));

                MessageBox.Show(string.Format("Imported: {0} elements.", addedCount), DialogTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void toolStripMessage_Leave(object sender, EventArgs e)
        {
            InsertFilterMessage(toolStripMessage.Text);
        }

        private void toolStripMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
            {
                RefilterItems();
                ActiveControl = list;
            }
        }

        private void SetItemSelected(int selectedIndex, ListViewItem item, bool sendEvent)
        {
            copyToolStripMenuItem.Enabled =
                jumpToFunctionToolStripMenuItem.Enabled =
                    toolStripJump.Enabled = (item != null) && toolStripCustomColumns.SelectedIndex != 0;

            bool different = selectedItemIndex != selectedIndex;
            selectedItem = item;
            selectedItemIndex = selectedIndex;

            if (ItemSelected != null && different && sendEvent)
                ItemSelected(this, selectedItemIndex, (selectedItem != null ? selectedItem.Tag as DebugViewData : null));
        }

        private void RefilterItems()
        {
            CaptureFilter();
            list.VirtualListSize = 0;

            items.ApplyFilter(filter);
            InsertFilterMessage(toolStripMessage.Text);

            // refresh:
            list.VirtualListSize = items.FilteredCount;
            list.Refresh();

            // get the same if possible index of previously selected item
            // and set it on the GUI with events notifications:
            int filteredIndex;
            bool isNewItem = items.GetIndexFromFilter(selectedItem, out filteredIndex);
            if (filteredIndex >= 0)
            {
                list.SelectedIndices.Clear();
                list.SelectedIndices.Add(filteredIndex);
                list.EnsureVisible(filteredIndex);
                SetItemSelected(filteredIndex, items.Filtered[filteredIndex], isNewItem);
            }
        }

        private void CaptureFilter()
        {
            filter.Update(toolStripMessage.Text);
        }

        private void toolStripMessage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (toolStripMessage.SelectedIndex == 0)
            {
                // ask for confirmation:
                if (MessageBox.Show(SharedStrings.Question_DebugView_ClearFilter, DialogTitle, MessageBoxButtons.YesNo,
                                    MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                    FilterMessages = null;
                else
                    ActiveControl = list;
                toolStripMessage.Text = string.Empty;
            }
            else
            {
                RefilterItems();
                ActiveControl = list;
            }
        }

        private void list_MouseDown(object sender, MouseEventArgs e)
        {
            ListViewItem item = list.GetItemAt(e.X, e.Y);

            // since in virtual-mode this control generates SelectedIndexChanged twice
            // (first call with null args) and causes flickering of Properties Window of Visual Studio
            // the trick is that during MouseDown all the selections are detected and propaged

            if (item == null)
                SetItemSelected(-1, null, true);
            else
                SetItemSelected(item.Index, item, true);
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selectedItem != null)
            {
                DebugViewData dbgData = selectedItem.Tag as DebugViewData;

                if (dbgData != null)
                    Clipboard.SetText(dbgData.Message);
            }
        }

        private void list_DoubleClick(object sender, EventArgs e)
        {
            // jump to the proper source file:
            if (selectedItem != null && toolStripJump.Enabled && ItemCodeJump != null)
                ItemCodeJump(this, selectedItem.Tag as DebugViewData, (DbgViewCodeJumpStyle)(toolStripCustomColumns.SelectedIndex - 1));
        }

        private void toolStripCustomColumns_SelectedIndexChanged(object sender, EventArgs e)
        {
            copyToolStripMenuItem.Enabled =
                jumpToFunctionToolStripMenuItem.Enabled =
                    toolStripJump.Enabled = (selectedItem != null) && toolStripCustomColumns.SelectedIndex != 0;
        }

        public void SetStyle(DbgViewCodeJumpStyle newStyle)
        {
            toolStripCustomColumns.SelectedIndex = (int)newStyle + 1;
        }
    }
}
