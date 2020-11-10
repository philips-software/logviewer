
namespace Analogy.UserControls
{
    partial class ComponentDownloadInformationUC
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.picture = new System.Windows.Forms.PictureBox();
            this.lblTitle = new DevExpress.XtraEditors.LabelControl();
            this.lblCurrentVersion = new DevExpress.XtraEditors.LabelControl();
            this.lblLatestVersion = new DevExpress.XtraEditors.LabelControl();
            this.btnCheckNow = new DevExpress.XtraEditors.SimpleButton();
            this.btnDownload = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.picture)).BeginInit();
            this.SuspendLayout();
            // 
            // picture
            // 
            this.picture.Location = new System.Drawing.Point(2, 3);
            this.picture.Name = "picture";
            this.picture.Size = new System.Drawing.Size(56, 55);
            this.picture.TabIndex = 0;
            this.picture.TabStop = false;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblTitle.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Flat;
            this.lblTitle.Location = new System.Drawing.Point(64, 13);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(332, 32);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "title";
            // 
            // lblCurrentVersion
            // 
            this.lblCurrentVersion.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblCurrentVersion.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Flat;
            this.lblCurrentVersion.Location = new System.Drawing.Point(402, 13);
            this.lblCurrentVersion.Name = "lblCurrentVersion";
            this.lblCurrentVersion.Size = new System.Drawing.Size(195, 32);
            this.lblCurrentVersion.TabIndex = 3;
            this.lblCurrentVersion.Text = "Current Version:";
            // 
            // lblLatestVersion
            // 
            this.lblLatestVersion.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblLatestVersion.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Flat;
            this.lblLatestVersion.Location = new System.Drawing.Point(603, 13);
            this.lblLatestVersion.Name = "lblLatestVersion";
            this.lblLatestVersion.Size = new System.Drawing.Size(195, 32);
            this.lblLatestVersion.TabIndex = 4;
            this.lblLatestVersion.Text = "Latest Version: N/A";
            // 
            // btnCheckNow
            // 
            this.btnCheckNow.Location = new System.Drawing.Point(804, 13);
            this.btnCheckNow.Name = "btnCheckNow";
            this.btnCheckNow.Size = new System.Drawing.Size(106, 32);
            this.btnCheckNow.TabIndex = 5;
            this.btnCheckNow.Text = "Check Now";
            // 
            // btnDownload
            // 
            this.btnDownload.Location = new System.Drawing.Point(916, 13);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(137, 32);
            this.btnDownload.TabIndex = 6;
            this.btnDownload.Text = "Download Now";
            this.btnDownload.Visible = false;
            // 
            // ComponentDownloadInformationUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.btnDownload);
            this.Controls.Add(this.btnCheckNow);
            this.Controls.Add(this.lblLatestVersion);
            this.Controls.Add(this.lblCurrentVersion);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.picture);
            this.Name = "ComponentDownloadInformationUC";
            this.Size = new System.Drawing.Size(1056, 97);
            this.Load += new System.EventHandler(this.ComponentDownloadInformationUC_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picture)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picture;
        private DevExpress.XtraEditors.LabelControl lblTitle;
        private DevExpress.XtraEditors.LabelControl lblCurrentVersion;
        private DevExpress.XtraEditors.LabelControl lblLatestVersion;
        private DevExpress.XtraEditors.SimpleButton btnCheckNow;
        private DevExpress.XtraEditors.SimpleButton btnDownload;
    }
}
