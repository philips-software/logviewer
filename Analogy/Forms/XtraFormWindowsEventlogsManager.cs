namespace Analogy.Forms
{
    public partial class XtraFormWindowsEventlogsManager : DevExpress.XtraEditors.XtraForm
    {
        public XtraFormWindowsEventlogsManager()
        {
            InitializeComponent();
        }

        private void XtraFormWindowsEventlogsManager_Load(object sender, System.EventArgs e)
        {
            Icon = UserSettingsManager.UserSettings.GetIcon();
        }
    }
}