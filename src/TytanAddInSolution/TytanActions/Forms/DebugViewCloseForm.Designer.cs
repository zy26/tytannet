namespace Pretorianie.Tytan.Forms
{
    partial class DebugViewCloseForm
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
            this.sources = new System.Windows.Forms.ListBox();
            this.bttStop = new System.Windows.Forms.Button();
            this.bttStopAll = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // linkFeedback
            // 
            this.linkFeedback.Location = new System.Drawing.Point(12, 279);
            // 
            // bttOK
            // 
            this.bttOK.Location = new System.Drawing.Point(336, 274);
            // 
            // bttCancel
            // 
            this.bttCancel.Location = new System.Drawing.Point(336, 274);
            this.bttCancel.Visible = false;
            // 
            // sources
            // 
            this.sources.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.sources.FormattingEnabled = true;
            this.sources.Location = new System.Drawing.Point(12, 12);
            this.sources.Name = "sources";
            this.sources.Size = new System.Drawing.Size(399, 251);
            this.sources.TabIndex = 3;
            // 
            // bttStop
            // 
            this.bttStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bttStop.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.bttStop.Location = new System.Drawing.Point(136, 274);
            this.bttStop.Name = "bttStop";
            this.bttStop.Size = new System.Drawing.Size(75, 23);
            this.bttStop.TabIndex = 4;
            this.bttStop.Text = "&Stop";
            this.bttStop.UseVisualStyleBackColor = true;
            this.bttStop.Click += new System.EventHandler(this.bttStop_Click);
            // 
            // bttStopAll
            // 
            this.bttStopAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bttStopAll.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.bttStopAll.Location = new System.Drawing.Point(217, 274);
            this.bttStopAll.Name = "bttStopAll";
            this.bttStopAll.Size = new System.Drawing.Size(75, 23);
            this.bttStopAll.TabIndex = 5;
            this.bttStopAll.Text = "Stop &all";
            this.bttStopAll.UseVisualStyleBackColor = true;
            this.bttStopAll.Click += new System.EventHandler(this.bttStopAll_Click);
            // 
            // DebugViewCloseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(423, 306);
            this.Controls.Add(this.sources);
            this.Controls.Add(this.bttStopAll);
            this.Controls.Add(this.bttStop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "DebugViewCloseForm";
            this.Text = "Closing data-source connection";
            this.Controls.SetChildIndex(this.bttStop, 0);
            this.Controls.SetChildIndex(this.bttOK, 0);
            this.Controls.SetChildIndex(this.bttStopAll, 0);
            this.Controls.SetChildIndex(this.sources, 0);
            this.Controls.SetChildIndex(this.linkFeedback, 0);
            this.Controls.SetChildIndex(this.bttCancel, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox sources;
        private System.Windows.Forms.Button bttStop;
        private System.Windows.Forms.Button bttStopAll;
    }
}
