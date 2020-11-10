using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Analogy.Interfaces;
using Analogy.LogLoaders;
using Analogy.Managers;

namespace Analogy
{
    public class BookmarkPersistManager : ILogMessageCreatedHandler
    {
        private static readonly Lazy<BookmarkPersistManager> instance =
            new Lazy<BookmarkPersistManager>(() => new BookmarkPersistManager());
        public EventHandler<LogMessageArgs> MessageReceived;
        public EventHandler<LogMessageArgs> MessageRemoved;
        public static BookmarkPersistManager Instance => instance.Value;
        private bool ContentChanged;
        private bool fileLoaded;
        private List<AnalogyLogMessage> Messages { get; set; }
        private string BookmarkFileName { get; } = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AnalogyBookmarks.log");
        public bool ForceNoFileCaching { get; set; } = false;
        public bool DoNotAddToRecentHistory { get; set; } = false;
        public BookmarkPersistManager()
        {
            Messages = new List<AnalogyLogMessage>();

        }

        public void AppendMessage(AnalogyLogMessage message, string dataSource)
        {
            Messages.Add(message);
        }

        public void AppendMessages(List<AnalogyLogMessage> messages, string dataSource)
        {
            Messages.AddRange(messages);
        }

        public void AppendMessage(DataRow dtr, string dataSource)
        {
            AnalogyLogMessage message = (AnalogyLogMessage)dtr["Object"];
            AppendMessage(message, dataSource);
        }

        public async Task<List<AnalogyLogMessage>> GetMessages()
        {
            if (fileLoaded || !File.Exists(BookmarkFileName))
            {
                return Messages;
            }

            //todo: which format;
            try
            {
                AnalogyJsonLogFile read = new AnalogyJsonLogFile();
                Messages = (await read.ReadFromFile(BookmarkFileName, new CancellationToken(), null)).ToList();
                fileLoaded = true;
            }
            catch (Exception e)
            {
                AnalogyLogManager.Instance.LogError("Error reading file: " + e, nameof(BookmarkPersistManager));
                XtraMessageBox.Show(e.Message, @"Error reading file", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return Messages;
        }

        public void AddBookmarkedMessage(AnalogyLogMessage message, string dataSource)
        {
            if (!Messages.Contains(message))
            {
                Messages.Add(message);
                ContentChanged = true;
                MessageReceived?.Invoke(this, new LogMessageArgs(message, Environment.MachineName, dataSource));
            }
        }

        public void RemoveBookmark(AnalogyLogMessage message)
        {
            if (Messages.Contains(message))
            {
                Messages.Remove(message);
                ContentChanged = true;
                MessageRemoved?.Invoke(this, new LogMessageArgs(message, Environment.MachineName, ""));
            }
        }

        public void SaveFile()
        {
            if (!ContentChanged)
            {
                return;
            }

            if (!Messages.Any())
            {
                if (File.Exists(BookmarkFileName))
                {
                    try
                    {
                        File.Delete(BookmarkFileName);
                    }
                    catch (Exception e)
                    {
                        AnalogyLogManager.Instance.LogError("Error deleting file: " + e, nameof(BookmarkPersistManager));
                    }
                }
            }
            else
            {

                try
                {
                    AnalogyJsonLogFile save = new AnalogyJsonLogFile();
                    save.Save(Messages, BookmarkFileName);
                }
                catch (Exception e)
                {
                    AnalogyLogManager.Instance.LogError("Error saving file: " + e, nameof(BookmarkPersistManager));
                    XtraMessageBox.Show(e.Message, @"Error Saving file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public void ClearBookmarks()
        {
            if (Messages.Any())
            {
                Messages.Clear();
                ContentChanged = true;
            }
        }
    }
}
