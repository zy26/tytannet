namespace Pretorianie.Tytan.Forms
{
    partial class AboutBoxTipsForm
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.txtTipText = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.bttTip = new System.Windows.Forms.Button();
            this.bttClose = new System.Windows.Forms.Button();
            this.lblTipNumber = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Pretorianie.Tytan.SharedIcons.TipsAndTricks;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(52, 47);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.panel1.BackColor = System.Drawing.Color.Silver;
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(52, 185);
            this.panel1.TabIndex = 4;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.Add(this.txtTipText);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Location = new System.Drawing.Point(64, 12);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(452, 185);
            this.panel2.TabIndex = 0;
            // 
            // txtTipText
            // 
            this.txtTipText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTipText.BackColor = System.Drawing.Color.White;
            this.txtTipText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtTipText.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.txtTipText.ForeColor = System.Drawing.Color.Black;
            this.txtTipText.HideSelection = false;
            this.txtTipText.Location = new System.Drawing.Point(12, 53);
            this.txtTipText.Name = "txtTipText";
            this.txtTipText.ReadOnly = true;
            this.txtTipText.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.txtTipText.Size = new System.Drawing.Size(428, 122);
            this.txtTipText.TabIndex = 2;
            this.txtTipText.Text = "Simple tip...";
            this.txtTipText.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.richTextBox1_LinkClicked);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label1.Location = new System.Drawing.Point(8, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(175, 22);
            this.label1.TabIndex = 0;
            this.label1.Text = "Do you know that ...";
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3.BackColor = System.Drawing.Color.Black;
            this.panel3.Location = new System.Drawing.Point(0, 46);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(454, 1);
            this.panel3.TabIndex = 1;
            // 
            // bttTip
            // 
            this.bttTip.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bttTip.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.bttTip.Location = new System.Drawing.Point(323, 204);
            this.bttTip.Name = "bttTip";
            this.bttTip.Size = new System.Drawing.Size(114, 23);
            this.bttTip.TabIndex = 0;
            this.bttTip.Text = "Next &tip";
            this.bttTip.UseVisualStyleBackColor = true;
            this.bttTip.Click += new System.EventHandler(this.bttTip_Click);
            // 
            // bttClose
            // 
            this.bttClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bttClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bttClose.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.bttClose.Location = new System.Drawing.Point(443, 204);
            this.bttClose.Name = "bttClose";
            this.bttClose.Size = new System.Drawing.Size(75, 23);
            this.bttClose.TabIndex = 1;
            this.bttClose.Text = "&Close";
            this.bttClose.UseVisualStyleBackColor = true;
            // 
            // lblTipNumber
            // 
            this.lblTipNumber.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblTipNumber.Location = new System.Drawing.Point(12, 209);
            this.lblTipNumber.Name = "lblTipNumber";
            this.lblTipNumber.Size = new System.Drawing.Size(52, 18);
            this.lblTipNumber.TabIndex = 2;
            this.lblTipNumber.Text = "1 of 20";
            this.lblTipNumber.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // AboutBoxTipsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.CancelButton = this.bttClose;
            this.ClientSize = new System.Drawing.Size(530, 238);
            this.Controls.Add(this.lblTipNumber);
            this.Controls.Add(this.bttClose);
            this.Controls.Add(this.bttTip);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutBoxTipsForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TytanNET - Tips & Tricks";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button bttTip;
        private System.Windows.Forms.Button bttClose;
        private System.Windows.Forms.RichTextBox txtTipText;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblTipNumber;
    }
}