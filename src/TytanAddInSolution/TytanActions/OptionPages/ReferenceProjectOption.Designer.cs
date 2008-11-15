namespace Pretorianie.Tytan.OptionPages
{
    partial class ReferenceProjectOption
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
            this.bttReload = new System.Windows.Forms.Button();
            this.listReferences = new System.Windows.Forms.ListBox();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.bttAdd = new System.Windows.Forms.Button();
            this.bttRemove = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.bttFind = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // bttReload
            // 
            this.bttReload.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.bttReload.Location = new System.Drawing.Point(3, 262);
            this.bttReload.Name = "bttReload";
            this.bttReload.Size = new System.Drawing.Size(90, 23);
            this.bttReload.TabIndex = 0;
            this.bttReload.Text = "Reload menu";
            this.bttReload.UseVisualStyleBackColor = true;
            this.bttReload.Click += new System.EventHandler(this.bttReload_Click);
            // 
            // listReferences
            // 
            this.listReferences.FormattingEnabled = true;
            this.listReferences.Location = new System.Drawing.Point(3, 29);
            this.listReferences.Name = "listReferences";
            this.listReferences.Size = new System.Drawing.Size(378, 199);
            this.listReferences.TabIndex = 1;
            this.listReferences.SelectedIndexChanged += new System.EventHandler(this.listReferences_SelectedIndexChanged);
            // 
            // txtPath
            // 
            this.txtPath.Location = new System.Drawing.Point(3, 236);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(257, 20);
            this.txtPath.TabIndex = 2;
            // 
            // bttAdd
            // 
            this.bttAdd.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.bttAdd.Location = new System.Drawing.Point(304, 234);
            this.bttAdd.Name = "bttAdd";
            this.bttAdd.Size = new System.Drawing.Size(77, 23);
            this.bttAdd.TabIndex = 3;
            this.bttAdd.Text = "Add";
            this.bttAdd.UseVisualStyleBackColor = true;
            this.bttAdd.Click += new System.EventHandler(this.bttAdd_Click);
            // 
            // bttRemove
            // 
            this.bttRemove.Enabled = false;
            this.bttRemove.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.bttRemove.Location = new System.Drawing.Point(304, 262);
            this.bttRemove.Name = "bttRemove";
            this.bttRemove.Size = new System.Drawing.Size(77, 23);
            this.bttRemove.TabIndex = 4;
            this.bttRemove.Text = "Remove";
            this.bttRemove.UseVisualStyleBackColor = true;
            this.bttRemove.Click += new System.EventHandler(this.bttRemove_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label1.Location = new System.Drawing.Point(3, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(138, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Links to custom assemblies:";
            // 
            // bttFind
            // 
            this.bttFind.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.bttFind.Location = new System.Drawing.Point(266, 234);
            this.bttFind.Name = "bttFind";
            this.bttFind.Size = new System.Drawing.Size(32, 23);
            this.bttFind.TabIndex = 6;
            this.bttFind.Text = "...";
            this.bttFind.UseVisualStyleBackColor = true;
            this.bttFind.Click += new System.EventHandler(this.bttFind_Click);
            // 
            // ReferenceProjectOption
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Controls.Add(this.bttFind);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.bttRemove);
            this.Controls.Add(this.bttAdd);
            this.Controls.Add(this.txtPath);
            this.Controls.Add(this.listReferences);
            this.Controls.Add(this.bttReload);
            this.Name = "ReferenceProjectOption";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bttReload;
        private System.Windows.Forms.ListBox listReferences;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.Button bttAdd;
        private System.Windows.Forms.Button bttRemove;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button bttFind;
    }
}
