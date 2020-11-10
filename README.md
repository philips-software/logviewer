# Analogy Log Viewer    <img src="./Assets/Analogy1024x1024.jpg" align="right" width="155px" height="155px">

<p align="center">
    
[![Codacy Badge](https://api.codacy.com/project/badge/Grade/9ff37aaf309b45f0bb2156465eb47b91)](https://app.codacy.com/gh/Analogy-LogViewer/Analogy.LogViewer?utm_source=github.com&utm_medium=referral&utm_content=Analogy-LogViewer/Analogy.LogViewer&utm_campaign=Badge_Grade)
[![Gitter](https://badges.gitter.im/Analogy-LogViewer/community.svg)](https://gitter.im/Analogy-LogViewer/community?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge) 
[![Build Status](https://dev.azure.com/Analogy-LogViewer/Analogy%20Log%20Viewer/_apis/build/status/Analogy-LogViewer.Analogy.LogViewer?branchName=master)](https://dev.azure.com/Analogy-LogViewer/Analogy%20Log%20Viewer/_build/latest?definitionId=1&branchName=master)  ![.NET Core Desktop](https://github.com/Analogy-LogViewer/Analogy.LogViewer/workflows/.NET%20Core%20Desktop/badge.svg)
[![Dependabot Status](https://api.dependabot.com/badges/status?host=github&repo=Analogy-LogViewer/Analogy.LogViewer)](https://dependabot.com)
<a href="https://github.com/Analogy-LogViewer/Analogy.LogViewer/issues">
    <img src="https://img.shields.io/github/issues/Analogy-LogViewer/Analogy.LogViewer"  alt="Issues" />
</a>
![GitHub closed issues](https://img.shields.io/github/issues-closed-raw/Analogy-LogViewer/Analogy.LogViewer)
<a href="https://github.com/Analogy-LogViewer/Analogy.LogViewer/blob/master/LICENSE.md">
    <img src="https://img.shields.io/github/license/Analogy-LogViewer/Analogy.LogViewer"  alt="License" />
</a>
<a href="https://github.com/Analogy-LogViewer/Analogy.LogViewer/releases">
    <img src="https://img.shields.io/github/v/release/Analogy-LogViewer/Analogy.LogViewer"  alt="Latest Release" />
</a>
<a href="https://github.com/Analogy-LogViewer/Analogy.LogViewer/compare/V4.2.11...master">
    <img src="https://img.shields.io/github/commits-since/Analogy-LogViewer/Analogy.LogViewer/latest"  alt="Commits Since Latest Release"/>
</a>
</p>

A log viewer that can parse log files and stream logs from C#, Python, Java and others.

## Content
[General](https://github.com/Analogy-LogViewer/Analogy.LogViewer#general)

[Usage and how to create custom data providers](https://github.com/Analogy-LogViewer/Analogy.LogViewer#usage)

[Logs Analysis and Visualizers](https://github.com/Analogy-LogViewer/Analogy.LogViewer#logs-analysis-and-visualizers)

[Issues](https://github.com/Analogy-LogViewer/Analogy.LogViewer#issues)

[Contact](https://github.com/Analogy-LogViewer/Analogy.LogViewer#contact)

## General
Analogy Log Viewer is multi purpose Log Viewer for Windows Operating systems with support for most log frameworks log files:
it supports both offline log files:
1. Supports common frameworks like [Serilog](https://github.com/Analogy-LogViewer/Analogy.LogViewer.Serilog), [NLog](https://github.com/Analogy-LogViewer/Analogy.LogViewer.NLog), etc.
2. Supports generic log files like plain text with [Regex parser](https://github.com/Analogy-LogViewer/Analogy.LogViewer.RegexParser) and [XML](https://github.com/Analogy-LogViewer/Analogy.LogViewer.XMLParser)/[Json](https://github.com/Analogy-LogViewer/Analogy.LogViewer.JsonParser) parsers.

And real time streaming of logs: 
1. [NLog Target](https://github.com/Analogy-LogViewer/Analogy.LogViewer.NLog.Targets) 
2. [Serilog Sink](https://github.com/Analogy-LogViewer/Analogy.LogViewer.Serilog) 
3. [Asp Core log provider](https://github.com/Analogy-LogViewer/Analogy.AspNetCore.LogProvider)
4. [Python streaming](https://github.com/Analogy-LogViewer/Analogy-Python-Logging)

 For a complete list visit [Analogy Overview repository](https://github.com/Analogy-LogViewer/Overview).

The application has many standard operations for analysis logs (like filtering, excluding) but its strength is in the ability to add additional custom data sources by implementing few interfaces.
This allows adding any logs formats and/or custom modification of logs before presenting the data in the UI Layer.

Some features of this tool are:
1.	Windows event log support (evtx files)
2.	Aggregation into single view.
3.	Search in multiple files
4.	Combine multiple files
5.	Compare logs 
6.	Themes support
7.	64 bit support (allow loading more files)
8.	Personalization (users settings per user) 
9.	Columns Extendable: Ability to add more columns specific to the data source implementation
10.	Exporting to Excel/CSV files
11. Collaboration-like feature: ability to send log messages to gRPC/WCF service and/or between data providers

Main interaction UI:
- Ribbon area: Log files operations (open) and tools (search/combine/Compare)
- Messages area: File system UI and Main Log viewer area
![Main screen](Assets/AnalogyMainUI2.jpg)

## Usage

Thre are 3 modes of operations:
- real time log server: a gRPC Windows service that can receive messages from any gRPC client and the Log viewer app can connect to it to show real time logs.
- real time logs: different implementation tha tcan receive logs in real time (e.g: Windows event log data provider that show event logs as they are created)
- Offline mode: Parse log files. There are many different implemetations.
For a full list: [see implementations](https://github.com/Analogy-LogViewer/Analogy#log-viewer-application) for common logs files/frameworks.

You can also create your own providers:
to implement a new data provider do the following:

0. Create new  cs project and make sure your assembly is named Analogy.LogViewer.*.dll.
1. reference nuget package [Analogy.LogViewer.Interfaces](https://www.nuget.org/packages/Analogy.LogViewer.Interfaces/).
2. implement interface:
```csharp
    public interface IAnalogyFactory
    {
        Guid FactoryId { get; }
        string Title { get; }
        IEnumerable<IAnalogyChangeLog> ChangeLog { get; }
        IEnumerable<string> Contributors { get; }
        string About { get; }
    }
```

The FactoryId is the identifier of your provider.

3. implement interfaces IAnalogyRealTimeDataProvider (for real time messages) or IAnalogyOfflineDataProvider (for existing log files).

```csharp
  public interface IAnalogyRealTimeDataProvider : IAnalogyDataProvider
  {
    event EventHandler<AnalogyDataSourceDisconnectedArgs> OnDisconnected;
    event EventHandler<AnalogyLogMessageArgs> OnMessageReady;
    event EventHandler<AnalogyLogMessagesArgs> OnManyMessagesReady;
    IAnalogyOfflineDataProvider FileOperationsHandler { get; }
    Task<bool> CanStartReceiving();
    void StartReceiving();
    void StopReceiving();
    bool IsConnected { get; }
  }
```

```csharp

  public interface IAnalogyOfflineDataProvider : IAnalogyDataProvider
  {
    bool DisableFilePoolingOption { get; }
    bool CanSaveToLogFile { get; }
    string FileOpenDialogFilters { get; }
    string FileSaveDialogFilters { get; }
    IEnumerable<string> SupportFormats { get; }
    string InitialFolderFullPath { get; }
    Task<IEnumerable<AnalogyLogMessage>> Process(
      string fileName,
      CancellationToken token,
      ILogMessageCreatedHandler messagesHandler);
    IEnumerable<FileInfo> GetSupportedFiles(DirectoryInfo dirInfo, bool recursiveLoad);
    Task SaveAsync(List<AnalogyLogMessage> messages, string fileName);
    bool CanOpenFile(string fileName);
    bool CanOpenAllFiles(IEnumerable<string> fileNames);
  }
```

4. Implement the container for those interfaces IAnalogyDataProvidersFactory (make sure FactoryId is the same one you created at step 2):

```csharp

    public interface IAnalogyDataProvidersFactory
    {
        /// <summary>
        /// the factory id which this Data providers factory belongs to
        /// </summary>
        Guid FactoryId { get; }
        string Title { get; }
        IEnumerable<IAnalogyDataProvider> DataProviders { get; }
    }
```

you can implement additional interfaces:
 - IAnalogyDataProviderSettings - add ability to create use control to load in the application user settings. You can create a specific UI to change specific settings for your provider.
this interface comes from nuget package [Analogy.DataProviders.Extensions](https://www.nuget.org/packages/Analogy.DataProviders.Extensions/).
 - IAnalogyCustomActionsFactory - custom action to add to the UI
 - IAnalogyShareableFactory - add ability to send log messages from one provider to another

Project [Analogy.LogViewer.Example](https://github.com/Analogy-LogViewer/Analogy.LogViewer.Example) has concrete example.

5. Put your dll at the same folder as the application. You can download [latest version](https://github.com/Analogy-LogViewer/Analogy.LogViewer/releases)

## Logs Analysis and Visualizers

The application has some analyzers and visualization.
- Time Distribution: shows at what time of day the message was logged.
- Frequency: shows count of how many repeated messages were logged (you can define the text to filter).
- On/Off Distribution: Show existance of message along the time.
![Plots Screen](Assets/gitHistoryDataVisualizer.jpg)


- Pie Charts: shows pie charts per source/module/log level.
![Pie Charts](Assets/AnalogyPie.jpg)


- Json Visualizer:
![Json Visualizer](Assets/JsonVisualizer.jpg)


## Issues
- Windows 10 blocks Zip files by default. Make sure to unblock them before unzipping. [How to unblocked](https://singularlabs.com/tips/how-to-unblock-a-zip-file-on-windows-10/).

<a name="contact"></a>
## Contact

### Owner
- [Lior Banai](mailto:liorbanai@gmail.com)
