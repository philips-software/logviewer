using Analogy.Properties;
using DevExpress.XtraEditors;
using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Analogy.Updater
{
   internal partial class DownloadUpdateDialog : XtraForm
    {
        public static IWebProxy? Proxy;
        private readonly string _downloadURL;
        private readonly string _targetFolder;
        private readonly TargetFrameworkAttribute _currentFrameworkAttribute;
        private readonly TaskCompletionSource<bool> _taskCompletionSource;

        private string _tempFile;
        public static BasicAuthentication? BasicAuthDownload { get; set; }

        private MyWebClient? _webClient;

        private DateTime _startedAt;

        public DownloadUpdateDialog(string downloadURL, string targetFolder,
            TargetFrameworkAttribute currentFrameworkAttribute, TaskCompletionSource<bool> taskCompletionSource)
        {
            InitializeComponent();
            _downloadURL = downloadURL;
            _targetFolder = targetFolder;
            _currentFrameworkAttribute = currentFrameworkAttribute;
            _taskCompletionSource = taskCompletionSource;
            _tempFile = Path.Combine(Path.GetTempPath(), "Analogy.Updater.zip"); 
            Icon = UserSettingsManager.UserSettings.GetIcon();

        }

        private void DownloadUpdateDialogLoad(object sender, EventArgs e)
        {
            _webClient = new MyWebClient
            {
                CachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore)
            };

            if (Proxy != null)
            {
                _webClient.Proxy = Proxy;
            }

            var uri = new Uri(_downloadURL);


            if (BasicAuthDownload != null)
            {
                _webClient.Headers[HttpRequestHeader.Authorization] = BasicAuthDownload.ToString();
            }

            _webClient.DownloadProgressChanged += OnDownloadProgressChanged;

            _webClient.DownloadFileCompleted += WebClientOnDownloadFileCompleted;

            _webClient.DownloadFileAsync(uri, _tempFile);

            void WebClientOnDownloadFileCompleted(object sender, AsyncCompletedEventArgs asyncCompletedEventArgs)
            {
                if (asyncCompletedEventArgs.Cancelled)
                {
                    _taskCompletionSource.SetResult(false);
                    return;
                }

                UnzipZipFileIntoTempFolder(_tempFile, _targetFolder);
                if (asyncCompletedEventArgs.Error != null)
                {
                    XtraMessageBox.Show(asyncCompletedEventArgs.Error.Message,
                        asyncCompletedEventArgs.Error.GetType().ToString(), MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                }
                _webClient?.Dispose();
                DialogResult = DialogResult.OK;
                _taskCompletionSource.SetResult(true);
                Close();
            }


        }

        private void UnzipZipFileIntoTempFolder(string zipPath, string extractPath)
        {
            string version = "net472";
            if (_currentFrameworkAttribute.FrameworkName.EndsWith("4.7.1") ||
                _currentFrameworkAttribute.FrameworkName.EndsWith("4.7.2"))
            {
                version = "net472";
            }
            else if (_currentFrameworkAttribute.FrameworkName.EndsWith("3.1"))
            {
                version = "netcoreapp3.1";
            }

            using (FileStream zipToOpen = new FileStream(zipPath, FileMode.Open))
            {
                using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Read))
                {
                    //build a list of files to be extracted
                    var entries = archive.Entries.Where(entry =>
                        !entry.FullName.EndsWith("/") && entry.FullName.Contains(version));
                    foreach (ZipArchiveEntry entry in entries)
                    {
                        string target = Path.Combine(extractPath, entry.Name);
                        string directory = Path.GetDirectoryName(target);
                        if (!Directory.Exists(directory))
                        {
                            Directory.CreateDirectory(directory);
                        }

                        try
                        {
                            entry.ExtractToFile(target, true);
                        }
                        catch (Exception e)
                        {
                            AnalogyLogger.Instance.LogException($"Error unpacking Updater: {e.Message}", e);
                        }

                    }
                }
            }
        }


        private void OnDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            if (_startedAt == default)
            {
                _startedAt = DateTime.Now;
            }
            else
            {
                var timeSpan = DateTime.Now - _startedAt;
                long totalSeconds = (long)timeSpan.TotalSeconds;
                if (totalSeconds > 0)
                {
                    var bytesPerSecond = e.BytesReceived / totalSeconds;
                    labelInformation.Text =
                        string.Format(Resources.DownloadSpeedMessage, BytesToString(bytesPerSecond));
                }
            }

            labelSize.Text = $@"{BytesToString(e.BytesReceived)} / {BytesToString(e.TotalBytesToReceive)}";
            progressBar.Value = e.ProgressPercentage;
        }

        private static string BytesToString(long byteCount)
        {
            string[] suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" };
            if (byteCount == 0)
            {
                return "0" + suf[0];
            }

            long bytes = Math.Abs(byteCount);
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return $"{(Math.Sign(byteCount) * num).ToString(CultureInfo.InvariantCulture)} {suf[place]}";
        }

        private static string TryToFindFileName(string contentDisposition, string lookForFileName)
        {
            string fileName = string.Empty;
            if (!string.IsNullOrEmpty(contentDisposition))
            {
                var index = contentDisposition.IndexOf(lookForFileName, StringComparison.CurrentCultureIgnoreCase);
                if (index >= 0)
                {
                    fileName = contentDisposition.Substring(index + lookForFileName.Length);
                }

                if (fileName.StartsWith("\""))
                {
                    var file = fileName.Substring(1, fileName.Length - 1);
                    var i = file.IndexOf("\"", StringComparison.CurrentCultureIgnoreCase);
                    if (i != -1)
                    {
                        fileName = file.Substring(0, i);
                    }
                }
            }

            return fileName;
        }

        private void DownloadUpdateDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_webClient == null)
            {
                DialogResult = DialogResult.Cancel;
            }
            else if (_webClient.IsBusy)
            {
                _webClient.CancelAsync();
                DialogResult = DialogResult.Cancel;
            }
            else
            {
                DialogResult = DialogResult.OK;
            }
        }
    }

    /// <inheritdoc />
    public class MyWebClient : WebClient
    {
        /// <summary>
        ///     Response Uri after any redirects.
        /// </summary>
        public Uri ResponseUri;

        /// <inheritdoc />
        protected override WebResponse GetWebResponse(WebRequest request, IAsyncResult result)
        {
            WebResponse webResponse = base.GetWebResponse(request, result);
            ResponseUri = webResponse.ResponseUri;
            return webResponse;
        }
    }

    public class BasicAuthentication
    {
        private string Username { get; }

        private string Password { get; }

        /// <summary>
        /// Initializes credentials for Basic Authentication.
        /// </summary>
        /// <param name="username">Username to use for Basic Authentication</param>
        /// <param name="password">Password to use for Basic Authentication</param>
        public BasicAuthentication(string username, string password)
        {
            Username = username;
            Password = password;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            var token = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{Username}:{Password}"));
            return $"Basic {token}";
        }
    }
}