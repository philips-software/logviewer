using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Analogy.Tools.JsonViewer;
using DevExpress.XtraEditors;
using Analogy.Interfaces;
using Newtonsoft.Json;

namespace Analogy.Tools
{
    public partial class JsonViewerForm : DevExpress.XtraEditors.XtraForm
    {
        private UserSettingsManager settings => UserSettingsManager.UserSettings;
        private JsonTreeView _jsonTreeView;
        private AnalogyLogMessage message;

        public JsonViewerForm()
        {
            InitializeComponent();
        }

        public JsonViewerForm(AnalogyLogMessage message):this()
        {
            this.message = message;
        }

        private void JsonViewerForm_Load(object sender, EventArgs e)
        {
            if (DesignMode)
            {
                return;
            }

            Icon = settings.GetIcon();
            _jsonTreeView = new JsonTreeView();
            splitContainerControl1.Panel2.Controls.Add(_jsonTreeView);
            _jsonTreeView.Dock = DockStyle.Fill;
            if (message != null)
            {
                memoEdit1.Text = message.Text;
                var json = ExtractJsonObject(message.Text);
                if (!string.IsNullOrEmpty(json))
                {
                    _jsonTreeView.ShowJson(json);
                }
            }



        }

        public string ExtractJsonObject(string mixedString)
        {
            for (var i = mixedString.IndexOf('{'); i > -1; i = mixedString.IndexOf('{', i + 1))
            {
                for (var j = mixedString.LastIndexOf('}'); j > -1; j = mixedString.LastIndexOf("}", j - 1))
                {
                    var jsonProbe = mixedString.Substring(i, j - i + 1);
                    try
                    {
                        var valid = JsonConvert.DeserializeObject(jsonProbe);
                        return jsonProbe;
                    }
                    catch
                    {
                    }
                }
            }

            return null;
        }

        private void sbtnLoad_Click(object sender, EventArgs e)
        {
            _jsonTreeView.ShowJson(memoEdit1.Text);
        }
    }
}