using Analogy.Interfaces;
using DevExpress.XtraEditors;

namespace Analogy.Forms
{
    public partial class SearchForm : XtraForm
    {
        private IAnalogyOfflineDataProvider offlineAnalogy;

        private SearchForm()
        {
            InitializeComponent();
        }

        public SearchForm(IAnalogyOfflineDataProvider offlineAnalogy) : this()
        {
            this.offlineAnalogy = offlineAnalogy;
            searchInFilesUC1.SetDataSource(offlineAnalogy);
        }

        private void SearchForm_Load(object sender, System.EventArgs e)
        {
            Icon = UserSettingsManager.UserSettings.GetIcon();
        }
    }
}
