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
        OutputFormatEnum _outputFormat = OutputFormatEnum.HTML;

        private void MainForm_Load(object sender, EventArgs e)
        {
            //Default compilation folder 
            _defaultFolderPath = Xteq5UIConstant.DefaultCompilationFolder;

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

            //Disable all output format
            DisableAllFormatCheckboxes();

            //Set HTML as the default option
            menuFormatHTML.Checked = true;
            _outputFormat = OutputFormatEnum.HTML;


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


        private void menuCmdHelpDocumentation_Click(object sender, EventArgs e)
        {
            ExecuteAndForget.Execute(Xteq5UIConstant.DocumentationURL);
        }

        private void menuHelpHowtoUse_Click(object sender, EventArgs e)
        {
            ExecuteAndForget.Execute(Xteq5UIConstant.HowtoUseURL);
        }


        private void menuCmdHelpHomepage_Click(object sender, EventArgs e)
        {
            ExecuteAndForget.Execute(Xteq5UIConstant.HomepageURL);
        }

        private void menuCmdHelpAbout_Click(object sender, EventArgs e)
        {
            //Title of about window
            string title = "About " + this.Text + " (" + Xteq5EngineConstant.Version.ToString() + ")";

            string content = Xteq5UIConstant.LicenseTxtContent;

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

            //Disable UI/button to prevent the user to start it again
            buttonGenerateReport.Enabled = false;
            this.UseWaitCursor = true;
            Application.DoEvents();

            //Create an progress object so we can get updates. 
            //This will capture the current SyncContext so we can assign an event handler directly without any special handling
            Progress<RunnerProgress> progressRunner = new Progress<RunnerProgress>();
            progressRunner.ProgressChanged += RunnerProgressUpdate;

            //Do this also for the report creation
            Progress<ReportCreationProgress> progressReport = new Progress<ReportCreationProgress>();
            progressReport.ProgressChanged += ReportProgressUpdate;


            SimplifiedXteq5Runner simpleRunner = new SimplifiedXteq5Runner(textBoxFolder.Text, textBoxUserText.Text, _outputFormat);
            bool result = await simpleRunner.RunAsync(progressRunner, progressReport);


            //Enable UI again
            this.UseWaitCursor = false;
            buttonGenerateReport.Enabled = true;
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

        private void RunnerProgressUpdate(object sender, RunnerProgress Status)
        {
            SetStatus(Status.ToString());
        }

        private void ReportProgressUpdate(object sender, ReportCreationProgress Status)
        {
            SetStatus(Status.ToString());
        }


        #region Output format menu handling
        private ToolStripMenuItem[] AllFormatMenuEntries()
        {
            ToolStripMenuItem[] array = { menuFormatHTML, menuFormatXML, menuFormatJSON };
            return array;
        }

        private void DisableAllFormatCheckboxes()
        {
            foreach (ToolStripMenuItem item in AllFormatMenuEntries())
            {
                item.Checked = false;
            }

        }

        private void DisableAllFormatCheckboxes(ToolStripMenuItem DoNotChange)
        {
            foreach (ToolStripMenuItem item in AllFormatMenuEntries())
            {
                if (item != DoNotChange)
                {
                    item.Checked = false;
                }
            }
        }

        private void menuFormatHTML_Click(object sender, EventArgs e)
        {
            DisableAllFormatCheckboxes(sender as ToolStripMenuItem);
            _outputFormat = OutputFormatEnum.HTML;
        }

        private void menuFormatXML_Click(object sender, EventArgs e)
        {
            DisableAllFormatCheckboxes(sender as ToolStripMenuItem);
            _outputFormat = OutputFormatEnum.XML;
        }

        private void menuFormatJSON_Click(object sender, EventArgs e)
        {
            DisableAllFormatCheckboxes(sender as ToolStripMenuItem);
            _outputFormat = OutputFormatEnum.JSON;
        }
        #endregion









    }
}
