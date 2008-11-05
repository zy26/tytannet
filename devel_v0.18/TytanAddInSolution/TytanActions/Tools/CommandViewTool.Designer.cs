namespace Pretorianie.Tytan.Tools
{
    partial class CommandViewTool
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CommandViewTool));
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.info = new System.Windows.Forms.TreeView();
            this.toolStripMainMenu = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripMessage = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripFeedback = new System.Windows.Forms.ToolStripLabel();
            this.toolStripRefresh = new System.Windows.Forms.ToolStripButton();
            this.toolStripSaveImage = new System.Windows.Forms.ToolStripButton();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.refreshViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.saveIconAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMainMenu.SuspendLayout();
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "empty.bmp");
            this.imageList.Images.SetKeyName(1, "VSFolder_closed.bmp");
            this.imageList.Images.SetKeyName(2, "VSFolder_open.bmp");
            this.imageList.Images.SetKeyName(3, "VSObject_Event.bmp");
            this.imageList.Images.SetKeyName(4, "RadialChart.bmp");
            this.imageList.Images.SetKeyName(5, "Control_ToolBar.bmp");
            // 
            // info
            // 
            this.info.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.info.ContextMenuStrip = this.contextMenuStrip;
            this.info.ImageIndex = 0;
            this.info.ImageList = this.imageList;
            this.info.Location = new System.Drawing.Point(0, 24);
            this.info.Name = "info";
            this.info.SelectedImageIndex = 0;
            this.info.Size = new System.Drawing.Size(530, 422);
            this.info.TabIndex = 2;
            this.info.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.info_AfterSelect);
            // 
            // toolStripMainMenu
            // 
            this.toolStripMainMenu.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripMainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripRefresh,
            this.toolStripSeparator1,
            this.toolStripSaveImage,
            this.toolStripSeparator2,
            this.toolStripLabel2,
            this.toolStripMessage,
            this.toolStripFeedback});
            this.toolStripMainMenu.Location = new System.Drawing.Point(0, 0);
            this.toolStripMainMenu.Name = "toolStripMainMenu";
            this.toolStripMainMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.toolStripMainMenu.Size = new System.Drawing.Size(530, 25);
            this.toolStripMainMenu.TabIndex = 3;
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(32, 22);
            this.toolStripLabel2.Text = "Filter:";
            this.toolStripLabel2.Visible = false;
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
            this.toolStripMessage.ToolTipText = "Name pattern to filter the view";
            this.toolStripMessage.Visible = false;
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
            // toolStripRefresh
            // 
            this.toolStripRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripRefresh.Image = global::Pretorianie.Tytan.SharedIcons.SychronizeList;
            this.toolStripRefresh.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.toolStripRefresh.Name = "toolStripRefresh";
            this.toolStripRefresh.Size = new System.Drawing.Size(23, 22);
            this.toolStripRefresh.Text = "toolStripRefresh";
            this.toolStripRefresh.ToolTipText = "Refresh view...";
            this.toolStripRefresh.Click += new System.EventHandler(this.toolStripRefresh_Click);
            // 
            // toolStripSaveImage
            // 
            this.toolStripSaveImage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripSaveImage.Image = global::Pretorianie.Tytan.SharedIcons.SaveForm;
            this.toolStripSaveImage.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.toolStripSaveImage.Name = "toolStripSaveImage";
            this.toolStripSaveImage.Size = new System.Drawing.Size(23, 22);
            this.toolStripSaveImage.Text = "toolStripSaveImage";
            this.toolStripSaveImage.ToolTipText = "Save icon...";
            this.toolStripSaveImage.Click += new System.EventHandler(this.toolStripSaveImage_Click);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.refreshViewToolStripMenuItem,
            this.toolStripMenuItem1,
            this.saveIconAsToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(149, 54);
            // 
            // refreshViewToolStripMenuItem
            // 
            this.refreshViewToolStripMenuItem.Image = global::Pretorianie.Tytan.SharedIcons.SychronizeList;
            this.refreshViewToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.refreshViewToolStripMenuItem.Name = "refreshViewToolStripMenuItem";
            this.refreshViewToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.refreshViewToolStripMenuItem.Text = "&Refresh View";
            this.refreshViewToolStripMenuItem.Click += new System.EventHandler(this.toolStripRefresh_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(145, 6);
            // 
            // saveIconAsToolStripMenuItem
            // 
            this.saveIconAsToolStripMenuItem.Image = global::Pretorianie.Tytan.SharedIcons.SaveForm;
            this.saveIconAsToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.saveIconAsToolStripMenuItem.Name = "saveIconAsToolStripMenuItem";
            this.saveIconAsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.saveIconAsToolStripMenuItem.Text = "Save icon as...";
            this.saveIconAsToolStripMenuItem.Click += new System.EventHandler(this.toolStripSaveImage_Click);
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
            this.toolStripSeparator2.Visible = false;
            // 
            // CommandViewTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.toolStripMainMenu);
            this.Controls.Add(this.info);
            this.Name = "CommandViewTool";
            this.Size = new System.Drawing.Size(530, 446);
            this.toolStripMainMenu.ResumeLayout(false);
            this.toolStripMainMenu.PerformLayout();
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.TreeView info;
        protected System.Windows.Forms.ToolStrip toolStripMainMenu;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripComboBox toolStripMessage;
        private System.Windows.Forms.ToolStripLabel toolStripFeedback;
        private System.Windows.Forms.ToolStripButton toolStripRefresh;
        private System.Windows.Forms.ToolStripButton toolStripSaveImage;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem refreshViewToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem saveIconAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;


    }
}
