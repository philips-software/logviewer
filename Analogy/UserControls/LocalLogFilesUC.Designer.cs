using DevExpress.XtraGrid.Views.Grid;

namespace Analogy
{
    partial class LocalLogFilesUC
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
            if (disposing)
            {

                if (components != null)
                {
                    components.Dispose();

                }
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LocalLogFilesUC));
            this.spltMain = new System.Windows.Forms.SplitContainer();
            this.splcLeft = new System.Windows.Forms.SplitContainer();
            this.folderTreeViewUC1 = new Analogy.FolderTreeViewUC();
            this.treeList1 = new DevExpress.XtraTreeList.TreeList();
            this.colName = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colChanged = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colSize = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colFullPath = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.standaloneBarDockControl1 = new DevExpress.XtraBars.StandaloneBarDockControl();
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.bBtnOpen = new DevExpress.XtraBars.BarButtonItem();
            this.bBtnRefresh = new DevExpress.XtraBars.BarButtonItem();
            this.bBtnDelete = new DevExpress.XtraBars.BarButtonItem();
            this.bBtnSelectAll = new DevExpress.XtraBars.BarButtonItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.chkbSelectionMode = new DevExpress.XtraEditors.CheckEdit();
            this.checkEditRecursiveLoad = new DevExpress.XtraEditors.CheckEdit();
            this.ucLogs1 = new Analogy.UCLogs();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.imageListBottom = new System.Windows.Forms.ImageList(this.components);
            this.tsPrimary = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.bar2 = new DevExpress.XtraBars.Bar();
            this.bBtnOpenFolder = new DevExpress.XtraBars.BarButtonItem();
            ((System.ComponentModel.ISupportInitialize)(this.spltMain)).BeginInit();
            this.spltMain.Panel1.SuspendLayout();
            this.spltMain.Panel2.SuspendLayout();
            this.spltMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splcLeft)).BeginInit();
            this.splcLeft.Panel1.SuspendLayout();
            this.splcLeft.Panel2.SuspendLayout();
            this.splcLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treeList1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkbSelectionMode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditRecursiveLoad.Properties)).BeginInit();
            this.tsPrimary.SuspendLayout();
            this.SuspendLayout();
            // 
            // spltMain
            // 
            this.spltMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spltMain.Location = new System.Drawing.Point(0, 0);
            this.spltMain.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.spltMain.Name = "spltMain";
            // 
            // spltMain.Panel1
            // 
            this.spltMain.Panel1.Controls.Add(this.splcLeft);
            // 
            // spltMain.Panel2
            // 
            this.spltMain.Panel2.Controls.Add(this.ucLogs1);
            this.spltMain.Size = new System.Drawing.Size(1387, 700);
            this.spltMain.SplitterDistance = 392;
            this.spltMain.TabIndex = 5;
            // 
            // splcLeft
            // 
            this.splcLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splcLeft.Location = new System.Drawing.Point(0, 0);
            this.splcLeft.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.splcLeft.Name = "splcLeft";
            this.splcLeft.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splcLeft.Panel1
            // 
            this.splcLeft.Panel1.Controls.Add(this.folderTreeViewUC1);
            // 
            // splcLeft.Panel2
            // 
            this.splcLeft.Panel2.Controls.Add(this.treeList1);
            this.splcLeft.Panel2.Controls.Add(this.standaloneBarDockControl1);
            this.splcLeft.Panel2.Controls.Add(this.chkbSelectionMode);
            this.splcLeft.Panel2.Controls.Add(this.checkEditRecursiveLoad);
            this.splcLeft.Size = new System.Drawing.Size(392, 700);
            this.splcLeft.SplitterDistance = 225;
            this.splcLeft.TabIndex = 4;
            // 
            // folderTreeViewUC1
            // 
            this.folderTreeViewUC1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.folderTreeViewUC1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F);
            this.folderTreeViewUC1.Location = new System.Drawing.Point(0, 0);
            this.folderTreeViewUC1.Margin = new System.Windows.Forms.Padding(4);
            this.folderTreeViewUC1.Name = "folderTreeViewUC1";
            this.folderTreeViewUC1.Size = new System.Drawing.Size(392, 225);
            this.folderTreeViewUC1.TabIndex = 0;
            // 
            // treeList1
            // 
            this.treeList1.Appearance.FocusedRow.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.treeList1.Appearance.FocusedRow.Options.UseFont = true;
            this.treeList1.Appearance.SelectedRow.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.treeList1.Appearance.SelectedRow.Options.UseFont = true;
            this.treeList1.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.colName,
            this.colChanged,
            this.colSize,
            this.colFullPath});
            this.treeList1.Cursor = System.Windows.Forms.Cursors.Default;
            this.treeList1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeList1.FixedLineWidth = 3;
            this.treeList1.Location = new System.Drawing.Point(0, 70);
            this.treeList1.Margin = new System.Windows.Forms.Padding(4);
            this.treeList1.MinWidth = 27;
            this.treeList1.Name = "treeList1";
            this.treeList1.OptionsBehavior.Editable = false;
            this.treeList1.OptionsFind.AlwaysVisible = true;
            this.treeList1.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.treeList1.OptionsSelection.MultiSelect = true;
            this.treeList1.OptionsSelection.UseIndicatorForSelection = true;
            this.treeList1.OptionsView.AutoWidth = false;
            this.treeList1.OptionsView.BestFitMode = DevExpress.XtraTreeList.TreeListBestFitMode.Fast;
            this.treeList1.OptionsView.BestFitNodes = DevExpress.XtraTreeList.TreeListBestFitNodes.All;
            this.treeList1.OptionsView.EnableAppearanceEvenRow = true;
            this.treeList1.OptionsView.FocusRectStyle = DevExpress.XtraTreeList.DrawFocusRectStyle.RowFullFocus;
            this.treeList1.OptionsView.ShowIndicator = false;
            this.treeList1.Size = new System.Drawing.Size(392, 401);
            this.treeList1.TabIndex = 8;
            this.treeList1.TreeLevelWidth = 24;
            this.treeList1.SelectionChanged += new System.EventHandler(this.TreeList1_SelectionChanged);
            // 
            // colName
            // 
            this.colName.Caption = "Name";
            this.colName.FieldName = "Name";
            this.colName.MinWidth = 44;
            this.colName.Name = "colName";
            this.colName.Visible = true;
            this.colName.VisibleIndex = 0;
            this.colName.Width = 140;
            // 
            // colChanged
            // 
            this.colChanged.Caption = "Last changed";
            this.colChanged.FieldName = "colChanged";
            this.colChanged.MinWidth = 140;
            this.colChanged.Name = "colChanged";
            this.colChanged.OptionsColumn.AllowEdit = false;
            this.colChanged.Visible = true;
            this.colChanged.VisibleIndex = 1;
            this.colChanged.Width = 141;
            // 
            // colSize
            // 
            this.colSize.Caption = "Size(Bytes)";
            this.colSize.FieldName = "Size";
            this.colSize.MinWidth = 27;
            this.colSize.Name = "colSize";
            this.colSize.Visible = true;
            this.colSize.VisibleIndex = 2;
            this.colSize.Width = 117;
            // 
            // colFullPath
            // 
            this.colFullPath.Caption = "Path";
            this.colFullPath.FieldName = "Path";
            this.colFullPath.MinWidth = 27;
            this.colFullPath.Name = "colFullPath";
            this.colFullPath.Visible = true;
            this.colFullPath.VisibleIndex = 3;
            this.colFullPath.Width = 116;
            // 
            // standaloneBarDockControl1
            // 
            this.standaloneBarDockControl1.CausesValidation = false;
            this.standaloneBarDockControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.standaloneBarDockControl1.Location = new System.Drawing.Point(0, 40);
            this.standaloneBarDockControl1.Manager = this.barManager1;
            this.standaloneBarDockControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.standaloneBarDockControl1.Name = "standaloneBarDockControl1";
            this.standaloneBarDockControl1.Size = new System.Drawing.Size(392, 30);
            this.standaloneBarDockControl1.Text = "standaloneBarDockControl1";
            this.standaloneBarDockControl1.AutoSize = true;
            // 
            // barManager1
            // 
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar1});
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.DockControls.Add(this.standaloneBarDockControl1);
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.bBtnOpen,
            this.bBtnRefresh,
            this.bBtnDelete,
            this.bBtnSelectAll});
            this.barManager1.MaxItemId = 4;
            // 
            // bar1
            // 
            this.bar1.BarName = "Custom 2";
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 0;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Standalone;
            this.bar1.FloatLocation = new System.Drawing.Point(98, 376);
            this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.bBtnOpen),
            new DevExpress.XtraBars.LinkPersistInfo(this.bBtnRefresh),
            new DevExpress.XtraBars.LinkPersistInfo(this.bBtnDelete),
            new DevExpress.XtraBars.LinkPersistInfo(this.bBtnSelectAll)});
            this.bar1.OptionsBar.AllowQuickCustomization = false;
            this.bar1.OptionsBar.UseWholeRow = true;
            this.bar1.StandaloneBarDockControl = this.standaloneBarDockControl1;
            this.bar1.Text = "Custom 2";
            // 
            // bBtnOpen
            // 
            this.bBtnOpen.Caption = "Open";
            this.bBtnOpen.Id = 0;
            this.bBtnOpen.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("bBtnOpen.ImageOptions.Image")));
            this.bBtnOpen.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("bBtnOpen.ImageOptions.LargeImage")));
            this.bBtnOpen.Name = "bBtnOpen";
            this.bBtnOpen.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            this.bBtnOpen.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bBtnOpen_ItemClick);
            // 
            // bBtnRefresh
            // 
            this.bBtnRefresh.Caption = "Refresh";
            this.bBtnRefresh.Id = 1;
            this.bBtnRefresh.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("bBtnRefresh.ImageOptions.Image")));
            this.bBtnRefresh.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("bBtnRefresh.ImageOptions.LargeImage")));
            this.bBtnRefresh.Name = "bBtnRefresh";
            this.bBtnRefresh.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            this.bBtnRefresh.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bBtnRefresh_ItemClick);
            // 
            // bBtnDelete
            // 
            this.bBtnDelete.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.bBtnDelete.Caption = "Delete";
            this.bBtnDelete.Id = 2;
            this.bBtnDelete.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("bBtnDelete.ImageOptions.Image")));
            this.bBtnDelete.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("bBtnDelete.ImageOptions.LargeImage")));
            this.bBtnDelete.Name = "bBtnDelete";
            this.bBtnDelete.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            this.bBtnDelete.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bBtnDelete_ItemClick);
            // 
            // bBtnSelectAll
            // 
            this.bBtnSelectAll.Caption = "Select All";
            this.bBtnSelectAll.Id = 3;
            this.bBtnSelectAll.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("bBtnSelectAll.ImageOptions.Image")));
            this.bBtnSelectAll.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("bBtnSelectAll.ImageOptions.LargeImage")));
            this.bBtnSelectAll.Name = "bBtnSelectAll";
            this.bBtnSelectAll.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            this.bBtnSelectAll.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bBtnSelectAll_ItemClick);
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.barManager1;
            this.barDockControlTop.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.barDockControlTop.Size = new System.Drawing.Size(1387, 0);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 700);
            this.barDockControlBottom.Manager = this.barManager1;
            this.barDockControlBottom.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.barDockControlBottom.Size = new System.Drawing.Size(1387, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlLeft.Manager = this.barManager1;
            this.barDockControlLeft.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 700);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1387, 0);
            this.barDockControlRight.Manager = this.barManager1;
            this.barDockControlRight.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 700);
            // 
            // chkbSelectionMode
            // 
            this.chkbSelectionMode.Dock = System.Windows.Forms.DockStyle.Top;
            this.chkbSelectionMode.EditValue = true;
            this.chkbSelectionMode.Location = new System.Drawing.Point(0, 20);
            this.chkbSelectionMode.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkbSelectionMode.Name = "chkbSelectionMode";
            this.chkbSelectionMode.Properties.Caption = "Clear log between selection";
            this.chkbSelectionMode.Properties.ImageOptions.ImageChecked = ((System.Drawing.Image)(resources.GetObject("chkbSelectionMode.Properties.ImageOptions.ImageChecked")));
            this.chkbSelectionMode.Properties.ImageOptions.ImageUnchecked = ((System.Drawing.Image)(resources.GetObject("chkbSelectionMode.Properties.ImageOptions.ImageUnchecked")));
            this.chkbSelectionMode.Size = new System.Drawing.Size(392, 20);
            this.chkbSelectionMode.TabIndex = 6;
            // 
            // checkEditRecursiveLoad
            // 
            this.checkEditRecursiveLoad.Dock = System.Windows.Forms.DockStyle.Top;
            this.checkEditRecursiveLoad.EditValue = true;
            this.checkEditRecursiveLoad.Location = new System.Drawing.Point(0, 0);
            this.checkEditRecursiveLoad.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkEditRecursiveLoad.Name = "checkEditRecursiveLoad";
            this.checkEditRecursiveLoad.Properties.Caption = "Load Recursive Files";
            this.checkEditRecursiveLoad.Properties.ImageOptions.ImageChecked = ((System.Drawing.Image)(resources.GetObject("checkEditRecursiveLoad.Properties.ImageOptions.ImageChecked")));
            this.checkEditRecursiveLoad.Properties.ImageOptions.ImageUnchecked = ((System.Drawing.Image)(resources.GetObject("checkEditRecursiveLoad.Properties.ImageOptions.ImageUnchecked")));
            this.checkEditRecursiveLoad.Size = new System.Drawing.Size(392, 20);
            this.checkEditRecursiveLoad.TabIndex = 7;
            // 
            // ucLogs1
            // 
            this.ucLogs1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucLogs1.DoNotAddToRecentHistory = false;
            this.ucLogs1.ForceNoFileCaching = false;
            this.ucLogs1.Location = new System.Drawing.Point(0, 0);
            this.ucLogs1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ucLogs1.Name = "ucLogs1";
            this.ucLogs1.Size = new System.Drawing.Size(991, 700);
            this.ucLogs1.TabIndex = 0;
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "Error_16x16.png");
            this.imageList.Images.SetKeyName(1, "Warning_16x16.png");
            this.imageList.Images.SetKeyName(2, "");
            this.imageList.Images.SetKeyName(3, "folder32x32.gif");
            this.imageList.Images.SetKeyName(4, "Error_32x32.png");
            this.imageList.Images.SetKeyName(5, "Warning_32x32.png");
            // 
            // imageListBottom
            // 
            this.imageListBottom.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListBottom.ImageStream")));
            this.imageListBottom.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListBottom.Images.SetKeyName(0, "Info_16x16.png");
            this.imageListBottom.Images.SetKeyName(1, "RichEditBookmark_16x16.png");
            this.imageListBottom.Images.SetKeyName(2, "RichEditBookmark_32x32.png");
            // 
            // tsPrimary
            // 
            this.tsPrimary.AutoSize = false;
            this.tsPrimary.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.tsPrimary.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator1});
            this.tsPrimary.Location = new System.Drawing.Point(0, 0);
            this.tsPrimary.Name = "tsPrimary";
            this.tsPrimary.Size = new System.Drawing.Size(1387, 46);
            this.tsPrimary.TabIndex = 6;
            this.tsPrimary.Visible = false;
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 46);
            // 
            // bar2
            // 
            this.bar2.BarName = "Custom 4";
            this.bar2.DockCol = 0;
            this.bar2.DockRow = 0;
            this.bar2.DockStyle = DevExpress.XtraBars.BarDockStyle.Standalone;
            this.bar2.FloatLocation = new System.Drawing.Point(346, 374);
            this.bar2.OptionsBar.AllowQuickCustomization = false;
            this.bar2.OptionsBar.UseWholeRow = true;
            this.bar2.Text = "Custom 4";
            // 
            // bBtnOpenFolder
            // 
            this.bBtnOpenFolder.Caption = "Open";
            this.bBtnOpenFolder.Id = 2;
            this.bBtnOpenFolder.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("bBtnOpenFolder.ImageOptions.Image")));
            this.bBtnOpenFolder.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("bBtnOpenFolder.ImageOptions.LargeImage")));
            this.bBtnOpenFolder.Name = "bBtnOpenFolder";
            this.bBtnOpenFolder.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            // 
            // LocalLogFilesUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.spltMain);
            this.Controls.Add(this.tsPrimary);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "LocalLogFilesUC";
            this.Size = new System.Drawing.Size(1387, 700);
            this.Load += new System.EventHandler(this.OfflineUCLogs_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.AnalogyUCLogs_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.AnalogyUCLogs_DragEnter);
            this.spltMain.Panel1.ResumeLayout(false);
            this.spltMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.spltMain)).EndInit();
            this.spltMain.ResumeLayout(false);
            this.splcLeft.Panel1.ResumeLayout(false);
            this.splcLeft.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splcLeft)).EndInit();
            this.splcLeft.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.treeList1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkbSelectionMode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditRecursiveLoad.Properties)).EndInit();
            this.tsPrimary.ResumeLayout(false);
            this.tsPrimary.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.SplitContainer spltMain;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.ToolStrip tsPrimary;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.SplitContainer splcLeft;
        private System.Windows.Forms.ImageList imageListBottom;
        private UCLogs ucLogs1;
        private DevExpress.XtraBars.StandaloneBarDockControl standaloneBarDockControl1;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraEditors.CheckEdit chkbSelectionMode;
        private DevExpress.XtraBars.Bar bar2;
        private DevExpress.XtraBars.BarButtonItem bBtnOpen;
        private DevExpress.XtraBars.BarButtonItem bBtnOpenFolder;
        private DevExpress.XtraBars.BarButtonItem bBtnRefresh;
        private DevExpress.XtraBars.BarButtonItem bBtnDelete;
        private FolderTreeViewUC folderTreeViewUC1;
        private DevExpress.XtraBars.BarButtonItem bBtnSelectAll;
        private DevExpress.XtraEditors.CheckEdit checkEditRecursiveLoad;
        private DevExpress.XtraTreeList.TreeList treeList1;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colName;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colChanged;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colSize;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colFullPath;
    }
}
