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
using HeadlessPS;
using Xteq5;
using Yamua;

namespace Xteq5GUI
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
            //Default compilation folder 
            _defaultFolderPath = UserInterface.Instance.DefaultCompilationFolder;

            //Check settings
            string folderAppSetting = Properties.Settings.Default.Folder;
            bool folderAppSettingIsRelative = Properties.Settings.Default.FolderIsRelative;

            if (string.IsNullOrEmpty(folderAppSetting))
            {
                //Use folderpath from user config
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
                //Use folder path from exe.config
                _folderFromAppSettings = true;
                labelFolderMessage.Text = "Folder enforced by app.config";

                //Disable buttons so user can not change the folder path
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
                        textBoxFolder.Text = PathExtension.FullPath(path);
                    }
                    catch
                    {
                        //use the old (faulty) path anyway and hope the user is smart enough to understand the error in app.config
                        textBoxFolder.Text = path;
                    }
                }
            }

            string arch = Environment.Is64BitProcess ? "64 bit" : "32 bit";
            SetStatus("Ready. Started as " + arch + " process.");
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            FormFontFixer.Fix(this);

            //Convert the horizontal line label to a real line by chaning it's size to 2
            labelHorizontalLine.Text = "";
            labelHorizontalLine.Height = 2;

            //Position it directly below the menu strip
            Point location = labelHorizontalLine.Location;
            location.Y = menuStrip.Size.Height;
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
            //TODO: Move this UI assembly
            ExecuteAndForget.Execute("https://github.com/texhex/Xteq5/wiki/");
        }

        private void homepageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //TODO: Move this UI assembly
            ExecuteAndForget.Execute("http://www.xteq5.com/");
        }

        private void menuCmdHelpAbout_Click(object sender, EventArgs e)
        {
            //Title of about window
            string title = "About " + this.Text + " (" + Xteq5Constant.EngineVersion.ToString() + ")";

            //Read license.txt
            string content = "";
            string filePath = ApplicationInformation.Instance.ParentFolder + @"licenses\license.txt";
            content = File.Exists(filePath) ? File.ReadAllText(filePath, Encoding.Default) : "Unable to load " + filePath;


            MonospacedTextForm mtform = new MonospacedTextForm();
            mtform.Title = title;
            mtform.Content = content;
            mtform.ShowDialog();
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
            SetStatus("Running, please wait...");

            this.UseWaitCursor = true;
            Application.DoEvents();

            SimplifiedXteq5Runner simpleRunner = new SimplifiedXteq5Runner(textBoxFolder.Text, textBoxUserText.Text);
            
            bool result = await simpleRunner.RunAsync();

            this.UseWaitCursor = false;
            Application.DoEvents();

            if (result == true)
            {
                //All good. Show the report.
                ExecuteAndForget.Execute(simpleRunner.ReportFilepath);
                SetStatus("Done. The report will be displayed in a second.");
            }
            else
            {
                //An error during execution was detected
                SetStatus("Failed");

                //Error message from Firefox: Well, this is embarrassing
                //Error message from Apache server: We're sorry, but something went wrong
                string title = "We're sorry, but something went wrong";

                string message = string.Format("{0}\r\n\r\n\r\nDo you wish to view technical details?", simpleRunner.FailedMessage);

                if (MessageBox.Show(message, title, MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
                {
                    MonospacedTextForm mtform = new MonospacedTextForm();
                    mtform.Title = "Details";
                    mtform.Content = simpleRunner.FailedException.ToString();
                    mtform.ShowDialog();
                }
            }


        }



    }
}
