using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using Microsoft.UpdateServices.Administration;
using Microsoft.UpdateServices.Administration.Internal;

namespace WSUSAdminAssistant
{
    public partial class frmMain : Form
    {
        // Initialise configuration data
        private clsConfig cfg = new clsConfig();
        private clsWSUS wsus;

        private DateTime lastupdate = Convert.ToDateTime("1970-01-01 00:00:00");
        private DateTime lastupdaterun = Convert.ToDateTime("1970-01-01 00:00:00");

        // Various lists associated with computer groups in the Unapproved Updates grid
        private List<DataGridViewColumn> GroupColumns = new List<DataGridViewColumn>();
        private List<ToolStripMenuItem> HideGroupMenuItems = new List<ToolStripMenuItem>();
        private clsConfig.GroupUpdateRuleCollection ShowGroups = new clsConfig.GroupUpdateRuleCollection();

        // Flags to let update procedures know if we've forced an update, or cancelled an operation
        private bool forceUpdate = true;
        private bool cancelNow = false;

        // Background workers
        BackgroundWorker wrkPinger = new BackgroundWorker();
        BackgroundWorker wrkSUSID = new BackgroundWorker();

        // User initiated task collection
        TaskCollection tasks;

        public frmMain()
        {
            InitializeComponent();

            // Initialise variables
            wsus = cfg.wsus;

            // Build collection of groups to show
            RebuildShowGroupsCollection();
        }

        // Provide an easy and consistent way of outputting debugging information
        private void DebugOutput(string output, params object[] args)
        {
            Debug.WriteLine(DateTime.Now.ToString("h:mm:ss.ff ") + String.Format(output, args));
        }

        private void DebugOutput(string output)
        {
            Debug.WriteLine(DateTime.Now.ToString("h:mm:ss.ff ") + output);
        }

        private void timUpdateData_Tick(object sender, EventArgs e)
        {
            // Disable timer until it's been processed fully
            timUpdateData.Enabled = false;

            // Determine which tab is selected and call it's update procedure
            if (tabAdminType.SelectedTab.Name == tabUnapproved.Name) UpdateUnapproved();
            if (tabAdminType.SelectedTab.Name == tabEndpointFaults.Name) UpdateEndpointFaults();
            if (tabAdminType.SelectedTab.Name == tabSuperceded.Name) UpdateSupercededUpdates();
            if (tabAdminType.SelectedTab.Name == tabWSUSNotCommunicating.Name) UpdateWSUSNotCommunicating();
            if (tabAdminType.SelectedTab.Name == tabServerRestarts.Name) UpdateServerReboots();
            
            // On return, ensure the "working" dialog is not showing and re-enable the timer
            timUpdateData.Enabled = true;
            gbxWorking.Visible = false;
        }

        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, Int32 wMsg, bool wParam, Int32 lParam);
        private const int WM_SETREDRAW = 11;

        private static void SuspendDrawing(Control parent)
        {
            parent.SuspendLayout();
            SendMessage(parent.Handle, WM_SETREDRAW, false, 0);
        }

        private static void ResumeDrawing(Control parent)
        {
            parent.ResumeLayout();
            SendMessage(parent.Handle, WM_SETREDRAW, true, 0);

            // Force redraw of control now
            parent.Refresh();
        }

        private void UpdateEndpointFaults()
        {
            // Update list when forced or after 2 minutes
            if (forceUpdate || DateTime.Now.Subtract(lastupdaterun).TotalSeconds > 120)
            {
                // Reset forced update flag and update timestamp
                forceUpdate = false;
                lastupdaterun = DateTime.Now;

                if (CheckDBConnection())
                {
                    // Show the update window
                    gbxWorking.Visible = true;
                    this.Refresh();

                    // Tag all rows as not having been updated...
                    foreach (DataGridViewRow r in grdEndpoints.Rows)
                        r.Cells[epUpdate.Index].Value = "N";

                    // Check which update types should be displayed and update those that need it.
                    if (butApproved.Checked) EndpointUpdateApproved();
                    if (butUpdateErrors.Checked) EndpointUpdateErrors();
                    if (butNotCommunicating.Checked) EndpointUpdateNotCommunicating();
                    if (butUnassigned.Checked) EndpointUpdateUngroupedComputers();
                    if (butDefaultSusID.Checked) EndpointUpdateDefaultSusID();
                    if (butGroupRules.Checked) EndpointIncorrectGroupUpdate();
                    if (butDuplicatePCs.Checked) EndpointUpdateDuplicateIPs();

                    // Remove any row that hasn't been updated
                    bool removed = true;
                    
                    while (removed)
                    {
                        removed = false;

                        foreach (DataGridViewRow r in grdEndpoints.Rows)
                        {
                            if (r.Cells[epUpdate.Index].Value.ToString() == "N")
                            {
                                grdEndpoints.Rows.Remove(r);
                                removed = true;
                            }
                        }
                    }
                }
            }

            // Sort the datagrid
            grdEndpoints.Sort(grdEndpoints.Columns[epSortOrder.Index], ListSortDirection.Ascending);

            // Alternate the row's background colour to make viewing easier - only inverting when a new PC is found
            string prevrow = "zzzzz";
            bool reverse = false;

            foreach (DataGridViewRow r in grdEndpoints.Rows)
            {
                if (r.Cells[epIP.Index].Value.ToString() != prevrow)
                {
                    reverse = !reverse;
                    prevrow = r.Cells[epIP.Index].Value.ToString();
                }

                if (reverse)
                    r.DefaultCellStyle.BackColor = Color.Empty;
                else
                    r.DefaultCellStyle.BackColor = Color.LightGray;
            }

            // Since this timer is active, ticks should occur every half a second...
            forceUpdate = false;
            timUpdateData.Interval = 500;
        }

        private void PingWorker(object sender, DoWorkEventArgs e)
        {
            // Overall loop that sleeps for 10 seconds while a form isn't active, or while all machines have been pinged in the last 30 seconds
            do
            {
                // Internal loop that keeps pinging machines whilst there are machines there to ping
                int rn;

                do
                {
                    // Find least recently pinged machine
                    rn = -1;
                    long lp = DateTime.Now.Ticks;

                    foreach (DataGridViewRow r in grdEndpoints.Rows)
                    {
                        // Has this machine ever been pinged?
                        if (r.Cells[epPingUpdated.Index].Value == null || r.Cells[epPingUpdated.Index].Value.ToString() == "")
                        {
                            // Nope, let's do this one...
                            rn = r.Index;
                            break;
                        }

                        // Yes.  Has it been pinged more than 30 seconds ago and longer ago than the last one found?
                        if (long.Parse(r.Cells[epPingUpdated.Index].Value.ToString()) < lp && (DateTime.Now.Ticks - long.Parse(r.Cells[epPingUpdated.Index].Value.ToString())) > (30 * TimeSpan.TicksPerSecond))
                        {
                            // Yep, make a note of it and keep going...
                            lp = long.Parse(r.Cells[epPingUpdated.Index].Value.ToString());
                            rn = r.Index;
                        }
                    }

                    // If a row was found, ping it.
                    if (rn != -1)
                        DoPing(grdEndpoints.Rows[rn]);

                    // Only continue immediately pinging if we found a row
                }
                while (rn != -1);

                // Snooze for 10 seconds before starting again.
                Thread.Sleep(10000);
            }
            while (true);

            // This thread should only exit when it is killed by the form being closed.
        }

        private void DoPing(DataGridViewRow r)
        {
            try
            {
                Ping p = new System.Net.NetworkInformation.Ping();
                PingReply pr;
                string pingstatus;

                pr = p.Send(r.Cells[epIP.Index].Value.ToString(), 1000);

                if (pr.Status == IPStatus.Success)
                {
                    pingstatus = pr.RoundtripTime.ToString() + "ms";
                }
                else
                {
                    pingstatus = "No Response";
                }

                // Update all grid rows with this IP address
                foreach (DataGridViewRow gr in grdEndpoints.Rows)
                {
                    if (gr.Cells[epIP.Index].Value.ToString() == r.Cells[epIP.Index].Value.ToString())
                    {
                        gr.Cells[epPing.Index].Value = pingstatus;
                        gr.Cells[epPingUpdated.Index].Value = DateTime.Now.Ticks;
                    }
                }
            }
            catch (Exception e)
            {
                r.Cells[epPing.Index].Value = "Error";
                r.Cells[epPing.Index].ToolTipText = e.Message;
            }

            // Note time was last updated
            r.Cells[epPingUpdated.Index].Value = DateTime.Now.Ticks;
        }

        private bool CheckDBConnection()
        {
            // Placeholder function to ensure that if the DB connection fails, tabs are disabled and the user is returned to the home tab to see what the
            // error is
            if (wsus.CheckDBConnection())
                return true;
            else
            {
                // Problem - reset tabs and return result
                tabHome.Select();
                ShowTabs(false);

                return false;
            }
        }

        private clsWSUS.UnapprovedUpdates CurrentUnapprovedUpdates = null;

        private void UpdateUnapproved()
        {
            if (CheckDBConnection())
            {
                // Unapproved updates tab is selected - check when the last update was modified.
                DateTime clu = wsus.GetLastUpdated(lastupdate);

                // Update if the last update time doesn't agree the one previously recorded, if it's been more than 5 minutes since the last update or if we've forced an update
                if (clu != lastupdate && Math.Abs(clu.Subtract(lastupdaterun).Seconds) < 10 || DateTime.Now.Subtract(lastupdaterun).Minutes > 5 || forceUpdate)
                {
                    // Show the update window
                    gbxWorking.Visible = true;
                    this.Refresh();

                    // Hide the Hide Groups menu (if it's open)
                    mnuHideGroups.HideDropDown();

                    // Get unapproved updates currently pending
                    CurrentUnapprovedUpdates = new clsWSUS.UnapprovedUpdates(cfg, ShowGroups);
                    CurrentUnapprovedUpdates.UpdateUnapprovedUpdates();

                    // Note the time of the last update
                    lastupdate = clu;

                    // Reset forced update flag
                    forceUpdate = false;

                    // Rederaw datagrid.
                    FillUpdateDataGrid();
                }
            }
        }

        private void FillUpdateDataGrid()
        {
            // Return immediately if we don't have a valid list of updates or if it's currently updating
            if (CurrentUnapprovedUpdates == null || CurrentUnapprovedUpdates.IsUpdating()) return;

            // Don't redraw the datagrid until we've finished updating it
            SuspendDrawing(grdUnapproved);

            // Have we ever created columns, or have rule collections changed since we last updated?
            bool groupschanged = false;

            if (GroupColumns.Count != CurrentUnapprovedUpdates.Groups.Count)
                // No group columns have been added, or the number of columns added differs to the number of groups
                groupschanged = true;
            else
            {
                // Loop through all groups and ensure that the columns match
                for (int i = 0; i < CurrentUnapprovedUpdates.Groups.Count; i++)
                {
                    if (!cfg.HideGroups.Contains(CurrentUnapprovedUpdates.Groups[i].computergroupid))
                    {
                        // Only consider the group if it's not hidden

                        if (CurrentUnapprovedUpdates.Groups[i].shortname != grdUnapproved.Columns[uaSortOrder.Index + i + 1].HeaderText)
                        {
                            // Header does not match
                            groupschanged = true;
                            break;
                        }
                    }
                }
            }

            if (groupschanged)
            {
                // Remove old columns, but only if they've already been added
                GroupColumns.Clear();
                HideGroupMenuItems.Clear();

                if (grdUnapproved.Columns.Count > uaSortOrder.Index)
                {
                    for (int i = uaSortOrder.Index + 1; i < grdUnapproved.Columns.Count; )
                        // Remove the column
                        grdUnapproved.Columns.RemoveAt(i);
                }

                // Add new columns
                foreach (clsConfig.GroupUpdateRule ur in CurrentUnapprovedUpdates.Groups)
                {
                    // Add column if it's not excluded
                    if (!cfg.HideGroups.Contains(ur.computergroupid))
                    {
                        DataGridViewColumn c = new DataGridViewTextBoxColumn();
                        c.Name = "uag" + ur.shortname.Replace(' ', '_');
                        c.HeaderText = ur.shortname;

                        // Format the column appropriately
                        c.Width = 55;
                        c.Resizable = DataGridViewTriState.False;
                        c.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                        // Store the computer group object in the header - handy for when approving or unapproving updates
                        c.Tag = ur.computergroup;

                        GroupColumns.Add(c);
                    }
                }

                foreach (clsConfig.GroupUpdateRule ur in cfg.GroupUpdateRules)
                {
                    // Construct menu items
                    ToolStripMenuItem m = new ToolStripMenuItem();
                    m.Text = ur.computergroup.Name;
                    m.Tag = ur.computergroupid;
                    m.DisplayStyle = ToolStripItemDisplayStyle.Text;

                    // Item should be ticked (i.e. hidden) if it's in the list of computer groups
                    m.Checked = cfg.HideGroups.Contains(ur.computergroupid);

                    // Add event handler for when the menu item is clicked
                    m.Click += mnuHideGroupItem_Click;

                    HideGroupMenuItems.Add(m);
                }

                // Add columns to grdUnapproved
                grdUnapproved.Columns.AddRange(GroupColumns.ToArray());

                // Add menu items
                mnuHideGroups.DropDownItems.Clear();
                mnuHideGroups.DropDownItems.AddRange(HideGroupMenuItems.ToArray());
            }

            // Loop through all rows and mark them as not updated
            foreach (DataGridViewRow r in grdUnapproved.Rows)
                r.Tag = "N";

            DateTime lastbreath = DateTime.Now;

            // Loop through each unapproved update and update the datagrid accordingly
            foreach (clsWSUS.UnapprovedUpdate uu in CurrentUnapprovedUpdates)
            {
                // Have we taken a breath recently to allow other things to happen?
                if (DateTime.Now.Subtract(lastbreath).TotalMilliseconds > 100)
                {
                    // Nope - take a breather.
                    Application.DoEvents();
                    lastbreath = DateTime.Now;
                }

                // Does this update meet the filter requirements set?
                bool filtered = true;

                if (txtFilterName.Text != "" && uu.Title.IndexOf(txtFilterName.Text, StringComparison.OrdinalIgnoreCase) > -1) filtered = false;
                if (txtFilterDescription.Text != "" && uu.Description.IndexOf(txtFilterDescription.Text, StringComparison.OrdinalIgnoreCase) > -1) filtered = false;
                if (txtFilterArticle.Text != "" && uu.KBArticle.IndexOf(txtFilterArticle.Text, StringComparison.OrdinalIgnoreCase) > -1) filtered = false;

                // If no filters are set, everything should be displayed
                if (txtFilterName.Text == "" && txtFilterDescription.Text == "" && txtFilterArticle.Text == "") filtered = false;

                // Is this update approvable for any PC in any group?  Does it meet the filter requirements?
                if (uu.PCsRequiringUpdate > 0 && !filtered)
                {
                    // Try to find an existing row, adding one if it's not found
                    DataGridViewRow r = FindUnapprovedUpdateRow(uu.UpdateID);

                    // Update the row
                    r.Cells[uaID.Index].Value = uu.UpdateID;
                    r.Cells[uaUpdateName.Index].Value = uu.Title;
                    r.Cells[uaDescription.Index].Value = uu.Description;
                    r.Cells[uaKB.Index].Value = uu.KBArticle;
                    r.Cells[uaSortOrder.Index].Value = uu.SortIndex;

                    // Tag the row as updated
                    r.Tag = "Y";

                    // Loop through each group, adding details
                    foreach (clsWSUS.PerGroupInformation gi in uu.Groups)
                    {
                        // Try to locate a cell
                        DataGridViewCell c = null;

                        // Loop through each cell, looking for the right column
                        for (int i = uaSortOrder.Index + 1; i < grdUnapproved.Columns.Count; i++)
                        {
                            if (grdUnapproved.Columns[i].HeaderText == gi.GroupRule.shortname)
                            {
                                // Got it - note it and break
                                c = r.Cells[i];
                                break;
                            }
                        }

                        if (c == null)
                        {
                            // Couldn't find cell - print some debugging information
                            DebugOutput("Couldn't find DataGridViewCell for group {0} ({1})", gi.GroupRule.shortname, "uag" + gi.GroupRule.shortname.Replace(' ', '_'));
                            break;
                        }

                        // Has the update been approved?
                        if (gi.Approved.HasValue)
                        {
                            // Yes - cell value should be "Approved".  Tooltip should show when update was approved.
                            c.Value = "Approved";
                            c.ToolTipText = string.Format("Approved {0}", gi.Approved.Value.ToLocalTime().ToString("ddd dMMMyy h:mm:sstt"));
                        }
                        else
                        {
                            // Is the update approvable?
                            if (gi.UpdateApprovableNow)
                            {
                                // Yes, show the number of PCs requiring the update.  No tooltip required.
                                c.Value = gi.PCs.ToString();
                                c.ToolTipText = "";
                            }
                            else
                            {
                                // Will the update be approvable in the future?
                                if (gi.Approvable.HasValue)
                                {
                                    // Yes.  Cell can be blank, tooltip should indicate when the update will be installable.
                                    c.Value = "Waiting";
                                    c.ToolTipText = string.Format("Update will be approvable on {0}", gi.Approvable.Value.ToLocalTime().ToString("ddd dMMMyy h:mm:sstt"));
                                }
                            }
                        }
                    }
                }
            }

            // Loop through all rows, removing any that hasn't been updated
            for (int i = 0; i < grdUnapproved.Rows.Count; )
            {
                // Has this row been updated?
                if (grdUnapproved.Rows[i].Tag.ToString() == "N")
                    // No - delete it.
                    grdUnapproved.Rows.RemoveAt(i);
                else
                    // Yes, look at the next row
                    i++;
            }

            // Sort datagrid
            uaSortOrder.SortMode = DataGridViewColumnSortMode.Automatic;
            grdUnapproved.Sort(uaSortOrder, ListSortDirection.Ascending);

            // Ensure UpdateName column isn't too wide (a maximum of a quarter of the window's width)
            grdUnapproved.Columns[uaUpdateName.Index].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Application.DoEvents();

            if (grdUnapproved.Columns[uaUpdateName.Index].Width > (this.Width / 4))
            {
                grdUnapproved.Columns[uaUpdateName.Index].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                grdUnapproved.Columns[uaUpdateName.Index].Width = this.Width / 4;
            }

            // Show total number of updates
            lblUpdatesToApprove.Text = grdUnapproved.Rows.Count.ToString() + " update(s)";

            // Since this timer is active, ticks should occur every 15 seconds...
            timUpdateData.Interval = 15000;

            // Re-enabling drawing of datagrid and note time of datagrid update
            ResumeDrawing(grdUnapproved);
            lastupdaterun = DateTime.Now;
        }

        void mnuHideGroupItem_Click(object sender, EventArgs e)
        {
            // Get the item that was clicked
            ToolStripMenuItem i = (ToolStripMenuItem)sender;

            // Reverse it's check
            i.Checked = !i.Checked;

            // Rebuild array of excluded groups
            List<string> eg = new List<string>();

            foreach (ToolStripMenuItem mi in HideGroupMenuItems)
                if (mi.Checked)
                    // If item is checked, it should be included in the list.  Item's tag will contain the GUID of the group
                    eg.Add(mi.Tag.ToString());

            cfg.HideGroups = eg;

            // Rebuild collection of groups to show
            RebuildShowGroupsCollection();

            // Trip timer in 1 second, ensure list will update and re-show the Hide Groups menu (makes selecting/deselecting multiple groups easier)
            timUpdateData.Interval = 1000;
            forceUpdate = true;

            mnuHideGroups.ShowDropDown();
        }

        private void RebuildShowGroupsCollection()
        {
            ShowGroups.Clear();

            foreach (clsConfig.GroupUpdateRule ur in cfg.GroupUpdateRules)
                if (!cfg.HideGroups.Contains(ur.computergroupid))
                    // Group not in hide list - add it
                    ShowGroups.Add(ur);
        }
        private DataGridViewRow FindUnapprovedUpdateRow(string updateid)
        {
            // Loop through each row in the datagrid, looking for an appropriate row
            foreach (DataGridViewRow r in grdUnapproved.Rows)
                if (r.Cells[uaID.Index].Value != null && r.Cells[uaID.Index].Value.ToString() == updateid)
                    // Found one - return it
                    return r;

            // Didn't find a row - return a newly added row
            return grdUnapproved.Rows[grdUnapproved.Rows.Add()];
        }

        private void EndpointUpdateUngroupedComputers()
        {
            if (CheckDBConnection())
            {
                // Update the data grid
                DataTable d = wsus.GetUnassignedComputers();

                foreach (DataRow dr in d.Rows)
                {
                    epRowData r = new epRowData("Not Assigned to a Group");

                    r.EndpointName = dr["name"];
                    r.ipAddress = dr["ipaddress"];
                    r.SetUpstreamServerByGuid(dr["parentserverid"], cfg.wsus.server);

                    AddUpdateEndpointGrid(r);
                }
            }
        }

        private void EndpointUpdateNotCommunicating()
        {
            // Get list of computers that hasn't updated in the last week
            ComputerTargetScope cs = new ComputerTargetScope();
            cs.ToLastSyncTime = DateTime.Now.AddDays(-7);
            cs.IncludeDownstreamComputerTargets = true;

            // Update grid
            foreach (IComputerTarget c in wsus.server.GetComputerTargets(cs))
            {
                epRowData r = new epRowData("Not Communicating", c);

                AddUpdateEndpointGrid(r);
            }
        }

        private void EndpointUpdateDuplicateIPs()
        {
            //  Get a complete list of computers
            ComputerTargetScope cs = new ComputerTargetScope();
            cs.IncludeDownstreamComputerTargets = true;

            ComputerTargetCollection cc = wsus.server.GetComputerTargets(cs);

            // Assemble a dictionary for comparison
            Dictionary<String, String> ca = new Dictionary<String, String>();

            foreach (IComputerTarget c in cc)
            {
                ca.Add(c.Id.ToString(), c.IPAddress.ToString());
            }
 
            // Loop through all computers
            foreach (IComputerTarget c in cc)
            {
                string cn = c.Id.ToString();
                string ip = c.IPAddress.ToString();

                // See if we can find one with a duplicate IP address
                foreach (KeyValuePair<string,string> e in ca)
                {
                    // Do we have a different computer to the current one and does it have a duplicate IP address
                    if (cn != e.Key && ip == e.Value)
                    {
                        // Yep, it's a duplicate.
                        epRowData r = new epRowData("Duplicate IP address", c);

                        // Add it and break
                        AddUpdateEndpointGrid(r);
                        break;
                    }
                }
            }
        }

        private void UpdateWSUSNotCommunicating()
        {
            // Reset forced update flag
            forceUpdate = false;

            // Show working box
            gbxWorking.Visible = true;
            this.Refresh();

            // Clear grid and populate with downstream server information
            grdWSUSNotCommunicting.Rows.Clear();

            foreach (IDownstreamServer s in wsus.server.GetDownstreamServers())
            {
                if (s.LastSyncTime < DateTime.Now.ToUniversalTime().AddHours(-24))
                {
                    int rn = grdWSUSNotCommunicting.Rows.Add();
                    DataGridViewRow r = grdWSUSNotCommunicting.Rows[rn];

                    r.Cells[wnuServerName.Index].Value = s.FullDomainName;
                    r.Cells[wnuLastSync.Index].Value = s.LastSyncTime.ToString("dd-MMM-yyyy h:mmtt");
                    r.Cells[wnuLastRollup.Index].Value = s.LastRollupTime.ToString("dd-MMM-yyyy h:mmtt");
                }
            }

            // Update time for this tab is 60 seconds
            timUpdateData.Interval = 60 * 60 * 1000;
        }

        private void AlternateRowColour(DataGridView g)
        {
            // Alternate the row's background colour to make viewing easier
            foreach (DataGridViewRow r in g.Rows)
            {
                if (r.Index % 2 == 0)
                    r.DefaultCellStyle.BackColor = Color.Empty;
                else
                    r.DefaultCellStyle.BackColor = Color.LightGray;
            }
        }

        private class epRowData
        {
            public epRowData(string fault)
            {
                Fault = fault;
            }

            public epRowData(string fault, IComputerTarget c)
            {
                Fault = fault;
                EndpointName = c.FullDomainName;
                ipAddress = c.IPAddress;
                LastContact = c.LastReportedStatusTime;
                LastStatus = c.LastSyncResult;
                UpstreamServer = c;
            }

            private string _ipAddress = "";

            public object ipAddress
            {
                get { return _ipAddress; }
                set { _ipAddress = value.ToString(); }
            }

            private string _LastStatus;

            public object LastStatus
            {
                get { return _LastStatus; }
                set { _LastStatus = value.ToString(); }
            }

            public string ComputerGroup = "";
            public DateTime LastContact;
            public string ExtraInfo;
            public string Fault;
            public int? ApprovedUpdates = null;
            public int? ErrorUpdates = null;

            private string _EndpointName = "";

            public object EndpointName
            {
                get { return _EndpointName; }
                set { _EndpointName = value.ToString(); }
            }

            private string _UpstreamServer;

            public object UpstreamServer
            {
                get { return _UpstreamServer; }
                set
                {
                    // If we pass a null value, the upstream server is assumed to be the local one.
                    if (value == null)
                        _UpstreamServer = "Local";

                    // If we pass a string, we assume that's the upstream server name.
                    if (value is string)
                        _UpstreamServer = value.ToString();

                    // If we pass a computer target object, figure out whether it's local or remote and act accordingly
                    if (value is IComputerTarget)
                    {
                        IComputerTarget c = (IComputerTarget)value;

                        if (c.SyncsFromDownstreamServer)
                            _UpstreamServer = c.GetParentServer().FullDomainName;
                        else
                            _UpstreamServer = "Local";
                    }
                }
            }

            public void SetUpstreamServerByGuid(object g, IUpdateServer wsus)
            {
                // If the object is a database null, it's the local server
                if (g is DBNull)
                    _UpstreamServer = "Local";
                else
                {
                    try
                    {
                        _UpstreamServer = wsus.GetDownstreamServer((Guid) g).FullDomainName;
                    }
                    catch (Exception e)
                    {
                        _UpstreamServer = e.Message + ", GUID: " + g.ToString();
                    }
                }
            }
        }

        private void AddUpdateEndpointGrid(epRowData row)
        {
            // Locate an existing matching row
            int rn = -1;

            foreach (DataGridViewRow dgr in grdEndpoints.Rows)
            {
                if (dgr.Cells[epName.Index].Value.ToString() == row.EndpointName.ToString() && dgr.Cells[epFault.Index].Value.ToString() == row.Fault)
                {
                    rn = dgr.Index;
                    break;
                }

            }

            // If no row is located, create a new one
            if (rn == -1) rn = grdEndpoints.Rows.Add();

            DataGridViewRow r = grdEndpoints.Rows[rn];

            // Fill in data grid
            r.Cells[epName.Index].Value = row.EndpointName;
            r.Cells[epIP.Index].Value = row.ipAddress;
            r.Cells[epLastStatus.Index].Value = row.LastStatus;
            r.Cells[epComputerGroup.Index].Value = row.ComputerGroup;
            r.Cells[epFault.Index].Value = row.Fault;
            r.Cells[epDownstreamServer.Index].Value = row.UpstreamServer;
            r.Cells[epExtraInfo.Index].Value = row.ExtraInfo;

            // Cleanly handle items with potentially no value
            if (row.LastContact > DateTime.MinValue) r.Cells[epLastContact.Index].Value = row.LastContact;
            if (row.ApprovedUpdates.HasValue) r.Cells[epApprovedUpdates.Index].Value = row.ApprovedUpdates;
            if (row.ErrorUpdates.HasValue) r.Cells[epUpdateErrors.Index].Value = row.ErrorUpdates;

            // Create sort index
            r.Cells[epSortOrder.Index].Value = row.ipAddress.ToString() + row.EndpointName.ToString();

            // Tag the row as updated
            r.Cells[epUpdate.Index].Value = "Y";
        }

        private void EndpointUpdateDefaultSusID()
        {
            // Loop through each default SUS ID and see if we can find a matching computer
            foreach (string susid in cfg.DefaultSusIDCollection)
            {
                try
                {
                    IComputerTarget c = wsus.server.GetComputerTarget(susid);

                    AddUpdateEndpointGrid(new epRowData("Default SUS ID", c));
                }
                catch (WsusObjectNotFoundException)
                {
                    // No PC found, no action required
                }
            }
        }

        private void EndpointIncorrectGroupUpdate()
        {
            // Get the group rules and sort by priority
            clsConfig.ComputerGroupRegexCollection rules = cfg.ComputerRegExList;
            rules.SortByPriority();

            if (CheckDBConnection())
            {
                // Update the data grid;
                DataTable t = wsus.GetComputerGroups();

                foreach (DataRow d in t.Rows)
                {
                    // Attempt to match the computer to a rule
                    clsConfig.ComputerGroupRegEx rx = rules.MatchPC(d["name"].ToString(), d["ipaddress"].ToString());

                    // Do we have a match?
                    if (rx != null)
                    {
                        // Yes - does it match the computer group we have?
                        if (d["groupname"].ToString() != rx.ComputerGroup)
                        {
                            // No - is it in a group we're supposed to be ignoring?
                            if (!cfg.IgnoreComputerGroupCollection.Values.ToArray().Contains(d["groupname"].ToString()))
                            {
                                // No - add it.
                                epRowData r = new epRowData("Incorrect Computer Group");

                                r.EndpointName = d["name"];
                                r.ipAddress = d["ipaddress"];
                                r.ComputerGroup = rx.ComputerGroup;
                                r.ExtraInfo = "Currently in " + d["groupname"].ToString();
                                r.SetUpstreamServerByGuid(d["parentserverid"], cfg.wsus.server);

                                AddUpdateEndpointGrid(r);
                            }
                        }
                    }
                }
            }
        }

        private void EndpointUpdateApproved()
        {
            if (CheckDBConnection())
            {
                // Retreive the list of approved updates that have not yet been applied.
                DataTable t = wsus.GetApprovedUpdates();

                foreach (DataRow d in t.Rows)
                {
                    epRowData r = new epRowData("Uninstalled Approved Updates");

                    // Fill in data grid
                    r.EndpointName = d["fulldomainname"];
                    r.ipAddress = d["ipaddress"];
                    r.ApprovedUpdates = int.Parse(d["approvedupdates"].ToString());
                    r.LastContact = DateTime.Parse(d["lastsynctime"].ToString());
                    r.SetUpstreamServerByGuid(d["parentserverid"], cfg.wsus.server);

                    AddUpdateEndpointGrid(r);
                }
            }
        }

        private void EndpointUpdateErrors()
        {
            if (CheckDBConnection())
            {
                // Update the data grid;
                DataTable t = wsus.GetUpdateErrors();

                foreach (DataRow d in t.Rows)
                {
                    // Fill in data grid
                    epRowData r = new epRowData("Updates With Errors");

                    r.EndpointName = d["fulldomainname"];
                    r.ipAddress = d["ipaddress"];
                    r.ErrorUpdates = int.Parse(d["updateerrors"].ToString());
                    r.LastContact = DateTime.Parse(d["lastsynctime"].ToString());
                    r.SetUpstreamServerByGuid(d["parentserverid"], cfg.wsus.server);

                    AddUpdateEndpointGrid(r);
                }
            }
        }

        private void UpdateSupercededUpdates()
        {
            // Show "refreshing" notification
            gbxWorking.Visible = true;
            this.Refresh();

            // Get list of updates and update IDs for superceded updates
            DataTable t = wsus.GetSupercededUpdates();

            // Empty the grid and refill it.
            grdSupercededUpdates.Rows.Clear();

            foreach (DataRow d in t.Rows)
            {
                DataGridViewRow r = grdSupercededUpdates.Rows[grdSupercededUpdates.Rows.Add()];

                r.Cells[suUpdateName.Index].Value = d["defaulttitle"].ToString();
                r.Cells[suUpdateID.Index].Value = d["updateid"].ToString();

                DataGridViewCheckBoxCell c = (DataGridViewCheckBoxCell)r.Cells[suSelect.Index];
                c.Value = false;
            }

            // Show number of updates
            lblUpdateCount.Text = grdSupercededUpdates.Rows.Count.ToString() + " update(s)";

            // Reset timer to trigger again only after 15 minutes.  Other activity will trigger updates.
            timUpdateData.Interval = 15 * 60 * 1000;
        }

        private void UpdateServerReboots()
        {
            if (DateTime.Now.Subtract(lastupdaterun).Minutes > 5 || forceUpdate)
            {
                // Show updating message
                gbxWorking.Visible = true;
                this.Refresh();

                // Reset last run time
                lastupdaterun = DateTime.Now;

                // Try to get a list of computers
                ComputerTargetScope cs = new ComputerTargetScope();
                cs.IncludedInstallationStates = UpdateInstallationStates.InstalledPendingReboot;
                cs.IncludeDownstreamComputerTargets = true;
                ComputerTargetCollection comp = wsus.server.GetComputerTargets(cs);

                // Clear the list of servers and update
                lstServers.Items.Clear();

                foreach (IComputerTarget c in comp)
                {
                    // Is this a Windows Server?
                    if (c.OSDescription.ToUpper().Contains("SERVER"))
                    {
                        // Yep - include it
                        lstServers.Items.Add(c.FullDomainName);
                    }
                }

                // Reset update interval to 5 minutes
                timUpdateData.Interval = 5 * 60 * 1000;
            }
        }

        private void ShowTabs(bool show)
        {
            foreach (TabPage t in tabAdminType.TabPages)
            {
                if (t.Name != tabHome.Name) ((Control)t).Enabled = show;
            }

            timUpdateData.Enabled = show;
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            // Let the user know something is happening
            Cursor.Current = Cursors.WaitCursor;

            // Populate stats list
            lvwStatus.Items.Add(new ListViewItem("SQL Server Connection:"));
            lvwStatus.Items.Add(new ListViewItem("WSUS Server Connection:"));

            // Link status items to WSUS class
            lvwStatus.Items[0].SubItems.Add(wsus.dbStatus);
            lvwStatus.Items[1].SubItems.Add(wsus.wsusStatus);


            lvwStatus.Columns[0].Width = -1;
            lvwStatus.Columns[1].Width = -1;

            if (wsus.server != null)
                ShowTabs(true);
            else
                ShowTabs(false);

            // Return window to it's saved location and state
            this.Location = cfg.WindowLocation;
            this.Size = cfg.WindowSize;

            if (cfg.WindowState == FormWindowState.Maximized.ToString()) this.WindowState = FormWindowState.Maximized;
            if (cfg.WindowState == FormWindowState.Normal.ToString()) this.WindowState = FormWindowState.Normal;
            if (cfg.WindowState == FormWindowState.Minimized.ToString()) this.WindowState = FormWindowState.Minimized;

            // Turn on timer if both SQL and WSUS are connected
            if (lvwStatus.Items[0].SubItems[1].Text == "OK" && lvwStatus.Items[1].SubItems[1].Text == "OK")
            {
                timUpdateData.Interval = 500;
                timUpdateData.Enabled = true;
            }

            // Restore Endpoint selections
            BitArray ba = new BitArray(new int[] { cfg.EndpointSelections });

            butCheckClick(butApproved, ba[0]);
            butCheckClick(butNotCommunicating, ba[1]);
            butCheckClick(butUnassigned, ba[2]);
            butCheckClick(butUpdateErrors, ba[3]);
            butCheckClick(butDefaultSusID, ba[4]);
            butCheckClick(butGroupRules, ba[5]);
            butCheckClick(butDuplicatePCs, ba[6]);

            // Set up and start the background pinger
            wrkPinger.DoWork += new DoWorkEventHandler(PingWorker);
            wrkPinger.RunWorkerAsync();

            wrkSUSID.DoWork += new DoWorkEventHandler(wrkSUSID_DoWork);

            // Initialise task collection
            tasks = new TaskCollection(cfg);
            tasks.TaskRun += tasks_TaskRun;
            // Bind the task list to grdTasks
            grdTasks.AutoGenerateColumns = false;
            grdTasks.DataSource = tasks.Tasks;

            // Bind to datagrid and configure row properties
            tskID.DataPropertyName = "TaskID";
            tskStatus.DataPropertyName = "CurrentStatus";
            tskIP.DataPropertyName = "IPAddress";
            
            tskCommand.DataPropertyName = "Command";
            tskCommand.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

            tskOutput.DataPropertyName = "Output";
            tskOutput.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

            grdEndpoints.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            // Return cursor to normal
            Cursor.Current = Cursors.Arrow;
        }

        void tasks_TaskRun(object sender, EventArgs e)
        {
            Task t = (Task)sender;

            // Find active task and select it
            DataGridViewRow r = null;

            foreach (DataGridViewRow gr in grdTasks.Rows)
                if (gr.Cells[tskID.Index].Value != null && gr.Cells[tskID.Index].Value.ToString() == t.TaskID.ToString())
                    r = gr;

            // Did we find a row?
            if (r != null)
                // Yes - select it
                grdTasks.CurrentCell = r.Cells[tskID.Index];
        }

        void tasks_ListChanged(object sender, ListChangedEventArgs e)
        {
            DebugOutput("Tasks list changed: Type {0},  Property {1}", e.ListChangedType.ToString(), e.PropertyDescriptor);
        }

        private void frmMain_Closing(object sender, FormClosingEventArgs e)
        {
            // Disable timer to ensure no further ticks happen
            timUpdateData.Enabled = false;

            // Save window location
            SaveWindowLocation();

            Application.Exit();
        }

        private void SaveWindowLocation()
        {
            // Save the window state and (if maximized) normalise window so it will save in the right location
            cfg.WindowState = this.WindowState.ToString();

            // Save the window size and location.
            if (this.WindowState == FormWindowState.Normal)
                cfg.WindowSize = this.Size;
            else
                cfg.WindowSize = this.RestoreBounds.Size;

            cfg.WindowLocation = this.Location;

            // Save Endpoint selections
            int eps = 0;

            if (butApproved.Checked) eps += 1;
            if (butNotCommunicating.Checked) eps += 2;
            if (butUnassigned.Checked) eps += 4;
            if (butUpdateErrors.Checked) eps += 8;
            if (butDefaultSusID.Checked) eps += 16;
            if (butGroupRules.Checked) eps += 32;
            if (butDuplicatePCs.Checked) eps += 64;

            cfg.EndpointSelections = eps;
        }

        private void tabAdminType_Selecting(object sender, TabControlCancelEventArgs e)
        {
            // If the refresh tab is selected, cancel the selection.  A refresh of the tab will take place because next we will...
            if (e.TabPage.Text.ToString() == "Refresh")
            {
                e.Cancel = true;
            }

            // Trigger an update on all tab changes
            ForceUpdate(100);
        }

        private void ForceUpdate(int delay)
        {
            forceUpdate = true;
            timUpdateData.Interval = delay;
        }

        private void frmMain_Resize(object sender, EventArgs e)
        {
            gbxWorking.Left = (this.Width / 2) - (gbxWorking.Width / 2);
            gbxWorking.Top = (this.Height / 2) - (gbxWorking.Height / 2);
        }

        private void butSelectAll_Click(object sender, EventArgs e)
        {
            // Check all items
            foreach (DataGridViewRow r in grdSupercededUpdates.Rows)
            {
                DataGridViewCheckBoxCell c = (DataGridViewCheckBoxCell)r.Cells[suSelect.Index];
                c.Value = true;
            }
        }

        private void butSelectNone_Click(object sender, EventArgs e)
        {
            // Uncheck all items
            foreach (DataGridViewRow r in grdSupercededUpdates.Rows)
            {
                DataGridViewCheckBoxCell c = (DataGridViewCheckBoxCell)r.Cells[suSelect.Index];
                c.Value = false;
            }
        }

        private void butDeclineSelected_Click(object sender, EventArgs e)
        {
            // Loop through all updates, declining those that were selected
            for (int i = 0; i < grdSupercededUpdates.Rows.Count; )
            {
                DataGridViewRow r = grdSupercededUpdates.Rows[i];

                // Is update checked?
                if ((bool)r.Cells[suSelect.Index].Value == false)
                {
                    // No, skip to the next update
                    i++;
                }
                else
                {
                    // Yes - decline update
                    UpdateRevisionId ur = new UpdateRevisionId();
                    ur.UpdateId = new Guid(r.Cells[suUpdateID.Index].Value.ToString());
                    IUpdate u = wsus.server.GetUpdate(ur);

                    u.Decline();

                    grdSupercededUpdates.Rows.Remove(r);
                    this.Refresh();
                }
            }

            // Trigger update of dialog box
            timUpdateData.Interval = 100;
        }

        private void ShowCancelApproveButton(bool show)
        {
            if (show)
            {
                // We're supposed to show the cancel button, which also implies disabling the approve and decline buttons, disabling the timer and resetting the
                // Cancel Now flag
                btnUACancel.Visible = true;
                btnUAApprove.Enabled = false;
                btnUADecline.Enabled = false;

                timUpdateData.Enabled = false;
                cancelNow = false;
                this.Refresh();
            }
            else
            {
                // We're supposed to hide the cancel button, which also implies enabling the approve and decline buttons and the timer
                btnUACancel.Visible = false;
                btnUAApprove.Enabled = true;
                btnUADecline.Enabled = true;

                timUpdateData.Enabled = true;
                this.Refresh();
            }
        }

        private void butCancelApprove_Click(object sender, EventArgs e)
        {
            cancelNow = true;
        }

        private void butCheckClick(ToolStripButton but)
        {
            // Toggle button state
            if (but.Checked)
            {
                but.Checked = false;
                but.Image = Properties.Resources.BuilderDialog_delete;
            }
            else
            {
                but.Checked = true;
                but.Image = Properties.Resources.SuccessComplete;
            }

            EndpointShowHideColumns();
        }

        private void butCheckClick(ToolStripButton but, bool state)
        {
            if (state)
            {
                but.Checked = true;
                but.Image = Properties.Resources.SuccessComplete;
            }
            else
            {
                but.Checked = false;
                but.Image = Properties.Resources.BuilderDialog_delete;
            }

            EndpointShowHideColumns();
        }

        private void EndpointShowHideColumns()
        {
            // Show and hide columns as appropriate for selections
            epApprovedUpdates.Visible = butApproved.Checked;
            epUpdateErrors.Visible = butUpdateErrors.Checked;
            epLastStatus.Visible = butNotCommunicating.Checked;
            epComputerGroup.Visible = butGroupRules.Checked;
            epExtraInfo.Visible = butGroupRules.Checked;

            // Trigger an update after a second to allow end-user to change other selections
            forceUpdate = true;
            timUpdateData.Interval = 1000;
        }

        private void butApproved_Click(object sender, EventArgs e)
        {
            butCheckClick(butApproved);
        }

        private void butNotCommunicating_Click(object sender, EventArgs e)
        {
            butCheckClick(butNotCommunicating);
        }

        private void butUnassigned_Click(object sender, EventArgs e)
        {
            butCheckClick(butUnassigned);
        }

        private void butUpdateErrors_Click(object sender, EventArgs e)
        {
            butCheckClick(butUpdateErrors);
        }

        private void grdEndpoints_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            // Create some useful row variables
            DataGridViewRow r1 = grdEndpoints.Rows[e.RowIndex1];
            DataGridViewRow r2 = grdEndpoints.Rows[e.RowIndex2];

            // Check ping results - anything other than a successful ping result should be equal
            bool p1;
            bool p2;

            if (r1.Cells[epPing.Index].Value == null || r1.Cells[epPing.Index].Value.ToString() == "" || r1.Cells[epPing.Index].Value.ToString() == "No Response")
                p1 = false;
            else
                p1 = true;

            if (r2.Cells[epPing.Index].Value == null || r2.Cells[epPing.Index].Value.ToString() == "" || r2.Cells[epPing.Index].Value.ToString() == "No Response")
                p2 = false;
            else
                p2 = true;

            // Are the two ping results the same
            if (p1 == p2)
            {
                // If the two IPs are the same, sort by fault
                if (r1.Cells[epIP.Index].Value.ToString() == r2.Cells[epIP.Index].Value.ToString())
                {
                    e.SortResult = System.String.Compare(r1.Cells[epFault.Index].Value.ToString(), r2.Cells[epFault.Index].Value.ToString());
                    e.Handled = true;
                }
                else
                {
                    e.SortResult = System.String.Compare(r1.Cells[epIP.Index].Value.ToString(), r2.Cells[epIP.Index].Value.ToString());
                    e.Handled = true;
                }
            }
            else if (!p1  && p2)
            {
                // Cell 2 should come first since only it has a successful ping
                e.SortResult = 1;
                e.Handled = true;
            }
            else if (p1 && !p2)
            {
                // Cell 1 should come first since only it has a successful ping
                e.SortResult = -1;
                e.Handled = true;
            }
        }

        private void butDefaultSusID_Click(object sender, EventArgs e)
        {
            butCheckClick(butDefaultSusID);
        }

        private void butGroupRules_Click(object sender, EventArgs e)
        {
            butCheckClick(butGroupRules);
        }

        private void butDuplicatePCs_Click(object sender, EventArgs e)
        {
            butCheckClick(butDuplicatePCs);
        }

        private void mnuWSUSServer_Click(object sender, EventArgs e)
        {
            Form f = new frmWSUSConfig(cfg);
            f.ShowDialog();
        }

        private void mnuComputerGroupRules_Click(object sender, EventArgs e)
        {
            Form f = new frmComputerGroupRules(cfg);
            f.ShowDialog();
        }

        private void mnuIngoreGroups_Click(object sender, EventArgs e)
        {
            Form f = new frmIgnoreComputerGroups(cfg);
            f.ShowDialog();
        }

        private void mnuDefaultSusIDList_Click(object sender, EventArgs e)
        {
            Form f = new frmDefaultSUS(cfg);
            f.Show();
        }

        private void mnuPreferences_Click(object sender, EventArgs e)
        {
            Form f = new frmPreferences(cfg);
            f.ShowDialog();
        }

        private void mnuCredentials_Click(object sender, EventArgs e)
        {
            Form f = new frmCredentials(cfg);
            f.ShowDialog();
        }

        private void mnuGroupApprovalRules_Click(object sender, EventArgs e)
        {
            Form f = new frmGroupUpdateRules(cfg, wsus);
            f.ShowDialog();
        }

        DataGridViewRow epcmRow;
        IPAddress epcmIPAddress;
        clsConfig.SecurityCredential epcmCreds;
        string epcmFullName;

        private void grdEndpoints_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Was this a right-click?
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                // Yes - save cell context, select cell and show pop-up menu
                epcmRow = grdEndpoints.Rows[e.RowIndex];
                epcmIPAddress = IPAddress.Parse(epcmRow.Cells[epIP.Index].Value.ToString());
                epcmFullName = epcmRow.Cells[epName.Index].Value.ToString();
                grdEndpoints.CurrentCell = grdEndpoints.Rows[e.RowIndex].Cells[e.ColumnIndex];

                // Look for credentials for this PC
                clsConfig.CredentialCollection cc = cfg.CredentialList;
                epcmCreds = cc[epcmIPAddress];

                // Update menu to show details of the selected PC
                epDetails.Text = epcmRow.Cells[epName.Index].Value.ToString() + " at " + epcmIPAddress.ToString();

                // Show pop-up menu
                cmEndpoint.Show(Cursor.Position);
            }
        }

        private void epGPUpdateForce_Click(object sender, EventArgs e)
        {
            PSExecCall(epcmIPAddress, epcmFullName, epcmCreds, "gpupdate /force");
        }

        private void epGPUpdate_Click(object sender, EventArgs e)
        {
            PSExecCall(epcmIPAddress, epcmFullName, epcmCreds, "gpupdate");
        }

        private void epDetectNow_Click(object sender, EventArgs e)
        {
            PSExecCall(epcmIPAddress, epcmFullName, epcmCreds, "wuauclt /detectnow");
        }

        private void epReportNow_Click(object sender, EventArgs e)
        {
            PSExecCall(epcmIPAddress, epcmFullName, epcmCreds, "wuauclt /reportnow");
        }

        private void epResetSusID_Click(object sender, EventArgs e)
        {
            try
            {
                // Stop the Windows Update service
                Task resetsusid = tasks.AddTask(epcmIPAddress, epcmFullName, epcmCreds, "net stop wuauserv");
                
                // Delete the three registry keys
                resetsusid.AddCommand("REG DELETE \"HKLM\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\WindowsUpdate\" /v AccountDomainSid /f");
                resetsusid.AddCommand("REG DELETE \"HKLM\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\WindowsUpdate\" /v PingID /f");
                resetsusid.AddCommand("REG DELETE \"HKLM\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\WindowsUpdate\" /v SusClientId /f");

                // Restart Windows Update service then wait for 5 seconds
                resetsusid.AddCommand("net start wuauserv");
                resetsusid.AddCommand("ping 127.0.0.1 -n 5 >nul");

                // Reset WSUS authorisation token and detect updates
                resetsusid.AddCommand("wuauclt /resetauthorization");
                resetsusid.AddCommand("ping 127.0.0.1 -n 2 >nul");
                resetsusid.AddCommand("wuauclt /detectnow");

                // After 15 seconds, force client to report it's status to it's WSUS server
                resetsusid.AddCommand("ping 127.0.0.1 -n 15 >nul");
                resetsusid.AddCommand("wuauclt /reportnow");

                // Task should time out after 2 minutes
                resetsusid.TimeoutInterval = new TimeSpan(0, 2, 0);
                resetsusid.Status = TaskStatus.Queued;
            }
            catch (ConfigurationException ex)
            {
                // Could not schedule task - inform user of reason why
                MessageBox.Show(ex.Message, ex.Summary, MessageBoxButtons.OK);
            }
        }

        private void mnuResetAuth_Click(object sender, EventArgs e)
        {
            PSExecCall(epcmIPAddress, epcmFullName, epcmCreds, "wuauclt /resetauthorization");
        }

        /// <summary>
        /// Scheule a simple task with no dependent tasks
        /// </summary>
        private void PSExecCall(IPAddress ip, string computername, clsConfig.SecurityCredential credentials, string command)
        {
            try
            {
                Task task = tasks.AddTask(ip, computername, credentials, command);
                task.Ready();
            }
            catch (ConfigurationException ex)
            {
                // Could not schedule task - inform user of reason why
                MessageBox.Show(ex.Message, ex.Summary, MessageBoxButtons.OK);
            }
        }

        private void mnuSUSWatcher_Click(object sender, EventArgs e)
        {
            if (wrkSUSID.IsBusy)
            {
                MessageBox.Show("SUS ID Watcher already running", "Can't start SUS ID Watcher", MessageBoxButtons.OK);
                return;
            }

            wrkSUSID.RunWorkerAsync();
        }

        private void wrkSUSID_DoWork(object sender, DoWorkEventArgs e)
        {
            Form f = new SUSWatcher.frmSUSWatch();

            f.ShowDialog();
        }

        private void grdUnapproved_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Are we getting some dumb values?  Ignore event if so.
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            // Create some variables to make processing a bit easier.
            DataGridViewRow r = grdUnapproved.Rows[e.RowIndex];
            DataGridViewCell c = r.Cells[e.ColumnIndex];

            // Work out default colours for this cell
            Color fore = SystemColors.ControlText;
            Color back;

            if (e.RowIndex % 2 == 0)
                // Lighter highlight for even rows
                back = SystemColors.Control;
            else
                // Darker highlight for odd rows
                back = DarkenColour(SystemColors.Control);

            // Is this one of the group columns?
            if (e.ColumnIndex > uaSortOrder.Index)
            {
                int PCs;
                // Are the contents of this cell numeric?
                if (c.Value != null && int.TryParse(c.Value.ToString(), out PCs))
                {
                    // Looks like it - this is a PC count.  Highlight the cell
                    if (e.RowIndex % 2 == 0)
                        // Lighter highlight for even rows
                        back = Color.LightGreen;
                    else
                        // Darker highlight for odd rows
                        back = DarkenColour(Color.LightGreen);
                }
                else
                    // No - the text colour should be light
                    fore = MidColour(SystemColors.ControlText, back);
            }

            // Set cell colour
            c.Style.ForeColor = fore;
            c.Style.BackColor = back;
        }

        private Color MidColour(Color a, Color b)
        {
            // Calculate the colour that's halfway between the two provided colours
            return Color.FromArgb((a.R + b.R) / 2, (a.G + b.G) / 2, (a.B + b.B) / 2);
        }

        private Color DarkenColour(Color c)
        {
            return Color.FromArgb((int)(c.R * 0.8), (int)(c.G * 0.8), (int)(c.B * 0.8));
        }

        private void btnUAApprove_Click(object sender, EventArgs e)
        {
            // Show cancel button
            ShowCancelApproveButton(true);

            // Loop through each selected cell and see what to approve
            foreach (DataGridViewCell c in grdUnapproved.SelectedCells)
            {
                // Break out of the loop now if the cancel flag has been set
                if (cancelNow) break;

                // Only allow the update to be approved if some PCs require it (and if this is an update column)
                int PCs;

                if (c.ColumnIndex > uaSortOrder.Index && int.TryParse(c.Value.ToString(), out PCs))
                {
                    // Ensure update is visible so end user can see what's going on...
                    grdUnapproved.CurrentCell = c;
                    this.Refresh();

                    // Get the appropriate update object
                    UpdateRevisionId ur = new UpdateRevisionId();
                    ur.UpdateId = new Guid(grdUnapproved.Rows[c.RowIndex].Cells[uaID.Index].Value.ToString());
                    IUpdate u = wsus.server.GetUpdate(ur);

                    // Grab computer group object from column header
                    IComputerTargetGroup tg = (IComputerTargetGroup)grdUnapproved.Columns[c.ColumnIndex].Tag;

                    // If a valid group was found, approve the update
                    if (tg != null)
                    {
                        bool canapprove = true;

                        // Does the update require a EULA approval?
                        if (u.RequiresLicenseAgreementAcceptance)
                        {
                            // Get license agreement, check to see if the license has been agreed to. and approve it if it hasn't
                            ILicenseAgreement eula = u.GetLicenseAgreement();
                            if (!eula.IsAccepted)
                            {
                                // EULA requires approval - display it to the user and request approval.
                                if (MessageBox.Show(eula.Text, string.Format("{0} requires end-user license acceptance", u.Title), MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                                    // EULA accepted.  Mark acceptance.
                                    u.AcceptLicenseAgreement();
                                else
                                    // EULA rejected - update can't be approved
                                    canapprove = false;
                            }
                        }

                        // If update can be approved, do it.
                        if (canapprove)
                        {
                            u.Approve(UpdateApprovalAction.Install, tg);

                            // Empty cell and unselect it so the end user knows the update has been approved
                            c.Value = "Approved";
                            c.Selected = false;
                            this.Refresh();
                        }

                        // Process outstanding events to allow end user to cancel approvals if they want
                        Application.DoEvents();
                    }
                }
            }

            // Hide the cancel button, enable the approve button and the timer
            ShowCancelApproveButton(false);

            // Trigger update of unapproved updates
            timUpdateData.Interval = 100;
            forceUpdate = true;
        }

        private void btnUACancel_Click(object sender, EventArgs e)
        {
            cancelNow = true;
        }

        private void btnUADecline_Click(object sender, EventArgs e)
        {
            // Warn user that this will decline updates for *all* groups, even if the update is already approved
            if (MessageBox.Show("Declining updates affects ALL groups, not just the selected group!" + Environment.NewLine +
                "Proceeding will decline this update for all groups, even if the update has already been approved for other groups." + Environment.NewLine + Environment.NewLine +
                "Do you wish to proceed?", "WARNING", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
            {
                // User declined to proceed - return without declining any updates
                return;
            }

            // Show the cancel button
            ShowCancelApproveButton(true);

            // Loop through each selected cell and see what to decline
            foreach (DataGridViewCell c in grdUnapproved.SelectedCells)
            {
                // Break out of the loop now if the cancel flag has been set
                if (cancelNow) break;

                // Only allow the update to be declined if some PCs require it.
                if (c.Value.ToString() != "")
                {
                    // Ensure update is visible so end user can see what's going on...
                    grdUnapproved.CurrentCell = c;
                    this.Refresh();

                    // Get the appropriate update object
                    UpdateRevisionId ur = new UpdateRevisionId();
                    ur.UpdateId = new Guid(grdUnapproved.Rows[c.RowIndex].Cells[uaID.Index].Value.ToString());
                    IUpdate u = wsus.server.GetUpdate(ur);

                    // Decline the update and update the cell so the end user knows what's going on.
                    u.Decline();
                    c.Value = "Declined";

                    // Process outstanding events to allow end user to cancel approvals if they want
                    Application.DoEvents();
                }
            }

            // Hide the cancel button, enable the approve button and the timer
            ShowCancelApproveButton(false);

            // Trigger update of unapproved updates
            timUpdateData.Interval = 100;
            forceUpdate = true;
        }

        private void grdUnapproved_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Create handy variables referring to this cell
            DataGridViewRow r = grdUnapproved.Rows[e.RowIndex];
            DataGridViewCell c = r.Cells[e.ColumnIndex];
            DataGridViewColumn gc = grdUnapproved.Columns[e.ColumnIndex];

            // Was this a left click?
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                // Left click - check to see if KB column was selected - if it was, open link in default browser
                if (gc.HeaderText == "KB Article")
                    Process.Start("http://support.microsoft.com/kb/" + c.Value.ToString());

                // If the column selected isn't an update row, deselect it
                if (e.ColumnIndex <= uaSortOrder.Index)
                    c.Selected = false;
            }
        }

        private void TriggerRefreshTimer()
        {
            // Only enable the timer if we're not reloading the list of updates
            if (true)
            {
                timRefreshGrid.Enabled = true;
                timRefreshGrid.Interval = 1000;
            }
        }
            
        private void txtFilterName_TextChanged(object sender, EventArgs e)
        {
            TriggerRefreshTimer();
        }

        private void txtFilterDescription_TextChanged(object sender, EventArgs e)
        {
            TriggerRefreshTimer();
        }

        private void timRefreshGrid_Tick(object sender, EventArgs e)
        {
            timRefreshGrid.Enabled = false;

            FillUpdateDataGrid();
        }
    }
}