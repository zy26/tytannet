namespace Pretorianie.Tytan.Forms
{
    partial class EnvironmentSessionListForm
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
            this.listSessions = new System.Windows.Forms.ListView();
            this.columnHeaderSession = new System.Windows.Forms.ColumnHeader();
            this.listItems = new System.Windows.Forms.ListView();
            this.columnHeaderName = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderAction = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderValue = new System.Windows.Forms.ColumnHeader();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbTarget = new System.Windows.Forms.ComboBox();
            this.bttDelete = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // linkFeedback
            // 
            this.linkFeedback.Location = new System.Drawing.Point(12, 330);
            // 
            // bttOK
            // 
            this.bttOK.Enabled = false;
            this.bttOK.Location = new System.Drawing.Point(546, 325);
            // 
            // bttCancel
            // 
            this.bttCancel.Location = new System.Drawing.Point(627, 325);
            // 
            // listSessions
            // 
            this.listSessions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.listSessions.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderSession});
            this.listSessions.FullRowSelect = true;
            this.listSessions.GridLines = true;
            this.listSessions.HideSelection = false;
            this.listSessions.Location = new System.Drawing.Point(12, 12);
            this.listSessions.Name = "listSessions";
            this.listSessions.ShowItemToolTips = true;
            this.listSessions.Size = new System.Drawing.Size(224, 307);
            this.listSessions.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listSessions.TabIndex = 3;
            this.listSessions.UseCompatibleStateImageBehavior = false;
            this.listSessions.View = System.Windows.Forms.View.Details;
            this.listSessions.SelectedIndexChanged += new System.EventHandler(this.listSessions_SelectedIndexChanged);
            this.listSessions.DoubleClick += new System.EventHandler(this.listSessions_DoubleClick);
            // 
            // columnHeaderSession
            // 
            this.columnHeaderSession.Text = "Session Name";
            this.columnHeaderSession.Width = 180;
            // 
            // listItems
            // 
            this.listItems.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listItems.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderName,
            this.columnHeaderAction,
            this.columnHeaderValue});
            this.listItems.FullRowSelect = true;
            this.listItems.GridLines = true;
            this.listItems.HideSelection = false;
            this.listItems.Location = new System.Drawing.Point(242, 12);
            this.listItems.Name = "listItems";
            this.listItems.Size = new System.Drawing.Size(460, 307);
            this.listItems.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listItems.TabIndex = 4;
            this.listItems.UseCompatibleStateImageBehavior = false;
            this.listItems.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderName
            // 
            this.columnHeaderName.Text = "Name";
            this.columnHeaderName.Width = 150;
            // 
            // columnHeaderAction
            // 
            this.columnHeaderAction.Text = "Action";
            this.columnHeaderAction.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // columnHeaderValue
            // 
            this.columnHeaderValue.Text = "Value";
            this.columnHeaderValue.Width = 210;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(359, 330);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Apply into:";
            // 
            // cmbTarget
            // 
            this.cmbTarget.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbTarget.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTarget.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cmbTarget.FormattingEnabled = true;
            this.cmbTarget.Items.AddRange(new object[] {
            "host",
            "user",
            "process"});
            this.cmbTarget.Location = new System.Drawing.Point(419, 327);
            this.cmbTarget.Name = "cmbTarget";
            this.cmbTarget.Size = new System.Drawing.Size(121, 21);
            this.cmbTarget.TabIndex = 6;
            // 
            // bttDelete
            // 
            this.bttDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bttDelete.Enabled = false;
            this.bttDelete.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.bttDelete.Location = new System.Drawing.Point(98, 325);
            this.bttDelete.Name = "bttDelete";
            this.bttDelete.Size = new System.Drawing.Size(75, 23);
            this.bttDelete.TabIndex = 7;
            this.bttDelete.Text = "Delete";
            this.bttDelete.UseVisualStyleBackColor = true;
            this.bttDelete.Click += new System.EventHandler(this.bttDelete_Click);
            // 
            // EnvironmentSessionListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(714, 357);
            this.Controls.Add(this.listSessions);
            this.Controls.Add(this.listItems);
            this.Controls.Add(this.cmbTarget);
            this.Controls.Add(this.bttDelete);
            this.Controls.Add(this.label1);
            this.Name = "EnvironmentSessionListForm";
            this.Text = "List of Stored Sessions";
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.bttDelete, 0);
            this.Controls.SetChildIndex(this.cmbTarget, 0);
            this.Controls.SetChildIndex(this.listItems, 0);
            this.Controls.SetChildIndex(this.listSessions, 0);
            this.Controls.SetChildIndex(this.linkFeedback, 0);
            this.Controls.SetChildIndex(this.bttOK, 0);
            this.Controls.SetChildIndex(this.bttCancel, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listSessions;
        private System.Windows.Forms.ColumnHeader columnHeaderSession;
        private System.Windows.Forms.ListView listItems;
        private System.Windows.Forms.ColumnHeader columnHeaderName;
        private System.Windows.Forms.ColumnHeader columnHeaderValue;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbTarget;
        private System.Windows.Forms.ColumnHeader columnHeaderAction;
        private System.Windows.Forms.Button bttDelete;
    }
}
