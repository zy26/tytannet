namespace VisualEditor.DebugView
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
            this.debugViewTool1 = new Pretorianie.Tytan.Tools.DebugViewTool();
            this.SuspendLayout();
            // 
            // debugViewTool1
            // 
            this.debugViewTool1.AutoScrollDown = true;
            this.debugViewTool1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.debugViewTool1.FilterMessages = new string[] {
        ""};
            this.debugViewTool1.Location = new System.Drawing.Point(0, 0);
            this.debugViewTool1.Name = "debugViewTool1";
            this.debugViewTool1.SelectedItemIndex = -1;
            this.debugViewTool1.ServiceEnabled = false;
            this.debugViewTool1.Size = new System.Drawing.Size(1091, 953);
            this.debugViewTool1.TabIndex = 0;
            this.debugViewTool1.VisibleCodeJump = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1091, 953);
            this.Controls.Add(this.debugViewTool1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Debug View Tool";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Pretorianie.Tytan.Tools.DebugViewTool debugViewTool1;
    }
}