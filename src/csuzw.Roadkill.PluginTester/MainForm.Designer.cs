namespace csuzw.Roadkill.PluginTester
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.scoMain = new System.Windows.Forms.SplitContainer();
            this.btnRun = new System.Windows.Forms.Button();
            this.txtInput = new System.Windows.Forms.TextBox();
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.cbxPlugins = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.scoMain)).BeginInit();
            this.scoMain.Panel1.SuspendLayout();
            this.scoMain.Panel2.SuspendLayout();
            this.scoMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // scoMain
            // 
            this.scoMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scoMain.Location = new System.Drawing.Point(0, 0);
            this.scoMain.Name = "scoMain";
            this.scoMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // scoMain.Panel1
            // 
            this.scoMain.Panel1.Controls.Add(this.cbxPlugins);
            this.scoMain.Panel1.Controls.Add(this.btnRun);
            this.scoMain.Panel1.Controls.Add(this.txtInput);
            // 
            // scoMain.Panel2
            // 
            this.scoMain.Panel2.Controls.Add(this.txtOutput);
            this.scoMain.Size = new System.Drawing.Size(795, 481);
            this.scoMain.SplitterDistance = 224;
            this.scoMain.TabIndex = 0;
            // 
            // btnRun
            // 
            this.btnRun.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRun.Location = new System.Drawing.Point(12, 189);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(771, 23);
            this.btnRun.TabIndex = 1;
            this.btnRun.Text = "Run";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // txtInput
            // 
            this.txtInput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtInput.Location = new System.Drawing.Point(12, 39);
            this.txtInput.Multiline = true;
            this.txtInput.Name = "txtInput";
            this.txtInput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtInput.Size = new System.Drawing.Size(771, 144);
            this.txtInput.TabIndex = 0;
            this.txtInput.Text = resources.GetString("txtInput.Text");
            // 
            // txtOutput
            // 
            this.txtOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOutput.Location = new System.Drawing.Point(12, 12);
            this.txtOutput.Multiline = true;
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.ReadOnly = true;
            this.txtOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtOutput.Size = new System.Drawing.Size(771, 229);
            this.txtOutput.TabIndex = 1;
            // 
            // cbxPlugins
            // 
            this.cbxPlugins.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxPlugins.FormattingEnabled = true;
            this.cbxPlugins.Location = new System.Drawing.Point(12, 12);
            this.cbxPlugins.Name = "cbxPlugins";
            this.cbxPlugins.Size = new System.Drawing.Size(771, 21);
            this.cbxPlugins.TabIndex = 2;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(795, 481);
            this.Controls.Add(this.scoMain);
            this.Name = "MainForm";
            this.Text = "Roadkill Plugin Tester";
            this.scoMain.Panel1.ResumeLayout(false);
            this.scoMain.Panel1.PerformLayout();
            this.scoMain.Panel2.ResumeLayout(false);
            this.scoMain.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scoMain)).EndInit();
            this.scoMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer scoMain;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.TextBox txtInput;
        private System.Windows.Forms.TextBox txtOutput;
        private System.Windows.Forms.ComboBox cbxPlugins;
    }
}

