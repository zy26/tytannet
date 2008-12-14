namespace Pretorianie.Tytan.Tools
{
    partial class DebugViewTool
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DebugViewTool));
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripFeedback = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripProcesses = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripMessage = new System.Windows.Forms.ToolStripComboBox();
            this.list = new System.Windows.Forms.ListView();
            this.columnNo = new System.Windows.Forms.ColumnHeader();
            this.columnTimeStamp = new System.Windows.Forms.ColumnHeader();
            this.columnPID = new System.Windows.Forms.ColumnHeader();
            this.columnName = new System.Windows.Forms.ColumnHeader();
            this.columnMessage = new System.Windows.Forms.ColumnHeader();
            this.contextMenuList = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.jumpToFunctionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.labelError = new System.Windows.Forms.Label();
            this.toolStripMainMenu = new System.Windows.Forms.ToolStrip();
            this.toolStripSaveAs = new System.Windows.Forms.ToolStripButton();
            this.toolStripOpen = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripClear = new System.Windows.Forms.ToolStripButton();
            this.toolStripStop = new System.Windows.Forms.ToolStripButton();
            this.toolStripStart = new System.Windows.Forms.ToolStripButton();
            this.toolStripScrollDown = new System.Windows.Forms.ToolStripButton();
            this.toolStripScrollNo = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripAddCom = new System.Windows.Forms.ToolStripButton();
            this.toolStripAddTCP = new System.Windows.Forms.ToolStripButton();
            this.toolStripCloseSource = new System.Windows.Forms.ToolStripButton();
            this.toolStripJump = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripCustomColumns = new System.Windows.Forms.ToolStripComboBox();
            this.contextMenuList.SuspendLayout();
            this.toolStripMainMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripFeedback
            // 
            this.toolStripFeedback.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripFeedback.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripFeedback.IsLink = true;
            this.toolStripFeedback.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.toolStripFeedback.Name = "toolStripFeedback";
            this.toolStripFeedback.Size = new System.Drawing.Size(80, 22);
            this.toolStripFeedback.Text = "Send feedback";
            this.toolStripFeedback.Click += new System.EventHandler(this.toolStripFeedback_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            this.toolStripSeparator2.Visible = false;
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(48, 22);
            this.toolStripLabel1.Text = "Process:";
            // 
            // toolStripProcesses
            // 
            this.toolStripProcesses.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolStripProcesses.DropDownWidth = 250;
            this.toolStripProcesses.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.toolStripProcesses.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripProcesses.MaxDropDownItems = 50;
            this.toolStripProcesses.Name = "toolStripProcesses";
            this.toolStripProcesses.Size = new System.Drawing.Size(121, 25);
            this.toolStripProcesses.ToolTipText = "Process to listen";
            this.toolStripProcesses.SelectedIndexChanged += new System.EventHandler(this.toolStripProcesses_SelectedIndexChanged);
            this.toolStripProcesses.DropDown += new System.EventHandler(this.toolStripProcesses_DropDown);
            this.toolStripProcesses.DropDownClosed += new System.EventHandler(this.toolStripProcesses_DropDownClosed);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(75, 22);
            this.toolStripLabel2.Text = "Message filter:";
            // 
            // toolStripMessage
            // 
            this.toolStripMessage.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.toolStripMessage.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.toolStripMessage.DropDownWidth = 250;
            this.toolStripMessage.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.toolStripMessage.MaxDropDownItems = 50;
            this.toolStripMessage.Name = "toolStripMessage";
            this.toolStripMessage.Size = new System.Drawing.Size(121, 25);
            this.toolStripMessage.ToolTipText = "Message to listen";
            this.toolStripMessage.SelectedIndexChanged += new System.EventHandler(this.toolStripMessage_SelectedIndexChanged);
            this.toolStripMessage.KeyDown += new System.Windows.Forms.KeyEventHandler(this.toolStripMessage_KeyDown);
            this.toolStripMessage.Leave += new System.EventHandler(this.toolStripMessage_Leave);
            // 
            // list
            // 
            this.list.AllowColumnReorder = true;
            this.list.CausesValidation = false;
            this.list.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnNo,
            this.columnTimeStamp,
            this.columnPID,
            this.columnName,
            this.columnMessage});
            this.list.ContextMenuStrip = this.contextMenuList;
            this.list.Dock = System.Windows.Forms.DockStyle.Fill;
            this.list.FullRowSelect = true;
            this.list.HideSelection = false;
            this.list.Location = new System.Drawing.Point(0, 25);
            this.list.MultiSelect = false;
            this.list.Name = "list";
            this.list.Size = new System.Drawing.Size(972, 504);
            this.list.TabIndex = 3;
            this.list.UseCompatibleStateImageBehavior = false;
            this.list.View = System.Windows.Forms.View.Details;
            this.list.VirtualMode = true;
            this.list.DoubleClick += new System.EventHandler(this.list_DoubleClick);
            this.list.MouseDown += new System.Windows.Forms.MouseEventHandler(this.list_MouseDown);
            // 
            // columnNo
            // 
            this.columnNo.Text = "No.";
            this.columnNo.Width = 40;
            // 
            // columnTimeStamp
            // 
            this.columnTimeStamp.Text = "Time Stamp";
            this.columnTimeStamp.Width = 85;
            // 
            // columnPID
            // 
            this.columnPID.Text = "PID";
            this.columnPID.Width = 50;
            // 
            // columnName
            // 
            this.columnName.Text = "Process";
            this.columnName.Width = 80;
            // 
            // columnMessage
            // 
            this.columnMessage.Text = "Message";
            this.columnMessage.Width = 800;
            // 
            // contextMenuList
            // 
            this.contextMenuList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem,
            this.clearToolStripMenuItem,
            this.toolStripMenuItem1,
            this.jumpToFunctionToolStripMenuItem});
            this.contextMenuList.Name = "contextMenuList";
            this.contextMenuList.Size = new System.Drawing.Size(165, 76);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.copyToolStripMenuItem.Text = "&Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // clearToolStripMenuItem
            // 
            this.clearToolStripMenuItem.Image = global::Pretorianie.Tytan.SharedIcons.Cross;
            this.clearToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
            this.clearToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.clearToolStripMenuItem.Text = "C&lear";
            this.clearToolStripMenuItem.Click += new System.EventHandler(this.toolStripClose_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(161, 6);
            this.toolStripMenuItem1.Visible = false;
            // 
            // jumpToFunctionToolStripMenuItem
            // 
            this.jumpToFunctionToolStripMenuItem.Image = global::Pretorianie.Tytan.SharedIcons.CodeJump;
            this.jumpToFunctionToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.jumpToFunctionToolStripMenuItem.Name = "jumpToFunctionToolStripMenuItem";
            this.jumpToFunctionToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.jumpToFunctionToolStripMenuItem.Text = "&Jump to function...";
            this.jumpToFunctionToolStripMenuItem.Visible = false;
            this.jumpToFunctionToolStripMenuItem.Click += new System.EventHandler(this.list_DoubleClick);
            // 
            // labelError
            // 
            this.labelError.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.labelError.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.labelError.Font = new System.Drawing.Font("Lucida Sans Unicode", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelError.ForeColor = System.Drawing.Color.Brown;
            this.labelError.Location = new System.Drawing.Point(0, 87);
            this.labelError.Name = "labelError";
            this.labelError.Size = new System.Drawing.Size(972, 54);
            this.labelError.TabIndex = 4;
            this.labelError.Text = "Unabel to receive debug messages.";
            this.labelError.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelError.Visible = false;
            // 
            // toolStripMainMenu
            // 
            this.toolStripMainMenu.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripMainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSaveAs,
            this.toolStripOpen,
            this.toolStripSeparator5,
            this.toolStripClear,
            this.toolStripSeparator1,
            this.toolStripStop,
            this.toolStripStart,
            this.toolStripScrollDown,
            this.toolStripScrollNo,
            this.toolStripSeparator6,
            this.toolStripAddCom,
            this.toolStripAddTCP,
            this.toolStripCloseSource,
            this.toolStripSeparator2,
            this.toolStripJump,
            this.toolStripSeparator3,
            this.toolStripLabel1,
            this.toolStripProcesses,
            this.toolStripLabel2,
            this.toolStripMessage,
            this.toolStripFeedback,
            this.toolStripSeparator4,
            this.toolStripLabel3,
            this.toolStripCustomColumns});
            this.toolStripMainMenu.Location = new System.Drawing.Point(0, 0);
            this.toolStripMainMenu.Name = "toolStripMainMenu";
            this.toolStripMainMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.toolStripMainMenu.Size = new System.Drawing.Size(972, 25);
            this.toolStripMainMenu.TabIndex = 2;
            // 
            // toolStripSaveAs
            // 
            this.toolStripSaveAs.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripSaveAs.Image = global::Pretorianie.Tytan.SharedIcons.SaveDisk;
            this.toolStripSaveAs.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.toolStripSaveAs.Name = "toolStripSaveAs";
            this.toolStripSaveAs.Size = new System.Drawing.Size(23, 22);
            this.toolStripSaveAs.ToolTipText = "Save as...";
            this.toolStripSaveAs.Click += new System.EventHandler(this.toolStripSaveAs_Click);
            // 
            // toolStripOpen
            // 
            this.toolStripOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripOpen.Image = global::Pretorianie.Tytan.SharedIcons.OpenFile;
            this.toolStripOpen.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.toolStripOpen.Name = "toolStripOpen";
            this.toolStripOpen.Size = new System.Drawing.Size(23, 22);
            this.toolStripOpen.ToolTipText = "Open file...";
            this.toolStripOpen.Click += new System.EventHandler(this.toolStripOpen_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripClear
            // 
            this.toolStripClear.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripClear.Enabled = false;
            this.toolStripClear.Image = global::Pretorianie.Tytan.SharedIcons.Cross;
            this.toolStripClear.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.toolStripClear.Name = "toolStripClear";
            this.toolStripClear.Size = new System.Drawing.Size(23, 22);
            this.toolStripClear.ToolTipText = "Clear content";
            this.toolStripClear.Click += new System.EventHandler(this.toolStripClose_Click);
            // 
            // toolStripStop
            // 
            this.toolStripStop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripStop.Image = global::Pretorianie.Tytan.SharedIcons.ControlPause;
            this.toolStripStop.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.toolStripStop.Name = "toolStripStop";
            this.toolStripStop.Size = new System.Drawing.Size(23, 22);
            this.toolStripStop.ToolTipText = "Stop monitoring";
            this.toolStripStop.Click += new System.EventHandler(this.toolStripStop_Click);
            // 
            // toolStripStart
            // 
            this.toolStripStart.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripStart.Image = global::Pretorianie.Tytan.SharedIcons.ControlPlay;
            this.toolStripStart.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.toolStripStart.Name = "toolStripStart";
            this.toolStripStart.Size = new System.Drawing.Size(23, 22);
            this.toolStripStart.ToolTipText = "Start monitoring";
            this.toolStripStart.Visible = false;
            this.toolStripStart.Click += new System.EventHandler(this.toolStripStart_Click);
            // 
            // toolStripScrollDown
            // 
            this.toolStripScrollDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripScrollDown.Image = global::Pretorianie.Tytan.SharedIcons.ArrowDown;
            this.toolStripScrollDown.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.toolStripScrollDown.Name = "toolStripScrollDown";
            this.toolStripScrollDown.Size = new System.Drawing.Size(23, 22);
            this.toolStripScrollDown.ToolTipText = "Auto scroll down (enabled)";
            this.toolStripScrollDown.Click += new System.EventHandler(this.toolStripScrollDown_Click);
            // 
            // toolStripScrollNo
            // 
            this.toolStripScrollNo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripScrollNo.Enabled = false;
            this.toolStripScrollNo.Image = global::Pretorianie.Tytan.SharedIcons.ArrowDownGray;
            this.toolStripScrollNo.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.toolStripScrollNo.Name = "toolStripScrollNo";
            this.toolStripScrollNo.Size = new System.Drawing.Size(23, 22);
            this.toolStripScrollNo.ToolTipText = "Auto scroll down (disabled)";
            this.toolStripScrollNo.Visible = false;
            this.toolStripScrollNo.Click += new System.EventHandler(this.toolStripScrollNo_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripAddCom
            // 
            this.toolStripAddCom.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripAddCom.Image = global::Pretorianie.Tytan.SharedIcons.ComConnection;
            this.toolStripAddCom.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.toolStripAddCom.Name = "toolStripAddCom";
            this.toolStripAddCom.Size = new System.Drawing.Size(23, 22);
            this.toolStripAddCom.Text = "Add COM port source";
            this.toolStripAddCom.Click += new System.EventHandler(this.toolStripAddCom_Click);
            // 
            // toolStripAddTCP
            // 
            this.toolStripAddTCP.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripAddTCP.Enabled = false;
            this.toolStripAddTCP.Image = global::Pretorianie.Tytan.SharedIcons.NetworkConnection;
            this.toolStripAddTCP.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.toolStripAddTCP.Name = "toolStripAddTCP";
            this.toolStripAddTCP.Size = new System.Drawing.Size(23, 22);
            this.toolStripAddTCP.Text = "Add network source";
            this.toolStripAddTCP.Visible = false;
            // 
            // toolStripCloseSource
            // 
            this.toolStripCloseSource.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripCloseSource.Enabled = false;
            this.toolStripCloseSource.Image = ((System.Drawing.Image)(resources.GetObject("toolStripCloseSource.Image")));
            this.toolStripCloseSource.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.toolStripCloseSource.Name = "toolStripCloseSource";
            this.toolStripCloseSource.Size = new System.Drawing.Size(23, 22);
            this.toolStripCloseSource.Text = "Close source";
            this.toolStripCloseSource.Click += new System.EventHandler(this.toolStripCloseSource_Click);
            // 
            // toolStripJump
            // 
            this.toolStripJump.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripJump.Image = global::Pretorianie.Tytan.SharedIcons.CodeJump;
            this.toolStripJump.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.toolStripJump.Name = "toolStripJump";
            this.toolStripJump.Size = new System.Drawing.Size(23, 22);
            this.toolStripJump.ToolTipText = "Jump to the code";
            this.toolStripJump.Visible = false;
            this.toolStripJump.Click += new System.EventHandler(this.list_DoubleClick);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            this.toolStripSeparator4.Visible = false;
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.Name = "toolStripLabel3";
            this.toolStripLabel3.Size = new System.Drawing.Size(60, 22);
            this.toolStripLabel3.Text = "Code jump:";
            this.toolStripLabel3.Visible = false;
            // 
            // toolStripCustomColumns
            // 
            this.toolStripCustomColumns.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolStripCustomColumns.DropDownWidth = 150;
            this.toolStripCustomColumns.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.toolStripCustomColumns.Items.AddRange(new object[] {
            "-- disabled --",
            "-- automatic --",
            "-- autodetect --",
            "1 = class",
            "1,2 = class::method",
            "2 = class",
            "2,3 = class::method",
            "3 = class",
            "3,4 = class::method",
            "4 = class",
            "4,5 = class::method",
            "5 = class",
            "5,6 = class:method"});
            this.toolStripCustomColumns.MaxDropDownItems = 30;
            this.toolStripCustomColumns.Name = "toolStripCustomColumns";
            this.toolStripCustomColumns.Size = new System.Drawing.Size(121, 25);
            this.toolStripCustomColumns.Visible = false;
            this.toolStripCustomColumns.SelectedIndexChanged += new System.EventHandler(this.toolStripCustomColumns_SelectedIndexChanged);
            // 
            // DebugViewTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelError);
            this.Controls.Add(this.list);
            this.Controls.Add(this.toolStripMainMenu);
            this.DoubleBuffered = true;
            this.Name = "DebugViewTool";
            this.Size = new System.Drawing.Size(972, 529);
            this.Load += new System.EventHandler(this.DebugViewTool_Load);
            this.contextMenuList.ResumeLayout(false);
            this.toolStripMainMenu.ResumeLayout(false);
            this.toolStripMainMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStripButton toolStripSaveAs;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel toolStripFeedback;
        private System.Windows.Forms.ToolStripButton toolStripScrollDown;
        private System.Windows.Forms.ToolStripButton toolStripClear;
        private System.Windows.Forms.ToolStripButton toolStripStop;
        private System.Windows.Forms.ToolStripButton toolStripStart;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolStripJump;
        private System.Windows.Forms.ToolStripButton toolStripScrollNo;
        private System.Windows.Forms.ListView list;
        private System.Windows.Forms.ColumnHeader columnNo;
        private System.Windows.Forms.ColumnHeader columnPID;
        private System.Windows.Forms.ColumnHeader columnMessage;
        private System.Windows.Forms.Label labelError;
        private System.Windows.Forms.ColumnHeader columnTimeStamp;
        private System.Windows.Forms.ColumnHeader columnName;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripComboBox toolStripProcesses;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripComboBox toolStripMessage;
        private System.Windows.Forms.ContextMenuStrip contextMenuList;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem jumpToFunctionToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem clearToolStripMenuItem;
        protected System.Windows.Forms.ToolStrip toolStripMainMenu;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripLabel toolStripLabel3;
        private System.Windows.Forms.ToolStripComboBox toolStripCustomColumns;
        private System.Windows.Forms.ToolStripButton toolStripOpen;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripButton toolStripAddCom;
        private System.Windows.Forms.ToolStripButton toolStripAddTCP;
        private System.Windows.Forms.ToolStripButton toolStripCloseSource;




    }
}
