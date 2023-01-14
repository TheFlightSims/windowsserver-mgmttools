using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Microsoft.UpdateServices;
using Microsoft.UpdateServices.Administration;

namespace WSUSAdminAssistant
{
    public partial class frmComputerGroupRules : Form
    {
        clsConfig cfg;
        clsWSUS wsus;

        public frmComputerGroupRules(clsConfig cfgobject)
        {
            InitializeComponent();

            // Set Priority column as an integer
            rxPriority.ValueType = typeof(int);

            cfg = cfgobject;
            wsus = cfg.wsus;
        }

        private void frmComputerGroupRules_Load(object sender, EventArgs e)
        {
            List<string> groups = new List<string>();

            // Connect to WSUS and update group list
            foreach (IComputerTargetGroup g in wsus.server.GetComputerTargetGroups())
                groups.Add(g.Name);

            // Sort groups then add them to the datagrid
            groups.Sort();

            foreach(string group in groups)
                rxComputerGroup.Items.Add(group);

            rxComputerGroup.Sorted = true;
            rxComputerGroup.SortMode = DataGridViewColumnSortMode.Automatic;

            // Get collection of RegEx rules, sort by priority and populate the table
            foreach (clsConfig.ComputerGroupRegEx rx in cfg.ComputerRegExList)
            {
                int i = grdRegEx.Rows.Add();
                DataGridViewRow r = grdRegEx.Rows[i];

                r.Cells["rxPriority"].Value = rx.Priority;
                r.Cells["rxComputerRegEx"].Value = rx.ComputerNameRegex;
                r.Cells["rxIPRegEx"].Value = rx.IPRegex;
                r.Cells["rxComputerGroup"].Value = rx.ComputerGroup;
                r.Cells["rxComment"].Value = rx.Comment;
                r.Cells["rxEnabled"].Value = rx.Enabled;
            }

            SortByPriority();
        }

        private bool IsValidRegEx(object pattern)
        {
            if (pattern == null) return true;

            try
            {
                Regex.Match("", (string)pattern);
            }
            catch (ArgumentException)
            {
                return false;
            }
            
            return true;
        }

        private string ValidateRow(DataGridViewRow row)
        {
            // Assume row is valid
            string valid = "OK";
            int i;

            // Check each cell is actually valid.  If not, the whole row isn't valid
            try { if (!int.TryParse(row.Cells["rxPriority"].Value.ToString(), out i)) valid = "Non-numeric Priority"; }
            catch { valid = "Non-numeric Priority"; }

            if (!IsValidRegEx(row.Cells["rxComputerRegEx"].Value)) valid = "Invalid Computer Name Regex Expression";
            if (!IsValidRegEx(row.Cells["rxIPRegEx"].Value)) valid = "Invalid IP Address Regex Expression";
            if (row.Cells["rxComputerRegEx"].Value == null && row.Cells["rxIPRegEx"].Value == null) valid = "No Computer Name or IP Address Regex Expression Supplied";
            if (row.Cells["rxComputerGroup"].Value == null || row.Cells["rxComputerGroup"].Value.ToString() == "") valid = "Empty Computer Group";
            
            return valid;
        }

        private bool ValidateGrid()
        {
            // Ensure form is commited before validation
            this.Validate();

            bool grdok = true;
            // Loop through each line in the form and validate it.
            foreach (DataGridViewRow r in grdRegEx.Rows)
            {
                string valid = ValidateRow(r);

                // If the cells validates OK or is the new item row
                if (valid == "OK" || r.IsNewRow)
                    // No problem, clear error text on form
                    r.ErrorText = "";
                else
                {
                    // Problem found - supply details and note invalid result
                    r.ErrorText = valid;
                    grdok = false;
                }
            }

            return grdok;
        }

        private clsConfig.ComputerGroupRegexCollection CollateForm()
        {
            clsConfig.ComputerGroupRegexCollection c = new clsConfig.ComputerGroupRegexCollection();
            // Resort grid
            SortByPriority();

            // Loop through each line in the form and add it if it's valid.  
            foreach (DataGridViewRow r in grdRegEx.Rows)
            {
                if (ValidateRow(r) == "OK" && !r.IsNewRow)
                {
                    clsConfig.ComputerGroupRegEx rx = new clsConfig.ComputerGroupRegEx();

                    rx.Priority = int.Parse(r.Cells["rxPriority"].Value.ToString());
                    rx.ComputerNameRegex = (string)r.Cells["rxComputerRegEx"].Value;
                    rx.IPRegex = (string)r.Cells["rxIPRegEx"].Value;
                    rx.ComputerGroup = (string)r.Cells["rxComputerGroup"].Value;
                    rx.Comment = (string)r.Cells["rxComment"].Value;
                    if (r.Cells["rxEnabled"].Value == null)
                        rx.Enabled = false;
                    else
                        rx.Enabled = (bool)r.Cells["rxEnabled"].Value;

                    c.Add(rx);
                }
            }

            return c;
        }

        private void SortByPriority()
        {
            grdRegEx.Columns["rxPriority"].SortMode = DataGridViewColumnSortMode.Automatic;
            grdRegEx.Sort(grdRegEx.Columns["rxPriority"], ListSortDirection.Ascending);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SortByPriority();

            // Check to see if grid is valid
            if (ValidateGrid())
            {
                // Get grid and save it to the XML file
                clsConfig.ComputerGroupRegexCollection c = CollateForm();

                cfg.ComputerRegExList = c;
            }
        }

        private void grdRegEx_RowLeave(object sender, DataGridViewCellEventArgs e)
        {
            // When the user leaves the current grid, revalidate the form
            ValidateGrid();
            grdRegEx.AutoResizeColumns();
        }

        private void btnTestRule_Click(object sender, EventArgs e)
        {
            // Ensure changes are committed to the datagrid
            this.Validate();

            // Get currently selected rule
            DataGridViewRow r = grdRegEx.CurrentRow;

            // Clear the list and start searching for PCs
            lstResults.Items.Clear();

            // Get list of PCs and which groups they're in
            DataTable t = wsus.GetComputerGroups();
            
            foreach (DataRow d in t.Rows)
            {
                // Assume match unless all of the non-null rules match
                bool matched = true;

                if (r.Cells["rxComputerRegEx"].Value != null && r.Cells["rxComputerRegEx"].Value.ToString() != "")
                {
                    Match m = Regex.Match(d["name"].ToString(), r.Cells["rxComputerRegEx"].Value.ToString());
                    if (!m.Success) matched = false;
                }

                if (r.Cells["rxIPRegEx"].Value != null && r.Cells["rxIPRegEx"].Value.ToString() != "")
                {
                    Match m = Regex.Match(d["ipaddress"].ToString(), r.Cells["rxIPRegEx"].Value.ToString());
                    if (!m.Success) matched = false;
                }

                if (matched)
                    lstResults.Items.Add(d["name"].ToString() + " (" + d["ipaddress"].ToString() + ") matched, currently in computer group " + d["groupname"].ToString());
            }

            // Did any PCs match?
            if (lstResults.Items.Count == 0) lstResults.Items.Add("No PCs matched this rule");
        }

        private void btnPCsNotCovered_Click(object sender, EventArgs e)
        {
            // Ensure changes are committed to the datagrid
            this.Validate();

            // Clear the list and start searching for PCs
            lstResults.Items.Clear();

            // Collate list of rules
            clsConfig.ComputerGroupRegexCollection rules = CollateForm();

            // Get list of PCs and which groups they're in
            DataTable t = wsus.GetComputerGroups();

            // Loop through the list of PCs and see if they match a rule
            foreach (DataRow d in t.Rows)
            {
                bool matched = false;

                // Loop through each rule and see if this PC matches
                foreach (clsConfig.ComputerGroupRegEx rx in rules)
                {
                    // Assume match unless all of the non-null rules match
                    matched = true;

                    if (rx.ComputerNameRegex != null && rx.ComputerNameRegex != "")
                    {
                        Match m = Regex.Match(d["name"].ToString(), rx.ComputerNameRegex);
                        if (!m.Success) matched = false;
                    }

                    if (rx.IPRegex != null && rx.IPRegex != "")
                    {
                        Match m = Regex.Match(d["ipaddress"].ToString(), rx.IPRegex);
                        if (!m.Success) matched = false;
                    }

                    if (matched) 
                        // We found a matching rule - break from loop
                        break;
                }

                // Did any rule match?
                if (!matched)
                    lstResults.Items.Add(d["name"].ToString() + " (" + d["ipaddress"].ToString() + ") not matched by any existing rule");
            }

            // Did any PCs fail to match?
            if (lstResults.Items.Count == 0) lstResults.Items.Add("All PCs matched an available rule (Please note, this includes disabled rules)");
        }

        private void frmComputerGroupRules_FormClosing(object sender, FormClosingEventArgs e)
        {
            // If Windows is shutting down, don't object
            if (e.CloseReason == CloseReason.WindowsShutDown)
                e.Cancel = false;
            else
            {
                // Otherwise check if form has changed - if it has, it may need to be saved first.
                // *** WORK TO DO *** 
            }
        }
    }
}
