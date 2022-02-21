using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PSParseGUI.Forms
{
    public partial class ConfigurationForm : Form
    {
        public ConfigurationForm()
        {
            InitializeComponent();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void SetDefault(string HandMatch, string TableMatch)
        {
            HandMatchTextBox.Text = HandMatch;
            TableMatchTextBox.Text = TableMatch;
        }

        public Tuple<string,string> GetResult()
        {
            return new Tuple<string, string>(HandMatchTextBox.Text, TableMatchTextBox.Text); 
        }
        private void ConfigurationForm_Shown(object sender, EventArgs e)
        {
            TableMatchTextBox.Focus();
        }
    }
}
