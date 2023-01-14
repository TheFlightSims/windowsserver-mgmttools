using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace WSUSAdminAssistant
{
    public partial class frmDefaultSUS : Form
    {
        private clsConfig cfg;

        public frmDefaultSUS(clsConfig cfgobject)
        {
            InitializeComponent();

            cfg = cfgobject;

            // Add each default GUID from the existing configuration
            foreach (string s in cfg.DefaultSusIDCollection)
                lstGUIDs.Items.Add(s);
        }

        private void txtSusID_TextChanged(object sender, EventArgs e)
        {
            // If we have a valid GUID, enable the Add button
            GuidConverter c = new GuidConverter();

            btnAdd.Enabled = c.IsValid(txtSusID.Text);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Check all items to ensure we don't already have the GUID in the list
            bool found = false;

            foreach (var i in lstGUIDs.Items)
            {
                if (i.ToString() == txtSusID.Text)
                {
                    // GUID already exists - note item and break
                    lstGUIDs.SelectedItem = txtSusID.Text;
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                lstGUIDs.Items.Add(txtSusID.Text);
                lstGUIDs.SelectedItem = txtSusID.Text;
            }
            
            txtSusID.Clear();
        }

        private void txtSusID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                btnAdd.PerformClick();
        }

        private void lstGUIDs_DoubleClick(object sender, EventArgs e)
        {
            // Remove selected item and add it to the textbox
            txtSusID.Text = lstGUIDs.SelectedItem.ToString();
            lstGUIDs.Items.Remove(txtSusID.Text);
            txtSusID.Focus();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            // Remove selected items
            for (int i = lstGUIDs.SelectedIndices.Count - 1; i >= 0; i--)
            {
                lstGUIDs.Items.RemoveAt(lstGUIDs.SelectedIndices[i]);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            // Close the form without saving
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Save items and close form
            cfg.DefaultSusIDCollection = lstGUIDs.Items.Cast<string>().ToArray();
            this.Close();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            // If an item is selected, remove it from the list and add it to the textbox
            if (lstGUIDs.SelectedItem != null)
            {
                txtSusID.Text = lstGUIDs.SelectedItem.ToString();
                lstGUIDs.Items.Remove(txtSusID.Text);
                txtSusID.Focus();
            }
        }

        private void lstGUIDs_KeyDown(object sender, KeyEventArgs e)
        {
            // If we got a delete key, delete the current item.
            if (e.KeyCode == Keys.Delete)
                btnDelete.PerformClick();
        }
    }
}
