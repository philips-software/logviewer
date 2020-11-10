using Analogy.Tools;

namespace Analogy.Forms
{
    partial class FileComparerForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FileComparerForm));
            this.logsComparerUC1 = new Analogy.Tools.LogsComparerUC();
            this.SuspendLayout();
            // 
            // logsComparerUC1
            // 
            this.logsComparerUC1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logsComparerUC1.Location = new System.Drawing.Point(0, 0);
            this.logsComparerUC1.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.logsComparerUC1.Name = "logsComparerUC1";
            this.logsComparerUC1.Size = new System.Drawing.Size(622, 453);
            this.logsComparerUC1.TabIndex = 0;
            // 
            // FileComparerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(622, 453);
            this.Controls.Add(this.logsComparerUC1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FileComparerForm";
            this.Text = "Logs comparer";
            this.Load += new System.EventHandler(this.FileComparerForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private LogsComparerUC logsComparerUC1;
    }
}