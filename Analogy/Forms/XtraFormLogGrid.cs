using Analogy.DataProviders;
using Analogy.Interfaces;
using Analogy.Managers;
using System.Collections.Generic;
using System.Linq;

namespace Analogy.Forms
{
    public partial class XtraFormLogGrid : DevExpress.XtraEditors.XtraForm
    {
        private readonly List<AnalogyLogMessage> _messages;
        private readonly string _dataSource;
        public XtraFormLogGrid()
        {
            InitializeComponent();
            _messages = new List<AnalogyLogMessage>();
            _dataSource = "Analogy";
            FactoryContainer analogy = FactoriesManager.Instance.GetBuiltInFactoryContainer(AnalogyBuiltInFactory.AnalogyGuid);
            var analogyDataProvider = analogy.DataProvidersFactories.First().DataProviders.First();
            ucLogs1.SetFileDataSource(analogyDataProvider, null);
        }
        public XtraFormLogGrid(List<AnalogyLogMessage> messages, string dataSource) : this()
        {
            _messages = messages;
            _dataSource = dataSource;
        }


        public XtraFormLogGrid(List<AnalogyLogMessage> messages, string dataSource, IAnalogyDataProvider dataProvider, IAnalogyOfflineDataProvider? fileProvider = null, string? processOrModule = null)
        {
            InitializeComponent();
            _messages = messages;
            _dataSource = dataSource;
            if (!string.IsNullOrEmpty(processOrModule))
            {
                ucLogs1.FilterResults(processOrModule!);
            }

            ucLogs1.SetFileDataSource(dataProvider, fileProvider);


        }

        private void XtraFormLogGrid_Load(object sender, System.EventArgs e)
        {
            Icon = UserSettingsManager.UserSettings.GetIcon();
            if (DesignMode)
            {
                return;
            }

            if (!_messages.Any())
            {
                return;
            }

            {
                ucLogs1.AppendMessages(_messages, _dataSource);
            }
        }

        public void AppendMessage(AnalogyLogMessage message, string dataSource) =>
            ucLogs1.AppendMessage(message, dataSource);
        public void AppendMessages(List<AnalogyLogMessage> messages, string dataSource) =>
            ucLogs1.AppendMessages(messages, dataSource);

    }
}