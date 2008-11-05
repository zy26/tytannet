namespace VisualEditor.NativeImage
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
            this.nativeImagePreviewTool1 = new Pretorianie.Tytan.Tools.NativeImagePreviewTool();
            this.SuspendLayout();
            // 
            // nativeImagePreviewTool1
            // 
            this.nativeImagePreviewTool1.BackColor = System.Drawing.SystemColors.Control;
            this.nativeImagePreviewTool1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nativeImagePreviewTool1.Location = new System.Drawing.Point(0, 0);
            this.nativeImagePreviewTool1.Name = "nativeImagePreviewTool1";
            this.nativeImagePreviewTool1.Size = new System.Drawing.Size(820, 597);
            this.nativeImagePreviewTool1.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(820, 597);
            this.Controls.Add(this.nativeImagePreviewTool1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NativeImage Preview Tool";
            this.ResumeLayout(false);

        }

        #endregion

        private Pretorianie.Tytan.Tools.NativeImagePreviewTool nativeImagePreviewTool1;
    }
}