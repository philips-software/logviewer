﻿using DevExpress.Data;
using DevExpress.LookAndFeel;
using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraPrinting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Analogy.DataSources;
using Analogy.Interfaces;
using static System.Enum;
using Message = System.Windows.Forms.Message;

namespace Analogy
{

    public partial class UCLogs : XtraUserControl, ILogMessageCreatedHandler
    {
        public bool ForceNoFileCaching { get; } = false;
        private PagingManager PagingManager { get; set; }
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        public event EventHandler<AnalogyClearedHistoryEventArgs> OnHistoryCleared;
        private Dictionary<string, List<AnalogyLogMessage>> groupingByChars;
        private string OldTextInclude = string.Empty;
        private string OldTextExclude = string.Empty;
        private UserSettingsManager Settings { get; } = UserSettingsManager.UserSettings;
        private IExtensionsManager ExtensionManager { get; set; } = ExtensionsManager.Instance;
        private IEnumerable<IAnalogyExtension> InPlaceRegisteredExtensions { get; set; }
        private List<IAnalogyExtension> UserControlRegisteredExtensions { get; set; }
        private List<int> HighlightRows { get; set; } = new List<int>();
        private int SelectedHighlightRow { get; set; }
        private ToolTip Tip { get; set; }
        private List<string> _excludeMostCommon = new List<string>();
        public const string DataGridDateColumnName = "Date";
        private bool _realtimeUpdate = true;

        private ReaderWriterLockSlim lockExternalWindowsObject = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
        private ReaderWriterLockSlim lockSlim;
        private DataTable _messageData;
        private DataTable _bookmarkedMessages;
        private IProgress<AnalogyProgressReport> ProgressReporter { get; set; }
        private readonly List<XtraFormLogGrid> _externalWindows = new List<XtraFormLogGrid>();
        private List<XtraFormLogGrid> ExternalWindows
        {
            get
            {
                lockExternalWindowsObject.EnterReadLock();
                var items = _externalWindows.ToList();
                lockExternalWindowsObject.ExitReadLock();
                return items;
            }
        }

        private int ExternalWindowsCount;
        private List<AnalogyLogMessage> Messages
        {
            get
            {
                var filterDatatable = GetFilteredDataTable();
                return filterDatatable.Rows.OfType<DataRow>().Select(r => (AnalogyLogMessage)r["Object"]).ToList();
            }
        }

        private List<AnalogyLogMessage> BookmarkedMessages
        {
            get { return _bookmarkedMessages.Rows.OfType<DataRow>().Select(r => (AnalogyLogMessage)r["Object"]).ToList(); }
        }

        private bool EnableOTA { get; } = false;//GeneralUtils.UseDebugMode("AnalogyOTA");
        private AnalogyLogMessage _currentMassage;
        private FilterCriteriaObject _filterCriteriaInline = new FilterCriteriaObject();
        private AutoCompleteStringCollection autoCompleteInclude = new AutoCompleteStringCollection();
        private AutoCompleteStringCollection autoCompleteExclude = new AutoCompleteStringCollection();
        public bool OnlineMode { get; set; }

        private bool FilterHasChanged { get; set; }
        private bool NewDataExist { get; set; }
        private bool hasAnyInPlaceExtensions;
        private bool hasAnyUserControlExtensions;
        private DateTime diffStartTime = DateTime.MinValue;
        private string LayoutFileName;
        private bool BookmarkView;
        private int pageNumber = 1;

        private int TotalPages => PagingManager.TotalPages;
        private IAnalogyOfflineDataProvider FileDataProvider { get; set; }
        private IAnalogyOfflineDataProvider AnalogyOfflineDataProvider { get; } = new AnalogyOfflineDataProvider();
        public UCLogs()
        {
            InitializeComponent();
            if (DesignMode) return;
            //splitContainerMain.IsSplitterFixed = false;
            //splitContainerMain.FixedPanel = SplitFixedPanel.None;
            //ClientSizeChanged += (s, e) => { splitContainerMain.SplitterPosition = (int)0.8 * splitContainerMain.Height; };
            LayoutFileName = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty, "layout.xml");
            PagingManager = new PagingManager(this);
            lockSlim = PagingManager.lockSlim;
            _messageData = PagingManager.CurrentPage();
        }

        private void UCLogs_Load(object sender, EventArgs e)
        {
            if (DesignMode) return;

            PagingManager.OnPageChanged += (s, arg) =>
            {
                if (IsDisposed) return;
                BeginInvoke(new MethodInvoker(() =>
                    lblPageNumber.Text = $"Page {pageNumber} / {arg.AnalogyPage.PageNumber}"));

            };
            LoadUISettings();
            BookmarkModeUI();

            hasAnyInPlaceExtensions = ExtensionManager.HasAnyInPlace;
            hasAnyUserControlExtensions = ExtensionManager.HasAnyInPlace;
            InPlaceRegisteredExtensions = ExtensionManager.InPlaceRegisteredExtensions.ToList();
            UserControlRegisteredExtensions = ExtensionManager.UserControlRegisteredExtensions.ToList();
            InitializeExtensionsColumns();
            ProgressReporter = new Progress<AnalogyProgressReport>((value) =>
            {
                progressBar1.Maximum = value.Total;
                if (value.Processed < progressBar1.Maximum && value.Total > 1)
                    progressBar1.Value = value.Processed;
                if (value.Processed == value.Total)
                    progressBar1.Visible = false;
            });

            logGrid.RowCountChanged += (s, arg) =>
            {
                if (Settings.AutoScrollToLastMessage && !IsDisposed)
                {
                    BeginInvoke(new MethodInvoker(() =>
                    {
                        logGrid.MoveLast();
                        logGrid.MakeRowVisible(logGrid.FocusedRowHandle);
                    }));

                }
            };

            gridControl.DataSource = _messageData.DefaultView;
            _bookmarkedMessages = Utils.DataTableConstructor();
            gridControlBookmarkedMessages.DataSource = _bookmarkedMessages;

            gridControl.Focus();
        }

        public void SetFileDataSource(IAnalogyOfflineDataProvider fileDataProvider)
        {
            FileDataProvider = fileDataProvider;
            if (FileDataProvider is EventLogDataProvider eventDataSource)
                eventDataSource.LogWindow = this;
            if (FileDataProvider == null)
            {
                //disable specific saving
                bBtnSaveLog.Visibility = BarItemVisibility.Never;
                bBtnSaveEntireLog.Visibility = BarItemVisibility.Never;
            }
        }
        //public UCLogs(bool bookmarkView) : this()
        //{
        //    BookmarkView = bookmarkView;
        //}

        public void ProcessCmdKeyFromParent(Keys keyData)
        {
            KeyEventArgs e = new KeyEventArgs(keyData);
            if (e.Control && e.KeyCode == Keys.F)
            {
                txtbIncludeText.Focus();
            }
            if (e.Shift && e.KeyCode == Keys.F)

            {
                txtbExclude.Focus();
            }

            if (e.Alt && e.KeyCode == Keys.E)
            {
                chkLstLogLevel.Items[0].CheckState = (chkLstLogLevel.Items[0].CheckState == CheckState.Checked)
                    ? CheckState.Unchecked
                    : CheckState.Checked;
            }
            if (e.Alt && e.KeyCode == Keys.W)
            {
                chkLstLogLevel.Items[1].CheckState = (chkLstLogLevel.Items[1].CheckState == CheckState.Checked)
                    ? CheckState.Unchecked
                    : CheckState.Checked;
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            KeyEventArgs e = new KeyEventArgs(keyData);
            if (e.Control && e.KeyCode == Keys.F)

            {
                txtbIncludeText.Focus();
                return true;
            }
            if (e.Shift && e.KeyCode == Keys.F)

            {
                txtbExclude.Focus();
                return true;
            }

            if (e.Alt && e.KeyCode == Keys.E)
            {
                chkLstLogLevel.Items[0].CheckState = (chkLstLogLevel.Items[0].CheckState == CheckState.Checked)
                    ? CheckState.Unchecked
                    : CheckState.Checked;
                return true;
            }
            if (e.Alt && e.KeyCode == Keys.W)
            {
                chkLstLogLevel.Items[1].CheckState = (chkLstLogLevel.Items[1].CheckState == CheckState.Checked)
                    ? CheckState.Unchecked
                    : CheckState.Checked;
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);

        }

        private void LoadUISettings()
        {
            Tip = new ToolTip();
            Tip.SetToolTip(pboxInfo, "Use & or + for AND operations. Use | for OR operations");
            Tip.SetToolTip(pboxInfoExclude, "Use , to separate values");

            spltFilteringBoth.SplitterDistance = spltFilteringBoth.Width - 150;
            pnlFilteringLeft.Dock = DockStyle.Fill;
            txtbIncludeText.MaskBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txtbIncludeText.MaskBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtbIncludeText.MaskBox.AutoCompleteCustomSource = autoCompleteInclude;

            txtbExclude.MaskBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txtbExclude.MaskBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtbExclude.MaskBox.AutoCompleteCustomSource = autoCompleteExclude;

            gridControl.ForceInitialize();
            if (File.Exists(LayoutFileName))
                gridControl.MainView.RestoreLayoutFromXml(LayoutFileName);
            if (Settings.SaveExcludeTexts)
            {
                txtbExclude.Text = Settings.ExcludedText;
                txtbExcludeSource.Text = Settings.ExcludedSource;
                txtbExcludeModule.Text = Settings.ExcludedModules;
            }
            btswitchRefreshLog.Checked = true;
            gridColumnCategory.Visible = false;
            logGrid.BestFitColumns();
            btswitchExpand.Checked = true;
            splitContainerMain.Collapsed = true;
            if (Settings.StartupErrorLogLevel)
                chkLstLogLevel.Items[0].CheckState = CheckState.Checked;
            logGrid.Appearance.Row.Font = new Font(logGrid.Appearance.Row.Font.Name, Settings.FontSize);
            btsAutoScrollToBottom.Checked = Settings.AutoScrollToLastMessage;
        }

        private void BookmarkModeUI()
        {
            if (BookmarkView)
            {
                gridControl.ContextMenuStrip = cmsBookmarked;
                bBtnRemoveBoomark.Visibility = BarItemVisibility.Always;
                bBtnImport.Visibility = BarItemVisibility.Never;
            }
        }

        private void InitializeExtensionsColumns()
        {
            foreach (IAnalogyExtension extension in InPlaceRegisteredExtensions)
            {
                var columns = extension.GetColumnsInfo();
                foreach (AnalogyColumnInfo column in columns)
                {
                    var gridColumn = new GridColumn();
                    gridColumn.Caption = column.ColumnCaption;
                    gridColumn.FieldName = column.ColumnName;
                    gridColumn.OptionsFilter.FilterPopupMode = FilterPopupMode.CheckedList;
                    gridColumn.VisibleIndex = ExtensionManager.GetIndexForExtension(extension);
                    logGrid.Columns.Add(gridColumn);
                    gridColumn.Visible = true;
                }

            }
        }



        private void UCLogs_DragEnter(object sender, DragEventArgs e) =>
            e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Copy : DragDropEffects.None;

        private async void UCLogs_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            await LoadFilesAsync(files.ToList(), false);

        }

        #region UI events

        private void logGrid_Click(object sender, EventArgs e)
        {
            if (!(e is DXMouseEventArgs args))
                return;
            GridHitInfo hi = logGrid.CalcHitInfo(new Point(args.X, args.Y));

            if (hi.RowHandle < 0) return;
            int[] selRows = logGrid.GetSelectedRows();

            if (selRows == null || selRows.Length != 1) return;

            int rownum = selRows.First();
            _currentMassage = (AnalogyLogMessage)logGrid.GetRowCellValue(rownum, "Object");
            LoadTextBoxes(_currentMassage);
            if (hasAnyInPlaceExtensions)
            {
                var rowHandle = hi.RowHandle;
                var column = hi.Column;
                if (column == null) return;
                foreach (IAnalogyExtension extension in InPlaceRegisteredExtensions)
                {
                    var columns = extension.GetColumnsInfo();
                    foreach (AnalogyColumnInfo exColumn in columns)
                    {
                        if (column.FieldName.Equals(exColumn.ColumnName) &&
                            column.Caption.Equals(exColumn.ColumnCaption))
                        {
                            var cellValue = logGrid.GetRowCellValue(rowHandle, exColumn.ColumnName);
                            AnalogyCellClickedEventArgs argsForEx =
                                new AnalogyCellClickedEventArgs(exColumn.ColumnName, cellValue, _currentMassage);
                            extension.CellClicked(sender, argsForEx);
                        }

                    }
                }


            }

        }

        private void PmsGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {


            (AnalogyLogMessage message, _) = GetMessageFromSelectedRowInGrid();
            if (message == null) return;
            LoadTextBoxes(message);

        }

        private void LogGrid_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                OpenMessageDetails();
            }
        }

        private void pmsGridView_RowStyle(object sender, RowStyleEventArgs e)
        {
            if (sender is GridView view && e.RowHandle >= 0)
            {
                string level = view.GetRowCellDisplayText(e.RowHandle, view.Columns["Level"]);
                var parsed = TryParse(level, true, out AnalogyLogLevel enumLevel);
                if (parsed)
                {
                    switch (enumLevel)
                    {
                        case AnalogyLogLevel.Critical:
                            e.Appearance.BackColor = Color.Red;
                            if (UserLookAndFeel.Default.ActiveLookAndFeel.ActiveSkinName.Contains("Dark"))
                                e.Appearance.ForeColor = Color.Black;
                            break;
                        case AnalogyLogLevel.Error:
                            e.Appearance.BackColor = Color.Pink;
                            if (UserLookAndFeel.Default.ActiveLookAndFeel.ActiveSkinName.Contains("Dark"))
                                e.Appearance.ForeColor = Color.Black;
                            break;
                        case AnalogyLogLevel.Warning:
                            e.Appearance.BackColor = Color.Yellow;
                            if (UserLookAndFeel.Default.ActiveLookAndFeel.ActiveSkinName.Contains("Dark"))
                                e.Appearance.ForeColor = Color.Black;
                            break;
                        case AnalogyLogLevel.Event:
                        case AnalogyLogLevel.Verbose:
                        case AnalogyLogLevel.Debug:
                        case AnalogyLogLevel.Disabled:
                        case AnalogyLogLevel.Trace:
                        case AnalogyLogLevel.AnalogyInformation:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                string text = view.GetRowCellDisplayText(e.RowHandle, view.Columns["Text"]);
                if (chkbHighlight.Checked && _filterCriteriaInline.Match(text, txtbHighlight.Text))
                {
                    e.Appearance.BackColor = Color.Aqua;
                }


            }
        }

        private void pmsGridView_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (!(e.RowHandle >= 0) || !e.Info.IsRowIndicator || !(sender is GridView view)) return;
            AnalogyLogMessage msg = (AnalogyLogMessage)view.GetRowCellValue(e.RowHandle, "Object");
            Image img = imageList.Images[7];
            switch (msg.Level)
            {
                case AnalogyLogLevel.Critical:
                case AnalogyLogLevel.Error:
                    img = imageList.Images[0];
                    break;
                case AnalogyLogLevel.Warning:
                    img = imageList.Images[1];
                    break;
                case AnalogyLogLevel.Trace:
                case AnalogyLogLevel.Event:
                    img = imageList.Images[7];
                    break;
                case AnalogyLogLevel.Verbose:
                    img = imageList.Images[2];
                    break;
                case AnalogyLogLevel.Debug:
                    img = imageList.Images[6];
                    break;
                case AnalogyLogLevel.Disabled:
                    break;
                case AnalogyLogLevel.AnalogyInformation:
                    img = imageList.Images[8];
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            Rectangle r = e.Bounds;
            int x = r.X + (r.Width - imageList.ImageSize.Width) / 2;
            int y = r.Y + (r.Height - imageList.ImageSize.Height) / 2;
            e.Cache.DrawImage(img, x, y);
            e.Handled = true;
        }

        private void tsmiCopy_Click(object sender, EventArgs e)
        {
            (AnalogyLogMessage m, _) = GetMessageFromSelectedRowInGrid();
            if (m != null)
                Clipboard.SetText(m.Text);
        }

        private void tsmiEmail_Click(object sender, EventArgs e)
        {

            (AnalogyLogMessage message, _) = GetMessageFromSelectedRowInGrid();
            if (message == null) return;
            try
            {
                Process.Start($"mailto:?Subject=Analogy message&Body={message.Text}");
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, @"Error opening mail client", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

            Clipboard.SetText(message.Text);

        }

        private void tsmiExclude_Click(object sender, EventArgs e)
        {
            (AnalogyLogMessage message, _) = GetMessageFromSelectedRowInGrid();
            if (message == null) return;
            var ef = new AnalogyExcludeMessage(message);
            if (ef.ShowDialog(this) == DialogResult.OK)
            {
                string exclude = ef.Exclude;
                txtbExclude.Text = txtbExclude.Text + "|" + exclude;
                chkExclude.Checked = true;
                FilterHasChanged = true;
            }
        }

        private void RefreshUserFilter()
        {
            FilterHasChanged = true;
            tmrRefreshFilter.Stop();
            tmrRefreshFilter.Start();
        }

        private void chkbInclude_CheckedChanged(object sender, EventArgs e)
        {
            if (!chkbIncludeText.Checked && !chkExclude.Checked)
            {
                logGrid.ClearColumnsFilter();
                gridColumnText.FilterInfo = null;
            }

            FilterHasChanged = true;
        }

        private void chkbExclude_CheckedChanged(object sender, EventArgs e)
        {
            if (!chkbIncludeText.Checked && !chkExclude.Checked)
            {
                logGrid.ClearColumnsFilter();
                gridColumnText.FilterInfo = null;
            }

            FilterHasChanged = true;
        }
        private void txtbInclude_TextChanged(object sender, EventArgs e)
        {
            if (OldTextInclude.Equals(txtbIncludeText.Text)) return;
            OldTextInclude = txtbIncludeText.Text;
            txtbHighlight.Text = txtbIncludeText.Text;
            if (string.IsNullOrEmpty(txtbIncludeText.Text))
            {
                chkbIncludeText.Checked = false;
                return;
            }

            chkbHighlight.Checked = false;
            chkbIncludeText.Checked = true;
            RefreshUserFilter();
        }

        private void txtbExclude_TextChanged(object sender, EventArgs e)
        {
            if (OldTextExclude.Equals(txtbExclude.Text)) return;
            Settings.ExcludedText = txtbExclude.Text;
            OldTextExclude = txtbExclude.Text;
            if (string.IsNullOrEmpty(txtbExclude.Text))
            {
                chkExclude.Checked = false;
                return;
            }

            chkExclude.Checked = true;
            RefreshUserFilter();
        }

        private void tmrRefreshFilter_Tick(object sender, EventArgs e)
        {
            if (FilterHasChanged)
            {
                FilterHasChanged = false;
                FilterResults();
            }
        }


        private void RadioButtons_clicked(object sender, EventArgs e)
        {
            chkbIncludeText.Checked = true;
            chkExclude.Checked = true;
            FilterHasChanged = true;
        }

        /// <summary>
        /// Set custom column display text
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridViewCustomColumnDisplayText(object sender, CustomColumnDisplayTextEventArgs e)
        {
            if (e.GroupRowHandle == BaseListSourceDataController.FilterRow &&
                e.Column.FieldName == DataGridDateColumnName)
            {
                e.DisplayText = e.Column.FilterInfo.DisplayText;
            }
        }

        /// <summary>
        /// Called when column filter button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridViewShowFilterPopupListBox(object sender, FilterPopupListBoxEventArgs e)
        {
            if (e.Column.FieldName != DataGridDateColumnName)
            {
                return;
            }

            e.ComboBox.Items.Clear();

            int index = 0;

            #region Create and add custom filter criteria to select the records which refer to the current date.

            // ALL
            ColumnFilterInfo fInfo = new ColumnFilterInfo();
            e.ComboBox.Items.Insert(index++, new FilterItem(Utils.DateFilterNone, fInfo));

            // Today
            fInfo = new ColumnFilterInfo(GetFilterString(DataGridDateColumnName, DateRangeFilter.Today),
                GetFilterDisplayText(DateRangeFilter.Today));
            e.ComboBox.Items.Insert(index++, new FilterItem(Utils.DateFilterToday, fInfo));

            // Last 2 days
            fInfo = new ColumnFilterInfo(GetFilterString(DataGridDateColumnName, DateRangeFilter.Last2Days),
                GetFilterDisplayText(DateRangeFilter.Last2Days));
            e.ComboBox.Items.Insert(index++, new FilterItem(Utils.DateFilterLast2Days, fInfo));
            // Last 3 days
            fInfo = new ColumnFilterInfo(GetFilterString(DataGridDateColumnName, DateRangeFilter.Last3Days),
                GetFilterDisplayText(DateRangeFilter.Last3Days));
            e.ComboBox.Items.Insert(index++, new FilterItem(Utils.DateFilterLast3Days, fInfo));
            // Last week
            fInfo = new ColumnFilterInfo(GetFilterString(DataGridDateColumnName, DateRangeFilter.LastWeek),
                GetFilterDisplayText(DateRangeFilter.LastWeek));
            e.ComboBox.Items.Insert(index++, new FilterItem(Utils.DateFilterLastWeek, fInfo));
            // Last 2 weeks
            fInfo = new ColumnFilterInfo(GetFilterString(DataGridDateColumnName, DateRangeFilter.Last2Weeks),
                GetFilterDisplayText(DateRangeFilter.Last2Weeks));
            e.ComboBox.Items.Insert(index++, new FilterItem(Utils.DateFilterLast2Weeks, fInfo));
            // Last month.
            fInfo = new ColumnFilterInfo(GetFilterString(DataGridDateColumnName, DateRangeFilter.LastMonth),
                GetFilterDisplayText(DateRangeFilter.LastMonth));
            e.ComboBox.Items.Insert(index, new FilterItem(Utils.DateFilterLastMonth, fInfo));

            #endregion Create and add custom filter criteria to select the records which refer to the current date.
        }

        #endregion

        #region Messages logic



        internal DataTable GetFilteredDataTable()
        {

            // Create a data view by applying the grid view row filter
            try
            {
                lockSlim.EnterReadLock();
                return new DataView(_messageData, _messageData.DefaultView.RowFilter, null,
                    DataViewRowState.CurrentRows).ToTable();
            }
            finally
            {
                lockSlim.ExitReadLock();
            }
        }

        private string GetFilterDisplayText(DateRangeFilter filterType)
        {
            string displayText = string.Empty;
            switch (filterType)
            {
                case DateRangeFilter.None:
                    displayText = Utils.DateFilterNone;
                    break;
                case DateRangeFilter.Today:
                    displayText = Utils.DateFilterToday;
                    break;
                case DateRangeFilter.Last2Days:
                    displayText = Utils.DateFilterLast2Days;
                    break;
                case DateRangeFilter.Last3Days:
                    displayText = Utils.DateFilterLast3Days;
                    break;
                case DateRangeFilter.LastWeek:
                    displayText = Utils.DateFilterLastWeek;
                    break;
                case DateRangeFilter.Last2Weeks:
                    displayText = Utils.DateFilterLast2Weeks;
                    break;
                case DateRangeFilter.LastMonth:
                    displayText = Utils.DateFilterLastMonth;
                    break;
            }

            return displayText;
        }

        /// <summary>
        /// Get the filter string for Quick filter
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="filterType"></param>
        /// <returns></returns>
        private string GetFilterString(string columnName, DateRangeFilter filterType)
        {
            DateTime startDate = DateTime.Today;
            DateTime endDate = DateTime.Today;

            switch (filterType)
            {
                // The filter expression for the TODAY item.
                case DateRangeFilter.Today:
                    startDate = DateTime.Today;
                    endDate = startDate.AddDays(1);
                    break;

                // The filter expression for the last 2 days item.
                case DateRangeFilter.Last2Days:
                    endDate = DateTime.Today.AddDays(1);
                    startDate = DateTime.Today.AddDays(-1);
                    break;

                // The filter expression for the last 3 days item.
                case DateRangeFilter.Last3Days:
                    endDate = DateTime.Today.AddDays(1);
                    startDate = DateTime.Today.AddDays(-2);
                    break;

                // The filter expression for the last week item.
                case DateRangeFilter.LastWeek:
                    endDate = DateTime.Today.AddDays(1);
                    startDate = DateTime.Today.AddDays(-7);
                    break;

                // The filter expression for the last 2 weeks item.
                case DateRangeFilter.Last2Weeks:
                    endDate = DateTime.Today.AddDays(1);
                    startDate = DateTime.Today.AddDays(-14);
                    break;

                // The filter expression for the last month item.
                case DateRangeFilter.LastMonth:
                    endDate = DateTime.Today.AddDays(1);
                    startDate = DateTime.Today.AddMonths(-1);
                    break;
            }

            string startDateStr = "#" + startDate.ToString("g", CultureInfo.CreateSpecificCulture("en-US")) + "#";
            string endDateStr = "#" + endDate.ToString("g", CultureInfo.CreateSpecificCulture("en-US")) + "#";
            var filter = "([" + columnName + "] >= " + startDateStr + " AND [" + columnName + "] < " + endDateStr + ")";
            return filter;
        }


        #endregion


        public void AppendMessage(AnalogyLogMessage message, string dataSource)
        {
            if (message.Level==AnalogyLogLevel.Disabled)
                return; //ignore those messages
            
            if (Settings.IdleMode && Utils.IdleTime().TotalMinutes > Settings.IdleTimeMinutes)
            {
                PagingManager.IncrementTotalMissedMessages();
            }
            lockSlim.EnterWriteLock();
            if (ExternalWindowsCount > 0)
            {
                foreach (XtraFormLogGrid grid in ExternalWindows)
                {
                    grid.AppendMessage(message, dataSource);
                }
            }


            DataRow dtr = PagingManager.AppendMessage(message, dataSource);
            if (diffStartTime > DateTime.MinValue)
            {
                dtr["TimeDiff"] = message.Date.Subtract(diffStartTime).ToString();
            }

            if (hasAnyInPlaceExtensions)
            {
                foreach (IAnalogyExtension extension in InPlaceRegisteredExtensions)
                {
                    var columns = extension.GetColumnsInfo();
                    foreach (AnalogyColumnInfo column in columns)
                    {
                        dtr[column.ColumnName] = extension.GetValueForCellColumn(message, column.ColumnName);
                    }
                }
            }

            if (hasAnyUserControlExtensions)
            {
                foreach (IAnalogyExtension extension in UserControlRegisteredExtensions)
                {
                    extension.NewMessage(message);
                }
            }
            lockSlim.ExitWriteLock();
            if (PagingManager.IsCurrentPageInView(_messageData))
                NewDataExist = true;
        }

        public void AppendMessages(List<AnalogyLogMessage> messages, string dataSource)
        {

            if (Settings.IdleMode && Utils.IdleTime().TotalMinutes > Settings.IdleTimeMinutes)
            {
                PagingManager.IncrementTotalMissedMessages();
            }
            lockSlim.EnterWriteLock();
            if (ExternalWindowsCount > 0)
            {
                foreach (XtraFormLogGrid grid in ExternalWindows)
                {
                    grid.AppendMessages(messages, dataSource);
                }
            }

            foreach (var (dtr, message) in PagingManager.AppendMessages(messages, dataSource))
            {
                if (diffStartTime > DateTime.MinValue)
                {
                    dtr["TimeDiff"] = message.Date.Subtract(diffStartTime).ToString();
                }

                if (hasAnyInPlaceExtensions)
                {
                    foreach (IAnalogyExtension extension in InPlaceRegisteredExtensions)
                    {
                        var columns = extension.GetColumnsInfo();
                        foreach (AnalogyColumnInfo column in columns)
                        {
                            dtr[column.ColumnName] = extension.GetValueForCellColumn(message, column.ColumnName);
                        }
                    }
                }

                dtr.EndEdit();
            }
            lockSlim.ExitWriteLock();
            if (PagingManager.IsCurrentPageInView(_messageData))
                NewDataExist = true;

            if (hasAnyUserControlExtensions)
            {
                foreach (IAnalogyExtension extension in UserControlRegisteredExtensions)
                {
                    extension.NewMessages(messages);
                }
            }

        }

        public void AcceptChanges(DataTable table, string source)
        {
            if (!IsHandleCreated) return;
            BeginInvoke(new MethodInvoker(() =>
            {
                lock (table.Rows.SyncRoot)
                {
#if DEBUG
                    Console.WriteLine(source);
#endif
                    logGrid.BeginDataUpdate();
                    table.AcceptChanges();
                    logGrid.EndDataUpdate();
                }
            }));
        }
        public void AcceptChanges(bool forceRefresh)
        {
            if (!IsHandleCreated) return;
            if (_realtimeUpdate || forceRefresh)

                BeginInvoke(new MethodInvoker(() =>
                {
                    lockSlim.EnterWriteLock();
                    try
                    {
                        logGrid.BeginDataUpdate();
                        _messageData.AcceptChanges();
                        logGrid.EndDataUpdate();
                        RefreshUIMessagesCount();
                    }
                    finally
                    {
                        lockSlim.ExitWriteLock();
                    }

                }));
        }

        private void UpdatePage(DataTable page)
        {

            _messageData = page;
            lockSlim.EnterWriteLock();
            try
            {
                gridControl.DataSource = _messageData.DefaultView;
                //NewDataExist = true;
                //FilterHasChanged = true;
                lblPageNumber.Text = $"Page {pageNumber} / {TotalPages}";
                NewDataExist = true;
                FilterResults();
            }
            finally
            {
                lockSlim.ExitWriteLock();
            }
        }

        public void FilterResults(string module)
        {
            txtbIncludeModule.Text = module;
            FilterResults();
        }
        private void FilterResults()
        {
            _filterCriteriaInline.TextInclude = chkbIncludeText.Checked ? txtbIncludeText.Text : string.Empty;
            _filterCriteriaInline.TextExclude =
                chkExclude.Checked ? txtbExclude.Text + "|" + string.Join("|", _excludeMostCommon) : string.Empty;
            _filterCriteriaInline.Levels = null;
            if (chkLstLogLevel.Items[0].CheckState == CheckState.Checked)
                _filterCriteriaInline.Levels = new[] { AnalogyLogLevel.Trace,AnalogyLogLevel.Disabled};
            if (chkLstLogLevel.Items[1].CheckState == CheckState.Checked)
                _filterCriteriaInline.Levels = new[] { AnalogyLogLevel.Error, AnalogyLogLevel.Critical, AnalogyLogLevel.Disabled };
            else if (chkLstLogLevel.Items[2].CheckState == CheckState.Checked)
                _filterCriteriaInline.Levels = new[] { AnalogyLogLevel.Warning, AnalogyLogLevel.Disabled };
            else if (chkLstLogLevel.Items[3].CheckState == CheckState.Checked)
                _filterCriteriaInline.Levels = new[] { AnalogyLogLevel.Debug, AnalogyLogLevel.Disabled };
            else if (chkLstLogLevel.Items[4].CheckState == CheckState.Checked)
                _filterCriteriaInline.Levels = new[] { AnalogyLogLevel.Verbose, AnalogyLogLevel.Disabled };


            if (chkbIncludeModules.Checked)
            {
                _filterCriteriaInline.Modules = string.IsNullOrEmpty(txtbIncludeModule.Text)
                    ? null
                    : txtbIncludeModule.Text.Split(new[] { ',', '|' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(val => val.Trim()).ToArray();
            }
            else
            {
                _filterCriteriaInline.Modules = null;
            }

            if (chkbIncludeSources.Checked)
            {
                _filterCriteriaInline.Sources = string.IsNullOrEmpty(txtbIncludeSource.Text)
                    ? null
                    : txtbIncludeSource.Text.Split(new[] { ',', '|' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(val => val.Trim()).ToArray();
            }
            else
            {
                _filterCriteriaInline.Sources = null;
            }

            if (chkbExcludeSources.Checked)
            {
                _filterCriteriaInline.ExcludedSources = string.IsNullOrEmpty(txtbExcludeSource.Text)
                    ? null
                    : txtbExcludeSource.Text.Split(new[] { ',', '|' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(val => val.Trim()).ToArray();
            }
            else
            {
                _filterCriteriaInline.ExcludedSources = null;
            }
            if (chkbExcludeModules.Checked)
            {
                _filterCriteriaInline.ExcludedModules = string.IsNullOrEmpty(txtbExcludeModule.Text)
                    ? null
                    : txtbExcludeModule.Text.Split(new[] { ',', '|' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(val => val.Trim()).ToArray();
            }
            else
            {
                _filterCriteriaInline.ExcludedModules = null;
            }


            lockSlim.EnterWriteLock();
            try
            {
                _messageData.BeginLoadData();
                _messageData.DefaultView.RowFilter = _filterCriteriaInline.GetSqlExpression();
                _messageData.EndLoadData();
            }
            finally
            {
                lockSlim.ExitWriteLock();
            }

            var location = LocateByValue(0, gridColumnObject, _currentMassage);
            if (location >= 0)
                logGrid.FocusedRowHandle = location;
            logGrid.RefreshData();
            RefreshUIMessagesCount();

        }
        public virtual int LocateByValue(int startRowHandle, GridColumn column, AnalogyLogMessage val)
        {
            if (!logGrid.DataController.IsReady || val == null)
                return int.MinValue;
            startRowHandle = Math.Max(0, startRowHandle);
            if (logGrid.IsServerMode)
            {
                if (startRowHandle != 0)
                    throw new ArgumentException("Argument must be '0' in server mode.", nameof(startRowHandle));
            }
            try
            {
                if (logGrid.IsServerMode)
                    return logGrid.DataController.FindRowByValue(column.FieldName, val, null);
                for (int rowHandle = startRowHandle; rowHandle < logGrid.DataController.VisibleListSourceRowCount; ++rowHandle)
                {
                    object rowCellValue = logGrid.GetRowCellValue(rowHandle, column.Caption);
                    if (Equals(val, rowCellValue))
                        return rowHandle;
                }
            }
            catch
            {
            }
            return int.MinValue;
        }
        private void RefreshUIMessagesCount()
        {
            if (!IsHandleCreated) return;
            BeginInvoke(new MethodInvoker(() =>
            {
                var msgs = Messages.ToList();
                lblTotalMessages.Text =
                    $"Total messages:{msgs.Count}. Errors:{msgs.Count(m => m.Level == AnalogyLogLevel.Error)}. Warnings:{msgs.Count(m => m.Level == AnalogyLogLevel.Warning)}. Criticals:{msgs.Count(m => m.Level == AnalogyLogLevel.Critical)}";
            }));
        }

        public void RefreshUI()
        {
            //gridColumnDataSource.VisibleIndex = 0;
            //gridColumnDate.VisibleIndex = 1;
            //gridColumnText.VisibleIndex = 2;
            //gridColumnSource.VisibleIndex = 3;
            //gridColumnLevel.VisibleIndex = 4;
            //gridColumnClass.VisibleIndex = 5;
            //gridColumnModule.VisibleIndex = 6;
            //gridColumnProcessID.VisibleIndex = 7;
            //gridColumnUser.VisibleIndex = 8;
            //gridColumnCategory.VisibleIndex = 9;
            //gridColumnAudit.VisibleIndex = 10;

        }

        public async Task LoadFilesAsync(List<string> fileNames, bool clearLogBeforeLoading)
        {
            cancellationTokenSource = new CancellationTokenSource();
            CancellationToken token = cancellationTokenSource.Token;
            if (clearLogBeforeLoading)
                ClearLogs(false);
            sBtnCancel.Visible = true;
            progressBar1.Value = 0;
            progressBar1.Maximum = fileNames.Count;
            progressBar1.Style = fileNames.Count > 1 ? ProgressBarStyle.Continuous : ProgressBarStyle.Marquee;

            progressBar1.Visible = true;
            int processed = 0;
            foreach (string filename in fileNames)
            {
                if (!File.Exists(filename))
                {
                    AnalogyLogMessage m = new AnalogyLogMessage($"File {filename} does not exist", AnalogyLogLevel.Critical, AnalogyLogClass.General, "Analogy", "None");
                    AppendMessage(m, "Analogy");
                    continue;
                }

                Text = @"File: " + filename;
                FileProcessor fp = new FileProcessor(this);
                await fp.Process(FileDataProvider, filename, cancellationTokenSource.Token);
                processed++;
                ProgressReporter.Report(new AnalogyProgressReport("Processed", processed, fileNames.Count, filename));
                if (token.IsCancellationRequested)
                {
                    progressBar1.Visible = false;
                    break;
                }
            }

            sBtnCancel.Visible = false;
        }

        private void ClearLogs(bool raiseEvent)
        {

            lockSlim.EnterWriteLock();

            if (raiseEvent)
            {
                OnHistoryCleared?.Invoke(this, new AnalogyClearedHistoryEventArgs(PagingManager.GetAllMessages()));
            }

            PagingManager.ClearLogs();
            pageNumber = 1;
            UpdatePage(PagingManager.FirstPage());
            AcceptChanges(true);
            rtxtContent.Text = string.Empty;
            if (BookmarkView)
                BookmarkPersistManager.Instance.ClearBookmarks();
            lockSlim.ExitWriteLock();

        }


        private void LoadTextBoxes(AnalogyLogMessage m)
        {
            if (InvokeRequired)
                BeginInvoke(new MethodInvoker(() => rtxtContent.Text = m.Text));
            else
            {
                rtxtContent.Text = m.Text;
            }

        }

        private void tsmiOTAFull_Click(object sender, EventArgs e)
        {
            if (EnableOTA)
            {
                AnalogyOTAClient client = new AnalogyOTAClient(GetFilteredDataTable());
                client.Show(this);
            }
            else
            {
                MessageBox.Show("Sending logs feature has been disabled", "Operation Is Not allowed",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void tsBtnShare_Click(object sender, EventArgs e)
        {
            if (EnableOTA)
            {
                AnalogyOTAClient client = new AnalogyOTAClient(GetFilteredDataTable());
                client.Show(this);
            }
            else
            {
                XtraMessageBox.Show("Sharing logs feature has been disabled", "Operation Is Not allowed",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        private void tmrNewData_Tick(object sender, EventArgs e)
        {
            if (NewDataExist)
            {
                NewDataExist = false;
                AcceptChanges(false);
            }

        }

        private void txtbInclude_MouseEnter(object sender, EventArgs e)
        {
            txtbIncludeText.Focus();
            txtbIncludeText.SelectAll();
        }

        private void txtbInclude_Enter(object sender, EventArgs e)
        {
            txtbIncludeText.SelectAll();
        }

        private void txtbExcludeSource_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtbExcludeSource.Text))
            {
                chkbExcludeSources.Checked = false;
            }
            else
            {
                if (!chkbExcludeSources.Checked)
                    chkbExcludeSources.Checked = true;
            }

            RefreshUserFilter();
            Settings.ExcludedSource = txtbExcludeSource.Text;

        }

        private void txtbExcludeModule_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtbExcludeModule.Text))
            {
                chkbExcludeModules.Checked = false;
            }
            else
            {
                if (!chkbExcludeModules.Checked)
                    chkbExcludeModules.Checked = true;
            }
            RefreshUserFilter();
            Settings.ExcludedModules = txtbExcludeModule.Text;
        }

        private void chkbExcludeSourceAndModule_CheckedChanged(object sender, EventArgs e)
        {
            FilterHasChanged = true;
        }

        private void gridControlBookmarkedMessages_DoubleClick(object sender, EventArgs e)
        {
            if (!(e is DXMouseEventArgs args))
                return;
            GoToMessage();

        }

        private void pmsGrid_DoubleClick(object sender, EventArgs e)
        {
            if (!(e is DXMouseEventArgs args))
                return;
            OpenMessageDetails();
            //CreateBookmark();

        }

        private void OpenMessageDetails()
        {
            (AnalogyLogMessage message, string dataSource) = GetMessageFromSelectedRowInGrid();
            if (message == null) return;
            FormMessageDetails details = new FormMessageDetails(message, Messages, dataSource);
            details.Show(this);
        }

        private void tsmiBookmark_Click(object sender, EventArgs e)
        {
            CreateBookmark(false);
        }

        private void CreateBookmark(bool persists)
        {

            (AnalogyLogMessage message, _) = GetMessageFromSelectedRowInGrid();
            int[] selRows = logGrid.GetSelectedRows();
            if (message == null) return;
            lockSlim.EnterWriteLock();

            DataRow dtr = _bookmarkedMessages.NewRow();
            dtr["Date"] = message.Date;
            dtr["Text"] = message.Text ?? "";
            dtr["Source"] = message.Source ?? "";
            dtr["Level"] = message.Level.ToString();
            dtr["Class"] = message.Class.ToString();
            dtr["Category"] = message.Category ?? "";
            dtr["User"] = message.User ?? "";
            dtr["Module"] = message.Module ?? "";
            dtr["Object"] = message;
            dtr["ProcessID"] = message.ProcessID;
            string dataSource = (string)logGrid.GetRowCellValue(selRows.First(), "DataProvider");
            dtr["DataProvider"] = dataSource;
            if (diffStartTime > DateTime.MinValue)
            {
                dtr["TimeDiff"] = message.Date.Subtract(diffStartTime).ToString();
            }

            _bookmarkedMessages.Rows.Add(dtr);
            _bookmarkedMessages.AcceptChanges();
            btswitchExpand.Checked = true;
            splitContainerMain.Collapsed = false;
            tcBottom.SelectedTabPage = xtpBookmarks;
            if (persists)
                BookmarkPersistManager.Instance.AddBookmarkedMessage(message, dataSource);
            lockSlim.ExitWriteLock();
        }

        private void GoToMessage()
        {
            int[] selRows = gridViewBookmarkedMessages.GetSelectedRows();
            if (selRows == null || selRows.Length != 1) return;
            int rownum = selRows.First();
            var currentRow = (DataRowView)gridViewBookmarkedMessages.GetRow(rownum);
            try
            {
                var LogMessage = currentRow["Object"] as AnalogyLogMessage;
                var location = LocateByValue(0, gridColumnObject, LogMessage);
                if (location >= 0)
                    logGrid.FocusedRowHandle = location;
                else
                    XtraMessageBox.Show("Cannot go to message", "Message not found", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
            }
            catch (Exception)
            {

                XtraMessageBox.Show("Cannot go to message", "Message not found", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void tsBtnopyBookmark_Click(object sender, EventArgs e)
        {

        }

        private void tsbtnClearBookmarks_Click(object sender, EventArgs e)
        {
            _bookmarkedMessages.Clear();
        }

        private void chkbHighlight_CheckedChanged(object sender, EventArgs e)
        {
            FilterHasChanged = true;
        }

        private void txtbHighlight_KeyUp(object sender, KeyEventArgs e)
        {
            chkbHighlight.Checked = !string.IsNullOrEmpty(txtbHighlight.Text);
            HighlightRows.Clear();
            FilterHasChanged = true;
        }

        private void tsmiExcludeSource_Click(object sender, EventArgs e)
        {

            (AnalogyLogMessage message, _) = GetMessageFromSelectedRowInGrid();
            txtbExcludeSource.Text = txtbExcludeSource.Text + "," + message?.Source;
        }

        private void tsmiExcludeModule_Click(object sender, EventArgs e)
        {
            (AnalogyLogMessage message, _) = GetMessageFromSelectedRowInGrid();
            txtbExcludeModule.Text = txtbExcludeModule.Text + "," + message?.Module;
        }

        private void txtbInclude_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                autoCompleteInclude.Add(txtbIncludeText.Text);
            }
        }

        private void txtbExclude_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                autoCompleteExclude.Add(txtbExclude.Text);
            }
        }

        private void tsmiTimeDiff_Click(object sender, EventArgs e)
        {
            (AnalogyLogMessage message, _) = GetMessageFromSelectedRowInGrid();
            if (message != null)
            {
                diffStartTime = message.Date;
                UpdateTimes();
            }
        }

        private void UpdateTimes()
        {
            gridColumnTimeDiff.Visible = true;
            gridColumnTimeDiff.VisibleIndex = 2;

            lockSlim.EnterWriteLock();
            _messageData.BeginLoadData();
            foreach (DataRow row in _messageData.Rows)
            {
                AnalogyLogMessage message = (AnalogyLogMessage)row["Object"];
                //row["TimeDiff"] = message.Date.Subtract(diffStartTime).ToString("d\\.hh\\:mm\\:ss\\.fff");
                row["TimeDiff"] = message.Date.Subtract(diffStartTime).ToString();
            }

            _messageData.EndLoadData();
            AcceptChanges(true);
            gridControl.RefreshDataSource();
            lockSlim.ExitWriteLock();
        }

        private async void tsbtnImport_Click(object sender, EventArgs e)
        {

            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter =
                "Plain XML log file (*.log)|*.log|JSON file (*.json)|*.json|NLOG file (*.nlog)|*.nlog|Zipped XML log file (*.zip)|*.zip|ETW log file (*.etl)|*.etl";
            openFileDialog1.Title = @"Import file to current view";
            openFileDialog1.Multiselect = false;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    await LoadFilesAsync(new List<string> { openFileDialog1.FileName }, false);
                }
                catch (Exception exception)
                {
                    XtraMessageBox.Show(exception.Message, @"Error Opening file", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }

            }

        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            if (HighlightRows.Any() && logGrid.GetSelectedRows().Any())
            {
                int selected = logGrid.GetSelectedRows().First();
                if (HighlightRows.All(r => r >= selected))
                    logGrid.SelectRow(HighlightRows.Last());
                else
                    logGrid.SelectRow(HighlightRows.First(r => r < selected));
            }
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            if (HighlightRows.Any() && logGrid.GetSelectedRows().Any())
            {
                int selected = logGrid.GetSelectedRows().First();
                if (HighlightRows.All(r => r <= selected))
                    logGrid.SelectRow(HighlightRows.First());
                else
                    logGrid.SelectRow(HighlightRows.First(r => r > selected));
            }
        }

        private void btswitchExpand_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            splitContainerMain.Collapsed = btswitchExpand.Checked;
        }

        private void btswitchRefreshLog_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            _realtimeUpdate = btswitchRefreshLog.Checked;
            AcceptChanges(false);
            //btswitchRefreshLog.Caption = _realtimeUpdate ? "Refresh log:" : "Paused:";
        }

        private void bBtnSaveLog_ItemClick(object sender, ItemClickEventArgs e)
        {
            var messages = Messages;
            SaveMessagesToLog(FileDataProvider, messages);
        }

        private async void SaveMessagesToLog(IAnalogyOfflineDataProvider fileHandler, List<AnalogyLogMessage> messages)
        {

            if (fileHandler != null && fileHandler.CanSaveToLogFile)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = fileHandler.FileSaveDialogFilters;

                if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
                {
                    try
                    {
                        await fileHandler.SaveAsync(messages, saveFileDialog.FileName);
                    }
                    catch (Exception e)
                    {
                        XtraMessageBox.Show(e.Message, @"Error Saving file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
            }
            else
            {
                if (XtraMessageBox.Show("Current Data Source does not support Save Operation" + Environment.NewLine + "Do you want to Save in Analogy XML Format?", @"Save not Supported", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
                {
                    SaveMessagesToLog(AnalogyOfflineDataProvider, messages);
                    //SaveFileDialog saveFileDialog = new SaveFileDialog();
                    //saveFileDialog.Filter = "Plain XML log file - Analogy Data format (*.xml)| *.xml";

                    //if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
                    //{
                    //    try
                    //    {
                    //        AnalogyXmlLogFile save = new AnalogyXmlLogFile();
                    //        save.Save(Messages, saveFileDialog.FileName);
                    //        XtraMessageBox.Show("Operation Completed", $"Messages were saved to {saveFileDialog.FileName}", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //    }
                    //    catch (Exception e)
                    //    {
                    //        XtraMessageBox.Show(e.Message, @"Error Saving file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //    }
                    //}
                }
                else
                {
                    XtraMessageBox.Show("Operation Aborted", @"Save file", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

        }
        private async void bBtnImport_ItemClick(object sender, ItemClickEventArgs e)
        {

            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter =
                "Plain XML log file (*.log)|*.log|JSON file (*.json)|*.json|NLOG file (*.nlog)|*.nlog|Zipped XML log file (*.zip)|*.zip|ETW log file (*.etl)|*.etl";
            openFileDialog1.Title = @"Import file to current view";
            openFileDialog1.Multiselect = false;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    await LoadFilesAsync(new List<string> { openFileDialog1.FileName }, false);
                }
                catch (Exception exception)
                {
                    XtraMessageBox.Show(exception.Message, @"Error Opening file", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }

            }
        }

        private void bBtnClearLog_ItemClick(object sender, ItemClickEventArgs e)
        {
            ClearLogs(true);
        }

        private void sBtnMostCommon_Click(object sender, EventArgs e)
        {
            List<string> items;

            lockSlim.EnterReadLock();
            items = Messages.Select(r => r.Text).ToList();
            lockSlim.ExitReadLock();

            AnalogyExclude ef = new AnalogyExclude(items, _excludeMostCommon);
            if (ef.ShowDialog(this) == DialogResult.OK)
            {
                _excludeMostCommon = AnalogyExclude.GlobalExclusion;
                chkExclude.Checked = true;
                FilterHasChanged = true;
            }
        }

        private void chkLstLogLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            chkbIncludeText.Checked = true;
            chkExclude.Checked = true;
            FilterHasChanged = true;
        }

        private void chkLstLogLevel_DrawItem(object sender, ListBoxDrawItemEventArgs e)
        {
            switch (e.Index)
            {
                case 1:
                    e.Appearance.BackColor = Color.Red;
                    break;
                case 2:
                    e.Appearance.BackColor = Color.Yellow;
                    break;
            }

        }

        private void chkLstLogLevel_ItemCheck(object sender, DevExpress.XtraEditors.Controls.ItemCheckEventArgs e)
        {
            chkbIncludeText.Checked = true;
            chkExclude.Checked = true;
            FilterHasChanged = true;
        }

        private void sBtnLength_Click(object sender, EventArgs e)
        {
            nudGroupBychars.Value = Math.Max(txtbGroupByCharsLimit.Text.Length, nudGroupBychars.Value);
            ApplyGrouping();
        }

        private void sBtnGroup_Click(object sender, EventArgs e)
        {
            ApplyGrouping();

        }

        private void ApplyGrouping()
        {
            List<IGrouping<string, AnalogyLogMessage>> grouped = Messages
                .GroupBy(s => s.Text.Substring(0, Math.Min(s.Text.Length, (int)nudGroupBychars.Value)))
                .OrderByDescending(i => i.Count()).ToList();
            groupingByChars = grouped.ToDictionary(g => g.Key, g => g.ToList());
            gCtrlGrouping.DataSource = groupingByChars.Keys;
        }
        private void bBtnCopyButtom_ItemClick(object sender, ItemClickEventArgs e)
        {
            Clipboard.SetText(rtxtContent.Text);
        }

        private void bBtnButtomExpand_ItemClick(object sender, ItemClickEventArgs e)
        {
        }

        private void barToggleSwitchItem1_CheckedChanged(object sender, ItemClickEventArgs e)
        {

            splitContainerMain.Collapsed = !btSwitchExpandButtomMessage.Checked;

        }

        private void bBtnGoToMessage_ItemClick(object sender, ItemClickEventArgs e)
        {
            GoToMessage();
        }

        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            _bookmarkedMessages.Clear();
        }

        private void bBtnopyBookmarked_ItemClick(object sender, ItemClickEventArgs e)
        {
            int[] selRows = gridViewBookmarkedMessages.GetSelectedRows();

            if (selRows != null && selRows.Length == 1)
            {
                var message = (AnalogyLogMessage)gridViewBookmarkedMessages.GetRowCellValue(selRows.First(), "Object");
                Clipboard.SetText(message.Text);
            }
        }

        private void tsmiSaveLayout_Click(object sender, EventArgs e)
        {
            SaveGridLayout();
        }

        private void SaveGridLayout()
        {
            try
            {
                gridControl.MainView.SaveLayoutToXml(LayoutFileName);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }
        private void tsmiBookmarkPersist_Click(object sender, EventArgs e)
        {
            CreateBookmark(true);
        }

        private void tsmiRemoveBookmark_Click(object sender, EventArgs e)
        {
            RemoveBookmark();
        }

        private void bBtnRemoveBoomark_ItemClick(object sender, ItemClickEventArgs e)
        {
            RemoveBookmark();
        }

        private void RemoveBookmark()
        {
            (AnalogyLogMessage message, _) = GetMessageFromSelectedRowInGrid();
            if (message != null)
                BookmarkPersistManager.Instance.RemoveBookmark(message);

        }
        //private void logGrid_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        //{
        //    if (e.Column.FieldName == "gridColumnLevelImage")
        //    {
        //        string severity = logGrid.GetListSourceRowCellValue(e.ListSourceRowIndex, gridView1.Columns["Level"])
        //            .ToString();
        //        LogLevel level = Utils.GetLogLevel(severity);
        //        switch (level)
        //        {
        //            case LogLevel.Critical:
        //            case LogLevel.Error:
        //                e.Value = imageList.Images[0];
        //                break;
        //            case LogLevel.Warning:
        //                e.Value = imageList.Images[1];
        //                break;
        //            case LogLevel.Event:
        //                e.Value = imageList.Images[2];
        //                break;
        //            case LogLevel.Verbose:
        //                e.Value = imageList.Images[2];
        //                break;
        //            case LogLevel.Debug:
        //                e.Value = imageList.Images[1];
        //                break;
        //            case LogLevel.Disabled:
        //                e.Value = imageList.Images[1];
        //                break;
        //            default:
        //                throw new ArgumentOutOfRangeException();
        //        }

        //    }
        //}

        public void SetBookmarkMode()
        {
            BookmarkView = true;
            BookmarkModeUI();
        }

        public void RemoveMessage(AnalogyLogMessage msgMessage)
        {
            var row = _messageData.AsEnumerable().SingleOrDefault(r => r["Object"] == msgMessage);
            if (row != null)
                _messageData.Rows.Remove(row);
        }

        private void cmsMessageOperation_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            (AnalogyLogMessage message, _) = GetMessageFromSelectedRowInGrid();
            if (message != null)
            {
                tsmiExcludeModule.Text = $"Exclude Process: {message.Module}";
                tsmiExcludeSource.Text = $"Exclude Source: {message.Source}";
            }
        }

        private (AnalogyLogMessage, string) GetMessageFromSelectedRowInGrid()
        {
            int[] selRows = logGrid.GetSelectedRows();
            if (selRows == null || !selRows.Any()) return (null, string.Empty);
            var row = selRows.First();
            string datasource = (string)logGrid.GetRowCellValue(row, "DataProvider");
            AnalogyLogMessage message = (AnalogyLogMessage)logGrid.GetRowCellValue(selRows.First(), "Object");
            return (message, datasource);

        }

        private void cmsBookmarked_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            (AnalogyLogMessage message, _) = GetMessageFromSelectedRowInGrid();
            if (message != null)
            {
                tsmiExcludeModuleBookmark.Text = $"Exclude Process: {message.Module}";
                tsmiExcludeSourceBookmark.Text = $"Exclude Source: {message.Source}";
            }
        }


        private void gridViewGrouping_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            var grouped = Utils.DataTableConstructor();
            string key =
                (string)gridViewGrouping.GetRowCellValue(e.FocusedRowHandle, gridViewGrouping.Columns.First());
            var messages = groupingByChars[key];
            foreach (var message in messages)
            {

                DataRow dtr = grouped.NewRow();

                dtr["Date"] = message.Date;
                dtr["Text"] = message.Text ?? "";
                dtr["Source"] = message.Source ?? "";
                dtr["Level"] = message.Level;
                dtr["Class"] = message.Class;
                dtr["Category"] = message.Category ?? "";
                dtr["User"] = message.User ?? "";
                dtr["Module"] = message.Module ?? "";
                dtr["Object"] = message;
                dtr["ProcessID"] = message.ProcessID;
                dtr["DataProvider"] = "";
                if (diffStartTime > DateTime.MinValue)
                {
                    dtr["TimeDiff"] = message.Date.Subtract(diffStartTime).ToString();
                }

                grouped.Rows.Add(dtr);

            }

            grouped.AcceptChanges();
            gridControlMessageGrouping.DataSource = grouped;
        }

        private void sBtnCancel_Click(object sender, EventArgs e)
        {
            cancellationTokenSource.Cancel(false);
            cancellationTokenSource = new CancellationTokenSource();
            sBtnCancel.Visible = false;
        }

        private void tsmiCopyMessages_Click(object sender, EventArgs e)
        {
            var messages = Messages;
            string all = string.Join(Environment.NewLine, messages.Select(m => $"{m.Date.ToString()}: {m.Text}"));
            Clipboard.SetText(all);
        }

        private void bBtnCopyAllBookmarks_ItemClick(object sender, ItemClickEventArgs e)
        {
            var messages = BookmarkedMessages;
            if (!messages.Any()) return;
            string all = string.Join(Environment.NewLine, messages.Select(m => $"{m.Date.ToString()}: {m.Text}"));
            Clipboard.SetText(all);
        }

        private void btsAutoScrollToBottom_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            Settings.AutoScrollToLastMessage = btsAutoScrollToBottom.Checked;
        }

        private void pmsGrid_Click(object sender, EventArgs e)
        {
            if (btsAutoScrollToBottom.Checked)
                btsAutoScrollToBottom.Checked = false;
        }

        private void sbtnPageFirst_Click(object sender, EventArgs e)
        {
            pageNumber = 1;
            UpdatePage(PagingManager.FirstPage());
        }

        private void sbtnPagePrevious_Click(object sender, EventArgs e)
        {
            if (pageNumber == 1) return;
            pageNumber--;
            UpdatePage(PagingManager.PrevPage().Data);
        }

        private void sBtnPageNext_Click(object sender, EventArgs e)
        {
            if (pageNumber == TotalPages) return;
            pageNumber++;
            UpdatePage(PagingManager.NextPage().Data);
        }

        private void sBtnLastPage_Click(object sender, EventArgs e)
        {
            pageNumber = TotalPages;
            UpdatePage(PagingManager.LastPage());
        }

        private void bBtnExportExcel_ItemClick(object sender, ItemClickEventArgs e)
        {


            var count = logGrid.RowCount;

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel file XLSX (*.xlsx)|*.xlsx|Excel file XLS (*.XLS)|*.xls";

            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                if (saveFileDialog.FilterIndex == 1)
                {
                    if (count > 1048576)
                    {
                        XtraMessageBox.Show($"XLSX files are limited to 1,048,576 rows (and 16,384 columns). You have {count} rows", "Export Aborted");
                    }
                    else
                    {
                        logGrid.ExportToXlsx(saveFileDialog.FileName);
                        OpenFolder(saveFileDialog.FileName);
                    }
                }
                if (saveFileDialog.FilterIndex == 2)
                {
                    if (count > 65536)
                    {
                        XtraMessageBox.Show($"XLS files are limited to 65,536 rows (and 256 columns). You have {count} rows", "Export Aborted");
                    }
                    else
                    {
                        logGrid.ExportToXls(saveFileDialog.FileName);
                        OpenFolder(saveFileDialog.FileName);
                    }
                }
            }
        }

        private void bBtnExportCSV_ItemClick(object sender, ItemClickEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Comma Separated File (*.csv)|*.csv";

            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                logGrid.ExportToCsv(saveFileDialog.FileName);
                OpenFolder(saveFileDialog.FileName);
            }
        }

        private void bBtnExportHtml_ItemClick(object sender, ItemClickEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "HTML File (*.html)|*.html";

            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                HtmlExportOptions op = new HtmlExportOptions();
                op.ExportMode = HtmlExportMode.SingleFile;
                logGrid.ExportToHtml(saveFileDialog.FileName, op);
                OpenFolder(saveFileDialog.FileName);
            }
        }
        private void OpenFolder(string filename)
        {
            try
            {
                Process.Start("explorer.exe", "/select, \"" + filename + "\"");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, @"Error Opening file location", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
        private void bBtnUndockView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var msg = Messages;
            if (!msg.Any()) return;
            var source = GetFilteredDataTable().Rows[0]?["DataProvider"]?.ToString();
            if (source == null) return;
            XtraFormLogGrid grid = new XtraFormLogGrid(msg, source);
            lockExternalWindowsObject.EnterWriteLock();
            _externalWindows.Add(grid);
            Interlocked.Increment(ref ExternalWindowsCount);
            lockExternalWindowsObject.ExitWriteLock();
            grid.FormClosing += (s, arg) =>
            {
                lockExternalWindowsObject.EnterWriteLock();
                Interlocked.Decrement(ref ExternalWindowsCount);
                _externalWindows.Remove(grid);
                lockExternalWindowsObject.ExitWriteLock();
            };
            grid.Show(this);
        }

        private void bBtnSaveEntireLog_ItemClick(object sender, ItemClickEventArgs e)
        {
            var messages = PagingManager.GetAllMessages();
            SaveMessagesToLog(FileDataProvider, messages);
        }

        private void logGrid_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            int row = e.FocusedRowHandle;
            if (row < 0) return;
            LoadTextBoxes((AnalogyLogMessage)logGrid.GetRowCellValue(e.FocusedRowHandle, "Object"));
        }

        private void tsmiIncreaseFont_Click(object sender, EventArgs e)
        {
            Settings.FontSize = logGrid.Appearance.Row.Font.Size + 2;
            logGrid.Appearance.Row.Font = new Font(logGrid.Appearance.Row.Font.Name, Settings.FontSize);
            SaveGridLayout();
        }

        private void tsmiDecreaseFont_Click(object sender, EventArgs e)
        {
            if (logGrid.Appearance.Row.Font.Size < 5) return;
            {
                Settings.FontSize = logGrid.Appearance.Row.Font.Size - 2;
                logGrid.Appearance.Row.Font = new Font(logGrid.Appearance.Row.Font.Name, Settings.FontSize);
                SaveGridLayout();
            }
        }

        private void tsmiClearLog_Click(object sender, EventArgs e)
        {
            ClearLogs(true);
        }

        private void tsmiREmoveAllPreviousMessages_Click(object sender, EventArgs e)
        {
            (AnalogyLogMessage current, _) = GetMessageFromSelectedRowInGrid();
            if (current == null) return;

            lockSlim.EnterWriteLock();
            while (_messageData.Rows.Count > 0)
            {
                if (_messageData.Rows[0]["Object"] != current)
                    _messageData.Rows.RemoveAt(0);
                else
                {
                    break;
                }
            }
            lockSlim.ExitWriteLock();
            RefreshUIMessagesCount();

        }

        private void bBtnDataVisualizer_ItemClick(object sender, ItemClickEventArgs e)
        {
            var messages = Messages;
            DataVisualizerForm sv = new DataVisualizerForm(messages);
            sv.Show(this);
        }

        private void bbiScreenshot_ItemClick(object sender, ItemClickEventArgs e)
        {
            Bitmap image = takeComponentScreenShot(gridControl);
            Clipboard.SetImage(image);
            MessageBox.Show("Screenshot of messages was copied to clipboard.", "Image was taken", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }
        private static Bitmap takeComponentScreenShot(Control control)
        {
            // find absolute position of the control in the screen.
            Control ctrl = control;
            Rectangle rect = new Rectangle(Point.Empty, ctrl.Size);
            do
            {
                rect.Offset(ctrl.Location);
                ctrl = ctrl.Parent;
            }
            while (ctrl != null);

            Bitmap bmp = new Bitmap(rect.Width, rect.Height, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(bmp);

            g.CopyFromScreen(rect.Left, rect.Top, 0, 0, bmp.Size, CopyPixelOperation.SourceCopy);

            return bmp;
        }

        private void BbtnSaveViewAgnostic_ItemClick(object sender, ItemClickEventArgs e)
        {
            var messages = Messages;
            SaveMessagesToLog(AnalogyOfflineDataProvider, messages);
        }

        private void BarButtonItemSaveEntireInAnalogy_ItemClick(object sender, ItemClickEventArgs e)
        {
            var messages = PagingManager.GetAllMessages();
            SaveMessagesToLog(AnalogyOfflineDataProvider, messages);
        }

        private void bBtnUndockViewPerProcess_ItemClick(object sender, ItemClickEventArgs e)
        {
            UndockViewPerProcess();
        }

        private void UndockViewPerProcess()
        {
            var msg = Messages;
            if (!msg.Any()) return;
            var source = GetFilteredDataTable().Rows[0]?["DataProvider"]?.ToString();
            if (source == null) return;

            var processes = msg.Select(m => m.Module).Distinct().ToList();
            foreach (string process in processes)
            {
                XtraFormLogGrid grid = new XtraFormLogGrid(msg, source, process);
                lockExternalWindowsObject.EnterWriteLock();
                _externalWindows.Add(grid);
                Interlocked.Increment(ref ExternalWindowsCount);
                lockExternalWindowsObject.ExitWriteLock();
                grid.FormClosing += (s, arg) =>
                {
                    lockExternalWindowsObject.EnterWriteLock();
                    Interlocked.Decrement(ref ExternalWindowsCount);
                    _externalWindows.Remove(grid);
                    lockExternalWindowsObject.ExitWriteLock();
                };
                grid.Show(this);
            }
        }

        private void sbtnTextInclude_Click(object sender, EventArgs e)
        {
            txtbIncludeText.Text = "";
        }

        private void sbtnTextExclude_Click(object sender, EventArgs e)
        {
            txtbExclude.Text = "";
        }

        private void sbtnExcludeSources_Click(object sender, EventArgs e)
        {
            txtbExcludeSource.Text = "";
        }

        private void sbtnIncludeSources_Click(object sender, EventArgs e)
        {
            txtbIncludeSource.Text = "";
        }

        private void txtbIncludeSource_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtbIncludeSource.Text))
            {
                chkbIncludeSources.Checked = false;
            }
            else
            {
                if (!chkbIncludeSources.Checked)
                    chkbIncludeSources.Checked = true;
            }

            RefreshUserFilter();
            Settings.IncludedSource = chkbIncludeSources.Text;
        }

        private void sbtnExcludeModules_Click(object sender, EventArgs e)
        {
            txtbExcludeModule.Text = "";
        }

        private void sbtnIncludeModules_Click(object sender, EventArgs e)
        {
            txtbIncludeModule.Text = "";
        }

        private void txtbIncludeModule_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtbIncludeModule.Text))
            {
                chkbIncludeModules.Checked = false;
            }
            else
            {
                if (!chkbIncludeModules.Checked)
                    chkbIncludeModules.Checked = true;
            }

            RefreshUserFilter();
            Settings.IncludedModule= chkbIncludeModules.Text;
        }

        private void sbtnUndockPerProcess_Click(object sender, EventArgs e)
        {
            UndockViewPerProcess();
        }
    }
}


