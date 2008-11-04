namespace Pretorianie.Tytan.Forms
{
    partial class InitConstructorRefactorForm
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
            this.dataVars = new System.Windows.Forms.DataGridView();
            this.columnEnabled = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.columnVariable = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnProperty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataVars)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // linkFeedback
            // 
            this.linkFeedback.Location = new System.Drawing.Point(12, 226);
            this.linkFeedback.TabIndex = 1;
            // 
            // bttOK
            // 
            this.bttOK.Location = new System.Drawing.Point(363, 221);
            this.bttOK.TabIndex = 2;
            // 
            // bttCancel
            // 
            this.bttCancel.Location = new System.Drawing.Point(444, 221);
            this.bttCancel.TabIndex = 3;
            // 
            // dataVars
            // 
            this.dataVars.AllowUserToAddRows = false;
            this.dataVars.AllowUserToDeleteRows = false;
            this.dataVars.AllowUserToOrderColumns = true;
            this.dataVars.AllowUserToResizeRows = false;
            this.dataVars.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataVars.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dataVars.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataVars.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnEnabled,
            this.columnVariable,
            this.columnProperty});
            this.dataVars.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dataVars.GridColor = System.Drawing.SystemColors.Control;
            this.dataVars.Location = new System.Drawing.Point(6, 19);
            this.dataVars.MultiSelect = false;
            this.dataVars.Name = "dataVars";
            this.dataVars.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dataVars.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataVars.Size = new System.Drawing.Size(495, 168);
            this.dataVars.TabIndex = 0;
            // 
            // columnEnabled
            // 
            this.columnEnabled.FillWeight = 60F;
            this.columnEnabled.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.columnEnabled.HeaderText = "Enabled";
            this.columnEnabled.Name = "columnEnabled";
            this.columnEnabled.Width = 60;
            // 
            // columnVariable
            // 
            this.columnVariable.FillWeight = 180F;
            this.columnVariable.HeaderText = "Variable";
            this.columnVariable.Name = "columnVariable";
            this.columnVariable.ReadOnly = true;
            this.columnVariable.Width = 180;
            // 
            // columnProperty
            // 
            this.columnProperty.FillWeight = 180F;
            this.columnProperty.HeaderText = "Parameter Name";
            this.columnProperty.Name = "columnProperty";
            this.columnProperty.Width = 180;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.dataVars);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(507, 203);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Variables -> Constructor Parameters";
            // 
            // InitConstructorRefactorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(531, 253);
            this.Controls.Add(this.groupBox1);
            this.Name = "InitConstructorRefactorForm";
            this.Text = "Init Constructor";
            this.Controls.SetChildIndex(this.linkFeedback, 0);
            this.Controls.SetChildIndex(this.bttOK, 0);
            this.Controls.SetChildIndex(this.bttCancel, 0);
            this.Controls.SetChildIndex(this.groupBox1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.dataVars)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataVars;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn columnEnabled;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnVariable;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnProperty;
    }
}
