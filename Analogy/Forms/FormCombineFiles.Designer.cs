namespace Analogy.Forms
{
    partial class FormCombineFiles
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCombineFiles));
            this.combineFilesUC1 = new Analogy.CombineFilesUC();
            this.SuspendLayout();
            // 
            // combineFilesUC1
            // 
            this.combineFilesUC1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.combineFilesUC1.Location = new System.Drawing.Point(0, 0);
            this.combineFilesUC1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.combineFilesUC1.Name = "combineFilesUC1";
            this.combineFilesUC1.Size = new System.Drawing.Size(1160, 619);
            this.combineFilesUC1.TabIndex = 0;
            // 
            // FormCombineFiles
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 22F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1160, 619);
            this.Controls.Add(this.combineFilesUC1);
            this.Font = new System.Drawing.Font("Times New Roman", 12F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.Name = "FormCombineFiles";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Combine Files";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FormCombineFiles_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private CombineFilesUC combineFilesUC1;
    }
}