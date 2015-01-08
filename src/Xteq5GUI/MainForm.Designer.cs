namespace Xteq5GUI
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
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.menuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.menuCmdFileExit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.menuCmdHelpHomepage = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuCmdHelpWiki = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuCmdHelpAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.statusContainer = new System.Windows.Forms.StatusStrip();
            this.statusbarLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.groupBoxFolder = new System.Windows.Forms.GroupBox();
            this.buttonRevertFolderToDefault = new System.Windows.Forms.Button();
            this.buttonViewFolderInExplorer = new System.Windows.Forms.Button();
            this.labelFolderMessage = new System.Windows.Forms.Label();
            this.buttonSelectFolder = new System.Windows.Forms.Button();
            this.textBoxFolder = new System.Windows.Forms.TextBox();
            this.groupBoxReport = new System.Windows.Forms.GroupBox();
            this.buttonGenerateReport = new System.Windows.Forms.Button();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBoxAdditonalText = new System.Windows.Forms.GroupBox();
            this.textBoxUserText = new System.Windows.Forms.TextBox();
            this.labelHorizontalLine = new System.Windows.Forms.Label();
            this.menuStrip.SuspendLayout();
            this.statusContainer.SuspendLayout();
            this.groupBoxFolder.SuspendLayout();
            this.groupBoxReport.SuspendLayout();
            this.groupBoxAdditonalText.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFile,
            this.menuHelp});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Margin = new System.Windows.Forms.Padding(4);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Padding = new System.Windows.Forms.Padding(8, 2, 0, 2);
            this.menuStrip.Size = new System.Drawing.Size(484, 27);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip1";
            // 
            // menuFile
            // 
            this.menuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuCmdFileExit});
            this.menuFile.Name = "menuFile";
            this.menuFile.Size = new System.Drawing.Size(41, 23);
            this.menuFile.Text = "&File";
            // 
            // menuCmdFileExit
            // 
            this.menuCmdFileExit.Name = "menuCmdFileExit";
            this.menuCmdFileExit.Size = new System.Drawing.Size(162, 24);
            this.menuCmdFileExit.Text = "E&xit";
            this.menuCmdFileExit.Click += new System.EventHandler(this.menuCmdFileExit_Click);
            // 
            // menuHelp
            // 
            this.menuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuCmdHelpHomepage,
            this.toolStripMenuItem1,
            this.menuCmdHelpWiki,
            this.toolStripMenuItem2,
            this.menuCmdHelpAbout});
            this.menuHelp.Name = "menuHelp";
            this.menuHelp.Size = new System.Drawing.Size(49, 23);
            this.menuHelp.Text = "&Help";
            // 
            // menuCmdHelpHomepage
            // 
            this.menuCmdHelpHomepage.Name = "menuCmdHelpHomepage";
            this.menuCmdHelpHomepage.Size = new System.Drawing.Size(222, 24);
            this.menuCmdHelpHomepage.Text = "&Homepage...";
            this.menuCmdHelpHomepage.Click += new System.EventHandler(this.homepageToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(219, 6);
            // 
            // menuCmdHelpWiki
            // 
            this.menuCmdHelpWiki.Name = "menuCmdHelpWiki";
            this.menuCmdHelpWiki.Size = new System.Drawing.Size(222, 24);
            this.menuCmdHelpWiki.Text = "&Create your own tests...";
            this.menuCmdHelpWiki.Click += new System.EventHandler(this.menuCmdHelpCreate_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(219, 6);
            // 
            // menuCmdHelpAbout
            // 
            this.menuCmdHelpAbout.Name = "menuCmdHelpAbout";
            this.menuCmdHelpAbout.Size = new System.Drawing.Size(222, 24);
            this.menuCmdHelpAbout.Text = "&About...";
            this.menuCmdHelpAbout.Click += new System.EventHandler(this.menuCmdHelpAbout_Click);
            // 
            // statusContainer
            // 
            this.statusContainer.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusbarLabel});
            this.statusContainer.Location = new System.Drawing.Point(0, 349);
            this.statusContainer.Name = "statusContainer";
            this.statusContainer.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
            this.statusContainer.Size = new System.Drawing.Size(484, 24);
            this.statusContainer.TabIndex = 1;
            // 
            // statusbarLabel
            // 
            this.statusbarLabel.Name = "statusbarLabel";
            this.statusbarLabel.Size = new System.Drawing.Size(98, 19);
            this.statusbarLabel.Text = "statusbarLabel";
            // 
            // groupBoxFolder
            // 
            this.groupBoxFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxFolder.Controls.Add(this.buttonRevertFolderToDefault);
            this.groupBoxFolder.Controls.Add(this.buttonViewFolderInExplorer);
            this.groupBoxFolder.Controls.Add(this.labelFolderMessage);
            this.groupBoxFolder.Controls.Add(this.buttonSelectFolder);
            this.groupBoxFolder.Controls.Add(this.textBoxFolder);
            this.groupBoxFolder.Location = new System.Drawing.Point(16, 37);
            this.groupBoxFolder.Margin = new System.Windows.Forms.Padding(4);
            this.groupBoxFolder.Name = "groupBoxFolder";
            this.groupBoxFolder.Padding = new System.Windows.Forms.Padding(4);
            this.groupBoxFolder.Size = new System.Drawing.Size(456, 131);
            this.groupBoxFolder.TabIndex = 0;
            this.groupBoxFolder.TabStop = false;
            this.groupBoxFolder.Text = "Folder to load assets and tests from";
            // 
            // buttonRevertFolderToDefault
            // 
            this.buttonRevertFolderToDefault.Location = new System.Drawing.Point(7, 87);
            this.buttonRevertFolderToDefault.Name = "buttonRevertFolderToDefault";
            this.buttonRevertFolderToDefault.Size = new System.Drawing.Size(216, 32);
            this.buttonRevertFolderToDefault.TabIndex = 3;
            this.buttonRevertFolderToDefault.Text = "Revert to default";
            this.buttonRevertFolderToDefault.UseVisualStyleBackColor = true;
            this.buttonRevertFolderToDefault.Click += new System.EventHandler(this.buttonRevertFolderToDefault_Click);
            // 
            // buttonViewFolderInExplorer
            // 
            this.buttonViewFolderInExplorer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonViewFolderInExplorer.Location = new System.Drawing.Point(232, 87);
            this.buttonViewFolderInExplorer.Name = "buttonViewFolderInExplorer";
            this.buttonViewFolderInExplorer.Size = new System.Drawing.Size(216, 32);
            this.buttonViewFolderInExplorer.TabIndex = 4;
            this.buttonViewFolderInExplorer.Text = "View in Explorer...";
            this.buttonViewFolderInExplorer.UseVisualStyleBackColor = true;
            this.buttonViewFolderInExplorer.Click += new System.EventHandler(this.buttonViewFolderInExplorer_Click);
            // 
            // labelFolderMessage
            // 
            this.labelFolderMessage.AutoSize = true;
            this.labelFolderMessage.Location = new System.Drawing.Point(4, 49);
            this.labelFolderMessage.Name = "labelFolderMessage";
            this.labelFolderMessage.Size = new System.Drawing.Size(46, 17);
            this.labelFolderMessage.TabIndex = 2;
            this.labelFolderMessage.Text = "label1";
            // 
            // buttonSelectFolder
            // 
            this.buttonSelectFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSelectFolder.Location = new System.Drawing.Point(347, 18);
            this.buttonSelectFolder.Margin = new System.Windows.Forms.Padding(4);
            this.buttonSelectFolder.Name = "buttonSelectFolder";
            this.buttonSelectFolder.Size = new System.Drawing.Size(100, 32);
            this.buttonSelectFolder.TabIndex = 2;
            this.buttonSelectFolder.Text = "...";
            this.buttonSelectFolder.UseVisualStyleBackColor = true;
            this.buttonSelectFolder.Click += new System.EventHandler(this.buttonSelectFolder_Click);
            // 
            // textBoxFolder
            // 
            this.textBoxFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxFolder.Enabled = false;
            this.textBoxFolder.Location = new System.Drawing.Point(7, 23);
            this.textBoxFolder.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxFolder.Name = "textBoxFolder";
            this.textBoxFolder.Size = new System.Drawing.Size(332, 22);
            this.textBoxFolder.TabIndex = 1;
            // 
            // groupBoxReport
            // 
            this.groupBoxReport.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxReport.Controls.Add(this.buttonGenerateReport);
            this.groupBoxReport.Location = new System.Drawing.Point(16, 231);
            this.groupBoxReport.Name = "groupBoxReport";
            this.groupBoxReport.Size = new System.Drawing.Size(456, 106);
            this.groupBoxReport.TabIndex = 7;
            this.groupBoxReport.TabStop = false;
            this.groupBoxReport.Text = "Report";
            // 
            // buttonGenerateReport
            // 
            this.buttonGenerateReport.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonGenerateReport.Location = new System.Drawing.Point(69, 21);
            this.buttonGenerateReport.Name = "buttonGenerateReport";
            this.buttonGenerateReport.Size = new System.Drawing.Size(310, 71);
            this.buttonGenerateReport.TabIndex = 8;
            this.buttonGenerateReport.Text = "Generate and display report";
            this.buttonGenerateReport.UseVisualStyleBackColor = true;
            this.buttonGenerateReport.Click += new System.EventHandler(this.buttonGenerateReport_Click);
            // 
            // folderBrowserDialog
            // 
            this.folderBrowserDialog.ShowNewFolderButton = false;
            // 
            // groupBoxAdditonalText
            // 
            this.groupBoxAdditonalText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxAdditonalText.Controls.Add(this.textBoxUserText);
            this.groupBoxAdditonalText.Location = new System.Drawing.Point(16, 175);
            this.groupBoxAdditonalText.Name = "groupBoxAdditonalText";
            this.groupBoxAdditonalText.Size = new System.Drawing.Size(456, 50);
            this.groupBoxAdditonalText.TabIndex = 5;
            this.groupBoxAdditonalText.TabStop = false;
            this.groupBoxAdditonalText.Text = "Additonal text to be included on the report";
            // 
            // textBoxUserText
            // 
            this.textBoxUserText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxUserText.Location = new System.Drawing.Point(6, 21);
            this.textBoxUserText.Name = "textBoxUserText";
            this.textBoxUserText.Size = new System.Drawing.Size(441, 22);
            this.textBoxUserText.TabIndex = 6;
            // 
            // labelHorizontalLine
            // 
            this.labelHorizontalLine.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelHorizontalLine.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelHorizontalLine.Location = new System.Drawing.Point(0, 26);
            this.labelHorizontalLine.Name = "labelHorizontalLine";
            this.labelHorizontalLine.Size = new System.Drawing.Size(484, 10);
            this.labelHorizontalLine.TabIndex = 0;
            this.labelHorizontalLine.Text = "HORIZONTAL LINE";
            // 
            // MainForm
            // 
            this.AcceptButton = this.buttonGenerateReport;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 373);
            this.Controls.Add(this.labelHorizontalLine);
            this.Controls.Add(this.groupBoxAdditonalText);
            this.Controls.Add(this.groupBoxReport);
            this.Controls.Add(this.groupBoxFolder);
            this.Controls.Add(this.statusContainer);
            this.Controls.Add(this.menuStrip);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.830189F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(500, 415);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TestUtil";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.statusContainer.ResumeLayout(false);
            this.statusContainer.PerformLayout();
            this.groupBoxFolder.ResumeLayout(false);
            this.groupBoxFolder.PerformLayout();
            this.groupBoxReport.ResumeLayout(false);
            this.groupBoxAdditonalText.ResumeLayout(false);
            this.groupBoxAdditonalText.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.StatusStrip statusContainer;
        private System.Windows.Forms.ToolStripStatusLabel statusbarLabel;
        private System.Windows.Forms.ToolStripMenuItem menuFile;
        private System.Windows.Forms.ToolStripMenuItem menuCmdFileExit;
        private System.Windows.Forms.ToolStripMenuItem menuHelp;
        private System.Windows.Forms.ToolStripMenuItem menuCmdHelpHomepage;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem menuCmdHelpAbout;
        private System.Windows.Forms.GroupBox groupBoxFolder;
        private System.Windows.Forms.Button buttonSelectFolder;
        private System.Windows.Forms.TextBox textBoxFolder;
        private System.Windows.Forms.GroupBox groupBoxReport;
        private System.Windows.Forms.Label labelFolderMessage;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.Button buttonRevertFolderToDefault;
        private System.Windows.Forms.Button buttonViewFolderInExplorer;
        private System.Windows.Forms.Button buttonGenerateReport;
        private System.Windows.Forms.GroupBox groupBoxAdditonalText;
        private System.Windows.Forms.TextBox textBoxUserText;
        private System.Windows.Forms.ToolStripMenuItem menuCmdHelpWiki;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.Label labelHorizontalLine;
    }
}

