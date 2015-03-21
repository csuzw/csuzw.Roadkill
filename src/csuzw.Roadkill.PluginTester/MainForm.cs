using csuzw.Roadkill.Core;
using Roadkill.Core.Configuration;
using Roadkill.Core.Database;
using Roadkill.Core.DI;
using System;
using System.Reflection;
using System.Windows.Forms;

namespace csuzw.Roadkill.PluginTester
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            InitializeRoadkill();

            ServiceLocator.RegisterType<ITextPlugin>(new GitHubExtensions.GitHubExtensions());
            ServiceLocator.RegisterType<ITextPlugin>(new TagTreeMenu.TagTreeMenu(ServiceLocator.GetInstance<IRepository>()));

            cbxPlugins.DataSource = ServiceLocator.GetAllInstances<ITextPlugin>();
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            txtOutput.Clear();
            var input = txtInput.Text;

            try
            {
                var plugin = (ITextPlugin)cbxPlugins.SelectedItem;

                input = plugin.BeforeParse(input);
                input = plugin.AfterParse(input);

                txtOutput.Text = input;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void InitializeRoadkill()
        {
            // Get the settings from the web.config
            var configReader = GetConfigReader(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            var applicationSettings = configReader.GetApplicationSettings();

            // Configure StructureMap dependencies
            var iocSetup = new DependencyManager(applicationSettings);
            iocSetup.Configure();
        }

        private ConfigReaderWriter GetConfigReader(string configPath)
        {
            ConstructorInfo ctor = typeof(FullTrustConfigReaderWriter).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic)[0];

            var configReader = (FullTrustConfigReaderWriter)ctor.Invoke(new object[] { configPath });

            return configReader;
        }

        private void cbxPlugins_SelectedIndexChanged(object sender, EventArgs e)
        {
            var plugin = cbxPlugins.SelectedItem as IHaveSampleInput;
            if (plugin != null)
            {
                txtInput.Text = plugin.SampleInput;
            }
        }
    }
}
