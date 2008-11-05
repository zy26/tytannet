namespace Pretorianie.Tytan.Forms
{
    partial class AboutBoxForm
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
            this.bttClose = new System.Windows.Forms.Button();
            this.lblFriendly = new System.Windows.Forms.Label();
            this.linkHomepage = new System.Windows.Forms.LinkLabel();
            this.txtDescription = new System.Windows.Forms.RichTextBox();
            this.txtInfo = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.aboutIcon = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.aboutIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // bttClose
            // 
            this.bttClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bttClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bttClose.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.bttClose.Location = new System.Drawing.Point(375, 231);
            this.bttClose.Name = "bttClose";
            this.bttClose.Size = new System.Drawing.Size(75, 23);
            this.bttClose.TabIndex = 0;
            this.bttClose.Text = "&Close";
            this.bttClose.UseVisualStyleBackColor = true;
            // 
            // lblFriendly
            // 
            this.lblFriendly.AutoSize = true;
            this.lblFriendly.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblFriendly.Location = new System.Drawing.Point(82, 12);
            this.lblFriendly.Name = "lblFriendly";
            this.lblFriendly.Size = new System.Drawing.Size(81, 13);
            this.lblFriendly.TabIndex = 1;
            this.lblFriendly.Text = "- friendly name -";
            // 
            // linkHomepage
            // 
            this.linkHomepage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.linkHomepage.AutoSize = true;
            this.linkHomepage.BackColor = System.Drawing.Color.Transparent;
            this.linkHomepage.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkHomepage.Location = new System.Drawing.Point(9, 236);
            this.linkHomepage.Name = "linkHomepage";
            this.linkHomepage.Size = new System.Drawing.Size(146, 13);
            this.linkHomepage.TabIndex = 3;
            this.linkHomepage.TabStop = true;
            this.linkHomepage.Text = "TytanNET - check for update";
            this.linkHomepage.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkClicked);
            // 
            // txtDescription
            // 
            this.txtDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDescription.BackColor = System.Drawing.SystemColors.Window;
            this.txtDescription.HideSelection = false;
            this.txtDescription.Location = new System.Drawing.Point(85, 94);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.ReadOnly = true;
            this.txtDescription.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
            this.txtDescription.ShowSelectionMargin = true;
            this.txtDescription.Size = new System.Drawing.Size(365, 126);
            this.txtDescription.TabIndex = 4;
            this.txtDescription.Text = "";
            this.txtDescription.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.txtDescription_LinkClicked);
            // 
            // txtInfo
            // 
            this.txtInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtInfo.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.txtInfo.Location = new System.Drawing.Point(82, 37);
            this.txtInfo.Name = "txtInfo";
            this.txtInfo.Size = new System.Drawing.Size(368, 54);
            this.txtInfo.TabIndex = 5;
            this.txtInfo.Text = "- info -";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.pictureBox1.Image = global::Pretorianie.Tytan.SharedIcons.Gradient;
            this.pictureBox1.Location = new System.Drawing.Point(8, 82);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(72, 141);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            // 
            // aboutIcon
            // 
            this.aboutIcon.BackColor = System.Drawing.SystemColors.Control;
            this.aboutIcon.Location = new System.Drawing.Point(12, 12);
            this.aboutIcon.Name = "aboutIcon";
            this.aboutIcon.Size = new System.Drawing.Size(64, 64);
            this.aboutIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.aboutIcon.TabIndex = 1;
            this.aboutIcon.TabStop = false;
            // 
            // AboutBoxForm
            // 
            this.AcceptButton = this.bttClose;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bttClose;
            this.ClientSize = new System.Drawing.Size(462, 261);
            this.Controls.Add(this.txtInfo);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.linkHomepage);
            this.Controls.Add(this.lblFriendly);
            this.Controls.Add(this.aboutIcon);
            this.Controls.Add(this.bttClose);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutBoxForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TytanNET - About";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.aboutIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bttClose;
        private System.Windows.Forms.PictureBox aboutIcon;
        private System.Windows.Forms.Label lblFriendly;
        private System.Windows.Forms.LinkLabel linkHomepage;
        private System.Windows.Forms.RichTextBox txtDescription;
        private System.Windows.Forms.Label txtInfo;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}