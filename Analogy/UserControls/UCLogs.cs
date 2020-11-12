using Analogy.DataProviders;
using Analogy.Interfaces;
using Analogy.Interfaces.DataTypes;
using Analogy.Managers;
using Analogy.Tools;
using DevExpress.Data;
using DevExpress.Data.Filtering;
using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Analogy.DataTypes;
using Analogy.Forms;
using ChangeEventArgs = DevExpress.XtraEditors.Controls.ChangeEventArgs;

namespace Analogy
{

    public partial class UCLogs : XtraUserControl, ILogMessageCreatedHandler, ILogWindow
    {
        public bool ForceNoFileCaching { get; set; } = false;
        public bool DoNotAddToRecentHistory { get; set; } = false;
        private PagingManager PagingManager { get; set; }
        private FileProcessor FileProcessor { get; set; }
        public ManualResetEvent columnAdderSync = new ManualResetEvent(false);
        public List<(string field, string caption)> CurrentColumnsFields { get; set; }
        public CancellationTokenSource CancellationTokenSource { get; set; } = new CancellationTokenSource();
        public event EventHandler<AnalogyClearedHistoryEventArgs> OnHistoryCleared;
        public event EventHandler<(string, AnalogyLogMessage)> OnFocusedRowChanged;
        private Dictionary<string, List<AnalogyLogMessage>> groupingByChars;
        private string OldTextInclude = string.Empty;
        private string OldTextExclude = string.Empty;
        public int fileLoadingCount;
        public List<FilterCriteriaUIOption> IncludeFilterCriteriaUIOptions { get; set; }
        public List<FilterCriteriaUIOption> ExcludeFilterCriteriaUIOptions { get; set; }
        private bool FullModeEnabled { get; set; }
        private bool LoadingInProgress => fileLoadingCount > 0;
        private UserSettingsManager Settings => UserSettingsManager.UserSettings;
        private IExtensionsManager ExtensionManager { get; set; } = ExtensionsManager.Instance;
        private IEnumerable<IAnalogyExtension> InPlaceRegisteredExtensions { get; set; }
        private List<IAnalogyExtension> UserControlRegisteredExtensions { get; set; }
        private List<int> HighlightRows { get; set; } = new List<int>();
        private List<string> _excludeMostCommon = new List<string>();
        public const string DataGridDateColumnName = "Date";
        private bool _realtimeUpdate = true;
        private bool _simpleMode;
        private ReaderWriterLockSlim lockExternalWindowsObject =
            new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);

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

        public List<AnalogyLogMessage> Messages
        {
            get
            {
                var filterDatatable = GetFilteredDataTable();
                return filterDatatable.Rows.OfType<DataRow>().Select(r => (AnalogyLogMessage)r["Object"]).ToList();
            }
        }

        private List<AnalogyLogMessage> BookmarkedMessages
        {
            get
            {
                return _bookmarkedMessages.Rows.OfType<DataRow>().Select(r => (AnalogyLogMessage)r["Object"]).ToList();
            }
        }

        private AnalogyLogMessage _currentMassage;
        private FilterCriteriaObject _filterCriteria = new FilterCriteriaObject();
        private AutoCompleteStringCollection autoCompleteInclude = new AutoCompleteStringCollection();
        private AutoCompleteStringCollection autoCompleteExclude = new AutoCompleteStringCollection();

        private List<string> LoadedFiles { get; set; }
        private bool NewDataExist { get; set; }
        private DateTime reloadDateTime = DateTime.MaxValue;
        private bool hasAnyInPlaceExtensions;
        private bool hasAnyUserControlExtensions;
        private DateTime diffStartTime = DateTime.MinValue;
        private bool BookmarkView;
        private int pageNumber = 1;
        private CancellationTokenSource filterTokenSource;
        private CancellationToken filterToken;

        private int TotalPages => PagingManager.TotalPages;
        public IAnalogyDataProvider DataProvider { get; set; }
        public IAnalogyOfflineDataProvider? FileDataProvider { get; set; }
        private IAnalogyOfflineDataProvider AnalogyOfflineDataProvider { get; } = new AnalogyOfflineDataProvider();
        private Dictionary<string, int> counts;
        public GridView LogGrid
        {
            get => logGrid;
            set => logGrid = value;
        }

        private LogLevelSelectionType logLevelSelectionType = UserSettingsManager.UserSettings.LogLevelSelection;

        public UCLogs()
        {

            InitializeComponent();
            _simpleMode = Settings.SimpleMode;
            counts = new Dictionary<string, int>();
            foreach (string value in Utils.LogLevels)
            {
                counts.Add(value, 0);
            }

            filterTokenSource = new CancellationTokenSource();
            filterToken = filterTokenSource.Token;
            FileProcessor = new FileProcessor(this);
            if (DesignMode)
            {
                return;
            }

            PagingManager = new PagingManager(this);
            lockSlim = PagingManager.lockSlim;
            _messageData = PagingManager.CurrentPage();
            var excludedColumns = new List<GridColumn>()
            {
                gridColumnDate,
                gridColumnTimeDiff,
                gridColumnText,
                gridColumnSource,
                gridColumnLevel,
                gridColumnModule,
                gridColumnObject,
            };
            CurrentColumnsFields =
                logGrid.Columns.Except(excludedColumns).Select(c => (c.FieldName, c.Caption)).ToList();

            deNewerThanFilter.DateTime = DateTime.Now;
            deOlderThanFilter.DateTime = DateTime.Now;
            IncludeFilterCriteriaUIOptions = CurrentColumnsFields.Select(c => new FilterCriteriaUIOption(c.caption, c.field, false)).ToList();
            ExcludeFilterCriteriaUIOptions = CurrentColumnsFields.Select(c => new FilterCriteriaUIOption(c.caption, c.field, false)).ToList();
            clbInclude.DisplayMember = nameof(FilterCriteriaUIOption.DisplayMember);
            clbInclude.ValueMember = nameof(FilterCriteriaUIOption.ValueMember);
            clbInclude.CheckMember = nameof(FilterCriteriaUIOption.CheckMember);
            clbInclude.DataSource = IncludeFilterCriteriaUIOptions;
            clbExclude.DisplayMember = nameof(FilterCriteriaUIOption.DisplayMember);
            clbExclude.ValueMember = nameof(FilterCriteriaUIOption.ValueMember);
            clbExclude.CheckMember = nameof(FilterCriteriaUIOption.CheckMember);
            clbExclude.DataSource = ExcludeFilterCriteriaUIOptions;
            _filterCriteria.IncludeFilterCriteriaUIOptions = IncludeFilterCriteriaUIOptions;
            _filterCriteria.ExcludeFilterCriteriaUIOptions = ExcludeFilterCriteriaUIOptions;


        }

        private void UCLogs_Load(object sender, EventArgs e)
        {
            if (DesignMode)
            {
                return;
            }

            xtcFiltersLeft.SelectedTabPage = xtpFilters;

            LoadUISettings();
            LoadReplacementHeaders();
            BookmarkModeUI();
            LoadExtensions();
            SetupEventsHandlers();
            ProgressReporter = new Progress<AnalogyProgressReport>((value) =>
            {
                progressBar1.Maximum = value.Total;
                if (value.Processed < progressBar1.Maximum && value.Total > 1)
                {
                    progressBar1.Value = value.Processed;
                }

                if (value.Processed == value.Total)
                {
                    progressBar1.Visible = false;
                }
            });

     


            gridControl.DataSource = _messageData.DefaultView;
            _bookmarkedMessages = Utils.DataTableConstructor();
            gridControlBookmarkedMessages.DataSource = _bookmarkedMessages;

            if (Settings.SaveSearchFilters)
            {
                txtbInclude.Text = string.IsNullOrEmpty(Settings.IncludeText) || Settings.IncludeText == txtbInclude.Properties.NullText ? null : Settings.IncludeText;
                txtbExclude.Text = string.IsNullOrEmpty(Settings.ExcludeText) || Settings.ExcludeText == txtbExclude.Properties.NullText ? null : Settings.ExcludeText; ;
                txtbSource.Text = string.IsNullOrEmpty(Settings.SourceText) || Settings.SourceText == txtbSource.Properties.NullText ? null : Settings.SourceText;
                txtbModule.Text = string.IsNullOrEmpty(Settings.ModuleText) || Settings.ModuleText == txtbModule.Properties.NullText ? null : Settings.ModuleText;
            }

            gridControl.Focus();
        }
        private void rgSearchMode_SelectedIndexChanged(object s, EventArgs e)
        {
            Settings.BuiltInSearchPanelMode = rgSearchMode.SelectedIndex == 0 ? BuiltInSearchPanelMode.Search : BuiltInSearchPanelMode.Filter;
            logGrid.OptionsFind.Behavior = Settings.BuiltInSearchPanelMode == BuiltInSearchPanelMode.Search
                ? FindPanelBehavior.Search
                : FindPanelBehavior.Filter;
        }
        private void SetupEventsHandlers()
        {
            #region

            bbiIncludeColumnHeaderFilter.ItemClick += (s, e) =>
            {
                if (bbiIncludeColumnHeaderFilter.Tag is ViewColumnFilterInfo filter)
                {
                    logGrid.ActiveFilter.Add(filter);
                }
            };
            bbiIncludeMessage.ItemClick += tsmiInclude_Click;
            bbiIncludeSource.ItemClick += (s, e) =>
            {
                (AnalogyLogMessage message, _) = GetMessageFromSelectedFocusedRowInGrid();
                if (!string.IsNullOrEmpty(message?.Source))
                {
                    txtbSource.Text = txtbSource.Text == txtbSource.Properties.NullText ? message.Source : txtbSource.Text + "," + message.Source;
                }
            };
            bbiIncludeModule.ItemClick += (s, e) =>
            {
                (AnalogyLogMessage message, _) = GetMessageFromSelectedFocusedRowInGrid();
                if (!string.IsNullOrEmpty(message?.Module))
                {
                    txtbModule.Text = txtbModule.Text == txtbModule.Properties.NullText ? message.Module : txtbModule.Text + "," + message.Module;
                }
            };
            bbiJsonViewer.ItemClick += (s, e) =>
            {
                (AnalogyLogMessage message, _) = GetMessageFromSelectedFocusedRowInGrid();
                JsonViewerForm j = new JsonViewerForm(message);
                j.Show(this);
            };
            bbiDiffTime.ItemClick += tsmiTimeDiff_Click;
            bbiIncreaseFontSize.ItemClick += tsmiIncreaseFont_Click;
            bbiDecreaseFontSize.ItemClick += tsmiDecreaseFont_Click;
            bbiSaveLayout.ItemClick += tsmiSaveLayout_Click;
            bbiExcludeModule.ItemClick += tsmiExcludeModule_Click;
            bbiExcludeSource.ItemClick += tsmiExcludeSource_Click;
            bbiExcludeMessage.ItemClick += tsmiExclude_Click;
            bbiAddNoteToMessage.ItemClick += tsmiAddCommentToMessage_Click;
            bbiCopyAllMessages.ItemClick += tsmiCopyMessages_Click;
            bbiCopyMessage.ItemClick += tsmiCopy_Click;
            bbiBookmarkPersist.ItemClick += tsmiBookmarkPersist_Click;
            bbiBookmarkNonPersist.ItemClick += tsmiBookmark_Click;
            bbiDatetiemFilterTo.ItemClick += tsmiDateFilterOlder_Click;
            bbiDatetiemFilterFrom.ItemClick += tsmiDateFilterNewer_Click;
            #endregion

            LogGrid.RowCountChanged += (s, arg) =>
            {
                if (Settings.AutoScrollToLastMessage && !IsDisposed)
                {
                    BeginInvoke(new MethodInvoker(() =>
                    {
                        LogGrid.MoveLast();
                        LogGrid.MakeRowVisible(LogGrid.FocusedRowHandle);
                    }));

                }
            };

            gridControl.KeyUp += (s, e) =>
            {
                Keys excludeModifier = e.KeyCode & ~Keys.Control & ~Keys.Shift & ~Keys.Alt;
                switch (excludeModifier)
                {
                    case Keys.Oemplus:
                    case Keys.Add:
                        btswitchMessageDetails.Checked = true;
                        Settings.ShowMessageDetails = btswitchMessageDetails.Checked;
                        break;
                    case Keys.OemMinus:
                    case Keys.Subtract:
                        btswitchMessageDetails.Checked = false;
                        Settings.ShowMessageDetails = btswitchMessageDetails.Checked;
                        break;
                }
            };
            gridControl.MainView.Layout += MainView_Layout;
            logGrid.RowStyle += pmsGridView_RowStyle;
            logGrid.MouseDown += LogGrid_MouseDown;
            logGrid.MouseUp += LogGrid_MouseUp;
            LogGridPopupMenu.BeforePopup += (_, __) => UpdatePopupTexts();
            logGrid.CustomSummaryCalculate += LogGrid_CustomSummaryCalculate;
            gridViewBookmarkedMessages.RowStyle += pmsGridView_RowStyle;
            gridViewGrouping2.RowStyle += pmsGridView_RowStyle;

            rgSearchMode.SelectedIndexChanged += rgSearchMode_SelectedIndexChanged;
            sbtnToggleSearchFilter.Click += (_, __) =>
            {
                Settings.IsBuiltInSearchPanelVisible = !Settings.IsBuiltInSearchPanelVisible;
                logGrid.OptionsFind.AlwaysVisible = Settings.IsBuiltInSearchPanelVisible;
            };
            clbInclude.ItemCheck += async (_, __) => await FilterHasChanged();
            clbExclude.ItemCheck += async (_, __) => await FilterHasChanged();
            deNewerThanFilter.EditValueChanged += async (s, e) =>
            {
                ceNewerThanFilter.Checked = true;
                await FilterHasChanged();
            };
            deNewerThanFilter.Properties.EditValueChanged += async (s, e) =>
            {
                ceNewerThanFilter.Checked = true;
                await FilterHasChanged();
            };

            deOlderThanFilter.EditValueChanged += async (s, e) =>
            {
                ceOlderThanFilter.Checked = true;
                await FilterHasChanged();
            };

            deOlderThanFilter.Properties.EditValueChanged += async (s, e) =>
            {
                ceOlderThanFilter.Checked = true;
                await FilterHasChanged();
            };

            ceOlderThanFilter.CheckedChanged += async (s, e) => await FilterHasChanged();
            ceNewerThanFilter.CheckedChanged += async (s, e) => await FilterHasChanged();
            ceModulesProcess.Click += async (s, e) => await FilterHasChanged();
            ceSources.Click += async (s, e) => await FilterHasChanged();
            ceIncludeText.CheckedChanged += async (s, e) =>
            {
                if (!ceIncludeText.Checked && !ceExcludeText.Checked)
                {
                    // LogGrid.ClearColumnsFilter();
                    gridColumnText.FilterInfo = null;
                }

                await FilterHasChanged();
            };
            ceExcludeText.CheckedChanged += async (s, e) =>
            {
                if (!ceIncludeText.Checked && !ceExcludeText.Checked)
                {
                    //LogGrid.ClearColumnsFilter();
                    gridColumnText.FilterInfo = null;
                }

                await FilterHasChanged();
            };
            logGrid.EndSorting += (s, e) =>
            {
                var sortOrder = gridColumnDate.SortOrder;
                Settings.DefaultDescendOrder = sortOrder == ColumnSortOrder.Descending;
            };
            bBtnFullGrid.ItemClick += (s, e) =>
            {
                FullModeEnabled = !FullModeEnabled;
                pnlFilters.Visible = !FullModeEnabled;
            };
            bBtnShare.ItemClick += (s, e) =>
            {
                AnalogyOTAForm share = new AnalogyOTAForm(GetFilteredDataTable());
                share.Show(this);
            };
            PagingManager.OnPageChanged += (s, arg) =>
            {
                if (IsDisposed)
                {
                    return;
                }

                BeginInvoke(new MethodInvoker(() =>
                    lblPageNumber.Text = $"Page {pageNumber} / {arg.AnalogyPage.PageNumber}"));

            };

            txtbInclude.Enter += (s, e) => txtbInclude.SelectAll();
            txtbInclude.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    var added = Settings.AddNewSearchesEntryToLists(txtbInclude.Text, true);
                    if (added)
                    {
                        autoCompleteInclude.Add(txtbInclude.Text);
                    }

                }
            };

            txtbInclude.EditValueChanged += EditValueChanged;
            txtbExclude.EditValueChanged += EditValueChanged;
            txtbSource.EditValueChanged += EditValueChanged;
            txtbModule.EditValueChanged += EditValueChanged;
            txtbInclude.TextChanged += async (s, e) =>
            {
                if (OldTextInclude.Equals(txtbInclude.Text) ||
                    txtbInclude.Text.Equals(txtbInclude.Properties.NullText))
                {
                    return;
                }

                OldTextInclude = txtbInclude.Text;
                // txtbHighlight.Text = txtbInclude.Text;
                if (string.IsNullOrEmpty(txtbInclude.Text))
                {
                    ceIncludeText.Checked = false;
                    txtbInclude.EditValue = null;
                    return;
                }

                chkbHighlight.Checked = false;
                ceIncludeText.Checked = true;
                await FilterHasChanged();
            };

            txtbExclude.Enter += (s, e) => txtbExclude.SelectAll();
            txtbExclude.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    var added = Settings.AddNewSearchesEntryToLists(txtbExclude.Text, false);
                    if (added)
                    {
                        autoCompleteExclude.Add(txtbExclude.Text);
                    }
                }
            };
            txtbExclude.TextChanged += async (s, e) =>
            {
                if (OldTextExclude.Equals(txtbExclude.Text)||
                    txtbExclude.Text.Equals(txtbExclude.Properties.NullText))
                {
                    return;
                }

                Settings.ExcludeText = txtbExclude.Text;
                OldTextExclude = txtbExclude.Text;
                if (string.IsNullOrEmpty(txtbExclude.Text))
                {
                    ceExcludeText.Checked = false;
                    txtbExclude.EditValue = null;
                    return;
                }

                ceExcludeText.Checked = true;
                await FilterHasChanged();
            };

            txtbSource.TextChanged += async (s, e) =>
            {
                if (string.IsNullOrEmpty(txtbSource.Text) ||
                    txtbSource.Text.Equals(txtbSource.Properties.NullText))
                {
                    ceSources.Checked = false;
                    txtbSource.EditValue = null;
                }
                else
                {
                    if (!ceSources.Checked)
                    {
                        ceSources.Checked = true;
                    }
                }

                await FilterHasChanged();
                Settings.SourceText = txtbSource.Text;
            };

            txtbModule.TextChanged += async (s, e) =>
            {
                if (string.IsNullOrEmpty(txtbModule.Text) ||
                    txtbModule.Text.Equals(txtbModule.Properties.NullText))
                {
                    ceModulesProcess.Checked = false;
                    txtbModule.EditValue = null;
                }
                else
                {
                    if (!ceModulesProcess.Checked)
                    {
                        ceModulesProcess.Checked = true;
                    }
                }

                await FilterHasChanged();
                Settings.ModuleText = txtbModule.Text;
            };

            bbtnReload.ItemClick += async (s, e) =>
            {
                reloadDateTime = FileProcessor.lastNewestMessage;
                await LoadFilesAsync(LoadedFiles, true, true);
            };

            bbiSaveBookmarks.ItemClick += (_, __) => SaveMessagesToLog(FileDataProvider, BookmarkedMessages);


        }

        private void EditValueChanged(object sender, EventArgs e)
        {

            if (sender is BaseEdit edit && e is ChangingEventArgs change && change.NewValue == string.Empty)
            {
                edit.EditValue = null;
            }
        }


        private void MainView_Layout(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Settings.LogGridFileName))
                {
                    gridControl.MainView.SaveLayoutToXml(Settings.LogGridFileName);
                }
            }
            catch (Exception ex)
            {
                AnalogyLogManager.Instance.LogError(ex.Message, nameof(MainView_Layout));
            }
        }

        private void LogGrid_CustomSummaryCalculate(object sender, CustomSummaryEventArgs e)
        {
            if (e.SummaryProcess == CustomSummaryProcess.Start)
            {
                foreach (var key in Utils.LogLevels)
                {
                    counts[key] = 0;
                }
            }
            else if (e.SummaryProcess == CustomSummaryProcess.Calculate)
            {
                counts[(string)e.FieldValue] = counts[(string)e.FieldValue] + 1;
            }

            else if (e.SummaryProcess == CustomSummaryProcess.Finalize)
            {
                lblTotalMessages.Text =
                    $"Total messages:{counts.Values.Sum()}. Errors:{counts["Error"]}. Warnings:{counts["Warning"]}. Critical:{counts["Critical"]}.";
            }
            //todo: add alerts
            //if (Settings.PreDefinedQueries.Alerts.Any())
            //{
            //    var messages = rows.Select(r => (AnalogyLogMessage)r["Object"]).ToList();
            //    alertCount = messages.Count(m =>
            //        Settings.PreDefinedQueries.Alerts.Any(a => FilterCriteriaObject.MatchAlert(m, a)));

            //}
        }

        private void LogGrid_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                GridView view = sender as GridView;
                GridHitInfo hitInfo = view.CalcHitInfo(e.Location);
                if (hitInfo.InRow && hitInfo.RowHandle >= 0 && !(hitInfo.Column == view.FocusedColumn && hitInfo.RowHandle == view.FocusedRowHandle))
                {
                    UpdatePopupTexts();
                    var value = view.GetRowCellValue(hitInfo.RowHandle, hitInfo.Column.FieldName);
                    if (value != null)
                    {
                        ViewColumnFilterInfo viewFilterInfo = new ViewColumnFilterInfo(view.Columns[hitInfo.Column.FieldName],
                            new ColumnFilterInfo($"[{hitInfo.Column.FieldName}] = '{value}'", ""));
                        string val = new string(value.ToString().Take(100).ToArray());
                        bbiIncludeColumnHeaderFilter.Caption = $"Set '{val}' as column header filter for column '{hitInfo.Column.Caption}'";
                        bbiIncludeColumnHeaderFilter.Tag = viewFilterInfo;
                        bbiIncludeColumnHeaderFilter.Visibility = BarItemVisibility.Always;
                    }
                    else
                    {
                        bbiIncludeColumnHeaderFilter.Tag = null;
                        bbiIncludeColumnHeaderFilter.Visibility = BarItemVisibility.Never;
                    }
                }
            }
        }

        private void UpdatePopupTexts()
        {
            (AnalogyLogMessage message, _) = GetMessageFromSelectedFocusedRowInGrid();
            if (message != null)
            {
                var module = new string(message.Module.Take(100).ToArray());
                var source = new string(message.Source.Take(100).ToArray());
                bbiIncludeModule.Caption = $"Include Process/Module: Append '{module}' to filter";
                bbiIncludeSource.Caption = $"Include Source: Append '{source}' to filter";
                bbiExcludeModule.Caption = $"Exclude Process/Module: Append '{module}' to filter";
                bbiExcludeSource.Caption = $"Exclude Source: Append '{source}' to filter";
                bbiDatetiemFilterFrom.Caption = $"Show all messages after {message.Date}";
                bbiDatetiemFilterTo.Caption = $"Show all messages Before {message.Date}";
                bbiDatetiemFilterFrom.Visibility = BarItemVisibility.Always;
                bbiDatetiemFilterTo.Visibility = BarItemVisibility.Always;
            }
            else
            {
                bbiDatetiemFilterFrom.Visibility = BarItemVisibility.Never;
                bbiDatetiemFilterTo.Visibility = BarItemVisibility.Never;
            }
        }

        private void LogGrid_MouseUp(object sender, MouseEventArgs e)
        {
            GridHitInfo hitInfo = logGrid.CalcHitInfo(e.Location);
            if (e.Button == MouseButtons.Right && !hitInfo.InColumnPanel)
            {
                LogGridPopupMenu.ShowPopup(Cursor.Position);
            }
        }
        private void LoadReplacementHeaders()
        {
            if (DataProvider == null)
            {
                return;
            }

            try
            {
                if (DataProvider.GetReplacementHeaders() == null || !DataProvider.GetReplacementHeaders().Any())
                {
                    return;
                }

                foreach ((string originalHeader, string replacementHeader) in DataProvider.GetReplacementHeaders())
                {
                    logGrid.Columns[originalHeader].Caption = replacementHeader;
                }

            }
            catch (Exception)
            {
                //ignore replacement
            }
        }

        public void SetFileDataSource(IAnalogyDataProvider dataProvider, IAnalogyOfflineDataProvider? fileDataProvider)
        {
            DataProvider = dataProvider;
            FileDataProvider = fileDataProvider;
            SetSaveButtonsVisibility(FileDataProvider != null);
        }


        public void SetSaveButtonsVisibility(bool on)
        {
            if (on)
            {
                //disable specific saving
                bBtnSaveLog.Visibility = BarItemVisibility.Always;
                bBtnSaveEntireLog.Visibility = BarItemVisibility.Always;
                bBtnSaveCurrentSelectionCustomFormat.Visibility = BarItemVisibility.Always;
            }
            else
            {
                bBtnSaveLog.Visibility = BarItemVisibility.Never;
                bBtnSaveEntireLog.Visibility = BarItemVisibility.Never;
                bBtnSaveCurrentSelectionCustomFormat.Visibility = BarItemVisibility.Never;
            }
        }

        public void ProcessCmdKeyFromParent(Keys keyData)
        {
            KeyEventArgs e = new KeyEventArgs(keyData);
            if (e.Control && e.KeyCode == Keys.D)
            {
                btswitchMessageDetails.Checked = !btswitchMessageDetails.Checked;
                Settings.ShowMessageDetails = btswitchMessageDetails.Checked;
                return;
            }
            if (e.Control && e.KeyCode == Keys.R)
            {
                btswitchRefreshLog.Checked = !btswitchRefreshLog.Checked;
                return;
            }
            if (e.Control && e.KeyCode == Keys.F)
            {
                rgSearchMode.SelectedIndexChanged -= rgSearchMode_SelectedIndexChanged;
                ToggleSearch();
                rgSearchMode.SelectedIndex = 0;
                rgSearchMode.SelectedIndexChanged += rgSearchMode_SelectedIndexChanged;
            }
            if (e.Alt && e.KeyCode == Keys.F)
            {
                rgSearchMode.SelectedIndexChanged -= rgSearchMode_SelectedIndexChanged;
                ToggleFilter();
                rgSearchMode.SelectedIndex = 1;
                rgSearchMode.SelectedIndexChanged += rgSearchMode_SelectedIndexChanged;
            }


            if (e.Alt && e.KeyCode == Keys.E || e.Alt && e.KeyCode == Keys.W)
            {
                if (e.KeyCode == Keys.W)
                {
                    chkLstLogLevel.Items[AnalogyLogLevel.Warning.ToString()].CheckState = chkLstLogLevel.Items[AnalogyLogLevel.Warning.ToString()].CheckState == CheckState.Checked
                        ? CheckState.Unchecked
                        : CheckState.Checked;
                }
                else
                {
                    switch (logLevelSelectionType)
                    {
                        case LogLevelSelectionType.Single:
                            chkLstLogLevel.Items["Error + Critical"].CheckState = chkLstLogLevel.Items["Error + Critical"].CheckState == CheckState.Checked
                                ? CheckState.Unchecked
                                : CheckState.Checked;
                            break;
                        case LogLevelSelectionType.Multiple:
                            chkLstLogLevel.Items["Error"].CheckState = chkLstLogLevel.Items["Error"].CheckState == CheckState.Checked
                                ? CheckState.Unchecked
                                : CheckState.Checked;
                            chkLstLogLevel.Items["Critical"].CheckState = chkLstLogLevel.Items["Critical"].CheckState == CheckState.Checked
                                ? CheckState.Unchecked
                                : CheckState.Checked;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }


        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            KeyEventArgs e = new KeyEventArgs(keyData);
            if (e.Control && e.KeyCode == Keys.D)
            {
                btswitchMessageDetails.Checked = !btswitchMessageDetails.Checked;
                Settings.ShowMessageDetails = btswitchMessageDetails.Checked;
                return true;
            }
            if (e.Control && e.KeyCode == Keys.R)
            {
                btswitchRefreshLog.Checked = !btswitchRefreshLog.Checked;
                return true;
            }

            if (e.Control && e.KeyCode == Keys.F)
            {
                rgSearchMode.SelectedIndexChanged -= rgSearchMode_SelectedIndexChanged;
                ToggleSearch();
                rgSearchMode.SelectedIndex = 0;
                rgSearchMode.SelectedIndexChanged += rgSearchMode_SelectedIndexChanged;
                return true;
            }
            if (e.Alt && e.KeyCode == Keys.F)
            {
                rgSearchMode.SelectedIndexChanged -= rgSearchMode_SelectedIndexChanged;
                ToggleFilter();
                rgSearchMode.SelectedIndex = 1;
                rgSearchMode.SelectedIndexChanged += rgSearchMode_SelectedIndexChanged;
                return true;
            }

            if (e.Alt && e.KeyCode == Keys.E || e.Alt && e.KeyCode == Keys.W)
            {
                if (e.KeyCode == Keys.W)
                {
                    chkLstLogLevel.Items[AnalogyLogLevel.Warning.ToString()].CheckState = chkLstLogLevel.Items[AnalogyLogLevel.Warning.ToString()].CheckState == CheckState.Checked
                        ? CheckState.Unchecked
                        : CheckState.Checked;
                }
                else
                {
                    switch (logLevelSelectionType)
                    {
                        case LogLevelSelectionType.Single:
                            chkLstLogLevel.Items["Error + Critical"].CheckState = chkLstLogLevel.Items["Error + Critical"].CheckState == CheckState.Checked
                                ? CheckState.Unchecked
                                : CheckState.Checked;
                            break;
                        case LogLevelSelectionType.Multiple:
                            chkLstLogLevel.Items["Error"].CheckState = chkLstLogLevel.Items["Error"].CheckState == CheckState.Checked
                                ? CheckState.Unchecked
                                : CheckState.Checked;
                            chkLstLogLevel.Items["Critical"].CheckState = chkLstLogLevel.Items["Critical"].CheckState == CheckState.Checked
                                ? CheckState.Unchecked
                                : CheckState.Checked;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);

        }

        private void ToggleSearch()
        {
            if (Settings.BuiltInSearchPanelMode == BuiltInSearchPanelMode.Search)
            {
                Settings.IsBuiltInSearchPanelVisible = !Settings.IsBuiltInSearchPanelVisible;
            }
            else
            {
                Settings.BuiltInSearchPanelMode = BuiltInSearchPanelMode.Search;
                Settings.IsBuiltInSearchPanelVisible = true;
                logGrid.OptionsFind.Behavior = FindPanelBehavior.Search;
            }
            logGrid.OptionsFind.AlwaysVisible = Settings.IsBuiltInSearchPanelVisible;
        }
        private void ToggleFilter()
        {
            if (Settings.BuiltInSearchPanelMode == BuiltInSearchPanelMode.Filter)
            {
                Settings.IsBuiltInSearchPanelVisible = !Settings.IsBuiltInSearchPanelVisible;
            }
            else
            {
                Settings.BuiltInSearchPanelMode = BuiltInSearchPanelMode.Filter;
                Settings.IsBuiltInSearchPanelVisible = true;
                logGrid.OptionsFind.Behavior = FindPanelBehavior.Filter;
            }
            logGrid.OptionsFind.AlwaysVisible = Settings.IsBuiltInSearchPanelVisible;
        }
        private void LoadUISettings()
        {
            Utils.SetLogLevel(chkLstLogLevel);
            xtcFilters.Visible = !_simpleMode;
            bBtnShare.Visibility =
                FactoriesManager.Instance.Factories.SelectMany(f => f.ShareableFactories)
                    .SelectMany(fc => fc.Shareables).Any()
                    ? BarItemVisibility.Always
                    : BarItemVisibility.Never;

            logGrid.Columns["Level"].SummaryItem.SummaryType = SummaryItemType.Custom;
            logGrid.Columns["Level"].SummaryItem.FieldName = "Level";
            logGrid.OptionsSelection.MultiSelect = true;
            logGrid.OptionsSelection.MultiSelectMode = GridMultiSelectMode.RowSelect;
            //logGrid.OptionsView.ShowFooter = true;
            //Set up a summary on the "yourfieldname" column
            //logGrid.Columns[0].SummaryItem.SummaryType = SummaryItemType.Custom;
            ////logGrid.Columns[0].SummaryItem.FieldName = "id";

            //logGrid.Columns[0].SummaryItem.DisplayFormat = "Rows: {0:n0}";
            logGrid.OptionsFind.AlwaysVisible = Settings.IsBuiltInSearchPanelVisible;
            rgSearchMode.SelectedIndex = Settings.BuiltInSearchPanelMode == BuiltInSearchPanelMode.Search ? 0 : 1;
            logGrid.OptionsFind.Behavior = Settings.BuiltInSearchPanelMode == BuiltInSearchPanelMode.Search
                ? FindPanelBehavior.Search
                : FindPanelBehavior.Filter;
            gridColumnDate.SortOrder =
                Settings.DefaultDescendOrder ? ColumnSortOrder.Descending : ColumnSortOrder.Ascending;
            txtbInclude.MaskBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txtbInclude.MaskBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
            if (Settings.RememberLastSearches)
            {
                autoCompleteInclude.AddRange(Settings.LastSearchesInclude.ToArray());
                autoCompleteExclude.AddRange(Settings.LastSearchesExclude.ToArray());
            }

            txtbInclude.MaskBox.AutoCompleteCustomSource = autoCompleteInclude;

            txtbExclude.MaskBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txtbExclude.MaskBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtbExclude.MaskBox.AutoCompleteCustomSource = autoCompleteExclude;

            gridControl.ForceInitialize();
            if (File.Exists(Settings.LogGridFileName))
            {
                gridControl.MainView.RestoreLayoutFromXml(Settings.LogGridFileName);
                gridControlBookmarkedMessages.MainView.RestoreLayoutFromXml(Settings.LogGridFileName);
            }
            btswitchRefreshLog.Checked = true;
            LogGrid.BestFitColumns();
            btswitchMessageDetails.Checked = Settings.ShowMessageDetails;
            splitContainerMain.Collapsed = !Settings.ShowMessageDetails;
            if (Settings.StartupErrorLogLevel)
            {
                chkLstLogLevel.Items[1].CheckState = CheckState.Checked;
            }

            LogGrid.Appearance.Row.Font = new Font(LogGrid.Appearance.Row.Font.Name, Settings.FontSettings.GridFontSize);
            btsAutoScrollToBottom.Checked = Settings.AutoScrollToLastMessage;

            logGrid.Columns["Date"].DisplayFormat.FormatType = FormatType.DateTime;
            logGrid.Columns["Date"].DisplayFormat.FormatString = Settings.DateTimePattern;

            gridViewBookmarkedMessages.Columns["Date"].DisplayFormat.FormatType = FormatType.DateTime;
            gridViewBookmarkedMessages.Columns["Date"].DisplayFormat.FormatString = Settings.DateTimePattern;

            gridViewGrouping2.Columns["Date"].DisplayFormat.FormatType = FormatType.DateTime;
            gridViewGrouping2.Columns["Date"].DisplayFormat.FormatString = Settings.DateTimePattern;


        }

        private void BookmarkModeUI()
        {
            if (BookmarkView)
            {
                bBtnRemoveBoomark.Visibility = BarItemVisibility.Always;
                bBtnImport.Visibility = BarItemVisibility.Never;
            }
        }

        public void LoadExtensions()
        {
            var extensions = ExtensionManager.RegisteredExtensions.Where(e => e.TargetProviderId == DataProvider.Id)
                .ToList();
            hasAnyInPlaceExtensions = extensions.Any(e => e.ExtensionType == AnalogyExtensionType.InPlace);
            hasAnyUserControlExtensions = extensions.Any(e => e.ExtensionType == AnalogyExtensionType.UserControl);
            InPlaceRegisteredExtensions =
                extensions.Where(e => e.ExtensionType == AnalogyExtensionType.InPlace).ToList();
            UserControlRegisteredExtensions =
                extensions.Where(e => e.ExtensionType == AnalogyExtensionType.UserControl).ToList();
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
                    LogGrid.Columns.Add(gridColumn);
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
            {
                return;
            }

            GridHitInfo hi = LogGrid.CalcHitInfo(new Point(args.X, args.Y));

            if (hi.RowHandle < 0)
            {
                return;
            }

            int[] selRows = LogGrid.GetSelectedRows();

            if (selRows == null || selRows.Length != 1)
            {
                return;
            }

            int rownum = selRows.First();
            _currentMassage = (AnalogyLogMessage)LogGrid.GetRowCellValue(rownum, "Object");
            LoadTextBoxes(_currentMassage);
            if (hasAnyInPlaceExtensions)
            {
                var rowHandle = hi.RowHandle;
                var column = hi.Column;
                if (column == null)
                {
                    return;
                }

                foreach (IAnalogyExtension extension in InPlaceRegisteredExtensions)
                {
                    var columns = extension.GetColumnsInfo();
                    foreach (AnalogyColumnInfo exColumn in columns)
                    {
                        if (column.FieldName.Equals(exColumn.ColumnName) &&
                            column.Caption.Equals(exColumn.ColumnCaption))
                        {
                            var cellValue = LogGrid.GetRowCellValue(rowHandle, exColumn.ColumnName);
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


            (AnalogyLogMessage message, _) = GetMessageFromSelectedFocusedRowInGrid();
            if (message == null)
            {
                return;
            }

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
            if (!Settings.ColorSettings.EnableMessagesColors || !(sender is GridView view) || e.RowHandle < 0)
            {
                return;
            }

            IAnalogyLogMessage message = (AnalogyLogMessage)view.GetRowCellValue(e.RowHandle, view.Columns["Object"]);
            if (message == null)
            {
                return;
            }

            if (!Settings.ColorSettings.OverrideLogLevelColor && Settings.ColorSettings.EnableNewMessagesColor &&
                message.Date > reloadDateTime)
            {
                e.Appearance.BackColor = Settings.ColorSettings.NewMessagesColor.BackgroundColor;
                e.Appearance.ForeColor = Settings.ColorSettings.NewMessagesColor.TextColor;
            }

            var (backgroundColorLevel, textColorLevel) = Settings.ColorSettings.GetColorForLogLevel(message.Level);
            e.Appearance.BackColor = backgroundColorLevel;
            e.Appearance.ForeColor = textColorLevel;

            if (Settings.ColorSettings.OverrideLogLevelColor && Settings.ColorSettings.EnableNewMessagesColor &&
                message.Date > reloadDateTime)
            {
                var (backgroundColor, textColor) = Settings.ColorSettings.NewMessagesColor;
                e.Appearance.BackColor = backgroundColor;
                e.Appearance.ForeColor = textColor;
            }
            string text = view.GetRowCellDisplayText(e.RowHandle, view.Columns["Text"]);
            foreach (PreDefineHighlight preDefineHighlight in Settings.PreDefinedQueries.Highlights)
            {
                if (FilterCriteriaObject.Match(text, preDefineHighlight.Text,
                    preDefineHighlight.PreDefinedQueryType))
                {
                    e.Appearance.BackColor = preDefineHighlight.Color;
                }
            }

            if (DataProvider.UseCustomColors)
            {
                IAnalogyLogMessage m =
                    (AnalogyLogMessage)view.GetRowCellValue(e.RowHandle, view.Columns["Object"]);
                if (m == null)
                {
                    return;
                }

                var colors = DataProvider.GetColorForMessage(m);
                if (colors.backgroundColor != Color.Empty)
                {
                    e.Appearance.BackColor = colors.backgroundColor;
                }

                if (colors.foregroundColor != Color.Empty)
                {
                    e.Appearance.ForeColor = colors.foregroundColor;
                }
            }

            if (chkbHighlight.Checked &&
                FilterCriteriaObject.Match(text, txtbHighlight.Text, PreDefinedQueryType.Contains))
            {
                var (backgroundColorHighlight, textColorHighlight) = Settings.ColorSettings.GetHighlightColor();
                e.Appearance.BackColor = backgroundColorHighlight;
                e.Appearance.ForeColor = textColorHighlight;
            }
        }

        private void pmsGridView_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (!(e.RowHandle >= 0) || !e.Info.IsRowIndicator || !(sender is GridView view))
            {
                return;
            }

            AnalogyLogMessage msg = (AnalogyLogMessage)view.GetRowCellValue(e.RowHandle, "Object");
            if (msg == null)
            {
                return;
            }

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
                case AnalogyLogLevel.Information:
                    img = imageList.Images[7];
                    break;
                case AnalogyLogLevel.Verbose:
                    img = imageList.Images[2];
                    break;
                case AnalogyLogLevel.Debug:
                    img = imageList.Images[6];
                    break;
                case AnalogyLogLevel.None:
                    break;
                case AnalogyLogLevel.Analogy:
                    img = imageList.Images[8];
                    break;
                case AnalogyLogLevel.Unknown:
                    img = imageList.Images[9];
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
            (AnalogyLogMessage m, _) = GetMessageFromSelectedFocusedRowInGrid();
            if (m != null)
            {
                Clipboard.SetText(m.Text);
            }
        }

        private void tsmiEmail_Click(object sender, EventArgs e)
        {

            (AnalogyLogMessage message, _) = GetMessageFromSelectedFocusedRowInGrid();
            if (message == null)
            {
                return;
            }

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

        private async Task FilterHasChanged()
        {
            async Task RefreshData(CancellationToken token)
            {
                await Task.Delay(500);
                if (token.IsCancellationRequested)
                {
                    return;
                }

                FilterResults();

            }

            filterTokenSource.Cancel();
            filterTokenSource = new CancellationTokenSource();
            filterToken = filterTokenSource.Token;
            await RefreshData(filterToken);
        }

        private async void tsmiExclude_Click(object sender, EventArgs e)
        {
            (AnalogyLogMessage message, _) = GetMessageFromSelectedFocusedRowInGrid();
            if (message == null)
            {
                return;
            }

            var ef = new AnalogyExcludeMessage(message);
            ef.Text = "Exclude";
            if (ef.ShowDialog(this) == DialogResult.OK)
            {
                string exclude = ef.MessageText;

                txtbExclude.Text = txtbExclude.Text == txtbExclude.Properties.NullText ? exclude : txtbExclude.Text + "|" + exclude;
                ceExcludeText.Checked = true;
                await FilterHasChanged();
            }
        }

        private async void tsmiInclude_Click(object sender, EventArgs e)
        {
            (AnalogyLogMessage message, _) = GetMessageFromSelectedFocusedRowInGrid();
            if (message == null)
            {
                return;
            }

            var ef = new AnalogyExcludeMessage(message);
            ef.Text = "Include";
            if (ef.ShowDialog(this) == DialogResult.OK)
            {
                string include = ef.MessageText;

                txtbInclude.Text = txtbInclude.Text == txtbInclude.Properties.NullText ? include : txtbInclude.Text + "|" + include;
                ceIncludeText.Checked = true;
                await FilterHasChanged();
            }
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
                string filter = _messageData.DefaultView.RowFilter;
                if (LogGrid.ActiveFilterEnabled && !string.IsNullOrEmpty(LogGrid.ActiveFilterString))
                {
                    CriteriaOperator op = LogGrid.ActiveFilterCriteria; //filterControl1.FilterCriteria  
                    string filterString = CriteriaToWhereClauseHelper.GetDataSetWhere(op);
                    filter = string.IsNullOrEmpty(filter) ? filterString : $"{filter} and {filterString}";
                }

                return new DataView(_messageData, filter, null, DataViewRowState.CurrentRows).ToTable();
            }
            finally
            {
                lockSlim.ExitReadLock();
            }
        }

        public List<AnalogyLogMessage> GetMessages() => PagingManager.GetAllMessages();

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
            if (message.Level == AnalogyLogLevel.None)
            {
                return; //ignore those messages
            }

            if (Settings.IdleMode && Utils.IdleTime().TotalMinutes > Settings.IdleTimeMinutes)
            {
                PagingManager.IncrementTotalMissedMessages();
            }

            if (ExternalWindowsCount > 0)
            {
                foreach (XtraFormLogGrid grid in ExternalWindows)
                {
                    grid.AppendMessage(message, dataSource);
                }
            }


            DataRow dtr = PagingManager.AppendMessage(message, dataSource);
            lockSlim.EnterWriteLock();
            if (diffStartTime > DateTime.MinValue)
            {
                dtr["TimeDiff"] = message.Date.Subtract(diffStartTime).ToString();
            }

            lockSlim.ExitWriteLock();
            if (message.AdditionalInformation != null && message.AdditionalInformation.Any() &&
                Settings.CheckAdditionalInformation)
            {
                AddExtraColumnsToLogGrid(logGrid, message);
            }

            lockSlim.EnterWriteLock();
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
            {
                NewDataExist = true;
            }
        }

        private void AddExtraColumnsToLogGrid(GridView gridView, AnalogyLogMessage message)
        {
            if (message.AdditionalInformation != null && message.AdditionalInformation.Any() &&
                Settings.CheckAdditionalInformation)
            {
                foreach (KeyValuePair<string, string> info in message.AdditionalInformation)
                {
                    if (!CurrentColumnsFields.Exists(c => c.field == info.Key))
                    {
                        if (InvokeRequired)
                        {
                            BeginInvoke(new MethodInvoker(() =>
                            {
                                if (!gridView.Columns.Select(g => g.FieldName).Contains(info.Key))
                                {
                                    gridView.Columns.Add(new GridColumn()
                                    { Caption = info.Key, FieldName = info.Key, Name = info.Key, Visible = true });
                                    CurrentColumnsFields.Add((info.Key, info.Key));
                                    IncludeFilterCriteriaUIOptions.Add(new FilterCriteriaUIOption(info.Key, info.Key, false));
                                    ExcludeFilterCriteriaUIOptions.Add(new FilterCriteriaUIOption(info.Key, info.Key, false));
                                }
                                columnAdderSync.Set();
                            }));
                            columnAdderSync.WaitOne();
                            columnAdderSync.Reset();
                        }
                        else
                        {
                            if (!gridView.Columns.Select(g => g.FieldName).Contains(info.Key))
                            {
                                gridView.Columns.Add(new GridColumn() { Caption = info.Key, FieldName = info.Key, Name = info.Key, Visible = true });
                                CurrentColumnsFields.Add((info.Key, info.Key));
                                IncludeFilterCriteriaUIOptions.Add(new FilterCriteriaUIOption(info.Key, info.Key, false));
                                ExcludeFilterCriteriaUIOptions.Add(new FilterCriteriaUIOption(info.Key, info.Key, false));
                            }
                        }

                    }
                }
            }
        }

        public void AppendMessages(List<AnalogyLogMessage> messages, string dataSource)
        {

            if (Settings.IdleMode && Utils.IdleTime().TotalMinutes > Settings.IdleTimeMinutes)
            {
                PagingManager.IncrementTotalMissedMessages();
            }

            //lockSlim.EnterWriteLock();
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

                if (message.AdditionalInformation != null && message.AdditionalInformation.Any() &&
                    Settings.CheckAdditionalInformation)
                {
                    AddExtraColumnsToLogGrid(logGrid, message);
                }

                dtr.EndEdit();
            }

            //lockSlim.ExitWriteLock();
            if (PagingManager.IsCurrentPageInView(_messageData))
            {
                NewDataExist = true;
            }

            if (hasAnyUserControlExtensions)
            {
                foreach (IAnalogyExtension extension in UserControlRegisteredExtensions)
                {
                    extension.NewMessages(messages);
                }
            }

        }

        private void AcceptChanges(bool forceRefresh)
        {
            if (!IsHandleCreated)
            {
                return;
            }

            if (_realtimeUpdate || forceRefresh)

            {
                BeginInvoke(new MethodInvoker(() =>
                {
                    lockSlim.EnterWriteLock();
                    try
                    {
                        // LogGrid.BeginDataUpdate();
                        _messageData.AcceptChanges();
                        // LogGrid.EndDataUpdate();
                    }
                    finally
                    {
                        lockSlim.ExitWriteLock();
                    }

                }));
            }
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
            if (IsDisposed) return;
            txtbModule.Text = module;
            FilterResults();
        }


        private void FilterResults()
        {
            if (txtbInclude.Text == txtbInclude.Properties.NullText)
            {
                txtbInclude.Text = string.Empty;
            }

            if (txtbExclude.Text == txtbExclude.Properties.NullText)
            {
                txtbExclude.Text = string.Empty;
            }

            if (txtbSource.Text == txtbSource.Properties.NullText)
            {
                txtbSource.Text = string.Empty;
            }

            if (txtbModule.Text == txtbModule.Properties.NullText)
            {
                txtbModule.Text = string.Empty;
            }

            string include = txtbInclude.Text.Trim();
            string exclude = txtbExclude.Text.Trim();
            if (!autoCompleteInclude.Contains(include))
            {
                autoCompleteInclude.Add(include);
            }

            if (!autoCompleteExclude.Contains(exclude))
            {
                autoCompleteExclude.Add(exclude);
            }

            Settings.AddNewSearchesEntryToLists(include, true);
            Settings.AddNewSearchesEntryToLists(exclude, false);
            _filterCriteria.NewerThan = ceNewerThanFilter.Checked ? deNewerThanFilter.DateTime : DateTime.MinValue;
            _filterCriteria.OlderThan = ceOlderThanFilter.Checked ? deOlderThanFilter.DateTime : DateTime.MaxValue;
            _filterCriteria.TextInclude = ceIncludeText.Checked ? txtbInclude.Text.Trim() : string.Empty;
            _filterCriteria.TextExclude = ceExcludeText.Checked
                ? txtbExclude.Text.Trim() + "|" + string.Join("|", _excludeMostCommon)
                : string.Empty;

            Settings.IncludeText = Settings.SaveSearchFilters ? txtbInclude.Text : string.Empty;
            Settings.ExcludeText = Settings.SaveSearchFilters ? txtbExclude.Text : string.Empty;

            _filterCriteria.Levels = null;
            switch (logLevelSelectionType)
            {
                case LogLevelSelectionType.Single:
                    if (chkLstLogLevel.Items[0].CheckState == CheckState.Checked)
                    {
                        _filterCriteria.Levels = new[]
                            {AnalogyLogLevel.Trace,  AnalogyLogLevel.Unknown};
                    }

                    if (chkLstLogLevel.Items[1].CheckState == CheckState.Checked)
                    {
                        _filterCriteria.Levels = new[]
                        {
                            AnalogyLogLevel.Error, AnalogyLogLevel.Critical,  AnalogyLogLevel.Unknown
                        };
                    }
                    else if (chkLstLogLevel.Items[2].CheckState == CheckState.Checked)
                    {
                        _filterCriteria.Levels = new[]
                            {AnalogyLogLevel.Warning,  AnalogyLogLevel.Unknown};
                    }
                    else if (chkLstLogLevel.Items[3].CheckState == CheckState.Checked)
                    {
                        _filterCriteria.Levels = new[]
                            {AnalogyLogLevel.Debug,  AnalogyLogLevel.Unknown};
                    }
                    else if (chkLstLogLevel.Items[4].CheckState == CheckState.Checked)
                    {
                        _filterCriteria.Levels = new[]
                            {AnalogyLogLevel.Verbose,  AnalogyLogLevel.Unknown};
                    }

                    break;
                case LogLevelSelectionType.Multiple:
                    _filterCriteria.Levels = chkLstLogLevel.CheckedItems.Cast<CheckedListBoxItem>()
                        .Select(level => (AnalogyLogLevel)Enum.Parse(typeof(AnalogyLogLevel), (string)level.Value))
                        .ToArray();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }


            if (ceSources.Checked && !string.IsNullOrEmpty(txtbSource.Text))
            {
                var items = txtbSource.Text.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(v => v.Trim()).ToList();
                var includeItems = items.Where(i => !i.StartsWith("-"));
                var excludeItems = items.Where(i => i.StartsWith("-") && i.Length > 1)
                    .Select(i => i.Substring(1, i.Length - 1));

                _filterCriteria.Sources = includeItems.Select(val => val.Trim()).ToArray();
                _filterCriteria.ExcludedSources = excludeItems.Select(val => val.Trim()).ToArray();
            }
            else
            {
                _filterCriteria.Sources = null;
                _filterCriteria.ExcludedSources = null;
            }

            Settings.SourceText = Settings.SaveSearchFilters ? txtbSource.Text : string.Empty;

            if (ceModulesProcess.Checked && !string.IsNullOrEmpty(txtbModule.Text))
            {

                var items = txtbModule.Text.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(v => v.Trim()).ToList();
                var includeItems = items.Where(i => !i.StartsWith("-"));
                var excludeItems = items.Where(i => i.StartsWith("-") && i.Length > 1)
                    .Select(i => i.Substring(1, i.Length - 1));

                _filterCriteria.Modules = includeItems.Select(val => val.Trim()).ToArray();
                _filterCriteria.ExcludedModules = excludeItems.Select(val => val.Trim()).ToArray();
            }
            else
            {
                _filterCriteria.Modules = null;
                _filterCriteria.ExcludedModules = null;
            }

            Settings.ModuleText = Settings.SaveSearchFilters ? txtbModule.Text : string.Empty;
            string filter = _filterCriteria.GetSqlExpression();
            lockSlim.EnterWriteLock();
            if (LogGrid.ActiveFilterEnabled && !string.IsNullOrEmpty(LogGrid.ActiveFilterString))
            {
                CriteriaOperator op = LogGrid.ActiveFilterCriteria;
                string filterString = CriteriaToWhereClauseHelper.GetDataSetWhere(op);
                filter = $"{filter} and {filterString}";
            }

            try
            {
                _messageData.DefaultView.RowFilter = filter;
                var location = LocateByValue(0, gridColumnObject, _currentMassage);
                if (location >= 0)
                {
                    LogGrid.FocusedRowHandle = location;
                }

                //LogGrid.RefreshData();
                //RefreshUIMessagesCount();
            }
            finally
            {
                lockSlim.ExitWriteLock();
            }



        }

        public virtual int LocateByValue(int startRowHandle, GridColumn column, AnalogyLogMessage val)
        {
            if (!LogGrid.DataController.IsReady || val == null)
            {
                return int.MinValue;
            }

            startRowHandle = Math.Max(0, startRowHandle);
            if (LogGrid.IsServerMode)
            {
                if (startRowHandle != 0)
                {
                    throw new ArgumentException("Argument must be '0' in server mode.", nameof(startRowHandle));
                }
            }

            try
            {
                if (LogGrid.IsServerMode)
                {
                    return LogGrid.DataController.FindRowByValue(column.FieldName, val, null);
                }

                for (int rowHandle = startRowHandle;
                    rowHandle < LogGrid.DataController.VisibleListSourceRowCount;
                    ++rowHandle)
                {
                    object rowCellValue = LogGrid.GetRowCellValue(rowHandle, column.Caption);
                    if (Equals(val, rowCellValue))
                    {
                        return rowHandle;
                    }
                }
            }
            catch
            {
                //do nothing
            }

            return int.MinValue;
        }

        //private void RefreshUIMessagesCount()
        //{
        //if (!IsHandleCreated) return;
        //BeginInvoke(new MethodInvoker(() =>
        //{
        //    var result = GetRowsCount();
        //    lblTotalMessages.Text =
        //        $"Total messages:{result.total}. Errors:{result.error}. Warnings:{result.warning}. Criticals:{result.critical}.";
        //    if (result.alerts > 0)
        //    {
        //        lblTotalMessagesAlert.Text = $" ALERTS EXISTS: {result.alerts}!";
        //        lblTotalMessagesAlert.Visible = true;
        //    }
        //    else
        //    {
        //        lblTotalMessagesAlert.Visible = false;
        //    }

        //}));
        //}

        public async Task LoadFilesAsync(List<string> fileNames, bool clearLogBeforeLoading,
            bool isReloadSoForceNoCaching = false)
        {
            LoadedFiles = fileNames;
            bbtnReload.Visibility = BarItemVisibility.Always;
            CancellationTokenSource = new CancellationTokenSource();
            CancellationToken token = CancellationTokenSource.Token;
            if (clearLogBeforeLoading)
            {
                ClearLogs(false);
            }

            sBtnCancel.Visible = true;
            progressBar1.Value = 0;
            progressBar1.Maximum = fileNames.Count;
            progressBar1.Style = fileNames.Count > 1 ? ProgressBarStyle.Continuous : ProgressBarStyle.Marquee;
            fileLoadingCount = fileNames.Count;
            progressBar1.Visible = true;
            int processed = 0;
            foreach (string filename in fileNames)
            {
                if (!File.Exists(filename))
                {
                    AnalogyLogMessage m = new AnalogyLogMessage($"File {filename} does not exist",
                        AnalogyLogLevel.Critical, AnalogyLogClass.General, "Analogy", "None");
                    AppendMessage(m, "Analogy");
                    continue;
                }

                Text = @"File: " + filename;
                await FileProcessor.Process(FileDataProvider, filename, token, isReloadSoForceNoCaching);
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
            {
                BookmarkPersistManager.Instance.ClearBookmarks();
            }

            lockSlim.ExitWriteLock();

        }


        private void LoadTextBoxes(AnalogyLogMessage m)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(() => rtxtContent.Text = m.Text));
            }
            else
            {
                rtxtContent.Text = m.Text;
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

        private void gridControlBookmarkedMessages_DoubleClick(object sender, EventArgs e)
        {
            if (!(e is DXMouseEventArgs args))
            {
                return;
            }

            GoToMessage();

        }

        private void pmsGrid_DoubleClick(object sender, EventArgs e)
        {
            if (!(e is DXMouseEventArgs args))
            {
                return;
            }

            OpenMessageDetails();
            //CreateBookmark();

        }

        private void OpenMessageDetails()
        {
            (AnalogyLogMessage message, string dataSource) = GetMessageFromSelectedFocusedRowInGrid();
            if (message == null)
            {
                return;
            }

            FormMessageDetails details = new FormMessageDetails(message, Messages, dataSource);
            details.Show(this);
        }

        private void tsmiBookmark_Click(object sender, EventArgs e)
        {
            CreateBookmark(false);
        }

        private void CreateBookmark(bool persists)
        {

            (AnalogyLogMessage message, _) = GetMessageFromSelectedFocusedRowInGrid();
            int[] selRows = LogGrid.GetSelectedRows();
            if (message == null)
            {
                return;
            }

            lockSlim.EnterWriteLock();
            string dataSource = (string)LogGrid.GetRowCellValue(selRows.First(), "DataProvider") ?? string.Empty;
            AddExtraColumnsIfNeededToTable(_bookmarkedMessages, gridViewBookmarkedMessages, message);
            DataRow dtr = Utils.CreateRow(_bookmarkedMessages, message, dataSource,
                Settings.CheckAdditionalInformation);
            if (diffStartTime > DateTime.MinValue)
            {
                dtr["TimeDiff"] = message.Date.Subtract(diffStartTime).ToString();
            }

            _bookmarkedMessages.Rows.Add(dtr);
            _bookmarkedMessages.AcceptChanges();
            btswitchMessageDetails.Checked = true;
            Settings.ShowMessageDetails = btswitchMessageDetails.Checked;
            splitContainerMain.Collapsed = false;
            tcBottom.SelectedTabPage = xtpBookmarks;
            if (persists)
            {
                BookmarkPersistManager.Instance.AddBookmarkedMessage(message, dataSource);
            }

            lockSlim.ExitWriteLock();
        }

        private void AddExtraColumnsIfNeededToTable(DataTable table, GridView view, AnalogyLogMessage message)
        {
            if (message.AdditionalInformation != null && message.AdditionalInformation.Any() &&
                Settings.CheckAdditionalInformation)
            {
                foreach (KeyValuePair<string, string> info in message.AdditionalInformation)
                {
                    if (!table.Columns.Contains(info.Key))
                    {

                        if (!InvokeRequired)
                        {
                            if (!view.Columns.Select(g => g.FieldName).Contains(info.Key))
                            {
                                view.Columns.Add(new GridColumn() { Caption = info.Key, FieldName = info.Key, Name = info.Key, Visible = true });
                                table.Columns.Add(info.Key);
                            }

                        }
                        else
                        {
                            BeginInvoke(new MethodInvoker(() =>
                            {
                                if (!view.Columns.Select(g => g.FieldName).Contains(info.Key))
                                {
                                    view.Columns.Add(new GridColumn()
                                    { Caption = info.Key, FieldName = info.Key, Name = info.Key, Visible = true });
                                    table.Columns.Add(info.Key);
                                }
                                columnAdderSync.Set();
                            }));
                            columnAdderSync.WaitOne();
                            columnAdderSync.Reset();

                        }
                    }
                }
            }


        }

        private void GoToMessage()
        {
            int[] selRows = gridViewBookmarkedMessages.GetSelectedRows();
            if (selRows == null || selRows.Length != 1)
            {
                return;
            }

            int rownum = selRows.First();
            var currentRow = (DataRowView)gridViewBookmarkedMessages.GetRow(rownum);
            try
            {
                var LogMessage = currentRow["Object"] as AnalogyLogMessage;
                var location = LocateByValue(0, gridColumnObject, LogMessage);
                if (location >= 0)
                {
                    LogGrid.FocusedRowHandle = location;
                }
                else
                {
                    XtraMessageBox.Show("Cannot go to message", "Message not found", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            catch (Exception)
            {

                XtraMessageBox.Show("Cannot go to message", "Message not found", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }



        private async void chkbHighlight_CheckedChanged(object sender, EventArgs e)
        {
            await FilterHasChanged();
        }

        private async void txtbHighlight_KeyUp(object sender, KeyEventArgs e)
        {
            chkbHighlight.Checked = !string.IsNullOrEmpty(txtbHighlight.Text);
            HighlightRows.Clear();
            await FilterHasChanged();
        }

        private void tsmiExcludeSource_Click(object sender, EventArgs e)
        {
            (AnalogyLogMessage message, _) = GetMessageFromSelectedFocusedRowInGrid();
            if (!string.IsNullOrEmpty(message?.Source))
            {
                txtbSource.Text = txtbSource.Text == txtbSource.Properties.NullText ? "-" + message.Source : txtbSource.Text + ", -" + message.Source;
            }
        }

        private void tsmiExcludeModule_Click(object sender, EventArgs e)
        {
            (AnalogyLogMessage message, _) = GetMessageFromSelectedFocusedRowInGrid();
            if (!string.IsNullOrEmpty(message?.Module))
            {
                txtbModule.Text = txtbModule.Text == txtbModule.Properties.NullText ? "-" + message.Module : txtbModule.Text + ",-" + message.Module;
            }
        }


        private void tsmiTimeDiff_Click(object sender, EventArgs e)
        {
            (AnalogyLogMessage message, _) = GetMessageFromSelectedFocusedRowInGrid();
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


        private void btswitchExpand_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            Settings.ShowMessageDetails = btswitchMessageDetails.Checked;
            splitContainerMain.Collapsed = !btswitchMessageDetails.Checked;
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
                        XtraMessageBox.Show(e.Message, @"Error Saving file", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }

                }
            }
            else
            {
                if (XtraMessageBox.Show(
                    "Current Data Source does not support Save Operation" + Environment.NewLine +
                    "Do you want to Save in Analogy Json Format?", @"Save not Supported", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Error) == DialogResult.Yes)
                {
                    SaveMessagesToLog(AnalogyOfflineDataProvider, messages);
                }
                else
                {
                    XtraMessageBox.Show("Operation Aborted", @"Save file", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }

        }

        private async void bBtnImport_ItemClick(object sender, ItemClickEventArgs e)
        {

            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter =
                "Plain XML log file (*.xml)|*.xml|JSON file (*.json)|*.json|NLOG file (*.nlog)|*.nlog|Zipped XML log file (*.zip)|*.zip|ETW log file (*.etl)|*.etl";
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

        private async void sBtnMostCommon_Click(object sender, EventArgs e)
        {
            List<string> items;

            lockSlim.EnterReadLock();
            items = Messages.Select(r => r.Text).ToList();
            lockSlim.ExitReadLock();

            AnalogyExclude ef = new AnalogyExclude(items, _excludeMostCommon);
            if (ef.ShowDialog(this) == DialogResult.OK)
            {
                _excludeMostCommon = AnalogyExclude.GlobalExclusion;
                ceExcludeText.Checked = true;
                await FilterHasChanged();
            }
        }

        private async void chkLstLogLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            await FilterHasChanged();
        }


        private async void chkLstLogLevel_ItemCheck(object sender, DevExpress.XtraEditors.Controls.ItemCheckEventArgs e)
        {
            await FilterHasChanged();
        }

        private void sBtnLength_Click(object sender, EventArgs e)
        {
            ApplyGrouping();
        }

        private void sBtnGroup_Click(object sender, EventArgs e)
        {
            ApplyGrouping();

        }

        private void ApplyGrouping()
        {
            if (rbGroupByTextLength.Checked)
            {
                gCtrlGrouping.DataSource = null;
                List<IGrouping<string, AnalogyLogMessage>> grouped = Messages
                    .GroupBy(s => s.Text.Substring(0, Math.Min(s.Text.Length, (int)nudGroupBychars.Value)))
                    .OrderByDescending(i => i.Count()).ToList();
                groupingByChars = grouped.ToDictionary(g => g.Key, g => g.ToList());
                gCtrlGrouping.DataSource = groupingByChars.Keys;
            }
            else // group by text
            {
                gCtrlGrouping.DataSource = null;
                List<IGrouping<string, AnalogyLogMessage>> grouped = Messages.Where(m => m.Text.Contains(txtbGroupByChars.Text, StringComparison.CurrentCultureIgnoreCase))
                    .GroupBy(s => s.Text)
                    .OrderByDescending(i => i.Count()).ToList();
                groupingByChars = grouped.ToDictionary(g => g.Key, g => g.ToList());
                gCtrlGrouping.DataSource = groupingByChars.Keys;
            }
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
                gridControl.MainView.SaveLayoutToXml(Settings.LogGridFileName);
            }
            catch (Exception e)
            {
                AnalogyLogger.Instance.LogException($"Error saving setting: {e.Message}", e, "Analogy");
                XtraMessageBox.Show(e.Message, $"Error Saving layout file: {e.Message}", MessageBoxButtons.OK, MessageBoxIcon.Error);

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
            (AnalogyLogMessage message, _) = GetMessageFromSelectedFocusedRowInGrid();
            if (message != null)
            {
                BookmarkPersistManager.Instance.RemoveBookmark(message);
            }
        }
        //private void logGrid_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        //{
        //    if (e.Column.FieldName == "gridColumnLevelImage")
        //    {
        //        string severity = logGrid.GetListSourceRowCellValue(e.ListSourceRowIndex, gridViewGrouping2.Columns["Level"])
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
            FactoryContainer analogy = FactoriesManager.Instance.GetBuiltInFactoryContainer(AnalogyBuiltInFactory.AnalogyGuid);
            var provider = analogy.DataProvidersFactories[0].DataProviders.First();
            SetFileDataSource(provider, null);
            BookmarkView = true;
            BookmarkModeUI();
        }

        public void RemoveMessage(AnalogyLogMessage msgMessage)
        {
            var row = _messageData.AsEnumerable().SingleOrDefault(r => r["Object"] == msgMessage);
            if (row != null)
            {
                _messageData.Rows.Remove(row);
            }
        }

        private (AnalogyLogMessage message, string dataProvider) GetMessageFromSelectedFocusedRowInGrid()
        {
            var row = LogGrid.GetFocusedRow();
            if (row == null)
            {
                return (null, string.Empty);
            }

            string dataSource = (string)LogGrid.GetFocusedRowCellValue("DataProvider");
            AnalogyLogMessage message = (AnalogyLogMessage)LogGrid.GetFocusedRowCellValue("Object");
            if (message.Module == null)
            {
                message.Module = string.Empty;
            }

            if (message.Source == null)
            {
                message.Source = string.Empty;
            }

            if (message.Text == null)
            {
                message.Text = string.Empty;
            }

            return (message, dataSource);

        }
        private List<AnalogyLogMessage> GetMessagesFromSelectedRowInGrid(out string dataProvider)
        {
            dataProvider = string.Empty;
            var selectedRowHandles = logGrid.GetSelectedRows();
            List<AnalogyLogMessage> messages = new List<AnalogyLogMessage>();
            for (int i = 0; i < selectedRowHandles.Length; i++)
            {

                if (selectedRowHandles[i] >= 0)
                {
                    dataProvider = (string)LogGrid.GetRowCellValue(selectedRowHandles[i], "DataProvider");
                    AnalogyLogMessage message = (AnalogyLogMessage)LogGrid.GetRowCellValue(selectedRowHandles[i], "Object");
                    messages.Add(message);
                }
            }

            return messages;

        }
        private void gridViewGrouping_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            if (e.FocusedRowHandle < 0)
            {
                return;
            }

            var grouped = Utils.DataTableConstructor();
            string key =
                (string)gridViewGrouping.GetRowCellValue(e.FocusedRowHandle, gridViewGrouping.Columns.First());
            var messages = groupingByChars[key];
            foreach (var message in messages)
            {
                AddExtraColumnsIfNeededToTable(grouped, gridViewGrouping2, message);
                DataRow dtr = Utils.CreateRow(grouped, message, "", Settings.CheckAdditionalInformation);
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
            CancellationTokenSource.Cancel(false);
            Interlocked.Exchange(ref fileLoadingCount, 0);

            CancellationTokenSource = new CancellationTokenSource();
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
            if (!messages.Any())
            {
                return;
            }

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
            {
                btsAutoScrollToBottom.Checked = false;
            }
        }

        private void sbtnPageFirst_Click(object sender, EventArgs e)
        {
            pageNumber = 1;
            UpdatePage(PagingManager.FirstPage());
        }

        private void sbtnPagePrevious_Click(object sender, EventArgs e)
        {
            if (pageNumber == 1)
            {
                return;
            }

            pageNumber--;
            UpdatePage(PagingManager.PrevPage().Data);
        }

        private void sBtnPageNext_Click(object sender, EventArgs e)
        {
            if (pageNumber == TotalPages)
            {
                return;
            }

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


            var count = LogGrid.RowCount;

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
                        LogGrid.ExportToXlsx(saveFileDialog.FileName);
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
                        LogGrid.ExportToXls(saveFileDialog.FileName);
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
                LogGrid.ExportToCsv(saveFileDialog.FileName);
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
                LogGrid.ExportToHtml(saveFileDialog.FileName, op);
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
            if (!msg.Any())
            {
                return;
            }

            var source = GetFilteredDataTable().Rows[0]?["DataProvider"]?.ToString();
            if (source == null)
            {
                return;
            }

            XtraFormLogGrid grid = new XtraFormLogGrid(msg, source, DataProvider, FileDataProvider);
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
            if (row < 0)
            {
                return;
            }

            AnalogyLogMessage m = (AnalogyLogMessage)LogGrid.GetRowCellValue(e.FocusedRowHandle, "Object");
            LoadTextBoxes(m);
            string dataProvider = (string)LogGrid.GetRowCellValue(e.FocusedRowHandle, "DataProvider");
            if (!LoadingInProgress)
            {
                OnFocusedRowChanged?.Invoke(this, (dataProvider, m));
            }
        }

        private void tsmiIncreaseFont_Click(object sender, EventArgs e)
        {
            var fontSize = Settings.FontSettings.GridFontSize = LogGrid.Appearance.Row.Font.Size + 2;
            LogGrid.Appearance.Row.Font = new Font(LogGrid.Appearance.Row.Font.Name, fontSize);
            gridViewBookmarkedMessages.Appearance.Row.Font = new Font(LogGrid.Appearance.Row.Font.Name, fontSize);
            SaveGridLayout();
        }

        private void tsmiDecreaseFont_Click(object sender, EventArgs e)
        {
            if (LogGrid.Appearance.Row.Font.Size < 5)
            {
                return;
            }

            {
                var fontSize = Settings.FontSettings.GridFontSize = LogGrid.Appearance.Row.Font.Size - 2;
                LogGrid.Appearance.Row.Font = new Font(LogGrid.Appearance.Row.Font.Name, fontSize);
                gridViewBookmarkedMessages.Appearance.Row.Font = new Font(LogGrid.Appearance.Row.Font.Name, fontSize);
                SaveGridLayout();
            }
        }

        private void tsmiClearLog_Click(object sender, EventArgs e)
        {
            ClearLogs(true);
        }

        private void tsmiREmoveAllPreviousMessages_Click(object sender, EventArgs e)
        {
            (AnalogyLogMessage current, _) = GetMessageFromSelectedFocusedRowInGrid();
            if (current == null)
            {
                return;
            }

            lockSlim.EnterWriteLock();
            while (_messageData.Rows.Count > 0)
            {
                if (!Equals(_messageData.Rows[0]["Object"], current))
                {
                    _messageData.Rows.RemoveAt(0);
                }
                else
                {
                    break;
                }
            }
            lockSlim.ExitWriteLock();
            //RefreshUIMessagesCount();

        }

        private void bBtnDataVisualizer_ItemClick(object sender, ItemClickEventArgs e)
        {
            DataVisualizerForm sv = new DataVisualizerForm(() => Messages);
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
            if (!msg.Any())
            {
                return;
            }

            var source = GetFilteredDataTable().Rows[0]?["DataProvider"]?.ToString();
            if (source == null)
            {
                return;
            }

            var processes = msg.Select(m => m.Module).Distinct().ToList();
            foreach (string process in processes)
            {
                XtraFormLogGrid grid = new XtraFormLogGrid(msg, source, DataProvider, FileDataProvider, process);
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
            txtbInclude.Text = "";
        }

        private void sbtnTextExclude_Click(object sender, EventArgs e)
        {
            txtbExclude.Text = "";
        }

        private void sbtnIncludeSources_Click(object sender, EventArgs e)
        {
            txtbSource.Text = "";
        }


        private void sbtnIncludeModules_Click(object sender, EventArgs e)
        {
            txtbModule.Text = "";
        }


        private void sbtnUndockPerProcess_Click(object sender, EventArgs e)
        {
            UndockViewPerProcess();
        }


        private void tsmiDateFilterNewer_Click(object sender, EventArgs e)
        {
            (AnalogyLogMessage message, _) = GetMessageFromSelectedFocusedRowInGrid();
            deNewerThanFilter.DateTime = message.Date;
            ceNewerThanFilter.Checked = true;
        }

        private void tsmiDateFilterOlder_Click(object sender, EventArgs e)
        {
            (AnalogyLogMessage message, _) = GetMessageFromSelectedFocusedRowInGrid();
            deOlderThanFilter.DateTime = message.Date;
            ceOlderThanFilter.Checked = true;
        }

        private void tsmiBookmarkDateFilterNewer_Click(object sender, EventArgs e)
        {
            (AnalogyLogMessage message, _) = GetMessageFromSelectedFocusedRowInGrid();
            deNewerThanFilter.DateTime = message.Date;
            ceNewerThanFilter.Checked = true;
        }

        private void tsmiBookmarkDateFilterOlder_Click(object sender, EventArgs e)
        {
            (AnalogyLogMessage message, _) = GetMessageFromSelectedFocusedRowInGrid();
            deOlderThanFilter.DateTime = message.Date;
            ceOlderThanFilter.Checked = true;
        }

        private void sbtnMoreHighlight_Click(object sender, EventArgs e)
        {
            UserSettingsForm user = new UserSettingsForm(1);
            user.ShowDialog(this);
        }

        private void sbtnPreDefinedFilters_Click(object sender, EventArgs e)
        {
            if (!Settings.PreDefinedQueries.Filters.Any())
            {
                return;
            }

            contextMenuStripFilters.Items.Clear();
            foreach (PreDefineFilter filter in Settings.PreDefinedQueries.Filters)
            {

                ToolStripMenuItem item = new ToolStripMenuItem(filter.ToString());
                item.Click += (s, arg) =>
                {
                    txtbInclude.Text = filter.IncludeText;
                    txtbExclude.Text = filter.ExcludeText;
                    txtbSource.Text = filter.Sources;
                    txtbModule.Text = filter.Modules;
                };
                contextMenuStripFilters.Items.Add(item);
            }

            contextMenuStripFilters.Show(sbtnPreDefinedFilters.PointToScreen(sbtnPreDefinedFilters.Location));
        }

        public void EnableFileReload(string fileName)
        {
            LoadedFiles = new List<string>() { fileName };
            bbtnReload.Visibility = BarItemVisibility.Always;
        }

        public void SetReloadColorDate(DateTime value) => reloadDateTime = value;

        private void bBtnSaveCurrentSelectionCustomFormat_ItemClick(object sender, ItemClickEventArgs e)
        {

            SaveMessagesToLog(FileDataProvider, GetMessagesFromSelectedRowInGrid(out _));
        }

        private void bBtnSaveCurrentSelectionAnalogyFormat_ItemClick(object sender, ItemClickEventArgs e)
        {
            SaveMessagesToLog(AnalogyOfflineDataProvider, GetMessagesFromSelectedRowInGrid(out _));

        }

        private void bBtnUndockSelection_ItemClick(object sender, ItemClickEventArgs e)
        {
            var msg = GetMessagesFromSelectedRowInGrid(out var source);
            if (!msg.Any() || source == null)
            {
                return;
            }

            XtraFormLogGrid grid = new XtraFormLogGrid(msg, source, DataProvider, FileDataProvider);
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

        private void nudGroupBychars_ValueChanged(object sender, EventArgs e)
        {
            rbGroupByTextLength.Checked = true;
        }

        private void txtbGroupByChars_Click(object sender, EventArgs e)
        {
            rbGroupByText.Checked = true;
        }

        private void tsmiAddCommentToMessage_Click(object sender, EventArgs e)
        {
            var msg = GetMessageFromSelectedFocusedRowInGrid();
            if (msg.message != null)
            {
                var addNoteForm = new AnalogyAddCommentsToMessage(msg.message);
                addNoteForm.Show(this);
            }

        }

        private void txtbInclude_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (IsDisposed) return;
            xtcFilters.SelectedTabPage = xtpFiltersIncludes;
        }

        private void txtbExclude_EditValueChanged(object sender, EventArgs e)
        {
            if (IsDisposed) return;
            xtcFilters.SelectedTabPage = xtpFiltersExclude;
        }
    }
}



