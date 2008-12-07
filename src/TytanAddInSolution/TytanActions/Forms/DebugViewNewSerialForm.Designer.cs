namespace Pretorianie.Tytan.Forms
{
    partial class DebugViewNewSerialForm
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
            this.cmbEncodings = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbBaudRate = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbPortNames = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // linkFeedback
            // 
            this.linkFeedback.Location = new System.Drawing.Point(12, 262);
            // 
            // bttOK
            // 
            this.bttOK.Location = new System.Drawing.Point(171, 257);
            // 
            // bttCancel
            // 
            this.bttCancel.Location = new System.Drawing.Point(252, 257);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.cmbEncodings);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.cmbBaudRate);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cmbPortNames);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(315, 239);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Properties";
            // 
            // cmbEncodings
            // 
            this.cmbEncodings.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEncodings.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cmbEncodings.FormattingEnabled = true;
            this.cmbEncodings.Location = new System.Drawing.Point(104, 94);
            this.cmbEncodings.Name = "cmbEncodings";
            this.cmbEncodings.Size = new System.Drawing.Size(183, 21);
            this.cmbEncodings.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label3.Location = new System.Drawing.Point(17, 97);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Encoding:";
            // 
            // cmbBaudRate
            // 
            this.cmbBaudRate.FormattingEnabled = true;
            this.cmbBaudRate.Location = new System.Drawing.Point(104, 62);
            this.cmbBaudRate.MaxDropDownItems = 20;
            this.cmbBaudRate.Name = "cmbBaudRate";
            this.cmbBaudRate.Size = new System.Drawing.Size(106, 21);
            this.cmbBaudRate.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label2.Location = new System.Drawing.Point(17, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Baud rate:";
            // 
            // cmbPortNames
            // 
            this.cmbPortNames.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPortNames.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cmbPortNames.FormattingEnabled = true;
            this.cmbPortNames.Location = new System.Drawing.Point(104, 30);
            this.cmbPortNames.MaxDropDownItems = 20;
            this.cmbPortNames.Name = "cmbPortNames";
            this.cmbPortNames.Size = new System.Drawing.Size(124, 21);
            this.cmbPortNames.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label1.Location = new System.Drawing.Point(17, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Port:";
            // 
            // DebugViewNewSerialForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(339, 289);
            this.Controls.Add(this.groupBox1);
            this.Name = "DebugViewNewSerialForm";
            this.Text = "New serial port connection";
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
        private System.Windows.Forms.ComboBox cmbPortNames;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbBaudRate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbEncodings;
        private System.Windows.Forms.Label label3;
    }
}
