using System;
using System.Windows.Forms;

namespace HyperVpassthroughdev
{
    public partial class SetMemory : Form
    {
        public SetMemory()
        {
            InitializeComponent();
            ShowDialog();
        }

        private void HighMemchanges(object sender, EventArgs e)
        {

        }

        public void OKClick(object sender, EventArgs e)
        {
            Close();
        }

        public UInt32[] ReturnResult()
        {
            UInt32 LowMemSet = (uint)Convert.ToInt32(LowMem.Text);
            UInt32 HighMemSet = (uint)Convert.ToInt32(HighMem.Text);
            UInt32[] Memory = { LowMemSet, HighMemSet };
            return Memory;
        }

        private void LowMemChanges(object sender, EventArgs e)
        {

        }

        private void labelhimem_Click(object sender, EventArgs e)
        {

        }
    }
}
