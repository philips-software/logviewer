﻿using Analogy.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Analogy
{
    public static class CommonChangeLog
    {
        public static IEnumerable<AnalogyChangeLog> GetChangeLog()
        {
            return new List<AnalogyChangeLog>
            {
                new AnalogyChangeLog("V4.2.14 - [Updater] show download progress #619",AnalogChangeLogType.Improvement,"Lior Banai",new DateTime(2020,10,30)),
                new AnalogyChangeLog("V4.2.13 - App hangs when trying to load file in use #636",AnalogChangeLogType.Bug,"Lior Banai",new DateTime(2020,10,27)),
                new AnalogyChangeLog("V4.2.12 - Add support for loading data providers from sub folders #645",AnalogChangeLogType.Improvement,"Lior Banai",new DateTime(2020,10,27)),
                new AnalogyChangeLog("V4.2.12 - [Feature] Add data provider update mechanism #596",AnalogChangeLogType.Feature,"Lior Banai",new DateTime(2020,10,26)),
                new AnalogyChangeLog("V4.2.12 - [Feature] Add Update to Analogy #595",AnalogChangeLogType.Feature,"Lior Banai",new DateTime(2020,10,26)),
                new AnalogyChangeLog("V4.2.12 - [Feature] Add Data providers download/install page #202",AnalogChangeLogType.Feature,"Lior Banai",new DateTime(2020,10,26)),
                new AnalogyChangeLog("V4.2.11 - 4.2.10 UnauthorizedAccessException when closing the app #568",AnalogChangeLogType.Bug,"Lior Banai",new DateTime(2020,10,19)),
                new AnalogyChangeLog("V4.2.10 - [Debugging] Add toggle for FirstChanceException #567",AnalogChangeLogType.Improvement,"Lior Banai",new DateTime(2020,10,17)),
                new AnalogyChangeLog("V4.2.10 - Remove focus on mouse over for filtering text boxes #566",AnalogChangeLogType.Improvement,"Lior Banai",new DateTime(2020,10,17)),
                new AnalogyChangeLog("V4.2.10 - [UI] Add Open folder dialog button #527",AnalogChangeLogType.Improvement,"Lior Banai",new DateTime(2020,10,10)),
                new AnalogyChangeLog("V4.2.10 - Enable Nullable annotations #559",AnalogChangeLogType.Improvement,"Lior Banai",new DateTime(2020,10,10)),
                new AnalogyChangeLog("V4.2.10 - The OK button on the 'Data Source Information' dialog doesn't seem to do anything. #546",AnalogChangeLogType.Bug,"Lior Banai",new DateTime(2020,10,09)),
                new AnalogyChangeLog("V4.2.10 - [UI] Columns header context menu is hidden being the standard menu #545",AnalogChangeLogType.Bug,"Lior Banai",new DateTime(2020,10,08)),
                new AnalogyChangeLog("V4.2.10 - [Extendability] Allows shuffling of provider between factories #534",AnalogChangeLogType.Improvement,"Lior Banai",new DateTime(2020,10,06)),
                new AnalogyChangeLog("V4.2.10 - [UI] add setting to toggle compact ribbon view #520",AnalogChangeLogType.Improvement,"Lior Banai",new DateTime(2020,10,02)),
                new AnalogyChangeLog("V4.2.9 - [UI] Analogy Save message still says XML format while it has been deprecated and replaced by Json #514",AnalogChangeLogType.Bug,"Lior Banai",new DateTime(2020,09,24)),
                new AnalogyChangeLog("V4.2.9 - [Usability] Add global font size settings for high resolution monitors #501",AnalogChangeLogType.Improvement,"Lior Banai",new DateTime(2020,09,20)),
                new AnalogyChangeLog("V4.2.9 - [feature] Add View as json tree on right click on log message #504",AnalogChangeLogType.Improvement,"Lior Banai",new DateTime(2020,09,19)),
                new AnalogyChangeLog("V4.2.8 - [Context menu] limit number of characters showed in the messages context menu. #505",AnalogChangeLogType.Improvement,"Lior Banai",new DateTime(2020,09,16)),
                new AnalogyChangeLog("V4.2.8 - Add Global tool interface - UI part #279",AnalogChangeLogType.Improvement,"Lior Banai",new DateTime(2020,09,12)),
                new AnalogyChangeLog("V4.2.8 - Add Icon/Image to Actions Commands #366",AnalogChangeLogType.Improvement,"Lior Banai",new DateTime(2020,09,12)),
                new AnalogyChangeLog("V4.2.8 - Create data provider selection for first time run #359",AnalogChangeLogType.Improvement,"Lior Banai",new DateTime(2020,09,11)),
                new AnalogyChangeLog("V4.2.8 - Replace Event log level with Information #477",AnalogChangeLogType.Improvement,"Lior Banai",new DateTime(2020,09,11)),
                new AnalogyChangeLog("V4.2.8 - Switch to json log format for bookmarks and internal log #476",AnalogChangeLogType.Improvement,"Lior Banai",new DateTime(2020,09,11)),
                new AnalogyChangeLog("V4.2.8 - [Filtering] Message layout does not affect view and keeps resetting #417",AnalogChangeLogType.Bug,"Lior Banai",new DateTime(2020,09,05)),
                new AnalogyChangeLog("V4.2.8 - Add Show what is new at start of application #420",AnalogChangeLogType.Improvement,"Lior Banai",new DateTime(2020,09,05)),
                new AnalogyChangeLog("V4.2.8 - Add option to select multi log levels (checkboxes instead of radio buttons) #415",AnalogChangeLogType.Improvement,"Lior Banai",new DateTime(2020,09,05)),
                new AnalogyChangeLog("V4.2.8 - [Debugging] handle Could not load file or assembly 'xxxx.resources.dll' #418",AnalogChangeLogType.Improvement,"Lior Banai",new DateTime(2020,09,05)),
                new AnalogyChangeLog("V4.2.7 - Create tutorial for first time run #358",AnalogChangeLogType.Improvement,"Lior Banai",new DateTime(2020,08,27)),
                new AnalogyChangeLog("V4.2.7 - Change default setting to save Excluded filtering #361",AnalogChangeLogType.Improvement,"Lior Banai",new DateTime(2020,08,26)),
                new AnalogyChangeLog("V4.2.7 - [UI] Add simple/Advance mode #360",AnalogChangeLogType.Improvement,"Lior Banai",new DateTime(2020,08,26)),
                new AnalogyChangeLog("V4.2.7 - [Real time logs] Focus is stolen on every refresh #357",AnalogChangeLogType.Bug,"Lior Banai",new DateTime(2020,08,25)),
                new AnalogyChangeLog("V4.2.7 - [Real time logs] add keyboard shortcut to toggle pause/Resume refresh of logs #355",AnalogChangeLogType.Improvement,"Lior Banai",new DateTime(2020,08,24)),
                new AnalogyChangeLog("V4.2.7 - [Filtering] append logic is incorrect. #330",AnalogChangeLogType.Bug,"Lior Banai",new DateTime(2020,08,23)),
                new AnalogyChangeLog("V4.2.7 - Make IAnalogyRealTimeDataProvider Async. #329",AnalogChangeLogType.Improvement,"Lior Banai",new DateTime(2020,08,23)),
                new AnalogyChangeLog("V4.2.7 - Remember show message details settings. #322",AnalogChangeLogType.Improvement,"Lior Banai",new DateTime(2020,08,18)),
                new AnalogyChangeLog("V4.2.7 - [Data Providers] Add gRPC provider to core 3.1 version. #319",AnalogChangeLogType.Improvement,"Lior Banai",new DateTime(2020,08,13)),
                new AnalogyChangeLog("V4.2.7 - [Feature] Add Generic XML Parser. #300",AnalogChangeLogType.Feature,"Lior Banai",new DateTime(2020,08,09)),
                new AnalogyChangeLog("V4.2.7 - [Feature] Add Generic Json Parser. #299",AnalogChangeLogType.Feature,"Lior Banai",new DateTime(2020,08,09)),
                new AnalogyChangeLog("V4.2.6 - Add + and - shortcuts to open message text details. #293",AnalogChangeLogType.Improvement,"Lior Banai",new DateTime(2020,08,07)),
                new AnalogyChangeLog("V4.2.6 - [Bookmarks] Add button to save non persist bookmarks. #291",AnalogChangeLogType.Improvement,"Lior Banai",new DateTime(2020,08,07)),
                new AnalogyChangeLog("V4.2.6 - Remove dependency on LiorBanai.NotificationWindow assembly. #289",AnalogChangeLogType.Improvement,"Lior Banai",new DateTime(2020,08,03)),
                new AnalogyChangeLog("V4.2.6 - [net471] appconfig is missing support for 4.7.1. #290",AnalogChangeLogType.Bug,"Lior Banai",new DateTime(2020,08,03)),
                new AnalogyChangeLog("V4.2.6 - remove shareable button if no shareable components exists. #284",AnalogChangeLogType.Improvement,"Lior Banai",new DateTime(2020,08,02)),
                new AnalogyChangeLog("V4.2.6 - Remember last position and state does not retains settings under some condition #283",AnalogChangeLogType.Bug,"Lior Banai",new DateTime(2020,07,31)),
                new AnalogyChangeLog("V4.2.6 - [UI] Enable/Disable theme colors for log grids #282",AnalogChangeLogType.Improvement,"Lior Banai",new DateTime(2020,07,31)),
                new AnalogyChangeLog("V4.2.6 - [UI] Log grid context menu does not respect theme colors #280",AnalogChangeLogType.Bug,"Lior Banai",new DateTime(2020,07,31)),
                new AnalogyChangeLog("V4.2.6 - [UI] some UI Elements does not fit to screen in different themes #277",AnalogChangeLogType.Bug,"Lior Banai",new DateTime(2020,07,31)),
                new AnalogyChangeLog("V4.2.6 - Searching based on a selected value in a custom column #271",AnalogChangeLogType.Improvement,"Lior Banai",new DateTime(2020,07,30)),
                new AnalogyChangeLog("V4.2.5 - [Core 3.1] First time run: Fix default user settings for LastUpdate #276",AnalogChangeLogType.Bug,"Lior Banai",new DateTime(2020,07,25)),
                new AnalogyChangeLog("V4.2.5 - [UI] Add color Palette to user settings and revamp the UI #273",AnalogChangeLogType.Improvement,"Lior Banai",new DateTime(2020,07,24)),
                new AnalogyChangeLog("V4.2.5 - [Performance] Reduce the usage of ReaderWriterLockSlim. #272",AnalogChangeLogType.Improvement,"Lior Banai",new DateTime(2020,07,24)),
                new AnalogyChangeLog("V4.2.5 - Set CTRL+F to open search panel automatically. #270",AnalogChangeLogType.Improvement,"Lior Banai",new DateTime(2020,07,24)),
                new AnalogyChangeLog("V4.2.5 - Add Search panel. #269",AnalogChangeLogType.Improvement,"Lior Banai",new DateTime(2020,07,24)),
                new AnalogyChangeLog("V4.2.5 - Add Go To / Jump to message. #267",AnalogChangeLogType.Improvement,"Lior Banai",new DateTime(2020,07,24)),
                new AnalogyChangeLog("V4.2.5 - Add Go To / Jump To timestamp option. #266",AnalogChangeLogType.Improvement,"Lior Banai",new DateTime(2020,07,24)),
                new AnalogyChangeLog("V4.2.5 - Exclude source/module/text does not clear the hints text. #259",AnalogChangeLogType.Bug,"Lior Banai",new DateTime(2020,07,22)),
                new AnalogyChangeLog("V4.2.5 - Allows loading data providers' dependencies from different locations. #184",AnalogChangeLogType.Improvement,"Lior Banai",new DateTime(2020,07,20)),
                new AnalogyChangeLog("V4.2.5 - Add option to select on which columns the search will be performed. #205",AnalogChangeLogType.Improvement,"Lior Banai",new DateTime(2020,07,19)),
                new AnalogyChangeLog("V4.2.5 - Support gz archive files. #231",AnalogChangeLogType.Improvement,"Lior Banai",new DateTime(2020,07,17)),
                new AnalogyChangeLog("V4.2.5 - Add .net framework 4.7.1 compilation target #236",AnalogChangeLogType.Improvement,"Lior Banai",new DateTime(2020,07,16)),
                new AnalogyChangeLog("V4.2.5 - Remove XML format for analogy. #230",AnalogChangeLogType.Improvement,"Lior Banai",new DateTime(2020,07,13)),
                new AnalogyChangeLog("V4.2.5 - Add search filters hints / indicators. #232",AnalogChangeLogType.Improvement,"Lior Banai",new DateTime(2020,07,13)),
                new AnalogyChangeLog("V4.2.5 - show open folder regardless of provider supplied folder. #229",AnalogChangeLogType.Improvement,"Lior Banai",new DateTime(2020,07,12)),
                new AnalogyChangeLog("V4.2.5 - Add Support for reading compressed Zip files. #218",AnalogChangeLogType.Improvement,"Lior Banai",new DateTime(2020,07,11)),
                new AnalogyChangeLog("Add ability to add notes to log messages. #217",AnalogChangeLogType.Improvement,"Lior Banai",new DateTime(2020,07,10)),
                new AnalogyChangeLog("Shows AnalogyLogMessage's AdditionalInformation Properties in Detailed Window. #215",AnalogChangeLogType.Improvement,"Lior Banai",new DateTime(2020,07,10)),
                new AnalogyChangeLog("System.ArgumentException when using the 'messages grouping' tab with a log file that contains custom columns. #225",AnalogChangeLogType.Bug,"Lior Banai",new DateTime(2020,07,10)),
                new AnalogyChangeLog("Add providers icon to the ribbon page. #219",AnalogChangeLogType.Improvement,"Lior Banai",new DateTime(2020,07,09)),
                new AnalogyChangeLog("Main log viewer window always starts up full screen. #213",AnalogChangeLogType.Improvement,"Lior Banai",new DateTime(2020,07,05)),
                new AnalogyChangeLog("Cleanup Filtering UI Elements. #212",AnalogChangeLogType.Improvement,"Lior Banai",new DateTime(2020,07,04)),
                new AnalogyChangeLog( "Add option to select rows and perform operation on the selection (save to file, open in new window, etc). #201",AnalogChangeLogType.Improvement,"Lior Banai", new DateTime(2020, 07, 03)),
                new AnalogyChangeLog( "Add dynamic columns at run time based on AdditionalInformation Property of AnalogyLogMessage. #206",AnalogChangeLogType.Improvement,"Lior Banai", new DateTime(2020, 07, 02)),
                new AnalogyChangeLog( "Add multi select of rows. #204",AnalogChangeLogType.Improvement,"Lior Banai", new DateTime(2020, 06, 28)), new AnalogyChangeLog( "Enable version check on startup. #200",AnalogChangeLogType.Improvement,"Lior Banai", new DateTime(2020, 06, 26)),
                new AnalogyChangeLog( "Ribbon Icons are not correct for real time data sources. #199",AnalogChangeLogType.Bug,"Lior Banai", new DateTime(2020, 06, 26)),
                new AnalogyChangeLog("[Windows Events log Parser] Unable to load system log files (*.evtx). #197",AnalogChangeLogType.Bug,"Lior Banai", new DateTime(2020, 06, 21)),
                new AnalogyChangeLog("Add MachineName to UI columns. #196",AnalogChangeLogType.Improvement,"Lior Banai", new DateTime(2020, 06, 20)),
                new AnalogyChangeLog("Add Recent Folder to The Ribbon UI. #193",AnalogChangeLogType.Feature,"Lior Banai", new DateTime(2020, 06, 19)),
                new AnalogyChangeLog("Remember Last X open folders locations in the Open folder UI. #183",AnalogChangeLogType.Improvement,"Lior Banai", new DateTime(2020, 06, 19)),
                new AnalogyChangeLog("Duplicate files are listed in the UI. #192",AnalogChangeLogType.Bug,"Lior Banai", new DateTime(2020, 06, 19)),
                new AnalogyChangeLog("NullReferenceException when using the data visualizer on a Serilog/Clef log file #189",AnalogChangeLogType.Bug,"Lior Banai", new DateTime(2020, 06, 18)),
                new AnalogyChangeLog("Upgrade to DevExpress 19.1.11. #187",AnalogChangeLogType.Improvement,"Lior Banai", new DateTime(2020, 06, 11)),
                new AnalogyChangeLog("Reload file is ignore when caching is on #181",AnalogChangeLogType.Bug,"Lior Banai", new DateTime(2020, 06, 04)),
                new AnalogyChangeLog("Log Analysis: Add free-text pie chart #177",AnalogChangeLogType.Improvement,"Lior Banai", new DateTime(2020, 05, 29)),
                new AnalogyChangeLog("Store date sorting order between runs #178",AnalogChangeLogType.Improvement,"Lior Banai", new DateTime(2020, 05, 29)),
                new AnalogyChangeLog("Allows application to minimized to the tray bar instead of closing #174",AnalogChangeLogType.Improvement,"Lior Banai", new DateTime(2020, 05, 28)),
                new AnalogyChangeLog("Add Cache cleaner #175",AnalogChangeLogType.Improvement,"Lior Banai", new DateTime(2020, 05, 28)),
                new AnalogyChangeLog("On reload of files add option to color mark the new messages #170",AnalogChangeLogType.Improvement,"Lior Banai", new DateTime(2020, 05, 28)),
                new AnalogyChangeLog("Add auto update/version checker #165",AnalogChangeLogType.Improvement,"Lior Banai", new DateTime(2020, 05, 22)),
                new AnalogyChangeLog("combine opened logs into one. #152",AnalogChangeLogType.Improvement,"Lior Banai", new DateTime(2020, 05, 22)),
            new AnalogyChangeLog("Add Reload option for loaded logs files #169",AnalogChangeLogType.Improvement,"Lior Banai", new DateTime(2020, 05, 14)),
            new AnalogyChangeLog("Add Columns extension support #149",AnalogChangeLogType.Improvement,"Lior Banai", new DateTime(2020, 05, 09)),
            new AnalogyChangeLog("Add an option to display date/time in local pattern #154",AnalogChangeLogType.Improvement,"Lior Banai", new DateTime(2020, 05, 09)),
            new AnalogyChangeLog("User settings: Allow user to change grid headers/titles #153",AnalogChangeLogType.Improvement,"Lior Banai", new DateTime(2020, 05, 08)),
            new AnalogyChangeLog("UI: Add alternate icons (light/dark themes) #167",AnalogChangeLogType.Improvement,"Lior Banai", new DateTime(2020, 05, 08)),
            new AnalogyChangeLog("Allow each Data provider to supply its own icons and Images for the UI #127",AnalogChangeLogType.Improvement,"Lior Banai", new DateTime(2020, 05, 02)),
            new AnalogyChangeLog("Open new logs as new tabs in the same application instance #155",AnalogChangeLogType.Improvement,"Lior Banai", new DateTime(2020, 04, 25)),
            new AnalogyChangeLog("Add Single Instance User settings #157",AnalogChangeLogType.Improvement,"Lior Banai", new DateTime(2020, 04, 25)),
            new AnalogyChangeLog("Date Time value is displayed wrong in Detailed Message window (Issue #156)", AnalogChangeLogType.Bug, "Lior Banai", new DateTime(2020, 04, 25)),
            new AnalogyChangeLog("Logs Analysis: add some pie charts (Issue #131)", AnalogChangeLogType.Feature, "Lior Banai", new DateTime(2020, 04, 06)),
            new AnalogyChangeLog("Logs Analysis: add log level distribution chart (Issue #129)", AnalogChangeLogType.Feature, "Lior Banai", new DateTime(2020, 04, 06)),
            new AnalogyChangeLog("Upgrade to DevExpress 19.1.10 (Issue #128)", AnalogChangeLogType.Bug, "Lior Banai", new DateTime(2020, 04, 06)),
            new AnalogyChangeLog("Bug: Dedicated user setting window does not call the Save method of the data provider (Issue #124", AnalogChangeLogType.Bug, "Lior Banai", new DateTime(2020, 04, 03)),
            new AnalogyChangeLog("Refactor interfaces (Issue #12 at https://github.com/Analogy-LogViewer/Analogy.Interfaces)", AnalogChangeLogType.Improvement, "Lior Banai", new DateTime(2020, 03, 31)),
            new AnalogyChangeLog("UI: Add button to open custom user setting window (Issue #113)", AnalogChangeLogType.Improvement, "Lior Banai", new DateTime(2020, 03, 15)),
            new AnalogyChangeLog("Add MessagePack binary format to built in supported Analogy Formats (Issue #102)", AnalogChangeLogType.Improvement, "Lior Banai", new DateTime(2020, 02, 29)),
            new AnalogyChangeLog("Add shortcut for toggle detailed messages info (Issue #96)", AnalogChangeLogType.Improvement, "Lior Banai", new DateTime(2020, 02, 14)),
            new AnalogyChangeLog("Upgrade DevExpress to v19.1.9 (Issue #97)", AnalogChangeLogType.Improvement, "Lior Banai", new DateTime(2020, 02, 14)),
            new AnalogyChangeLog("NullReferenceException during row painting (Philips ICAP data provider). (Issue #85)", AnalogChangeLogType.Improvement, "Lior Banai", new DateTime(2019, 12, 27)),
            new AnalogyChangeLog("Persist Analogy internal log to file (issue #83)", AnalogChangeLogType.Improvement, "Lior Banai", new DateTime(2020, 01, 14)),
            new AnalogyChangeLog("Move to SDK-Style projects and package references. (issue #82)", AnalogChangeLogType.Improvement, "Lior Banai", new DateTime(2019, 12, 27)),
            new AnalogyChangeLog("Upgrade DevExpress Version to 19.1.8 (Latest supported with current license)", AnalogChangeLogType.Improvement, "Lior Banai", new DateTime(2019, 12, 27)),
            new AnalogyChangeLog("Default user setting are invalid for some properties (issue #76)", AnalogChangeLogType.Bug, "Lior Banai", new DateTime(2020, 01, 03)),
            new AnalogyChangeLog("Auto complete user setting is not working (issue #78)", AnalogChangeLogType.Bug, "Lior Banai", new DateTime(2019, 12, 27)),
            new AnalogyChangeLog("Add docking/un-docking management when more than one log is opened (issue #75)", AnalogChangeLogType.Improvement, "Lior Banai", new DateTime(2019, 12, 24)),
            new AnalogyChangeLog("Changes in 'include Text' text box effect 'highlight lines...' text box (Issue #74)", AnalogChangeLogType.Improvement, "Lior Banai", new DateTime(2019, 12, 20)),
            new AnalogyChangeLog("Add alert messages and notifications (Issue #46)", AnalogChangeLogType.Improvement, "Lior Banai", new DateTime(2019, 12, 14)),
            new AnalogyChangeLog("Update all dependencies nuget versions (issue #73)", AnalogChangeLogType.None, "Lior Banai", new DateTime(2019, 12, 14)),
            new AnalogyChangeLog("Shortcuts keys are not correct (issue #72)", AnalogChangeLogType.Bug, "Lior Banai", new DateTime(2019, 12, 14)),
            new AnalogyChangeLog("Search filtering: Add user predefined search queries (issue #11)", AnalogChangeLogType.Improvement, "Lior Banai", new DateTime(2019, 12, 13)),
            new AnalogyChangeLog("Add individual colors rules (issue #03)", AnalogChangeLogType.Improvement, "Lior Banai", new DateTime(2019, 12, 12)),
            new AnalogyChangeLog("File listing should be sorted from newest to oldest and customizable by the user (issue #69)", AnalogChangeLogType.Improvement, "Lior Banai", new DateTime(2019, 12, 09)),
            new AnalogyChangeLog("Consolidate all data providers (per factory) under the ribbon UI (issue #63)", AnalogChangeLogType.Improvement, "Lior Banai", new DateTime(2019, 12, 09)),
            new AnalogyChangeLog("changing log level checks include text & Exclude text boxes (issue #68)", AnalogChangeLogType.Bug, "Lior Banai", new DateTime(2019, 12, 08)),
            new AnalogyChangeLog("Performance: Improve performance when querying messages for count display (issue #67)", AnalogChangeLogType.Improvement, "Lior Banai", new DateTime(2019, 12, 07)),
            new AnalogyChangeLog("Add an option to remember last opened data provider (issue #65)", AnalogChangeLogType.Improvement, "Lior Banai", new DateTime(2019, 12, 03)),
            new AnalogyChangeLog("Add logging of errors during startup for better debugging (issue #66)", AnalogChangeLogType.Improvement, "Lior Banai", new DateTime(2019, 12, 03)),
            new AnalogyChangeLog("Add Date/time option to filter the messages (issue #62)", AnalogChangeLogType.Improvement, "Lior Banai", new DateTime(2019, 11, 29)),
            new AnalogyChangeLog("Add export/import settings (issue #47)", AnalogChangeLogType.Improvement, "Lior Banai", new DateTime(2019, 11, 23)),
            new AnalogyChangeLog("Add default file-data provider associations (issue #45)", AnalogChangeLogType.Improvement, "Lior Banai", new DateTime(2019, 11, 23)),
            new AnalogyChangeLog("Allows disabling of data providers (issue #60)", AnalogChangeLogType.Improvement, "Lior Banai", new DateTime(2019, 11, 22)),
            new AnalogyChangeLog("User Settings: Add Export/Import of messages' color settings (Issue #56)", AnalogChangeLogType.Improvement, "Lior Banai", new DateTime(2019, 11, 19)),
            new AnalogyChangeLog("Bookmarks window is not customizable (font size). (issue #59)", AnalogChangeLogType.Bug, "Lior Banai", new DateTime(2019, 11, 19)),
            new AnalogyChangeLog("Detailed message window is ignoring in-place filtering (in the grid control) and shows incorrect number of messages (issue #58)", AnalogChangeLogType.Bug, "Lior Banai", new DateTime(2019, 11, 19)),
            new AnalogyChangeLog("Usability: add indication in the file list of the loaded file. (Issue #57))", AnalogChangeLogType.Improvement, "Lior Banai", new DateTime(2019, 11, 19)),
            new AnalogyChangeLog("user settings: Add option to set default date sort option (ascend/descend) (issue #55)", AnalogChangeLogType.Improvement, "Lior Banai", new DateTime(2019, 11, 17)),
            new AnalogyChangeLog("File Pooling: add indication of number of new messages per update (issue #51)", AnalogChangeLogType.Improvement, "Lior Banai", new DateTime(2019, 11, 14)),
            new AnalogyChangeLog("File Pooling: Add an option to ignore global caching settings or adding to recent files history. (issue #54)", AnalogChangeLogType.Improvement, "Lior Banai", new DateTime(2019, 11, 14)),
            new AnalogyChangeLog("NLog Data Provider: Add import/export of settings. (issue #52)", AnalogChangeLogType.Improvement, "Lior Banai", new DateTime(2019, 11, 12)),
            new AnalogyChangeLog("Built-in Data providers are ignored when double clicking and log file. (issue #50)", AnalogChangeLogType.Bug, "Lior Banai", new DateTime(2019, 11, 10)),
            new AnalogyChangeLog("File pooling: duplicate entries are added (issue #49)", AnalogChangeLogType.Bug, "Lior Banai", new DateTime(2019, 11, 10)),
            new AnalogyChangeLog("Coloring: Add user settings for log level colors (issue #48)", AnalogChangeLogType.Improvement, "Lior Banai", new DateTime(2019, 11, 09)),
            new AnalogyChangeLog("Data Providers: Add NLog data Provider (issue #19)", AnalogChangeLogType.Improvement, "Lior Banai", new DateTime(2019, 11, 07)),
            new AnalogyChangeLog("Performance: Load data providers asynchronously during startup (issue #40)", AnalogChangeLogType.Improvement, "Lior Banai", new DateTime(2019, 10, 21)),
            new AnalogyChangeLog("adding .net Core 3.0 Target", AnalogChangeLogType.Improvement, "Lior Banai", new DateTime(2019, 10, 26)),
            new AnalogyChangeLog("Offline files: Add option for file pooling. (issue #36)", AnalogChangeLogType.Improvement, "Lior Banai", new DateTime(2019, 10, 24)),
            new AnalogyChangeLog("cleanup old namespaces (issue #34)", AnalogChangeLogType.Improvement, "Lior Banai", new DateTime(2019, 10, 18)),
            new AnalogyChangeLog("frameworks update: interface dll is not target .net standard 2.0 and Analogy UI is .net framework 4.7.2 (issues #32,#33)", AnalogChangeLogType.Improvement, "Lior Banai", new DateTime(2019, 10, 18)),
            new AnalogyChangeLog("UI: not all UI elements support themes (issue #28)", AnalogChangeLogType.Bug, "Lior Banai", new DateTime(2019, 10, 14)),
            new AnalogyChangeLog("UI: Add an option (user setting) to auto scroll to last message (Issue #26)", AnalogChangeLogType.Improvement, "Lior Banai", new DateTime(2019, 10, 13)),
            new AnalogyChangeLog("Fix: DevExpress Control: Sometimes an error occurs and the grid displays a big red X (Issue #4)", AnalogChangeLogType.Bug, "Lior Banai", new DateTime(2019, 10, 11)),
            new AnalogyChangeLog("UI: Add fast file caching switching (Issue #23)", AnalogChangeLogType.Improvement, "Lior Banai", new DateTime(2019, 10, 11)),
            new AnalogyChangeLog("UI: Add more information about messages in the Message's detail window (Issue #22)", AnalogChangeLogType.Improvement, "Lior Banai", new DateTime(2019, 10, 09)),
            new AnalogyChangeLog("Fix: Online Data Providers are not disposed during shutdown of Analogy (Issue #21)", AnalogChangeLogType.Bug, "Lior Banai", new DateTime(2019, 10, 09)),
            new AnalogyChangeLog("Fix: Folder loading is ignoring data source supported files (Issue #18)", AnalogChangeLogType.Improvement, "Lior Banai", new DateTime(2019, 09, 26)),
            new AnalogyChangeLog("Add an option to un-dock current view and keep receiving new messages (Issue #16)", AnalogChangeLogType.Improvement, "Lior Banai", new DateTime(2019, 09, 25)),
            new AnalogyChangeLog("duplicate chrome tab behavior (close tab keyboard shortcut, close all other tabs, etc) (Issue #8)", AnalogChangeLogType.Improvement, "Lior Banai", new DateTime(2019, 09, 24)),
            new AnalogyChangeLog("remove Auto Start value form the interface and make it UI option", AnalogChangeLogType.Improvement, "Lior Banai", new DateTime(2019, 09, 21)),
            new AnalogyChangeLog("Startup data sources: add a user setting to define list of startup data sources (Issue #10)", AnalogChangeLogType.Improvement, "Lior Banai", new DateTime(2019, 09, 21)),
            new AnalogyChangeLog("Windows Event logs: add support for custom logs in addition to Windows logs (Issue #7)", AnalogChangeLogType.Improvement, "Lior Banai", new DateTime(2019, 09, 20)),
            new AnalogyChangeLog("user settings are not saved between different versions (Issue #13)", AnalogChangeLogType.Bug, "Lior Banai", new DateTime(2019, 09, 20)),
            new AnalogyChangeLog("Add resource usage indication and idle mode (Issue #6)", AnalogChangeLogType.Improvement, "Lior Banai", new DateTime(2019, 09, 20)),
            new AnalogyChangeLog("Fix unable to save logs for real time Data sources", AnalogChangeLogType.Bug, "Lior Banai", new DateTime(2019, 09, 09)),
            new AnalogyChangeLog("Add more information in the offline files listing.", AnalogChangeLogType.Improvement, "Lior Banai", new DateTime(2019, 08, 31)),
            new AnalogyChangeLog("Fix about Page loading.", AnalogChangeLogType.Bug, "Lior Banai", new DateTime(2019, 08, 30)),
            new AnalogyChangeLog("First release as open source tool.", AnalogChangeLogType.Feature, "Lior Banai", new DateTime(2019, 08, 19)),
            };
        }

        public static string GetChangeLogFull => string.Join(Environment.NewLine,
            GetChangeLog().OrderByDescending(c => c.Date).Select(cl => $"{cl.Date.ToShortDateString()}: {cl.ChangeInformation} ({cl.Name})"));

    }
}
