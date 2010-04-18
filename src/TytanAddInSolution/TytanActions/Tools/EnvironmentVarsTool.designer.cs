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
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripClearHistory = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripViewType = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripFilter = new System.Windows.Forms.ToolStripComboBox();
            this.list = new System.Windows.Forms.ListView();
            this.columnName = new System.Windows.Forms.ColumnHeader();
            this.columnValue = new System.Windows.Forms.ColumnHeader();
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSaveSession = new System.Windows.Forms.ToolStripButton();
            this.toolStripLoadSession = new System.Windows.Forms.ToolStripButton();
            this.toolStripUndoSession = new System.Windows.Forms.ToolStripButton();
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
            this.toolStripSaveSession,
            this.toolStripLoadSession,
            this.toolStripUndoSession,
            this.toolStripSeparator4,
            this.toolStripAdd,
            this.toolStripDelete,
            this.toolStripEdit,
            this.toolStripSeparator1,
            this.toolStripCmd,
            this.toolStripSeparator5,
            this.toolStripClearHistory,
            this.toolStripSeparator2,
            this.toolStripLabel1,
            this.toolStripViewType,
            this.toolStripSeparator3,
            this.toolStripLabel2,
            this.toolStripFilter});
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
            this.toolStripFeedback.Size = new System.Drawing.Size(84, 22);
            this.toolStripFeedback.Text = "Send feedback";
            this.toolStripFeedback.Click += new System.EventHandler(this.toolStripFeedback_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripClearHistory
            // 
            this.toolStripClearHistory.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripClearHistory.Image = global::Pretorianie.Tytan.SharedIcons.PreviousPageHS;
            this.toolStripClearHistory.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.toolStripClearHistory.Name = "toolStripClearHistory";
            this.toolStripClearHistory.Size = new System.Drawing.Size(23, 22);
            this.toolStripClearHistory.Text = "Clear edited variables cached values.";
            this.toolStripClearHistory.Click += new System.EventHandler(this.toolStripClearHistory_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(44, 22);
            this.toolStripLabel1.Text = "Target:";
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
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(36, 22);
            this.toolStripLabel2.Text = "Filter:";
            // 
            // toolStripFilter
            // 
            this.toolStripFilter.DropDownWidth = 190;
            this.toolStripFilter.Name = "toolStripFilter";
            this.toolStripFilter.Size = new System.Drawing.Size(121, 25);
            this.toolStripFilter.ToolTipText = "Filter variables by name using regular expression (Ctrl+F)";
            this.toolStripFilter.SelectedIndexChanged += new System.EventHandler(this.toolStripFilter_SelectedIndexChanged);
            this.toolStripFilter.KeyDown += new System.Windows.Forms.KeyEventHandler(this.toolStripFilter_KeyDown);
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
            this.list.KeyDown += new System.Windows.Forms.KeyEventHandler(this.list_KeyDown);
            // 
            // columnName
            // 
            this.columnName.Text = "Name";
            this.columnName.Width = 220;
            // 
            // columnValue
            // 
            this.columnValue.Text = "Value";
            this.columnValue.Width = 460;
            // 
            // contextMenu
            // 
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addToolStripMenuItem,
            this.editToolStripMenuItem,
            this.toolStripMenuItem1,
            this.deleteToolStripMenuItem});
            this.contextMenu.Name = "contextMenu";
            this.contextMenu.Size = new System.Drawing.Size(108, 76);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(104, 6);
            // 
            // addToolStripMenuItem
            // 
            this.addToolStripMenuItem.Image = global::Pretorianie.Tytan.SharedIcons.Plus;
            this.addToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.addToolStripMenuItem.Name = "addToolStripMenuItem";
            this.addToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.addToolStripMenuItem.Text = "Add...";
            this.addToolStripMenuItem.Click += new System.EventHandler(this.toolStripAdd_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Image = global::Pretorianie.Tytan.SharedIcons.Edit;
            this.editToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.editToolStripMenuItem.Text = "Edit...";
            this.editToolStripMenuItem.Click += new System.EventHandler(this.toolStripEdit_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Image = global::Pretorianie.Tytan.SharedIcons.Minus;
            this.deleteToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.toolStripDelete_Click);
            // 
            // toolStripSaveSession
            // 
            this.toolStripSaveSession.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripSaveSession.Enabled = false;
            this.toolStripSaveSession.Image = global::Pretorianie.Tytan.SharedIcons.SaveHS;
            this.toolStripSaveSession.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.toolStripSaveSession.Name = "toolStripSaveSession";
            this.toolStripSaveSession.Size = new System.Drawing.Size(23, 22);
            this.toolStripSaveSession.ToolTipText = "Save current session...";
            this.toolStripSaveSession.Click += new System.EventHandler(this.toolStripSaveSession_Click);
            // 
            // toolStripLoadSession
            // 
            this.toolStripLoadSession.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripLoadSession.Image = global::Pretorianie.Tytan.SharedIcons.OpenHS;
            this.toolStripLoadSession.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.toolStripLoadSession.Name = "toolStripLoadSession";
            this.toolStripLoadSession.Size = new System.Drawing.Size(23, 22);
            this.toolStripLoadSession.ToolTipText = "Load stored session...";
            this.toolStripLoadSession.Click += new System.EventHandler(this.toolStripLoadSession_Click);
            // 
            // toolStripUndoSession
            // 
            this.toolStripUndoSession.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripUndoSession.Enabled = false;
            this.toolStripUndoSession.Image = global::Pretorianie.Tytan.SharedIcons.Edit_UndoHS;
            this.toolStripUndoSession.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.toolStripUndoSession.Name = "toolStripUndoSession";
            this.toolStripUndoSession.Size = new System.Drawing.Size(23, 22);
            this.toolStripUndoSession.ToolTipText = "Undo current session...";
            this.toolStripUndoSession.Click += new System.EventHandler(this.toolStripUndoSession_Click);
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
        private System.Windows.Forms.ToolStripButton toolStripSaveSession;
        private System.Windows.Forms.ToolStripButton toolStripLoadSession;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripComboBox toolStripFilter;
        private System.Windows.Forms.ToolStripButton toolStripUndoSession;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripButton toolStripClearHistory;
    }
}
