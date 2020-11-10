using Analogy.Interfaces;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Analogy
{
    public partial class MessageDetailsUC : XtraUserControl
    {
        private AnalogyLogMessage Message { get; set; }
        private List<AnalogyLogMessage> Messages { get; }
        private string DataSource { get; }
        public MessageDetailsUC()
        {
            InitializeComponent();
        }

        public MessageDetailsUC(AnalogyLogMessage msg, List<AnalogyLogMessage> messages, string dataSource) : this()
        {
            Message = msg;
            Messages = messages;
            DataSource = dataSource;
        }

        private void UCMessageDetails_Load(object sender, EventArgs e)
        {
            xtraTabControlMessageInfo.SelectedTabPage = xtraTabPageText;
            LoadMessage();
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Left)
            {
                LoadPreviousMessage();
                return true;
            }

            if (keyData == Keys.Right)
            {
                LoadNextMessage();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);

        }
        private void LoadMessage()
        {
       
            xtraTabPageAdditionalInformation.PageVisible = Message.AdditionalInformation != null && Message.AdditionalInformation.Any();
            if (Message.AdditionalInformation != null)
            {
                memoAdditionalInformation.Text = string.Join(Environment.NewLine,
                    Message.AdditionalInformation.Select(kv => $"{kv.Key}:{kv.Value}"));
            }

            memoText.Text = Message.Text;
            txtbMachineName.Text = Message.MachineName;
            txtID.Text = Message.Id.ToString();
            txtbDataSource.Text = DataSource;
            txtbDateValue.Text = Message.Date.ToString(UserSettingsManager.UserSettings.DateTimePattern);
            txtbLevelValue.Text = Message.Level.ToString();
            txtbProcessModuleName.Text =Message.Module;
            txtbProcessId.Text = Message.ProcessId.ToString();
            txtbThreadId.Text = Message.ThreadId.ToString();
            txtSourceValue.Text = Message.Source;
            txtbMethod.Text = Message.MethodName;
            txtbFileName.Text = Message.FileName;
            txtbUser.Text = Message.User;
            txtbLineNumber.Text = Message.LineNumber.ToString();
            txtbIndex.Text = $"{Messages.IndexOf(Message) + 1} of {Messages.Count}";
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            LoadNextMessage();
        }

        private void LoadPreviousMessage()
        {
            if (Messages.First() == Message)
            {
                return;
            }

            Message = Messages[Messages.IndexOf(Message) - 1];
            LoadMessage();
        }
        private void LoadNextMessage()
        {
            if (Messages.Last() == Message)
            {
                return;
            }

            Message = Messages[Messages.IndexOf(Message) + 1];
            LoadMessage();
        }
        private void btnPrev_Click(object sender, EventArgs e)
        {
            LoadPreviousMessage();
        }

        private void sBtnCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(memoText.Text);
        }


    }
}
