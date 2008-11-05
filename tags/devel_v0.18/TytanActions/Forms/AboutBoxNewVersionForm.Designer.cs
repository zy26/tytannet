namespace Pretorianie.Tytan.Forms
{
    partial class AboutBoxNewVersionForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblText = new System.Windows.Forms.Label();
            this.bttClose = new System.Windows.Forms.Button();
            this.bttDownload = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.lblText);
            this.groupBox1.Location = new System.Drawing.Point(12, 10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(412, 93);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Action description";
            // 
            // lblText
            // 
            this.lblText.AutoSize = true;
            this.lblText.Location = new System.Drawing.Point(26, 31);
            this.lblText.Name = "lblText";
            this.lblText.Size = new System.Drawing.Size(36, 13);
            this.lblText.TabIndex = 0;
            this.lblText.Text = "- text -";
            // 
            // bttClose
            // 
            this.bttClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bttClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bttClose.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.bttClose.Location = new System.Drawing.Point(349, 109);
            this.bttClose.Name = "bttClose";
            this.bttClose.Size = new System.Drawing.Size(75, 23);
            this.bttClose.TabIndex = 1;
            this.bttClose.Text = "&Close";
            this.bttClose.UseVisualStyleBackColor = true;
            // 
            // bttDownload
            // 
            this.bttDownload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bttDownload.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.bttDownload.Location = new System.Drawing.Point(268, 109);
            this.bttDownload.Name = "bttDownload";
            this.bttDownload.Size = new System.Drawing.Size(75, 23);
            this.bttDownload.TabIndex = 2;
            this.bttDownload.Text = "&Download";
            this.bttDownload.UseVisualStyleBackColor = true;
            this.bttDownload.Click += new System.EventHandler(this.bttDownload_Click);
            // 
            // AboutBoxNewVersionForm
            // 
            this.AcceptButton = this.bttDownload;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bttClose;
            this.ClientSize = new System.Drawing.Size(436, 139);
            this.Controls.Add(this.bttDownload);
            this.Controls.Add(this.bttClose);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutBoxNewVersionForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TytanNET - Update";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button bttClose;
        private System.Windows.Forms.Button bttDownload;
        private System.Windows.Forms.Label lblText;
    }
}