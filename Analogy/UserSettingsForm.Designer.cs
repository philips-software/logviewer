﻿namespace Philips.Analogy
{
    partial class UserSettingsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserSettingsForm));
            this.tabControlMain = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
            this.gcFiles = new DevExpress.XtraEditors.GroupControl();
            this.tsFileCaching = new DevExpress.XtraEditors.ToggleSwitch();
            this.gcFiltering = new DevExpress.XtraEditors.GroupControl();
            this.checkEditSearchAlsoInSourceAndModule = new DevExpress.XtraEditors.CheckEdit();
            this.chkEditPaging = new DevExpress.XtraEditors.CheckEdit();
            this.nudPageLength = new System.Windows.Forms.NumericUpDown();
            this.tsErrorLevelAsDefault = new DevExpress.XtraEditors.ToggleSwitch();
            this.tsFilteringExclude = new DevExpress.XtraEditors.ToggleSwitch();
            this.tsAutoComplete = new DevExpress.XtraEditors.ToggleSwitch();
            this.xtraTabPage2 = new DevExpress.XtraTab.XtraTabPage();
            this.tsHistory = new DevExpress.XtraEditors.ToggleSwitch();
            this.tpLookAndFeel = new DevExpress.XtraTab.XtraTabPage();
            this.tsStartupRibbonMinimized = new DevExpress.XtraEditors.ToggleSwitch();
            this.tsSimpleMode = new DevExpress.XtraEditors.ToggleSwitch();
            this.tpStatistics = new DevExpress.XtraTab.XtraTabPage();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.lblOpenedFiles = new DevExpress.XtraEditors.LabelControl();
            this.lblRunningTime = new DevExpress.XtraEditors.LabelControl();
            this.lblLaunchCount = new DevExpress.XtraEditors.LabelControl();
            this.btnClearStatistics = new DevExpress.XtraEditors.SimpleButton();
            this.tsUserStatistics = new DevExpress.XtraEditors.ToggleSwitch();
            this.xtraTabPage3 = new DevExpress.XtraTab.XtraTabPage();
            this.tswitchExtensionsStartup = new DevExpress.XtraEditors.ToggleSwitch();
            this.chklItems = new System.Windows.Forms.CheckedListBox();
            this.xtPage = new DevExpress.XtraTab.XtraTabPage();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.xTabMRU = new DevExpress.XtraTab.XtraTabPage();
            this.lblRecent = new DevExpress.XtraEditors.LabelControl();
            this.nudRecent = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.tabControlMain)).BeginInit();
            this.tabControlMain.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcFiles)).BeginInit();
            this.gcFiles.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tsFileCaching.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcFiltering)).BeginInit();
            this.gcFiltering.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditSearchAlsoInSourceAndModule.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkEditPaging.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPageLength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tsErrorLevelAsDefault.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tsFilteringExclude.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tsAutoComplete.Properties)).BeginInit();
            this.xtraTabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tsHistory.Properties)).BeginInit();
            this.tpLookAndFeel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tsStartupRibbonMinimized.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tsSimpleMode.Properties)).BeginInit();
            this.tpStatistics.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tsUserStatistics.Properties)).BeginInit();
            this.xtraTabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tswitchExtensionsStartup.Properties)).BeginInit();
            this.xtPage.SuspendLayout();
            this.xTabMRU.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudRecent)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControlMain
            // 
            this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlMain.HeaderLocation = DevExpress.XtraTab.TabHeaderLocation.Left;
            this.tabControlMain.HeaderOrientation = DevExpress.XtraTab.TabOrientation.Horizontal;
            this.tabControlMain.Location = new System.Drawing.Point(0, 0);
            this.tabControlMain.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabControlMain.MinimumSize = new System.Drawing.Size(698, 310);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedTabPage = this.xtraTabPage1;
            this.tabControlMain.Size = new System.Drawing.Size(804, 438);
            this.tabControlMain.TabIndex = 0;
            this.tabControlMain.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage1,
            this.xtraTabPage2,
            this.tpLookAndFeel,
            this.tpStatistics,
            this.xtraTabPage3,
            this.xtPage,
            this.xTabMRU});
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.gcFiles);
            this.xtraTabPage1.Controls.Add(this.gcFiltering);
            this.xtraTabPage1.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("xtraTabPage1.ImageOptions.Image")));
            this.xtraTabPage1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.xtraTabPage1.Name = "xtraTabPage1";
            this.xtraTabPage1.Size = new System.Drawing.Size(654, 432);
            this.xtraTabPage1.Text = "Filtering and Files";
            // 
            // gcFiles
            // 
            this.gcFiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcFiles.Controls.Add(this.tsFileCaching);
            this.gcFiles.Location = new System.Drawing.Point(3, 227);
            this.gcFiles.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gcFiles.Name = "gcFiles";
            this.gcFiles.Size = new System.Drawing.Size(652, 201);
            this.gcFiles.TabIndex = 4;
            this.gcFiles.Text = "Files";
            // 
            // tsFileCaching
            // 
            this.tsFileCaching.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tsFileCaching.EditValue = true;
            this.tsFileCaching.Location = new System.Drawing.Point(4, 30);
            this.tsFileCaching.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tsFileCaching.Name = "tsFileCaching";
            this.tsFileCaching.Properties.OffText = "Don\'t use Caching of loaded Files";
            this.tsFileCaching.Properties.OnText = "Use Caching of loaded Files";
            this.tsFileCaching.Size = new System.Drawing.Size(633, 24);
            this.tsFileCaching.TabIndex = 1;
            this.tsFileCaching.Toggled += new System.EventHandler(this.tsFileCaching_Toggled);
            // 
            // gcFiltering
            // 
            this.gcFiltering.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcFiltering.Controls.Add(this.checkEditSearchAlsoInSourceAndModule);
            this.gcFiltering.Controls.Add(this.chkEditPaging);
            this.gcFiltering.Controls.Add(this.nudPageLength);
            this.gcFiltering.Controls.Add(this.tsErrorLevelAsDefault);
            this.gcFiltering.Controls.Add(this.tsFilteringExclude);
            this.gcFiltering.Controls.Add(this.tsAutoComplete);
            this.gcFiltering.Location = new System.Drawing.Point(3, 2);
            this.gcFiltering.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gcFiltering.Name = "gcFiltering";
            this.gcFiltering.Size = new System.Drawing.Size(652, 221);
            this.gcFiltering.TabIndex = 3;
            this.gcFiltering.Text = "Filtering and Search";
            // 
            // checkEditSearchAlsoInSourceAndModule
            // 
            this.checkEditSearchAlsoInSourceAndModule.Location = new System.Drawing.Point(5, 164);
            this.checkEditSearchAlsoInSourceAndModule.Name = "checkEditSearchAlsoInSourceAndModule";
            this.checkEditSearchAlsoInSourceAndModule.Properties.Caption = "Search text also in Source and Module/Process columns";
            this.checkEditSearchAlsoInSourceAndModule.Size = new System.Drawing.Size(288, 19);
            this.checkEditSearchAlsoInSourceAndModule.TabIndex = 6;
            this.checkEditSearchAlsoInSourceAndModule.CheckedChanged += new System.EventHandler(this.checkEditSearchAlsoInSourceAndModule_CheckedChanged);
            // 
            // chkEditPaging
            // 
            this.chkEditPaging.Location = new System.Drawing.Point(5, 131);
            this.chkEditPaging.Name = "chkEditPaging";
            this.chkEditPaging.Properties.Caption = "Enable Paging (number of row per page):";
            this.chkEditPaging.Size = new System.Drawing.Size(288, 19);
            this.chkEditPaging.TabIndex = 5;
            this.chkEditPaging.CheckedChanged += new System.EventHandler(this.chkEditPaging_CheckedChanged);
            // 
            // nudPageLength
            // 
            this.nudPageLength.Location = new System.Drawing.Point(395, 132);
            this.nudPageLength.Maximum = new decimal(new int[] {
            1874919424,
            2328306,
            0,
            0});
            this.nudPageLength.Minimum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.nudPageLength.Name = "nudPageLength";
            this.nudPageLength.Size = new System.Drawing.Size(232, 21);
            this.nudPageLength.TabIndex = 4;
            this.nudPageLength.Value = new decimal(new int[] {
            200000,
            0,
            0,
            0});
            this.nudPageLength.ValueChanged += new System.EventHandler(this.nudPageLength_ValueChanged);
            // 
            // tsErrorLevelAsDefault
            // 
            this.tsErrorLevelAsDefault.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tsErrorLevelAsDefault.Location = new System.Drawing.Point(4, 58);
            this.tsErrorLevelAsDefault.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tsErrorLevelAsDefault.Name = "tsErrorLevelAsDefault";
            this.tsErrorLevelAsDefault.Properties.OffText = "Don\'t filter logs on Load";
            this.tsErrorLevelAsDefault.Properties.OnText = "Start logs with Error and Critical  level as default filtering";
            this.tsErrorLevelAsDefault.Size = new System.Drawing.Size(633, 24);
            this.tsErrorLevelAsDefault.TabIndex = 3;
            this.tsErrorLevelAsDefault.Toggled += new System.EventHandler(this.tsErrorLevelAsDefault_Toggled);
            // 
            // tsFilteringExclude
            // 
            this.tsFilteringExclude.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tsFilteringExclude.Location = new System.Drawing.Point(4, 30);
            this.tsFilteringExclude.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tsFilteringExclude.Name = "tsFilteringExclude";
            this.tsFilteringExclude.Properties.OffText = "Don\'t save excluded filtering text upon exit";
            this.tsFilteringExclude.Properties.OnText = "Save excluded filtering text for next startup";
            this.tsFilteringExclude.Size = new System.Drawing.Size(633, 24);
            this.tsFilteringExclude.TabIndex = 1;
            this.tsFilteringExclude.Toggled += new System.EventHandler(this.tsFilteringExclude_Toggled);
            // 
            // tsAutoComplete
            // 
            this.tsAutoComplete.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tsAutoComplete.Enabled = false;
            this.tsAutoComplete.Location = new System.Drawing.Point(4, 85);
            this.tsAutoComplete.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tsAutoComplete.Name = "tsAutoComplete";
            this.tsAutoComplete.Properties.OffText = "Don\'t save auto completion List";
            this.tsAutoComplete.Properties.OnText = "Save excluded filtering text for next startup";
            this.tsAutoComplete.Size = new System.Drawing.Size(633, 24);
            this.tsAutoComplete.TabIndex = 2;
            // 
            // xtraTabPage2
            // 
            this.xtraTabPage2.Controls.Add(this.tsHistory);
            this.xtraTabPage2.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("xtraTabPage2.ImageOptions.Image")));
            this.xtraTabPage2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.xtraTabPage2.Name = "xtraTabPage2";
            this.xtraTabPage2.Size = new System.Drawing.Size(654, 432);
            this.xtraTabPage2.Text = "History";
            // 
            // tsHistory
            // 
            this.tsHistory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tsHistory.Location = new System.Drawing.Point(25, 9);
            this.tsHistory.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tsHistory.Name = "tsHistory";
            this.tsHistory.Properties.OffText = "Don\'t show history of cleared Messages";
            this.tsHistory.Properties.OnText = "Show history of cleared Messages";
            this.tsHistory.Size = new System.Drawing.Size(574, 24);
            this.tsHistory.TabIndex = 0;
            this.tsHistory.Toggled += new System.EventHandler(this.tsHistory_Toggled);
            // 
            // tpLookAndFeel
            // 
            this.tpLookAndFeel.Controls.Add(this.tsStartupRibbonMinimized);
            this.tpLookAndFeel.Controls.Add(this.tsSimpleMode);
            this.tpLookAndFeel.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("tpLookAndFeel.ImageOptions.Image")));
            this.tpLookAndFeel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tpLookAndFeel.Name = "tpLookAndFeel";
            this.tpLookAndFeel.Size = new System.Drawing.Size(654, 432);
            this.tpLookAndFeel.Text = "Look And Feel";
            // 
            // tsStartupRibbonMinimized
            // 
            this.tsStartupRibbonMinimized.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tsStartupRibbonMinimized.EditValue = true;
            this.tsStartupRibbonMinimized.Location = new System.Drawing.Point(12, 5);
            this.tsStartupRibbonMinimized.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tsStartupRibbonMinimized.Name = "tsStartupRibbonMinimized";
            this.tsStartupRibbonMinimized.Properties.OffText = "Show Ribbon";
            this.tsStartupRibbonMinimized.Properties.OnText = "Ribbon is minimized as default";
            this.tsStartupRibbonMinimized.Size = new System.Drawing.Size(550, 24);
            this.tsStartupRibbonMinimized.TabIndex = 3;
            this.tsStartupRibbonMinimized.Toggled += new System.EventHandler(this.tsStartupRibbonMinimized_Toggled);
            // 
            // tsSimpleMode
            // 
            this.tsSimpleMode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tsSimpleMode.Enabled = false;
            this.tsSimpleMode.Location = new System.Drawing.Point(12, 33);
            this.tsSimpleMode.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tsSimpleMode.Name = "tsSimpleMode";
            this.tsSimpleMode.Properties.OffText = "Use advanced UI (Restart required)";
            this.tsSimpleMode.Properties.OnText = "Use simplified UI (Restart required)";
            this.tsSimpleMode.Size = new System.Drawing.Size(550, 24);
            this.tsSimpleMode.TabIndex = 2;
            this.tsSimpleMode.Visible = false;
            this.tsSimpleMode.Toggled += new System.EventHandler(this.tsSimpleMode_Toggled);
            // 
            // tpStatistics
            // 
            this.tpStatistics.Controls.Add(this.groupControl1);
            this.tpStatistics.Controls.Add(this.tsUserStatistics);
            this.tpStatistics.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("tpStatistics.ImageOptions.Image")));
            this.tpStatistics.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tpStatistics.Name = "tpStatistics";
            this.tpStatistics.Size = new System.Drawing.Size(654, 432);
            this.tpStatistics.Text = "User Statistics";
            // 
            // groupControl1
            // 
            this.groupControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupControl1.Controls.Add(this.lblOpenedFiles);
            this.groupControl1.Controls.Add(this.lblRunningTime);
            this.groupControl1.Controls.Add(this.lblLaunchCount);
            this.groupControl1.Controls.Add(this.btnClearStatistics);
            this.groupControl1.Location = new System.Drawing.Point(4, 41);
            this.groupControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(644, 385);
            this.groupControl1.TabIndex = 3;
            this.groupControl1.Text = "Statistics";
            // 
            // lblOpenedFiles
            // 
            this.lblOpenedFiles.Location = new System.Drawing.Point(22, 73);
            this.lblOpenedFiles.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.lblOpenedFiles.Name = "lblOpenedFiles";
            this.lblOpenedFiles.Size = new System.Drawing.Size(130, 13);
            this.lblOpenedFiles.TabIndex = 3;
            this.lblOpenedFiles.Text = "Number Of Opened Files: 0";
            // 
            // lblRunningTime
            // 
            this.lblRunningTime.Location = new System.Drawing.Point(22, 51);
            this.lblRunningTime.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.lblRunningTime.Name = "lblRunningTime";
            this.lblRunningTime.Size = new System.Drawing.Size(77, 13);
            this.lblRunningTime.TabIndex = 2;
            this.lblRunningTime.Text = "Running Time: 0";
            // 
            // lblLaunchCount
            // 
            this.lblLaunchCount.Location = new System.Drawing.Point(22, 29);
            this.lblLaunchCount.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.lblLaunchCount.Name = "lblLaunchCount";
            this.lblLaunchCount.Size = new System.Drawing.Size(153, 13);
            this.lblLaunchCount.TabIndex = 1;
            this.lblLaunchCount.Text = "Number of Analogy Launches: 0";
            // 
            // btnClearStatistics
            // 
            this.btnClearStatistics.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearStatistics.Location = new System.Drawing.Point(556, 24);
            this.btnClearStatistics.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnClearStatistics.Name = "btnClearStatistics";
            this.btnClearStatistics.Size = new System.Drawing.Size(82, 27);
            this.btnClearStatistics.TabIndex = 0;
            this.btnClearStatistics.Text = "Clear";
            this.btnClearStatistics.Click += new System.EventHandler(this.btnClearStatistics_Click);
            // 
            // tsUserStatistics
            // 
            this.tsUserStatistics.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tsUserStatistics.Location = new System.Drawing.Point(25, 9);
            this.tsUserStatistics.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tsUserStatistics.Name = "tsUserStatistics";
            this.tsUserStatistics.Properties.OffText = "User Statistics are disabled";
            this.tsUserStatistics.Properties.OnText = "User Statistics are enabled";
            this.tsUserStatistics.Size = new System.Drawing.Size(617, 24);
            this.tsUserStatistics.TabIndex = 2;
            this.tsUserStatistics.Toggled += new System.EventHandler(this.tsUserStatistics_Toggled);
            // 
            // xtraTabPage3
            // 
            this.xtraTabPage3.Controls.Add(this.tswitchExtensionsStartup);
            this.xtraTabPage3.Controls.Add(this.chklItems);
            this.xtraTabPage3.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("xtraTabPage3.ImageOptions.Image")));
            this.xtraTabPage3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.xtraTabPage3.Name = "xtraTabPage3";
            this.xtraTabPage3.Size = new System.Drawing.Size(654, 432);
            this.xtraTabPage3.Text = "Extensions";
            // 
            // tswitchExtensionsStartup
            // 
            this.tswitchExtensionsStartup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tswitchExtensionsStartup.Location = new System.Drawing.Point(15, 9);
            this.tswitchExtensionsStartup.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tswitchExtensionsStartup.Name = "tswitchExtensionsStartup";
            this.tswitchExtensionsStartup.Properties.OffText = "No Startup extensions ";
            this.tswitchExtensionsStartup.Properties.OnText = "Load following Extensions at startup:";
            this.tswitchExtensionsStartup.Size = new System.Drawing.Size(544, 24);
            this.tswitchExtensionsStartup.TabIndex = 7;
            this.tswitchExtensionsStartup.Toggled += new System.EventHandler(this.tswitchExtensionsStartup_Toggled);
            // 
            // chklItems
            // 
            this.chklItems.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chklItems.CheckOnClick = true;
            this.chklItems.FormattingEnabled = true;
            this.chklItems.Location = new System.Drawing.Point(15, 39);
            this.chklItems.Margin = new System.Windows.Forms.Padding(5);
            this.chklItems.Name = "chklItems";
            this.chklItems.Size = new System.Drawing.Size(630, 372);
            this.chklItems.TabIndex = 6;
            this.chklItems.SelectedIndexChanged += new System.EventHandler(this.chklItems_SelectedIndexChanged);
            // 
            // xtPage
            // 
            this.xtPage.Controls.Add(this.labelControl3);
            this.xtPage.Controls.Add(this.labelControl4);
            this.xtPage.Controls.Add(this.labelControl2);
            this.xtPage.Controls.Add(this.labelControl1);
            this.xtPage.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("xtPage.ImageOptions.Image")));
            this.xtPage.Name = "xtPage";
            this.xtPage.Size = new System.Drawing.Size(654, 432);
            this.xtPage.Text = "Shortcuts";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(22, 99);
            this.labelControl3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(236, 13);
            this.labelControl3.TabIndex = 7;
            this.labelControl3.Text = "Toggle on/off warning log level filtering: ALT + W";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(22, 73);
            this.labelControl4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(264, 13);
            this.labelControl4.TabIndex = 6;
            this.labelControl4.Text = "Toggle on/off Error + Critical log level filtering: ALT + E";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(22, 45);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(188, 13);
            this.labelControl2.TabIndex = 5;
            this.labelControl2.Text = "Go to exclude filter textbox: SHIFT + F";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(22, 19);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(172, 13);
            this.labelControl1.TabIndex = 4;
            this.labelControl1.Text = "Go to include filter textbox: Ctrl + F";
            // 
            // xTabMRU
            // 
            this.xTabMRU.Controls.Add(this.lblRecent);
            this.xTabMRU.Controls.Add(this.nudRecent);
            this.xTabMRU.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("xTabMRU.ImageOptions.Image")));
            this.xTabMRU.Name = "xTabMRU";
            this.xTabMRU.Size = new System.Drawing.Size(654, 432);
            this.xTabMRU.Text = "Most Recently Used";
            // 
            // lblRecent
            // 
            this.lblRecent.Location = new System.Drawing.Point(17, 10);
            this.lblRecent.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.lblRecent.Name = "lblRecent";
            this.lblRecent.Size = new System.Drawing.Size(149, 13);
            this.lblRecent.TabIndex = 3;
            this.lblRecent.Text = "Number of recent files to keep:";
            // 
            // nudRecent
            // 
            this.nudRecent.Location = new System.Drawing.Point(212, 8);
            this.nudRecent.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.nudRecent.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.nudRecent.Name = "nudRecent";
            this.nudRecent.Size = new System.Drawing.Size(63, 21);
            this.nudRecent.TabIndex = 2;
            this.nudRecent.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudRecent.ValueChanged += new System.EventHandler(this.nudRecent_ValueChanged);
            // 
            // UserSettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(804, 438);
            this.Controls.Add(this.tabControlMain);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "UserSettingsForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "User Settings";
            this.Load += new System.EventHandler(this.UserSettingsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tabControlMain)).EndInit();
            this.tabControlMain.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcFiles)).EndInit();
            this.gcFiles.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tsFileCaching.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcFiltering)).EndInit();
            this.gcFiltering.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.checkEditSearchAlsoInSourceAndModule.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkEditPaging.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPageLength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tsErrorLevelAsDefault.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tsFilteringExclude.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tsAutoComplete.Properties)).EndInit();
            this.xtraTabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tsHistory.Properties)).EndInit();
            this.tpLookAndFeel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tsStartupRibbonMinimized.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tsSimpleMode.Properties)).EndInit();
            this.tpStatistics.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tsUserStatistics.Properties)).EndInit();
            this.xtraTabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tswitchExtensionsStartup.Properties)).EndInit();
            this.xtPage.ResumeLayout(false);
            this.xtPage.PerformLayout();
            this.xTabMRU.ResumeLayout(false);
            this.xTabMRU.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudRecent)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraTab.XtraTabControl tabControlMain;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage2;
        private DevExpress.XtraEditors.ToggleSwitch tsFilteringExclude;
        private DevExpress.XtraEditors.ToggleSwitch tsHistory;
        private DevExpress.XtraEditors.ToggleSwitch tsAutoComplete;
        private DevExpress.XtraEditors.LabelControl lblRecent;
        private System.Windows.Forms.NumericUpDown nudRecent;
        private DevExpress.XtraTab.XtraTabPage tpLookAndFeel;
        private DevExpress.XtraTab.XtraTabPage tpStatistics;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.LabelControl lblOpenedFiles;
        private DevExpress.XtraEditors.LabelControl lblRunningTime;
        private DevExpress.XtraEditors.LabelControl lblLaunchCount;
        private DevExpress.XtraEditors.SimpleButton btnClearStatistics;
        private DevExpress.XtraEditors.ToggleSwitch tsUserStatistics;
        private DevExpress.XtraEditors.GroupControl gcFiles;
        private DevExpress.XtraEditors.ToggleSwitch tsFileCaching;
        private DevExpress.XtraEditors.GroupControl gcFiltering;
        private DevExpress.XtraEditors.ToggleSwitch tsSimpleMode;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage3;
        private System.Windows.Forms.CheckedListBox chklItems;
        private DevExpress.XtraEditors.ToggleSwitch tswitchExtensionsStartup;
        private DevExpress.XtraEditors.ToggleSwitch tsStartupRibbonMinimized;
        private DevExpress.XtraEditors.ToggleSwitch tsErrorLevelAsDefault;
        private DevExpress.XtraEditors.CheckEdit chkEditPaging;
        private System.Windows.Forms.NumericUpDown nudPageLength;
        private DevExpress.XtraTab.XtraTabPage xtPage;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraTab.XtraTabPage xTabMRU;
        private DevExpress.XtraEditors.CheckEdit checkEditSearchAlsoInSourceAndModule;
    }
}