namespace VisualEditor.Registry
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.registryViewTool1 = new Pretorianie.Tytan.Tools.RegistryViewTool();
            this.SuspendLayout();
            // 
            // registryViewTool1
            // 
            this.registryViewTool1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.registryViewTool1.Location = new System.Drawing.Point(0, 0);
            this.registryViewTool1.Name = "registryViewTool1";
            this.registryViewTool1.Size = new System.Drawing.Size(874, 855);
            this.registryViewTool1.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(874, 855);
            this.Controls.Add(this.registryViewTool1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Registry Editor Tool";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.ResumeLayout(false);

        }

        #endregion

        private Pretorianie.Tytan.Tools.RegistryViewTool registryViewTool1;
    }
}