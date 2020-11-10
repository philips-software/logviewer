using Analogy.Interfaces;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Analogy.Interfaces.DataTypes;

namespace Analogy
{
    public partial class FilesOperationsUC : XtraUserControl, ILogMessageCreatedHandler
    {
        public bool ForceNoFileCaching { get; set; } = false;
        public bool DoNotAddToRecentHistory { get; set; } = false;
        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private int refreshDelay = 100;
        private DateTime LastReportUpdate = DateTime.Now;
        public IAnalogyOfflineDataProvider DataProvider { get; set; }

        private FileProcessingManager Manager { get; } = new FileProcessingManager();
        private IProgress<AnalogyProgressReport> ProgressReporter { get; set; }
        private IProgress<string> TextProgressReporter { get; set; }
        private List<string> FileNames { get; set; }
        private List<string> TextMessages { get; set; }

        private List<Tuple<string, AnalogyLogMessage>> Messages { get; } = new List<Tuple<string, AnalogyLogMessage>>();
        private object lockObject = new object();
        private bool _aborted;

        private bool Aborted
        {
            get => _aborted;
            set
            {
                _aborted = value;
                if (_aborted)
                {
                    cancellationTokenSource.Cancel();
                    TextProgressReporter.Report("Cancellation in progress. Please wait");
                }
            }
        }

        public FilesOperationsUC()
        {
            InitializeComponent();
            TextMessages = new List<string>();
            ProgressReporter = new Progress<AnalogyProgressReport>((value) =>
            {
                string status = $"{value.Prefix}:{value.FinishedProcessing}";
                TextMessages.Add(status);
                if (DateTime.Now.Subtract(LastReportUpdate).TotalMilliseconds > refreshDelay || (value.Processed == value.Total))
                {
                    if (IsDisposed)
                    {
                        return;
                    }

                    LastReportUpdate = DateTime.Now;
                    richTextBox1.Text += status + Environment.NewLine;
                    richTextBox1.SelectionStart = richTextBox1.Text.Length;
                    richTextBox1.ScrollToCaret();
                    progressBar1.Maximum = value.Total;
                    progressBar1.Value = value.Processed;
                    lblStatus.Text = $"Processed File {value.Processed} out of {value.Total}";
                }
            });
            TextProgressReporter = new Progress<string>(text =>
            {
                //if (DateTime.Now.Subtract(LastReportUpdate).TotalMilliseconds > refreshDelay)
                //{
                if (IsDisposed)
                {
                    return;
                }

                //LastReportUpdate = DateTime.Now;
                richTextBox1.Text += text + Environment.NewLine;
                richTextBox1.SelectionStart = richTextBox1.Text.Length;
                richTextBox1.ScrollToCaret();
                //}
            });
        }

        public FilesOperationsUC(List<string> filenames) : this()
        {
            FileNames = filenames;
        }

        public async Task ProcessFilesAndSearch(IAnalogyOfflineDataProvider dataProvider, string txtToSearch)
        {
            DataProvider = dataProvider;
            Aborted = false;
            sBtnAbort.Enabled = true;
            int processed = 0;
            foreach (string filename in FileNames)
            {
                if (Manager.AlreadyProcessed(Path.GetFileName(filename)))
                {
                    continue;
                }

                FileProcessor fp = new FileProcessor(this);
                await fp.Process(DataProvider, filename, cancellationTokenSource.Token);
                processed += 1;
                ProgressReporter.Report(new AnalogyProgressReport("Processed", processed, FileNames.Count, filename));
                if (Aborted)
                {
                    ProgressReporter.Report(new AnalogyProgressReport("Aborted", FileNames.Count, FileNames.Count, filename));
                    break;
                }
                var found = Messages.Where(entry => entry.Item2.Text.Contains(txtToSearch)).Select(e => e.Item1).Distinct()
                    .ToList();
                richTextBox1.Text = $"Found text: ({txtToSearch}) in the following files:";

                foreach (string file in found)
                {
                    richTextBox1.Text += Environment.NewLine + file;
                }
                Messages.Clear();
            }
        }


        public async Task ProcessFilesAndCombine(IAnalogyOfflineDataProvider dataProvider)
        {
            DataProvider = dataProvider;
            Aborted = false;
            sBtnAbort.Enabled = true;
            int processed = 0;
            foreach (string filename in FileNames)
            {
                if (Manager.AlreadyProcessed(Path.GetFileName(filename)))
                {

                    richTextBox1.Text += Environment.NewLine + $"File {filename} was already processed. getting data from cache";
                    continue;
                }

                FileProcessor fp = new FileProcessor(this);
                await fp.Process(DataProvider, filename, cancellationTokenSource.Token);
                processed += 1;
                ProgressReporter.Report(new AnalogyProgressReport("Processed", processed, FileNames.Count, filename));
                if (Aborted)
                {
                    ProgressReporter.Report(new AnalogyProgressReport("Aborted", FileNames.Count, FileNames.Count, filename));
                    break;
                }
            }


        }

        public void AppendMessage(AnalogyLogMessage message, string dataSource)
        {
            lock (lockObject)
            {
                Messages.Add(new Tuple<string, AnalogyLogMessage>(dataSource, message));
            }
        }

        public void AppendMessages(List<AnalogyLogMessage> messages, string dataSource)
        {

            lock (lockObject)
            {
                Messages.AddRange(messages.Select(m => new Tuple<string, AnalogyLogMessage>(dataSource, m)));
                Manager.DoneProcessingFile(messages, dataSource);
            }
        }

        public void AppendMessage(DataRow dtr, string dataSource)
        {
            AnalogyLogMessage message = (AnalogyLogMessage)dtr["Object"];
            AppendMessage(message, dataSource);
        }

        public Task LoadFilesAsync(List<string> fileNames, bool clearLogBeforeLoading)
        {
            throw new NotImplementedException();
        }

        public void SetFilesToProcess(List<string> files) => FileNames = files;

        public List<AnalogyLogMessage> GetMessages() => Messages.Select(m => m.Item2).ToList();

        private void sBtnAbort_Click(object sender, EventArgs e)
        {
            Aborted = true;
            sBtnAbort.Enabled = false;
        }

        public void CollapsePanel(bool top)
        {

            splitContainerControl1.PanelVisibility = top ? SplitPanelVisibility.Panel1 : SplitPanelVisibility.Panel2;

        }
    }
}




