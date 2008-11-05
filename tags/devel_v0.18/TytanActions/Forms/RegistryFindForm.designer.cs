namespace Pretorianie.Tytan.Forms
{
    partial class RegistryFindForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.cmbSearchText = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chbValues = new System.Windows.Forms.CheckBox();
            this.chbKeys = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // linkFeedback
            // 
            this.linkFeedback.Location = new System.Drawing.Point(12, 130);
            this.linkFeedback.TabIndex = 3;
            // 
            // bttOK
            // 
            this.bttOK.Location = new System.Drawing.Point(211, 125);
            this.bttOK.TabIndex = 4;
            // 
            // bttCancel
            // 
            this.bttCancel.Location = new System.Drawing.Point(292, 125);
            this.bttCancel.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Search text:";
            // 
            // cmbSearchText
            // 
            this.cmbSearchText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbSearchText.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cmbSearchText.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbSearchText.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cmbSearchText.FormattingEnabled = true;
            this.cmbSearchText.Location = new System.Drawing.Point(82, 19);
            this.cmbSearchText.MaxDropDownItems = 20;
            this.cmbSearchText.Name = "cmbSearchText";
            this.cmbSearchText.Size = new System.Drawing.Size(285, 21);
            this.cmbSearchText.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.checkBox1);
            this.groupBox1.Controls.Add(this.chbValues);
            this.groupBox1.Controls.Add(this.chbKeys);
            this.groupBox1.Location = new System.Drawing.Point(15, 46);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(352, 73);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Look-up options";
            // 
            // chbValues
            // 
            this.chbValues.AutoSize = true;
            this.chbValues.Checked = true;
            this.chbValues.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbValues.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.chbValues.Location = new System.Drawing.Point(24, 43);
            this.chbValues.Name = "chbValues";
            this.chbValues.Size = new System.Drawing.Size(93, 18);
            this.chbValues.TabIndex = 2;
            this.chbValues.Text = "Value names";
            this.chbValues.UseVisualStyleBackColor = true;
            // 
            // chbKeys
            // 
            this.chbKeys.AutoSize = true;
            this.chbKeys.Checked = true;
            this.chbKeys.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbKeys.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.chbKeys.Location = new System.Drawing.Point(24, 19);
            this.chbKeys.Name = "chbKeys";
            this.chbKeys.Size = new System.Drawing.Size(84, 18);
            this.chbKeys.TabIndex = 0;
            this.chbKeys.Text = "Key names";
            this.chbKeys.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.checkBox1.Location = new System.Drawing.Point(196, 20);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(99, 18);
            this.checkBox1.TabIndex = 1;
            this.checkBox1.Text = "Data contents";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // RegistryFindForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(379, 157);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbSearchText);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "RegistryFindForm";
            this.Text = "Find registry entry";
            this.Controls.SetChildIndex(this.groupBox1, 0);
            this.Controls.SetChildIndex(this.cmbSearchText, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.linkFeedback, 0);
            this.Controls.SetChildIndex(this.bttOK, 0);
            this.Controls.SetChildIndex(this.bttCancel, 0);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbSearchText;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chbValues;
        private System.Windows.Forms.CheckBox chbKeys;
        private System.Windows.Forms.CheckBox checkBox1;
    }
}