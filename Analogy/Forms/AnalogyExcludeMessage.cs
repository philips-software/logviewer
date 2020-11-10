using System;
using System.Windows.Forms;
using Analogy.Interfaces;
using DevExpress.XtraEditors;

namespace Analogy.Forms
{
    public partial class AnalogyExcludeMessage : XtraForm
    {
        public string MessageText { get; set; }
        public AnalogyExcludeMessage()
        {
            InitializeComponent();
            Icon = UserSettingsManager.UserSettings.GetIcon();
        }

        public AnalogyExcludeMessage(AnalogyLogMessage m) : this()
        {
            txtbMessage.Text = m.Text;
        }
        private void sBtnOk_Click(object sender, EventArgs e)
        {
            MessageText = FilterCriteriaObject.EscapeLikeValue(txtbMessage.Text);
            DialogResult = DialogResult.OK;
            Close();
        }

        private void sBtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void AnalogyExcludeMessage_Load(object sender, EventArgs e)
        {
            Icon = UserSettingsManager.UserSettings.GetIcon();
        }
    }
}
