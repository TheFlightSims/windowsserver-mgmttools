using System;
using System.Windows.Forms;

namespace HyperVpassthroughdev
{
    public partial class SetMemory : Form
    {
        public SetMemory()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void HighMemchanges(object sender, EventArgs e)
        {

        }

        public void OKClick(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToInt32(HighMem.Text) < Convert.ToInt32("512"))
                {
                    MessageBox.Show($"The high memory location must be higher than 512Mib", $"Error", MessageBoxButtons.OK);
                }
                else
                {
                    Close();
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show($"An error has occurred: \n" + exp, "Error", MessageBoxButtons.OK);
            }

        }

        public UInt32 ReturnResult()
        {
            ShowDialog();
            return (uint)Convert.ToInt32(HighMem.Text);
        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }
    }
}
