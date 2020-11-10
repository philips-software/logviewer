using System;
using System.IO;
using Analogy.DataTypes;

namespace Analogy.Forms
{
    public partial class XtraFormClientServer : DevExpress.XtraEditors.XtraForm
    {
        public XtraFormClientServer()
        {
            InitializeComponent();
        }

        public void UpdateUI()
        {
            if (rBtnNetwork.Checked)
            {
                lblPath.Text = "Network Path:";
            }
            else if (rBtnLocal.Checked)
            {
                lblPath.Text = "Local Path:";
            }
        }

        private void radioButtons_CheckedChanged(object sender, EventArgs e)
        {
            UpdateUI();
        }

        private void sBtnTest_Click(object sender, EventArgs e)
        {
            var valid = TestConnection();
            sBtnTest.Image = valid ? Properties.Resources.OK_32x32 : Properties.Resources.NotOk_32x32;
        }

        private bool TestConnection() => CheckPath(txtbPath.Text);


        private bool CheckPath(string path)
        {
            try
            {
                return Directory.Exists(path);
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void sBtnAdd_Click(object sender, EventArgs e)
        {
            var valid = TestConnection();
            if (valid)
            {
                if (rBtnNetwork.Checked)
                {
                    DataSource data = new DataSource(txtbPath.Text, txtbPath.Text, txtDisplayName.Text, DataSourceType.NetworkPath);
                    ClientServerDataSourceManager.Instance.Add(data);
                    return;
                }
                if (rBtnLocal.Checked)
                {
                    DataSource data = new DataSource(txtbPath.Text, txtbPath.Text, txtDisplayName.Text, DataSourceType.LocalPath);
                    ClientServerDataSourceManager.Instance.Add(data);
                }

                Close();

            }
        }

        private void XtraFormClientServer_Load(object sender, EventArgs e)
        {
            Icon = UserSettingsManager.UserSettings.GetIcon();
        }
    }
}