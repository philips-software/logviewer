using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Analogy.Interfaces;
using DevExpress.XtraEditors;
using MessagePack;

namespace Analogy.Forms
{
    public partial class AnalogyOTAForm : XtraForm
    {
        private class ComboboxItem
        {
            public string Name { get; set; }
            public IAnalogyShareable Shareable { get; set; }

        }
        private IAnalogyShareable Shareable { get; set; }
        private List<AnalogyLogMessage> messages;
        public AnalogyOTAForm()
        {
            messages = new List<AnalogyLogMessage>();
            InitializeComponent();
        }

        public AnalogyOTAForm(DataTable data) : this()
        {
            List<ComboboxItem> items = new List<ComboboxItem>();
            foreach (DataRow dataRow in data.Rows)
            {
                messages.Add(dataRow["Object"] as AnalogyLogMessage);
            }

            foreach (var fc in FactoriesManager.Instance.Factories)
            {
                foreach (var sf in fc.ShareableFactories)
                {
                    foreach (var share in sf.Shareables)
                    {
                        items.Add(new ComboboxItem() { Name = sf.Title, Shareable = share });
                    }
                }
            }

            cbShares.DataSource = items;
            cbShares.DisplayMember = "Name";
        }

        private void AnalogyOTAForm_Load(object sender, EventArgs e)
        {
            Icon = UserSettingsManager.UserSettings.GetIcon();
        }

        private async void AnalogyOTAForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Shareable != null)
            {
                await Shareable.CleanupSender();
            }
        }


        private async void sbtnInit_Click(object sender, EventArgs e)
        {
            if (cbShares.SelectedItem == null || !(cbShares.SelectedItem is ComboboxItem item))
            {
                return;
            }

            Shareable = item.Shareable;
            await Shareable.InitializeSender();
            sBtnSend.Enabled = true;
        }

        private void sBtnSend_Click(object sender, EventArgs e)
        {
            if (rbList.Checked)
            {
                Shareable.SendMessages(messages, "");
            }
            else
            {
                byte[] data = MessagePackSerializer.Serialize(messages);
                Shareable.SendMessages(data, "");
            }
        }
    }
}
