using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Yamua;
using TestUtil;



namespace TestUtilGUI
{
    /* MTH: About the design of this tool I would like to quote [John Skeet](http://codeblog.jonskeet.uk/2014/09/30/the-mysteries-of-bcl-time-zone-data/):
     * 
     * " I'm really not a UX designer. I make sure that a UI does what I need it to, then run away before it breaks. 
     *   Apologies to anyone who feels offended by the awfulness of this. "
     */
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        string _defaultFolderPath = "";
        bool _folderFromAppSettings = false;

        private void MainForm_Load(object sender, EventArgs e)
        {
            //Generate default folder 
            string sPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            _defaultFolderPath = Path.GetFullPath(sPath) + @"\TestUtil";

            //HTML Template
            //_htmlTemplatePath = ApplicationInformation.Instance.ParentFolder + "BootstrapTemplate1.html";

            //Check settings
            string folderAppSetting = Properties.Settings.Default.Folder;
            bool folderAppSettingIsRelative = Properties.Settings.Default.FolderIsRelative;

            if (string.IsNullOrEmpty(folderAppSetting))
            {
                //Use user config
                string folder = Properties.Settings.Default.FolderUser;
                if (Directory.Exists(folder))
                {
                    textBoxFolder.Text = folder;
                }
                else
                {
                    SetFolderToDefault();
                }

                //Clear message when not using exe.config
                labelFolderMessage.Text = "";
            }
            else
            {
                _folderFromAppSettings = true;
                labelFolderMessage.Text = "Folder enforced by app.config";

                buttonSelectFolder.Enabled = false;
                buttonRevertFolderToDefault.Enabled = false;

                if (folderAppSettingIsRelative == false)
                {
                    textBoxFolder.Text = folderAppSetting;
                }
                else
                {
                    string path = ApplicationInformation.Instance.Folder;
                    path += folderAppSetting;

                    //This might throw an NoSupportedException if the path is invalid                    
                    try
                    {
                        textBoxFolder.Text = Path.GetFullPath(path);
                    }
                    catch
                    {
                        //use the old (faulty) path anyway and hope the user is smart enough to understand the error in app.config
                        textBoxFolder.Text = path;
                    }
                }
            }

            this.Text += " (" + TestUtilConstant.AssemblyVersion.ToString() + ")";

            string arch = Environment.Is64BitProcess ? "64 bit" : "32 bit";
            SetStatus("Ready. Started as " + arch + " process.");
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            FormFontFixer.Fix(this);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //User configuration is only saved when AppSettings are not set
            if (_folderFromAppSettings == false)
            {
                //Save the current configuration 
                Properties.Settings.Default.FolderUser = textBoxFolder.Text;
                Properties.Settings.Default.Save();
            }

        }

        private void SetStatus(string Text)
        {
            statusbarLabel.Text = Text;
        }

        private void SetFolderToDefault()
        {
            textBoxFolder.Text = _defaultFolderPath;
        }

        private void menuCmdFileExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void menuCmdHelpCreate_Click(object sender, EventArgs e)
        {
            ExecuteAndForget.Execute("https://github.com/texhex/testutil/wiki/");
        }

        private void homepageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExecuteAndForget.Execute("http://www.testutil.com/");
        }

        private void menuCmdHelpAbout_Click(object sender, EventArgs e)
        {
            AboutForm about = new AboutForm();
            about.Text += this.Text;
            about.ShowDialog();
        }

        private void buttonSelectFolder_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.Description = groupBoxFolder.Text;
            folderBrowserDialog.SelectedPath = textBoxFolder.Text;

            DialogResult result = folderBrowserDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                textBoxFolder.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void buttonRevertFolderToDefault_Click(object sender, EventArgs e)
        {
            SetFolderToDefault();
        }

        private void buttonViewFolderInExplorer_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxFolder.Text) == false)
            {
                ExecuteAndForget.Execute(textBoxFolder.Text);
            }
        }

        private async void buttonGenerateReport_Click(object sender, EventArgs e)
        {
            try
            {
                SetStatus("Running...");
                this.UseWaitCursor = true;
                Application.DoEvents();

                TestUtilRunner runner = new TestUtilRunner();
                Report report = await runner.RunAsync(textBoxFolder.Text);

                report.UserText = textBoxUserText.Text;

                SetStatus("Generating report...");

                string htmlTemplatePath = Path.Combine(textBoxFolder.Text, "BootstrapTemplate1.html");
                BootstrapHTMLGenerator generator = new BootstrapHTMLGenerator(htmlTemplatePath);
                string tempFile = generator.GenerateAndSaveFile(report);

                ExecuteAndForget.Execute(tempFile);

                SetStatus("Done. The report will be displayed in your browser");
            }
            catch (Exception exc)
            {
                SetStatus("Failed");
                //Well, this is embarrassing.
                MessageBox.Show(string.Format("Sorry it didn't work out.\r\n\r\n{0}", exc.ToString()), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                MessageBox.Show(exc.ToString());
            }

            finally
            {
                this.UseWaitCursor = false;
            }

        }



    }
}
