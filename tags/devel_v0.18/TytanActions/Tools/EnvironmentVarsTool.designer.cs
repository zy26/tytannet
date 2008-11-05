namespace Pretorianie.Tytan.Tools
{
    partial class EnvironmentVarsTool
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
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripFeedback = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripViewType = new System.Windows.Forms.ToolStripComboBox();
            this.list = new System.Windows.Forms.ListView();
            this.columnName = new System.Windows.Forms.ColumnHeader();
            this.columnValue = new System.Windows.Forms.ColumnHeader();
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripAdd = new System.Windows.Forms.ToolStripButton();
            this.toolStripDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripEdit = new System.Windows.Forms.ToolStripButton();
            this.toolStripCmd = new System.Windows.Forms.ToolStripButton();
            this.toolStrip.SuspendLayout();
            this.contextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip
            // 
            this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripFeedback,
            this.toolStripAdd,
            this.toolStripDelete,
            this.toolStripEdit,
            this.toolStripSeparator1,
            this.toolStripCmd,
            this.toolStripSeparator2,
            this.toolStripLabel1,
            this.toolStripViewType});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(792, 25);
            this.toolStrip.TabIndex = 1;
            this.toolStrip.Text = "toolStrip";
            // 
            // toolStripFeedback
            // 
            this.toolStripFeedback.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripFeedback.IsLink = true;
            this.toolStripFeedback.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.toolStripFeedback.Name = "toolStripFeedback";
            this.toolStripFeedback.Size = new System.Drawing.Size(78, 22);
            this.toolStripFeedback.Text = "Send feedback";
            this.toolStripFeedback.Click += new System.EventHandler(this.toolStripFeedback_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(33, 22);
            this.toolStripLabel1.Text = "View:";
            // 
            // toolStripViewType
            // 
            this.toolStripViewType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolStripViewType.DropDownWidth = 150;
            this.toolStripViewType.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.toolStripViewType.Items.AddRange(new object[] {
            "host",
            "user",
            "process"});
            this.toolStripViewType.Name = "toolStripViewType";
            this.toolStripViewType.Size = new System.Drawing.Size(121, 25);
            this.toolStripViewType.SelectedIndexChanged += new System.EventHandler(this.toolStripViewType_SelectedIndexChanged);
            // 
            // list
            // 
            this.list.AllowColumnReorder = true;
            this.list.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnName,
            this.columnValue});
            this.list.ContextMenuStrip = this.contextMenu;
            this.list.Dock = System.Windows.Forms.DockStyle.Fill;
            this.list.FullRowSelect = true;
            this.list.GridLines = true;
            this.list.HideSelection = false;
            this.list.Location = new System.Drawing.Point(0, 25);
            this.list.MultiSelect = false;
            this.list.Name = "list";
            this.list.Size = new System.Drawing.Size(792, 453);
            this.list.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.list.TabIndex = 2;
            this.list.UseCompatibleStateImageBehavior = false;
            this.list.View = System.Windows.Forms.View.Details;
            this.list.SelectedIndexChanged += new System.EventHandler(this.list_SelectedIndexChanged);
            this.list.DoubleClick += new System.EventHandler(this.toolStripEdit_Click);
            // 
            // columnName
            // 
            this.columnName.Width = 200;
            // 
            // columnValue
            // 
            this.columnValue.Width = 300;
            // 
            // contextMenu
            // 
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addToolStripMenuItem,
            this.editToolStripMenuItem,
            this.toolStripMenuItem1,
            this.deleteToolStripMenuItem});
            this.contextMenu.Name = "contextMenu";
            this.contextMenu.Size = new System.Drawing.Size(153, 98);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(113, 6);
            // 
            // addToolStripMenuItem
            // 
            this.addToolStripMenuItem.Image = global::Pretorianie.Tytan.SharedIcons.Plus;
            this.addToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.addToolStripMenuItem.Name = "addToolStripMenuItem";
            this.addToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.addToolStripMenuItem.Text = "Add...";
            this.addToolStripMenuItem.Click += new System.EventHandler(this.toolStripAdd_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Image = global::Pretorianie.Tytan.SharedIcons.Edit;
            this.editToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.editToolStripMenuItem.Text = "Edit...";
            this.editToolStripMenuItem.Click += new System.EventHandler(this.toolStripEdit_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Image = global::Pretorianie.Tytan.SharedIcons.Minus;
            this.deleteToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.toolStripDelete_Click);
            // 
            // toolStripAdd
            // 
            this.toolStripAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripAdd.Image = global::Pretorianie.Tytan.SharedIcons.Plus;
            this.toolStripAdd.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.toolStripAdd.Name = "toolStripAdd";
            this.toolStripAdd.Size = new System.Drawing.Size(23, 22);
            this.toolStripAdd.ToolTipText = "Add new variable...";
            this.toolStripAdd.Click += new System.EventHandler(this.toolStripAdd_Click);
            // 
            // toolStripDelete
            // 
            this.toolStripDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDelete.Enabled = false;
            this.toolStripDelete.Image = global::Pretorianie.Tytan.SharedIcons.Minus;
            this.toolStripDelete.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.toolStripDelete.Name = "toolStripDelete";
            this.toolStripDelete.Size = new System.Drawing.Size(23, 22);
            this.toolStripDelete.ToolTipText = "Delete selected variable";
            this.toolStripDelete.Click += new System.EventHandler(this.toolStripDelete_Click);
            // 
            // toolStripEdit
            // 
            this.toolStripEdit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripEdit.Enabled = false;
            this.toolStripEdit.Image = global::Pretorianie.Tytan.SharedIcons.Edit;
            this.toolStripEdit.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.toolStripEdit.Name = "toolStripEdit";
            this.toolStripEdit.Size = new System.Drawing.Size(23, 22);
            this.toolStripEdit.ToolTipText = "Edit selected variable";
            this.toolStripEdit.Click += new System.EventHandler(this.toolStripEdit_Click);
            // 
            // toolStripCmd
            // 
            this.toolStripCmd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripCmd.Image = global::Pretorianie.Tytan.SharedIcons.CommandConsole;
            this.toolStripCmd.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.toolStripCmd.Name = "toolStripCmd";
            this.toolStripCmd.Size = new System.Drawing.Size(23, 22);
            this.toolStripCmd.ToolTipText = "Run CommandConsole window...";
            this.toolStripCmd.Click += new System.EventHandler(this.toolStripCmd_Click);
            // 
            // EnvironmentVarsTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.list);
            this.Controls.Add(this.toolStrip);
            this.DoubleBuffered = true;
            this.Name = "EnvironmentVarsTool";
            this.Size = new System.Drawing.Size(792, 478);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.contextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        protected System.Windows.Forms.ToolStrip toolStrip;
        protected System.Windows.Forms.ToolStripLabel toolStripFeedback;
        private System.Windows.Forms.ListView list;
        private System.Windows.Forms.ColumnHeader columnName;
        private System.Windows.Forms.ColumnHeader columnValue;
        private System.Windows.Forms.ToolStripButton toolStripAdd;
        private System.Windows.Forms.ToolStripButton toolStripDelete;
        private System.Windows.Forms.ToolStripButton toolStripEdit;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripComboBox toolStripViewType;
        private System.Windows.Forms.ToolStripButton toolStripCmd;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.ToolStripMenuItem addToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
    }
}
