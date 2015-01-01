using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Yamua;

namespace TestUtilGUI
{
    public partial class MonospacedTextForm : Form
    {
        public MonospacedTextForm()
        {
            InitializeComponent();
        }

        //Title for this window
        public string Title { get; set; }

        //Content to be displayed
        public string Content { get; set; }


        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void richTextBox_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            ExecuteAndForget.Execute(e.LinkText);
        }

        private void MonospacedTextForm_Shown(object sender, EventArgs e)
        {
            FormFontFixer.Fix(this);            

            if (string.IsNullOrWhiteSpace(Title) == false)
            {
                this.Text = Title;
            }

            richTextBox.Text = "No content provided";

            //Set richtextbox to monospaced font. +1 to make it a little bigger
            //We need to do this BEFORE we add content or we need to work with .SelectAll(), .SelectFont= etc.
            richTextBox.Font = new Font(FontFamily.GenericMonospace, richTextBox.Font.Size + 1); 

            if (string.IsNullOrWhiteSpace(Content) == false)
            {
                richTextBox.Text = Content;
            }

            //Debug information to display currently used font
            //MessageBox.Show(richTextBox.Font.ToString());
        }
    }
}
