namespace Pretorianie.Tytan.Core.BaseForms
{
    partial class BasePackageForm
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
            this.linkFeedback = new System.Windows.Forms.LinkLabel();
            this.bttOK = new System.Windows.Forms.Button();
            this.bttCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // linkFeedback
            // 
            this.linkFeedback.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.linkFeedback.AutoSize = true;
            this.linkFeedback.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkFeedback.Location = new System.Drawing.Point(12, 394);
            this.linkFeedback.Name = "linkFeedback";
            this.linkFeedback.Size = new System.Drawing.Size(80, 13);
            this.linkFeedback.TabIndex = 0;
            this.linkFeedback.TabStop = true;
            this.linkFeedback.Text = "Send feedback";
            this.linkFeedback.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkFeedback_LinkClicked);
            // 
            // bttOK
            // 
            this.bttOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bttOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.bttOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.bttOK.Location = new System.Drawing.Point(485, 389);
            this.bttOK.Name = "bttOK";
            this.bttOK.Size = new System.Drawing.Size(75, 23);
            this.bttOK.TabIndex = 1;
            this.bttOK.Text = "&OK";
            this.bttOK.UseVisualStyleBackColor = true;
            // 
            // bttCancel
            // 
            this.bttCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bttCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bttCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.bttCancel.Location = new System.Drawing.Point(566, 389);
            this.bttCancel.Name = "bttCancel";
            this.bttCancel.Size = new System.Drawing.Size(75, 23);
            this.bttCancel.TabIndex = 2;
            this.bttCancel.Text = "&Cancel";
            this.bttCancel.UseVisualStyleBackColor = true;
            // 
            // BaseRefactorForm
            // 
            this.AcceptButton = this.bttOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bttCancel;
            this.ClientSize = new System.Drawing.Size(653, 421);
            this.Controls.Add(this.bttCancel);
            this.Controls.Add(this.bttOK);
            this.Controls.Add(this.linkFeedback);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BaseRefactorForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        /// <summary>
        /// Link to author.
        /// </summary>
        protected System.Windows.Forms.LinkLabel linkFeedback;
        /// <summary>
        /// OK button.
        /// </summary>
        protected System.Windows.Forms.Button bttOK;
        /// <summary>
        /// Cancel button.
        /// </summary>
        protected System.Windows.Forms.Button bttCancel;
    }
}