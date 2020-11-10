namespace Analogy.Forms
{
    partial class UserSettingsDataProvidersForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserSettingsDataProvidersForm));
            this.tabControlMain = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPageIIS = new DevExpress.XtraTab.XtraTabPage();
            this.labelControl10 = new DevExpress.XtraEditors.LabelControl();
            this.xtraTabPageJson = new DevExpress.XtraTab.XtraTabPage();
            this.labelControl12 = new DevExpress.XtraEditors.LabelControl();
            this.xtraTabPageXML = new DevExpress.XtraTab.XtraTabPage();
            this.labelControl13 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.tabControlMain)).BeginInit();
            this.tabControlMain.SuspendLayout();
            this.xtraTabPageIIS.SuspendLayout();
            this.xtraTabPageJson.SuspendLayout();
            this.xtraTabPageXML.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControlMain
            // 
            this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlMain.HeaderLocation = DevExpress.XtraTab.TabHeaderLocation.Left;
            this.tabControlMain.HeaderOrientation = DevExpress.XtraTab.TabOrientation.Horizontal;
            this.tabControlMain.Location = new System.Drawing.Point(0, 0);
            this.tabControlMain.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabControlMain.MinimumSize = new System.Drawing.Size(814, 382);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedTabPage = this.xtraTabPageIIS;
            this.tabControlMain.Size = new System.Drawing.Size(904, 627);
            this.tabControlMain.TabIndex = 0;
            this.tabControlMain.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPageIIS,
            this.xtraTabPageJson,
            this.xtraTabPageXML});
            // 
            // xtraTabPageIIS
            // 
            this.xtraTabPageIIS.Controls.Add(this.labelControl10);
            this.xtraTabPageIIS.ImageOptions.Image = global::Analogy.Properties.Resources.iis;
            this.xtraTabPageIIS.Name = "xtraTabPageIIS";
            this.xtraTabPageIIS.Size = new System.Drawing.Size(779, 620);
            this.xtraTabPageIIS.Text = "IIS Logs";
            // 
            // labelControl10
            // 
            this.labelControl10.Location = new System.Drawing.Point(21, 10);
            this.labelControl10.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl10.Name = "labelControl10";
            this.labelControl10.Size = new System.Drawing.Size(142, 16);
            this.labelControl10.TabIndex = 5;
            this.labelControl10.Text = "No special settings exists";
            // 
            // xtraTabPageJson
            // 
            this.xtraTabPageJson.Controls.Add(this.labelControl12);
            this.xtraTabPageJson.ImageOptions.Image = global::Analogy.Properties.Resources.jsonfile32x32;
            this.xtraTabPageJson.Name = "xtraTabPageJson";
            this.xtraTabPageJson.Size = new System.Drawing.Size(779, 620);
            this.xtraTabPageJson.Text = "Json Parser";
            // 
            // labelControl12
            // 
            this.labelControl12.Location = new System.Drawing.Point(13, 10);
            this.labelControl12.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl12.Name = "labelControl12";
            this.labelControl12.Size = new System.Drawing.Size(78, 16);
            this.labelControl12.TabIndex = 14;
            this.labelControl12.Text = "Coming soon.";
            // 
            // xtraTabPageXML
            // 
            this.xtraTabPageXML.Controls.Add(this.labelControl13);
            this.xtraTabPageXML.ImageOptions.Image = global::Analogy.Properties.Resources.xml32x32;
            this.xtraTabPageXML.Name = "xtraTabPageXML";
            this.xtraTabPageXML.Size = new System.Drawing.Size(779, 620);
            this.xtraTabPageXML.Text = "XML Parser";
            // 
            // labelControl13
            // 
            this.labelControl13.Location = new System.Drawing.Point(10, 10);
            this.labelControl13.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl13.Name = "labelControl13";
            this.labelControl13.Size = new System.Drawing.Size(78, 16);
            this.labelControl13.TabIndex = 14;
            this.labelControl13.Text = "Coming soon.";
            // 
            // UserSettingsDataProvidersForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(904, 627);
            this.Controls.Add(this.tabControlMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "UserSettingsDataProvidersForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Built-In Data Providers Settings";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.UserSettingsDataProvidersForm_FormClosing);
            this.Load += new System.EventHandler(this.UserSettingsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tabControlMain)).EndInit();
            this.tabControlMain.ResumeLayout(false);
            this.xtraTabPageIIS.ResumeLayout(false);
            this.xtraTabPageIIS.PerformLayout();
            this.xtraTabPageJson.ResumeLayout(false);
            this.xtraTabPageJson.PerformLayout();
            this.xtraTabPageXML.ResumeLayout(false);
            this.xtraTabPageXML.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraTab.XtraTabControl tabControlMain;
        private DevExpress.XtraTab.XtraTabPage xtraTabPageJson;
        private DevExpress.XtraTab.XtraTabPage xtraTabPageXML;
        private DevExpress.XtraEditors.LabelControl labelControl12;
        private DevExpress.XtraEditors.LabelControl labelControl13;
        private DevExpress.XtraTab.XtraTabPage xtraTabPageIIS;
        private DevExpress.XtraEditors.LabelControl labelControl10;
    }
}