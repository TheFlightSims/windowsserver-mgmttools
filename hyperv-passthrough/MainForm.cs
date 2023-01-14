/* 
 Adding required libraries
 */
using Microsoft.Management.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Forms;
using DeviceData = System.Tuple<Microsoft.HyperV.PowerShell.VirtualMachine, Microsoft.HyperV.PowerShell.VMAssignedDevice>;

namespace DiscreteDeviceAssigner
{
    public partial class MainForm : Form
    { 
        public MainForm()
        {
            InitializeComponent();
        }

        //Update virtual machine and device display
        private void UpdateVM()
        {
            listView1.Groups.Clear();
            listView1.Items.Clear();

            //Get the list of virtual machines
            var vms = PowerShellWrapper.GetVM();
            var groups = new List<ListViewGroup>();
            //Display VM name and its state
            foreach (var vm in vms)
            {
                ListViewGroup group = new ListViewGroup("[State: " + vm.State + "] " + vm.Name);
                groups.Add(group);
            }

            //Get the list of devices under each virtual machine
            var lviss = new List<ListViewItem>[vms.Count];
            _ = Parallel.For(0, vms.Count, (int i) =>
            {
                var vm = vms[i];
                var group = groups[i];
                lviss[i] = new List<ListViewItem>();
                var lvis = lviss[i];
                foreach (var dd in PowerShellWrapper.GetVMAssignableDevice(vm))
                {
                    var dev = PowerShellWrapper.GetPnpDevice(dd.InstanceID);
                    string name = dd.Name;

                    //Filter any Plug-n-Play hardware is already mounted into the VM
                    string clas = dev.CimInstanceProperties["PnpClass"] != null ? dev.CimInstanceProperties["PnpClass"].Value as string : null;
                    
                    lvis.Add(new ListViewItem(new string[] { name != null ? name : "", clas != null ? clas : "", dd.LocationPath }, group)
                    {
                        Tag = new DeviceData(vm, dd),
                    });
                }
                lvis.Add(new ListViewItem("...", group)
                {
                    Tag = new DeviceData(vm, null),
                });
            });

            //Refresh List
            listView1.BeginUpdate();
            foreach (ListViewGroup group in groups)
            {
                listView1.Groups.Add(group);
            }
            foreach (var lvis in lviss)
            {
                foreach (var lvi in lvis)
                {
                    listView1.Items.Add(lvi);
                }
            }
            listView1.EndUpdate();
        }

        //Loading event
        private async void Form1_Load(object sender, EventArgs e)
        {
            await Task.Delay(1);
            UpdateVM();
        }

        //Right - click the menu
        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (listView1.SelectedItems.Count != 0)
                {
                    DeviceData data = listView1.SelectedItems[0].Tag as DeviceData;
                    contextMenuStrip.Tag = data;
                    contextMenuStrip.Items[0].Text = data.Item1.Name;
                    contextMenuStrip.Show(sender as Control, e.Location);
                }
            }
        }

        //Right-click the menu to call out event
        private void contextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            DeviceData data = contextMenuStrip.Tag as DeviceData;
            if (data.Item2 == null)
            {
                移除设备ToolStripMenuItem.Enabled = false;
                复制地址toolStripMenuItem.Enabled = false;
            }
            else
            {
                移除设备ToolStripMenuItem.Enabled = true;
                复制地址toolStripMenuItem.Enabled = true;
            }
            uint lowMMIO = 0;
            try
            {
                //This sentence will be inexplicable
                lowMMIO = data.Item1.LowMemoryMappedIoSpace;
            }
            catch { }
            LMMIOtoolStripTextBox.Text = (lowMMIO / 1024 / 1024).ToString();
            HMMIOtoolStripTextBox.Text = (data.Item1.HighMemoryMappedIoSpace / 1024 / 1024).ToString();
            GCCTtoolStripMenuItem.Checked = data.Item1.GuestControlledCacheTypes;
        }

        //Add device
        private void 添加设备ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeviceData data = contextMenuStrip.Tag as DeviceData;
            CimInstance dev = new PnpDeviceForm().GetResult();
            if (dev != null)
            {
                string name = dev.CimInstanceProperties["Name"] != null ? dev.CimInstanceProperties["Name"].Value as string : null;
                if (name == null) name = "";
                //Display confirm dialog box
                if (MessageBox.Show("Add this device: " + name + " to this following virtual machine: " + data.Item1.Name, "Confirm?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    try
                    {
                        PowerShellWrapper.AddVMAssignableDevice(data.Item1, dev);
                    }
                    //Display an error if none of action can happen
                    catch (Exception ex)
                    {   
                        MessageBox.Show(ex.Message, "Error");
                    }
                    UpdateVM();
                }
            }
        }

        //Removal device from specific virtual machine
        private void 移除设备ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeviceData data = contextMenuStrip.Tag as DeviceData;
            if (MessageBox.Show("Perform removal device " + data.Item2.Name + " from " + data.Item1.Name, "Confirm?", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    PowerShellWrapper.RemoveVMAssignableDevice(data.Item1, data.Item2);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error");
                }
                UpdateVM();
            }
        }

        //Replication address
        private void 复制地址ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeviceData data = contextMenuStrip.Tag as DeviceData;
            Clipboard.SetText(data.Item2.LocationPath);
        }

        //refresh the list
        private void 刷新列表ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateVM();
        }

        //GuestControlledCacheTypes
        private void GCCTtoolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeviceData data = contextMenuStrip.Tag as DeviceData;
            try
            {
                PowerShellWrapper.SetGuestControlledCacheTypes(data.Item1, !GCCTtoolStripMenuItem.Checked);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        //HighMemoryMappedIoSpace
        private void HMMIOtoolStripTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                DeviceData data = contextMenuStrip.Tag as DeviceData;
                ulong mb;
                if (ulong.TryParse(HMMIOtoolStripTextBox.Text, out mb))
                {
                    var vm = data.Item1;
                    ulong bytes = mb * 1024 * 1024;
                    if (bytes != vm.HighMemoryMappedIoSpace && bytes != 0)
                    {
                        try
                        {
                            PowerShellWrapper.SetHighMemoryMappedIoSpace(vm, bytes);
                            //Success
                            contextMenuStrip.Close();
                            return;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Error");
                        }
                    }
                }

                //Failed
                HMMIOtoolStripTextBox.Text = (data.Item1.HighMemoryMappedIoSpace / 1024 / 1024).ToString();
            }
        }

        //LowMemoryMappedIoSpace
        private void LMMIOtoolStripTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                DeviceData data = contextMenuStrip.Tag as DeviceData;
                uint mb;
                if (uint.TryParse(LMMIOtoolStripTextBox.Text, out mb))
                {
                    var vm = data.Item1;
                    uint bytes = mb * 1024 * 1024;
                    uint lowMMIO = 0;
                    try
                    {
                        //This sentence will be inexplicable
                        lowMMIO = data.Item1.LowMemoryMappedIoSpace;
                    }
                    catch { }
                    if ((lowMMIO == 0 || bytes != lowMMIO) && bytes != 0)
                    {
                        try
                        {
                            PowerShellWrapper.SetLowMemoryMappedIoSpace(vm, bytes);
                            //Success
                            contextMenuStrip.Close();
                            return;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Error");
                        }
                    }
                }

                //Failed
                LMMIOtoolStripTextBox.Text = (data.Item1.LowMemoryMappedIoSpace / 1024 / 1024).ToString();
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void 其它toolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void highMemoryMappedIoSpaceToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}