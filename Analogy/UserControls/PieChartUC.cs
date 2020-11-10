using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Analogy.DataTypes;

namespace Analogy
{
    public partial class PieChartUC : DevExpress.XtraEditors.XtraUserControl
    {
        private ChartControl pieChart;

        public PieChartUC()
        {
            InitializeComponent();
        }

        private void PieChartUC_Load(object sender, EventArgs e)
        {
        }


        public void SetDataSources(ItemStatistics statistics)
        {
            if (pieChart == null)
            {
                pieChart = new ChartControl();
                pieChart.Titles.Add(new ChartTitle() { Text = statistics.Name });
                pieChart.AllowGesture = true;
            }
            pieChart.Titles.Clear();
            pieChart.Titles.Add(new ChartTitle() { Text = statistics.Name });
            pieChart.Series.Clear();
            // Create a pie series. 
            Series series1 = new Series(statistics.Name, ViewType.Pie3D);

            // Bind the series to data. 
            series1.DataSource = statistics.AsList();
            series1.ArgumentDataMember = nameof(Statistics.Name);
            series1.ValueDataMembers.AddRange(nameof(Statistics.Value));

            // Add the series to the chart. 
            pieChart.Series.Add(series1);

            // Format the the series labels. 
            series1.Label.TextPattern = "{A}: {VP:p0} ({V})";

            // Format the series legend items. 
            series1.LegendTextPattern = "{A}";

            // Adjust the position of series labels.  
            ((PieSeriesLabel)series1.Label).Position = PieSeriesLabelPosition.TwoColumns;

            // Detect overlapping of series labels. 
            ((PieSeriesLabel)series1.Label).ResolveOverlappingMode = ResolveOverlappingMode.Default;

            // Access the view-type-specific options of the series. 
            Pie3DSeriesView myView = (Pie3DSeriesView)series1.View;
            // Specify a data filter to explode points. 
            //myView.ExplodedPointsFilters.Add(new SeriesPointFilter(SeriesPointKey.Value_1,
            //    DataFilterCondition.GreaterThanOrEqual, 9));
            //myView.ExplodedPointsFilters.Add(new SeriesPointFilter(SeriesPointKey.Argument,
            //    DataFilterCondition.NotEqual, "Others"));
            //myView.ExplodeMode = PieExplodeMode.UseFilters;
            myView.ExplodedDistancePercentage = 30;
            myView.ExplodeMode = PieExplodeMode.All;

            // Customize the legend. 
            pieChart.Legend.Visibility = DevExpress.Utils.DefaultBoolean.True;
            // Add the chart to the form. 
            pieChart.Dock = DockStyle.Fill;
            this.Controls.Add(pieChart);
        }
        public void SetDataSources(string name, List<Statistics> statistics)
        {
            if (pieChart == null)
            {
                pieChart = new ChartControl();
                pieChart.Titles.Add(new ChartTitle() { Text = name });
                pieChart.AllowGesture = true;
            }
            pieChart.Titles.Clear();
            pieChart.Titles.Add(new ChartTitle() { Text = name });
            pieChart.Series.Clear();
            // Create a pie series. 
            Series series1 = new Series(name, ViewType.Pie3D);

            // Bind the series to data. 
            series1.DataSource = statistics;
            series1.ArgumentDataMember = nameof(Statistics.Name);
            series1.ValueDataMembers.AddRange(nameof(Statistics.Value));

            // Add the series to the chart. 
            pieChart.Series.Add(series1);

            // Format the the series labels. 
            series1.Label.TextPattern = "{A}: {VP:p0} ({V})";

            // Format the series legend items. 
            series1.LegendTextPattern = "{A}";

            // Adjust the position of series labels.  
            ((PieSeriesLabel)series1.Label).Position = PieSeriesLabelPosition.TwoColumns;

            // Detect overlapping of series labels. 
            ((PieSeriesLabel)series1.Label).ResolveOverlappingMode = ResolveOverlappingMode.Default;

            // Access the view-type-specific options of the series. 
            Pie3DSeriesView myView = (Pie3DSeriesView)series1.View;
            // Specify a data filter to explode points. 
            //myView.ExplodedPointsFilters.Add(new SeriesPointFilter(SeriesPointKey.Value_1,
            //    DataFilterCondition.GreaterThanOrEqual, 9));
            //myView.ExplodedPointsFilters.Add(new SeriesPointFilter(SeriesPointKey.Argument,
            //    DataFilterCondition.NotEqual, "Others"));
            //myView.ExplodeMode = PieExplodeMode.UseFilters;
            myView.ExplodedDistancePercentage = 30;
            myView.ExplodeMode = PieExplodeMode.All;

            // Customize the legend. 
            pieChart.Legend.Visibility = DevExpress.Utils.DefaultBoolean.True;
            // Add the chart to the form. 
            pieChart.Dock = DockStyle.Fill;
            this.Controls.Add(pieChart);
        }
    }
}


