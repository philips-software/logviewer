﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Analogy.Properties;
using Analogy.Types;
using DevExpress.XtraEditors;

namespace Analogy.Forms
{
    public partial class FirstTimeRunForm : DevExpress.XtraEditors.XtraForm
    {
        public List<FactoryCheckItem> Factories { get; set; } = new List<FactoryCheckItem>();
        public FirstTimeRunForm()
        {
            InitializeComponent();
            // This line of code is generated by Data Source Configuration Wizard
            // Uncomment next line to set the total number of data records stored within your source
            // unboundSource1.SetRowCount(42);
            // This line of code is generated by Data Source Configuration Wizard
        }

        private void FirstTimeRunForm_Load(object sender, EventArgs e)
        {
            if (DesignMode) return;
            Icon = UserSettingsManager.UserSettings.GetIcon();
            foreach (var fc in FactoriesManager.Instance.Factories)
            {
                string about = fc.Factory.About;
                var image = FactoriesManager.Instance.GetLargeImage(fc.Factory.FactoryId);
                var dp = new FactoryCheckItem(fc.Factory.Title, fc.Factory.FactoryId, about, image);
                Factories.Add(dp);
              
            }

            chkLstDataProviderStatus.DataSource = Factories;
            }

        // This event is generated by Data Source Configuration Wizard
        void unboundSource1_ValueNeeded(object sender, DevExpress.Data.UnboundSourceValueNeededEventArgs e)
        {

            // Handle this event to obtain data from your data source
            // e.Value = something /* TODO: Assign the real data here.*/
        }

        // This event is generated by Data Source Configuration Wizard
        void unboundSource1_ValuePushed(object sender, DevExpress.Data.UnboundSourceValuePushedEventArgs e)
        {

            // Handle this event to save modified data back to your data source
            // something = e.Value; /* TODO: Propagate the value into the storage.*/
        }

        // This event is generated by Data Source Configuration Wizard
        void unboundSource1_ValueNeeded_1(object sender, DevExpress.Data.UnboundSourceValueNeededEventArgs e)
        {

            // Handle this event to obtain data from your data source
            // e.Value = something /* TODO: Assign the real data here.*/
        }

        // This event is generated by Data Source Configuration Wizard
        void unboundSource1_ValuePushed_1(object sender, DevExpress.Data.UnboundSourceValuePushedEventArgs e)
        {

            // Handle this event to save modified data back to your data source
            // something = e.Value; /* TODO: Propagate the value into the storage.*/
        }

        // This event is generated by Data Source Configuration Wizard
        void unboundSource1_ValueNeeded_2(object sender, DevExpress.Data.UnboundSourceValueNeededEventArgs e)
        {

            // Handle this event to obtain data from your data source
            // e.Value = something /* TODO: Assign the real data here.*/
        }
    }
}