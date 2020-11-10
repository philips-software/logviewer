using Analogy.Managers;
using System;

namespace Analogy.Forms
{
    public partial class UpdateForm : DevExpress.XtraEditors.XtraForm
    {
        private UpdateManager Updater => UpdateManager.Instance;
        private UserSettingsManager Settings => UserSettingsManager.UserSettings;

        public UpdateForm()
        {
            InitializeComponent();
        }

        private void UpdateForm_Load(object sender, EventArgs e)
        {
            Icon = Settings.GetIcon();
            lblCurrentVersion.Text =
                $"Your current version is: V{Updater.CurrentVersion}. (Target Framework:{Updater.CurrentFrameworkAttribute.FrameworkName})";
            lblLatestVersion.Text =
                $"Latest version is: {(Updater.LastVersionChecked?.TagName == null ? "not checked" : Updater.LastVersionChecked.TagName)}";

            if (Updater.LastVersionChecked != null && Updater.LastVersionChecked.TagName != null)
            {
                richTextBoxRelease.Text = Updater.LastVersionChecked.ToString();
                hyperLinkEditLatest.Text = Updater.LastVersionChecked.HtmlUrl;
                if (Updater.NewVersionExist)
                {
                    sbtnUpdateNow.Visible = true;
                }
            }
        }

        private async void sbtnCheckUpdate_Click(object sender, EventArgs e)
        {
            var (_, release) = await Updater.CheckVersion(true);
            Settings.LastVersionChecked = release;
            lblLatestVersion.Text = $"Latest version is: {release.TagName}.";
            richTextBoxRelease.Text = release.ToString();
            hyperLinkEditLatest.Text = release.HtmlUrl;
            if (Updater.NewVersionExist)
            {
                sbtnUpdateNow.Visible = true;
            }
        }

        private async void sbtnUpdateNow_Click(object sender, EventArgs e)
        {
            var downloadInfo = Updater.DownloadInformation;
            sbtnUpdateNow.Enabled = false;
            await Updater.InitiateUpdate(downloadInfo.title, downloadInfo.DownloadURL);
            sbtnUpdateNow.Enabled = true;
        }
    }
}
