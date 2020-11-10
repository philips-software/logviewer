using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Analogy.Forms;
using DevExpress.XtraEditors;

namespace Analogy.WhatIsNew
{
    public partial class WhatIsNew4_2_8 : DevExpress.XtraEditors.XtraUserControl
    {
        public WhatIsNew4_2_8()
        {
            InitializeComponent();
        }

        private void OpenGithubIssue(object sender, DevExpress.Utils.HyperlinkClickEventArgs e)
        {
            ((HyperlinkLabelControl) sender).LinkVisited = true;
            Utils.OpenLink(e.Link);
        }

        private void hyperlinkUserSettingsLookAndFeel_HyperlinkClick(object sender, DevExpress.Utils.HyperlinkClickEventArgs e)
        {
            UserSettingsForm user = new UserSettingsForm(3);
            user.ShowDialog(this);
        }
    }
}
