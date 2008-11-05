namespace Pretorianie.Tytan.Forms
{
    partial class MultiRenameForm
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
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmbLetter = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.buttonRefresh = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.numericDigits = new System.Windows.Forms.NumericUpDown();
            this.numericStep = new System.Windows.Forms.NumericUpDown();
            this.numericStart = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonCounter = new System.Windows.Forms.Button();
            this.buttonNameRange = new System.Windows.Forms.Button();
            this.buttonNameFull = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textFormula = new System.Windows.Forms.TextBox();
            this.checkState = new System.Windows.Forms.CheckBox();
            this.listView = new System.Windows.Forms.ListView();
            this.columnName = new System.Windows.Forms.ColumnHeader();
            this.columnNewName = new System.Windows.Forms.ColumnHeader();
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.stateAboveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeStateAboveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.enableStateAboveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.disableStateAboveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.stateBelowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeStateBelowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.enableStateBelowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.disableStateBelowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericDigits)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericStep)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericStart)).BeginInit();
            this.contextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // linkFeedback
            // 
            this.linkFeedback.Location = new System.Drawing.Point(12, 410);
            this.linkFeedback.TabIndex = 1;
            // 
            // bttOK
            // 
            this.bttOK.Location = new System.Drawing.Point(494, 405);
            this.bttOK.TabIndex = 2;
            // 
            // bttCancel
            // 
            this.bttCancel.Location = new System.Drawing.Point(575, 405);
            this.bttCancel.TabIndex = 3;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.cmbLetter);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.buttonRefresh);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.buttonCounter);
            this.groupBox1.Controls.Add(this.buttonNameRange);
            this.groupBox1.Controls.Add(this.buttonNameFull);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.textFormula);
            this.groupBox1.Controls.Add(this.checkState);
            this.groupBox1.Controls.Add(this.listView);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(638, 387);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Formula";
            // 
            // cmbLetter
            // 
            this.cmbLetter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmbLetter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLetter.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cmbLetter.FormattingEnabled = true;
            this.cmbLetter.Items.AddRange(new object[] {
            "force upper",
            "force lower",
            "-- the same --"});
            this.cmbLetter.Location = new System.Drawing.Point(141, 349);
            this.cmbLetter.Name = "cmbLetter";
            this.cmbLetter.Size = new System.Drawing.Size(116, 21);
            this.cmbLetter.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(80, 352);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(55, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "First letter:";
            // 
            // buttonRefresh
            // 
            this.buttonRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRefresh.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonRefresh.Location = new System.Drawing.Point(402, 312);
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.Size = new System.Drawing.Size(74, 23);
            this.buttonRefresh.TabIndex = 8;
            this.buttonRefresh.Text = "Refresh";
            this.buttonRefresh.UseVisualStyleBackColor = true;
            this.buttonRefresh.Click += new System.EventHandler(this.buttonRefresh_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.numericDigits);
            this.groupBox2.Controls.Add(this.numericStep);
            this.groupBox2.Controls.Add(this.numericStart);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox2.Location = new System.Drawing.Point(482, 281);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(150, 100);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Counter [C]";
            // 
            // numericDigits
            // 
            this.numericDigits.Location = new System.Drawing.Point(74, 71);
            this.numericDigits.Name = "numericDigits";
            this.numericDigits.Size = new System.Drawing.Size(63, 20);
            this.numericDigits.TabIndex = 5;
            this.numericDigits.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // numericStep
            // 
            this.numericStep.Location = new System.Drawing.Point(74, 45);
            this.numericStep.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numericStep.Name = "numericStep";
            this.numericStep.Size = new System.Drawing.Size(63, 20);
            this.numericStep.TabIndex = 3;
            this.numericStep.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // numericStart
            // 
            this.numericStart.Location = new System.Drawing.Point(74, 19);
            this.numericStart.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numericStart.Name = "numericStart";
            this.numericStart.Size = new System.Drawing.Size(63, 20);
            this.numericStart.TabIndex = 1;
            this.numericStart.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 73);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(36, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Digits:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 47);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Step:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Start at:";
            // 
            // buttonCounter
            // 
            this.buttonCounter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonCounter.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonCounter.Location = new System.Drawing.Point(202, 312);
            this.buttonCounter.Name = "buttonCounter";
            this.buttonCounter.Size = new System.Drawing.Size(55, 23);
            this.buttonCounter.TabIndex = 5;
            this.buttonCounter.Text = "[C]";
            this.buttonCounter.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.buttonCounter.UseVisualStyleBackColor = true;
            this.buttonCounter.Click += new System.EventHandler(this.buttonCounter_Click);
            // 
            // buttonNameRange
            // 
            this.buttonNameRange.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonNameRange.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonNameRange.Location = new System.Drawing.Point(141, 312);
            this.buttonNameRange.Name = "buttonNameRange";
            this.buttonNameRange.Size = new System.Drawing.Size(55, 23);
            this.buttonNameRange.TabIndex = 6;
            this.buttonNameRange.Text = "[N#-#]...";
            this.buttonNameRange.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.buttonNameRange.UseVisualStyleBackColor = true;
            this.buttonNameRange.Click += new System.EventHandler(this.buttonNameRange_Click);
            // 
            // buttonNameFull
            // 
            this.buttonNameFull.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonNameFull.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonNameFull.Location = new System.Drawing.Point(80, 312);
            this.buttonNameFull.Name = "buttonNameFull";
            this.buttonNameFull.Size = new System.Drawing.Size(55, 23);
            this.buttonNameFull.TabIndex = 4;
            this.buttonNameFull.Text = "[N]";
            this.buttonNameFull.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.buttonNameFull.UseVisualStyleBackColor = true;
            this.buttonNameFull.Click += new System.EventHandler(this.buttonNameFull_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 292);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Format:";
            // 
            // textFormula
            // 
            this.textFormula.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textFormula.Location = new System.Drawing.Point(80, 289);
            this.textFormula.Name = "textFormula";
            this.textFormula.Size = new System.Drawing.Size(396, 20);
            this.textFormula.TabIndex = 3;
            this.textFormula.Text = "[N]";
            // 
            // checkState
            // 
            this.checkState.AutoSize = true;
            this.checkState.Checked = true;
            this.checkState.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkState.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.checkState.Location = new System.Drawing.Point(9, 19);
            this.checkState.Name = "checkState";
            this.checkState.Size = new System.Drawing.Size(88, 18);
            this.checkState.TabIndex = 0;
            this.checkState.Text = "Global state";
            this.checkState.UseVisualStyleBackColor = true;
            this.checkState.CheckedChanged += new System.EventHandler(this.checkState_CheckedChanged);
            // 
            // listView
            // 
            this.listView.AllowColumnReorder = true;
            this.listView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listView.CheckBoxes = true;
            this.listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnName,
            this.columnNewName});
            this.listView.ContextMenuStrip = this.contextMenu;
            this.listView.FullRowSelect = true;
            this.listView.GridLines = true;
            this.listView.HideSelection = false;
            this.listView.Location = new System.Drawing.Point(6, 43);
            this.listView.Name = "listView";
            this.listView.Size = new System.Drawing.Size(626, 235);
            this.listView.TabIndex = 1;
            this.listView.UseCompatibleStateImageBehavior = false;
            this.listView.View = System.Windows.Forms.View.Details;
            this.listView.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listView_ItemChecked);
            // 
            // columnName
            // 
            this.columnName.Text = "Name";
            this.columnName.Width = 300;
            // 
            // columnNewName
            // 
            this.columnNewName.Text = "New Name";
            this.columnNewName.Width = 300;
            // 
            // contextMenu
            // 
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.stateAboveToolStripMenuItem,
            this.toolStripMenuItem1,
            this.stateBelowToolStripMenuItem});
            this.contextMenu.Name = "contextMenu";
            this.contextMenu.Size = new System.Drawing.Size(145, 54);
            // 
            // stateAboveToolStripMenuItem
            // 
            this.stateAboveToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.changeStateAboveToolStripMenuItem,
            this.toolStripMenuItem2,
            this.enableStateAboveToolStripMenuItem,
            this.disableStateAboveToolStripMenuItem});
            this.stateAboveToolStripMenuItem.Name = "stateAboveToolStripMenuItem";
            this.stateAboveToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.stateAboveToolStripMenuItem.Text = "State above";
            // 
            // changeStateAboveToolStripMenuItem
            // 
            this.changeStateAboveToolStripMenuItem.Name = "changeStateAboveToolStripMenuItem";
            this.changeStateAboveToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.changeStateAboveToolStripMenuItem.Text = "Change state";
            this.changeStateAboveToolStripMenuItem.Click += new System.EventHandler(this.changeStateAboveToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(147, 6);
            // 
            // enableStateAboveToolStripMenuItem
            // 
            this.enableStateAboveToolStripMenuItem.Name = "enableStateAboveToolStripMenuItem";
            this.enableStateAboveToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.enableStateAboveToolStripMenuItem.Text = "Enable";
            this.enableStateAboveToolStripMenuItem.Click += new System.EventHandler(this.enableStateAboveToolStripMenuItem_Click);
            // 
            // disableStateAboveToolStripMenuItem
            // 
            this.disableStateAboveToolStripMenuItem.Name = "disableStateAboveToolStripMenuItem";
            this.disableStateAboveToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.disableStateAboveToolStripMenuItem.Text = "Disable";
            this.disableStateAboveToolStripMenuItem.Click += new System.EventHandler(this.disableStateAboveToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(141, 6);
            // 
            // stateBelowToolStripMenuItem
            // 
            this.stateBelowToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.changeStateBelowToolStripMenuItem,
            this.toolStripMenuItem3,
            this.enableStateBelowToolStripMenuItem,
            this.disableStateBelowToolStripMenuItem});
            this.stateBelowToolStripMenuItem.Name = "stateBelowToolStripMenuItem";
            this.stateBelowToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.stateBelowToolStripMenuItem.Text = "State below";
            // 
            // changeStateBelowToolStripMenuItem
            // 
            this.changeStateBelowToolStripMenuItem.Name = "changeStateBelowToolStripMenuItem";
            this.changeStateBelowToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.changeStateBelowToolStripMenuItem.Text = "Change state";
            this.changeStateBelowToolStripMenuItem.Click += new System.EventHandler(this.changeStateBelowToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(147, 6);
            // 
            // enableStateBelowToolStripMenuItem
            // 
            this.enableStateBelowToolStripMenuItem.Name = "enableStateBelowToolStripMenuItem";
            this.enableStateBelowToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.enableStateBelowToolStripMenuItem.Text = "Enable";
            this.enableStateBelowToolStripMenuItem.Click += new System.EventHandler(this.enableStateBelowToolStripMenuItem_Click);
            // 
            // disableStateBelowToolStripMenuItem
            // 
            this.disableStateBelowToolStripMenuItem.Name = "disableStateBelowToolStripMenuItem";
            this.disableStateBelowToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.disableStateBelowToolStripMenuItem.Text = "Disable";
            this.disableStateBelowToolStripMenuItem.Click += new System.EventHandler(this.disableStateBelowToolStripMenuItem_Click);
            // 
            // MultiRenameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(662, 437);
            this.Controls.Add(this.groupBox1);
            this.Name = "MultiRenameForm";
            this.Text = "Multi rename tool - [Methods]";
            this.Controls.SetChildIndex(this.groupBox1, 0);
            this.Controls.SetChildIndex(this.linkFeedback, 0);
            this.Controls.SetChildIndex(this.bttOK, 0);
            this.Controls.SetChildIndex(this.bttCancel, 0);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericDigits)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericStep)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericStart)).EndInit();
            this.contextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListView listView;
        private System.Windows.Forms.ColumnHeader columnName;
        private System.Windows.Forms.ColumnHeader columnNewName;
        private System.Windows.Forms.CheckBox checkState;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textFormula;
        private System.Windows.Forms.Button buttonNameFull;
        private System.Windows.Forms.Button buttonCounter;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.NumericUpDown numericDigits;
        private System.Windows.Forms.NumericUpDown numericStep;
        private System.Windows.Forms.NumericUpDown numericStart;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonNameRange;
        private System.Windows.Forms.Button buttonRefresh;
        private System.Windows.Forms.ComboBox cmbLetter;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem stateAboveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changeStateAboveToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem enableStateAboveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem disableStateAboveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stateBelowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changeStateBelowToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem enableStateBelowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem disableStateBelowToolStripMenuItem;
    }
}