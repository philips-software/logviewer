namespace Analogy.Forms
{
    partial class XtraFormWindowsEventlogsManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(XtraFormWindowsEventlogsManager));
            this.xtraUCWindowsEventLogs1 = new Analogy.XtraUCWindowsEventLogs();
            this.SuspendLayout();
            // 
            // xtraUCWindowsEventLogs1
            // 
            this.xtraUCWindowsEventLogs1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraUCWindowsEventLogs1.Location = new System.Drawing.Point(0, 0);
            this.xtraUCWindowsEventLogs1.Name = "xtraUCWindowsEventLogs1";
            this.xtraUCWindowsEventLogs1.Size = new System.Drawing.Size(739, 466);
            this.xtraUCWindowsEventLogs1.TabIndex = 0;
            // 
            // XtraFormWindowsEventlogsManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(739, 466);
            this.Controls.Add(this.xtraUCWindowsEventLogs1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "XtraFormWindowsEventlogsManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Windows event logs";
            this.Load += new System.EventHandler(this.XtraFormWindowsEventlogsManager_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private XtraUCWindowsEventLogs xtraUCWindowsEventLogs1;
    }
}