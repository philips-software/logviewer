using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using Analogy.DataTypes;
using Analogy.Forms;
using Analogy.Interfaces;
using Analogy.Managers;

namespace Analogy
{

    public partial class WindowsEventLog : UserControl
    {
        private List<EventLog> Custom = new List<EventLog>();
        public string SelectedPath { get; set; }

        public WindowsEventLog()
        {
            InitializeComponent();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            ucLogs1.ProcessCmdKeyFromParent(keyData);
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void WindowsEventLog_Load(object sender, EventArgs e)
        {
            if (DesignMode)
            {
                return;
            }

            ucLogs1.btswitchRefreshLog.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            ucLogs1.btsAutoScrollToBottom.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            //SetupLogs();
        }




        private void lBoxSources_SelectedValueChanged(object sender, EventArgs e)
        {
            if (lBoxSources.SelectedItem is DataSource data)
            {
                SelectedPath = data.Path;

            }
        }



        private void bBtnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            XtraFormWindowsEventlogsManager f = new XtraFormWindowsEventlogsManager();
            f.ShowDialog(this);
            //SetupLogs();
        }

        private void bBtnRemove_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (lBoxSources.SelectedItem is DataSource data)
            {
                if (XtraMessageBox.Show($"Are you sure you want to delete {data}?", "Confirmation Required",
                        MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    ClientServerDataSourceManager.Instance.Remove(data);
                    lBoxSources.Items.Remove(data);

                }
            }
        }

        private class Counter
        {
            public string Name { get; }
            private int Count { get; set; }

            public Counter(string name)
            {
                Name = name;
                Count = 0;
            }

            public void Increment() => Count++;
            public override string ToString()
            {
                return $"Log: {Name}. Messages: {Count}";
            }
        }

        private void lBoxSources_SelectedIndexChanged(object sender, EventArgs e)
        {
            Counter item = lBoxSources.SelectedItem as Counter;
            if (item == null)
            {
                return;
            }

            string module = item.Name == "All" ? string.Empty : item.Name;
            ucLogs1.FilterResults(module);
        }
    }
}


