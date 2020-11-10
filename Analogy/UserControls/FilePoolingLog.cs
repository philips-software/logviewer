using Analogy.Interfaces;
using Analogy.Managers;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;
using Analogy.DataTypes;
using Analogy.Forms;
using Message = System.Windows.Forms.Message;

namespace Analogy
{

    public partial class FilePoolingUCLogs : UserControl
    {
        private bool showHistory = UserSettingsManager.UserSettings.ShowHistoryOfClearedMessages;
        private static int clearHistoryCounter;
        private string FileName { get; set; }
        public bool Enable { get; set; } = true;
        private FilePoolingManager PoolingManager { get; }
        public FilePoolingUCLogs(IAnalogyOfflineDataProvider offlineDataProvider, string fileName, string initialFolder)
        {
            InitializeComponent();
            FileName = fileName;
            PoolingManager = new FilePoolingManager(FileName,ucLogs1, offlineDataProvider);
            ucLogs1.SetFileDataSource(offlineDataProvider, offlineDataProvider);
            ucLogs1.EnableFileReload(FileName);
          
            PoolingManager.OnNewMessages += (s, data) =>
            {
                AppendMessages(data.messages, data.dataSource);
                OnNewMessages(data.messages);
            };
            this.Disposed += FilePoolingUCLogs_Disposed;

        }

        private void FilePoolingUCLogs_Disposed(object sender, EventArgs e)
        {
            PoolingManager.StopMonitoring();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            ucLogs1.ProcessCmdKeyFromParent(keyData);
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private async void OnlineUCLogs_Load(object sender, EventArgs e)
        {
            if (DesignMode)
            {
                return;
            }

            ucLogs1.OnHistoryCleared += UcLogs1_OnHistoryCleared;
            spltMain.Panel1Collapsed = true;
            await PoolingManager.Init();
        }

        private void UcLogs1_OnHistoryCleared(object sender, AnalogyClearedHistoryEventArgs e)
        {
            Interlocked.Increment(ref clearHistoryCounter);
            listBoxClearHistory.SelectedIndexChanged -= ListBoxClearHistoryIndexChanged;
            spltMain.Panel1Collapsed = !showHistory;
            string entry = $"cleared #{clearHistoryCounter} ({e.ClearedMessages.Count} messages)";
            FileProcessingManager.Instance.DoneProcessingFile(e.ClearedMessages, entry);
            listBoxClearHistory.Items.Add(entry);
            listBoxClearHistory.SelectedItem = null;
            listBoxClearHistory.SelectedIndex = -1;
            listBoxClearHistory.SelectedIndexChanged += ListBoxClearHistoryIndexChanged;
        }
        private void OnNewMessages(List<AnalogyLogMessage> messages)
        {
            if (IsDisposed || !IsHandleCreated)
            {
                return;
            }

            BeginInvoke(new MethodInvoker(() =>
            {
                Interlocked.Increment(ref clearHistoryCounter);
                listBoxClearHistory.SelectedIndexChanged -= ListBoxClearHistoryIndexChanged;
                spltMain.Panel1Collapsed = !showHistory;
                string entry = $"New #{clearHistoryCounter} ({messages.Count} messages)";
                FileProcessingManager.Instance.DoneProcessingFile(messages, entry);
                listBoxClearHistory.Items.Add(entry);
                listBoxClearHistory.SelectedItem = null;
                listBoxClearHistory.SelectedIndex = -1;
                listBoxClearHistory.SelectedIndexChanged += ListBoxClearHistoryIndexChanged;
            }));

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void AppendMessages(List<AnalogyLogMessage> messages, string dataSource)
        {
            if (Enable && !IsDisposed)
            {

                string interned = string.Intern(dataSource);
                ucLogs1.AppendMessages(messages, interned);

            }
        }

        private void bbtnClear_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            listBoxClearHistory.Items.Clear();
        }

        private void bbtnHide_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (IsDisposed)
            {
                return;
            }

            showHistory = false;
            spltMain.Panel1Collapsed = true;
        }

        private void ListBoxClearHistoryIndexChanged(object sender, EventArgs e)
        {
            if (listBoxClearHistory.SelectedItem == null)
            {
                return;
            }

            var messages = FileProcessingManager.Instance.GetMessages((string)listBoxClearHistory.SelectedItem);
            XtraFormLogGrid grid = new XtraFormLogGrid(messages, Environment.MachineName, ucLogs1.DataProvider, ucLogs1.FileDataProvider);
            grid.Show(this);
        }
    }

}


