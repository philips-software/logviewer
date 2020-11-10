using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace Analogy.Forms
{

    public partial class AnalogyExclude : XtraForm
    {
        private class AnalogyCheckListItem
        {
            internal string Text { get; }
            private int Count { get; }

            public AnalogyCheckListItem(string text, int count)
            {
                Text = text;
                Count = count;
            }

            public override string ToString()
            {
                return $" {nameof(Count)} {Count}: {Text}";
            }
        }

        public static List<string> GlobalExclusion = new List<string>();
        public AnalogyExclude()
        {
            InitializeComponent();
        }

        public AnalogyExclude(List<string> items, List<string> excludeMostCommon) : this()
        {
            var group = items.GroupBy(i => i).OrderByDescending(i => i.Count());
            foreach (IGrouping<string, string> grouping in group)
            {

                bool checkedItem = excludeMostCommon.Contains(grouping.Key);
                checkedListBoxControl1.Items.Add(new AnalogyCheckListItem(grouping.Key, grouping.Count()), checkedItem);
            }
        }

        private void sBtnOk_Click(object sender, EventArgs e)
        {
            GlobalExclusion = checkedListBoxControl1.CheckedItems.Cast<DevExpress.XtraEditors.Controls.CheckedListBoxItem>().Select(i => FilterCriteriaObject.EscapeLikeValue((i.Value as AnalogyCheckListItem)?.Text)).ToList();
            DialogResult = DialogResult.OK;
            Close();
        }

        private void sBtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            foreach (int i in checkedListBoxControl1.CheckedIndices)
            {
                checkedListBoxControl1.SetItemCheckState(i, CheckState.Unchecked);
            }
        }

        private void AnalogyExclude_Load(object sender, EventArgs e)
        {
            Icon = UserSettingsManager.UserSettings.GetIcon();
        }
    }

    public class SqlParser
    {
        public List<string> Parse(string sql)
        {
            //TSql100Parser parser = new TSql100Parser(false);
            //IScriptFragment fragment;
            //IList<ParseError> errors;
            //fragment = parser.Parse(new StringReader(sql), out errors);
            //if (errors != null && errors.Count > 0)
            //{
            //    List<string> errorList = new List<string>();
            //    foreach (var error in errors)
            //    {
            //        errorList.Add(error.Message);
            //    }
            //    return errorList;
            //}
            return null;
        }
    }
}
