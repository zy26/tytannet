namespace Pretorianie.Tytan.Forms
{
    partial class EnvironmentSessionItemsForm
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
            this.listVariables = new System.Windows.Forms.ListView();
            this.columnHeaderTarget = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderName = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderAction = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderValue = new System.Windows.Forms.ColumnHeader();
            this.bttClear = new System.Windows.Forms.Button();
            this.columnHeaderPrimary = new System.Windows.Forms.ColumnHeader();
            this.SuspendLayout();
            // 
            // bttOK
            // 
            this.bttOK.Text = "&Revert";
            // 
            // listVariables
            // 
            this.listVariables.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listVariables.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderTarget,
            this.columnHeaderName,
            this.columnHeaderAction,
            this.columnHeaderValue,
            this.columnHeaderPrimary});
            this.listVariables.FullRowSelect = true;
            this.listVariables.GridLines = true;
            this.listVariables.HideSelection = false;
            this.listVariables.Location = new System.Drawing.Point(12, 12);
            this.listVariables.Name = "listVariables";
            this.listVariables.Size = new System.Drawing.Size(629, 371);
            this.listVariables.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listVariables.TabIndex = 3;
            this.listVariables.UseCompatibleStateImageBehavior = false;
            this.listVariables.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderTarget
            // 
            this.columnHeaderTarget.Text = "Target";
            // 
            // columnHeaderName
            // 
            this.columnHeaderName.Text = "Name";
            this.columnHeaderName.Width = 140;
            // 
            // columnHeaderAction
            // 
            this.columnHeaderAction.Text = "Action";
            this.columnHeaderAction.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // columnHeaderValue
            // 
            this.columnHeaderValue.Text = "Value";
            this.columnHeaderValue.Width = 360;
            // 
            // bttClear
            // 
            this.bttClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bttClear.DialogResult = System.Windows.Forms.DialogResult.Ignore;
            this.bttClear.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.bttClear.Location = new System.Drawing.Point(98, 389);
            this.bttClear.Name = "bttClear";
            this.bttClear.Size = new System.Drawing.Size(75, 23);
            this.bttClear.TabIndex = 4;
            this.bttClear.Text = "Clea&r";
            this.bttClear.UseVisualStyleBackColor = true;
            // 
            // columnHeaderPrimary
            // 
            this.columnHeaderPrimary.Text = "Primary Value";
            this.columnHeaderPrimary.Width = 360;
            // 
            // EnvironmentSessionItemsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(653, 421);
            this.Controls.Add(this.listVariables);
            this.Controls.Add(this.bttClear);
            this.Name = "EnvironmentSessionItemsForm";
            this.Text = "Current Session State";
            this.Controls.SetChildIndex(this.bttClear, 0);
            this.Controls.SetChildIndex(this.listVariables, 0);
            this.Controls.SetChildIndex(this.linkFeedback, 0);
            this.Controls.SetChildIndex(this.bttOK, 0);
            this.Controls.SetChildIndex(this.bttCancel, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listVariables;
        private System.Windows.Forms.ColumnHeader columnHeaderName;
        private System.Windows.Forms.ColumnHeader columnHeaderValue;
        private System.Windows.Forms.ColumnHeader columnHeaderAction;
        private System.Windows.Forms.ColumnHeader columnHeaderTarget;
        private System.Windows.Forms.Button bttClear;
        private System.Windows.Forms.ColumnHeader columnHeaderPrimary;
    }
}
