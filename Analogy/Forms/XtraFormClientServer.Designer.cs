namespace Analogy.Forms
{
    partial class XtraFormClientServer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(XtraFormClientServer));
            this.sBtnAdd = new DevExpress.XtraEditors.SimpleButton();
            this.lblPath = new DevExpress.XtraEditors.LabelControl();
            this.txtbPath = new System.Windows.Forms.TextBox();
            this.sBtnTest = new DevExpress.XtraEditors.SimpleButton();
            this.rBtnNetwork = new System.Windows.Forms.RadioButton();
            this.rBtnLocal = new System.Windows.Forms.RadioButton();
            this.txtDisplayName = new System.Windows.Forms.TextBox();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.SuspendLayout();
            // 
            // sBtnAdd
            // 
            this.sBtnAdd.Location = new System.Drawing.Point(468, 94);
            this.sBtnAdd.Name = "sBtnAdd";
            this.sBtnAdd.Size = new System.Drawing.Size(163, 58);
            this.sBtnAdd.TabIndex = 0;
            this.sBtnAdd.Text = "Add";
            this.sBtnAdd.Click += new System.EventHandler(this.sBtnAdd_Click);
            // 
            // lblPath
            // 
            this.lblPath.Location = new System.Drawing.Point(12, 57);
            this.lblPath.Name = "lblPath";
            this.lblPath.Size = new System.Drawing.Size(78, 16);
            this.lblPath.TabIndex = 4;
            this.lblPath.Text = "IP/Hostname:";
            // 
            // txtbPath
            // 
            this.txtbPath.Location = new System.Drawing.Point(111, 56);
            this.txtbPath.Name = "txtbPath";
            this.txtbPath.Size = new System.Drawing.Size(332, 23);
            this.txtbPath.TabIndex = 5;
            // 
            // sBtnTest
            // 
            this.sBtnTest.Location = new System.Drawing.Point(468, 20);
            this.sBtnTest.Name = "sBtnTest";
            this.sBtnTest.Size = new System.Drawing.Size(163, 58);
            this.sBtnTest.TabIndex = 6;
            this.sBtnTest.Text = "Test";
            this.sBtnTest.Click += new System.EventHandler(this.sBtnTest_Click);
            // 
            // rBtnNetwork
            // 
            this.rBtnNetwork.AutoSize = true;
            this.rBtnNetwork.Location = new System.Drawing.Point(12, 12);
            this.rBtnNetwork.Name = "rBtnNetwork";
            this.rBtnNetwork.Size = new System.Drawing.Size(112, 21);
            this.rBtnNetwork.TabIndex = 9;
            this.rBtnNetwork.Text = "Network Path";
            this.rBtnNetwork.UseVisualStyleBackColor = true;
            this.rBtnNetwork.CheckedChanged += new System.EventHandler(this.radioButtons_CheckedChanged);
            // 
            // rBtnLocal
            // 
            this.rBtnLocal.AutoSize = true;
            this.rBtnLocal.Location = new System.Drawing.Point(143, 12);
            this.rBtnLocal.Name = "rBtnLocal";
            this.rBtnLocal.Size = new System.Drawing.Size(101, 21);
            this.rBtnLocal.TabIndex = 10;
            this.rBtnLocal.Text = "Local Folder";
            this.rBtnLocal.UseVisualStyleBackColor = true;
            this.rBtnLocal.CheckedChanged += new System.EventHandler(this.radioButtons_CheckedChanged);
            // 
            // txtDisplayName
            // 
            this.txtDisplayName.Location = new System.Drawing.Point(111, 87);
            this.txtDisplayName.Name = "txtDisplayName";
            this.txtDisplayName.Size = new System.Drawing.Size(332, 23);
            this.txtDisplayName.TabIndex = 12;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(12, 88);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(82, 16);
            this.labelControl1.TabIndex = 11;
            this.labelControl1.Text = "Display Name:";
            // 
            // XtraFormClientServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(655, 190);
            this.Controls.Add(this.txtDisplayName);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.rBtnLocal);
            this.Controls.Add(this.rBtnNetwork);
            this.Controls.Add(this.sBtnTest);
            this.Controls.Add(this.txtbPath);
            this.Controls.Add(this.lblPath);
            this.Controls.Add(this.sBtnAdd);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "XtraFormClientServer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Other File Locations";
            this.Load += new System.EventHandler(this.XtraFormClientServer_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton sBtnAdd;
        private DevExpress.XtraEditors.LabelControl lblPath;
        private System.Windows.Forms.TextBox txtbPath;
        private DevExpress.XtraEditors.SimpleButton sBtnTest;
        private System.Windows.Forms.RadioButton rBtnNetwork;
        private System.Windows.Forms.RadioButton rBtnLocal;
        private System.Windows.Forms.TextBox txtDisplayName;
        private DevExpress.XtraEditors.LabelControl labelControl1;
    }
}