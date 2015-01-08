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
            //Generate default folder 
            string programDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            _defaultFolderPath = Path.GetFullPath(programDataFolder) + @"\" + Xteq5Constant.DirectoryNameCommonApplicationData;

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
                        textBoxFolder.Text = Path.GetFullPath(path);
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

            //Convert the horizontal line label to a real line
            labelHorizontalLine.Text = "";
            labelHorizontalLine.Height = 2;
            
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
            ExecuteAndForget.Execute("https://github.com/texhex/Xteq5/Wiki/_fwLinkScript");
        }

        private void homepageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExecuteAndForget.Execute("http://www.Xteq5.com/");
        }

        private void menuCmdHelpAbout_Click(object sender, EventArgs e)
        {            
            //Title of about window
            string title = "About " + this.Text + " (" + Xteq5Constant.AssemblyVersion.ToString() + ")";

            //Read license.txt
            string content = "";
            string filePath = ApplicationInformation.Instance.ParentFolder + @"licenses\license.txt";
            content = File.Exists(filePath) ? File.ReadAllText(filePath, Encoding.Default) : "Unable to load " + filePath;


            MonospacedTextForm mtform = new MonospacedTextForm();
            mtform.Title=title;
            mtform.Content=content;
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
            SetStatus("Running...");

            bool failed = true;
            string failedMessage = "Unknown";
            Exception failedDetailsException = new Exception("Unknown exception");

            try
            {            
                this.UseWaitCursor = true;
                Application.DoEvents();

                Xteq5Runner runner = new Xteq5Runner();
                Report report = await runner.RunAsync(textBoxFolder.Text);

                report.UserText = textBoxUserText.Text;

                SetStatus("Generating report...");

                string htmlTemplatePath = Path.Combine(textBoxFolder.Text, "BootstrapTemplate1.html");
                BootstrapHTMLGenerator generator = new BootstrapHTMLGenerator(htmlTemplatePath);
                string tempFile = generator.GenerateAndSaveFile(report);

                ExecuteAndForget.Execute(tempFile);

                SetStatus("Done. The report will be displayed in a second.");
                failed = false;
            }

            catch (FileNotFoundException fnfe)
            {
                //In most cases this means we couldn't load System.Management.Automation. 
                if (fnfe.Message.Contains("System.Management.Automation"))
                {
                    //We need to explain to the user that this means PowerShell is missing. 
                    failedMessage = "PowerShell is required, but could not be loaded. Please re-run Setup.exe and follow the provided link to install it.";
                }
                else
                {
                    //In any other case we have no idea what is missing, so use the message from the exception.
                    failedMessage = fnfe.Message;
                }
                
                failedDetailsException = fnfe;
            }
            catch (TemplateFileNotFoundException tfnfe)
            {
                //Template file does not exist in folder 
                failedMessage = "The template file is missing. Please re-run Setup.exe to install it.";
                failedDetailsException = tfnfe;
            }
            catch (PowerShellTestFailedException pstfe)
            {
                //The powershell test failed
                failedMessage = "Testing the PowerShell environment failed. Please see the technical details.";
                failedDetailsException = pstfe;
            }
            catch (DirectoryNotFoundException dnfe)
            {
                //The selected folder is missing something. The message already includes all details so we will reuse it. 
                failedMessage = "The selected folder can't be used: " + dnfe.Message;
                failedDetailsException = dnfe;
            }
            catch (Exception exc)
            {
                //No idea what happened. Use the message from the exception.
                failedMessage = exc.Message;
                failedDetailsException = exc;
            }

            finally
            {
                this.UseWaitCursor = false;
            }


            //Hopefully failed is not TRUE
            if (failed)
            {
                SetStatus("Failed");

                //Error message from Firefox: Well, this is embarrassing
                //Error message from Apache server: We're sorry, but something went wrong
                string title = "We're sorry, but something went wrong";

                string message = string.Format("{0}\r\n\r\n\r\nDo you wish to view technical details?", failedMessage);

                if (MessageBox.Show(message, title, MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
                {
                    MonospacedTextForm mtform = new MonospacedTextForm();
                    mtform.Title = "Details";
                    mtform.Content = failedDetailsException.ToString();
                    mtform.ShowDialog();
                }

            }

        }



    }
}
