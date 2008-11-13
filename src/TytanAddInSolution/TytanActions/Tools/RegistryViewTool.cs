using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using Pretorianie.Tytan.Core.CustomAddIn;
using Pretorianie.Tytan.Core.Data;
using Pretorianie.Tytan.Core.Helpers;
using Pretorianie.Tytan.Forms;

namespace Pretorianie.Tytan.Tools
{
    public partial class RegistryViewTool : UserControl
    {
        protected const string PlaceholderName = "-- placeholder --";

        private RegistryNewValueForm dlgNewValue;
        private RegistryAddFavorite dlgAddFavorite;
        private RegistryRemoveFavorite dlgRemoveFavorite;
        private RegistryFindForm dlgFind;
        private SaveFileDialog dlgExport;

        private readonly NamedValueCollection<string> favorites = new NamedValueCollection<string>();
        private readonly BackForwardNavigator<string> navigator = new BackForwardNavigator<string>(64);
        private bool canStoreSelectNavigation = true;

        public const int UnaccessibleNodeImageIndex = 2;
        public const string DialogTitle = "Registry View";
        public const string ToolName = "RegistryViewTool";

        private const string Settings_Path = "Path";
        private const string Settings_Favorites = "Favorites";

        private readonly TreeNode classesRoot;
        // private readonly TreeNode localMachineRoot;
        private readonly TreeNode currentUserRoot;

        public RegistryViewTool()
        {
            InitializeComponent();

            // add default keys:
            treeView.Nodes.Clear();

            treeView.Nodes.Add(classesRoot = ToTreeNode(null, Registry.ClassesRoot, true));
            treeView.Nodes.Add(currentUserRoot = ToTreeNode(null, Registry.CurrentUser, true));
            treeView.Nodes.Add(/*localMachineRoot = */ ToTreeNode(null, Registry.LocalMachine, true));
            treeView.Nodes.Add(ToTreeNode(null, Registry.Users, true));

            // store the reference of the created tool:
            CustomAddInManager.LastCreatedPackageTool = this;
        }

        #region State Management
        
        private void RegistryViewTool_Load(object sender, EventArgs e)
        {
            RestoreState();
        }

        /// <summary>
        /// Restores the stored state of the editor.
        /// </summary>
        public void RestoreState()
        {
            PersistentStorageData data = PersistentStorageHelper.Load(ToolName);

            if (data != null && data.Count > 0)
            {
                NavigateTo(data.GetString(Settings_Path));
                ImportFavorites(data.GetMultiString(Settings_Favorites));
            }

            navigator.Clear();
            RefreshNavigationButtons();
            RefreshFavoritesMenu();
        }

        /// <summary>
        /// Writes the current state of the control to a persistent storage area.
        /// </summary>
        public void StoreState()
        {
            if (treeView.SelectedNode != null)
            {
                PersistentStorageData data = new PersistentStorageData(ToolName);

                data.Add(Settings_Path, treeView.SelectedNode.FullPath);
                data.Add(Settings_Favorites, ExportFavorites());

                PersistentStorageHelper.Save(data);
            }
        }

        private string[] ExportFavorites()
        {
            if (favorites.Count == 0)
                return new string[0];

            string[] result = new string[favorites.Count * 2];
            int i = 0;

            // for each item generate two entries:
            foreach (string name in favorites.Names)
            {
                result[i++] = name;
                result[i++] = favorites[name];
            }

            return result;
        }

        private void ImportFavorites(string[] data)
        {
            if (data != null && data.Length > 0)
            {
                int size = data.Length/2;

                // read all the data and store inside proper collection:
                for (int i = 0, j = 0; i < size; i++, j += 2)
                    favorites.Add(data[j], data[j + 1]);
            }
        }

        /// <summary>
        /// Jumps on the <c>TreeView</c> to the given registry path.
        /// </summary>
        private void NavigateTo(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                canStoreSelectNavigation = false;

                textCurrentPath.Text = path;
                textCurrentPath_Leave(null, EventArgs.Empty);

                if (treeView.SelectedNode != null)
                    treeView.SelectedNode.EnsureVisible();

                canStoreSelectNavigation = true;
            }
        }

        /// <summary>
        /// Adds the currently selected node to the back-forward navigator.
        /// </summary>
        private void StoreCurrentSelection()
        {
            if (treeView.SelectedNode != null && canStoreSelectNavigation)
            {
                navigator.Add(CurrentNavigationPath);
                RefreshNavigationButtons();
            }
        }

        private string CurrentNavigationPath
        {
            get { return (treeView.SelectedNode != null ? treeView.SelectedNode.FullPath : null); }
        }

        #endregion

        #region Operations

        /// <summary>
        /// Removes unneeded brackets from the specified path.
        /// </summary>
        private static string ClearPath(string path)
        {
            if (string.IsNullOrEmpty(path))
                return null;

            path = path.Trim();

            if (path.StartsWith("[") && path.EndsWith("]"))
                return path.Substring(1, path.Length - 2);

            return path;
        }
        
        /// <summary>
        /// Updates the view to show specified path.
        /// </summary>
        private void UpdateViewByPath(string path)
        {
            string[] items = (string.IsNullOrEmpty(path) ? null : ClearPath(path).Split(new char[] { '\\', '|', '/' }, StringSplitOptions.RemoveEmptyEntries));

            if (items != null && items.Length > 0)
            {
                StoreCurrentSelection();

                canStoreSelectNavigation = false;
                treeView.BeginUpdate();
                
                FindNode(treeView, treeView.Nodes, items, 0);
                // and select proper item inside the list view:
                FindItem(listView, listView.Items, items[items.Length - 1]);

                treeView.EndUpdate();
                canStoreSelectNavigation = true;
            }
        }

        /// <summary>
        /// Selects specified item on the <paramref name="listView"/>.
        /// </summary>
        private static void FindItem(ListView listView, ListView.ListViewItemCollection listViewItemCollection, string name)
        {
            foreach(ListViewItem i in listViewItemCollection)
                if (string.Compare(i.Name, name, true) == 0)
                {
                    i.Selected = true;
                    i.EnsureVisible();
                }
        }

        /// <summary>
        /// Selects specified item on the <paramref name="treeView"/>.
        /// </summary>
        private static void FindNode(TreeView treeView, TreeNodeCollection nodes, string[] items, int index)
        {
            TreeNode startNode = null;

            // find the main tree:
            foreach (TreeNode n in nodes)
            {
                if (string.Compare(n.Text, items[index], true) == 0)
                {
                    startNode = n;
                    break;
                }
            }

            // expand that node:
            if (startNode != null)
            {
                treeView.SelectedNode = startNode;
                startNode.Expand();
                if (items.Length > index + 1)
                    FindNode(treeView, startNode.Nodes, items, index + 1);
            }
        }

        /// <summary>
        /// Converts the registry description to the <see cref="TreeNode"/> element.
        /// </summary>
        private static TreeNode ToTreeNode(string name, RegistryKey key, bool mainKey)
        {
            string keyName = (string.IsNullOrEmpty(name) ? key.Name : name);
            TreeNode n = new TreeNode(keyName);

            // serialize to node:
            if (mainKey)
                n.Tag = key;
            n.Name = keyName;

            // add [+]:
            try
            {
                if (key.SubKeyCount > 0)
                {
                    n.Nodes.Add(PlaceholderName);
                }
            }
            catch
            {
                n.Nodes.Add(new TreeNode("not accessible", UnaccessibleNodeImageIndex, UnaccessibleNodeImageIndex));
            }

            return n;
        }

        /// <summary>
        /// Gets the <see cref="RegistryKey"/> assigned to parent of particular node.
        /// </summary>
        private static RegistryKey GetParentKey(TreeNode node)
        {
            RegistryKey key = node.Tag as RegistryKey;

            while (node != null)
            {
                node = node.Parent;
                if (node != null)
                    key = node.Tag as RegistryKey;
            }

            return key;
        }

        /// <summary>
        /// Opens a new <see cref="RegistryKey"/> for particular node.
        /// It should be closed if 'close' parameter is 'true'.
        /// </summary>
        private static RegistryKey GetKey(TreeNode node, bool writable, out bool close)
        {
            RegistryKey key = GetParentKey(node);

            close = false;
            if (key != null)
            {
                int index = node.FullPath.IndexOf('\\');
                string keyName = (index > 0 ? node.FullPath.Substring(index + 1) : null);

                if (!string.IsNullOrEmpty(keyName))
                {
                    try
                    {
                        key = key.OpenSubKey(keyName, writable);
                        if (key != null)
                            close = true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, DialogTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
            }

            return key;
        }

        private static string GetSafeValueName(string valueName)
        {
            return (string.IsNullOrEmpty(valueName) ? "(default)" : valueName);
        }

        /// <summary>
        /// Gets the index of an image for given data type read from registry.
        /// </summary>
        private static int GetImageIndex(RegistryValueKind kind)
        {
            switch (kind)
            {
                case RegistryValueKind.Binary:
                case RegistryValueKind.DWord:
                case RegistryValueKind.QWord:
                case RegistryValueKind.Unknown:
                    return 3;
                default:
                    return 4;
            }
        }

        private static string GetValueString(RegistryKey key, string valueName, RegistryValueKind kind)
        {
            try
            {
                switch (kind)
                {
                    case RegistryValueKind.QWord:
                    case RegistryValueKind.DWord:
                        return string.Format("0x{0:X} ({0})", key.GetValue(valueName));
                    case RegistryValueKind.Binary:
                        {
                            StringBuilder s = new StringBuilder();
                            byte[] data = (byte[]) key.GetValue(valueName);
                            if (data != null && data.Length > 0)
                            {
                                if (data[0] < 10)
                                    s.AppendFormat("0x0{0:X}", data[0]);
                                else
                                    s.AppendFormat("0x{0:X}", data[0]);
                                for (int i = 1; i < data.Length; i++)
                                    if (data[i] < 10)
                                        s.AppendFormat(" 0{0:X}", data[i]);
                                    else
                                        s.AppendFormat(" {0:X}", data[i]);
                            }
                            return s.ToString();
                        }
                    case RegistryValueKind.MultiString:
                        {
                            StringBuilder s = new StringBuilder();
                            string[] data = (string[]) key.GetValue(valueName);
                            if (data != null && data.Length > 0)
                                foreach (string p in data)
                                    s.Append(p).Append(Environment.NewLine);

                            return s.ToString();
                        }
                    case RegistryValueKind.ExpandString:
                        {
                            string x =
                                key.GetValue(valueName, null, RegistryValueOptions.DoNotExpandEnvironmentNames) as
                                string;
                            return (x ?? "(not set)");
                        }
                    default:
                        {
                            object x = key.GetValue(valueName);
                            return x == null ? "(not set)" : x.ToString();
                        }
                }
            }
            catch (Exception ex)
            {
                return "[not set: " + ex.Message;
            }
        }

        private void PopulateListView(RegistryKey key)
        {
            ListViewItem item;
            string[] values = (key != null ? key.GetValueNames() : null);
            RegistryValueKind kind;

            listView.Items.Clear();

            if (values != null)
            {
                foreach (string v in values)
                {
                    try
                    {
                        kind = key.GetValueKind(v);
                        item = new ListViewItem();
                        item.Text = GetSafeValueName(v);
                        item.Name = v;
                        item.SubItems.Add(kind.ToString().ToUpper());
                        item.ImageIndex = GetImageIndex(kind);

                        item.SubItems.Add(GetValueString(key, v, kind));
                        listView.Items.Add(item);
                    }
                    catch
                    {
                        listView.Items.Add(new ListViewItem(v + " [error]"));
                    }
                }
            }
        }

        private void RefreshSelection()
        {
            bool nodeSelected = treeView.SelectedNode != null;
            bool itemSelected = listView.SelectedItems != null && listView.SelectedItems.Count > 0;

            jumpToolStripMenuItem.Enabled = itemSelected;
            toolStripEdit.Enabled = editToolStripMenu.Enabled = itemSelected;
            toolStripDelete.Enabled = nodeSelected || itemSelected;

            renameToolStripMenu.Enabled =
                deleteToolStripMenu.Enabled =
                copyNameToolStripMenuItem1.Enabled =
                copyvalueToolStripMenuItem.Enabled = itemSelected;

            newKeyToolMenuTree.Enabled =
                deleteToolStripMenuTree.Enabled = nodeSelected;
        }

        private void RefreshNavigationButtons()
        {
            toolStripNavBack.Enabled = navigator.CanGoBack;
            toolStripNavForward.Enabled = navigator.CanGoForward;
        }

        private void RefreshFavoritesMenu()
        {
            // remove all the old items:
            int count = toolStripDropDownFavorites.DropDownItems.Count;
            for ( int i = 3; i < count; i++)
                toolStripDropDownFavorites.DropDownItems.RemoveAt(3);
            toolStripDropDownFavorites.DropDownItems[2].Visible = favorites.Count > 0;

            // add new items:
            foreach(string name in favorites.Names)
            {
                toolStripDropDownFavorites.DropDownItems.Add(
                    new ToolStripMenuItem(name, SharedIcons.BulletGo, ClickedFavorite, favorites[name]));
            }
        }

        private void ClickedFavorite(object sender, EventArgs e)
        {
            ToolStripMenuItem x = sender as ToolStripMenuItem;

            if (x != null)
            {
                StoreCurrentSelection();
                NavigateTo(x.Name);
                RefreshNavigationButtons();
            }
        }

        #endregion

        #region Add / Edit / Remove
        
        /// <summary>
        /// Adds new key to specified node.
        /// </summary>
        private bool AddNewKey(string keyName, TreeNode node)
        {
            try
            {
                bool close;
                bool result = false;
                RegistryKey key = GetKey(node, true, out close);

                // add new key:
                if (key != null)
                {
                    RegistryKey sub = key.CreateSubKey(keyName);
                    result = true;

                    if (close)
                        key.Close();
                    if (sub != null)
                        sub.Close();

                    // refresh the view:
                    if (treeView.SelectedNode.Nodes.Count == 0)
                        treeView.SelectedNode.Nodes.Add(PlaceholderName);
                }

                return result;
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
                Trace.WriteLine(ex.StackTrace);

                MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace, DialogTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        
        /// <summary>
        /// Removes specified key.
        /// </summary>
        private static bool DeleteKey(TreeNode node, string keyName, bool force)
        {
            try
            {
                bool close;
                bool result = false;
                RegistryKey key = GetKey(node, true, out close);

                // remove new key:
                if (key != null && (force || (MessageBox.Show("Do you want to delete key: '" + keyName + "' ?", DialogTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)))
                {
                    key.DeleteSubKeyTree(keyName);
                    result = true;

                    if (close)
                        key.Close();
                }

                return result;
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
                Trace.WriteLine(ex.StackTrace);

                MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace, DialogTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        
        /// <summary>
        /// Adds new named value.
        /// </summary>
        private bool AddNewValue(TreeNode node, string name, RegistryValueKind type, object value, bool refreshList)
        {
            try
            {
                bool close;
                bool result = false;
                RegistryKey key = GetKey(node, true, out close);

                // add new value:
                if (key != null)
                {
                    key.SetValue(name, value, type);
                    result = true;

                    if (refreshList)
                        PopulateListView(key); 

                    if (close)
                        key.Close();
                }

                return result;
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
                Trace.WriteLine(ex.StackTrace);

                MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace, DialogTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private bool DeleteKeyValue(TreeNode node, string valueDisplayName, string valueName, bool force, bool refreshList)
        {
            try
            {
                bool close;
                bool result = false;
                RegistryKey key = GetKey(node, true, out close);

                // add new value:
                if (key != null && (force || (MessageBox.Show("Do you want to delete value: '" + valueDisplayName + "' ?", DialogTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)))
                {
                    key.DeleteValue(valueName, false);
                    result = true;

                    if (refreshList)
                        PopulateListView(key);

                    if (close)
                        key.Close();
                }

                return result;
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
                Trace.WriteLine(ex.StackTrace);

                MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace, DialogTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        #endregion

        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            textCurrentPath.Text = e.Node.FullPath;

            // set data:
            bool close;
            RegistryKey key = GetKey(e.Node, false, out close);

            PopulateListView(key);

            if (close)
                key.Close();

            RefreshSelection();
        }

        private void treeView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            try
            {
                RegistryKey key;
                RegistryKey current;

                if (e.Node != null)
                {
                    e.Node.Nodes.Clear();
                    bool close;
                    key = GetKey(e.Node, false, out close);

                    string[] names = key.GetSubKeyNames();

                    foreach (string name in names)
                    {
                        try
                        {
                            current = key.OpenSubKey(name);
                            if (current != null)
                            {
                                e.Node.Nodes.Add(ToTreeNode(name, current, false));
                                current.Close();
                            }
                        }
                        catch
                        {
                            e.Node.Nodes.Add(new TreeNode(name + SharedStrings.RegistryView_NotAccessible,
                                                          UnaccessibleNodeImageIndex, UnaccessibleNodeImageIndex));
                        }
                    }

                    if (close)
                        key.Close();
                }
            }
            catch // (Exception ex)
            {
                if (e != null && e.Node != null && !e.Node.Text.EndsWith(SharedStrings.RegistryView_NotAccessible))
                {
                    e.Node.Text += SharedStrings.RegistryView_NotAccessible;
                    e.Node.ImageIndex = UnaccessibleNodeImageIndex;
                    e.Node.SelectedImageIndex = UnaccessibleNodeImageIndex;
                }
            }
        }

        private void textCurrentPath_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
            {
                UpdateViewByPath(textCurrentPath.Text);
                ActiveControl = treeView;
            }
        }

        private void textCurrentPath_Leave(object sender, EventArgs e)
        {
            UpdateViewByPath(textCurrentPath.Text);
        }

        private void toolStripFeedback_Click(object sender, EventArgs e)
        {
            CallHelper.SendFeedback(this);
        }

        private void listView_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshSelection();
        }

        private void jumpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string value = listView.SelectedItems[0].SubItems[2].Text.ToLower();
            TreeNode childNode = null;
            TreeNode currentNode;

            // find the child element:
            foreach (TreeNode n in treeView.SelectedNode.Nodes)
                if (n.Text.ToLower().StartsWith(value))
                {
                    childNode = n;
                    break;
                }

            // find at the same level as the current node:
            currentNode = treeView.SelectedNode;
            while (childNode == null && currentNode.Parent != null)
            {
                foreach (TreeNode n in treeView.SelectedNode.Parent.Nodes)
                    if (n.Text.ToLower().StartsWith(value))
                    {
                        childNode = n;
                        break;
                    }
                currentNode = currentNode.Parent;
            }

            // check if that can be found inside HKEY_CLASSES_ROOT:
            if (childNode == null && classesRoot != null && currentUserRoot != null && value.StartsWith("{"))
            {
                TreeNode clsid;

                // inside local for current user: HKEY_CURRENT_USER\Software\Classes\CLSID
                currentUserRoot.Expand();
                clsid = currentUserRoot.Nodes["Software"];
                clsid.Expand();
                clsid = clsid.Nodes["Classes"];
                clsid.Expand();
                clsid = clsid.Nodes["CLSID"];
                if (clsid != null)
                {
                    clsid.Expand();
                    foreach (TreeNode n in clsid.Nodes)
                    {
                        if (n.Text.ToLower().StartsWith(value))
                        {
                            childNode = n;
                            n.Expand();
                            break;
                        }
                    }
                }

                if (childNode == null)
                {
                    // or inside global classes definitions: HKEY_CLASSES_ROOT\CLSID
                    classesRoot.Expand();
                    clsid = classesRoot.Nodes["CLSID"];

                    if (clsid != null)
                    {
                        clsid.Expand();
                        foreach (TreeNode n in clsid.Nodes)
                        {
                            if (n.Text.ToLower().StartsWith(value))
                            {
                                childNode = n;
                                n.Expand();
                                break;
                            }
                        }
                    }
                }
            }

            if (childNode != null)
                treeView.SelectedNode = childNode;
            
            // keep it always on the screen:
            if (treeView.SelectedNode != null)
            {
                treeView.SelectedNode.EnsureVisible();
                ActiveControl = treeView;
            }
        }

        private void listView_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            if (e.CancelEdit)
            {
                bool close;
                RegistryKey key = GetKey(treeView.SelectedNode, true, out close);
                bool process = true;
                ListViewItem item = listView.Items[e.Item];

                if (key != null && e.Label != item.Name)
                {
                    try
                    {
                        try
                        {
                            if (key.GetValueKind(e.Label) != RegistryValueKind.Unknown)
                            {
                                process =
                                    (MessageBox.Show(
                                         string.Format(SharedStrings.RegistryView_ReplaceConfirm, e.Label),
                                         DialogTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                                         MessageBoxDefaultButton.Button2) == DialogResult.Yes);
                                if (process)
                                    key.DeleteValue(e.Label);
                            }
                        }
                        catch (Exception ex)
                        {
                            Trace.Write(ex.Message);
                        }

                        // set new value:
                        if (process)
                        {
                            key.SetValue(e.Label, key.GetValue(item.Name), key.GetValueKind(item.Name));
                            key.DeleteValue(item.Name);
                            key.Flush();

                            PopulateListView(key);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(SharedStrings.RegistryView_UnableToChangeName + Environment.NewLine + ex.Message,
                                        DialogTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                e.CancelEdit = true;
                if (close && key != null)
                    key.Close();
            }
        }

        private void treeView_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                treeView.SelectedNode = treeView.GetNodeAt(e.Location);
        }

        private void newKeyToolMenuTree_Click(object sender, EventArgs e)
        {
            RegistryNewKeyForm dlg = new RegistryNewKeyForm(SharedStrings.RegistryView_TitleAddKey);

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                // insert new key into registry:
                if (AddNewKey(dlg.RegistryKeyName, treeView.SelectedNode))
                {
                    // refresh the current tree-view:
                    treeView.SelectedNode.Collapse();

                    textCurrentPath.Text += "\\" + dlg.RegistryKeyName;
                    textCurrentPath_Leave(null, EventArgs.Empty);
                }
            }
        }

        private void deleteToolStripMenuTree_Click(object sender, EventArgs e)
        {
            if (treeView.SelectedNode != null && treeView.SelectedNode.Parent != null)
            {
                TreeNode parent = treeView.SelectedNode.Parent;

                if (DeleteKey(parent, treeView.SelectedNode.Text, false))
                {
                    // refresh the current tree-view:
                    parent.Collapse();
                    parent.Expand();
                }
            }
        }

        private void newBinaryDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dlgNewValue == null)
                dlgNewValue = new RegistryNewValueForm();

            // initialize the dialog:
            dlgNewValue.SetUI(SharedStrings.RegistryView_TitleAddBinary, string.Empty, string.Empty, RegistryValueKind.DWord, RegistryValueKind.Binary, RegistryValueKind.DWord, RegistryValueKind.QWord);

            // add value:
            if (dlgNewValue.ShowDialog() == DialogResult.OK)
                AddNewValue(treeView.SelectedNode, dlgNewValue.RegistryValueName, dlgNewValue.RegistryValueType, dlgNewValue.RegistryValue, true);
        }

        private void newStringDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dlgNewValue == null)
                dlgNewValue = new RegistryNewValueForm();

            // initialize the dialog:
            dlgNewValue.SetUI(SharedStrings.RegistryView_TitleAddString, string.Empty, string.Empty, RegistryValueKind.String, RegistryValueKind.ExpandString, RegistryValueKind.MultiString, RegistryValueKind.String);

            // add value:
            if (dlgNewValue.ShowDialog() == DialogResult.OK)
                AddNewValue(treeView.SelectedNode, dlgNewValue.RegistryValueName, dlgNewValue.RegistryValueType, dlgNewValue.RegistryValue, true);
        }

        private void editToolStripMenu_Click(object sender, EventArgs e)
        {
            RegistryKey key;

            if (listView.SelectedItems != null && listView.SelectedItems.Count > 0)
            {
                if (dlgNewValue == null)
                    dlgNewValue = new RegistryNewValueForm();

                bool close;
                key = GetKey(treeView.SelectedNode, true, out close);
                string valueName = listView.SelectedItems[0].Name;

                if (close)
                {
                    // initialize the dialog:
                    dlgNewValue.SetUI(SharedStrings.RegistryView_TitleEdit, valueName,
                                      key.GetValue(valueName, null, RegistryValueOptions.DoNotExpandEnvironmentNames),
                                      key.GetValueKind(valueName),
                                      RegistryValueKind.Binary, RegistryValueKind.DWord,
                                      RegistryValueKind.ExpandString, RegistryValueKind.MultiString,
                                      RegistryValueKind.QWord, RegistryValueKind.String);

                    // add value:
                    if (dlgNewValue.ShowDialog() == DialogResult.OK)
                    {
                        if (dlgNewValue.RegistryValueName != valueName)
                            key.DeleteValue(valueName);

                        AddNewValue(treeView.SelectedNode, dlgNewValue.RegistryValueName, dlgNewValue.RegistryValueType,
                                    dlgNewValue.RegistryValue, true);
                    }

                    key.Close();
                }
            }
        }

        private void copyNameToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (listView.SelectedItems != null && listView.SelectedItems.Count > 0)
                Clipboard.SetText(listView.SelectedItems[0].Name);
        }

        private void copyvalueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView.SelectedItems != null && listView.SelectedItems.Count > 0)
                Clipboard.SetText(listView.SelectedItems[0].SubItems[2].Text);
        }

        private void deleteToolStripMenu_Click(object sender, EventArgs e)
        {
            DeleteKeyValue(treeView.SelectedNode, listView.SelectedItems[0].Text, listView.SelectedItems[0].Name, false, true);
        }

        private void renameToolStripMenu_Click(object sender, EventArgs e)
        {
            if (listView.SelectedItems != null && listView.SelectedItems.Count > 0)
                listView.SelectedItems[0].BeginEdit();
        }

        private void toolStripFind_Click(object sender, EventArgs e)
        {
            if (dlgFind == null)
                dlgFind = new RegistryFindForm();
            else
                dlgFind.SetUI();

            // search as user chose:
            if (dlgFind.ShowDialog() == DialogResult.OK)
            {
                dlgFind.AddHistory(dlgFind.SearchText);
                dlgFind.IsConfirmed = true;
                toolStripSearchAgain_Click(sender, e);
            }
        }

        private void toolStripSearchAgain_Click(object sender, EventArgs e)
        {
            if (dlgFind == null || !dlgFind.IsConfirmed)
                toolStripFind_Click(sender, e);
            else
            {
                if (dlgFind.SearchKeys || dlgFind.SearchValues)
                    Search(treeView.SelectedNode,
                           (listView.SelectedIndices == null || listView.SelectedIndices.Count == 0
                                ? -1
                                : listView.SelectedIndices[0]), StringHelper.CreateStarFilter(dlgFind.SearchText),
                           dlgFind.SearchKeys, dlgFind.SearchValues, dlgFind.SearchContent);
                else
                    MessageBox.Show(SharedStrings.RegistryView_SelectOptionToSearch, DialogTitle, MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
            }
        }

        private void Search(TreeNode startNode, int listIndex, StringHelper.IStringFilter filter, bool keys, bool values, bool content)
        {
            treeView.BeginUpdate();

            if (keys)
            {
                do
                {
                    // search by key name:
                    startNode.Expand();

                    // search by value:
                    if (values)
                    {
                        bool close;
                        RegistryKey key = GetKey(startNode, false, out close);
                        string[] keyValues = (key != null ? key.GetValueNames() : null);
                        string foundName = null;

                        if (listIndex >= 0 && keyValues != null && keyValues.Length > 0)
                        {
                            if (listIndex >= keyValues.Length)
                                keyValues = null;
                            else
                            {
                                // remove given number of first elements:
                                Array.Sort(keyValues);
                                string[] newValues = new string[keyValues.Length - listIndex - 1];
                                for (int i = 0, j = listIndex + 1; i < newValues.Length; i++, j++)
                                    newValues[i] = keyValues[j];
                                listIndex = -1;
                                keyValues = newValues;
                            }
                        }

                        if (keyValues != null && keyValues.Length > 0)
                        {
                            foreach (string v in keyValues)
                            {
                                if (filter.Match(v))
                                {
                                    // found active item:
                                    foundName = GetSafeValueName(v);
                                    break;
                                }
                            }

                            // check if parse also content:
                            if (content && foundName == null)
                            {
                                foreach (string v in keyValues)
                                {
                                    RegistryValueKind kind = key.GetValueKind(v);

                                    if (filter.Match(GetValueString(key, v, kind)))
                                    {
                                        // found active item:
                                        foundName = GetSafeValueName(v);
                                        break;
                                    }
                                }
                            }

                            if (foundName != null)
                            {
                                treeView.SelectedNode = startNode;
                                treeView.EndUpdate();
                                startNode.EnsureVisible();
                                ActiveControl = listView;

                                foreach(ListViewItem i in listView.Items)
                                    if(i.SubItems[0].Text == foundName)
                                    {
                                        i.Selected = true;
                                        i.EnsureVisible();
                                    }
                                return;
                            }
                        }

                        if (close && key != null)
                            key.Close();
                    }

                    startNode = startNode.NextVisibleNode;

                    if (startNode != null && filter.Match(startNode.Text))
                    {
                        treeView.SelectedNode = startNode;
                        startNode.EnsureVisible();
                        ActiveControl = treeView;
                        break;
                    }

                } while (startNode != null);
            }
            else
            {
                // search only among current items in list view:
                if (listIndex < 0)
                    listIndex = 0;
                for (int i = listIndex; i < listView.Items.Count; i++)
                {
                    ListViewItem item = listView.Items[i];
                    ListViewItem.ListViewSubItemCollection subItems = item.SubItems;

                    if ((values && filter.Match(subItems[0].Text))
                        || (content && filter.Match(subItems[2].Text)))
                    {
                        item.Selected = true;
                        item.EnsureVisible();
                        ActiveControl = listView;
                        break;
                    }
                }
            }

            treeView.EndUpdate();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView.SelectedNode != null)
                Clipboard.SetText(treeView.SelectedNode.Text);
        }

        private void toolStripNavBack_Click(object sender, EventArgs e)
        {
            string path;

            if (navigator.GoBack(out path, CurrentNavigationPath))
            {
                NavigateTo(path);
                RefreshNavigationButtons();

                ActiveControl = treeView;
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            string path;

            if (navigator.GoForward(out path, CurrentNavigationPath))
            {
                NavigateTo(path);
                RefreshNavigationButtons();

                ActiveControl = treeView;
            }
        }

        private void treeView_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            StoreCurrentSelection();
        }

        private void collapseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(treeView.SelectedNode != null)
                treeView.SelectedNode.Collapse(false);
        }

        private void expandAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(treeView.SelectedNode != null)
                treeView.SelectedNode.ExpandAll();
        }

        private void addToFavoritesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(dlgAddFavorite == null)
                dlgAddFavorite = new RegistryAddFavorite();

            dlgAddFavorite.Initialize(treeView.SelectedNode.FullPath);
            if(dlgAddFavorite.ShowDialog() == DialogResult.OK)
            {
                favorites.Add(dlgAddFavorite.FavoriteName, dlgAddFavorite.FavoriteLocation);
                RefreshFavoritesMenu();
            }
        }

        private void deleteFromFavoritesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(dlgRemoveFavorite == null)
                dlgRemoveFavorite = new RegistryRemoveFavorite();

            dlgRemoveFavorite.Initialize(favorites);

            if(dlgRemoveFavorite.ShowDialog() == DialogResult.OK)
            {
                favorites.Clear();
                ImportFavorites(dlgRemoveFavorite.Favorites);
                RefreshFavoritesMenu();
            }
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dlgExport == null)
            {
                dlgExport = new SaveFileDialog();
                dlgExport.Title = "Exporting registry";
                dlgExport.Filter = "Registry files|*.reg|All files|*.*";
                dlgExport.DefaultExt = ".reg";
                dlgExport.CheckPathExists = true;
                dlgExport.OverwritePrompt = true;
            }
            dlgExport.FilterIndex = 0;
            dlgExport.FileName = string.Format("export_{0:yyyyMMdd}.reg", DateTime.Now);

            if (dlgExport.ShowDialog() == DialogResult.OK)
            {
                bool close;
                RegistryKey key = GetKey(treeView.SelectedNode, false, out close);

                if (close)
                {
                    string header = "Windows Registry Editor Version 5.00\r\n\r\n";
                    string exportedData = RegistryHelper.ExportRegistry(key);
                    key.Close();

                    try
                    {
                        File.WriteAllText(dlgExport.FileName, header + exportedData);
                    }
                    catch (Exception ex)
                    {
                        Trace.Write(ex.Message);
                    }
                }
            }
        }
    }
}
