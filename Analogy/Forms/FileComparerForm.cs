using Analogy.Interfaces;

namespace Analogy.Forms
{
    public partial class FileComparerForm : DevExpress.XtraEditors.XtraForm
    {
        private IAnalogyOfflineDataProvider offlineAnalogy;

        private FileComparerForm()
        {
            InitializeComponent();
        }

        public FileComparerForm(IAnalogyOfflineDataProvider offlineAnalogy) : this()
        {
            this.offlineAnalogy = offlineAnalogy;
            logsComparerUC1.SetDataSource(offlineAnalogy);
        }

        private void FileComparerForm_Load(object sender, System.EventArgs e)
        {
            Icon = UserSettingsManager.UserSettings.GetIcon();
        }
    }
}