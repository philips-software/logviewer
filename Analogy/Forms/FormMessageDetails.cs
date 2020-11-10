using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Analogy.Interfaces;
using DevExpress.XtraEditors;

namespace Analogy.Forms
{
    public partial class FormMessageDetails : XtraForm
    {
        public FormMessageDetails()
        {
            InitializeComponent();
        }

        public FormMessageDetails(AnalogyLogMessage msg, List<AnalogyLogMessage> messages, string dataSource) : this()
        {
            MessageDetailsUC uc = new MessageDetailsUC(msg, messages, dataSource);
            spltCMain.Panel1.Controls.Add(uc);
            uc.Dock = DockStyle.Fill;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FormMessageDetails_Load(object sender, EventArgs e)
        {
            Icon = UserSettingsManager.UserSettings.GetIcon();
        }
    }
}
