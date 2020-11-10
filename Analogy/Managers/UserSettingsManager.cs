using Analogy.CommonUtilities.Web;
using Analogy.DataTypes;
using Analogy.Interfaces;
using Analogy.Interfaces.Factories;
using Analogy.Managers;
using Analogy.Properties;
using DevExpress.LookAndFeel;
using DevExpress.XtraBars.Ribbon;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Analogy
{
    public class UserSettingsManager
    {
        public event EventHandler OnFactoryOrderChanged;


        private static readonly Lazy<UserSettingsManager> _instance =
            new Lazy<UserSettingsManager>(() => new UserSettingsManager());

        private CommandLayout _ribbonStyle;
        private bool _enableFirstChanceException;
        public event EventHandler<bool> OnEnableFirstChanceExceptionChanged;
        public event EventHandler<CommandLayout> OnRibbonControlStyleChanged;

        public string ApplicationSkinName { get; set; }
        public static UserSettingsManager UserSettings { get; set; } = _instance.Value;
        public bool SaveSearchFilters { get; set; }
        public string IncludeText { get; set; }
        public string ExcludeText { get; set; }

        public string SourceText { get; set; }
        public string ModuleText { get; set; }
        public List<(Guid ID, string FileName)> RecentFiles { get; set; }
        public List<(Guid ID, string Path)> RecentFolders { get; set; }
        public bool ShowHistoryOfClearedMessages { get; set; }
        public int RecentFilesCount { get; set; }
        public int RecentFoldersCount { get; set; }
        public bool EnableUserStatistics { get; set; }
        public TimeSpan AnalogyRunningTime { get; set; }
        public uint AnalogyLaunches { get; set; }
        public uint AnalogyOpenedFiles { get; set; }
        public bool EnableFileCaching { get; set; }
        public string DisplayRunningTime => $"{AnalogyRunningTime:dd\\.hh\\:mm\\:ss} days";
        public bool LoadExtensionsOnStartup { get; set; }
        public List<Guid> StartupExtensions { get; set; }
        public bool StartupRibbonMinimized { get; set; }
        public bool StartupErrorLogLevel { get; set; }
        public bool PagingEnabled { get; set; }
        public int PagingSize { get; set; }
        public bool ShowChangeLogAtStartUp { get; set; }
        public bool SearchAlsoInSourceAndModule { get; set; }
        public string InitialSelectedDataProvider { get; set; } = "D3047F5D-CFEB-4A69-8F10-AE5F4D3F2D04";
        public bool IdleMode { get; set; }
        public int IdleTimeMinutes { get; set; }

        public List<string> EventLogs { get; set; }

        public List<Guid> AutoStartDataProviders { get; set; }
        public bool AutoScrollToLastMessage { get; set; }
        public bool DefaultDescendOrder { get; set; }
        //public LogParserSettingsContainer LogParsersSettings { get; set; }
        public ColorSettings ColorSettings { get; set; }
        public List<Guid> FactoriesOrder { get; set; }
        public List<FactorySettings> FactoriesSettings { get; set; }
        public Guid LastOpenedDataProvider { get; set; }
        public bool RememberLastOpenedDataProvider { get; set; }
        public bool RememberLastSearches { get; set; }
        public PreDefinedQueries PreDefinedQueries { get; set; }
        public int NumberOfLastSearches { get; set; }
        public List<string> LastSearchesInclude { get; set; }
        public List<string> LastSearchesExclude { get; set; }
        public int AnalogyInternalLogPeriod { get; set; }
        public List<string> AdditionalProbingLocations { get; set; }
        public bool SingleInstance { get; set; }
        public string AnalogyIcon { get; set; }
        public string LogGridFileName => Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty, "AnalogyGridlayout.xml");
        public string DateTimePattern { get; set; }
        public UpdateMode UpdateMode { get; set; }
        public DateTime LastUpdate { get; set; }
        public GithubObjects.GithubReleaseEntry? LastVersionChecked { get; set; }
        public string GitHubToken { get; } = Environment.GetEnvironmentVariable("AnalogyGitHub_Token") ?? string.Empty;
        public bool MinimizedToTrayBar { get; set; }
        public bool CheckAdditionalInformation { get; set; }

        public AnalogyPositionState AnalogyPosition { get; set; }
        public bool EnableCompressedArchives { get; set; }
        public bool IsBuiltInSearchPanelVisible { get; set; }
        public BuiltInSearchPanelMode BuiltInSearchPanelMode { get; set; }
        public string ApplicationSvgPaletteName { get; set; }
        public LookAndFeelStyle ApplicationStyle { get; set; } = LookAndFeelStyle.Skin;
        public bool ShowMessageDetails { get; set; }
        public bool SimpleMode { get; set; }
        public bool IsFirstRun { get; set; }
        public LogLevelSelectionType LogLevelSelection { get; set; }
        public bool ShowWhatIsNewAtStartup { get; set; }
        public FontSettings FontSettings { get; set; }

        public bool EnableFirstChanceException
        {
            get => _enableFirstChanceException;
            set
            {
                if (_enableFirstChanceException != value)
                {
                    _enableFirstChanceException = value;
                    OnEnableFirstChanceExceptionChanged?.Invoke(this, value);
                }
            }
        }

        public List<DataProviderInformation> SupportedDataProviders { get; set; }
        public CommandLayout RibbonStyle
        {
            get => _ribbonStyle;
            set
            {
                if (_ribbonStyle != value)
                {
                    _ribbonStyle = value;
                    OnRibbonControlStyleChanged?.Invoke(this, value);

                }
            }
        }

        public UserSettingsManager()
        {
            Load();
        }

        public void Load()
        {
            SupportedDataProviders = new List<DataProviderInformation>();
            SupportedDataProviders.Add(new DataProviderInformation("Analogy.LogViewer.Serilog", "Analogy.LogViewer.Serilog.dll", "https://api.github.com/repos/Analogy-LogViewer/Analogy.LogViewer.Serilog"));
            SupportedDataProviders.Add(new DataProviderInformation("Analogy.LogViewer.RabbitMq", "Analogy.LogViewer.RabbitMq.dll", "https://api.github.com/repos/Analogy-LogViewer/Analogy.LogViewer.RabbitMq"));
            SupportedDataProviders.Add(new DataProviderInformation("Analogy.LogViewer.RSSReader", "Analogy.LogViewer.RSSReader.dll", "https://api.github.com/repos/Analogy-LogViewer/Analogy.LogViewer.RSSReader"));
            SupportedDataProviders.Add(new DataProviderInformation("Analogy.LogViewer.VisualStudioLogParser", "Analogy.LogViewer.VisualStudioLogParser.dll", "https://api.github.com/repos/Analogy-LogViewer/Analogy.LogViewer.VisualStudioLogParser"));
            SupportedDataProviders.Add(new DataProviderInformation("Analogy.LogViewer.WhatsApp", "Analogy.LogViewer.WhatsApp.dll", "https://api.github.com/repos/Analogy-LogViewer/Analogy.LogViewer.WhatsApp"));
            SupportedDataProviders.Add(new DataProviderInformation("Analogy.LogViewer.XMLParser", "Analogy.LogViewer.XMLParser.dll", "https://api.github.com/repos/Analogy-LogViewer/Analogy.LogViewer.XMLParser"));
            SupportedDataProviders.Add(new DataProviderInformation("Analogy.LogViewer.WCF", "Analogy.LogViewer.WCF.dll", "https://api.github.com/repos/Analogy-LogViewer/Analogy.LogViewer.WCF"));
            SupportedDataProviders.Add(new DataProviderInformation("Analogy.LogViewer.Philips.ICAP", "Analogy.LogViewer.Philips.ICAP.dll", "https://api.github.com/repos/Analogy-LogViewer/Analogy.LogViewer.Philips.ICAP"));
            SupportedDataProviders.Add(new DataProviderInformation("Analogy.LogViewer.Philips.CT", "Analogy.LogViewer.Philips.CT.dll", "https://api.github.com/repos/Analogy-LogViewer/Analogy.LogViewer.Philips.CT"));
            SupportedDataProviders.Add(new DataProviderInformation("Analogy.LogViewer.JsonParser", "Analogy.LogViewer.JsonParser.dll", "https://api.github.com/repos/Analogy-LogViewer/Analogy.LogViewer.JsonParser"));
            SupportedDataProviders.Add(new DataProviderInformation("Analogy.LogViewer.Log4jXml", "Analogy.LogViewer.Log4jXml.dll", "https://api.github.com/repos/Analogy-LogViewer/Analogy.LogViewer.Log4jXml"));
            SupportedDataProviders.Add(new DataProviderInformation("Analogy.LogViewer.KafkaProvider ", "Analogy.LogViewer.KafkaProvider .dll", "https://api.github.com/repos/Analogy-LogViewer/Analogy.LogViewer.KafkaProvider"));
            SupportedDataProviders.Add(new DataProviderInformation("Analogy.LogViewer.IISLogsProvider", "Analogy.LogViewer.IISLogsProvider.dll", "https://api.github.com/repos/Analogy-LogViewer/Analogy.LogViewer.IISLogsProvider"));
            SupportedDataProviders.Add(new DataProviderInformation("Analogy.LogViewer.Github", "Analogy.LogViewer.Github.dll", "https://api.github.com/repos/Analogy-LogViewer/Analogy.LogViewer.Github"));
            SupportedDataProviders.Add(new DataProviderInformation("Analogy.LogViewer.GitHistory", "Analogy.LogViewer.GitHistory.dll", "https://api.github.com/repos/Analogy-LogViewer/Analogy.LogViewer.GitHistory"));
            SupportedDataProviders.Add(new DataProviderInformation("Analogy.LogViewer.Affirmations", "Analogy.LogViewer.Affirmations.dll", "https://api.github.com/repos/Analogy-LogViewer/Analogy.LogViewer.Affirmations"));
            SupportedDataProviders.Add(new DataProviderInformation("Analogy.LogViewer.Log4Net", "Analogy.LogViewer.Log4Net.dll", "https://api.github.com/repos/Analogy-LogViewer/Analogy.LogViewer.Log4Net"));
            SupportedDataProviders.Add(new DataProviderInformation("Analogy.LogViewer.Nlog", "Analogy.LogViewer.NLogProvider.dll", "https://api.github.com/repos/Analogy-LogViewer/Analogy.LogViewer.Nlog"));
            SupportedDataProviders.Add(new DataProviderInformation("Analogy.LogViewer.PlainTextParser", "Analogy.LogViewer.PlainTextParser.dll", "https://api.github.com/repos/Analogy-LogViewer/Analogy.LogViewer.PlainTextParser"));
            SupportedDataProviders.Add(new DataProviderInformation("Analogy.LogViewer.PowerToys", "Analogy.LogViewer.PowerToys.dll", "https://api.github.com/repos/Analogy-LogViewer/Analogy.LogViewer.PowerToys"));
            SupportedDataProviders.Add(new DataProviderInformation("Analogy.LogViewer.RegexParser", "Analogy.LogViewer.RegexParser.dll", "https://api.github.com/repos/Analogy-LogViewer/Analogy.LogViewer.RegexParser"));
            SupportedDataProviders.Add(new DataProviderInformation("Analogy.LogViewer.gRPC", "Analogy.LogViewer.gRPC.dll", "https://api.github.com/repos/Analogy-LogViewer/Analogy.LogViewer.gRPC"));
            SupportedDataProviders.Add(new DataProviderInformation("Analogy.LogViewer.WindowsEventLogs", "Analogy.LogViewer.WindowsEventLogs.dll", "https://api.github.com/repos/Analogy-LogViewer/Analogy.LogViewer.WindowsEventLogs"));
            SupportedDataProviders.Add(new DataProviderInformation("Analogy.LogViewer.Example", "Analogy.LogViewer.Example.dll", "https://api.github.com/repos/Analogy-LogViewer/Analogy.LogViewer.Example"));


            EnableCompressedArchives = true;
            AnalogyInternalLogPeriod = 5;
            if (Settings.Default.UpgradeRequired)
            {
                Settings.Default.Upgrade();
                Settings.Default.UpgradeRequired = false;
                Settings.Default.Save();
                ShowWhatIsNewAtStartup = true;
            }

            DateTimePattern = !string.IsNullOrEmpty(Settings.Default.DateTimePattern)
                ? Settings.Default.DateTimePattern
                : "yyyy.MM.dd HH:mm:ss.ff";
            IsFirstRun = Settings.Default.FirstRun;
            AnalogyIcon = Settings.Default.AnalogyIcon;
            ApplicationSkinName = Settings.Default.ApplicationSkinName;
            EnableUserStatistics = Settings.Default.EnableUserStatistics;
            AnalogyRunningTime = Settings.Default.AnalogyRunningTime;
            AnalogyLaunches = Settings.Default.AnalogyLaunchesCount;
            AnalogyOpenedFiles = Settings.Default.OpenFilesCount;
            ExcludeText = Settings.Default.ExcludeText;
            IncludeText = Settings.Default.IncludeText;
            SourceText = Settings.Default.SourceText;
            ModuleText = Settings.Default.ModuleText;
            ShowHistoryOfClearedMessages = Settings.Default.ShowHistoryClearedMessages;
            SaveSearchFilters = Settings.Default.SaveSearchFilters;
            RecentFiles = ParseSettings<List<(Guid ID, string FileName)>>(Settings.Default.RecentFiles);
            RecentFilesCount = Settings.Default.RecentFilesCount;
            RecentFoldersCount = Settings.Default.RecentFoldersCount;
            RecentFolders = ParseSettings<List<(Guid ID, string Path)>>(Settings.Default.RecentFolders);
            EnableFileCaching = Settings.Default.EnableFileCaching;
            LoadExtensionsOnStartup = Settings.Default.LoadExtensionsOnStartup;
            StartupExtensions = ParseSettings<List<Guid>>(Settings.Default.StartupExtensions);
            StartupRibbonMinimized = Settings.Default.StartupRibbonMinimized;
            StartupErrorLogLevel = Settings.Default.StartupErrorLogLevel;
            PagingEnabled = Settings.Default.PagingEnabled;
            PagingSize = Settings.Default.PagingSize;
            ShowChangeLogAtStartUp = Settings.Default.ShowChangeLogAtStartUp;
            IncludeText = Settings.Default.IncludeText;
            SearchAlsoInSourceAndModule = Settings.Default.SearchAlsoInSourceAndModule;
            IdleMode = Settings.Default.IdleMode;
            IdleTimeMinutes = Settings.Default.IdleTimeMinutes;
            EventLogs = ParseSettings<List<string>>(Settings.Default.WindowsEventLogs);
            AutoStartDataProviders = ParseSettings<List<Guid>>(Settings.Default.AutoStartDataProviders);
            AutoScrollToLastMessage = Settings.Default.AutoScrollToLastMessage;
            ColorSettings = ParseSettings<ColorSettings>(Settings.Default.ColorSettings);
            DefaultDescendOrder = Settings.Default.DefaultDescendOrder;
            FactoriesOrder = ParseSettings<List<Guid>>(Settings.Default.FactoriesOrder);
            FactoriesSettings = ParseSettings<List<FactorySettings>>(Settings.Default.FactoriesSettings)
                .Where(f => f.FactoryId != Guid.Empty).ToList();
            LastOpenedDataProvider = Settings.Default.LastOpenedDataProvider;
            PreDefinedQueries = ParseSettings<PreDefinedQueries>(Settings.Default.PreDefinedQueries);
            RememberLastOpenedDataProvider = Settings.Default.RememberLastOpenedDataProvider;
            RememberLastSearches = Settings.Default.RememberLastSearches;
            LastSearchesInclude = ParseSettings<List<string>>(Settings.Default.LastSearchesInclude);
            LastSearchesExclude = ParseSettings<List<string>>(Settings.Default.LastSearchesExclude);
            SimpleMode = Settings.Default.SimpleMode;
            NumberOfLastSearches = Settings.Default.NumberOfLastSearches;
            AdditionalProbingLocations = ParseSettings<List<string>>(Settings.Default.AdditionalProbingLocations);
            SingleInstance = Settings.Default.SingleInstance;
            LastUpdate = Settings.Default.LastUpdate;
            LastVersionChecked = ParseSettings<GithubObjects.GithubReleaseEntry>(Settings.Default.LastVersionChecked);
            switch (Settings.Default.UpdateMode)
            {
                case 0:
                    UpdateMode = UpdateMode.Never;
                    break;
                case 1:
                    UpdateMode = UpdateMode.EachStartup;
                    break;
                case 2:
                    UpdateMode = UpdateMode.OnceAWeek;
                    break;
                case 3:
                    UpdateMode = UpdateMode.OnceAMonth;
                    break;
            }

            if (Enum.TryParse(Settings.Default.ApplicationStyle, out LookAndFeelStyle style))
            {
                ApplicationStyle = style;
            }
            if (Enum.TryParse(Settings.Default.LogLevelSelection, out LogLevelSelectionType type))
            {
                LogLevelSelection = type;
            }
            ApplicationSvgPaletteName = Settings.Default.ApplicationSvgPaletteName;
            MinimizedToTrayBar = Settings.Default.MinimizedToTrayBar;
            CheckAdditionalInformation = Settings.Default.CheckAdditionalInformation;
            AnalogyPosition = ParseSettings<AnalogyPositionState>(Settings.Default.AnalogyPosition) ??
                              new AnalogyPositionState();
            EnableCompressedArchives = Settings.Default.EnableCompressedArchives;
            IsBuiltInSearchPanelVisible = Settings.Default.IsBuiltInSearchPanelVisible;
            if (Enum.TryParse(Settings.Default.BuiltInSearchPanelMode, out BuiltInSearchPanelMode result))
            {
                BuiltInSearchPanelMode = result;
            }

            ShowMessageDetails = Settings.Default.ShowMessageDetails;
            ShowWhatIsNewAtStartup = Settings.Default.ShowWhatIsNewAtStartup;
            FontSettings = ParseSettings<FontSettings>(Settings.Default.FontSettings);
            RibbonStyle = (CommandLayout)Settings.Default.RibbonStyle;
            EnableFirstChanceException = Settings.Default.EnableFirstChanceException;
        }

        private T ParseSettings<T>(string data) where T : new()
        {
            try
            {
                return string.IsNullOrEmpty(data) ?
                    new T() :
                    JsonConvert.DeserializeObject<T>(data);
            }
            catch (Exception e)
            {
                AnalogyLogManager.Instance.LogError("Error during parsing: " + e, nameof(UserSettingsManager));
                return new T();
            }


        }
        public void Save()
        {
            Settings.Default.FirstRun = false;
            Settings.Default.DateTimePattern = !string.IsNullOrEmpty(DateTimePattern)
                ? DateTimePattern
                : "yyyy.MM.dd HH:mm:ss.ff";
            Settings.Default.AnalogyIcon = AnalogyIcon;
            Settings.Default.ApplicationSkinName = ApplicationSkinName;
            Settings.Default.EnableUserStatistics = EnableUserStatistics;
            Settings.Default.AnalogyRunningTime = AnalogyRunningTime;
            Settings.Default.AnalogyLaunchesCount = AnalogyLaunches;
            Settings.Default.OpenFilesCount = AnalogyOpenedFiles;
            Settings.Default.ModuleText = ModuleText;
            Settings.Default.SourceText = SourceText;
            Settings.Default.ExcludeText = ExcludeText;
            Settings.Default.IncludeText = IncludeText;
            Settings.Default.ShowHistoryClearedMessages = ShowHistoryOfClearedMessages;
            Settings.Default.SaveSearchFilters = SaveSearchFilters;
            Settings.Default.RecentFilesCount = RecentFilesCount;
            Settings.Default.RecentFiles = JsonConvert.SerializeObject(RecentFiles.Take(RecentFilesCount).ToList());
            Settings.Default.RecentFoldersCount = RecentFoldersCount;
            Settings.Default.RecentFolders = JsonConvert.SerializeObject(RecentFolders.Take(RecentFoldersCount).ToList());
            Settings.Default.EnableFileCaching = EnableFileCaching;
            Settings.Default.LoadExtensionsOnStartup = LoadExtensionsOnStartup;
            Settings.Default.StartupExtensions = JsonConvert.SerializeObject(StartupExtensions);
            Settings.Default.StartupRibbonMinimized = StartupRibbonMinimized;
            Settings.Default.StartupErrorLogLevel = StartupErrorLogLevel;
            Settings.Default.PagingEnabled = PagingEnabled;
            Settings.Default.PagingSize = PagingSize;
            Settings.Default.ShowChangeLogAtStartUp = false;
            Settings.Default.IncludeText = IncludeText;
            Settings.Default.SearchAlsoInSourceAndModule = SearchAlsoInSourceAndModule;
            Settings.Default.IdleMode = IdleMode;
            Settings.Default.IdleTimeMinutes = IdleTimeMinutes;
            Settings.Default.WindowsEventLogs = JsonConvert.SerializeObject(EventLogs);
            Settings.Default.AutoStartDataProviders = JsonConvert.SerializeObject(AutoStartDataProviders);
            Settings.Default.AutoScrollToLastMessage = AutoScrollToLastMessage;
            Settings.Default.ColorSettings = JsonConvert.SerializeObject(ColorSettings);
            Settings.Default.DefaultDescendOrder = DefaultDescendOrder;
            Settings.Default.FactoriesOrder = JsonConvert.SerializeObject(FactoriesOrder);
            Settings.Default.FactoriesSettings = JsonConvert.SerializeObject(FactoriesSettings);
            Settings.Default.LastOpenedDataProvider = LastOpenedDataProvider;
            Settings.Default.RememberLastOpenedDataProvider = RememberLastOpenedDataProvider;
            Settings.Default.PreDefinedQueries = JsonConvert.SerializeObject(PreDefinedQueries);
            Settings.Default.RememberLastSearches = RememberLastSearches;
            Settings.Default.NumberOfLastSearches = NumberOfLastSearches;
            Settings.Default.LastSearchesInclude = JsonConvert.SerializeObject(LastSearchesInclude.Take(NumberOfLastSearches).ToList());
            Settings.Default.LastSearchesExclude = JsonConvert.SerializeObject(LastSearchesExclude.Take(NumberOfLastSearches).ToList());
            Settings.Default.AdditionalProbingLocations = JsonConvert.SerializeObject(AdditionalProbingLocations);
            Settings.Default.SingleInstance = SingleInstance;
            Settings.Default.LastUpdate = LastUpdate;
            Settings.Default.UpdateMode = (int)UpdateMode;
            Settings.Default.LastVersionChecked = JsonConvert.SerializeObject(LastVersionChecked);
            Settings.Default.MinimizedToTrayBar = MinimizedToTrayBar;
            Settings.Default.CheckAdditionalInformation = CheckAdditionalInformation;
            Settings.Default.AnalogyPosition = JsonConvert.SerializeObject(AnalogyPosition);
            Settings.Default.EnableCompressedArchives = EnableCompressedArchives;
            Settings.Default.IsBuiltInSearchPanelVisible = IsBuiltInSearchPanelVisible;
            Settings.Default.BuiltInSearchPanelMode = BuiltInSearchPanelMode.ToString();
            Settings.Default.ApplicationStyle = ApplicationStyle.ToString();
            Settings.Default.ApplicationSvgPaletteName = ApplicationSvgPaletteName;
            Settings.Default.ShowMessageDetails = ShowMessageDetails;
            Settings.Default.SimpleMode = SimpleMode;
            Settings.Default.LogLevelSelection = LogLevelSelection.ToString();
            Settings.Default.ShowWhatIsNewAtStartup = ShowWhatIsNewAtStartup;
            Settings.Default.FontSettings = JsonConvert.SerializeObject(FontSettings);
            Settings.Default.RibbonStyle = (int)RibbonStyle;
            Settings.Default.EnableFirstChanceException = EnableFirstChanceException;
            Settings.Default.Save();

        }

        public void AddToRecentFiles(Guid iD, string file)
        {
            AnalogyOpenedFiles += 1;
            if (!RecentFiles.Contains((iD, file)))
            {
                RecentFiles.Insert(0, (iD, file));
            }
        }
        public void AddToRecentFolders(Guid iD, string path)
        {
            if (!RecentFolders.Contains((iD, path)))
            {
                RecentFolders.Insert(0, (iD, path));
            }
        }
        public void ClearStatistics()
        {
            AnalogyRunningTime = TimeSpan.FromSeconds(0);
            AnalogyLaunches = 0;
            AnalogyOpenedFiles = 0;
        }

        public void UpdateRunningTime() => AnalogyRunningTime =
            AnalogyRunningTime.Add(DateTime.Now.Subtract(Process.GetCurrentProcess().StartTime));

        public void IncreaseNumberOfLaunches() => AnalogyLaunches++;

        public FactorySettings GetFactorySetting(Guid factoryID)
        {
            bool Exists(Guid guid) => guid == factoryID;
            if (FactoriesSettings.Exists(f => Exists(f.FactoryId)))
            {
                return FactoriesSettings.Single(f => Exists(f.FactoryId));
            }

            return null;
        }
        public FactorySettings GetOrAddFactorySetting(IAnalogyFactory factory)
        {
            bool Exists(Guid guid) => guid == factory.FactoryId;
            if (FactoriesSettings.Exists(f => Exists(f.FactoryId)))
            {
                return FactoriesSettings.Single(f => Exists(f.FactoryId));
            }

            var createNew = new FactorySettings
            {
                FactoryId = factory.FactoryId,
                Status = DataProviderFactoryStatus.NotSet,
                UserSettingFileAssociations = new List<string>()
            };
            FactoriesSettings.Add(createNew);
            return createNew;

        }

        public void UpdateOrder(List<Guid> order)
        {
            if (FactoriesOrder.SequenceEqual(order))
            {
                return;
            }

            FactoriesOrder = order;
            OnFactoryOrderChanged?.Invoke(this, new EventArgs());
        }

        public IEnumerable<FactorySettings> GetFactoriesThatHasFileAssociation(string[] files) =>
            FactoriesSettings.Where(factory => factory.Status != DataProviderFactoryStatus.Disabled &&
                                               factory.UserSettingFileAssociations != null &&
                                               factory.UserSettingFileAssociations.Any(i =>
                                                   Utils.MatchedAll(i, files)));
        public bool AddNewSearchesEntryToLists(string text, bool include)
        {
            if (include)
            {
                if (LastSearchesInclude.Contains(text, StringComparison.InvariantCultureIgnoreCase))
                {
                    return false;
                }

                LastSearchesInclude.Add(text);
                return true;
            }
            else
            {
                if (LastSearchesExclude.Contains(text, StringComparison.InvariantCultureIgnoreCase))
                {
                    return false;
                }

                LastSearchesExclude.Add(text);
                return true;
            }
        }

        public Icon GetIcon()
        {
            return AnalogyIcon == "Dark" ? Resources.AnalogyIconDark : Resources.AnalogyIconLight;
        }
        public Image GetImage()
        {
            return AnalogyIcon == "Dark" ? Resources.AnalogyDark : Resources.AnalogyLight;
        }

        public IEnumerable<(Guid ID, string FileName)> GetRecentFiles(Guid offlineAnalogyId) =>
            RecentFiles.Where(itm => itm.ID == offlineAnalogyId);

        public IEnumerable<(Guid ID, string Path)> GetRecentFolders(Guid offlineAnalogyId) =>
            RecentFolders.Where(itm => itm.ID == offlineAnalogyId);

    }

    [Serializable]
    public class ColorSettings
    {
        public bool EnableMessagesColors { get; set; }
        public Dictionary<AnalogyLogLevel, (Color BackgroundColor, Color TextColor)> LogLevelColors { get; set; }

        public (Color BackgroundColor, Color TextColor) HighlightColor { get; set; }
        public (Color BackgroundColor, Color TextColor) NewMessagesColor { get; set; }
        public bool EnableNewMessagesColor { get; set; }
        public bool OverrideLogLevelColor { get; set; }

        public ColorSettings()
        {
            EnableMessagesColors = true;
            HighlightColor = (Color.Aqua, Color.Black);
            NewMessagesColor = (Color.PaleTurquoise, Color.Black);
            var logLevelValues = Enum.GetValues(typeof(AnalogyLogLevel));
            LogLevelColors = new Dictionary<AnalogyLogLevel, (Color BackgroundColor, Color TextColor)>(logLevelValues.Length);

            foreach (AnalogyLogLevel level in logLevelValues)

            {
                switch (level)
                {
                    case AnalogyLogLevel.Unknown:
                        LogLevelColors.Add(level, (Color.White, Color.Black));
                        break;
                    case AnalogyLogLevel.None:
                        LogLevelColors.Add(level, (Color.LightGray, Color.Black));
                        break;
                    case AnalogyLogLevel.Trace:
                        LogLevelColors.Add(level, (Color.White, Color.Black));
                        break;
                    case AnalogyLogLevel.Verbose:
                        LogLevelColors.Add(level, (Color.White, Color.Black));
                        break;
                    case AnalogyLogLevel.Debug:
                        LogLevelColors.Add(level, (Color.White, Color.Black));
                        break;
                    case AnalogyLogLevel.Information:
                        LogLevelColors.Add(level, (Color.White, Color.Black));
                        break;
                    case AnalogyLogLevel.Warning:
                        LogLevelColors.Add(level, (Color.Yellow, Color.Black));
                        break;
                    case AnalogyLogLevel.Error:
                        LogLevelColors.Add(level, (Color.Pink, Color.Black));
                        break;
                    case AnalogyLogLevel.Critical:
                        LogLevelColors.Add(level, (Color.Red, Color.Black));
                        break;
                    case AnalogyLogLevel.Analogy:
                        LogLevelColors.Add(level, (Color.White, Color.Black));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public (Color BackgroundColor, Color TextColor) GetColorForLogLevel(AnalogyLogLevel level) => LogLevelColors[level];

        public (Color BackgroundColor, Color TextColor) GetHighlightColor() => HighlightColor;
        public (Color BackgroundColor, Color TextColor) GetNewMessagesColor() => NewMessagesColor;

        public void SetColorForLogLevel(AnalogyLogLevel level, Color backgroundColor, Color textColor) => LogLevelColors[level] = (backgroundColor, textColor);
        public void SetHighlightColor(Color backgroundColor, Color textColor) => HighlightColor = (backgroundColor, textColor);
        public void SetNewMessagesColor(Color backgroundColor, Color textColor) => NewMessagesColor = (backgroundColor, textColor);
        public string AsJson() => JsonConvert.SerializeObject(this);
        public static ColorSettings FromJson(string fileName) => JsonConvert.DeserializeObject<ColorSettings>(fileName);
    }

    [Serializable]
    public class FactorySettings
    {
        public string FactoryName { get; set; }
        public Guid FactoryId { get; set; }
        public List<string> UserSettingFileAssociations { get; set; }
        public DataProviderFactoryStatus Status { get; set; }

        public FactorySettings()
        {
            UserSettingFileAssociations = new List<string>();
        }
        public override string ToString() => $"{nameof(FactoryName)}: {FactoryName}, {nameof(FactoryId)}: {FactoryId}, {nameof(Status)}: {Status}";

    }

    public enum PreDefinedQueryType
    {
        Contains,
        Equals
    }
    [Serializable]
    public class PreDefinedQueries
    {
        public List<PreDefineHighlight> Highlights { get; set; }

        public List<PreDefineFilter> Filters { get; set; }

        public List<PreDefineAlert> Alerts { get; set; }

        public PreDefinedQueries()
        {
            Highlights = new List<PreDefineHighlight>();
            Filters = new List<PreDefineFilter>();
            Alerts = new List<PreDefineAlert>();
        }

        public void AddHighlight(string text, PreDefinedQueryType type, Color color) => Highlights.Add(new PreDefineHighlight(type, text, color));
        public void AddFilter(string includeText, string excludeText, string sources, string modules) => Filters.Add(new PreDefineFilter(includeText, excludeText, sources, modules));
        public void AddAlert(string includeText, string excludeText, string sources, string modules) => Alerts.Add(new PreDefineAlert(includeText, excludeText, sources, modules));

        public void RemoveHighlight(PreDefineHighlight highlight)
        {
            if (Highlights.Contains(highlight))
            {
                Highlights.Remove(highlight);
            }
        }

        public void RemoveFilter(PreDefineFilter filter)
        {
            if (Filters.Contains(filter))
            {
                Filters.Remove(filter);
            }
        }
        public void RemoveAlert(PreDefineAlert alert)
        {
            if (Alerts.Contains(alert))
            {
                Alerts.Remove(alert);
            }
        }
    }
    [Serializable]
    public class PreDefineHighlight
    {
        public PreDefinedQueryType PreDefinedQueryType { get; set; }
        public string Text { get; set; }
        public Color Color { get; set; }

        public PreDefineHighlight(PreDefinedQueryType preDefinedQueryType, string text, Color color)
        {
            PreDefinedQueryType = preDefinedQueryType;
            Text = text ?? string.Empty;
            Color = color;
        }
        public override string ToString()
        {
            return $"Highlight: {Text}. Type:{PreDefinedQueryType}. Color:{Color}";
        }
    }

    [Serializable]
    public class PreDefineFilter
    {
        public string IncludeText { get; }
        public string ExcludeText { get; }
        public string Sources { get; }
        public string Modules { get; }


        public PreDefineFilter(string includeText, string excludeText, string sources, string modules)
        {
            IncludeText = includeText ?? string.Empty;
            ExcludeText = excludeText ?? string.Empty;
            Sources = sources ?? string.Empty;
            Modules = modules ?? string.Empty;
        }
        public override string ToString()
        {
            return $"Filter: Message Text:{IncludeText}. Exclude:{ExcludeText}. Sources:{Sources}. Modules:{Modules}";
        }
    }

    [Serializable]
    public class PreDefineAlert
    {
        public string IncludeText { get; }
        public string ExcludeText { get; }
        public string Sources { get; }
        public string Modules { get; }


        public PreDefineAlert(string includeText, string excludeText, string sources, string modules)
        {
            IncludeText = includeText ?? string.Empty;
            ExcludeText = excludeText ?? string.Empty;
            Sources = sources ?? string.Empty;
            Modules = modules ?? string.Empty;
        }
        public override string ToString()
        {
            return $"Alert: Message Text:{IncludeText}. Exclude:{ExcludeText}. Sources:{Sources}. Modules:{Modules}";
        }
    }
}

