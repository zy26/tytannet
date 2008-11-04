namespace Pretorianie.Tytan.Tools
{
    partial class RegistryViewTool
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RegistryViewTool));
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripFeedback = new System.Windows.Forms.ToolStripLabel();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.keyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.binaryDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stringDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripSeparator();
            this.collapseAllToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.expandAllToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripSeparator();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripEdit = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripNavBack = new System.Windows.Forms.ToolStripButton();
            this.toolStripNavForward = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripDropDownFavorites = new System.Windows.Forms.ToolStripDropDownButton();
            this.addToFavoritesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteFromFavoritesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuFavouritesSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripFind = new System.Windows.Forms.ToolStripButton();
            this.toolStripSearchAgain = new System.Windows.Forms.ToolStripButton();
            this.textCurrentPath = new System.Windows.Forms.TextBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.treeView = new System.Windows.Forms.TreeView();
            this.contextMenuTree = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.newKeyToolMenuTree = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuTree = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.collapseAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.expandAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripSeparator();
            this.exportToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.listView = new System.Windows.Forms.ListView();
            this.columnName = new System.Windows.Forms.ColumnHeader();
            this.columnType = new System.Windows.Forms.ColumnHeader();
            this.columnValue = new System.Windows.Forms.ColumnHeader();
            this.contextMenuList = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.editToolStripMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.renameToolStripMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.copyNameToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.copyvalueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.newkeyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newBinaryDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newStringDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.jumpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.contextMenuTree.SuspendLayout();
            this.contextMenuList.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip
            // 
            this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripFeedback,
            this.toolStripDropDownButton1,
            this.toolStripDelete,
            this.toolStripSeparator1,
            this.toolStripEdit,
            this.toolStripSeparator2,
            this.toolStripNavBack,
            this.toolStripNavForward,
            this.toolStripSeparator3,
            this.toolStripDropDownFavorites,
            this.toolStripFind,
            this.toolStripSearchAgain});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(745, 25);
            this.toolStrip.TabIndex = 0;
            // 
            // toolStripFeedback
            // 
            this.toolStripFeedback.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripFeedback.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripFeedback.IsLink = true;
            this.toolStripFeedback.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.toolStripFeedback.Name = "toolStripFeedback";
            this.toolStripFeedback.Size = new System.Drawing.Size(78, 22);
            this.toolStripFeedback.Text = "Send feedback";
            this.toolStripFeedback.Click += new System.EventHandler(this.toolStripFeedback_Click);
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.keyToolStripMenuItem,
            this.toolStripMenuItem1,
            this.binaryDataToolStripMenuItem,
            this.stringDataToolStripMenuItem,
            this.toolStripMenuItem6,
            this.collapseAllToolStripMenuItem1,
            this.expandAllToolStripMenuItem1,
            this.toolStripMenuItem7,
            this.exportToolStripMenuItem});
            this.toolStripDropDownButton1.Image = global::Pretorianie.Tytan.SharedIcons.FolderAdd;
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(29, 22);
            this.toolStripDropDownButton1.Text = "Add new item";
            // 
            // keyToolStripMenuItem
            // 
            this.keyToolStripMenuItem.Image = global::Pretorianie.Tytan.SharedIcons.FolderKey;
            this.keyToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.keyToolStripMenuItem.Name = "keyToolStripMenuItem";
            this.keyToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.keyToolStripMenuItem.Text = "&Key...";
            this.keyToolStripMenuItem.ToolTipText = "Add new key into the tree";
            this.keyToolStripMenuItem.Click += new System.EventHandler(this.newKeyToolMenuTree_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(149, 6);
            // 
            // binaryDataToolStripMenuItem
            // 
            this.binaryDataToolStripMenuItem.Image = global::Pretorianie.Tytan.SharedIcons.RegistryBinary;
            this.binaryDataToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.binaryDataToolStripMenuItem.Name = "binaryDataToolStripMenuItem";
            this.binaryDataToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.binaryDataToolStripMenuItem.Text = "&Binary data...";
            this.binaryDataToolStripMenuItem.ToolTipText = "Add new binary data value";
            this.binaryDataToolStripMenuItem.Click += new System.EventHandler(this.newBinaryDataToolStripMenuItem_Click);
            // 
            // stringDataToolStripMenuItem
            // 
            this.stringDataToolStripMenuItem.Image = global::Pretorianie.Tytan.SharedIcons.RegistryString;
            this.stringDataToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.stringDataToolStripMenuItem.Name = "stringDataToolStripMenuItem";
            this.stringDataToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.stringDataToolStripMenuItem.Text = "&String data...";
            this.stringDataToolStripMenuItem.ToolTipText = "Add new string data value";
            this.stringDataToolStripMenuItem.Click += new System.EventHandler(this.newStringDataToolStripMenuItem_Click);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(149, 6);
            // 
            // collapseAllToolStripMenuItem1
            // 
            this.collapseAllToolStripMenuItem1.Name = "collapseAllToolStripMenuItem1";
            this.collapseAllToolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
            this.collapseAllToolStripMenuItem1.Text = "Collapse all";
            this.collapseAllToolStripMenuItem1.Click += new System.EventHandler(this.collapseAllToolStripMenuItem_Click);
            // 
            // expandAllToolStripMenuItem1
            // 
            this.expandAllToolStripMenuItem1.Name = "expandAllToolStripMenuItem1";
            this.expandAllToolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
            this.expandAllToolStripMenuItem1.Text = "Expand all";
            this.expandAllToolStripMenuItem1.Click += new System.EventHandler(this.expandAllToolStripMenuItem_Click);
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(149, 6);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Image = global::Pretorianie.Tytan.SharedIcons.RegistryExport;
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.exportToolStripMenuItem.Text = "Export...";
            this.exportToolStripMenuItem.Click += new System.EventHandler(this.exportToolStripMenuItem_Click);
            // 
            // toolStripDelete
            // 
            this.toolStripDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDelete.Image = global::Pretorianie.Tytan.SharedIcons.Cross;
            this.toolStripDelete.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.toolStripDelete.Name = "toolStripDelete";
            this.toolStripDelete.Size = new System.Drawing.Size(23, 22);
            this.toolStripDelete.Text = "Delete selected key or value";
            this.toolStripDelete.Click += new System.EventHandler(this.deleteToolStripMenuTree_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripEdit
            // 
            this.toolStripEdit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripEdit.Image = global::Pretorianie.Tytan.SharedIcons.Edit;
            this.toolStripEdit.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.toolStripEdit.Name = "toolStripEdit";
            this.toolStripEdit.Size = new System.Drawing.Size(23, 22);
            this.toolStripEdit.Text = "toolStripButton2";
            this.toolStripEdit.ToolTipText = "Edit value...";
            this.toolStripEdit.Click += new System.EventHandler(this.editToolStripMenu_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripNavBack
            // 
            this.toolStripNavBack.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripNavBack.Image = global::Pretorianie.Tytan.SharedIcons.NavigationBack;
            this.toolStripNavBack.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripNavBack.Name = "toolStripNavBack";
            this.toolStripNavBack.Size = new System.Drawing.Size(23, 22);
            this.toolStripNavBack.ToolTipText = "Navigate back...";
            this.toolStripNavBack.Click += new System.EventHandler(this.toolStripNavBack_Click);
            // 
            // toolStripNavForward
            // 
            this.toolStripNavForward.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripNavForward.Image = global::Pretorianie.Tytan.SharedIcons.NavigationForward;
            this.toolStripNavForward.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripNavForward.Name = "toolStripNavForward";
            this.toolStripNavForward.Size = new System.Drawing.Size(23, 22);
            this.toolStripNavForward.ToolTipText = "Navigate forward...";
            this.toolStripNavForward.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripDropDownFavorites
            // 
            this.toolStripDropDownFavorites.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDropDownFavorites.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addToFavoritesToolStripMenuItem,
            this.deleteFromFavoritesToolStripMenuItem,
            this.toolStripMenuFavouritesSeparator});
            this.toolStripDropDownFavorites.Image = global::Pretorianie.Tytan.SharedIcons.FolderStar;
            this.toolStripDropDownFavorites.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownFavorites.Name = "toolStripDropDownFavorites";
            this.toolStripDropDownFavorites.Size = new System.Drawing.Size(29, 22);
            this.toolStripDropDownFavorites.ToolTipText = "Favorites...";
            // 
            // addToFavoritesToolStripMenuItem
            // 
            this.addToFavoritesToolStripMenuItem.Image = global::Pretorianie.Tytan.SharedIcons.Plus;
            this.addToFavoritesToolStripMenuItem.Name = "addToFavoritesToolStripMenuItem";
            this.addToFavoritesToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.addToFavoritesToolStripMenuItem.Text = "Add to favorites...";
            this.addToFavoritesToolStripMenuItem.Click += new System.EventHandler(this.addToFavoritesToolStripMenuItem_Click);
            // 
            // deleteFromFavoritesToolStripMenuItem
            // 
            this.deleteFromFavoritesToolStripMenuItem.Image = global::Pretorianie.Tytan.SharedIcons.Minus;
            this.deleteFromFavoritesToolStripMenuItem.Name = "deleteFromFavoritesToolStripMenuItem";
            this.deleteFromFavoritesToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.deleteFromFavoritesToolStripMenuItem.Text = "Delete from favorites...";
            this.deleteFromFavoritesToolStripMenuItem.Click += new System.EventHandler(this.deleteFromFavoritesToolStripMenuItem_Click);
            // 
            // toolStripMenuFavouritesSeparator
            // 
            this.toolStripMenuFavouritesSeparator.Name = "toolStripMenuFavouritesSeparator";
            this.toolStripMenuFavouritesSeparator.Size = new System.Drawing.Size(196, 6);
            this.toolStripMenuFavouritesSeparator.Visible = false;
            // 
            // toolStripFind
            // 
            this.toolStripFind.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripFind.Image = global::Pretorianie.Tytan.SharedIcons.FolderFind;
            this.toolStripFind.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.toolStripFind.Name = "toolStripFind";
            this.toolStripFind.Size = new System.Drawing.Size(23, 22);
            this.toolStripFind.Text = "Find...";
            this.toolStripFind.Click += new System.EventHandler(this.toolStripFind_Click);
            // 
            // toolStripSearchAgain
            // 
            this.toolStripSearchAgain.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripSearchAgain.Image = global::Pretorianie.Tytan.SharedIcons.FolderFindNext;
            this.toolStripSearchAgain.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSearchAgain.Name = "toolStripSearchAgain";
            this.toolStripSearchAgain.Size = new System.Drawing.Size(23, 22);
            this.toolStripSearchAgain.ToolTipText = "Find next...";
            this.toolStripSearchAgain.Click += new System.EventHandler(this.toolStripSearchAgain_Click);
            // 
            // textCurrentPath
            // 
            this.textCurrentPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textCurrentPath.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textCurrentPath.Location = new System.Drawing.Point(3, 454);
            this.textCurrentPath.Name = "textCurrentPath";
            this.textCurrentPath.Size = new System.Drawing.Size(739, 22);
            this.textCurrentPath.TabIndex = 2;
            this.textCurrentPath.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textCurrentPath_KeyDown);
            this.textCurrentPath.Leave += new System.EventHandler(this.textCurrentPath_Leave);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(3, 28);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeView);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.listView);
            this.splitContainer1.Size = new System.Drawing.Size(739, 420);
            this.splitContainer1.SplitterDistance = 246;
            this.splitContainer1.TabIndex = 2;
            // 
            // treeView
            // 
            this.treeView.ContextMenuStrip = this.contextMenuTree;
            this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView.FullRowSelect = true;
            this.treeView.HideSelection = false;
            this.treeView.ImageIndex = 0;
            this.treeView.ImageList = this.imageList;
            this.treeView.Location = new System.Drawing.Point(0, 0);
            this.treeView.Name = "treeView";
            this.treeView.SelectedImageIndex = 1;
            this.treeView.Size = new System.Drawing.Size(246, 420);
            this.treeView.TabIndex = 0;
            this.treeView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.treeView_MouseClick);
            this.treeView.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeView_BeforeExpand);
            this.treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterSelect);
            this.treeView.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeView_BeforeSelect);
            // 
            // contextMenuTree
            // 
            this.contextMenuTree.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newKeyToolMenuTree,
            this.toolStripMenuItem2,
            this.copyToolStripMenuItem,
            this.deleteToolStripMenuTree,
            this.toolStripMenuItem5,
            this.collapseAllToolStripMenuItem,
            this.expandAllToolStripMenuItem,
            this.toolStripMenuItem8,
            this.exportToolStripMenuItem1});
            this.contextMenuTree.Name = "contextMenuTree";
            this.contextMenuTree.Size = new System.Drawing.Size(139, 154);
            // 
            // newKeyToolMenuTree
            // 
            this.newKeyToolMenuTree.Image = global::Pretorianie.Tytan.SharedIcons.FolderKey;
            this.newKeyToolMenuTree.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.newKeyToolMenuTree.Name = "newKeyToolMenuTree";
            this.newKeyToolMenuTree.Size = new System.Drawing.Size(138, 22);
            this.newKeyToolMenuTree.Text = "&New key...";
            this.newKeyToolMenuTree.Click += new System.EventHandler(this.newKeyToolMenuTree_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(135, 6);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Image = global::Pretorianie.Tytan.SharedIcons.Copy;
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.copyToolStripMenuItem.Text = "&Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuTree
            // 
            this.deleteToolStripMenuTree.Image = global::Pretorianie.Tytan.SharedIcons.Cross;
            this.deleteToolStripMenuTree.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.deleteToolStripMenuTree.Name = "deleteToolStripMenuTree";
            this.deleteToolStripMenuTree.Size = new System.Drawing.Size(138, 22);
            this.deleteToolStripMenuTree.Text = "&Delete";
            this.deleteToolStripMenuTree.Click += new System.EventHandler(this.deleteToolStripMenuTree_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(135, 6);
            // 
            // collapseAllToolStripMenuItem
            // 
            this.collapseAllToolStripMenuItem.Name = "collapseAllToolStripMenuItem";
            this.collapseAllToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.collapseAllToolStripMenuItem.Text = "Collapse all";
            this.collapseAllToolStripMenuItem.Click += new System.EventHandler(this.collapseAllToolStripMenuItem_Click);
            // 
            // expandAllToolStripMenuItem
            // 
            this.expandAllToolStripMenuItem.Name = "expandAllToolStripMenuItem";
            this.expandAllToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.expandAllToolStripMenuItem.Text = "Expand all";
            this.expandAllToolStripMenuItem.Click += new System.EventHandler(this.expandAllToolStripMenuItem_Click);
            // 
            // toolStripMenuItem8
            // 
            this.toolStripMenuItem8.Name = "toolStripMenuItem8";
            this.toolStripMenuItem8.Size = new System.Drawing.Size(135, 6);
            // 
            // exportToolStripMenuItem1
            // 
            this.exportToolStripMenuItem1.Image = global::Pretorianie.Tytan.SharedIcons.RegistryExport;
            this.exportToolStripMenuItem1.Name = "exportToolStripMenuItem1";
            this.exportToolStripMenuItem1.Size = new System.Drawing.Size(138, 22);
            this.exportToolStripMenuItem1.Text = "Export...";
            this.exportToolStripMenuItem1.Click += new System.EventHandler(this.exportToolStripMenuItem_Click);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "folder.png");
            this.imageList.Images.SetKeyName(1, "folder_go.png");
            this.imageList.Images.SetKeyName(2, "50.png");
            this.imageList.Images.SetKeyName(3, "RegistryBinary.png");
            this.imageList.Images.SetKeyName(4, "RegistryString.png");
            // 
            // listView
            // 
            this.listView.AllowColumnReorder = true;
            this.listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnName,
            this.columnType,
            this.columnValue});
            this.listView.ContextMenuStrip = this.contextMenuList;
            this.listView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView.FullRowSelect = true;
            this.listView.GridLines = true;
            this.listView.HideSelection = false;
            this.listView.LabelEdit = true;
            this.listView.Location = new System.Drawing.Point(0, 0);
            this.listView.MultiSelect = false;
            this.listView.Name = "listView";
            this.listView.Size = new System.Drawing.Size(489, 420);
            this.listView.SmallImageList = this.imageList;
            this.listView.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listView.TabIndex = 0;
            this.listView.UseCompatibleStateImageBehavior = false;
            this.listView.View = System.Windows.Forms.View.Details;
            this.listView.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.listView_AfterLabelEdit);
            this.listView.SelectedIndexChanged += new System.EventHandler(this.listView_SelectedIndexChanged);
            this.listView.DoubleClick += new System.EventHandler(this.editToolStripMenu_Click);
            // 
            // columnName
            // 
            this.columnName.Text = "Name";
            this.columnName.Width = 190;
            // 
            // columnType
            // 
            this.columnType.Text = "Type";
            this.columnType.Width = 70;
            // 
            // columnValue
            // 
            this.columnValue.Text = "Value";
            this.columnValue.Width = 250;
            // 
            // contextMenuList
            // 
            this.contextMenuList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editToolStripMenu,
            this.renameToolStripMenu,
            this.copyNameToolStripMenuItem1,
            this.copyvalueToolStripMenuItem,
            this.deleteToolStripMenu,
            this.toolStripMenuItem4,
            this.newkeyToolStripMenuItem,
            this.newBinaryDataToolStripMenuItem,
            this.newStringDataToolStripMenuItem,
            this.toolStripMenuItem3,
            this.jumpToolStripMenuItem});
            this.contextMenuList.Name = "contextMenuList";
            this.contextMenuList.Size = new System.Drawing.Size(177, 214);
            // 
            // editToolStripMenu
            // 
            this.editToolStripMenu.Image = global::Pretorianie.Tytan.SharedIcons.Edit;
            this.editToolStripMenu.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.editToolStripMenu.Name = "editToolStripMenu";
            this.editToolStripMenu.Size = new System.Drawing.Size(176, 22);
            this.editToolStripMenu.Text = "&Edit...";
            this.editToolStripMenu.Click += new System.EventHandler(this.editToolStripMenu_Click);
            // 
            // renameToolStripMenu
            // 
            this.renameToolStripMenu.Name = "renameToolStripMenu";
            this.renameToolStripMenu.Size = new System.Drawing.Size(176, 22);
            this.renameToolStripMenu.Text = "&Rename";
            this.renameToolStripMenu.Click += new System.EventHandler(this.renameToolStripMenu_Click);
            // 
            // copyNameToolStripMenuItem1
            // 
            this.copyNameToolStripMenuItem1.Image = ((System.Drawing.Image)(resources.GetObject("copyNameToolStripMenuItem1.Image")));
            this.copyNameToolStripMenuItem1.Name = "copyNameToolStripMenuItem1";
            this.copyNameToolStripMenuItem1.Size = new System.Drawing.Size(176, 22);
            this.copyNameToolStripMenuItem1.Text = "&Copy name";
            this.copyNameToolStripMenuItem1.Click += new System.EventHandler(this.copyNameToolStripMenuItem1_Click);
            // 
            // copyvalueToolStripMenuItem
            // 
            this.copyvalueToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("copyvalueToolStripMenuItem.Image")));
            this.copyvalueToolStripMenuItem.Name = "copyvalueToolStripMenuItem";
            this.copyvalueToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.copyvalueToolStripMenuItem.Text = "Copy &value";
            this.copyvalueToolStripMenuItem.Click += new System.EventHandler(this.copyvalueToolStripMenuItem_Click);
            // 
            // deleteToolStripMenu
            // 
            this.deleteToolStripMenu.Image = global::Pretorianie.Tytan.SharedIcons.Cross;
            this.deleteToolStripMenu.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.deleteToolStripMenu.Name = "deleteToolStripMenu";
            this.deleteToolStripMenu.Size = new System.Drawing.Size(176, 22);
            this.deleteToolStripMenu.Text = "&Delete";
            this.deleteToolStripMenu.Click += new System.EventHandler(this.deleteToolStripMenu_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(173, 6);
            // 
            // newkeyToolStripMenuItem
            // 
            this.newkeyToolStripMenuItem.Image = global::Pretorianie.Tytan.SharedIcons.FolderKey;
            this.newkeyToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Black;
            this.newkeyToolStripMenuItem.Name = "newkeyToolStripMenuItem";
            this.newkeyToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.newkeyToolStripMenuItem.Text = "New &key...";
            this.newkeyToolStripMenuItem.Click += new System.EventHandler(this.newKeyToolMenuTree_Click);
            // 
            // newBinaryDataToolStripMenuItem
            // 
            this.newBinaryDataToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("newBinaryDataToolStripMenuItem.Image")));
            this.newBinaryDataToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.newBinaryDataToolStripMenuItem.Name = "newBinaryDataToolStripMenuItem";
            this.newBinaryDataToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.newBinaryDataToolStripMenuItem.Text = "New &binary data...";
            this.newBinaryDataToolStripMenuItem.Click += new System.EventHandler(this.newBinaryDataToolStripMenuItem_Click);
            // 
            // newStringDataToolStripMenuItem
            // 
            this.newStringDataToolStripMenuItem.Image = global::Pretorianie.Tytan.SharedIcons.RegistryString;
            this.newStringDataToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.newStringDataToolStripMenuItem.Name = "newStringDataToolStripMenuItem";
            this.newStringDataToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.newStringDataToolStripMenuItem.Text = "New &string data...";
            this.newStringDataToolStripMenuItem.Click += new System.EventHandler(this.newStringDataToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(173, 6);
            // 
            // jumpToolStripMenuItem
            // 
            this.jumpToolStripMenuItem.Image = global::Pretorianie.Tytan.SharedIcons.RegistryJump;
            this.jumpToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.jumpToolStripMenuItem.Name = "jumpToolStripMenuItem";
            this.jumpToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.jumpToolStripMenuItem.Text = "&Navigate to key";
            this.jumpToolStripMenuItem.Click += new System.EventHandler(this.jumpToolStripMenuItem_Click);
            // 
            // RegistryViewTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.textCurrentPath);
            this.Controls.Add(this.toolStrip);
            this.Name = "RegistryViewTool";
            this.Size = new System.Drawing.Size(745, 477);
            this.Load += new System.EventHandler(this.RegistryViewTool_Load);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.contextMenuTree.ResumeLayout(false);
            this.contextMenuList.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.TextBox textCurrentPath;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView treeView;
        private System.Windows.Forms.ListView listView;
        private System.Windows.Forms.ColumnHeader columnName;
        private System.Windows.Forms.ColumnHeader columnValue;
        private System.Windows.Forms.ColumnHeader columnType;
        private System.Windows.Forms.ToolStripLabel toolStripFeedback;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.ContextMenuStrip contextMenuList;
        private System.Windows.Forms.ToolStripMenuItem jumpToolStripMenuItem;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripButton toolStripDelete;
        private System.Windows.Forms.ToolStripMenuItem keyToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem binaryDataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stringDataToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripEdit;
        private System.Windows.Forms.ContextMenuStrip contextMenuTree;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuTree;
        private System.Windows.Forms.ToolStripMenuItem newKeyToolMenuTree;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem newBinaryDataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newStringDataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenu;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem renameToolStripMenu;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenu;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolStripFind;
        private System.Windows.Forms.ToolStripMenuItem newkeyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyNameToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem copyvalueToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton toolStripNavBack;
        private System.Windows.Forms.ToolStripButton toolStripNavForward;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem collapseAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem expandAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownFavorites;
        private System.Windows.Forms.ToolStripMenuItem addToFavoritesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteFromFavoritesToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuFavouritesSeparator;
        private System.Windows.Forms.ToolStripButton toolStripSearchAgain;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem collapseAllToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem expandAllToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem7;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem8;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem1;
    }
}
