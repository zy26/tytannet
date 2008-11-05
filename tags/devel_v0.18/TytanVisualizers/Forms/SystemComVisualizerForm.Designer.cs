namespace Pretorianie.Tytan.Forms
{
    partial class SystemComVisualizerForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SystemComVisualizerForm));
            this.typesTree = new System.Windows.Forms.TreeView();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.txtTypeName = new System.Windows.Forms.TextBox();
            this.bttClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // typesTree
            // 
            this.typesTree.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.typesTree.ImageIndex = 0;
            this.typesTree.ImageList = this.imageList;
            this.typesTree.Location = new System.Drawing.Point(12, 12);
            this.typesTree.Name = "typesTree";
            this.typesTree.SelectedImageIndex = 0;
            this.typesTree.Size = new System.Drawing.Size(580, 289);
            this.typesTree.TabIndex = 0;
            this.typesTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.typesTree_AfterSelect);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Magenta;
            this.imageList.Images.SetKeyName(0, "20.png");
            this.imageList.Images.SetKeyName(1, "19.png");
            this.imageList.Images.SetKeyName(2, "VSObject_Namespace.bmp");
            this.imageList.Images.SetKeyName(3, "VSFolder_closed.bmp");
            this.imageList.Images.SetKeyName(4, "VSFolder_open.bmp");
            this.imageList.Images.SetKeyName(5, "VSObject_Field.bmp");
            this.imageList.Images.SetKeyName(6, "VSObject_Interface.bmp");
            this.imageList.Images.SetKeyName(7, "VSObject_Method.bmp");
            this.imageList.Images.SetKeyName(8, "VSObject_Structure.bmp");
            this.imageList.Images.SetKeyName(9, "VSProject_component.bmp");
            // 
            // txtTypeName
            // 
            this.txtTypeName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTypeName.HideSelection = false;
            this.txtTypeName.Location = new System.Drawing.Point(12, 307);
            this.txtTypeName.Name = "txtTypeName";
            this.txtTypeName.ReadOnly = true;
            this.txtTypeName.Size = new System.Drawing.Size(580, 20);
            this.txtTypeName.TabIndex = 1;
            // 
            // bttClose
            // 
            this.bttClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bttClose.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.bttClose.Location = new System.Drawing.Point(517, 333);
            this.bttClose.Name = "bttClose";
            this.bttClose.Size = new System.Drawing.Size(75, 23);
            this.bttClose.TabIndex = 2;
            this.bttClose.Text = "&Close";
            this.bttClose.UseVisualStyleBackColor = true;
            // 
            // SystemComVisualizerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(604, 368);
            this.Controls.Add(this.bttClose);
            this.Controls.Add(this.txtTypeName);
            this.Controls.Add(this.typesTree);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SystemComVisualizerForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Info: System.__ComObject";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView typesTree;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.TextBox txtTypeName;
        private System.Windows.Forms.Button bttClose;
    }
}