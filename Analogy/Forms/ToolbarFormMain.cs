namespace Analogy.Forms
{
    public partial class ToolbarFormMain : DevExpress.XtraEditors.XtraForm
    {
        public ToolbarFormMain()
        {
            InitializeComponent();
        }

        private void ToolbarFormMain_Load(object sender, System.EventArgs e)
        {
            Icon = UserSettingsManager.UserSettings.GetIcon();
        }
    }
}