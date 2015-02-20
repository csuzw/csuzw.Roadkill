using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Roadkill.PluginTester
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            txtOutput.Clear();
            var input = txtInput.Text;

            try
            {
                var plugin = new Roadkill.GitHubExtensions.GitHubExtensions();

                input = plugin.BeforeParse(input);
                input = plugin.AfterParse(input);

                txtOutput.Text = input;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
