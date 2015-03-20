using Roadkill.Core.Plugins;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace csuzw.Roadkill.PluginTester
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            cbxPlugins.DataSource = GetPlugins();
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            txtOutput.Clear();
            var input = txtInput.Text;

            try
            {
                var plugin = (TextPlugin)Activator.CreateInstance((Type)cbxPlugins.SelectedItem);

                input = plugin.BeforeParse(input);
                input = plugin.AfterParse(input);

                txtOutput.Text = input;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private IList<Type> GetPlugins()
        {
            var assemblies = GetAssemblies();

            var types = GetAssemblies().SelectMany(a => {
                try
                {
                    return a.GetTypes().Where(t => t.IsSubclassOf(typeof(TextPlugin)));
                }
                catch
                {
                    return new Type[] { };
                }
            });

            return types.ToList();
        }

        private IEnumerable<Assembly> GetAssemblies()
        {
            var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
            var loadedPaths = loadedAssemblies.Select(a => a.Location).ToArray();

            var referencedPaths = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll");
            var toLoad = referencedPaths.Where(r => !loadedPaths.Contains(r, StringComparer.InvariantCultureIgnoreCase)).ToList();
            toLoad.ForEach(path => loadedAssemblies.Add(AppDomain.CurrentDomain.Load(AssemblyName.GetAssemblyName(path))));

            return loadedAssemblies;
        }
    }
}
