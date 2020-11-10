using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace Analogy.Forms
{
    public partial class ProcessNameAndID : XtraForm
    {
        private IOrderedEnumerable<ProcessName> processes;
        public ProcessNameAndID()
        {
            InitializeComponent();

        }

        private void ProcessNameAndID_Load(object sender, EventArgs e)
        {
            Icon = UserSettingsManager.UserSettings.GetIcon();
            processes = Process.GetProcesses().Select(p => new ProcessName(p.ProcessName, p.Id)).OrderByDescending(p => p.ID);
            gridControl1.DataSource = processes;
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            label1.Text = "NA";
            if (e.KeyCode == Keys.Enter)
            {
                label1.Text = int.TryParse(textBox1.Text, out var val)
                    ? processes.FirstOrDefault(p => p.ID == val)?.Name
                    : processes.FirstOrDefault(p => p.Name.Equals(textBox1.Text, StringComparison.InvariantCultureIgnoreCase))?.ID.ToString();
            }
        }

        private class ProcessName
        {
            public string Name { get; }
            public int ID { get; }

            public ProcessName(string name, int id)
            {
                Name = name;
                ID = id;
            }
        }
    }


}
