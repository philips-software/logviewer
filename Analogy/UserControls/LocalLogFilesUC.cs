using Analogy.DataTypes;
using Analogy.Interfaces;
using DevExpress.XtraBars;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Analogy
{

    public partial class LocalLogFilesUC : UserControl
    {
        private List<string> extrenalFiles = new List<string>();
        public string SelectedPath { get; set; }
        private IAnalogyOfflineDataProvider DataProvider { get; }
        public ILogMessageCreatedHandler Handler => ucLogs1;
        //private List<string> TreeListFileNodes { get; set; }
        public LocalLogFilesUC(string? initSelectedPath = null)
        {
            SelectedPath = initSelectedPath ?? string.Empty;
            InitializeComponent();
            treeList1.Columns["colChanged"].SortOrder = SortOrder.Descending;
            treeList1.Appearance.HideSelectionRow.Assign(treeList1.ViewInfo.PaintAppearance.FocusedRow);
            ucLogs1.SetSaveButtonsVisibility(false);
        }

        public LocalLogFilesUC(IAnalogyOfflineDataProvider dataProvider, string[]? fileNames = null, string? initialSelectedPath = null) : this(initialSelectedPath)
        {

            DataProvider = dataProvider;
            if (fileNames != null)
            {
                extrenalFiles.AddRange(fileNames);
            }

            ucLogs1.SetFileDataSource(dataProvider, dataProvider);
        }

        public LocalLogFilesUC(IAnalogyDataProvider dataProvider, CancellationTokenSource cts) : this()
        {
            ucLogs1.SetFileDataSource(dataProvider, null);
            ucLogs1.CancellationTokenSource = cts;
        }

        public void ShowFolderAndFilesPanel(bool on) => spltMain.Panel1Collapsed = !on;
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            ucLogs1.ProcessCmdKeyFromParent(keyData);
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private async void OfflineUCLogs_Load(object sender, EventArgs e)
        {
            if (DesignMode)
            {
                return;
            }

            folderTreeViewUC1.FolderChanged += FolderTreeViewUC1_FolderChanged;
            ucLogs1.btswitchRefreshLog.Visibility = BarItemVisibility.Never;
            ucLogs1.btsAutoScrollToBottom.Visibility = BarItemVisibility.Never;
            if (extrenalFiles.Any())
            {
                if (File.Exists(extrenalFiles.First()))
                {
                    SelectedPath = Path.GetDirectoryName(extrenalFiles.First());
                }
            }

            folderTreeViewUC1.SetFolder(SelectedPath, DataProvider);
            PopulateFiles(SelectedPath);

            if (extrenalFiles.Any())
            {
                await LoadFilesAsync(extrenalFiles, false);
            }
            ucLogs1.OnFocusedRowChanged += UcLogs1_OnFocusedRowChanged;
        }

        private void UcLogs1_OnFocusedRowChanged(object sender, (string file, AnalogyLogMessage e) data)
        {
            var t = treeList1.Nodes.FirstOrDefault(n => data.file.Contains(n["Path"].ToString(), StringComparison.InvariantCultureIgnoreCase));
            if (t != null && treeList1.FocusedNode != t)
            {
                treeList1.SelectionChanged -= TreeList1_SelectionChanged;
                treeList1.Selection.Clear();
                treeList1.FocusedNode = null;
                t.Selected = true;
                treeList1.SetFocusedNode(t);
                treeList1.Selection.Add(t);
                treeList1.SelectionChanged += TreeList1_SelectionChanged;
            }
        }

        private void FolderTreeViewUC1_FolderChanged(object sender, FolderSelectionEventArgs e)
        {
            if (Directory.Exists(e.SelectedFolderPath))
            {
                PopulateFiles(e.SelectedFolderPath);
            }
        }

        private void AnalogyUCLogs_DragEnter(object sender, DragEventArgs e) =>
            e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Copy : DragDropEffects.None;
        private async void AnalogyUCLogs_DragDrop(object sender, DragEventArgs e)
        {
            if (DataProvider == null)
            {
                return;
            }

            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            await LoadFilesAsync(files.ToList(), chkbSelectionMode.Checked);
        }

        private void PopulateFiles(string folder)
        {
            if (string.IsNullOrEmpty(folder) || !Directory.Exists(folder) || DataProvider == null)
            {
                return;
            }

            SelectedPath = folder;
            treeList1.SelectionChanged -= TreeList1_SelectionChanged;
            bool recursiveLoad = checkEditRecursiveLoad.Checked;
            DirectoryInfo dirInfo = new DirectoryInfo(folder);
            UserSettingsManager.UserSettings.AddToRecentFolders(DataProvider.Id, folder);
            List<FileInfo> fileInfos = DataProvider.GetSupportedFiles(dirInfo, recursiveLoad).Distinct(new FileInfoComparer()).ToList();
            treeList1.Nodes.Clear();
            // TreeListFileNodes.Clear();
            foreach (FileInfo fi in fileInfos)
            {
                treeList1.Nodes.Add(fi.Name, fi.LastWriteTime, fi.Length, fi.FullName);
                // TreeListFileNodes.Add(fi.FullName);
            }

            treeList1.BestFitColumns();
            treeList1.ClearSelection();

            treeList1.SelectionChanged += TreeList1_SelectionChanged;
        }

        private async Task LoadFilesAsync(List<string> fileNames, bool clearLog)
        {
            ucLogs1.OnFocusedRowChanged -= UcLogs1_OnFocusedRowChanged;
            await ucLogs1.LoadFilesAsync(fileNames, clearLog);
            ucLogs1.OnFocusedRowChanged += UcLogs1_OnFocusedRowChanged;

        }

        private void bBtnOpen_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (treeList1.Selection.Any())
            {
                var filename = (string)treeList1.Selection.First().GetValue(colFullPath);
                if (filename == null || !File.Exists(filename))
                {
                    return;
                }

                Process.Start("explorer.exe", "/select, \"" + filename + "\"");
            }
        }

        private void bBtnDelete_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (treeList1.Selection.Any())
            {
                var filename = (string)treeList1.Selection.First().GetValue(colFullPath);
                if (filename == null || !File.Exists(filename))
                {
                    return;
                }

                var result = MessageBox.Show($"Are you sure you want to delete {filename}?", "Delete confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
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

        private void bBtnRefresh_ItemClick(object sender, ItemClickEventArgs e)
        {
            PopulateFiles(SelectedPath);
        }

        private void bBtnSelectAll_ItemClick(object sender, ItemClickEventArgs e)
        {
            treeList1.SelectAll();
        }

        private async void TreeList1_SelectionChanged(object sender, EventArgs e)
        {
            List<string> files = treeList1.Selection.Select(node => (string)node.GetValue(colFullPath)).ToList();
            await LoadFilesAsync(files, chkbSelectionMode.Checked);
        }

    }

}


