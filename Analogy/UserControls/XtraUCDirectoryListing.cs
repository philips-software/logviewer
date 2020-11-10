using DevExpress.XtraEditors;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Analogy.DataTypes;

namespace Analogy
{
    public partial class XtraUCDirectoryListing : XtraUserControl
    {
        public event EventHandler<FolderSelectionEventArgs> FolderChanged;
        bool loadDrives = true;
        private string startupDrive;

        public XtraUCDirectoryListing() : this(true)
        {

        }
        public XtraUCDirectoryListing(bool localDriveOnly)
        {
            loadDrives = localDriveOnly;
            InitializeComponent();
            if (DesignMode)
            {
                return;
            }

            treeList1.DataSource = new object();

        }


        private void treeList1_CustomDrawNodeCell(object sender, DevExpress.XtraTreeList.CustomDrawNodeCellEventArgs e)
        {
            if (e.Column == this.colSize)
            {
                if (e.Node.GetDisplayText("Type") == "File")
                {
                    e.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                    e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Italic);
                    Int64 size = Convert.ToInt64(e.Node.GetValue("Size"));
                    if (size >= 1024)
                    {
                        e.CellText = string.Format("{0:### ### ###} KB", size / 1024);
                    }
                    else
                    {
                        e.CellText = string.Format("{0} Bytes", size);
                    }
                }
                else
                {
                    e.CellText = String.Format("<{0}>", e.Node.GetDisplayText("Type"));
                }
            }

            if (e.Column == this.colName)
            {
                if (e.Node.GetDisplayText("Type") == "File")
                {
                    e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
                }
            }
        }

        private void treeList1_GetStateImage(object sender, DevExpress.XtraTreeList.GetStateImageEventArgs e)
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

        private void treeList1_VirtualTreeGetCellValue(object sender, DevExpress.XtraTreeList.VirtualTreeGetCellValueInfo e)
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

        private void treeList1_VirtualTreeGetChildNodes(object sender, DevExpress.XtraTreeList.VirtualTreeGetChildNodesInfo e)
        {
            Cursor current = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;
            if (!loadDrives)
            {
                string[] roots = Directory.GetLogicalDrives();
                e.Children = roots;
                loadDrives = true;
            }
            else
            {
                try
                {
                    string path = startupDrive;
                    if (e.Node is string node)
                    {
                        path = node;
                    }

                    if (Directory.Exists(path))
                    {
                        string[] dirs = Directory.GetDirectories(path);
                        string[] files = Directory.GetFiles(path);
                        string[] arr = new string[dirs.Length + files.Length];
                        dirs.CopyTo(arr, 0);
                        files.CopyTo(arr, dirs.Length);
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

        private void treeList1_SelectionChanged(object sender, EventArgs e)
        {

        }

        public void SetPath(string path)
        {
            startupDrive = path;
            treeList1.ClearNodes();
            treeList1.DataSource = new object();
        }

        private void treeList1_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            if (e.Node == null)
            {
                return;
            }

            FolderChanged?.Invoke(this, new FolderSelectionEventArgs(e.Node.GetDisplayText("Path")));
        }
    }
}
