using Analogy.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using Analogy.DataTypes;

namespace Analogy
{
    public partial class FolderTreeViewUC : UserControl
    {
        public event EventHandler<FolderSelectionEventArgs> FolderChanged;
        private string SelectedPath { get; set; } = string.Empty;
        private IAnalogyOfflineDataProvider DataProvider { get; set; }
        public FolderTreeViewUC()
        {
            InitializeComponent();
            xtraUCFileSystem1.SetListing(true, false);
            xtraUCFileSystem1.FolderChanged += (s, e) => FolderChanged?.Invoke(this, new FolderSelectionEventArgs(e.SelectedFolderPath));
        }

        private async void txtbFolder_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && Directory.Exists(txtbFolder.Text))
            {
                await PopulateFolders(txtbFolder.Text, DataProvider);
            }
        }
        private async Task PopulateFolders(string folderText, IAnalogyOfflineDataProvider dataProvider)
        {
            if (Directory.Exists(folderText))
            {
                xtraUCFileSystem1.SetPath(folderText, dataProvider);
                txtbFolder.Text = folderText;
                FolderChanged?.Invoke(this, new FolderSelectionEventArgs(folderText));
                SetRecentFolderList();
            }
            else
            {
                await Task.CompletedTask;
            }
        }

        private async void FolderTreeViewUC_Load(object sender, EventArgs e)
        {
            txtbFolder.Text = SelectedPath;
            if (Directory.Exists(txtbFolder.Text))
            {
                await PopulateFolders(txtbFolder.Text, DataProvider);
            }
        }

        private async void btnOpenFolder_Click(object sender, EventArgs e)
        {
#if NETCOREAPP3_1
            var folderBrowserDialog1 = new FolderBrowserDialog();
            folderBrowserDialog1.SelectedPath = SelectedPath;
            if (DialogResult.OK == folderBrowserDialog1.ShowDialog(this))
            {
                SelectedPath = folderBrowserDialog1.SelectedPath;
                await PopulateFolders(SelectedPath, DataProvider);
            }
#else
            using (var dialog = new Microsoft.WindowsAPICodePack.Dialogs.CommonOpenFileDialog())
            {
                dialog.InitialDirectory = SelectedPath;
                dialog.IsFolderPicker = true;
                if (dialog.ShowDialog() == Microsoft.WindowsAPICodePack.Dialogs.CommonFileDialogResult.Ok)
                {
                    SelectedPath = dialog.FileName;
                    await PopulateFolders(SelectedPath, DataProvider);
                }
            }
#endif
        }

        public void SetFolder(string folder, IAnalogyOfflineDataProvider dataProvider)
        {
            this.DataProvider = dataProvider;
            if (string.IsNullOrEmpty(folder) || !Directory.Exists(folder))
            {
                return;
            }

            SelectedPath = folder;
            txtbFolder.Text = folder;
            xtraUCFileSystem1.SetPath(folder, dataProvider);
            SetRecentFolderList();
        }

        private void SetRecentFolderList()
        {
            cmsFolders.Items.Clear();

            foreach (var folder in UserSettingsManager.UserSettings.GetRecentFolders(DataProvider.Id))
            {
                cmsFolders.Items.Add(folder.Path, null, (_, __) => SetFolder(folder.Path, DataProvider));
            }
        }

        private void sbtnRecent_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                cmsFolders.Show(sbtnRecent.PointToScreen(e.Location));
            }
        }
    }


}
