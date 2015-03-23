using Roadkill.Core.Configuration;
using Roadkill.Core.Database;
using Roadkill.Core.DI;
using Roadkill.Core.Plugins;
using Roadkill.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace csuzw.Roadkill.PluginTester
{
    public partial class MainForm : Form
    {
        private readonly Dictionary<TextPlugin, string> _plugins = new Dictionary<TextPlugin, string>();

        public MainForm()
        {
            InitializeComponent();

            InitializeRoadkill();

            _plugins.Add(new PageReferences.PageReferences(ServiceLocator.GetInstance<IRepository>()), SampleInput.PageReferences);
            _plugins.Add(new GitHubExtensions.GitHubExtensions(), SampleInput.GitHubExtensions);
            _plugins.Add(new TagTreeMenu.TagTreeMenu(ServiceLocator.GetInstance<IRepository>()), SampleInput.TagTreeMenu);

            cbxPlugins.DataSource = _plugins.Keys.ToList();
            cbxPlugins.DisplayMember = "Name";
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            txtOutput.Clear();
            var input = txtInput.Text;

            try
            {
                var plugin = (TextPlugin)cbxPlugins.SelectedItem;

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
            var plugin = cbxPlugins.SelectedItem as TextPlugin;
            if (plugin != null)
            {
                txtInput.Text = _plugins[plugin];
            }
        }

        #region Sample Input Constants

        private static class SampleInput
        {
            public const string GitHubExtensions = @"## Table

Some text

<code>Some other text

```csharp
someshit
```</code>

More text with some <code>~~code~~</code>. You get ~~the~~ picture.

```javascript
Transform this shit!
```

## Bongos";

            public const string TagTreeMenu = @"{menu=One~Test(Two(Four),Three(Five|Six))}";

            public const string PageReferences = @"{PageReferences=Foo}";
        }

        #endregion
    }
}
