namespace Analogy.Forms
{
    partial class FirstTimeRunForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FirstTimeRunForm));
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.tpWelcome = new DevExpress.XtraTab.XtraTabPage();
            this.btnNext1 = new DevExpress.XtraEditors.SimpleButton();
            this.memoEdit1 = new DevExpress.XtraEditors.MemoEdit();
            this.pictureEdit1 = new DevExpress.XtraEditors.PictureEdit();
            this.tpRibbon = new DevExpress.XtraTab.XtraTabPage();
            this.pictureEdit2 = new DevExpress.XtraEditors.PictureEdit();
            this.btnBack1 = new DevExpress.XtraEditors.SimpleButton();
            this.btnNext2 = new DevExpress.XtraEditors.SimpleButton();
            this.memoEdit2 = new DevExpress.XtraEditors.MemoEdit();
            this.tpSettings = new DevExpress.XtraTab.XtraTabPage();
            this.btnBack2 = new DevExpress.XtraEditors.SimpleButton();
            this.btnNext3 = new DevExpress.XtraEditors.SimpleButton();
            this.memoEdit3 = new DevExpress.XtraEditors.MemoEdit();
            this.pictureEdit3 = new DevExpress.XtraEditors.PictureEdit();
            this.tpDataProviders = new DevExpress.XtraTab.XtraTabPage();
            this.chkLstDataProviderStatus = new DevExpress.XtraEditors.CheckedListBoxControl();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.btnBack3 = new DevExpress.XtraEditors.SimpleButton();
            this.btnNext4 = new DevExpress.XtraEditors.SimpleButton();
            this.lblDataProviders = new DevExpress.XtraEditors.LabelControl();
            this.tpFinish = new DevExpress.XtraTab.XtraTabPage();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.btnOK = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.hyperLinkEdit2 = new DevExpress.XtraEditors.HyperLinkEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.hyperLinkEdit1 = new DevExpress.XtraEditors.HyperLinkEdit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.tpWelcome.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.memoEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).BeginInit();
            this.tpRibbon.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoEdit2.Properties)).BeginInit();
            this.tpSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.memoEdit3.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit3.Properties)).BeginInit();
            this.tpDataProviders.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkLstDataProviderStatus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.tpFinish.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.hyperLinkEdit2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.hyperLinkEdit1.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControl1.Location = new System.Drawing.Point(0, 0);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.tpWelcome;
            this.xtraTabControl1.ShowTabHeader = DevExpress.Utils.DefaultBoolean.False;
            this.xtraTabControl1.Size = new System.Drawing.Size(1121, 396);
            this.xtraTabControl1.TabIndex = 1;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.tpWelcome,
            this.tpRibbon,
            this.tpSettings,
            this.tpDataProviders,
            this.tpFinish});
            // 
            // tpWelcome
            // 
            this.tpWelcome.Controls.Add(this.btnNext1);
            this.tpWelcome.Controls.Add(this.memoEdit1);
            this.tpWelcome.Controls.Add(this.pictureEdit1);
            this.tpWelcome.Name = "tpWelcome";
            this.tpWelcome.Size = new System.Drawing.Size(1114, 389);
            this.tpWelcome.Text = "Welcome";
            // 
            // btnNext1
            // 
            this.btnNext1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNext1.Location = new System.Drawing.Point(996, 325);
            this.btnNext1.Name = "btnNext1";
            this.btnNext1.Size = new System.Drawing.Size(115, 34);
            this.btnNext1.TabIndex = 2;
            this.btnNext1.Tag = "0";
            this.btnNext1.Text = "Next";
            this.btnNext1.Click += new System.EventHandler(this.SetNext);
            // 
            // memoEdit1
            // 
            this.memoEdit1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.memoEdit1.EditValue = resources.GetString("memoEdit1.EditValue");
            this.memoEdit1.Location = new System.Drawing.Point(395, 3);
            this.memoEdit1.Name = "memoEdit1";
            this.memoEdit1.Size = new System.Drawing.Size(716, 316);
            this.memoEdit1.TabIndex = 1;
            // 
            // pictureEdit1
            // 
            this.pictureEdit1.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureEdit1.EditValue = global::Analogy.Properties.Resources.AnalogyBanner512x512;
            this.pictureEdit1.Location = new System.Drawing.Point(0, 0);
            this.pictureEdit1.Name = "pictureEdit1";
            this.pictureEdit1.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
            this.pictureEdit1.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Zoom;
            this.pictureEdit1.Size = new System.Drawing.Size(389, 389);
            this.pictureEdit1.TabIndex = 0;
            // 
            // tpRibbon
            // 
            this.tpRibbon.Controls.Add(this.pictureEdit2);
            this.tpRibbon.Controls.Add(this.btnBack1);
            this.tpRibbon.Controls.Add(this.btnNext2);
            this.tpRibbon.Controls.Add(this.memoEdit2);
            this.tpRibbon.Name = "tpRibbon";
            this.tpRibbon.Size = new System.Drawing.Size(1114, 389);
            this.tpRibbon.Text = "Application Ribbon";
            // 
            // pictureEdit2
            // 
            this.pictureEdit2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureEdit2.EditValue = global::Analogy.Properties.Resources.Ribbon2;
            this.pictureEdit2.Location = new System.Drawing.Point(0, 245);
            this.pictureEdit2.Name = "pictureEdit2";
            this.pictureEdit2.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
            this.pictureEdit2.Size = new System.Drawing.Size(1114, 101);
            this.pictureEdit2.TabIndex = 1;
            // 
            // btnBack1
            // 
            this.btnBack1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBack1.Location = new System.Drawing.Point(875, 352);
            this.btnBack1.Name = "btnBack1";
            this.btnBack1.Size = new System.Drawing.Size(115, 34);
            this.btnBack1.TabIndex = 4;
            this.btnBack1.Tag = "1";
            this.btnBack1.Text = "Back";
            this.btnBack1.Click += new System.EventHandler(this.SetBack);
            // 
            // btnNext2
            // 
            this.btnNext2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNext2.Location = new System.Drawing.Point(996, 352);
            this.btnNext2.Name = "btnNext2";
            this.btnNext2.Size = new System.Drawing.Size(115, 34);
            this.btnNext2.TabIndex = 3;
            this.btnNext2.Tag = "1";
            this.btnNext2.Text = "Next";
            this.btnNext2.Click += new System.EventHandler(this.SetNext);
            // 
            // memoEdit2
            // 
            this.memoEdit2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.memoEdit2.EditValue = resources.GetString("memoEdit2.EditValue");
            this.memoEdit2.Location = new System.Drawing.Point(0, 0);
            this.memoEdit2.Name = "memoEdit2";
            this.memoEdit2.Size = new System.Drawing.Size(1114, 389);
            this.memoEdit2.TabIndex = 2;
            // 
            // tpSettings
            // 
            this.tpSettings.Controls.Add(this.btnBack2);
            this.tpSettings.Controls.Add(this.btnNext3);
            this.tpSettings.Controls.Add(this.memoEdit3);
            this.tpSettings.Controls.Add(this.pictureEdit3);
            this.tpSettings.Name = "tpSettings";
            this.tpSettings.Size = new System.Drawing.Size(1114, 389);
            this.tpSettings.Text = "Settings";
            // 
            // btnBack2
            // 
            this.btnBack2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBack2.Location = new System.Drawing.Point(875, 326);
            this.btnBack2.Name = "btnBack2";
            this.btnBack2.Size = new System.Drawing.Size(115, 34);
            this.btnBack2.TabIndex = 8;
            this.btnBack2.Tag = "2";
            this.btnBack2.Text = "Back";
            this.btnBack2.Click += new System.EventHandler(this.SetBack);
            // 
            // btnNext3
            // 
            this.btnNext3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNext3.Location = new System.Drawing.Point(996, 326);
            this.btnNext3.Name = "btnNext3";
            this.btnNext3.Size = new System.Drawing.Size(115, 34);
            this.btnNext3.TabIndex = 7;
            this.btnNext3.Tag = "2";
            this.btnNext3.Text = "Next";
            this.btnNext3.Click += new System.EventHandler(this.SetNext);
            // 
            // memoEdit3
            // 
            this.memoEdit3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.memoEdit3.EditValue = resources.GetString("memoEdit3.EditValue");
            this.memoEdit3.Location = new System.Drawing.Point(420, 3);
            this.memoEdit3.Name = "memoEdit3";
            this.memoEdit3.Size = new System.Drawing.Size(688, 317);
            this.memoEdit3.TabIndex = 6;
            // 
            // pictureEdit3
            // 
            this.pictureEdit3.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureEdit3.EditValue = global::Analogy.Properties.Resources.SettingsMenu;
            this.pictureEdit3.Location = new System.Drawing.Point(0, 0);
            this.pictureEdit3.Name = "pictureEdit3";
            this.pictureEdit3.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
            this.pictureEdit3.Size = new System.Drawing.Size(414, 389);
            this.pictureEdit3.TabIndex = 5;
            // 
            // tpDataProviders
            // 
            this.tpDataProviders.Controls.Add(this.chkLstDataProviderStatus);
            this.tpDataProviders.Controls.Add(this.panelControl1);
            this.tpDataProviders.Controls.Add(this.lblDataProviders);
            this.tpDataProviders.Name = "tpDataProviders";
            this.tpDataProviders.Size = new System.Drawing.Size(1114, 389);
            this.tpDataProviders.Text = "Data Providers";
            // 
            // chkLstDataProviderStatus
            // 
            this.chkLstDataProviderStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkLstDataProviderStatus.ItemHeight = 62;
            this.chkLstDataProviderStatus.Location = new System.Drawing.Point(0, 36);
            this.chkLstDataProviderStatus.Name = "chkLstDataProviderStatus";
            this.chkLstDataProviderStatus.Size = new System.Drawing.Size(1114, 311);
            this.chkLstDataProviderStatus.TabIndex = 14;
            this.chkLstDataProviderStatus.ItemCheck += new DevExpress.XtraEditors.Controls.ItemCheckEventHandler(this.chkLstDataProviderStatus_ItemCheck);
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.btnBack3);
            this.panelControl1.Controls.Add(this.btnNext4);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl1.Location = new System.Drawing.Point(0, 347);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1114, 42);
            this.panelControl1.TabIndex = 15;
            // 
            // btnBack3
            // 
            this.btnBack3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBack3.Location = new System.Drawing.Point(874, 5);
            this.btnBack3.Name = "btnBack3";
            this.btnBack3.Size = new System.Drawing.Size(115, 34);
            this.btnBack3.TabIndex = 10;
            this.btnBack3.Tag = "3";
            this.btnBack3.Text = "Back";
            this.btnBack3.Click += new System.EventHandler(this.SetBack);
            // 
            // btnNext4
            // 
            this.btnNext4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNext4.Location = new System.Drawing.Point(995, 5);
            this.btnNext4.Name = "btnNext4";
            this.btnNext4.Size = new System.Drawing.Size(115, 34);
            this.btnNext4.TabIndex = 9;
            this.btnNext4.Tag = "3";
            this.btnNext4.Text = "Next";
            this.btnNext4.Click += new System.EventHandler(this.SetNext);
            // 
            // lblDataProviders
            // 
            this.lblDataProviders.AutoEllipsis = true;
            this.lblDataProviders.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Vertical;
            this.lblDataProviders.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblDataProviders.Location = new System.Drawing.Point(0, 0);
            this.lblDataProviders.Margin = new System.Windows.Forms.Padding(5);
            this.lblDataProviders.Name = "lblDataProviders";
            this.lblDataProviders.Padding = new System.Windows.Forms.Padding(3, 10, 10, 10);
            this.lblDataProviders.Size = new System.Drawing.Size(1114, 36);
            this.lblDataProviders.TabIndex = 13;
            this.lblDataProviders.Text = "A default setup of data providers was set.  You can change this setup by enabling" +
    "/disabling any data providers. You can change the selection later on in the sett" +
    "ings window.";
            // 
            // tpFinish
            // 
            this.tpFinish.Controls.Add(this.labelControl3);
            this.tpFinish.Controls.Add(this.btnOK);
            this.tpFinish.Controls.Add(this.labelControl2);
            this.tpFinish.Controls.Add(this.hyperLinkEdit2);
            this.tpFinish.Controls.Add(this.labelControl1);
            this.tpFinish.Controls.Add(this.hyperLinkEdit1);
            this.tpFinish.Name = "tpFinish";
            this.tpFinish.Size = new System.Drawing.Size(1114, 389);
            this.tpFinish.Text = "Done";
            // 
            // labelControl3
            // 
            this.labelControl3.AutoEllipsis = true;
            this.labelControl3.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Vertical;
            this.labelControl3.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelControl3.Location = new System.Drawing.Point(0, 0);
            this.labelControl3.Margin = new System.Windows.Forms.Padding(5);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Padding = new System.Windows.Forms.Padding(3, 10, 10, 10);
            this.labelControl3.Size = new System.Drawing.Size(1114, 36);
            this.labelControl3.TabIndex = 14;
            this.labelControl3.Text = "You can visit the Github repositories for more information:";
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(996, 325);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(115, 34);
            this.btnOK.TabIndex = 8;
            this.btnOK.Tag = "2";
            this.btnOK.Text = "OK";
            this.btnOK.Click += new System.EventHandler(this.AcceptAndClose);
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(13, 77);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(238, 16);
            this.labelControl2.TabIndex = 3;
            this.labelControl2.Text = "For a complete list of data providers visit:";
            // 
            // hyperLinkEdit2
            // 
            this.hyperLinkEdit2.EditValue = "https://github.com/Analogy-LogViewer/Analogy";
            this.hyperLinkEdit2.Location = new System.Drawing.Point(334, 74);
            this.hyperLinkEdit2.Name = "hyperLinkEdit2";
            this.hyperLinkEdit2.Size = new System.Drawing.Size(774, 22);
            this.hyperLinkEdit2.TabIndex = 2;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(13, 50);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(227, 16);
            this.labelControl1.TabIndex = 1;
            this.labelControl1.Text = "Primary Analogy Log Viewer repository:";
            // 
            // hyperLinkEdit1
            // 
            this.hyperLinkEdit1.EditValue = "https://github.com/Analogy-LogViewer/Analogy.LogViewer";
            this.hyperLinkEdit1.Location = new System.Drawing.Point(334, 47);
            this.hyperLinkEdit1.Name = "hyperLinkEdit1";
            this.hyperLinkEdit1.Size = new System.Drawing.Size(774, 22);
            this.hyperLinkEdit1.TabIndex = 0;
            // 
            // FirstTimeRunForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1121, 396);
            this.Controls.Add(this.xtraTabControl1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FirstTimeRunForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Analogy Log Viewer: First Run";
            this.Load += new System.EventHandler(this.FirstTimeRunForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.tpWelcome.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.memoEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).EndInit();
            this.tpRibbon.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoEdit2.Properties)).EndInit();
            this.tpSettings.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.memoEdit3.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit3.Properties)).EndInit();
            this.tpDataProviders.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chkLstDataProviderStatus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.tpFinish.ResumeLayout(false);
            this.tpFinish.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.hyperLinkEdit2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.hyperLinkEdit1.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraTab.XtraTabPage tpWelcome;
        private DevExpress.XtraTab.XtraTabPage tpDataProviders;
        private DevExpress.XtraEditors.PictureEdit pictureEdit1;
        private DevExpress.XtraEditors.SimpleButton btnNext1;
        private DevExpress.XtraEditors.MemoEdit memoEdit1;
        private DevExpress.XtraTab.XtraTabPage tpRibbon;
        private DevExpress.XtraEditors.PictureEdit pictureEdit2;
        private DevExpress.XtraEditors.SimpleButton btnNext2;
        private DevExpress.XtraEditors.MemoEdit memoEdit2;
        private DevExpress.XtraEditors.SimpleButton btnBack1;
        private DevExpress.XtraTab.XtraTabPage tpSettings;
        private DevExpress.XtraEditors.SimpleButton btnBack2;
        private DevExpress.XtraEditors.SimpleButton btnNext3;
        private DevExpress.XtraEditors.MemoEdit memoEdit3;
        private DevExpress.XtraEditors.PictureEdit pictureEdit3;
        private DevExpress.XtraTab.XtraTabPage tpFinish;
        private DevExpress.XtraEditors.SimpleButton btnOK;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.HyperLinkEdit hyperLinkEdit2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.HyperLinkEdit hyperLinkEdit1;
        private DevExpress.XtraEditors.CheckedListBoxControl chkLstDataProviderStatus;
        private DevExpress.XtraEditors.LabelControl lblDataProviders;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton btnBack3;
        private DevExpress.XtraEditors.SimpleButton btnNext4;
        private DevExpress.XtraEditors.LabelControl labelControl3;
    }
}