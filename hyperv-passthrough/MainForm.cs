﻿using HyperVpassthroughdev;
using Microsoft.Management.Infrastructure;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using DeviceData = System.Tuple<Microsoft.HyperV.PowerShell.VirtualMachine, Microsoft.HyperV.PowerShell.VMAssignedDevice>;

namespace HyperVPassthoughDevice
{
    public partial class MainForm : Form
    {
        private DialogResult messchk;

        public MainForm()
        {
            InitializeComponent();
        }

        private void UpdateVM()
        {
            listView1.Groups.Clear();
            listView1.Items.Clear();

            var vms = PowerShellWrapper.GetVM();
            var groups = new List<ListViewGroup>();
            foreach (var vm in vms)
            {
                ListViewGroup group = new ListViewGroup("[State: " + vm.State + "] " + vm.Name);
                groups.Add(group);
            }

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

        private async void Form1_Load(object sender, EventArgs e)
        {
            await Task.Delay(1);
            UpdateVM();
        }

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

        private void 添加设备ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeviceData data = contextMenuStrip.Tag as DeviceData;
            CimInstance dev = new PnpDeviceForm().GetResult();
            if (dev != null)
            {
                string name = dev.CimInstanceProperties["Name"] != null ? dev.CimInstanceProperties["Name"].Value as string : null;
                if (name == null) name = "";
                messchk = MessageBox.Show("Add this device: " + name + " to this following virtual machine: " + data.Item1.Name, "Confirm?", MessageBoxButtons.YesNo);
                if (messchk == DialogResult.Yes)
                {
                    if (Convert.ToString(dev.CimInstanceProperties["PnpClass"]).Contains("Display") == true)
                    {
                        if (MessageBox.Show($"The selected device is the GPU. Do you want to pass through it using DDA mode?", $"Prompt for DDA", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            try
                            {
                                MessageBox.Show($"The GPU Passthrough is renewed to the DDA method, but still under development.", $"Note", MessageBoxButtons.OK);
                                PowerShellWrapper.GpuPartitioning(data.Item1, dev);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message, "Error");
                            }
                            UpdateVM();
                        }
                    }
                    else
                    {
                        try
                        {
                            PowerShellWrapper.AddVMAssignableDevice(data.Item1, dev);
                        }
                        catch (Exception ex)
                        {
                            if (Convert.ToString(data.Item1.State).Contains("Running") == true)
                            {
                                MessageBox.Show($"You need to turn off the virtual machine first. \n" +
                                    $"The specific virtual machine is running.", $"Error", MessageBoxButtons.OK);
                            }
                            MessageBox.Show(ex.Message, "Error");
                        }
                        UpdateVM();
                    }
                }
            }
        }

        private void 移除设备ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                MessageBox.Show($"The specific VM has no device selected, nor the error has occured. \nError:" + ex.Message, $"Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void memchangeloc(object sender, EventArgs e)
        {
            DeviceData data = contextMenuStrip.Tag as DeviceData;
            try
            {
                UInt32[] LowHighCollection = new SetMemory().ReturnResult();
                UInt32 MemorySetLow = LowHighCollection[0];
                UInt32 MemorySetHigh = LowHighCollection[1];
                PowerShellWrapper.SetLowMemoryMappedIoSpace(data.Item1, MemorySetLow * 1024 * 1024);
                PowerShellWrapper.SetHighMemoryMappedIoSpace(data.Item1, MemorySetHigh * 1024 * 1024);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK);
            }

        }

        private void RemoveAllDev(object sender, EventArgs e)
        {
            if (MessageBox.Show($"This will stop all virtual machines and perform the removal of all assigned device.\nContinue?", $"Warning", MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
            {
                try
                {
                    PowerShellWrapper.StartRemovalAssignedDevices();
                    UpdateVM();
                }
                catch (Exception exception)
                {
                    MessageBox.Show($"An error has occured:\n\n" + exception, $"Error", MessageBoxButtons.OK);
                }
            }
        }

        private void CheckAssignableDevice(object sender, EventArgs e)
        {
            new DisplayAssignableDevices();
        }

        /*
         * Security functions
         */
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void 其它toolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void contextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }
    }
}