using Analogy.Interfaces;
using DevExpress.XtraCharts;
using DevExpress.XtraEditors.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Analogy.DataTypes;

namespace Analogy
{
    public partial class DataVisualizerUC : DevExpress.XtraEditors.XtraUserControl
    {

        private Func<List<AnalogyLogMessage>> Messages { get; set; }

        private List<string> Items { get; set; }
        private List<string> MessagesText => Messages().Select(r => r.Text).ToList();

        public DataVisualizerUC()
        {
            Items = new List<string>();
            InitializeComponent();
        }

        public DataVisualizerUC(Func<List<AnalogyLogMessage>> messagesFunc) : this()
        {
            Messages = messagesFunc;
            logStatisticsUC1.Statistics = new LogStatistics(messagesFunc);
        }
        public DataVisualizerUC(List<AnalogyLogMessage> messages) : this()
        {
            Messages = () => messages;
            logStatisticsUC1.Statistics = new LogStatistics(messages);
        }

        private void DataVisualizerUC_Load(object sender, EventArgs e)
        {

        }

        private void Plot()
        {
            logStatisticsUC1.RefreshStatistics(new LogStatistics(Messages));
            Dictionary<string, Dictionary<TimeSpan, int>> frequency =
                new Dictionary<string, Dictionary<TimeSpan, int>>();
            Dictionary<string, Dictionary<TimeSpan, int>> frequencyCount =
                new Dictionary<string, Dictionary<TimeSpan, int>>();
            Dictionary<string, List<AnalogyLogMessage>> timeDistribution =
                new Dictionary<string, List<AnalogyLogMessage>>();

            foreach (var item in Items)
            {
                frequency[item] = new Dictionary<TimeSpan, int>();
                frequencyCount[item] = new Dictionary<TimeSpan, int>();
                timeDistribution[item] = new List<AnalogyLogMessage>();
            }

            var msgs = Messages();
            foreach (var m in msgs)
            {

                foreach (var item in Items)
                {
                    if (m.Text.Contains(item))
                    {
                        timeDistribution[item].Add(m);
                        if (!frequency[item].ContainsKey(m.Date.TimeOfDay))
                        {
                            frequency[item].Add(m.Date.TimeOfDay, 1);
                        }

                        if (!frequencyCount[item].ContainsKey(m.Date.TimeOfDay))
                        {
                            frequencyCount[item].Add(m.Date.TimeOfDay, 1);
                        }
                        else
                        {
                            frequencyCount[item][m.Date.TimeOfDay] += 1;
                        }
                    }
                    else
                    {
                        if (!frequency[item].ContainsKey(m.Date.TimeOfDay))
                        {
                            frequency[item].Add(m.Date.TimeOfDay, 0);
                        }

                        if (!frequencyCount[item].ContainsKey(m.Date.TimeOfDay))
                        {
                            frequencyCount[item].Add(m.Date.TimeOfDay, 0);
                        }
                    }
                }
            }

            chartControlOnOff.Series.Clear();
            chartControlOnOff.DataSource = CreateTable(frequency);
            chartControlOnOff.SeriesDataMember = "Name";
            chartControlOnOff.SeriesTemplate.ArgumentDataMember = "Date";
            chartControlOnOff.SeriesTemplate.ValueDataMembers.AddRange("ValueX");

            chartControlOnOff.SeriesTemplate.ArgumentScaleType = ScaleType.DateTime;
            chartControlOnOff.SeriesTemplate.ChangeView(ViewType.Line);
            XYDiagram diagram = (XYDiagram)chartControlOnOff.Diagram;
            diagram.AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Millisecond;
            diagram.AxisX.DateTimeScaleOptions.GridAlignment = DateTimeGridAlignment.Hour;
            diagram.AxisX.Label.TextPattern = "{A:t}";


            chartControlFrequency.Series.Clear();
            chartControlFrequency.DataSource = CreateTable(frequencyCount);
            chartControlFrequency.SeriesDataMember = "Name";
            chartControlFrequency.SeriesTemplate.ArgumentDataMember = "Date";
            chartControlFrequency.SeriesTemplate.ValueDataMembers.AddRange(new string[] { "ValueX" });
            chartControlFrequency.SeriesTemplate.ArgumentScaleType = ScaleType.DateTime;
            chartControlFrequency.SeriesTemplate.ChangeView(ViewType.Bar);
            XYDiagram diagram2 = (XYDiagram)chartControlFrequency.Diagram;
            diagram2.AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Millisecond;
            diagram2.AxisX.DateTimeScaleOptions.GridAlignment = DateTimeGridAlignment.Hour;
            diagram2.AxisX.Label.TextPattern = "{A:t}";

            chartTimeDistribution.Series.Clear();
            chartTimeDistribution.DataSource = CreateTimeDistributionTable(timeDistribution);
            chartTimeDistribution.SeriesDataMember = "Name";
            chartTimeDistribution.SeriesTemplate.ArgumentDataMember = "Date";
            chartTimeDistribution.SeriesTemplate.ValueDataMembers.AddRange("ValueY");
            chartTimeDistribution.SeriesTemplate.ArgumentScaleType = ScaleType.DateTime;
            chartTimeDistribution.SeriesTemplate.ChangeView(ViewType.Point);
            XYDiagram diagram3 = (XYDiagram)chartTimeDistribution.Diagram;
            diagram3.AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Millisecond;
            diagram3.AxisX.DateTimeScaleOptions.GridAlignment = DateTimeGridAlignment.Hour;
            diagram3.AxisX.Label.TextPattern = "{A:l}";
            diagram3.AxisY.VisualRange.MinValue = 0;
            diagram3.AxisY.VisualRange.MaxValue = 24;

        }

        private DataTable CreateTable(Dictionary<string, Dictionary<TimeSpan, int>> data)
        {
            DataTable tbl = new DataTable();
            tbl.Columns.Add("Name", typeof(string));
            tbl.Columns.Add("Date", typeof(DateTime));
            tbl.Columns.Add("ValueX", typeof(int));
            tbl.Columns.Add("ValueY", typeof(int));

            foreach (KeyValuePair<string, Dictionary<TimeSpan, int>> freq in data)
            {
                string item = freq.Key;
                foreach (KeyValuePair<TimeSpan, int> val in freq.Value)
                {
                    tbl.Rows.Add(item, new DateTime(val.Key.Ticks), val.Value, val.Value);
                }

            }

            return tbl;
        }

        private DataTable CreateTimeDistributionTable(Dictionary<string, List<AnalogyLogMessage>> data)
        {
            DataTable tbl = new DataTable();
            tbl.Columns.Add("Name", typeof(string));
            tbl.Columns.Add("Date", typeof(DateTime));
            tbl.Columns.Add("ValueX", typeof(long));
            tbl.Columns.Add("ValueY", typeof(float));

            foreach (KeyValuePair<string, List<AnalogyLogMessage>> td in data)
            {
                string item = td.Key;
                foreach (AnalogyLogMessage val in td.Value)
                {
                    tbl.Rows.Add(item, val.Date, val.Date.Ticks,
                        val.Date.Hour + (float)val.Date.Minute / 60 + (float)val.Date.Second / 60 / 60);
                }

            }

            return tbl;
        }

        private void chklistItems_ItemCheck(object sender, DevExpress.XtraEditors.Controls.ItemCheckEventArgs e)
        {
            Items = chklistItems.CheckedItems.Cast<CheckedListBoxItem>().Select(i => i.Value.ToString()).ToList();
            Plot();

        }

        private void sBtnAdd_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textEdit1.Text))
            {
                Items.Add(textEdit1.Text.Substring(textEdit1.Text.IndexOf(':') + 1));
                chklistItems.Items.Add(textEdit1.Text, true);
                ceAutoRefresh.Enabled = true;
                seRefreshInterval.Enabled = true;
                Plot();
            }
            else
            {
                Items.Add("");
                chklistItems.Items.Add("", true);
                Plot();
            }
        }

        private void seRefreshInterval_EditValueChanged(object sender, EventArgs e)
        {
            ceAutoRefresh.CheckState = CheckState.Checked;
            tmrPlotting.Interval = (int)seRefreshInterval.Value * 1000;
        }

        private void ceAutoRefresh_EditValueChanged(object sender, EventArgs e)
        {
            if (ceAutoRefresh.Checked)
            {
                tmrPlotting.Interval = (int)seRefreshInterval.Value * 1000;
                tmrPlotting.Enabled = true;
            }
            else
            {
                tmrPlotting.Enabled = false;
            }

        }

        private void tmrPlotting_Tick(object sender, EventArgs e)
        {
            tmrPlotting.Enabled = false;
            Plot();
            tmrPlotting.Enabled = true;
        }
    }
}