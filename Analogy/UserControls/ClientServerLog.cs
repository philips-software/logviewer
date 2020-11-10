using Analogy.Interfaces;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Analogy.DataTypes;
using Analogy.Forms;

namespace Analogy
{

    public partial class ClientServerUCLog : UserControl
    {
        public string SelectedPath { get; set; }
        private IAnalogyOfflineDataProvider DataProvider { get; }
        public ClientServerUCLog()
        {
            InitializeComponent();
        }

        public ClientServerUCLog(IAnalogyOfflineDataProvider dataProvider) : this()
        {
            DataProvider = dataProvider;
            ucLogs1.SetFileDataSource(dataProvider, DataProvider);
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            ucLogs1.ProcessCmdKeyFromParent(keyData);
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void ClientServerUCLog_Load(object sender, EventArgs e)
        {
            if (DesignMode)
            {
                return;
            }

            lBoxSources.DataSource = ClientServerDataSourceManager.Instance.DataSources;
            ucLogs1.btswitchRefreshLog.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            ucLogs1.btsAutoScrollToBottom.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            lBoxSources.SelectedIndex = -1;
            lBoxSources.SelectedValueChanged += lBoxSources_SelectedValueChanged;
        }

        private async void lBoxFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            await LoadFilesAsync(lBoxFiles.SelectedItems.Cast<FileInfo>().Select(f => f.FullName).ToList(), chkbSelectionMode.Checked);

        }
        private void PopulateFiles(string folder)
        {
            if (!Directory.Exists(folder))
            {
                return;
            }

            lBoxFiles.SelectedIndexChanged -= lBoxFiles_SelectedIndexChanged;
            bool recursiveLoad = checkEditRecursiveLoad.Checked;
            UserSettingsManager.UserSettings.AddToRecentFolders(DataProvider.Id, folder);
            DirectoryInfo dirInfo = new DirectoryInfo(folder);
            var fileInfos = DataProvider.GetSupportedFiles(dirInfo, recursiveLoad).Distinct(new FileInfoComparer()).OrderByDescending(f => f.LastWriteTime);
            lBoxFiles.DisplayMember = recursiveLoad ? "FullName" : "Name";
            lBoxFiles.DataSource = fileInfos;
            lBoxFiles.SelectedIndexChanged += lBoxFiles_SelectedIndexChanged;

        }


        public async Task LoadFilesAsync(List<string> fileNames, bool clearLog)
        {
            await ucLogs1.LoadFilesAsync(fileNames, clearLog);

        }

        private void lBoxSources_SelectedValueChanged(object sender, EventArgs e)
        {
            if (lBoxSources.SelectedItem is DataSource data)
            {
                SelectedPath = data.Path;
                PopulateFiles(data.Path);
            }
        }


        private void bBtnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            XtraFormClientServer f = new XtraFormClientServer();
            f.ShowDialog(this);
            lBoxSources.DataSource = ClientServerDataSourceManager.Instance.DataSources;
        }

        private void bBtnRemove_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (lBoxSources.SelectedItem is DataSource data)
            {
                if (XtraMessageBox.Show($"Are you sure you want to delete {data}?", "Confirmation Required", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    ClientServerDataSourceManager.Instance.Remove(data);
                    lBoxSources.Items.Remove(data);

                }
            }
        }

        private void bBtnOpenFolder_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (lBoxFiles.SelectedItem != null)
            {
                var filename = (lBoxFiles.SelectedItem as FileInfo)?.FullName;
                if (filename == null || !File.Exists(filename))
                {
                    return;
                }

                try
                {
                    Process.Start("explorer.exe", "/select, \"" + filename + "\"");
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message, @"Error Opening file location", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }

        private void bBtnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (lBoxFiles.SelectedItem != null)
            {
                var filename = (lBoxFiles.SelectedItem as FileInfo)?.FullName;
                if (filename == null)
                {
                    return;
                }

                var result = XtraMessageBox.Show($"Are you sure you want to delete {filename}?", "Delete confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (result == DialogResult.Yes)
                {
                    if (File.Exists(filename))
                    {
                        try
                        {
                            File.Delete(filename);
                            PopulateFiles(SelectedPath);
                        }
                        catch (Exception exception)
                        {
                            MessageBox.Show(exception.Message, @"Error deleting file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        private void bBtnRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (string.IsNullOrEmpty(SelectedPath) || !Directory.Exists(SelectedPath))
            {
                return;
            }

            PopulateFiles(SelectedPath);
        }
    }

}


