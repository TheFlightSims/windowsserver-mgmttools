using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace WSUSAdminAssistant
{
    public partial class frmCredentials : Form
    {
        public clsConfig cfg;

        public frmCredentials(clsConfig cfgobject)
        {
            InitializeComponent();

            cfg = cfgobject;
        }

        private void grdCredentials_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // If we're displaying the password column, reset the display to asterisks
            if (e.ColumnIndex == crPassword.Index && e.Value != null)
            {
                string v = e.Value.ToString();
                e.Value = v.Substring(0, 1) + new string('*', v.Length - 2) + v.Substring(v.Length - 1, 1);
            }
        }

        private void grdCredentials_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            DataGridViewRow r = grdCredentials.Rows[e.RowIndex];

            // Is this a new row?
            if (!r.IsNewRow)
                // No - check it
                ValidateRow(r);
        }

        public class SortByMaskedIP : System.Collections.IComparer
        {
            private int ipcol;
            private int netmaskcol;

            public SortByMaskedIP(int ipindex, int netmaskindex)
            {
                ipcol = ipindex;
                netmaskcol = netmaskindex;
            }

            // <summary>
            // Takes object ip addresses and network masks and tries to convert it to a fully masked subnet.
            // Validation is also performed - if either parameter is invalid, a null is returned.
            // <param name="ip">The IP address within the subnet to convert</param>
            // <param name="netmask">The netmask to convert.  This is expected to be the number of bits to mask (e.g. 24, not 255.255.255.0)</param>
            public bool MaskIPAddress(object ip, object netmask, out byte[] ipaddress, out byte networkmask)
            {
                // If either parameter is null, this is not a valid object
                if (ip == null || netmask == null)
                {
                    ipaddress = null;
                    networkmask = 0;
                    return false;
                }

                // Try to convert the IP address
                IPAddress ia;

                if (!IPAddress.TryParse(ip.ToString(), out ia))
                {
                    // Invalid IP address
                    ipaddress = null;
                    networkmask = 0;
                    return false;
                }

                // Try to convert the netmask
                byte nm;
                if (!byte.TryParse(netmask.ToString(), out nm))
                {
                    // Invalid netmask
                    ipaddress = null;
                    networkmask = 0;
                    return false;
                }

                // Convert the IP address to an array of bytes
                byte[] i = ia.GetAddressBytes();

                // Validate netmask
                if (nm > i.Length * 8)
                {
                    // Invalid netmask
                    ipaddress = null;
                    networkmask = 0;
                    return false;
                }

                // Mask IP address appropriately
                int bits = nm;

                for (int b = 0; b < i.Length; b++)
                {
                    if (bits > 8)
                        // No action required, move on to the next load of bits
                        bits -= 8;
                    else if (bits == 0)
                        // Mask out all bytes in this byte
                        i[b] = 255;
                    else
                    {
                        // Calculate what's left of the bitmask
                        int msk = (byte)(255 >> bits);

                        // Mask this byte appropriately
                        i[b] = (byte)(i[b] | msk);

                        // No bits left to process
                        bits = 0;
                    }
                }

                // Successfully masked IP address - return it.
                ipaddress = i;
                networkmask = nm;
                return true;
            }

            public int Compare(object x, object y)
            {
                DataGridViewRow r1 = (DataGridViewRow)x;
                DataGridViewRow r2 = (DataGridViewRow)y;

                // Try to convert the right columns to IP addresses
                byte[] i1, i2;
                byte nm1, nm2;

                bool v1 = MaskIPAddress(r1.Cells[ipcol].Value, r1.Cells[netmaskcol].Value, out i1, out nm1);
                bool v2 = MaskIPAddress(r2.Cells[ipcol].Value, r2.Cells[netmaskcol].Value, out i2, out nm2);

                // Invalid rows sink to the bottom
                if (!v1 && v2) return -1;
                if (v1 && !v2) return 1;
                if (!v1 && !v2) return 0;

                // If IP address lengths are different, sort by IPv4 first
                if (i1.Length > i2.Length) return -1;
                if (i1.Length < i2.Length) return 1;

                // Find first byte that differs and compare them
                for (int i = 0; i < i1.Length; i++)
                {
                    if (i1[i] > i2[i]) return 1;
                    if (i1[i] < i2[i]) return -1;
                }

                // Masked IPs are the same - sort by the most specific netmask
                if (nm1 > nm2) return -1;
                if (nm1 < nm2) return 1;

                // IPs and masks are exactly the same.
                return 0;
            }
        }

        private bool ValidateRow(DataGridViewRow r)
        {
            // Assume everything validates unless one of the following checks fails
            bool ok = true;
            foreach (DataGridViewCell c in r.Cells) c.ErrorText = "";

            // Check Network address is a valid IP address
            IPAddress ip;

            if (r.Cells["crNetwork"].Value == null || !IPAddress.TryParse(r.Cells["crNetwork"].Value.ToString(), out ip))
            {
                r.Cells["crNetwork"].ErrorText = "Invalid network address";
                ok = false;
            }
            else
            {
                // Check the netmask column is a valid number of bits
                byte netmask;
                DataGridViewCell c = r.Cells["crNetmask"];

                if (c.Value == null || !byte.TryParse(c.Value.ToString(), out netmask))
                {
                    c.ErrorText = "Invalid network mask (must be the number of bits - e.g. 24 not 255.255.255.0)";
                    ok = false;
                }
                else
                {
                    c.ErrorText = "";

                    // Check that the number of bits is correct for the type of IP address
                    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork && netmask > 32) { c.ErrorText = "IPv4 netmasks must be 32 bits or less"; ok = false; }
                    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6 && netmask > 128) { c.ErrorText = "IPv6 netmasks must be 128 bits or less"; ok = false; }
                }
            }

            // Check that there is a username and password supplied
            if (r.Cells["crUser"].Value == null || r.Cells["crUser"].Value.ToString() == "") { r.Cells["crUser"].ErrorText = "Username must be supplied"; ok = false; }
            if (r.Cells["crPassword"].Value == null || r.Cells["crPassword"].Value.ToString() == "") { r.Cells["crPassword"].ErrorText = "Password must be supplied - empty network passwords not supported"; ok = false; }

            return ok;
        }

        private clsConfig.CredentialCollection CollateForm()
        {
            clsConfig.CredentialCollection cc = new clsConfig.CredentialCollection();

            // Loop through each row and (if it's valid) add it to the collection
            foreach (DataGridViewRow r in grdCredentials.Rows)
            {
                if (!r.IsNewRow && ValidateRow(r))
                {
                    clsConfig.SecurityCredential c = new clsConfig.SecurityCredential();

                    c.ip = IPAddress.Parse(r.Cells[crNetwork.Index].Value.ToString()).ToString();
                    c.netmask = byte.Parse(r.Cells[crNetmask.Index].Value.ToString());
                    c.description = r.Cells[crDescription.Index].Value.ToString();

                    if (r.Cells[crDomain.Index].Value == null)
                        c.domain = "";
                    else
                        c.domain = r.Cells[crDomain.Index].Value.ToString();

                    c.username = r.Cells[crUser.Index].Value.ToString();
                    c.password = r.Cells[crPassword.Index].Value.ToString();

                    cc.Add(c);
                }
            }

            return cc;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Sort the datagrid
            grdCredentials.Sort(new SortByMaskedIP(crNetwork.Index, crNetmask.Index));

            // Validate each row
            bool ok = true;

            foreach (DataGridViewRow r in grdCredentials.Rows)
                if (!r.IsNewRow && !ValidateRow(r)) { ok = false; break; }

            // Save without prompting if all rows are OK.  Prompt if not all rows are OK.
            if (ok || (!ok && MessageBox.Show("Not all rows are valid - rows with errors (highlighted) will not be saved if you continue.  Save security credentials?", "Not all rows are valid", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes))
            {
                clsConfig.CredentialCollection cc = CollateForm();

                // Sort the collection, then save it.
                cc.SortByMaskedIP();

                cfg.CredentialList = cc;

                // Close form
                this.Close();
            }
        }

        private void frmCredentials_Load(object sender, EventArgs e)
        {
            // Populate grid
            clsConfig.CredentialCollection cc = cfg.CredentialList;

            // Sort the collection before populating the grid
            cc.SortByMaskedIP();

            foreach (clsConfig.SecurityCredential c in cc)
            {
                DataGridViewRow r = grdCredentials.Rows[grdCredentials.Rows.Add()];

                r.Cells["crNetwork"].Value = c.ip.ToString();
                r.Cells["crNetmask"].Value = c.netmask;
                r.Cells["crDescription"].Value = c.description;
                r.Cells["crDomain"].Value = c.domain;
                r.Cells["crUser"].Value = c.username;
                r.Cells["crPassword"].Value = c.password;
            }

            // Sort the datagrid
            grdCredentials.Sort(new SortByMaskedIP(crNetwork.Index, crNetmask.Index));

            // Set default selected cell to the first column of the last row
            grdCredentials.CurrentCell = grdCredentials.Rows[grdCredentials.Rows.Count - 1].Cells[0];
        }

        private void grdCredentials_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Sort the datagrid
            grdCredentials.Sort(new SortByMaskedIP(crNetwork.Index, crNetmask.Index));
        }
    }
}
