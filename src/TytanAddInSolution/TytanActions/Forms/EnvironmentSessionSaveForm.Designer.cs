namespace Pretorianie.Tytan.Forms
{
    partial class EnvironmentSessionSaveForm
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
            this.txtName = new System.Windows.Forms.TextBox();
            this.listItems = new System.Windows.Forms.ListView();
            this.columnHeaderName = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderAction = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderValue = new System.Windows.Forms.ColumnHeader();
            this.chkAll = new System.Windows.Forms.CheckBox();
            this.chkClear = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // linkFeedback
            // 
            this.linkFeedback.Location = new System.Drawing.Point(12, 317);
            this.linkFeedback.TabIndex = 4;
            // 
            // bttOK
            // 
            this.bttOK.Location = new System.Drawing.Point(497, 312);
            this.bttOK.TabIndex = 5;
            // 
            // bttCancel
            // 
            this.bttCancel.Location = new System.Drawing.Point(578, 312);
            this.bttCancel.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label1.Location = new System.Drawing.Point(12, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Name:";
            // 
            // txtName
            // 
            this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtName.Location = new System.Drawing.Point(56, 15);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(597, 20);
            this.txtName.TabIndex = 0;
            // 
            // listItems
            // 
            this.listItems.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listItems.CheckBoxes = true;
            this.listItems.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderName,
            this.columnHeaderAction,
            this.columnHeaderValue});
            this.listItems.FullRowSelect = true;
            this.listItems.GridLines = true;
            this.listItems.Location = new System.Drawing.Point(12, 41);
            this.listItems.Name = "listItems";
            this.listItems.Size = new System.Drawing.Size(641, 249);
            this.listItems.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listItems.TabIndex = 2;
            this.listItems.UseCompatibleStateImageBehavior = false;
            this.listItems.View = System.Windows.Forms.View.Details;
            this.listItems.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listItems_ItemChecked);
            // 
            // columnHeaderName
            // 
            this.columnHeaderName.Text = "Name";
            this.columnHeaderName.Width = 220;
            // 
            // columnHeaderAction
            // 
            this.columnHeaderAction.Text = "Action";
            this.columnHeaderAction.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // columnHeaderValue
            // 
            this.columnHeaderValue.Text = "Value";
            this.columnHeaderValue.Width = 320;
            // 
            // chkAll
            // 
            this.chkAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkAll.AutoSize = true;
            this.chkAll.Checked = true;
            this.chkAll.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAll.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.chkAll.Location = new System.Drawing.Point(15, 296);
            this.chkAll.Name = "chkAll";
            this.chkAll.Size = new System.Drawing.Size(170, 18);
            this.chkAll.TabIndex = 3;
            this.chkAll.Text = "Select / deselect all changes";
            this.chkAll.UseVisualStyleBackColor = true;
            this.chkAll.Click += new System.EventHandler(this.chkAll_Click);
            // 
            // chkClear
            // 
            this.chkClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkClear.AutoSize = true;
            this.chkClear.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.chkClear.Location = new System.Drawing.Point(343, 315);
            this.chkClear.Name = "chkClear";
            this.chkClear.Size = new System.Drawing.Size(148, 18);
            this.chkClear.TabIndex = 8;
            this.chkClear.Text = "Clear the current session";
            this.chkClear.UseVisualStyleBackColor = true;
            // 
            // EnvironmentSessionSaveForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(665, 344);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.listItems);
            this.Controls.Add(this.chkAll);
            this.Controls.Add(this.chkClear);
            this.Name = "EnvironmentSessionSaveForm";
            this.Text = "Save Session";
            this.Controls.SetChildIndex(this.chkClear, 0);
            this.Controls.SetChildIndex(this.chkAll, 0);
            this.Controls.SetChildIndex(this.listItems, 0);
            this.Controls.SetChildIndex(this.txtName, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.linkFeedback, 0);
            this.Controls.SetChildIndex(this.bttOK, 0);
            this.Controls.SetChildIndex(this.bttCancel, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.ListView listItems;
        private System.Windows.Forms.ColumnHeader columnHeaderName;
        private System.Windows.Forms.CheckBox chkAll;
        private System.Windows.Forms.ColumnHeader columnHeaderValue;
        private System.Windows.Forms.ColumnHeader columnHeaderAction;
        private System.Windows.Forms.CheckBox chkClear;
    }
}
