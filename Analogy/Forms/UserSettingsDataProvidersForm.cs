using System;
using System.Linq;
using System.Windows.Forms;
using Analogy.Interfaces;
using Analogy.Managers;
using Analogy.Properties;
using DevExpress.XtraEditors;
using DevExpress.XtraTab;

namespace Analogy.Forms
{

    public partial class UserSettingsDataProvidersForm : XtraForm
    {
        private struct RealTimeCheckItem
        {
            public string Name;
            public Guid ID;

            public RealTimeCheckItem(string name, Guid id)
            {
                Name = name;
                ID = id;
            }

            public override string ToString() => $"{Name} ({ID})";
        }

        private UserSettingsManager Settings => UserSettingsManager.UserSettings;
        private readonly int _initialSelection = -1;

        public UserSettingsDataProvidersForm()
        {
            InitializeComponent();

        }

        public UserSettingsDataProvidersForm(int tabIndex) : this()
        {
            _initialSelection = tabIndex;
        }

        public UserSettingsDataProvidersForm(string tabName) : this()
        {
            var tab = tabControlMain.TabPages.SingleOrDefault(t => t.Name == tabName);
            if (tab != null)
            {
                _initialSelection = tab.TabIndex;
            }
        }

        private void LoadSettings()
        {

            AddExternalUserControlSettings();
        }

        private void AddExternalUserControlSettings()
        {
            foreach (IAnalogyDataProviderSettings settings in FactoriesManager.Instance.GetProvidersSettings())
            {
                XtraTabPage tab = new XtraTabPage();
                tab.Text = settings.Title;
                UserControl uc = settings.DataProviderSettings;
                tab.Controls.Add(uc);
                tab.Image = settings.LargeImage ?? Resources.Technology_32x32;
                //ab.
                uc.Dock = DockStyle.Fill;
                tabControlMain.TabPages.Add(tab);
            }
        }

        private void UserSettingsForm_Load(object sender, EventArgs e)
        {
            Icon = UserSettingsManager.UserSettings.GetIcon();
            LoadSettings();

            if (_initialSelection >= 0)
            {
                tabControlMain.SelectedTabPageIndex = _initialSelection;
            }
        }

        private void UserSettingsDataProvidersForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveSetting();
        }

        public async void SaveSetting()
        {
            foreach (IAnalogyDataProviderSettings settings in FactoriesManager.Instance.GetProvidersSettings())
            {
                try
                {
                    await settings.SaveSettingsAsync();
                }
                catch (Exception e)
                {
                    AnalogyLogManager.Instance.LogError("Error during call for SaveSetting for data provider: " + e, nameof(SaveSetting));
                }

            }
        }
    }
}
