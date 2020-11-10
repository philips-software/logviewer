using Analogy.DataTypes;
using Analogy.Interfaces;
using DevExpress.XtraEditors;
using DevExpress.XtraTreeList;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Analogy
{
    public partial class XtraUCFileSystem : XtraUserControl
    {
        public event EventHandler<FolderSelectionEventArgs>? FolderChanged;
        private string? _startupDrive;
        private bool _listFolders;
        private bool _listFiles;
        private IAnalogyOfflineDataProvider? DataProvider { get; set; }

        public XtraUCFileSystem() : this(false, false)
        {

        }
        public XtraUCFileSystem(bool listFoldersToLoad, bool listFilesToLoad)
        {
            _listFolders = listFoldersToLoad;
            _listFiles = listFilesToLoad;
            InitializeComponent();
            if (DesignMode)
            {
                return;
            }

            treeList1.DataSource = new object();

        }


        private void treeList1_CustomDrawNodeCell(object sender, CustomDrawNodeCellEventArgs e)
        {
            if (e.Column == colSize)
            {
                if (e.Node.GetDisplayText("Type") == "File")
                {
                    e.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                    e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Italic);
                    Int64 size = Convert.ToInt64(e.Node.GetValue("Size"));
                    e.CellText = size >= 1024 ? $"{size / 1024:### ### ###} KB" : $"{size} Bytes";
                }
                else
                {
                    e.CellText = $"<{e.Node.GetDisplayText("Type")}>";
                }
            }

            if (e.Column == colName)
            {
                if (e.Node.GetDisplayText("Type") == "File")
                {
                    e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
                }
            }
        }

        private void treeList1_GetStateImage(object sender, GetStateImageEventArgs e)
        {
            if (e.Node.GetDisplayText("Type") == "Folder")
            {
                e.NodeImageIndex = e.Node.Expanded ? 1 : 0;
            }
            else if (e.Node.GetDisplayText("Type") == "File")
            {
                e.NodeImageIndex = 2;
            }
            else
            {
                e.NodeImageIndex = 3;
            }
        }

        private void treeList1_VirtualTreeGetCellValue(object sender, VirtualTreeGetCellValueInfo e)
        {
            DirectoryInfo di = new DirectoryInfo((string)e.Node);
            if (e.Column == colName)
            {
                e.CellData = di.Name;
                return;
            }

            if (e.Column == colType)
            {
                if (IsDrive((string)e.Node))
                {
                    e.CellData = "Drive";
                }
                else if (!IsFile(di))
                {
                    e.CellData = "Folder";
                }
                else
                {
                    e.CellData = "File";
                }

                if (e.Column == colFullPath)
                {
                    e.CellData = (string)e.Node;
                }
                return;
            }

            if (e.Column == colSize)
            {
                if (IsFile(di))
                {
                    e.CellData = new FileInfo((string)e.Node).Length;
                }
                else
                {
                    e.CellData = null;
                }

                return;
            }

            if (e.Column == colFullPath)
            {
                e.CellData = (string)e.Node;
            }
        }

        bool IsFile(DirectoryInfo info)
        {
            try
            {
                return (info.Attributes & FileAttributes.Directory) == 0;
            }
            catch
            {
                return false;
            }
        }
        bool IsDrive(string val)
        {
            string[] drives = Directory.GetLogicalDrives();
            foreach (string drive in drives)
            {
                if (drive.Equals(val))
                {
                    return true;
                }
            }
            return false;
        }

        private void treeList1_VirtualTreeGetChildNodes(object sender, VirtualTreeGetChildNodesInfo e)
        {
            Cursor current = Cursor.Current!;
            Cursor.Current = Cursors.WaitCursor;
            {
                try
                {
                    string path = _startupDrive!;
                    if (e.Node is string node)
                    {
                        path = node;
                    }

                    if (Directory.Exists(path))
                    {
                        string[] dirs = new string[0];
                        if (_listFolders)
                        {
                            try
                            {
                                dirs = Directory.GetDirectories(path);
                            }
                            catch (Exception)
                            {
                                dirs = new string[0];
                            }
                        }

                        string[] files = _listFiles && DataProvider != null ? DataProvider.GetSupportedFiles(new DirectoryInfo(path), false).Select(f => f.Name).Distinct().ToArray() : new string[0];
                        string[] arr = new string[dirs.Length + files.Length];
                        if (_listFolders)
                        {
                            dirs.CopyTo(arr, 0);
                        }

                        if (!_listFolders)
                        {
                            files.CopyTo(arr, dirs.Length);
                        }

                        e.Children = arr;
                    }
                    else
                    {
                        e.Children = new object[] { };
                    }
                }
                catch { e.Children = new object[] { }; }
            }
            Cursor.Current = current;

        }

        public void SetPath(string path, IAnalogyOfflineDataProvider dataProvider)
        {
            DataProvider = dataProvider;
            UserSettingsManager.UserSettings.AddToRecentFolders(dataProvider.Id, path);
            _startupDrive = path;
            treeList1.ClearNodes();
            treeList1.DataSource = new object();
        }

        private void treeList1_FocusedNodeChanged(object sender, FocusedNodeChangedEventArgs e)
        {
            if (e.Node == null)
            {
                return;
            }

            FolderChanged?.Invoke(this, new FolderSelectionEventArgs(e.Node.GetDisplayText("Path")));
        }

        public void SetListing(bool listFolders, bool listFiles)
        {
            _listFolders = listFolders;
            _listFiles = listFiles;
            colType.Visible = false;
            if (listFolders & !listFiles)
            {
                colSize.Visible = false;
                treeList1.OptionsSelection.MultiSelect = false;
            }
            else
            {
                colSize.Visible = true;
                treeList1.OptionsSelection.MultiSelect = true;
                treeList1.OptionsSelection.MultiSelectMode = TreeListMultiSelectMode.CellSelect;
            }
        }
    }
}
