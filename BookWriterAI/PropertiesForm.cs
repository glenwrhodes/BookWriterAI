using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BookWriterAI
{
    public partial class PropertiesForm : Form
    {

        public string ApiKey;
        public string Model;
        public double Temperature;

        public PropertiesForm()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void PropertiesForm_Load(object sender, EventArgs e)
        {
            ApiKeyTextBox.Text = ApiKey;
            ModeTextBox.Text = Model;
            TemperatureNumUpDown.Value = (decimal)Temperature;
        }

        private void TemperatureNumUpDown_ValueChanged(object sender, EventArgs e)
        {
            Temperature = (double)TemperatureNumUpDown.Value;
        }

        private void ApiKeyTextBox_TextChanged(object sender, EventArgs e)
        {
            ApiKey = ApiKeyTextBox.Text;
        }

        private void ModeTextBox_TextChanged(object sender, EventArgs e)
        {
            Model = ModeTextBox.Text;
        }
    }
}
