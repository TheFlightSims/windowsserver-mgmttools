namespace WSUSAdminAssistant
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.timUpdateData = new System.Windows.Forms.Timer(this.components);
            this.gbxWorking = new System.Windows.Forms.GroupBox();
            this.picReloading = new System.Windows.Forms.PictureBox();
            this.lblReload = new System.Windows.Forms.Label();
            this.cmEndpoint = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.epDetails = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripSeparator();
            this.epGPUpdate = new System.Windows.Forms.ToolStripMenuItem();
            this.epGPUpdateForce = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.epResetSusID = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuResetAuth = new System.Windows.Forms.ToolStripMenuItem();
            this.epDetectNow = new System.Windows.Forms.ToolStripMenuItem();
            this.epReportNow = new System.Windows.Forms.ToolStripMenuItem();
            this.dataGridViewImageColumn1 = new System.Windows.Forms.DataGridViewImageColumn();
            this.tabRefresh = new System.Windows.Forms.TabPage();
            this.tabSuperceded = new System.Windows.Forms.TabPage();
            this.grdSupercededUpdates = new System.Windows.Forms.DataGridView();
            this.suUpdateName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.suUpdateID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.suSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.tlsSuperceded = new System.Windows.Forms.ToolStrip();
            this.lblUpdateCount = new System.Windows.Forms.ToolStripButton();
            this.butDeclineSelected = new System.Windows.Forms.ToolStripButton();
            this.butSelectNone = new System.Windows.Forms.ToolStripButton();
            this.butSelectAll = new System.Windows.Forms.ToolStripButton();
            this.tabServerRestarts = new System.Windows.Forms.TabPage();
            this.lstServers = new System.Windows.Forms.ListBox();
            this.tabWSUSNotCommunicating = new System.Windows.Forms.TabPage();
            this.grdWSUSNotCommunicting = new System.Windows.Forms.DataGridView();
            this.wnuServerName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.wnuLastSync = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.wnuLastRollup = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabEndpointFaults = new System.Windows.Forms.TabPage();
            this.splEndpoint = new System.Windows.Forms.SplitContainer();
            this.grdEndpoints = new System.Windows.Forms.DataGridView();
            this.epName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.epUpdate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.epIP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.epDownstreamServer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.epComputerGroup = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.epFault = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.epApprovedUpdates = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.epUpdateErrors = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.epLastContact = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.epLastStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.epPing = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.epExtraInfo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.epSortOrder = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.epPingUpdated = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tlsEndpoint = new System.Windows.Forms.ToolStrip();
            this.butApproved = new System.Windows.Forms.ToolStripButton();
            this.butUpdateErrors = new System.Windows.Forms.ToolStripButton();
            this.butNotCommunicating = new System.Windows.Forms.ToolStripButton();
            this.butUnassigned = new System.Windows.Forms.ToolStripButton();
            this.butDefaultSusID = new System.Windows.Forms.ToolStripButton();
            this.butGroupRules = new System.Windows.Forms.ToolStripButton();
            this.butDuplicatePCs = new System.Windows.Forms.ToolStripButton();
            this.grdTasks = new System.Windows.Forms.DataGridView();
            this.tskID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tskStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tskIP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tskCommand = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tskOutput = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabUnapproved = new System.Windows.Forms.TabPage();
            this.grdUnapproved = new System.Windows.Forms.DataGridView();
            this.uaUpdateName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.uaID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.uaUpdated = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.uaDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.uaKB = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.uaSortOrder = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tlsUnapproved = new System.Windows.Forms.ToolStrip();
            this.btnUAApprove = new System.Windows.Forms.ToolStripButton();
            this.btnUADecline = new System.Windows.Forms.ToolStripButton();
            this.btnUACancel = new System.Windows.Forms.ToolStripButton();
            this.lblUpdatesToApprove = new System.Windows.Forms.ToolStripLabel();
            this.mnuHideGroups = new System.Windows.Forms.ToolStripDropDownButton();
            this.tlsFilterName = new System.Windows.Forms.ToolStripLabel();
            this.txtFilterName = new System.Windows.Forms.ToolStripTextBox();
            this.lblFilterDescription = new System.Windows.Forms.ToolStripLabel();
            this.txtFilterDescription = new System.Windows.Forms.ToolStripTextBox();
            this.lblFilterArticle = new System.Windows.Forms.ToolStripLabel();
            this.txtFilterArticle = new System.Windows.Forms.ToolStripTextBox();
            this.tabHome = new System.Windows.Forms.TabPage();
            this.lvwStatus = new System.Windows.Forms.ListView();
            this.ssInfo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ssData = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ssExtraInfo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.mnu = new System.Windows.Forms.MenuStrip();
            this.mnuOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuWSUSServer = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuComputerGroupRules = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuIngoreGroups = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuGroupApprovalRules = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCredentials = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDefaultSusIDList = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuPreferences = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuUtilities = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSUSWatcher = new System.Windows.Forms.ToolStripMenuItem();
            this.tabAdminType = new System.Windows.Forms.TabControl();
            this.timRefreshGrid = new System.Windows.Forms.Timer(this.components);
            this.gbxWorking.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picReloading)).BeginInit();
            this.cmEndpoint.SuspendLayout();
            this.tabSuperceded.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdSupercededUpdates)).BeginInit();
            this.tlsSuperceded.SuspendLayout();
            this.tabServerRestarts.SuspendLayout();
            this.tabWSUSNotCommunicating.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdWSUSNotCommunicting)).BeginInit();
            this.tabEndpointFaults.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splEndpoint)).BeginInit();
            this.splEndpoint.Panel1.SuspendLayout();
            this.splEndpoint.Panel2.SuspendLayout();
            this.splEndpoint.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdEndpoints)).BeginInit();
            this.tlsEndpoint.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdTasks)).BeginInit();
            this.tabUnapproved.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdUnapproved)).BeginInit();
            this.tlsUnapproved.SuspendLayout();
            this.tabHome.SuspendLayout();
            this.mnu.SuspendLayout();
            this.tabAdminType.SuspendLayout();
            this.SuspendLayout();
            // 
            // timUpdateData
            // 
            this.timUpdateData.Interval = 500;
            this.timUpdateData.Tick += new System.EventHandler(this.timUpdateData_Tick);
            // 
            // gbxWorking
            // 
            this.gbxWorking.Controls.Add(this.picReloading);
            this.gbxWorking.Controls.Add(this.lblReload);
            this.gbxWorking.Location = new System.Drawing.Point(68, 169);
            this.gbxWorking.Name = "gbxWorking";
            this.gbxWorking.Size = new System.Drawing.Size(240, 102);
            this.gbxWorking.TabIndex = 2;
            this.gbxWorking.TabStop = false;
            this.gbxWorking.Text = "Working...";
            this.gbxWorking.Visible = false;
            // 
            // picReloading
            // 
            this.picReloading.Image = ((System.Drawing.Image)(resources.GetObject("picReloading.Image")));
            this.picReloading.Location = new System.Drawing.Point(18, 29);
            this.picReloading.Name = "picReloading";
            this.picReloading.Size = new System.Drawing.Size(48, 50);
            this.picReloading.TabIndex = 1;
            this.picReloading.TabStop = false;
            // 
            // lblReload
            // 
            this.lblReload.AutoSize = true;
            this.lblReload.Location = new System.Drawing.Point(72, 46);
            this.lblReload.Name = "lblReload";
            this.lblReload.Size = new System.Drawing.Size(149, 13);
            this.lblReload.TabIndex = 0;
            this.lblReload.Text = "Please wait... reloading data...";
            // 
            // cmEndpoint
            // 
            this.cmEndpoint.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.epDetails,
            this.toolStripMenuItem6,
            this.epGPUpdate,
            this.epGPUpdateForce,
            this.toolStripMenuItem5,
            this.epResetSusID,
            this.mnuResetAuth,
            this.epDetectNow,
            this.epReportNow});
            this.cmEndpoint.Name = "cmEndpoint";
            this.cmEndpoint.Size = new System.Drawing.Size(231, 170);
            // 
            // epDetails
            // 
            this.epDetails.Enabled = false;
            this.epDetails.Name = "epDetails";
            this.epDetails.Size = new System.Drawing.Size(230, 22);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(227, 6);
            // 
            // epGPUpdate
            // 
            this.epGPUpdate.Name = "epGPUpdate";
            this.epGPUpdate.Size = new System.Drawing.Size(230, 22);
            this.epGPUpdate.Text = "&Group Policy Update";
            this.epGPUpdate.Click += new System.EventHandler(this.epGPUpdate_Click);
            // 
            // epGPUpdateForce
            // 
            this.epGPUpdateForce.Name = "epGPUpdateForce";
            this.epGPUpdateForce.Size = new System.Drawing.Size(230, 22);
            this.epGPUpdateForce.Text = "Group Policy Update (&Forced)";
            this.epGPUpdateForce.Click += new System.EventHandler(this.epGPUpdateForce_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(227, 6);
            // 
            // epResetSusID
            // 
            this.epResetSusID.Name = "epResetSusID";
            this.epResetSusID.Size = new System.Drawing.Size(230, 22);
            this.epResetSusID.Text = "&Reset SUS ID";
            this.epResetSusID.Click += new System.EventHandler(this.epResetSusID_Click);
            // 
            // mnuResetAuth
            // 
            this.mnuResetAuth.Name = "mnuResetAuth";
            this.mnuResetAuth.Size = new System.Drawing.Size(230, 22);
            this.mnuResetAuth.Text = "Reset &Authorisation Token";
            this.mnuResetAuth.Click += new System.EventHandler(this.mnuResetAuth_Click);
            // 
            // epDetectNow
            // 
            this.epDetectNow.Name = "epDetectNow";
            this.epDetectNow.Size = new System.Drawing.Size(230, 22);
            this.epDetectNow.Text = "&Detect Updates Now";
            this.epDetectNow.Click += new System.EventHandler(this.epDetectNow_Click);
            // 
            // epReportNow
            // 
            this.epReportNow.Name = "epReportNow";
            this.epReportNow.Size = new System.Drawing.Size(230, 22);
            this.epReportNow.Text = "&Report Update Status Now";
            this.epReportNow.Click += new System.EventHandler(this.epReportNow_Click);
            // 
            // dataGridViewImageColumn1
            // 
            this.dataGridViewImageColumn1.HeaderText = "Status";
            this.dataGridViewImageColumn1.Name = "dataGridViewImageColumn1";
            // 
            // tabRefresh
            // 
            this.tabRefresh.Location = new System.Drawing.Point(4, 22);
            this.tabRefresh.Name = "tabRefresh";
            this.tabRefresh.Padding = new System.Windows.Forms.Padding(3);
            this.tabRefresh.Size = new System.Drawing.Size(1038, 459);
            this.tabRefresh.TabIndex = 2;
            this.tabRefresh.Text = "Refresh";
            this.tabRefresh.UseVisualStyleBackColor = true;
            // 
            // tabSuperceded
            // 
            this.tabSuperceded.Controls.Add(this.grdSupercededUpdates);
            this.tabSuperceded.Controls.Add(this.tlsSuperceded);
            this.tabSuperceded.Location = new System.Drawing.Point(4, 22);
            this.tabSuperceded.Name = "tabSuperceded";
            this.tabSuperceded.Padding = new System.Windows.Forms.Padding(3);
            this.tabSuperceded.Size = new System.Drawing.Size(1038, 459);
            this.tabSuperceded.TabIndex = 5;
            this.tabSuperceded.Text = "Superceded Updates";
            this.tabSuperceded.UseVisualStyleBackColor = true;
            // 
            // grdSupercededUpdates
            // 
            this.grdSupercededUpdates.AllowUserToAddRows = false;
            this.grdSupercededUpdates.AllowUserToDeleteRows = false;
            this.grdSupercededUpdates.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.ColumnHeader;
            this.grdSupercededUpdates.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdSupercededUpdates.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.suUpdateName,
            this.suUpdateID,
            this.suSelect});
            this.grdSupercededUpdates.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdSupercededUpdates.Location = new System.Drawing.Point(3, 28);
            this.grdSupercededUpdates.Name = "grdSupercededUpdates";
            this.grdSupercededUpdates.Size = new System.Drawing.Size(1032, 428);
            this.grdSupercededUpdates.TabIndex = 0;
            // 
            // suUpdateName
            // 
            this.suUpdateName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.suUpdateName.HeaderText = "Update Name";
            this.suUpdateName.Name = "suUpdateName";
            this.suUpdateName.ReadOnly = true;
            // 
            // suUpdateID
            // 
            this.suUpdateID.HeaderText = "Update ID";
            this.suUpdateID.Name = "suUpdateID";
            this.suUpdateID.ReadOnly = true;
            this.suUpdateID.Visible = false;
            this.suUpdateID.Width = 81;
            // 
            // suSelect
            // 
            this.suSelect.HeaderText = "Select";
            this.suSelect.Name = "suSelect";
            this.suSelect.Width = 43;
            // 
            // tlsSuperceded
            // 
            this.tlsSuperceded.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblUpdateCount,
            this.butDeclineSelected,
            this.butSelectNone,
            this.butSelectAll});
            this.tlsSuperceded.Location = new System.Drawing.Point(3, 3);
            this.tlsSuperceded.Name = "tlsSuperceded";
            this.tlsSuperceded.Size = new System.Drawing.Size(1032, 25);
            this.tlsSuperceded.TabIndex = 1;
            this.tlsSuperceded.Text = "toolStrip1";
            // 
            // lblUpdateCount
            // 
            this.lblUpdateCount.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.lblUpdateCount.Image = ((System.Drawing.Image)(resources.GetObject("lblUpdateCount.Image")));
            this.lblUpdateCount.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.lblUpdateCount.Name = "lblUpdateCount";
            this.lblUpdateCount.Size = new System.Drawing.Size(23, 22);
            // 
            // butDeclineSelected
            // 
            this.butDeclineSelected.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.butDeclineSelected.Image = ((System.Drawing.Image)(resources.GetObject("butDeclineSelected.Image")));
            this.butDeclineSelected.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.butDeclineSelected.Name = "butDeclineSelected";
            this.butDeclineSelected.Size = new System.Drawing.Size(159, 22);
            this.butDeclineSelected.Text = "Decline Selected Updates";
            this.butDeclineSelected.Click += new System.EventHandler(this.butDeclineSelected_Click);
            // 
            // butSelectNone
            // 
            this.butSelectNone.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.butSelectNone.Image = ((System.Drawing.Image)(resources.GetObject("butSelectNone.Image")));
            this.butSelectNone.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.butSelectNone.Name = "butSelectNone";
            this.butSelectNone.Size = new System.Drawing.Size(90, 22);
            this.butSelectNone.Text = "Select None";
            this.butSelectNone.Click += new System.EventHandler(this.butSelectNone_Click);
            // 
            // butSelectAll
            // 
            this.butSelectAll.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.butSelectAll.Image = ((System.Drawing.Image)(resources.GetObject("butSelectAll.Image")));
            this.butSelectAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.butSelectAll.Name = "butSelectAll";
            this.butSelectAll.Size = new System.Drawing.Size(75, 22);
            this.butSelectAll.Text = "Select All";
            this.butSelectAll.Click += new System.EventHandler(this.butSelectAll_Click);
            // 
            // tabServerRestarts
            // 
            this.tabServerRestarts.Controls.Add(this.lstServers);
            this.tabServerRestarts.Location = new System.Drawing.Point(4, 22);
            this.tabServerRestarts.Name = "tabServerRestarts";
            this.tabServerRestarts.Padding = new System.Windows.Forms.Padding(3);
            this.tabServerRestarts.Size = new System.Drawing.Size(1038, 459);
            this.tabServerRestarts.TabIndex = 3;
            this.tabServerRestarts.Text = "Servers Requiring Restarts";
            this.tabServerRestarts.UseVisualStyleBackColor = true;
            // 
            // lstServers
            // 
            this.lstServers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstServers.FormattingEnabled = true;
            this.lstServers.Location = new System.Drawing.Point(3, 3);
            this.lstServers.Name = "lstServers";
            this.lstServers.Size = new System.Drawing.Size(1032, 453);
            this.lstServers.TabIndex = 0;
            // 
            // tabWSUSNotCommunicating
            // 
            this.tabWSUSNotCommunicating.Controls.Add(this.grdWSUSNotCommunicting);
            this.tabWSUSNotCommunicating.Location = new System.Drawing.Point(4, 22);
            this.tabWSUSNotCommunicating.Name = "tabWSUSNotCommunicating";
            this.tabWSUSNotCommunicating.Size = new System.Drawing.Size(1038, 459);
            this.tabWSUSNotCommunicating.TabIndex = 7;
            this.tabWSUSNotCommunicating.Text = "WSUS servers not communicating";
            this.tabWSUSNotCommunicating.UseVisualStyleBackColor = true;
            // 
            // grdWSUSNotCommunicting
            // 
            this.grdWSUSNotCommunicting.AllowUserToAddRows = false;
            this.grdWSUSNotCommunicting.AllowUserToDeleteRows = false;
            this.grdWSUSNotCommunicting.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdWSUSNotCommunicting.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.wnuServerName,
            this.wnuLastSync,
            this.wnuLastRollup});
            this.grdWSUSNotCommunicting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdWSUSNotCommunicting.Location = new System.Drawing.Point(0, 0);
            this.grdWSUSNotCommunicting.Name = "grdWSUSNotCommunicting";
            this.grdWSUSNotCommunicting.ReadOnly = true;
            this.grdWSUSNotCommunicting.Size = new System.Drawing.Size(1038, 459);
            this.grdWSUSNotCommunicting.TabIndex = 2;
            // 
            // wnuServerName
            // 
            this.wnuServerName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.wnuServerName.HeaderText = "Server Name";
            this.wnuServerName.Name = "wnuServerName";
            this.wnuServerName.ReadOnly = true;
            // 
            // wnuLastSync
            // 
            this.wnuLastSync.HeaderText = "Last Sync";
            this.wnuLastSync.Name = "wnuLastSync";
            this.wnuLastSync.ReadOnly = true;
            this.wnuLastSync.Width = 150;
            // 
            // wnuLastRollup
            // 
            this.wnuLastRollup.HeaderText = "Last Rollup Time";
            this.wnuLastRollup.Name = "wnuLastRollup";
            this.wnuLastRollup.ReadOnly = true;
            this.wnuLastRollup.Width = 150;
            // 
            // tabEndpointFaults
            // 
            this.tabEndpointFaults.Controls.Add(this.splEndpoint);
            this.tabEndpointFaults.Location = new System.Drawing.Point(4, 22);
            this.tabEndpointFaults.Name = "tabEndpointFaults";
            this.tabEndpointFaults.Size = new System.Drawing.Size(1038, 459);
            this.tabEndpointFaults.TabIndex = 9;
            this.tabEndpointFaults.Text = "Endpoint Faults";
            this.tabEndpointFaults.UseVisualStyleBackColor = true;
            // 
            // splEndpoint
            // 
            this.splEndpoint.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splEndpoint.Location = new System.Drawing.Point(0, 0);
            this.splEndpoint.Name = "splEndpoint";
            this.splEndpoint.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splEndpoint.Panel1
            // 
            this.splEndpoint.Panel1.Controls.Add(this.grdEndpoints);
            this.splEndpoint.Panel1.Controls.Add(this.tlsEndpoint);
            // 
            // splEndpoint.Panel2
            // 
            this.splEndpoint.Panel2.Controls.Add(this.grdTasks);
            this.splEndpoint.Size = new System.Drawing.Size(1038, 459);
            this.splEndpoint.SplitterDistance = 353;
            this.splEndpoint.TabIndex = 3;
            // 
            // grdEndpoints
            // 
            this.grdEndpoints.AllowUserToAddRows = false;
            this.grdEndpoints.AllowUserToDeleteRows = false;
            this.grdEndpoints.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdEndpoints.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.epName,
            this.epUpdate,
            this.epIP,
            this.epDownstreamServer,
            this.epComputerGroup,
            this.epFault,
            this.epApprovedUpdates,
            this.epUpdateErrors,
            this.epLastContact,
            this.epLastStatus,
            this.epPing,
            this.epExtraInfo,
            this.epSortOrder,
            this.epPingUpdated});
            this.grdEndpoints.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdEndpoints.Location = new System.Drawing.Point(0, 25);
            this.grdEndpoints.MultiSelect = false;
            this.grdEndpoints.Name = "grdEndpoints";
            this.grdEndpoints.ReadOnly = true;
            this.grdEndpoints.Size = new System.Drawing.Size(1038, 328);
            this.grdEndpoints.TabIndex = 4;
            this.grdEndpoints.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.grdEndpoints_CellMouseClick);
            this.grdEndpoints.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.grdEndpoints_SortCompare);
            // 
            // epName
            // 
            this.epName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.epName.HeaderText = "PC Name";
            this.epName.Name = "epName";
            this.epName.ReadOnly = true;
            // 
            // epUpdate
            // 
            this.epUpdate.HeaderText = "Update";
            this.epUpdate.Name = "epUpdate";
            this.epUpdate.ReadOnly = true;
            this.epUpdate.Visible = false;
            // 
            // epIP
            // 
            this.epIP.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.epIP.HeaderText = "IP Address";
            this.epIP.Name = "epIP";
            this.epIP.ReadOnly = true;
            this.epIP.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // epDownstreamServer
            // 
            this.epDownstreamServer.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.epDownstreamServer.HeaderText = "Downstream WSUS Server";
            this.epDownstreamServer.Name = "epDownstreamServer";
            this.epDownstreamServer.ReadOnly = true;
            this.epDownstreamServer.Width = 119;
            // 
            // epComputerGroup
            // 
            this.epComputerGroup.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.epComputerGroup.HeaderText = "Computer Group";
            this.epComputerGroup.Name = "epComputerGroup";
            this.epComputerGroup.ReadOnly = true;
            // 
            // epFault
            // 
            this.epFault.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.epFault.HeaderText = "Endpoint Fault";
            this.epFault.Name = "epFault";
            this.epFault.ReadOnly = true;
            this.epFault.Width = 92;
            // 
            // epApprovedUpdates
            // 
            this.epApprovedUpdates.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.epApprovedUpdates.HeaderText = "Approved Updates";
            this.epApprovedUpdates.Name = "epApprovedUpdates";
            this.epApprovedUpdates.ReadOnly = true;
            this.epApprovedUpdates.Width = 70;
            // 
            // epUpdateErrors
            // 
            this.epUpdateErrors.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.epUpdateErrors.HeaderText = "Updates With Errors";
            this.epUpdateErrors.Name = "epUpdateErrors";
            this.epUpdateErrors.ReadOnly = true;
            this.epUpdateErrors.Width = 92;
            // 
            // epLastContact
            // 
            this.epLastContact.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.epLastContact.HeaderText = "Last Contact";
            this.epLastContact.Name = "epLastContact";
            this.epLastContact.ReadOnly = true;
            this.epLastContact.Width = 85;
            // 
            // epLastStatus
            // 
            this.epLastStatus.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.epLastStatus.HeaderText = "Last Status";
            this.epLastStatus.Name = "epLastStatus";
            this.epLastStatus.ReadOnly = true;
            this.epLastStatus.Width = 78;
            // 
            // epPing
            // 
            this.epPing.HeaderText = "Ping";
            this.epPing.Name = "epPing";
            this.epPing.ReadOnly = true;
            // 
            // epExtraInfo
            // 
            this.epExtraInfo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.epExtraInfo.HeaderText = "Extra Information";
            this.epExtraInfo.Name = "epExtraInfo";
            this.epExtraInfo.ReadOnly = true;
            this.epExtraInfo.Width = 102;
            // 
            // epSortOrder
            // 
            this.epSortOrder.HeaderText = "Sort Order";
            this.epSortOrder.Name = "epSortOrder";
            this.epSortOrder.ReadOnly = true;
            this.epSortOrder.Visible = false;
            // 
            // epPingUpdated
            // 
            this.epPingUpdated.HeaderText = "Updated";
            this.epPingUpdated.Name = "epPingUpdated";
            this.epPingUpdated.ReadOnly = true;
            this.epPingUpdated.Visible = false;
            // 
            // tlsEndpoint
            // 
            this.tlsEndpoint.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tlsEndpoint.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.butApproved,
            this.butUpdateErrors,
            this.butNotCommunicating,
            this.butUnassigned,
            this.butDefaultSusID,
            this.butGroupRules,
            this.butDuplicatePCs});
            this.tlsEndpoint.Location = new System.Drawing.Point(0, 0);
            this.tlsEndpoint.Name = "tlsEndpoint";
            this.tlsEndpoint.Size = new System.Drawing.Size(1038, 25);
            this.tlsEndpoint.TabIndex = 3;
            // 
            // butApproved
            // 
            this.butApproved.Checked = true;
            this.butApproved.CheckState = System.Windows.Forms.CheckState.Checked;
            this.butApproved.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.butApproved.Name = "butApproved";
            this.butApproved.Size = new System.Drawing.Size(187, 22);
            this.butApproved.Text = "Approved but Unapplied Updates";
            this.butApproved.Click += new System.EventHandler(this.butApproved_Click);
            // 
            // butUpdateErrors
            // 
            this.butUpdateErrors.Checked = true;
            this.butUpdateErrors.CheckState = System.Windows.Forms.CheckState.Checked;
            this.butUpdateErrors.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.butUpdateErrors.Name = "butUpdateErrors";
            this.butUpdateErrors.Size = new System.Drawing.Size(113, 22);
            this.butUpdateErrors.Text = "Updates with Errors";
            this.butUpdateErrors.Click += new System.EventHandler(this.butUpdateErrors_Click);
            // 
            // butNotCommunicating
            // 
            this.butNotCommunicating.Checked = true;
            this.butNotCommunicating.CheckState = System.Windows.Forms.CheckState.Checked;
            this.butNotCommunicating.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.butNotCommunicating.Name = "butNotCommunicating";
            this.butNotCommunicating.Size = new System.Drawing.Size(121, 22);
            this.butNotCommunicating.Text = "Not Communicating";
            this.butNotCommunicating.Click += new System.EventHandler(this.butNotCommunicating_Click);
            // 
            // butUnassigned
            // 
            this.butUnassigned.Checked = true;
            this.butUnassigned.CheckState = System.Windows.Forms.CheckState.Checked;
            this.butUnassigned.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.butUnassigned.Name = "butUnassigned";
            this.butUnassigned.Size = new System.Drawing.Size(141, 22);
            this.butUnassigned.Text = "Not Assigned to a Group";
            this.butUnassigned.Click += new System.EventHandler(this.butUnassigned_Click);
            // 
            // butDefaultSusID
            // 
            this.butDefaultSusID.Checked = true;
            this.butDefaultSusID.CheckState = System.Windows.Forms.CheckState.Checked;
            this.butDefaultSusID.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.butDefaultSusID.Name = "butDefaultSusID";
            this.butDefaultSusID.Size = new System.Drawing.Size(86, 22);
            this.butDefaultSusID.Text = "Default SUS ID";
            this.butDefaultSusID.Click += new System.EventHandler(this.butDefaultSusID_Click);
            // 
            // butGroupRules
            // 
            this.butGroupRules.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.butGroupRules.Name = "butGroupRules";
            this.butGroupRules.Size = new System.Drawing.Size(143, 22);
            this.butGroupRules.Text = "PCs not in Correct Group";
            this.butGroupRules.Click += new System.EventHandler(this.butGroupRules_Click);
            // 
            // butDuplicatePCs
            // 
            this.butDuplicatePCs.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.butDuplicatePCs.Name = "butDuplicatePCs";
            this.butDuplicatePCs.Size = new System.Drawing.Size(84, 22);
            this.butDuplicatePCs.Text = "Duplicate PCs";
            this.butDuplicatePCs.Click += new System.EventHandler(this.butDuplicatePCs_Click);
            // 
            // grdTasks
            // 
            this.grdTasks.AllowUserToAddRows = false;
            this.grdTasks.AllowUserToDeleteRows = false;
            this.grdTasks.AllowUserToResizeColumns = false;
            this.grdTasks.AllowUserToResizeRows = false;
            this.grdTasks.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.grdTasks.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdTasks.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.tskID,
            this.tskStatus,
            this.tskIP,
            this.tskCommand,
            this.tskOutput});
            this.grdTasks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdTasks.Location = new System.Drawing.Point(0, 0);
            this.grdTasks.Name = "grdTasks";
            this.grdTasks.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.grdTasks.Size = new System.Drawing.Size(1038, 102);
            this.grdTasks.TabIndex = 0;
            // 
            // tskID
            // 
            this.tskID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.tskID.DataPropertyName = "TaskID";
            this.tskID.HeaderText = "Task ID";
            this.tskID.Name = "tskID";
            this.tskID.ReadOnly = true;
            this.tskID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.tskID.Width = 51;
            // 
            // tskStatus
            // 
            this.tskStatus.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.tskStatus.DataPropertyName = "CurrentStatus";
            this.tskStatus.HeaderText = "Status";
            this.tskStatus.Name = "tskStatus";
            this.tskStatus.ReadOnly = true;
            this.tskStatus.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.tskStatus.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.tskStatus.Width = 43;
            // 
            // tskIP
            // 
            this.tskIP.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.tskIP.DataPropertyName = "IPAddress";
            this.tskIP.HeaderText = "IP Address";
            this.tskIP.Name = "tskIP";
            this.tskIP.ReadOnly = true;
            this.tskIP.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.tskIP.Width = 64;
            // 
            // tskCommand
            // 
            this.tskCommand.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.tskCommand.DataPropertyName = "Command";
            this.tskCommand.HeaderText = "Command";
            this.tskCommand.Name = "tskCommand";
            this.tskCommand.ReadOnly = true;
            this.tskCommand.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.tskCommand.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.tskCommand.Width = 60;
            // 
            // tskOutput
            // 
            this.tskOutput.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.tskOutput.HeaderText = "Output";
            this.tskOutput.Name = "tskOutput";
            this.tskOutput.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // tabUnapproved
            // 
            this.tabUnapproved.Controls.Add(this.grdUnapproved);
            this.tabUnapproved.Controls.Add(this.tlsUnapproved);
            this.tabUnapproved.Location = new System.Drawing.Point(4, 22);
            this.tabUnapproved.Name = "tabUnapproved";
            this.tabUnapproved.Padding = new System.Windows.Forms.Padding(3);
            this.tabUnapproved.Size = new System.Drawing.Size(1038, 459);
            this.tabUnapproved.TabIndex = 11;
            this.tabUnapproved.Text = "Unapproved Updates";
            this.tabUnapproved.UseVisualStyleBackColor = true;
            // 
            // grdUnapproved
            // 
            this.grdUnapproved.AllowUserToAddRows = false;
            this.grdUnapproved.AllowUserToDeleteRows = false;
            this.grdUnapproved.AllowUserToResizeColumns = false;
            this.grdUnapproved.AllowUserToResizeRows = false;
            this.grdUnapproved.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdUnapproved.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.uaUpdateName,
            this.uaID,
            this.uaUpdated,
            this.uaDescription,
            this.uaKB,
            this.uaSortOrder});
            this.grdUnapproved.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdUnapproved.Location = new System.Drawing.Point(3, 28);
            this.grdUnapproved.Name = "grdUnapproved";
            this.grdUnapproved.ReadOnly = true;
            this.grdUnapproved.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.grdUnapproved.Size = new System.Drawing.Size(1032, 428);
            this.grdUnapproved.TabIndex = 4;
            this.grdUnapproved.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.grdUnapproved_CellFormatting);
            this.grdUnapproved.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.grdUnapproved_CellMouseClick);
            // 
            // uaUpdateName
            // 
            this.uaUpdateName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.uaUpdateName.FillWeight = 50F;
            this.uaUpdateName.HeaderText = "Update Name";
            this.uaUpdateName.Name = "uaUpdateName";
            this.uaUpdateName.ReadOnly = true;
            this.uaUpdateName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.uaUpdateName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.uaUpdateName.Width = 79;
            // 
            // uaID
            // 
            this.uaID.HeaderText = "Update ID";
            this.uaID.Name = "uaID";
            this.uaID.ReadOnly = true;
            this.uaID.Visible = false;
            // 
            // uaUpdated
            // 
            this.uaUpdated.HeaderText = "Updated";
            this.uaUpdated.Name = "uaUpdated";
            this.uaUpdated.ReadOnly = true;
            this.uaUpdated.Visible = false;
            // 
            // uaDescription
            // 
            this.uaDescription.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.uaDescription.HeaderText = "Description";
            this.uaDescription.Name = "uaDescription";
            this.uaDescription.ReadOnly = true;
            // 
            // uaKB
            // 
            this.uaKB.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.uaKB.HeaderText = "KB Article";
            this.uaKB.Name = "uaKB";
            this.uaKB.ReadOnly = true;
            this.uaKB.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.uaKB.Width = 78;
            // 
            // uaSortOrder
            // 
            this.uaSortOrder.HeaderText = "SortOrder";
            this.uaSortOrder.Name = "uaSortOrder";
            this.uaSortOrder.ReadOnly = true;
            this.uaSortOrder.Visible = false;
            // 
            // tlsUnapproved
            // 
            this.tlsUnapproved.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnUAApprove,
            this.btnUADecline,
            this.btnUACancel,
            this.lblUpdatesToApprove,
            this.mnuHideGroups,
            this.tlsFilterName,
            this.txtFilterName,
            this.lblFilterDescription,
            this.txtFilterDescription,
            this.lblFilterArticle,
            this.txtFilterArticle});
            this.tlsUnapproved.Location = new System.Drawing.Point(3, 3);
            this.tlsUnapproved.Name = "tlsUnapproved";
            this.tlsUnapproved.Size = new System.Drawing.Size(1032, 25);
            this.tlsUnapproved.TabIndex = 5;
            this.tlsUnapproved.Text = "toolStrip1";
            // 
            // btnUAApprove
            // 
            this.btnUAApprove.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnUAApprove.Image = global::WSUSAdminAssistant.Properties.Resources.OK;
            this.btnUAApprove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnUAApprove.Name = "btnUAApprove";
            this.btnUAApprove.Size = new System.Drawing.Size(72, 22);
            this.btnUAApprove.Text = "Approve";
            this.btnUAApprove.Click += new System.EventHandler(this.btnUAApprove_Click);
            // 
            // btnUADecline
            // 
            this.btnUADecline.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnUADecline.Image = global::WSUSAdminAssistant.Properties.Resources.NoAction;
            this.btnUADecline.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnUADecline.Name = "btnUADecline";
            this.btnUADecline.Size = new System.Drawing.Size(66, 22);
            this.btnUADecline.Text = "Decline";
            this.btnUADecline.Click += new System.EventHandler(this.btnUADecline_Click);
            // 
            // btnUACancel
            // 
            this.btnUACancel.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnUACancel.Image = global::WSUSAdminAssistant.Properties.Resources.Critical;
            this.btnUACancel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnUACancel.Name = "btnUACancel";
            this.btnUACancel.Size = new System.Drawing.Size(63, 22);
            this.btnUACancel.Text = "Cancel";
            this.btnUACancel.Visible = false;
            this.btnUACancel.Click += new System.EventHandler(this.btnUACancel_Click);
            // 
            // lblUpdatesToApprove
            // 
            this.lblUpdatesToApprove.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.lblUpdatesToApprove.Name = "lblUpdatesToApprove";
            this.lblUpdatesToApprove.Size = new System.Drawing.Size(0, 22);
            // 
            // mnuHideGroups
            // 
            this.mnuHideGroups.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.mnuHideGroups.Image = ((System.Drawing.Image)(resources.GetObject("mnuHideGroups.Image")));
            this.mnuHideGroups.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mnuHideGroups.Name = "mnuHideGroups";
            this.mnuHideGroups.Size = new System.Drawing.Size(86, 22);
            this.mnuHideGroups.Text = "&Hide Groups";
            // 
            // tlsFilterName
            // 
            this.tlsFilterName.Name = "tlsFilterName";
            this.tlsFilterName.Size = new System.Drawing.Size(87, 22);
            this.tlsFilterName.Text = "Filter by Name:";
            // 
            // txtFilterName
            // 
            this.txtFilterName.Name = "txtFilterName";
            this.txtFilterName.Size = new System.Drawing.Size(100, 25);
            this.txtFilterName.TextChanged += new System.EventHandler(this.txtFilterName_TextChanged);
            // 
            // lblFilterDescription
            // 
            this.lblFilterDescription.Name = "lblFilterDescription";
            this.lblFilterDescription.Size = new System.Drawing.Size(115, 22);
            this.lblFilterDescription.Text = "Filter by Description:";
            // 
            // txtFilterDescription
            // 
            this.txtFilterDescription.Name = "txtFilterDescription";
            this.txtFilterDescription.Size = new System.Drawing.Size(100, 25);
            this.txtFilterDescription.TextChanged += new System.EventHandler(this.txtFilterDescription_TextChanged);
            // 
            // lblFilterArticle
            // 
            this.lblFilterArticle.Name = "lblFilterArticle";
            this.lblFilterArticle.Size = new System.Drawing.Size(106, 22);
            this.lblFilterArticle.Text = "Filter by KB Article:";
            // 
            // txtFilterArticle
            // 
            this.txtFilterArticle.Name = "txtFilterArticle";
            this.txtFilterArticle.Size = new System.Drawing.Size(100, 25);
            // 
            // tabHome
            // 
            this.tabHome.Controls.Add(this.lvwStatus);
            this.tabHome.Controls.Add(this.mnu);
            this.tabHome.Location = new System.Drawing.Point(4, 22);
            this.tabHome.Name = "tabHome";
            this.tabHome.Size = new System.Drawing.Size(1038, 459);
            this.tabHome.TabIndex = 10;
            this.tabHome.Text = "Home";
            this.tabHome.UseVisualStyleBackColor = true;
            // 
            // lvwStatus
            // 
            this.lvwStatus.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lvwStatus.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ssInfo,
            this.ssData,
            this.ssExtraInfo});
            this.lvwStatus.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvwStatus.Location = new System.Drawing.Point(41, 64);
            this.lvwStatus.Name = "lvwStatus";
            this.lvwStatus.Size = new System.Drawing.Size(607, 238);
            this.lvwStatus.TabIndex = 5;
            this.lvwStatus.UseCompatibleStateImageBehavior = false;
            this.lvwStatus.View = System.Windows.Forms.View.Details;
            // 
            // ssInfo
            // 
            this.ssInfo.Text = "";
            this.ssInfo.Width = 25;
            // 
            // ssData
            // 
            this.ssData.Text = "";
            // 
            // mnu
            // 
            this.mnu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuOptions,
            this.mnuUtilities});
            this.mnu.Location = new System.Drawing.Point(0, 0);
            this.mnu.Name = "mnu";
            this.mnu.Size = new System.Drawing.Size(1038, 24);
            this.mnu.TabIndex = 0;
            this.mnu.Text = "menuStrip1";
            // 
            // mnuOptions
            // 
            this.mnuOptions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuWSUSServer,
            this.mnuComputerGroupRules,
            this.mnuIngoreGroups,
            this.mnuGroupApprovalRules,
            this.mnuCredentials,
            this.mnuDefaultSusIDList,
            this.toolStripMenuItem4,
            this.mnuPreferences});
            this.mnuOptions.Name = "mnuOptions";
            this.mnuOptions.Size = new System.Drawing.Size(61, 20);
            this.mnuOptions.Text = "&Options";
            // 
            // mnuWSUSServer
            // 
            this.mnuWSUSServer.Name = "mnuWSUSServer";
            this.mnuWSUSServer.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W)));
            this.mnuWSUSServer.Size = new System.Drawing.Size(288, 22);
            this.mnuWSUSServer.Text = "&WSUS Server";
            this.mnuWSUSServer.Click += new System.EventHandler(this.mnuWSUSServer_Click);
            // 
            // mnuComputerGroupRules
            // 
            this.mnuComputerGroupRules.Name = "mnuComputerGroupRules";
            this.mnuComputerGroupRules.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.mnuComputerGroupRules.Size = new System.Drawing.Size(288, 22);
            this.mnuComputerGroupRules.Text = "Computer &Group Rules";
            this.mnuComputerGroupRules.Click += new System.EventHandler(this.mnuComputerGroupRules_Click);
            // 
            // mnuIngoreGroups
            // 
            this.mnuIngoreGroups.Name = "mnuIngoreGroups";
            this.mnuIngoreGroups.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I)));
            this.mnuIngoreGroups.Size = new System.Drawing.Size(288, 22);
            this.mnuIngoreGroups.Text = "Computer Groups to &Ignore";
            this.mnuIngoreGroups.Click += new System.EventHandler(this.mnuIngoreGroups_Click);
            // 
            // mnuGroupApprovalRules
            // 
            this.mnuGroupApprovalRules.Name = "mnuGroupApprovalRules";
            this.mnuGroupApprovalRules.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.mnuGroupApprovalRules.Size = new System.Drawing.Size(288, 22);
            this.mnuGroupApprovalRules.Text = "Computer Group &Approval Rules";
            this.mnuGroupApprovalRules.Click += new System.EventHandler(this.mnuGroupApprovalRules_Click);
            // 
            // mnuCredentials
            // 
            this.mnuCredentials.Name = "mnuCredentials";
            this.mnuCredentials.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.mnuCredentials.Size = new System.Drawing.Size(288, 22);
            this.mnuCredentials.Text = "Security &Credentials";
            this.mnuCredentials.Click += new System.EventHandler(this.mnuCredentials_Click);
            // 
            // mnuDefaultSusIDList
            // 
            this.mnuDefaultSusIDList.Name = "mnuDefaultSusIDList";
            this.mnuDefaultSusIDList.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this.mnuDefaultSusIDList.Size = new System.Drawing.Size(288, 22);
            this.mnuDefaultSusIDList.Text = "Default &SUS ID List";
            this.mnuDefaultSusIDList.Click += new System.EventHandler(this.mnuDefaultSusIDList_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(285, 6);
            // 
            // mnuPreferences
            // 
            this.mnuPreferences.Name = "mnuPreferences";
            this.mnuPreferences.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this.mnuPreferences.Size = new System.Drawing.Size(288, 22);
            this.mnuPreferences.Text = "&Preferences";
            this.mnuPreferences.Click += new System.EventHandler(this.mnuPreferences_Click);
            // 
            // mnuUtilities
            // 
            this.mnuUtilities.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuSUSWatcher});
            this.mnuUtilities.Name = "mnuUtilities";
            this.mnuUtilities.Size = new System.Drawing.Size(58, 20);
            this.mnuUtilities.Text = "&Utilities";
            // 
            // mnuSUSWatcher
            // 
            this.mnuSUSWatcher.Name = "mnuSUSWatcher";
            this.mnuSUSWatcher.Size = new System.Drawing.Size(208, 22);
            this.mnuSUSWatcher.Text = "Duplicate SUS ID Watcher";
            this.mnuSUSWatcher.Click += new System.EventHandler(this.mnuSUSWatcher_Click);
            // 
            // tabAdminType
            // 
            this.tabAdminType.Controls.Add(this.tabHome);
            this.tabAdminType.Controls.Add(this.tabUnapproved);
            this.tabAdminType.Controls.Add(this.tabEndpointFaults);
            this.tabAdminType.Controls.Add(this.tabWSUSNotCommunicating);
            this.tabAdminType.Controls.Add(this.tabServerRestarts);
            this.tabAdminType.Controls.Add(this.tabSuperceded);
            this.tabAdminType.Controls.Add(this.tabRefresh);
            this.tabAdminType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabAdminType.Location = new System.Drawing.Point(0, 0);
            this.tabAdminType.Name = "tabAdminType";
            this.tabAdminType.SelectedIndex = 0;
            this.tabAdminType.Size = new System.Drawing.Size(1046, 485);
            this.tabAdminType.TabIndex = 0;
            this.tabAdminType.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tabAdminType_Selecting);
            // 
            // timRefreshGrid
            // 
            this.timRefreshGrid.Tick += new System.EventHandler(this.timRefreshGrid_Tick);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1046, 485);
            this.Controls.Add(this.gbxWorking);
            this.Controls.Add(this.tabAdminType);
            this.MainMenuStrip = this.mnu;
            this.Name = "frmMain";
            this.Text = "WSUS Administration Assistant";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_Closing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.Resize += new System.EventHandler(this.frmMain_Resize);
            this.gbxWorking.ResumeLayout(false);
            this.gbxWorking.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picReloading)).EndInit();
            this.cmEndpoint.ResumeLayout(false);
            this.tabSuperceded.ResumeLayout(false);
            this.tabSuperceded.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdSupercededUpdates)).EndInit();
            this.tlsSuperceded.ResumeLayout(false);
            this.tlsSuperceded.PerformLayout();
            this.tabServerRestarts.ResumeLayout(false);
            this.tabWSUSNotCommunicating.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdWSUSNotCommunicting)).EndInit();
            this.tabEndpointFaults.ResumeLayout(false);
            this.splEndpoint.Panel1.ResumeLayout(false);
            this.splEndpoint.Panel1.PerformLayout();
            this.splEndpoint.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splEndpoint)).EndInit();
            this.splEndpoint.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdEndpoints)).EndInit();
            this.tlsEndpoint.ResumeLayout(false);
            this.tlsEndpoint.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdTasks)).EndInit();
            this.tabUnapproved.ResumeLayout(false);
            this.tabUnapproved.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdUnapproved)).EndInit();
            this.tlsUnapproved.ResumeLayout(false);
            this.tlsUnapproved.PerformLayout();
            this.tabHome.ResumeLayout(false);
            this.tabHome.PerformLayout();
            this.mnu.ResumeLayout(false);
            this.mnu.PerformLayout();
            this.tabAdminType.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timUpdateData;
        private System.Windows.Forms.GroupBox gbxWorking;
        private System.Windows.Forms.Label lblReload;
        private System.Windows.Forms.PictureBox picReloading;
        private System.Windows.Forms.ContextMenuStrip cmEndpoint;
        private System.Windows.Forms.ToolStripMenuItem epGPUpdate;
        private System.Windows.Forms.ToolStripMenuItem epGPUpdateForce;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem epResetSusID;
        private System.Windows.Forms.ToolStripMenuItem epDetectNow;
        private System.Windows.Forms.ToolStripMenuItem epReportNow;
        private System.Windows.Forms.ToolStripMenuItem mnuResetAuth;
        private System.Windows.Forms.ToolStripMenuItem epDetails;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem6;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn1;
        private System.Windows.Forms.TabPage tabRefresh;
        private System.Windows.Forms.TabPage tabSuperceded;
        private System.Windows.Forms.DataGridView grdSupercededUpdates;
        private System.Windows.Forms.DataGridViewTextBoxColumn suUpdateName;
        private System.Windows.Forms.DataGridViewTextBoxColumn suUpdateID;
        private System.Windows.Forms.DataGridViewCheckBoxColumn suSelect;
        private System.Windows.Forms.ToolStrip tlsSuperceded;
        private System.Windows.Forms.ToolStripButton lblUpdateCount;
        private System.Windows.Forms.ToolStripButton butDeclineSelected;
        private System.Windows.Forms.ToolStripButton butSelectNone;
        private System.Windows.Forms.ToolStripButton butSelectAll;
        private System.Windows.Forms.TabPage tabServerRestarts;
        private System.Windows.Forms.ListBox lstServers;
        private System.Windows.Forms.TabPage tabWSUSNotCommunicating;
        private System.Windows.Forms.DataGridView grdWSUSNotCommunicting;
        private System.Windows.Forms.DataGridViewTextBoxColumn wnuServerName;
        private System.Windows.Forms.DataGridViewTextBoxColumn wnuLastSync;
        private System.Windows.Forms.DataGridViewTextBoxColumn wnuLastRollup;
        private System.Windows.Forms.TabPage tabEndpointFaults;
        private System.Windows.Forms.SplitContainer splEndpoint;
        private System.Windows.Forms.DataGridView grdEndpoints;
        private System.Windows.Forms.ToolStrip tlsEndpoint;
        private System.Windows.Forms.ToolStripButton butApproved;
        private System.Windows.Forms.ToolStripButton butUpdateErrors;
        private System.Windows.Forms.ToolStripButton butNotCommunicating;
        private System.Windows.Forms.ToolStripButton butUnassigned;
        private System.Windows.Forms.ToolStripButton butDefaultSusID;
        private System.Windows.Forms.ToolStripButton butGroupRules;
        private System.Windows.Forms.DataGridView grdTasks;
        private System.Windows.Forms.DataGridViewTextBoxColumn tskID;
        private System.Windows.Forms.DataGridViewTextBoxColumn tskStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn tskIP;
        private System.Windows.Forms.DataGridViewTextBoxColumn tskCommand;
        private System.Windows.Forms.DataGridViewTextBoxColumn tskOutput;
        private System.Windows.Forms.TabPage tabUnapproved;
        private System.Windows.Forms.DataGridView grdUnapproved;
        private System.Windows.Forms.DataGridViewTextBoxColumn uaUpdateName;
        private System.Windows.Forms.DataGridViewTextBoxColumn uaID;
        private System.Windows.Forms.DataGridViewTextBoxColumn uaUpdated;
        private System.Windows.Forms.DataGridViewTextBoxColumn uaDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn uaKB;
        private System.Windows.Forms.DataGridViewTextBoxColumn uaSortOrder;
        private System.Windows.Forms.ToolStrip tlsUnapproved;
        private System.Windows.Forms.ToolStripButton btnUAApprove;
        private System.Windows.Forms.ToolStripButton btnUADecline;
        private System.Windows.Forms.ToolStripButton btnUACancel;
        private System.Windows.Forms.ToolStripLabel lblUpdatesToApprove;
        private System.Windows.Forms.TabPage tabHome;
        private System.Windows.Forms.MenuStrip mnu;
        private System.Windows.Forms.ToolStripMenuItem mnuOptions;
        private System.Windows.Forms.ToolStripMenuItem mnuWSUSServer;
        private System.Windows.Forms.ToolStripMenuItem mnuComputerGroupRules;
        private System.Windows.Forms.ToolStripMenuItem mnuGroupApprovalRules;
        private System.Windows.Forms.ToolStripMenuItem mnuCredentials;
        private System.Windows.Forms.ToolStripMenuItem mnuDefaultSusIDList;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem mnuPreferences;
        private System.Windows.Forms.ToolStripMenuItem mnuUtilities;
        private System.Windows.Forms.ToolStripMenuItem mnuSUSWatcher;
        private System.Windows.Forms.TabControl tabAdminType;
        private System.Windows.Forms.ToolStripDropDownButton mnuHideGroups;
        private System.Windows.Forms.ToolStripMenuItem mnuIngoreGroups;
        private System.Windows.Forms.ListView lvwStatus;
        private System.Windows.Forms.ColumnHeader ssInfo;
        private System.Windows.Forms.ColumnHeader ssData;
        private System.Windows.Forms.ColumnHeader ssExtraInfo;
        private System.Windows.Forms.ToolStripLabel tlsFilterName;
        private System.Windows.Forms.ToolStripTextBox txtFilterName;
        private System.Windows.Forms.ToolStripLabel lblFilterDescription;
        private System.Windows.Forms.ToolStripTextBox txtFilterDescription;
        private System.Windows.Forms.ToolStripLabel lblFilterArticle;
        private System.Windows.Forms.ToolStripTextBox txtFilterArticle;
        private System.Windows.Forms.Timer timRefreshGrid;
        private System.Windows.Forms.ToolStripButton butDuplicatePCs;
        private System.Windows.Forms.DataGridViewTextBoxColumn epName;
        private System.Windows.Forms.DataGridViewTextBoxColumn epUpdate;
        private System.Windows.Forms.DataGridViewTextBoxColumn epIP;
        private System.Windows.Forms.DataGridViewTextBoxColumn epDownstreamServer;
        private System.Windows.Forms.DataGridViewTextBoxColumn epComputerGroup;
        private System.Windows.Forms.DataGridViewTextBoxColumn epFault;
        private System.Windows.Forms.DataGridViewTextBoxColumn epApprovedUpdates;
        private System.Windows.Forms.DataGridViewTextBoxColumn epUpdateErrors;
        private System.Windows.Forms.DataGridViewTextBoxColumn epLastContact;
        private System.Windows.Forms.DataGridViewTextBoxColumn epLastStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn epPing;
        private System.Windows.Forms.DataGridViewTextBoxColumn epExtraInfo;
        private System.Windows.Forms.DataGridViewTextBoxColumn epSortOrder;
        private System.Windows.Forms.DataGridViewTextBoxColumn epPingUpdated;
    }
}

