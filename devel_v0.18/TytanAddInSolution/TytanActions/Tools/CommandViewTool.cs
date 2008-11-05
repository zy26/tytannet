using System;
using System.Collections.Generic;
using System.Windows.Forms;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.CommandBars;
using Pretorianie.Tytan.Core.CustomAddIn;
using Pretorianie.Tytan.Core.Data;
using Pretorianie.Tytan.Core.Helpers;
using System.Diagnostics;

namespace Pretorianie.Tytan.Tools
{
    /// <summary>
    /// Control that displays commands and toolbars infos.
    /// </summary>
    public partial class CommandViewTool : UserControl
    {
        /// <summary>
        /// Event handler.
        /// </summary>
        public delegate void CommandSelectedHandler(CommandViewTool sender, CommandInfo item);

        /// <summary>
        /// Event raised each time new Command is selected.
        /// </summary>
        public event CommandSelectedHandler CommandSelected;

        private readonly TreeNode commandsTree;
        private readonly TreeNode toolBarsTree;
        private readonly NamedItemTreeCollection<Command> commandCollection;

        private const int IconFolderClosed = 1;
        private const int IconFolderOpen = 2;

        private const int IconEventActive = 3;
        private const int IconEventInactive = IconEventActive;

        private const int IconCommandsActive = 4;
        private const int IconCommandsInactive = IconCommandsActive;

        private const int IconToolbarsActive = 5;
        private const int IconToolbarsInactive = IconToolbarsActive;

        private const string DialogTitle = "Command Preview";

        private DTE2 appObj;
        private CommandInfo currentItem;
        private SaveFileDialog dlgSaveIcon;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public CommandViewTool()
        {
            InitializeComponent();

            commandsTree = new TreeNode("Commands", IconFolderClosed, IconFolderOpen);
            toolBarsTree = new TreeNode("Tool Bars", IconFolderClosed, IconFolderOpen);
            commandCollection = new NamedItemTreeCollection<Command>();

            // store the reference of the created tool:
            CustomAddInManager.LastCreatedPackageTool = this;
        }

        /// <summary>
        /// Appends info about commands and toolbar items to proper parent nodes.
        /// </summary>
        private void GenerateInfo(DTE2 appObject, TreeNode nodeCommands, TreeNode nodeToolBars)
        {
            SortedDictionary<string, TreeNode> subItems = new SortedDictionary<string, TreeNode>();

            appObj = appObject;

            foreach (Command c in appObject.Commands)
            {
                if (!string.IsNullOrEmpty(c.Name))
                    commandCollection.Add(c.Name, c);
            }
            SerializeInfo(commandCollection, nodeCommands, IconFolderClosed, IconFolderOpen);

            // serialize all CommandBarPopup and CommandBarButton infos:
            foreach (CommandBar bar in (CommandBars)appObject.CommandBars)
            {
                if (!string.IsNullOrEmpty(bar.Name))
                {
                    TreeNode node = new TreeNode(bar.Name, IconFolderClosed, IconFolderOpen);
                    node.Tag = bar;

                    // generate info for sub-elements:
                    SerializeCommandBarInfo(bar, node);
                    Add(subItems, node);
                }
            }
            // add sorted element to parent TreeNode:
            foreach (string k in subItems.Keys)
                nodeToolBars.Nodes.Add(subItems[k]);
        }


        private static void SerializeInfo(NamedItemTreeCollection<Command> folderCollection, TreeNode nodeCommands, int inactiveIcon, int activeIcon)
        {
            // enumerate all folders and generate their info:
            foreach (KeyValuePair<string, NamedItemTreeCollection<Command>> f in folderCollection.Folders)
            {
                TreeNode n = new TreeNode(f.Key, inactiveIcon, activeIcon);
                nodeCommands.Nodes.Add(n);

                // add all child elements:
                SerializeInfo(f.Value, n, inactiveIcon, activeIcon);
            }

            // add other elements:
            foreach (KeyValuePair<string, Command> c in folderCollection.Items)
            {
                TreeNode m = new TreeNode(c.Key, IconEventInactive, IconEventActive);
                m.Tag = c.Value;

                nodeCommands.Nodes.Add(m);
            }
        }

        private static void SerializeCommandBarInfo(CommandBar bar, TreeNode parentNode)
        {
            SortedDictionary<string, TreeNode> subItems = new SortedDictionary<string, TreeNode>();
            TreeNode ctrlNode;

            // enumerate all buttons:
            foreach (CommandBarControl c in bar.Controls)
            {
                if (c.Type == MsoControlType.msoControlButton)
                {
                    ctrlNode = new TreeNode(c.Caption, IconToolbarsInactive, IconToolbarsActive);
                    ctrlNode.Tag = c;
                    Add(subItems, ctrlNode);
                }
                else
                {
                    if (c.Type == MsoControlType.msoControlPopup)
                    {
                        CommandBarPopup p = (CommandBarPopup) c;
                        CommandBar x = p.CommandBar;
                        TreeNode node = new TreeNode(p.Caption, IconFolderClosed, IconFolderOpen);
                        node.Tag = x;

                        // generate info for sub-elements:
                        SerializeCommandBarInfo(x, node);
                        if(node.Nodes.Count == 0)
                        {
                            node.ImageIndex = IconToolbarsInactive;
                            node.SelectedImageIndex = IconToolbarsActive;
                        }

                        Add(subItems, node);
                    }
                }
            }

            // add sorted element to parent TreeNode:
            foreach (string k in subItems.Keys)
                parentNode.Nodes.Add(subItems[k]);
        }

        private static TreeNode SerializeInfo(CommandBar commandBar)
        {
            SortedDictionary<string, TreeNode> folders = new SortedDictionary<string, TreeNode>();
            SortedDictionary<string, TreeNode> nodes = new SortedDictionary<string, TreeNode>();

            TreeNode item = new TreeNode(commandBar.Name, IconFolderClosed, IconFolderOpen);
            TreeNode ctrlNode;

            item.Tag = commandBar;

            // enumerate all buttons:
            foreach (CommandBarControl c in commandBar.Controls)
            {
                if (c.Type == MsoControlType.msoControlButton)
                {
                    ctrlNode = new TreeNode(c.Caption, IconToolbarsInactive, IconToolbarsActive);
                    ctrlNode.Tag = c;
                    Add(nodes, ctrlNode);
                }
                else
                {
                    if (c.Type == MsoControlType.msoControlPopup)
                        Add(folders, SerializeInfo(((CommandBarPopup) c).CommandBar));
                }
            }

            // add nodes to given item:
            foreach (TreeNode n in folders.Values)
                item.Nodes.Add(n);
            foreach (TreeNode n in nodes.Values)
                item.Nodes.Add(n);

            return item;
        }

        private static void Add(IDictionary<string, TreeNode> collection, TreeNode item)
        {
            int i = 0;
            string name = item.Text;
            string format = item.Nodes.Count > 0 ? "{0}_{1}" : "{0}__{1}";

            do
            {
                if (i > 0) 
                    name = string.Format(format, item.Text, i);
                i++;
            } while (collection.ContainsKey(name));
            collection.Add(name, item);
        }

        /// <summary>
        /// Generates new data inside the TreeView reflecting commands and toolbars set inside VisualStudio IDE>
        /// </summary>
        public void RefreshInfos (DTE2 appObject)
        {
            try
            {
                info.Nodes.Clear();
                commandsTree.Nodes.Clear();
                toolBarsTree.Nodes.Clear();
                commandCollection.Clear();

                // generate preview:
                GenerateInfo(appObject, commandsTree, toolBarsTree);

                // insert data to the view:
                info.Nodes.AddRange(new TreeNode[] {commandsTree, toolBarsTree});
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);
                Trace.WriteLine(e.StackTrace);
            }
        }

        private void toolStripFeedback_Click(object sender, EventArgs e)
        {
            CallHelper.SendFeedback(this);
        }

        private void info_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node != null && CommandSelected != null)
            {
                if (e.Node.Tag is Command)
                    currentItem = new CommandInfo(e.Node.Tag as Command);
                else if (e.Node.Tag is CommandBarButton)
                    currentItem = new CommandInfo(appObj, e.Node.Tag as CommandBarButton);
                else
                    currentItem = new CommandInfo();

                // fire event:
                CommandSelected(this, currentItem);
            }
            else
            {
                currentItem = null;
            }
        }

        private void toolStripRefresh_Click(object sender, EventArgs e)
        {
            RefreshInfos(appObj);
        }

        private void toolStripSaveImage_Click(object sender, EventArgs e)
        {
            if (currentItem == null || currentItem.Image == null)
            {
                MessageBox.Show(SharedStrings.CommandPreview_NoValidCommandSelected, DialogTitle, MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }

            if (dlgSaveIcon == null)
            {
                dlgSaveIcon = new SaveFileDialog();
                dlgSaveIcon.Title = "Exporting icon";
                dlgSaveIcon.OverwritePrompt = true;
                dlgSaveIcon.DefaultExt = ".png";
                dlgSaveIcon.Filter = "Image files|*.bmp;*.gif;*.jpg;*.png;*.ico;*.cur;*jpeg|All files|*.*";
                dlgSaveIcon.FilterIndex = 0;
            }
            dlgSaveIcon.FileName = null;

            // save icon go specified file:
            if (dlgSaveIcon.ShowDialog() == DialogResult.OK)
                currentItem.Image.Save(dlgSaveIcon.FileName);
        }
    }
}
