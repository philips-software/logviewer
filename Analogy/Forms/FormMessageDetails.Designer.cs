namespace Analogy.Forms
{
    partial class FormMessageDetails
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMessageDetails));
            this.spltCMain = new System.Windows.Forms.SplitContainer();
            this.btnClose = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.spltCMain)).BeginInit();
            this.spltCMain.Panel2.SuspendLayout();
            this.spltCMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // spltCMain
            // 
            this.spltCMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spltCMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.spltCMain.Location = new System.Drawing.Point(0, 0);
            this.spltCMain.Name = "spltCMain";
            this.spltCMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // spltCMain.Panel2
            // 
            this.spltCMain.Panel2.Controls.Add(this.btnClose);
            this.spltCMain.Size = new System.Drawing.Size(918, 582);
            this.spltCMain.SplitterDistance = 537;
            this.spltCMain.TabIndex = 0;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(803, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(103, 33);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // FormMessageDetails
            // 
            this.AcceptButton = this.btnClose;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(918, 582);
            this.Controls.Add(this.spltCMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormMessageDetails";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Details";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FormMessageDetails_Load);
            this.spltCMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.spltCMain)).EndInit();
            this.spltCMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer spltCMain;
        private DevExpress.XtraEditors.SimpleButton btnClose;
    }
}