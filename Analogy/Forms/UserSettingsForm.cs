using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Analogy.DataTypes;
using Analogy.Interfaces;
using Analogy.Managers;
using Analogy.Properties;
using DevExpress.Utils;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;

namespace Analogy.Forms
{

    public partial class UserSettingsForm : XtraForm
    {


        private DataTable messageData;
        private UserSettingsManager Settings { get; } = UserSettingsManager.UserSettings;
        private int InitialSelection = -1;

        public UserSettingsForm()
        {
            InitializeComponent();
            messageData = Utils.DataTableConstructor();
        }

        public UserSettingsForm(int tabIndex) : this()
        {
            InitialSelection = tabIndex;
        }

        private void UserSettingsForm_Load(object sender, EventArgs e)
        {
            ShowIcon = true;
            logGrid.MouseDown += logGrid_MouseDown;
            Icon = UserSettingsManager.UserSettings.GetIcon();
            LoadSettings();
            SetupEventsHandlers();
            if (File.Exists(Settings.LogGridFileName))
            {
                gridControl.MainView.RestoreLayoutFromXml(Settings.LogGridFileName);
            }

            gridControl.DataSource = messageData.DefaultView;
            SetupExampleMessage("Test 1");
            SetupExampleMessage("Test 2");
            if (InitialSelection >= 0)
            {
                tabControlMain.SelectedTabPageIndex = InitialSelection;
            }
        }
        void logGrid_MouseDown(object sender, MouseEventArgs e)
        {
            GridHitInfo info = logGrid.CalcHitInfo(e.Location);
            if (info.InColumnPanel)
            {
                teHeader.Tag = info.Column;
                teHeader.Text = info.Column.Caption;
            }
        }
        private void SetupEventsHandlers()
        {
            tsAutoComplete.IsOnChanged += (s, e) => { Settings.RememberLastSearches = tsAutoComplete.IsOn; };
            nudAutoCompleteCount.ValueChanged += (s, e) =>
            {
                Settings.NumberOfLastSearches = (int)nudAutoCompleteCount.Value;
            };
            cpeNewMessagesColor.ColorChanged += RowColors_ColorChanged;
            cpeHighlightColor.ColorChanged += RowColors_ColorChanged;
            cpeLogLevelAnalogyInformation.ColorChanged += RowColors_ColorChanged;
            cpeLogLevelCritical.ColorChanged += RowColors_ColorChanged;
            cpeLogLevelError.ColorChanged += RowColors_ColorChanged;
            cpeLogLevelWarning.ColorChanged += RowColors_ColorChanged;
            cpeLogLevelEvent.ColorChanged += RowColors_ColorChanged;
            cpeLogLevelDebug.ColorChanged += RowColors_ColorChanged;
            cpeLogLevelVerbose.ColorChanged += RowColors_ColorChanged;
            cpeLogLevelTrace.ColorChanged += RowColors_ColorChanged;
            cpeLogLevelDisabled.ColorChanged += RowColors_ColorChanged;
            cpeLogLevelUnknown.ColorChanged += RowColors_ColorChanged;
            cpeLogLevelAnalogyInformationText.ColorChanged += RowColors_ColorChanged;
            cpeLogLevelCriticalText.ColorChanged += RowColors_ColorChanged;
            cpeLogLevelErrorText.ColorChanged += RowColors_ColorChanged;
            cpeLogLevelWarningText.ColorChanged += RowColors_ColorChanged;
            cpeLogLevelEventText.ColorChanged += RowColors_ColorChanged;
            cpeLogLevelDebugText.ColorChanged += RowColors_ColorChanged;
            cpeLogLevelVerboseText.ColorChanged += RowColors_ColorChanged;
            cpeLogLevelTraceText.ColorChanged += RowColors_ColorChanged;
            cpeLogLevelDisabledText.ColorChanged += RowColors_ColorChanged;
            cpeLogLevelUnknownText.ColorChanged += RowColors_ColorChanged;
            cpeHighlightColorText.ColorChanged += RowColors_ColorChanged;
            cpeNewMessagesColorText.ColorChanged += RowColors_ColorChanged;
            gridControl.MainView.Layout += MainView_Layout;
            tsLogLevels.IsOnChanged += (s, e) =>
             {
                 Settings.LogLevelSelection = tsLogLevels.IsOn ? LogLevelSelectionType.Multiple : LogLevelSelectionType.Single;
                 Utils.SetLogLevel(chkLstLogLevel);
             };

            tsRibbonCompactStyle.IsOnChanged += (s, e) =>
            {
                Settings.RibbonStyle = tsRibbonCompactStyle.IsOn
                    ? CommandLayout.Simplified
                    : CommandLayout.Classic;
            };
        }

        private void MainView_Layout(object sender, EventArgs e)
        {
            if (tabControlMain.SelectedTabPage == xtraTabPageFilter && xtraTabPageFilter.Visible)
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
        }

        private void LoadSettings()
        {
            tsSimpleMode.IsOn = Settings.SimpleMode;
            tsEnableCompressedArchive.IsOn = Settings.EnableCompressedArchives;
            tsRememberLastPositionAndState.IsOn = Settings.AnalogyPosition.RememberLastPosition;
            logGrid.Columns["Date"].DisplayFormat.FormatType = FormatType.DateTime;
            logGrid.Columns["Date"].DisplayFormat.FormatString = Settings.DateTimePattern;
            tsHistory.IsOn = Settings.ShowHistoryOfClearedMessages;
            teDateTimeFormat.Text = Settings.DateTimePattern;
            tsFilteringExclude.IsOn = Settings.SaveSearchFilters;
            listBoxFoldersProbing.Items.AddRange(Settings.AdditionalProbingLocations.ToArray());
            tsAutoComplete.IsOn = Settings.RememberLastSearches;
            nudRecentFiles.Value = Settings.RecentFilesCount;
            nudRecentFolders.Value = Settings.RecentFoldersCount;
            tsUserStatistics.IsOn = Settings.EnableUserStatistics;
            //tsSimpleMode.IsOn = Settings.SimpleMode;
            tsFileCaching.IsOn = Settings.EnableFileCaching;
            tsStartupRibbonMinimized.IsOn = Settings.StartupRibbonMinimized;
            tsErrorLevelAsDefault.IsOn = Settings.StartupErrorLogLevel;
            chkEditPaging.Checked = Settings.PagingEnabled;
            if (Settings.PagingEnabled)
            {
                nudPageLength.Value = Settings.PagingSize;
            }
            else
            {
                nudPageLength.Enabled = false;
            }

            checkEditSearchAlsoInSourceAndModule.Checked = Settings.SearchAlsoInSourceAndModule;
            toggleSwitchIdleMode.IsOn = Settings.IdleMode;
            nudIdleTime.Value = Settings.IdleTimeMinutes;
            tsDataTimeAscendDescend.IsOn = Settings.DefaultDescendOrder;

            var startup = Settings.AutoStartDataProviders;
            var loaded = FactoriesManager.Instance.GetRealTimeDataSourcesNamesAndIds();
            foreach (var realTime in loaded)
            {
                FactoryCheckItem itm = new FactoryCheckItem(realTime.Name, realTime.ID, realTime.Description, realTime.Image);
                chkLstItemRealTimeDataSources.Items.Add(itm, startup.Contains(itm.ID));
            }

            foreach (var setting in Settings.FactoriesOrder)
            {
                FactorySettings factory = Settings.GetFactorySetting(setting);
                if (factory == null)
                {
                    continue;
                }

                var factoryContainer = FactoriesManager.Instance.FactoryContainer(factory.FactoryId);
                string about = (factoryContainer?.Factory != null) ? factoryContainer.Factory.About : "Disabled";
                var image = FactoriesManager.Instance.GetLargeImage(factory.FactoryId);
                FactoryCheckItem itm = new FactoryCheckItem(factory.FactoryName, factory.FactoryId, about, image);
                chkLstDataProviderStatus.Items.Add(itm, factory.Status == DataProviderFactoryStatus.Enabled);
            }
            //add missing:
            foreach (var factory in Settings.FactoriesSettings.Where(itm => !Settings.FactoriesOrder.Contains(itm.FactoryId)))
            {
                var factoryContainer = FactoriesManager.Instance.FactoryContainer(factory.FactoryId);
                string about = (factoryContainer?.Factory != null) ? factoryContainer.Factory.About : "Disabled";
                var image = FactoriesManager.Instance.GetLargeImage(factory.FactoryId);
                FactoryCheckItem itm = new FactoryCheckItem(factory.FactoryName, factory.FactoryId, about, image);
                chkLstDataProviderStatus.Items.Add(itm, factory.Status != DataProviderFactoryStatus.Disabled);
            }

            //file associations:

            cbDataProviderAssociation.Properties.DataSource = Settings.FactoriesSettings;
            cbDataProviderAssociation.Properties.DisplayMember = "FactoryName";
            cbDataProviderAssociation.EditValue = Settings.FactoriesSettings.First();
            //cbDataProviderAssociation.Properties.ValueMember = "ID";
            tsRememberLastOpenedDataProvider.IsOn = Settings.RememberLastOpenedDataProvider;
            lboxHighlightItems.DataSource = Settings.PreDefinedQueries.Highlights;
            lboxAlerts.DataSource = Settings.PreDefinedQueries.Alerts;
            lboxFilters.DataSource = Settings.PreDefinedQueries.Filters;
            nudAutoCompleteCount.Value = Settings.NumberOfLastSearches;
            tsSingleInstance.IsOn = Settings.SingleInstance;
            if (Settings.AnalogyIcon == "Light")
            {
                rbtnLightIconColor.Checked = true;
            }
            else
            {
                rbtnDarkIconColor.Checked = true;
            }
            LoadColorsSettings();
            cbUpdates.Properties.Items.AddRange(typeof(UpdateMode).GetDisplayValues().Values);
            cbUpdates.SelectedItem = UpdateManager.Instance.UpdateMode.GetDisplay();
            tsTraybar.IsOn = Settings.MinimizedToTrayBar;
            tsCheckAdditionalInformation.IsOn = Settings.CheckAdditionalInformation;
            tsLogLevels.IsOn = Settings.LogLevelSelection == LogLevelSelectionType.Multiple;
            Utils.SetLogLevel(chkLstLogLevel);

            switch (Settings.FontSettings.FontSelectionType)
            {
                case FontSelectionType.Normal:
                    rbFontSizeNormal.Checked = true;
                    break;
                case FontSelectionType.Large:
                    rbFontSizeLarge.Checked = true;
                    break;
                case FontSelectionType.VeryLarge:
                    rbFontSizeVeryLarge.Checked = true;
                    break;
                case FontSelectionType.Manual:
                default:
                    rbFontSizeNormal.Checked = true;
                    break;
            }
            switch (Settings.FontSettings.MenuFontSelectionType)
            {
                case FontSelectionType.Normal:
                    rbMenuFontSizeNormal.Checked = true;
                    break;
                case FontSelectionType.Large:
                    rbMenuFontSizeLarge.Checked = true;
                    break;
                case FontSelectionType.VeryLarge:
                    rbMenuFontSizeVeryLarge.Checked = true;
                    break;
                case FontSelectionType.Manual:
                default:
                    rbMenuFontSizeNormal.Checked = true;
                    break;

            }

            tsRibbonCompactStyle.IsOn = Settings.RibbonStyle == CommandLayout.Simplified;
            tsEnableFirstChanceException.IsOn = Settings.EnableFirstChanceException;
        }

        private void SaveSetting()
        {
            SaveColorsSettings();
            Settings.SimpleMode = tsSimpleMode.IsOn;
            Settings.LogLevelSelection = tsLogLevels.IsOn ? LogLevelSelectionType.Multiple : LogLevelSelectionType.Single;
            Settings.RecentFilesCount = (int)nudRecentFiles.Value;
            Settings.RecentFoldersCount = (int)nudRecentFolders.Value;
            List<Guid> order = new List<Guid>(chkLstDataProviderStatus.Items.Count);
            foreach (CheckedListBoxItem item in chkLstDataProviderStatus.Items)
            {
                FactoryCheckItem fci = (FactoryCheckItem)item.Value;
                order.Add(fci.ID);
                var guid = fci.ID;
                var factory = Settings.FactoriesSettings.SingleOrDefault(f => f.FactoryId == guid);
                if (factory != null)
                {
                    factory.Status = item.CheckState == CheckState.Checked
                        ? DataProviderFactoryStatus.Enabled
                        : DataProviderFactoryStatus.Disabled;
                }
            }

            Settings.RememberLastOpenedDataProvider = tsRememberLastOpenedDataProvider.IsOn;
            Settings.RememberLastSearches = tsAutoComplete.IsOn;
            Settings.UpdateOrder(order);
            Settings.AdditionalProbingLocations = listBoxFoldersProbing.Items.Cast<string>().ToList();
            Settings.SingleInstance = tsSingleInstance.IsOn;
            Settings.AnalogyIcon = rbtnLightIconColor.Checked ? "Light" : "Dark";
            var options = typeof(UpdateMode).GetDisplayValues();
            UpdateManager.Instance.UpdateMode = (UpdateMode)Enum.Parse(typeof(UpdateMode),
                options.Single(k => k.Value == cbUpdates.SelectedItem.ToString()).Key, true);
            Settings.MinimizedToTrayBar = tsTraybar.IsOn;
            Settings.CheckAdditionalInformation = tsCheckAdditionalInformation.IsOn;
            Settings.AnalogyPosition.RememberLastPosition = tsRememberLastPositionAndState.IsOn;
            Settings.EnableCompressedArchives = tsEnableCompressedArchive.IsOn;

            if (rbFontSizeNormal.Checked)
            {
                Settings.FontSettings.SetFontSelectionType(FontSelectionType.Normal);
            }

            if (rbFontSizeLarge.Checked)
            {
                Settings.FontSettings.SetFontSelectionType(FontSelectionType.Large);
            }

            if (rbFontSizeVeryLarge.Checked)
            {
                Settings.FontSettings.SetFontSelectionType(FontSelectionType.VeryLarge);
            }

            if (rbMenuFontSizeNormal.Checked)
            {
                Settings.FontSettings.SetMenuFontSelectionType(FontSelectionType.Normal);
            }

            if (rbMenuFontSizeLarge.Checked)
            {
                Settings.FontSettings.SetMenuFontSelectionType(FontSelectionType.Large);
            }

            if (rbMenuFontSizeVeryLarge.Checked)
            {
                Settings.FontSettings.SetMenuFontSelectionType(FontSelectionType.VeryLarge);
            }

            Settings.EnableFirstChanceException = tsEnableFirstChanceException.IsOn;

            Settings.Save();
        }

        private void SaveColorsSettings()
        {
            Settings.ColorSettings.SetColorForLogLevel(AnalogyLogLevel.Unknown, cpeLogLevelUnknown.Color, cpeLogLevelUnknownText.Color);
            Settings.ColorSettings.SetColorForLogLevel(AnalogyLogLevel.None, cpeLogLevelDisabled.Color, cpeLogLevelDisabledText.Color);
            Settings.ColorSettings.SetColorForLogLevel(AnalogyLogLevel.Trace, cpeLogLevelTrace.Color, cpeLogLevelTraceText.Color);
            Settings.ColorSettings.SetColorForLogLevel(AnalogyLogLevel.Verbose, cpeLogLevelVerbose.Color, cpeLogLevelVerboseText.Color);
            Settings.ColorSettings.SetColorForLogLevel(AnalogyLogLevel.Debug, cpeLogLevelDebug.Color, cpeLogLevelDebugText.Color);
            Settings.ColorSettings.SetColorForLogLevel(AnalogyLogLevel.Information, cpeLogLevelEvent.Color, cpeLogLevelEventText.Color);
            Settings.ColorSettings.SetColorForLogLevel(AnalogyLogLevel.Warning, cpeLogLevelWarning.Color, cpeLogLevelWarningText.Color);
            Settings.ColorSettings.SetColorForLogLevel(AnalogyLogLevel.Error, cpeLogLevelError.Color, cpeLogLevelErrorText.Color);
            Settings.ColorSettings.SetColorForLogLevel(AnalogyLogLevel.Critical, cpeLogLevelCritical.Color, cpeLogLevelCriticalText.Color);
            Settings.ColorSettings.SetColorForLogLevel(AnalogyLogLevel.Analogy, cpeLogLevelAnalogyInformation.Color, cpeLogLevelAnalogyInformationText.Color);
            Settings.ColorSettings.SetHighlightColor(cpeHighlightColor.Color, cpeHighlightColorText.Color);
            Settings.ColorSettings.SetNewMessagesColor(cpeNewMessagesColor.Color, cpeNewMessagesColorText.Color);
            Settings.ColorSettings.EnableNewMessagesColor = ceNewMessagesColor.Checked;
            Settings.ColorSettings.OverrideLogLevelColor = ceOverrideLogLevelColor.Checked;
            Settings.ColorSettings.EnableMessagesColors = tsEnableColors.IsOn;
        }
        private void LoadColorsSettings()
        {
            tsEnableColors.IsOn = Settings.ColorSettings.EnableMessagesColors;
            cpeLogLevelUnknown.Color = Settings.ColorSettings.GetColorForLogLevel(AnalogyLogLevel.Unknown).BackgroundColor;
            cpeLogLevelDisabled.Color = Settings.ColorSettings.GetColorForLogLevel(AnalogyLogLevel.None).BackgroundColor;
            cpeLogLevelTrace.Color = Settings.ColorSettings.GetColorForLogLevel(AnalogyLogLevel.Trace).BackgroundColor;
            cpeLogLevelVerbose.Color = Settings.ColorSettings.GetColorForLogLevel(AnalogyLogLevel.Verbose).BackgroundColor;
            cpeLogLevelDebug.Color = Settings.ColorSettings.GetColorForLogLevel(AnalogyLogLevel.Debug).BackgroundColor;
            cpeLogLevelEvent.Color = Settings.ColorSettings.GetColorForLogLevel(AnalogyLogLevel.Information).BackgroundColor;
            cpeLogLevelWarning.Color = Settings.ColorSettings.GetColorForLogLevel(AnalogyLogLevel.Warning).BackgroundColor;
            cpeLogLevelError.Color = Settings.ColorSettings.GetColorForLogLevel(AnalogyLogLevel.Error).BackgroundColor;
            cpeLogLevelCritical.Color = Settings.ColorSettings.GetColorForLogLevel(AnalogyLogLevel.Critical).BackgroundColor;
            cpeLogLevelAnalogyInformation.Color = Settings.ColorSettings.GetColorForLogLevel(AnalogyLogLevel.Analogy).BackgroundColor;
            cpeHighlightColor.Color = Settings.ColorSettings.GetHighlightColor().BackgroundColor;
            cpeNewMessagesColor.Color = Settings.ColorSettings.GetNewMessagesColor().BackgroundColor;

            cpeLogLevelUnknownText.Color = Settings.ColorSettings.GetColorForLogLevel(AnalogyLogLevel.Unknown).TextColor;
            cpeLogLevelDisabledText.Color = Settings.ColorSettings.GetColorForLogLevel(AnalogyLogLevel.None).TextColor;
            cpeLogLevelTraceText.Color = Settings.ColorSettings.GetColorForLogLevel(AnalogyLogLevel.Trace).TextColor;
            cpeLogLevelVerboseText.Color = Settings.ColorSettings.GetColorForLogLevel(AnalogyLogLevel.Verbose).TextColor;
            cpeLogLevelDebugText.Color = Settings.ColorSettings.GetColorForLogLevel(AnalogyLogLevel.Debug).TextColor;
            cpeLogLevelEventText.Color = Settings.ColorSettings.GetColorForLogLevel(AnalogyLogLevel.Information).TextColor;
            cpeLogLevelWarningText.Color = Settings.ColorSettings.GetColorForLogLevel(AnalogyLogLevel.Warning).TextColor;
            cpeLogLevelErrorText.Color = Settings.ColorSettings.GetColorForLogLevel(AnalogyLogLevel.Error).TextColor;
            cpeLogLevelCriticalText.Color = Settings.ColorSettings.GetColorForLogLevel(AnalogyLogLevel.Critical).TextColor;
            cpeLogLevelAnalogyInformationText.Color = Settings.ColorSettings.GetColorForLogLevel(AnalogyLogLevel.Analogy).TextColor;
            cpeHighlightColorText.Color = Settings.ColorSettings.GetHighlightColor().TextColor;
            cpeNewMessagesColorText.Color = Settings.ColorSettings.GetNewMessagesColor().TextColor;

            ceNewMessagesColor.Checked = Settings.ColorSettings.EnableNewMessagesColor;
            ceOverrideLogLevelColor.Checked = Settings.ColorSettings.OverrideLogLevelColor;
            UpdateColors();
        }

        private void SetupExampleMessage(string text)
        {
            DataRow dtr = messageData.NewRow();
            dtr.BeginEdit();
            dtr["Date"] = DateTime.Now;
            dtr["Text"] = text;
            dtr["Source"] = "Analogy";
            dtr["Level"] = AnalogyLogLevel.Information.ToString();
            dtr["Class"] = AnalogyLogClass.General.ToString();
            dtr["Category"] = "None";
            dtr["User"] = "None";
            dtr["Module"] = "Analogy";
            dtr["ProcessID"] = Process.GetCurrentProcess().Id;
            dtr["ThreadID"] = Thread.CurrentThread.ManagedThreadId;
            dtr["DataProvider"] = string.Empty;
            dtr["MachineName"] = "None";
            dtr.EndEdit();
            messageData.Rows.Add(dtr);
            messageData.AcceptChanges();
        }
        private void tsFilteringExclude_Toggled(object sender, EventArgs e)
        {
            Settings.SaveSearchFilters = tsFilteringExclude.IsOn;

        }

        private void tsHistory_Toggled(object sender, EventArgs e)
        {
            Settings.ShowHistoryOfClearedMessages = tsHistory.IsOn;
        }


        private void tsUserStatistics_Toggled(object sender, EventArgs e)
        {
            EnableDisableUserStatistics(tsUserStatistics.IsOn);
            Settings.EnableUserStatistics = tsUserStatistics.IsOn;

        }

        private void EnableDisableUserStatistics(bool isOn)
        {
            if (isOn)
            {
                lblLaunchCount.Text = $"Number of Analogy Launches: {Settings.AnalogyLaunches}";
                lblRunningTime.Text = $"Running Time: {Settings.DisplayRunningTime}";
                lblOpenedFiles.Text = $"Number Of Opened Files: {Settings.AnalogyOpenedFiles}";
            }
            else
            {
                lblLaunchCount.Text = $"Number of Analogy Launches: 0";
                lblRunningTime.Text = $"Running Time: 0";
                lblOpenedFiles.Text = $"Number Of Opened Files: N/A";
            }
        }

        private void btnClearStatistics_Click(object sender, EventArgs e)
        {
            XtraMessageBox.AllowCustomLookAndFeel = true;
            var result = XtraMessageBox.Show("Clear statistics?", "Confirmation Required", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                Settings.ClearStatistics();
            }

        }

        private void tsSimpleMode_Toggled(object sender, EventArgs e)
        {
            //Settings.SimpleMode = tsSimpleMode.IsOn;
        }

        private void tsFileCaching_Toggled(object sender, EventArgs e)
        {
            Settings.EnableFileCaching = tsFileCaching.IsOn;
        }

        private void tsStartupRibbonMinimized_Toggled(object sender, EventArgs e)
        {
            Settings.StartupRibbonMinimized = tsStartupRibbonMinimized.IsOn;
        }

        private void tsErrorLevelAsDefault_Toggled(object sender, EventArgs e)
        {
            Settings.StartupErrorLogLevel = tsErrorLevelAsDefault.IsOn;
        }

        private void chkEditPaging_CheckedChanged(object sender, EventArgs e)
        {
            Settings.PagingEnabled = chkEditPaging.Checked;
            nudPageLength.Enabled = Settings.PagingEnabled;
        }

        private void nudPageLength_ValueChanged(object sender, EventArgs e)
        {
            Settings.PagingSize = (int)nudPageLength.Value;
        }

        private void checkEditSearchAlsoInSourceAndModule_CheckedChanged(object sender, EventArgs e)
        {
            Settings.SearchAlsoInSourceAndModule = checkEditSearchAlsoInSourceAndModule.Checked;
        }

        private void ToggleSwitchIdleMode_Toggled(object sender, EventArgs e)
        {
            Settings.IdleMode = toggleSwitchIdleMode.IsOn;

        }

        private void NudIdleTime_ValueChanged(object sender, EventArgs e)
        {
            Settings.IdleTimeMinutes = (int)nudIdleTime.Value;

        }

        private void ChkLstItemRealTimeDataSources_SelectedIndexChanged(object sender, EventArgs e)
        {
            Settings.AutoStartDataProviders =
                chkLstItemRealTimeDataSources.CheckedItems.Cast<FactoryCheckItem>().Select(r => r.ID).ToList();
        }

        private void UserSettingsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveSetting();
        }



        private void tsDataTimeAscendDescend_Toggled(object sender, EventArgs e)
        {
            Settings.DefaultDescendOrder = tsDataTimeAscendDescend.IsOn;

        }

        private void sBtnExportColors_Click(object sender, EventArgs e)
        {
            SaveSetting();
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Analogy Color Settings (*.json)|*.json";
            saveFileDialog.Title = @"Export Analogy Color settings";

            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {

                try
                {
                    File.WriteAllText(saveFileDialog.FileName, Settings.ColorSettings.AsJson());
                    XtraMessageBox.Show("File Saved", @"Export settings", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                }
                catch (Exception ex)
                {
                    AnalogyLogManager.Instance.LogError("Error during save to file: " + e, nameof(sBtnExportColors_Click));
                    XtraMessageBox.Show("Error Export: " + ex.Message, @"Error Saving file", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }

            }
        }

        private void sBtnImportColors_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Analogy Color Settings (*.json)|*.json";
            openFileDialog1.Title = @"Import NLog settings";
            openFileDialog1.Multiselect = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var json = File.ReadAllText(openFileDialog1.FileName);
                    ColorSettings color = ColorSettings.FromJson(json);
                    Settings.ColorSettings = color;
                    LoadColorsSettings();
                    XtraMessageBox.Show("File Imported. Save settings if desired", @"Import settings",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                }
                catch (Exception ex)
                {
                    AnalogyLogManager.Instance.LogError("Error during import data: " + e, nameof(sBtnImportColors_Click));
                    XtraMessageBox.Show("Error Import: " + ex.Message, @"Error Import file", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        private void btnDataProviderCustomSettings_Click(object sender, EventArgs e)
        {
            UserSettingsDataProvidersForm user = new UserSettingsDataProvidersForm();
            user.ShowDialog(this);
        }

        private void sBtnMoveUp_Click(object sender, EventArgs e)
        {
            if (chkLstDataProviderStatus.SelectedIndex <= 0)
            {
                return;
            }

            var selectedIndex = chkLstDataProviderStatus.SelectedIndex;
            var currentValue = chkLstDataProviderStatus.Items[selectedIndex];
            chkLstDataProviderStatus.Items[selectedIndex] = chkLstDataProviderStatus.Items[selectedIndex - 1];
            chkLstDataProviderStatus.Items[selectedIndex - 1] = currentValue;
            chkLstDataProviderStatus.SelectedIndex = chkLstDataProviderStatus.SelectedIndex - 1;
        }

        private void sBtnMoveDown_Click(object sender, EventArgs e)
        {
            if (chkLstDataProviderStatus.SelectedIndex == chkLstDataProviderStatus.Items.Count - 1)
            {
                return;
            }

            var selectedIndex = chkLstDataProviderStatus.SelectedIndex;
            var currentValue = chkLstDataProviderStatus.Items[selectedIndex + 1];
            chkLstDataProviderStatus.Items[selectedIndex + 1] = chkLstDataProviderStatus.Items[selectedIndex];
            chkLstDataProviderStatus.Items[selectedIndex] = currentValue;
            chkLstDataProviderStatus.SelectedIndex = chkLstDataProviderStatus.SelectedIndex + 1;
        }

        private void cbDataProviderAssociation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbDataProviderAssociation.EditValue is FactorySettings setting && setting.UserSettingFileAssociations.Any())
            {
                txtbDataProviderAssociation.Text = string.Join(",", setting.UserSettingFileAssociations);
            }
            else
            {
                txtbDataProviderAssociation.Text = string.Empty;
            }
        }

        private void btnSetFileAssociation_Click(object sender, EventArgs e)
        {
            if (cbDataProviderAssociation.EditValue is FactorySettings setting)
            {
                setting.UserSettingFileAssociations = txtbDataProviderAssociation.Text
                    .Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
            }
        }

        private void rbtnHighlightContains_CheckedChanged(object sender, EventArgs e)
        {
            teHighlightContains.Enabled = rbtnHighlightContains.Checked;
            teHighlightEquals.Enabled = rbtnHighlightEquals.Checked;
        }

        private void rbtnHighlightEquals_CheckedChanged(object sender, EventArgs e)
        {
            teHighlightContains.Enabled = rbtnHighlightContains.Checked;
            teHighlightEquals.Enabled = rbtnHighlightEquals.Checked;
        }

        private void sbtnAddHighlight_Click(object sender, EventArgs e)
        {
            if (rbtnHighlightContains.Checked)
            {
                Settings.PreDefinedQueries.AddHighlight(teHighlightContains.Text, PreDefinedQueryType.Contains, cpeHighlightPreDefined.Color);
                lboxHighlightItems.DataSource = Settings.PreDefinedQueries.Highlights;
                lboxHighlightItems.Refresh();
            }

            if (rbtnHighlightEquals.Checked)
            {
                Settings.PreDefinedQueries.AddHighlight(teHighlightEquals.Text, PreDefinedQueryType.Equals, cpeHighlightPreDefined.Color);
                lboxHighlightItems.DataSource = Settings.PreDefinedQueries.Highlights;
                lboxHighlightItems.Refresh();
            }
        }

        private void sbtnDeleteHighlight_Click(object sender, EventArgs e)
        {
            if (lboxHighlightItems.SelectedItem is PreDefineHighlight highlight)
            {
                Settings.PreDefinedQueries.RemoveHighlight(highlight);
                lboxHighlightItems.DataSource = Settings.PreDefinedQueries.Highlights;
                lboxHighlightItems.Refresh();
            }
        }

        private void sbtnAddFilter_Click(object sender, EventArgs e)
        {
            Settings.PreDefinedQueries.AddFilter(txtbIncludeTextFilter.Text, txtbExcludeFilter.Text, txtbSourcesFilter.Text, txtbModulesFilter.Text);
            lboxFilters.DataSource = Settings.PreDefinedQueries.Filters;
            lboxFilters.Refresh();
        }

        private void sbtnDeleteFilter_Click(object sender, EventArgs e)
        {
            if (lboxFilters.SelectedItem is PreDefineFilter filter)
            {
                Settings.PreDefinedQueries.RemoveFilter(filter);
                lboxFilters.DataSource = Settings.PreDefinedQueries.Filters;
                lboxFilters.Refresh();
            }
        }

        private void sbtnAddAlerts_Click(object sender, EventArgs e)
        {
            Settings.PreDefinedQueries.AddAlert(txtbIncludeTextAlert.Text, txtbExcludeAlert.Text, txtbSourcesAlert.Text, txtbModulesAlert.Text);
            lboxAlerts.DataSource = Settings.PreDefinedQueries.Alerts;
            lboxAlerts.Refresh();
        }

        private void btnDeleteAlerts_Click(object sender, EventArgs e)
        {
            if (lboxAlerts.SelectedItem is PreDefineAlert alert)
            {
                Settings.PreDefinedQueries.RemoveAlert(alert);
                lboxAlerts.DataSource = Settings.PreDefinedQueries.Alerts;
                lboxAlerts.Refresh();
            }
        }

        private void btnFolderProbingBrowse_Click(object sender, EventArgs e)
        {
#if NETCOREAPP3_1
            using (FolderBrowserDialog folderDlg = new FolderBrowserDialog
            {
                ShowNewFolderButton = false
            })
            {
                // Show the FolderBrowserDialog.  
                DialogResult result = folderDlg.ShowDialog();
                if (result == DialogResult.OK)
                {
                    teFoldersProbing.Text = folderDlg.SelectedPath;
                }
            }
#else
            using (var dialog = new Microsoft.WindowsAPICodePack.Dialogs.CommonOpenFileDialog())
            {
                dialog.IsFolderPicker = true;
                if (dialog.ShowDialog() == Microsoft.WindowsAPICodePack.Dialogs.CommonFileDialogResult.Ok)
                {
                    teFoldersProbing.Text = dialog.FileName;
                }
            }
#endif
        }

        private void btnFolderProbingAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(teFoldersProbing.Text))
            {
                return;
            }

            listBoxFoldersProbing.Items.Add(teFoldersProbing.Text);
        }

        private void btnDeleteFolderProbing_Click(object sender, EventArgs e)
        {
            if (listBoxFoldersProbing.SelectedItem != null)
            {
                listBoxFoldersProbing.Items.Remove(listBoxFoldersProbing.SelectedItem);
            }
        }

        private void rbtnDarkIconColor_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnDarkIconColor.Checked)
            {
                peAnalogy.Image = Resources.AnalogyDark;
            }
        }

        private void rbtnLightIconColor_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnLightIconColor.Checked)
            {
                peAnalogy.Image = Resources.AnalogyLight;
            }
        }

        private void btnHeaderSet_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(teHeader.Text) && teHeader.Tag is DevExpress.XtraGrid.Columns.GridColumn column)
            {
                column.Caption = teHeader.Text;
                SaveGridLayout();
            }
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

        private void btnDateTimeFormat_Click(object sender, EventArgs e)
        {

            logGrid.Columns["Date"].DisplayFormat.FormatType = FormatType.DateTime;
            logGrid.Columns["Date"].DisplayFormat.FormatString = teDateTimeFormat.Text;
            Settings.DateTimePattern = teDateTimeFormat.Text;
        }

        private void ceNewMessagesColor_CheckedChanged(object sender, EventArgs e)
        {
            cpeNewMessagesColor.Enabled = ceNewMessagesColor.Checked;

        }

        private void RowColors_ColorChanged(object sender, EventArgs e)
        {
            tsEnableColors.IsOn = true;
            UpdateColors();
        }

        private void UpdateColors()
        {

            lblLogLevelUnknown.BackColor = cpeLogLevelUnknown.Color;
            lblLogLevelDisabled.BackColor = cpeLogLevelDisabled.Color;
            lblLogLevelTrace.BackColor = cpeLogLevelTrace.Color;
            lblLogLevelVerbose.BackColor = cpeLogLevelVerbose.Color;
            lblLogLevelDebug.BackColor = cpeLogLevelDebug.Color;
            lblLogLevelEvent.BackColor = cpeLogLevelEvent.Color;
            lblLogLevelWarning.BackColor = cpeLogLevelWarning.Color;
            lblLogLevelError.BackColor = cpeLogLevelError.Color;
            lblLogLevelCritical.BackColor = cpeLogLevelCritical.Color;
            lblLogLevelAnalogyInformation.BackColor = cpeLogLevelAnalogyInformation.Color;
            lblHighlightColor.BackColor = cpeHighlightColor.Color;
            ceNewMessagesColor.BackColor = cpeNewMessagesColor.Color;
            lblLogLevelUnknown.ForeColor = cpeLogLevelUnknownText.Color;
            lblLogLevelDisabled.ForeColor = cpeLogLevelDisabledText.Color;
            lblLogLevelTrace.ForeColor = cpeLogLevelTraceText.Color;
            lblLogLevelVerbose.ForeColor = cpeLogLevelVerboseText.Color;
            lblLogLevelDebug.ForeColor = cpeLogLevelDebugText.Color;
            lblLogLevelEvent.ForeColor = cpeLogLevelEventText.Color;
            lblLogLevelWarning.ForeColor = cpeLogLevelWarningText.Color;
            lblLogLevelError.ForeColor = cpeLogLevelErrorText.Color;
            lblLogLevelCritical.ForeColor = cpeLogLevelCriticalText.Color;
            lblLogLevelAnalogyInformation.ForeColor = cpeLogLevelAnalogyInformationText.Color;
            lblHighlightColor.ForeColor = cpeHighlightColorText.Color;
            ceNewMessagesColor.ForeColor = cpeNewMessagesColorText.Color;
        }
    }
}

