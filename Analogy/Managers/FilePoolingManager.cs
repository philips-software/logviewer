using Analogy.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Analogy.Managers
{
    internal class FilePoolingManager : ILogMessageCreatedHandler
    {
        private string FileName { get; }
        private FileProcessor FileProcessor { get; }
        private IAnalogyOfflineDataProvider OfflineDataProvider { get; }
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly List<AnalogyLogMessage> _messages;
        public EventHandler<(List<AnalogyLogMessage> messages, string dataSource)> OnNewMessages;
        public bool ForceNoFileCaching { get; set; } = true;

        public bool DoNotAddToRecentHistory { get; set; } = true;
        private readonly object _sync;
        private FileSystemWatcher _watchFile;
        private bool _readingInprogress;
        private DateTime lastWriteTime = DateTime.MinValue;
        private UCLogs LogUI;
        private readonly AnalogyLogMessageCustomEqualityComparer _customEqualityComparer;
        public FilePoolingManager(string fileName, UCLogs logUI, IAnalogyOfflineDataProvider offlineDataProvider)
        {
            _sync = new object();
            LogUI = logUI;
            _customEqualityComparer = new AnalogyLogMessageCustomEqualityComparer { CompareId = false };
            _cancellationTokenSource = new CancellationTokenSource();
            OfflineDataProvider = offlineDataProvider;
            _messages = new List<AnalogyLogMessage>();
            FileName = fileName;
            FileProcessor = new FileProcessor(this);

        }


        public Task Init()
        {
            _watchFile = new FileSystemWatcher
            {
                Path = Path.GetDirectoryName(FileName),
                Filter = Path.GetFileName(FileName)
            };
            _watchFile.Changed += WatchFile_Changed;
            _watchFile.Deleted += WatchFile_Deleted;
            _watchFile.Renamed += WatchFile_Renamed;
            _watchFile.Error += WatchFile_Error;
            _watchFile.EnableRaisingEvents = true;
            AnalogyLogMessage m = new AnalogyLogMessage
            {
                Text = $"Start monitoring file {FileName}.",
                FileName = FileName,
                Level = AnalogyLogLevel.Analogy,
                Category = "",
                Source = "Analogy",
                Class = AnalogyLogClass.General,
                Date = DateTime.Now
            };
            OnNewMessages?.Invoke(this, (new List<AnalogyLogMessage> { m }, FileName));
            return FileProcessor.Process(OfflineDataProvider, FileName, _cancellationTokenSource.Token);
        }

        public void StopMonitoring()
        {

            _watchFile.EnableRaisingEvents = false;
            _watchFile.Changed -= WatchFile_Changed;
            _watchFile.Deleted -= WatchFile_Deleted;
            _watchFile.Renamed -= WatchFile_Renamed;
            _watchFile.Error -= WatchFile_Error;
            _watchFile.Dispose();
        }
        public void AppendMessage(AnalogyLogMessage message, string dataSource)
        {
            lock (_sync)
            {
                if (!_messages.Contains(message))
                {
                    _messages.Add(message);
                    OnNewMessages?.Invoke(this, (new List<AnalogyLogMessage> { message }, dataSource));
                }
            }
        }

        public void AppendMessages(List<AnalogyLogMessage> messagesFromFile, string dataSource)
        {

            lock (_sync)
            {
                var newMessages = messagesFromFile.Except(_messages, _customEqualityComparer).ToList();
                if (newMessages.Any())
                {
                    _messages.AddRange(newMessages);
                    OnNewMessages?.Invoke(this, (newMessages, dataSource));
                }
            }
        }

        private void WatchFile_Error(object sender, ErrorEventArgs e)
        {
            _watchFile.EnableRaisingEvents = false;
            _watchFile.Dispose();
            AnalogyLogMessage m = new AnalogyLogMessage
            {
                Text = $"Error monitoring file {FileName}. Reason {e.GetException()}",
                FileName = FileName,
                Level = AnalogyLogLevel.Critical,
                Category = "",
                Source = "Analogy",
                Class = AnalogyLogClass.General,
                Date = DateTime.Now
            };
            OnNewMessages?.Invoke(this, (new List<AnalogyLogMessage> { m }, FileName));
        }

        private async void WatchFile_Renamed(object sender, RenamedEventArgs e)
        {
            _watchFile.EnableRaisingEvents = false;
            AnalogyLogMessage m = new AnalogyLogMessage
            {
                Text = $"{FileName} has changed to {e.FullPath} from {e.OldName}. restarting monitoring",
                FileName = FileName,
                Level = AnalogyLogLevel.Warning,
                Category = "",
                Source = "Analogy",
                Class = AnalogyLogClass.General,
                Date = DateTime.Now
            };
            _watchFile.Dispose();
            OnNewMessages?.Invoke(this, (new List<AnalogyLogMessage> { m }, FileName));
            await Init();

        }

        private void WatchFile_Deleted(object sender, FileSystemEventArgs e)
        {
            _watchFile.EnableRaisingEvents = false;
            AnalogyLogMessage m = new AnalogyLogMessage
            {
                Text = $"{FileName} has been deleted. Stopping monitoring",
                FileName = FileName,
                Level = AnalogyLogLevel.Warning,
                Category = "",
                Class = AnalogyLogClass.General,
                Date = DateTime.Now
            };
            _watchFile.Dispose();
            OnNewMessages?.Invoke(this, (new List<AnalogyLogMessage> { m }, FileName));
        }

        private async void WatchFile_Changed(object sender, FileSystemEventArgs e)
        {
            if (_readingInprogress)
            {
                return;
            }

            FileInfo f = new FileInfo(e.FullPath);
            if (lastWriteTime == f.LastWriteTime)
            {
                return;
            }

            lastWriteTime = f.LastWriteTime;
            lock (_sync)
            {
                if (_readingInprogress)
                {
                    return;
                }

                _watchFile.EnableRaisingEvents = false;
                _readingInprogress = true;
            }

            try
            {
                if (e.ChangeType == WatcherChangeTypes.Changed)
                {
                    LogUI.SetReloadColorDate(FileProcessor.lastNewestMessage);
                    await FileProcessor.Process(OfflineDataProvider, FileName, _cancellationTokenSource.Token);
                }
            }
            catch (Exception exception)
            {
                AnalogyLogMessage m = new AnalogyLogMessage
                {
                    Text = $"Error monitoring file {FileName}. Reason {exception}",
                    FileName = FileName,
                    Level = AnalogyLogLevel.Warning,
                    Category = "",
                    Class = AnalogyLogClass.General,
                    Date = DateTime.Now
                };
                OnNewMessages?.Invoke(this, (new List<AnalogyLogMessage> { m }, FileName));
                AnalogyLogManager.Instance.LogErrorMessage(m);
            }
            finally
            {
                _readingInprogress = false;
                _watchFile.EnableRaisingEvents = true;
            }
        }

    }
}

