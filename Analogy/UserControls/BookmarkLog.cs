using System;
using System.Windows.Forms;
using DevExpress.XtraBars;

namespace Analogy
{

    public partial class BookmarkLog : UserControl
    {
        public BookmarkLog()
        {
            InitializeComponent();
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            ucLogs1.ProcessCmdKeyFromParent(keyData);
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private async void BookmarkLog_Load(object sender, EventArgs e)
        {
            if (DesignMode)
            {
                return;
            }

            ucLogs1.btswitchRefreshLog.Visibility = BarItemVisibility.Never;
            ucLogs1.btsAutoScrollToBottom.Visibility = BarItemVisibility.Never;
            ucLogs1.SetBookmarkMode();
            var messages = await BookmarkPersistManager.Instance.GetMessages();
            ucLogs1.AppendMessages(messages, "Analogy bookmarks");
            BookmarkPersistManager.Instance.MessageReceived += (s, msg) => ucLogs1.AppendMessage(msg.Message, msg.DataSource);
            BookmarkPersistManager.Instance.MessageRemoved += (s, msg) => ucLogs1.RemoveMessage(msg.Message);
        }
    }

}


