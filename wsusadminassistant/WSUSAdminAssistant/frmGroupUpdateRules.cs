using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Microsoft.UpdateServices.Administration;

namespace WSUSAdminAssistant
{
    public partial class frmGroupUpdateRules : Form
    {
        private clsConfig cfg;
        private clsWSUS wsus;

        private clsConfig.GroupUpdateRuleCollection grouprules;
        private ComputerTargetGroupCollection gc;
        private List<string> groupnames;

        private TreeNode root;
        private int minheight;

        public frmGroupUpdateRules(clsConfig cfgobject, clsWSUS wsusobject)
        {
            // Let the user know something is happening
            Cursor.Current = Cursors.WaitCursor;

            InitializeComponent();

            cfg = cfgobject;
            wsus = wsusobject;

            // Get list of computer groups and sort it.
            gc = wsus.ComputerGroups;
            groupnames = new List<string>();

            foreach (IComputerTargetGroup tg in gc) groupnames.Add(tg.Name);

            groupnames.Sort();

            // Read existing group rules
            grouprules = cfg.GroupUpdateRules;

            // Update treeview with existing rules
            UpdateGroupUpdateRules();

            // Set default display order (maximum + 1)
            numDisplayOrder.Value = grouprules.MaxDisplayOrder + 1;

            // Set this window to be marginly less wide than the current monitor
            Rectangle scr = Screen.FromControl(this).Bounds;

            // Make this window 20 pixels narrower than the current screen and centre it
            this.Left = 10;
            this.Width = scr.Width - 20;

            // Note the current height of the form - this will be the minimum height of the form
            minheight = this.Height;

            // Finished - reset cursor to normal
            Cursor.Current = Cursors.Arrow;
        }

        private string TimeSpanReadable(TimeSpan ts)
        {
            if (ts.Days > 0 && (ts.Days % 7) == 0)
                // Number of days over zero are divisible by 7 - we're talking weeks
                return (ts.Days / 7).ToString() + " week(s)";

            if (ts.Days > 7 && (ts.Days % 7) != 0)
                // Interval is weeks and days
                return (ts.Days / 7).ToString() + " week(s), " + (ts.Days % 7).ToString() + " day(s)";

            if (ts.Days > 0 && ts.Days < 7 && ts.Hours == 0)
                // Interval is in days only
                return ts.Days.ToString() + " day(s)";

            if (ts.Days > 0 && ts.Hours != 0)
                // Interval is days and hours
                return ts.Days.ToString() + " day(s), " + ts.Hours.ToString() + " hour(s)";

            if (ts.Days == 0 && ts.Hours > 0 && ts.Minutes == 0)
                // Interval is hours only
                return ts.Hours.ToString() + " hour(s)";

            if (ts.Hours > 0 && ts.Minutes > 0)
                // Interval is in hours and minutes
                return ts.Hours.ToString() + " hours(s), " + ts.Minutes.ToString() + " minute(s)";

            if (ts.Hours == 0 && ts.Minutes > 0)
                // Interval is in minutes only
                return ts.Minutes.ToString() + " minute(s)";

            if (ts == new TimeSpan(0, 0, 0))
                // Immedite
                return "immediately";

            // Unknown format - return standard TimeSpan string
            return ts.ToString();
        }
            
        private void UpdateGroupUpdateRules()
        {
            // Sort rules before displaying
            grouprules.SortByDisplayOrder();

            // Disable redrawing treeview while we clear it and update with current rules
            trvGroupRules.BeginUpdate();
            trvGroupRules.Nodes.Clear();

            // Add root node and start going through all computer groups
            root = trvGroupRules.Nodes.Add("Release Day");
            AddChildGroupNodes(root, null, 0);

            // Expand tree view
            trvGroupRules.ExpandAll();

            // Redraw the tree now that we've finished updating
            trvGroupRules.EndUpdate();

            // Populate group combo box (parent box will be populated when a group is selected)
            RepopulateGroupList();
        }

        private void AddChildGroupNodes(TreeNode node, IComputerTargetGroup parent, int depth)
        {
            if (depth > 10)
            {
                // We're recursing too much - advise end user no further levels will be added and that loop recursion should be checked for
                MessageBox.Show("Computer group rules may be no more than 15 levels deep.  No further levels will be added.  Please check for recursive loops.", "Update Rule Recursion", MessageBoxButtons.OK);
                return;
            }

            // Loop through all computer groups, checking for groups with the specified parent
            foreach (clsConfig.GroupUpdateRule gu in grouprules)
            {
                if ((gu.parentcomputergroup == null && parent == null) || (gu.parentcomputergroup != null && parent != null && gu.parentcomputergroup.Id == parent.Id))
                {
                    // Describe the group
                    string nodedesc;

                    if (gu.parentcomputergroup == null)
                        // Child of root node
                        nodedesc = gu.computergroup.Name + ": short name \"" + gu.shortname + "\", display order " + gu.displayorder.ToString() + ", sort weight " + gu.sortweight.ToString() + ", updates approvable " + TimeSpanReadable(gu.updateinterval) + " after being received by root WSUS server";
                    else
                        // Child of another group
                        nodedesc = gu.computergroup.Name + ": short name \"" + gu.shortname + "\", display order " + gu.displayorder.ToString() + ", sort weight " + gu.sortweight.ToString() + ", updates approvable " + TimeSpanReadable(gu.updateinterval) +
                            " after being approved for " + gu.parentcomputergroup.Name + ", or " + TimeSpanReadable(gu.childupdateinterval) + " after updates for " +
                            gu.parentcomputergroup.Name + " were approvable if no computers in this group require the update";

                    // Add a tree node and recurse for children
                    TreeNode tn = node.Nodes.Add(nodedesc);
                    tn.Name = gu.computergroup.Name;
                    tn.Tag = gu;

                    AddChildGroupNodes(tn, gu.computergroup, depth + 1);
                }
            }
        }

        private void RepopulateGroupList()
        {
            // Clear the group combo box and repopulate only with nodes not already in the rule tree
            cboComputerGroup.Items.Clear();

            foreach (string g in groupnames)
            {
                // Look for the current group
                TreeNode[] tn = trvGroupRules.Nodes.Find(g, true);

                // Did we find a match?
                if (tn.Length == 0)
                    // Nope - add it to the combo box
                    cboComputerGroup.Items.Add(g);
            }
        }
                
        private void cboComputerGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Clear parent group textbox and repopulate
            cboParentGroup.Items.Clear();

            foreach (string g in groupnames)
                // Loop through rule collection and see if the current group is already there
                foreach (clsConfig.GroupUpdateRule ur in grouprules)
                    if (ur.computergroup.Name == g)
                    {
                        // Found current computer in existing rules, but are we editing or adding a group?
                        if (cboComputerGroup.Enabled)
                        {
                            // We're adding - add it to the list
                            cboParentGroup.Items.Add(g);
                            break;
                        }
                        else
                        {
                            // We're editing - only add it to the list if the group isn't a child
                            clsConfig.GroupUpdateRule pr = grouprules[cboComputerGroup.Text];
                            if (!grouprules.ChildGroups(pr, true).Contains(g))
                                // Not in list, ok to add
                                cboParentGroup.Items.Add(g);
                        }
                    }

            // Add the Release Day "group"
            cboParentGroup.Items.Insert(0, "Release Day");

            // Validate input
            ValidateInput();
        }

        private bool ValidateInput()
        {
            // Assume the input is valid unless one of the following conditions are met
            bool ok = true;

            if (!groupnames.Contains(cboComputerGroup.Text))
                // Not a valid group name
                ok = false;

            if (txtShortName.Text == "")
                // Short names can't be null
                ok = false;

            if (cboParentGroup.Text != root.Text && !groupnames.Contains(cboParentGroup.Text))
                // Not a valid parent group
                ok = false;

            if (!cboParentInterval.Items.Contains(cboParentInterval.Text))
                // Not a valid interval
                ok = false;


            if (cboParentGroup.Text != root.Text && !cboNoApprovalInterval.Items.Contains(cboNoApprovalInterval.Text))
                // Not a valid interval
                ok = false;

            // Enable or disable the "Add" button based on whether the input was valid or not and return the value for consumption (if required)
            btnAdd.Enabled = ok;
            return ok;
        }

        private void numDisplayOrder_ValueChanged(object sender, EventArgs e)
        {
            ValidateInput();
        }

        private void numSortWeight_ValueChanged(object sender, EventArgs e)
        {
            ValidateInput();
        }

        private void txtShortName_TextChanged(object sender, EventArgs e)
        {
            ValidateInput();
        }
        private void cboParentInterval_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Reset maximum value of numeric box dependant on interval type
            switch (cboParentInterval.Text)
            {
                case "Immediately":
                    numParentInterval.Visible = false;
                    break;

                case "Minutes":
                    numParentInterval.Visible = true;
                    numParentInterval.Maximum = 300; // 5 hours
                    break;

                case "Hours":
                    numParentInterval.Visible = true;
                    numParentInterval.Maximum = 168; // 1 week
                    break;

                case "Days":
                    numParentInterval.Visible = true;
                    numParentInterval.Maximum = 28; // 4 weeks
                    break;

                case "Weeks":
                    numParentInterval.Visible = true;
                    numParentInterval.Maximum = 104; // ~2 years.  If you ain't gonna approve it in 2 years, you ain't gonna approve it.
                    break;
            }

            ValidateInput();
        }

        private void cboNoApprovalInterval_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Reset maximum value of numeric box dependant on interval type
            switch (cboNoApprovalInterval.Text)
            {
                case "Minutes":
                    numNoApprovalInterval.Maximum = 300; // 5 hours
                    break;

                case "Hours":
                    numNoApprovalInterval.Maximum = 168; // 1 week
                    break;

                case "Days":
                    numNoApprovalInterval.Maximum = 28; // 4 weeks
                    break;

                case "Weeks":
                    numNoApprovalInterval.Maximum = 104; // ~2 years.  If you ain't gonna approve it in 2 years, you ain't gonna approve it.
                    break;
            }

            ValidateInput();
        }

        private void cboParentGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            // If root node selected, the second interval isn't relevant - disable it.
            if (cboParentGroup.Text == root.Text)
            {
                lblParentInterval2.Text = "...after update is received by root WSUS server";
                cboParentInterval.DataSource = new List<string>() { "Immediately", "Minutes", "Hours", "Days", "Weeks" };
                numNoApprovalInterval.Visible = false;
                cboNoApprovalInterval.Visible = false;
                lblParentInterval3.Visible = false;
            }
            else
            {
                lblParentInterval2.Text = "...after approval for parent or...";
                cboParentInterval.DataSource = new List<string>() { "Minutes", "Hours", "Days", "Weeks" };
                numNoApprovalInterval.Visible = true;
                cboNoApprovalInterval.Visible = true;
                lblParentInterval3.Visible = true;
            }
            
            ValidateInput();
        }

        private void numParentInterval_ValueChanged(object sender, EventArgs e)
        {
            ValidateInput();
        }

        private void numNoApprovalInterval_ValueChanged(object sender, EventArgs e)
        {
            ValidateInput();
        }

        private TimeSpan ToTimeSpan(int interval, string intervaltype)
        {
            TimeSpan ts;

            switch (intervaltype)
            {
                case "Immediately":
                    ts = new TimeSpan(0, 0, 0);
                    break;

                case "Minutes":
                    ts = new TimeSpan(0, interval, 0);
                    break;

                case "Hours":
                    ts = new TimeSpan(interval, 0, 0);
                    break;

                case "Days":
                    ts = new TimeSpan(interval, 0, 0, 0);
                    break;

                case "Weeks":
                    ts = new TimeSpan(interval * 7, 0, 0, 0);
                    break;

                default:
                    // Failsafe - should never get here.  Default to 1 day.
                    ts = new TimeSpan(1, 0, 0, 0);
                    break;
            }

            return ts;
        }

        private IComputerTargetGroup FindComputerGroup(string groupname)
        {
            // Loop through all computer groups, looking for a matching group
            foreach (IComputerTargetGroup tg in wsus.ComputerGroups)
            {
                if (tg.Name == groupname)
                    // Found group - return it
                    return tg;
            }

            // Could not find groupname - throw exception
            throw new WsusObjectNotFoundException("Computer group " + groupname + " does not exist.");
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Try to convert to a group update rule object
            clsConfig.GroupUpdateRule ur = new clsConfig.GroupUpdateRule(wsus);

            ur.displayorder = (int)numDisplayOrder.Value;
            ur.sortweight = (int)numSortWeight.Value;

            ur.computergroup = FindComputerGroup(cboComputerGroup.Text);
            ur.shortname = txtShortName.Text;

            // Parent group and intervals should be handled differently depending on whether the parent is the root or not
            if (cboParentGroup.Text == root.Text)
            {
                // This is the root node.  Null parent group, only the first interval is relevant
                ur.parentcomputergroup = null;
                ur.updateinterval = ToTimeSpan((int)numParentInterval.Value, cboParentInterval.Text);
            }
            else
            {
                // Parent is another computer group.  Set it and both intervals are relevant
                ur.parentcomputergroup = FindComputerGroup(cboParentGroup.Text);
                ur.updateinterval = ToTimeSpan((int)numParentInterval.Value, cboParentInterval.Text);
                ur.childupdateinterval = ToTimeSpan((int)numNoApprovalInterval.Value, cboNoApprovalInterval.Text);
            }

            // Add or update the group rule
            grouprules.AddEdit(ur);

            // Update the treeview and reset the form
            UpdateGroupUpdateRules();

            numDisplayOrder.Value = grouprules.MaxDisplayOrder + 1;
            numSortWeight.Value = 0;

            cboComputerGroup.Enabled = true;
            cboComputerGroup.Text = "";
            txtShortName.Text = "";
            cboParentGroup.Text = "";

            numParentInterval.Value = 1;
            cboParentInterval.Text = "";

            numNoApprovalInterval.Value = 1;
            cboNoApprovalInterval.Text = "";

            btnAdd.Enabled = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Sort rules before saving
            grouprules.SortByDisplayOrder();

            // Save rules and close form
            cfg.GroupUpdateRules = grouprules;
            this.Close();
        }

        private void trvGroupRules_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (trvGroupRules.SelectedNode == null || trvGroupRules.SelectedNode == root)
            {
                // Either the root item or no item is selected, therefore can't remove or edit anything
                btnEdit.Enabled = false;
                btnRemove.Enabled = false;
            }
            else
            {
                // Editing is available
                btnEdit.Enabled = true;

                // Are there any children of this node?
                clsConfig.GroupUpdateRule o = (clsConfig.GroupUpdateRule)trvGroupRules.SelectedNode.Tag;

                if (grouprules.ChildGroups(o).Count > 0)
                    // Yes, you can't delete this rule
                    btnRemove.Enabled = false;
                else
                    // No, you can delete the rule
                    btnRemove.Enabled = true;
            }
        }
        private void TimeSpanToForm(TimeSpan ts, NumericUpDown num, ComboBox cbo)
        {
            if (ts == new TimeSpan(0, 0, 0))
            {
                // Immediate
                cbo.Text = "Immediately";
                return;
            }

            if (ts.Minutes > 0)
            {
                cbo.Text = "Minutes";
                num.Value = (int)ts.TotalMinutes;
                return;
            }

            if (ts.Hours > 0)
            {
                cbo.Text = "Hours";
                num.Value = (int)ts.TotalHours;
                return;
            }

            if (ts.Days > 0)
            {
                // If the number of days is divisible by 7, we're talking weeks
                if (ts.Days % 7 == 0)
                {
                    cbo.Text = "Weeks";
                    num.Value = (int)ts.TotalDays / 7;
                    return;
                }
                else
                {
                    cbo.Text = "Days";
                    num.Value = (int)ts.TotalDays;
                    return;
                }
            }

            // Something is very wrong - we shouldn't have gotten here!
            throw new ArgumentOutOfRangeException("Timespan could not be parsed");
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            // Get current group rule
            clsConfig.GroupUpdateRule ur = (clsConfig.GroupUpdateRule)trvGroupRules.SelectedNode.Tag;

            // Copy the information to the form
            numDisplayOrder.Value = ur.displayorder;
            numSortWeight.Value = (decimal)ur.sortweight;

            cboComputerGroup.Text = ur.computergroup.Name;
            txtShortName.Text = ur.shortname;

            // If we're dealing with a null parent, we only need set the parent computer group.  If not, we've got more information to load...
            if (ur.parentcomputergroup == null)
                cboParentGroup.Text = root.Text;
            else
            {
                cboParentGroup.Text = ur.parentcomputergroup.Name;
                TimeSpanToForm(ur.childupdateinterval, numNoApprovalInterval, cboNoApprovalInterval);
            }

            TimeSpanToForm(ur.updateinterval, numParentInterval, cboParentInterval);

            cboComputerGroup.Enabled = false;
            btnEdit.Enabled = false;
            btnRemove.Enabled = false;
        }

        private void trvGroupRules_DoubleClick(object sender, EventArgs e)
        {
            btnEdit.PerformClick();

            // Ensure the tree node is open when it's been double clicked
            trvGroupRules.SelectedNode.Expand();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            // Get current group rule
            clsConfig.GroupUpdateRule ur = (clsConfig.GroupUpdateRule)trvGroupRules.SelectedNode.Tag;

            grouprules.Remove(ur.computergroup.Name);
        }

        private void cboComputerGroup_EnabledChanged(object sender, EventArgs e)
        {
            // Label the Add/Update button correctly
            if (cboComputerGroup.Enabled)
                // Adding a new item
                btnAdd.Text = "&Add";
            else
                btnAdd.Text = "&Update";
        }

        private void frmGroupUpdateRules_Resize(object sender, EventArgs e)
        {
            // Enforce the minimum height of the control
            if (this.Height < minheight) this.Height = minheight;
        }
    }
}
