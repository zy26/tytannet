namespace Pretorianie.Tytan.Forms
{
    partial class PropertyRefactorForm
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbProp = new System.Windows.Forms.ComboBox();
            this.cmbVar = new System.Windows.Forms.ComboBox();
            this.cmbMethod = new System.Windows.Forms.ComboBox();
            this.textRegionName = new System.Windows.Forms.TextBox();
            this.checkComments = new System.Windows.Forms.CheckBox();
            this.checkRegion = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkUpdateNames = new System.Windows.Forms.CheckBox();
            this.dataVars = new System.Windows.Forms.DataGridView();
            this.columnEnabled = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.columnVariable = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnNewVariable = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnProperty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataVars)).BeginInit();
            this.SuspendLayout();
            // 
            // linkFeedback
            // 
            this.linkFeedback.Location = new System.Drawing.Point(12, 403);
            // 
            // bttOK
            // 
            this.bttOK.Location = new System.Drawing.Point(533, 398);
            // 
            // bttCancel
            // 
            this.bttCancel.Location = new System.Drawing.Point(614, 398);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.cmbProp);
            this.groupBox2.Controls.Add(this.cmbVar);
            this.groupBox2.Controls.Add(this.cmbMethod);
            this.groupBox2.Controls.Add(this.textRegionName);
            this.groupBox2.Controls.Add(this.checkComments);
            this.groupBox2.Controls.Add(this.checkRegion);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox2.Location = new System.Drawing.Point(12, 275);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(677, 117);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Options";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label3.Location = new System.Drawing.Point(20, 84);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(111, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Property access level:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label2.Location = new System.Drawing.Point(20, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(110, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Variable access level:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label1.Location = new System.Drawing.Point(20, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Output methods:";
            // 
            // cmbProp
            // 
            this.cmbProp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProp.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cmbProp.FormattingEnabled = true;
            this.cmbProp.Items.AddRange(new object[] {
            "-- increased variable level --",
            "-- the same as variable level --",
            "public",
            "internal",
            "protected",
            "protected internal",
            "private"});
            this.cmbProp.Location = new System.Drawing.Point(150, 81);
            this.cmbProp.Name = "cmbProp";
            this.cmbProp.Size = new System.Drawing.Size(159, 21);
            this.cmbProp.TabIndex = 7;
            // 
            // cmbVar
            // 
            this.cmbVar.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbVar.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cmbVar.FormattingEnabled = true;
            this.cmbVar.Items.AddRange(new object[] {
            "-- decrease level --",
            "-- do not change --",
            "public",
            "internal",
            "protected",
            "protected internal",
            "private"});
            this.cmbVar.Location = new System.Drawing.Point(150, 54);
            this.cmbVar.Name = "cmbVar";
            this.cmbVar.Size = new System.Drawing.Size(159, 21);
            this.cmbVar.TabIndex = 7;
            // 
            // cmbMethod
            // 
            this.cmbMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMethod.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cmbMethod.FormattingEnabled = true;
            this.cmbMethod.Items.AddRange(new object[] {
            "Getter and Setter",
            "Only Getter",
            "Only Setter"});
            this.cmbMethod.Location = new System.Drawing.Point(150, 27);
            this.cmbMethod.Name = "cmbMethod";
            this.cmbMethod.Size = new System.Drawing.Size(121, 21);
            this.cmbMethod.TabIndex = 6;
            // 
            // textRegionName
            // 
            this.textRegionName.Location = new System.Drawing.Point(521, 27);
            this.textRegionName.Name = "textRegionName";
            this.textRegionName.Size = new System.Drawing.Size(138, 20);
            this.textRegionName.TabIndex = 5;
            // 
            // checkComments
            // 
            this.checkComments.AutoSize = true;
            this.checkComments.Checked = true;
            this.checkComments.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkComments.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.checkComments.Location = new System.Drawing.Point(364, 53);
            this.checkComments.Name = "checkComments";
            this.checkComments.Size = new System.Drawing.Size(102, 18);
            this.checkComments.TabIndex = 4;
            this.checkComments.Text = "Add comments";
            this.checkComments.UseVisualStyleBackColor = true;
            // 
            // checkRegion
            // 
            this.checkRegion.AutoSize = true;
            this.checkRegion.Checked = true;
            this.checkRegion.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkRegion.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.checkRegion.Location = new System.Drawing.Point(364, 29);
            this.checkRegion.Name = "checkRegion";
            this.checkRegion.Size = new System.Drawing.Size(134, 18);
            this.checkRegion.TabIndex = 3;
            this.checkRegion.Text = "Encapsulate in region";
            this.checkRegion.UseVisualStyleBackColor = true;
            this.checkRegion.CheckedChanged += new System.EventHandler(this.checkRegion_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.checkUpdateNames);
            this.groupBox1.Controls.Add(this.dataVars);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(677, 257);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Variables -> Properties";
            // 
            // checkUpdateNames
            // 
            this.checkUpdateNames.AutoSize = true;
            this.checkUpdateNames.Checked = true;
            this.checkUpdateNames.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkUpdateNames.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.checkUpdateNames.Location = new System.Drawing.Point(115, 17);
            this.checkUpdateNames.Name = "checkUpdateNames";
            this.checkUpdateNames.Size = new System.Drawing.Size(101, 18);
            this.checkUpdateNames.TabIndex = 0;
            this.checkUpdateNames.Text = "Update names";
            this.checkUpdateNames.UseVisualStyleBackColor = true;
            this.checkUpdateNames.CheckedChanged += new System.EventHandler(this.checkUpdateNames_CheckedChanged);
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
            this.columnNewVariable,
            this.columnProperty});
            this.dataVars.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dataVars.GridColor = System.Drawing.SystemColors.Control;
            this.dataVars.Location = new System.Drawing.Point(6, 40);
            this.dataVars.MultiSelect = false;
            this.dataVars.Name = "dataVars";
            this.dataVars.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dataVars.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataVars.Size = new System.Drawing.Size(665, 201);
            this.dataVars.TabIndex = 1;
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
            // columnNewVariable
            // 
            this.columnNewVariable.HeaderText = "New Variable Name";
            this.columnNewVariable.Name = "columnNewVariable";
            this.columnNewVariable.Width = 180;
            // 
            // columnProperty
            // 
            this.columnProperty.FillWeight = 180F;
            this.columnProperty.HeaderText = "Property";
            this.columnProperty.Name = "columnProperty";
            this.columnProperty.Width = 180;
            // 
            // PropertyRefactorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(701, 430);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "PropertyRefactorForm";
            this.Text = "Property Refactoring";
            this.Controls.SetChildIndex(this.linkFeedback, 0);
            this.Controls.SetChildIndex(this.bttOK, 0);
            this.Controls.SetChildIndex(this.bttCancel, 0);
            this.Controls.SetChildIndex(this.groupBox1, 0);
            this.Controls.SetChildIndex(this.groupBox2, 0);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataVars)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox checkComments;
        private System.Windows.Forms.CheckBox checkRegion;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dataVars;
        private System.Windows.Forms.DataGridViewCheckBoxColumn columnEnabled;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnVariable;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnNewVariable;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnProperty;
        private System.Windows.Forms.TextBox textRegionName;
        private System.Windows.Forms.ComboBox cmbVar;
        private System.Windows.Forms.ComboBox cmbMethod;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbProp;
        private System.Windows.Forms.CheckBox checkUpdateNames;

    }
}
