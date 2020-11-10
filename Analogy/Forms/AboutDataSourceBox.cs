using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Analogy.Interfaces.Factories;
using DevExpress.XtraEditors;

namespace Analogy.Forms
{
    partial class AboutDataSourceBox : XtraForm
    {
        private readonly IAnalogyFactory _factory;
        private readonly Assembly _factoryAssembly;
        public AboutDataSourceBox()
        {
            InitializeComponent();
        }

        public AboutDataSourceBox(IAnalogyFactory factory) : this()
        {
            _factory = factory;
            _factoryAssembly = FactoriesManager.Instance.GetAssemblyOfFactory(factory);
        }

        private string AssemblyTitle
        {
            get
            {
                object[] attributes = _factoryAssembly.GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                    {
                        return titleAttribute.Title;
                    }
                }
                return System.IO.Path.GetFileNameWithoutExtension(_factoryAssembly.CodeBase);
            }
        }

        private string AssemblyVersion => FileVersionInfo.GetVersionInfo(_factoryAssembly.Location).FileVersion;

        private string AssemblyDescription
        {
            get
            {
                object[] attributes = _factoryAssembly.GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        private string AssemblyProduct
        {
            get
            {
                object[] attributes = _factoryAssembly.GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        private string AssemblyCopyright
        {
            get
            {
                object[] attributes = _factoryAssembly.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        private string AssemblyCompany
        {
            get
            {
                object[] attributes = _factoryAssembly.GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }

        private void AboutDataSourceBox_Load(object sender, EventArgs e)
        {
            Text = $"About {AssemblyTitle}";
            labelProductName.Text = AssemblyProduct;
            labelVersion.Text = $"Version {AssemblyVersion}";
            labelCopyright.Text = AssemblyCopyright;
            labelCompanyName.Text = AssemblyCompany;
            meDescription.Text = $"{AssemblyDescription}{Environment.NewLine}{_factory.About}";
            meChangeLog.Text = string.Join(Environment.NewLine,
                _factory.ChangeLog.OrderByDescending(c => c.Date).Select(cl => $"{cl.Date.ToShortDateString()}: {cl.ChangeInformation} ({cl.Name})"));
            meContributions.Text = string.Join(Environment.NewLine, _factory.Contributors);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
