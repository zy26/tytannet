namespace Pretorianie.Tytan.Forms
{
    partial class RegistryNewValueForm
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
            this.txtValue = new System.Windows.Forms.TextBox();
            this.cmbType = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.bttToMonoFont = new System.Windows.Forms.Button();
            this.bttToLower = new System.Windows.Forms.Button();
            this.bttToUpper = new System.Windows.Forms.Button();
            this.bttWordWrap = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // linkFeedback
            // 
            this.linkFeedback.Location = new System.Drawing.Point(12, 397);
            this.linkFeedback.TabIndex = 1;
            // 
            // bttOK
            // 
            this.bttOK.Location = new System.Drawing.Point(485, 392);
            this.bttOK.TabIndex = 2;
            // 
            // bttCancel
            // 
            this.bttCancel.Location = new System.Drawing.Point(566, 392);
            this.bttCancel.TabIndex = 3;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.bttToMonoFont);
            this.groupBox1.Controls.Add(this.bttToLower);
            this.groupBox1.Controls.Add(this.bttToUpper);
            this.groupBox1.Controls.Add(this.bttWordWrap);
            this.groupBox1.Controls.Add(this.txtValue);
            this.groupBox1.Controls.Add(this.cmbType);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtName);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(629, 374);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // txtValue
            // 
            this.txtValue.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtValue.HideSelection = false;
            this.txtValue.Location = new System.Drawing.Point(83, 102);
            this.txtValue.Multiline = true;
            this.txtValue.Name = "txtValue";
            this.txtValue.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtValue.Size = new System.Drawing.Size(529, 253);
            this.txtValue.TabIndex = 5;
            this.txtValue.WordWrap = false;
            // 
            // cmbType
            // 
            this.cmbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbType.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cmbType.FormattingEnabled = true;
            this.cmbType.Location = new System.Drawing.Point(83, 64);
            this.cmbType.Name = "cmbType";
            this.cmbType.Size = new System.Drawing.Size(132, 21);
            this.cmbType.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Type:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 105);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Value:";
            // 
            // txtName
            // 
            this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtName.HideSelection = false;
            this.txtName.Location = new System.Drawing.Point(83, 24);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(529, 20);
            this.txtName.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name:";
            // 
            // bttToMonoFont
            // 
            this.bttToMonoFont.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.bttToMonoFont.Image = global::Pretorianie.Tytan.SharedIcons.TextLetterSpacing;
            this.bttToMonoFont.Location = new System.Drawing.Point(19, 227);
            this.bttToMonoFont.Name = "bttToMonoFont";
            this.bttToMonoFont.Size = new System.Drawing.Size(44, 23);
            this.bttToMonoFont.TabIndex = 9;
            this.bttToMonoFont.UseVisualStyleBackColor = true;
            this.bttToMonoFont.Click += new System.EventHandler(this.bttToMonoFont_Click);
            // 
            // bttToLower
            // 
            this.bttToLower.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.bttToLower.Image = global::Pretorianie.Tytan.SharedIcons.TextSmallCaps;
            this.bttToLower.Location = new System.Drawing.Point(19, 198);
            this.bttToLower.Name = "bttToLower";
            this.bttToLower.Size = new System.Drawing.Size(44, 23);
            this.bttToLower.TabIndex = 8;
            this.bttToLower.UseVisualStyleBackColor = true;
            this.bttToLower.Click += new System.EventHandler(this.bttToLower_Click);
            // 
            // bttToUpper
            // 
            this.bttToUpper.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.bttToUpper.Image = global::Pretorianie.Tytan.SharedIcons.TextAllCaps;
            this.bttToUpper.Location = new System.Drawing.Point(19, 169);
            this.bttToUpper.Name = "bttToUpper";
            this.bttToUpper.Size = new System.Drawing.Size(44, 23);
            this.bttToUpper.TabIndex = 7;
            this.bttToUpper.UseVisualStyleBackColor = true;
            this.bttToUpper.Click += new System.EventHandler(this.bttToUpper_Click);
            // 
            // bttWordWrap
            // 
            this.bttWordWrap.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.bttWordWrap.Image = global::Pretorianie.Tytan.SharedIcons.TextHorizontalRule;
            this.bttWordWrap.Location = new System.Drawing.Point(19, 140);
            this.bttWordWrap.Name = "bttWordWrap";
            this.bttWordWrap.Size = new System.Drawing.Size(44, 23);
            this.bttWordWrap.TabIndex = 6;
            this.bttWordWrap.UseVisualStyleBackColor = true;
            this.bttWordWrap.Click += new System.EventHandler(this.bttWordWrap_Click);
            // 
            // RegistryNewValueForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(653, 424);
            this.Controls.Add(this.groupBox1);
            this.Name = "RegistryNewValueForm";
            this.Controls.SetChildIndex(this.groupBox1, 0);
            this.Controls.SetChildIndex(this.linkFeedback, 0);
            this.Controls.SetChildIndex(this.bttOK, 0);
            this.Controls.SetChildIndex(this.bttCancel, 0);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtValue;
        private System.Windows.Forms.ComboBox cmbType;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button bttWordWrap;
        private System.Windows.Forms.Button bttToLower;
        private System.Windows.Forms.Button bttToUpper;
        private System.Windows.Forms.Button bttToMonoFont;
    }
}