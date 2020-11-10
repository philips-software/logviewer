using Analogy.Interfaces.DataTypes;
using Analogy.Managers;
using DevExpress.XtraEditors;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Analogy.UserControls
{
    public partial class ComponentDownloadInformationUC : XtraUserControl
    {
        private FactoryContainer? Factory { get; }
        private IAnalogyDownloadInformation? DownloadInfo { get; }

        public ComponentDownloadInformationUC()
        {
            InitializeComponent();
            btnCheckNow.Click += async (s, e) =>
            {
                if (DownloadInfo != null)
                {
                    await DownloadInfo.CheckVersion();
                }
                UpdateLatestVersionText();


            };
            btnDownload.Click += async (s, e) =>
            {
                if (DownloadInfo != null && DownloadInfo.IsUpdateAvailable && DownloadInfo.DownloadURL != null)
                {
                    btnDownload.Enabled = false;
                    await UpdateManager.Instance.InitiateUpdate(DownloadInfo.Name, DownloadInfo.DownloadURL);
                    btnDownload.Enabled = true;
                }
                else
                {
                    XtraMessageBox.Show("Download file was not found.", "", MessageBoxButtons.OK);
                }
            };
        }


        public ComponentDownloadInformationUC(FactoryContainer factory) : this()
        {
            Factory = factory;
            DownloadInfo = Factory.DownloadInformation!;
        }

        public ComponentDownloadInformationUC(IAnalogyDownloadInformation downloadInfo):this()
        {
            DownloadInfo = downloadInfo;
        }
        private void ComponentDownloadInformationUC_Load(object sender, EventArgs e)
        {
            if (Factory != null)
            {
                if (Factory.Factory.LargeImage != null)
                {
                    picture.Image = Factory.Factory.LargeImage;
                }

                lblTitle.Text = Factory.Factory.Title;
            }
            else
            {
                lblTitle.Text = DownloadInfo?.Name;
            }

            lblCurrentVersion.Text = DownloadInfo?.InstalledVersion > new Version(0, 0, 0)
                    ? DownloadInfo.InstalledVersion.ToString()
                    : "N/A";
            UpdateLatestVersionText();
        }

        private void UpdateLatestVersionText()
        {
            if (DownloadInfo?.LatestVersion != null)
            {
                lblLatestVersion.Text = "released Version: " + DownloadInfo.LatestVersion.ToString();
                if (DownloadInfo.LatestVersion >= DownloadInfo.InstalledVersion || DownloadInfo.InstalledVersion.Equals(new Version(0, 0, 0)))
                {
                    lblLatestVersion.Appearance.BackColor = Color.GreenYellow;
                    btnDownload.Visible = true;
                }

            }
        }


    }
}
