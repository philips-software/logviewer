namespace Analogy.Forms
{
    partial class AnalogyExclude
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AnalogyExclude));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.checkedListBoxControl1 = new DevExpress.XtraEditors.CheckedListBoxControl();
            this.PnlBottom = new System.Windows.Forms.Panel();
            this.sBtnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.sBtnOk = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.checkedListBoxControl1)).BeginInit();
            this.PnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(4);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.simpleButton2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.checkedListBoxControl1);
            this.splitContainer1.Panel2.Controls.Add(this.PnlBottom);
            this.splitContainer1.Size = new System.Drawing.Size(1132, 640);
            this.splitContainer1.SplitterDistance = 55;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 0;
            // 
            // simpleButton2
            // 
            this.simpleButton2.Location = new System.Drawing.Point(1, 2);
            this.simpleButton2.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(114, 26);
            this.simpleButton2.TabIndex = 9;
            this.simpleButton2.Text = "Deselect All";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // checkedListBoxControl1
            // 
            this.checkedListBoxControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkedListBoxControl1.ItemAutoHeight = true;
            this.checkedListBoxControl1.Location = new System.Drawing.Point(0, 0);
            this.checkedListBoxControl1.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.checkedListBoxControl1.Name = "checkedListBoxControl1";
            this.checkedListBoxControl1.Size = new System.Drawing.Size(1132, 561);
            this.checkedListBoxControl1.TabIndex = 2;
            // 
            // PnlBottom
            // 
            this.PnlBottom.Controls.Add(this.sBtnCancel);
            this.PnlBottom.Controls.Add(this.sBtnOk);
            this.PnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.PnlBottom.Location = new System.Drawing.Point(0, 561);
            this.PnlBottom.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.PnlBottom.Name = "PnlBottom";
            this.PnlBottom.Size = new System.Drawing.Size(1132, 19);
            this.PnlBottom.TabIndex = 1;
            // 
            // sBtnCancel
            // 
            this.sBtnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.sBtnCancel.Location = new System.Drawing.Point(1092, 2);
            this.sBtnCancel.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.sBtnCancel.Name = "sBtnCancel";
            this.sBtnCancel.Size = new System.Drawing.Size(37, 11);
            this.sBtnCancel.TabIndex = 8;
            this.sBtnCancel.Text = "Cancel";
            this.sBtnCancel.Click += new System.EventHandler(this.sBtnCancel_Click);
            // 
            // sBtnOk
            // 
            this.sBtnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.sBtnOk.Location = new System.Drawing.Point(1053, 2);
            this.sBtnOk.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.sBtnOk.Name = "sBtnOk";
            this.sBtnOk.Size = new System.Drawing.Size(38, 11);
            this.sBtnOk.TabIndex = 7;
            this.sBtnOk.Text = "OK";
            this.sBtnOk.Click += new System.EventHandler(this.sBtnOk_Click);
            // 
            // AnalogyExclude
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1132, 640);
            this.Controls.Add(this.splitContainer1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "AnalogyExclude";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Additional exclusion list";
            this.Load += new System.EventHandler(this.AnalogyExclude_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.checkedListBoxControl1)).EndInit();
            this.PnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel PnlBottom;
        private DevExpress.XtraEditors.SimpleButton sBtnCancel;
        private DevExpress.XtraEditors.SimpleButton sBtnOk;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.CheckedListBoxControl checkedListBoxControl1;
    }
}