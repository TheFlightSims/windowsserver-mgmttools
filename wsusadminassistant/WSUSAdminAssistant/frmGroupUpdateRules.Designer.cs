namespace WSUSAdminAssistant
{
    partial class frmGroupUpdateRules
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
            this.spl = new System.Windows.Forms.SplitContainer();
            this.splLeft = new System.Windows.Forms.SplitContainer();
            this.txtShortName = new System.Windows.Forms.TextBox();
            this.lblShortName = new System.Windows.Forms.Label();
            this.numSortWeight = new System.Windows.Forms.NumericUpDown();
            this.lblSortWeight = new System.Windows.Forms.Label();
            this.numDisplayOrder = new System.Windows.Forms.NumericUpDown();
            this.lblDisplayOrder = new System.Windows.Forms.Label();
            this.lblComputerGroup = new System.Windows.Forms.Label();
            this.cboComputerGroup = new System.Windows.Forms.ComboBox();
            this.lblParentGroup = new System.Windows.Forms.Label();
            this.lblParentInterval3 = new System.Windows.Forms.Label();
            this.cboParentGroup = new System.Windows.Forms.ComboBox();
            this.cboNoApprovalInterval = new System.Windows.Forms.ComboBox();
            this.lblParentInterval1 = new System.Windows.Forms.Label();
            this.numNoApprovalInterval = new System.Windows.Forms.NumericUpDown();
            this.numParentInterval = new System.Windows.Forms.NumericUpDown();
            this.lblParentInterval2 = new System.Windows.Forms.Label();
            this.cboParentInterval = new System.Windows.Forms.ComboBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.trvGroupRules = new System.Windows.Forms.TreeView();
            ((System.ComponentModel.ISupportInitialize)(this.spl)).BeginInit();
            this.spl.Panel1.SuspendLayout();
            this.spl.Panel2.SuspendLayout();
            this.spl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splLeft)).BeginInit();
            this.splLeft.Panel1.SuspendLayout();
            this.splLeft.Panel2.SuspendLayout();
            this.splLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSortWeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDisplayOrder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numNoApprovalInterval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numParentInterval)).BeginInit();
            this.SuspendLayout();
            // 
            // spl
            // 
            this.spl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spl.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.spl.IsSplitterFixed = true;
            this.spl.Location = new System.Drawing.Point(0, 0);
            this.spl.Name = "spl";
            // 
            // spl.Panel1
            // 
            this.spl.Panel1.Controls.Add(this.splLeft);
            // 
            // spl.Panel2
            // 
            this.spl.Panel2.Controls.Add(this.trvGroupRules);
            this.spl.Size = new System.Drawing.Size(545, 405);
            this.spl.SplitterDistance = 197;
            this.spl.TabIndex = 0;
            // 
            // splLeft
            // 
            this.splLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splLeft.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splLeft.IsSplitterFixed = true;
            this.splLeft.Location = new System.Drawing.Point(0, 0);
            this.splLeft.Name = "splLeft";
            this.splLeft.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splLeft.Panel1
            // 
            this.splLeft.Panel1.Controls.Add(this.txtShortName);
            this.splLeft.Panel1.Controls.Add(this.lblShortName);
            this.splLeft.Panel1.Controls.Add(this.numSortWeight);
            this.splLeft.Panel1.Controls.Add(this.lblSortWeight);
            this.splLeft.Panel1.Controls.Add(this.numDisplayOrder);
            this.splLeft.Panel1.Controls.Add(this.lblDisplayOrder);
            this.splLeft.Panel1.Controls.Add(this.lblComputerGroup);
            this.splLeft.Panel1.Controls.Add(this.cboComputerGroup);
            this.splLeft.Panel1.Controls.Add(this.lblParentGroup);
            this.splLeft.Panel1.Controls.Add(this.lblParentInterval3);
            this.splLeft.Panel1.Controls.Add(this.cboParentGroup);
            this.splLeft.Panel1.Controls.Add(this.cboNoApprovalInterval);
            this.splLeft.Panel1.Controls.Add(this.lblParentInterval1);
            this.splLeft.Panel1.Controls.Add(this.numNoApprovalInterval);
            this.splLeft.Panel1.Controls.Add(this.numParentInterval);
            this.splLeft.Panel1.Controls.Add(this.lblParentInterval2);
            this.splLeft.Panel1.Controls.Add(this.cboParentInterval);
            this.splLeft.Panel1MinSize = 295;
            // 
            // splLeft.Panel2
            // 
            this.splLeft.Panel2.Controls.Add(this.btnAdd);
            this.splLeft.Panel2.Controls.Add(this.btnSave);
            this.splLeft.Panel2.Controls.Add(this.btnRemove);
            this.splLeft.Panel2.Controls.Add(this.btnEdit);
            this.splLeft.Panel2MinSize = 58;
            this.splLeft.Size = new System.Drawing.Size(197, 405);
            this.splLeft.SplitterDistance = 341;
            this.splLeft.TabIndex = 17;
            // 
            // txtShortName
            // 
            this.txtShortName.Location = new System.Drawing.Point(13, 123);
            this.txtShortName.Name = "txtShortName";
            this.txtShortName.Size = new System.Drawing.Size(176, 20);
            this.txtShortName.TabIndex = 7;
            this.txtShortName.TextChanged += new System.EventHandler(this.txtShortName_TextChanged);
            // 
            // lblShortName
            // 
            this.lblShortName.AutoSize = true;
            this.lblShortName.Location = new System.Drawing.Point(10, 107);
            this.lblShortName.Name = "lblShortName";
            this.lblShortName.Size = new System.Drawing.Size(63, 13);
            this.lblShortName.TabIndex = 6;
            this.lblShortName.Text = "Short Name";
            // 
            // numSortWeight
            // 
            this.numSortWeight.Location = new System.Drawing.Point(108, 26);
            this.numSortWeight.Maximum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            0});
            this.numSortWeight.Name = "numSortWeight";
            this.numSortWeight.Size = new System.Drawing.Size(81, 20);
            this.numSortWeight.TabIndex = 3;
            this.numSortWeight.ValueChanged += new System.EventHandler(this.numSortWeight_ValueChanged);
            // 
            // lblSortWeight
            // 
            this.lblSortWeight.AutoSize = true;
            this.lblSortWeight.Location = new System.Drawing.Point(105, 10);
            this.lblSortWeight.Name = "lblSortWeight";
            this.lblSortWeight.Size = new System.Drawing.Size(63, 13);
            this.lblSortWeight.TabIndex = 2;
            this.lblSortWeight.Text = "Sort Weight";
            // 
            // numDisplayOrder
            // 
            this.numDisplayOrder.Location = new System.Drawing.Point(13, 26);
            this.numDisplayOrder.Maximum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            0});
            this.numDisplayOrder.Name = "numDisplayOrder";
            this.numDisplayOrder.Size = new System.Drawing.Size(81, 20);
            this.numDisplayOrder.TabIndex = 1;
            this.numDisplayOrder.ValueChanged += new System.EventHandler(this.numDisplayOrder_ValueChanged);
            // 
            // lblDisplayOrder
            // 
            this.lblDisplayOrder.AutoSize = true;
            this.lblDisplayOrder.Location = new System.Drawing.Point(10, 10);
            this.lblDisplayOrder.Name = "lblDisplayOrder";
            this.lblDisplayOrder.Size = new System.Drawing.Size(70, 13);
            this.lblDisplayOrder.TabIndex = 0;
            this.lblDisplayOrder.Text = "Display Order";
            // 
            // lblComputerGroup
            // 
            this.lblComputerGroup.AutoSize = true;
            this.lblComputerGroup.Location = new System.Drawing.Point(10, 61);
            this.lblComputerGroup.Name = "lblComputerGroup";
            this.lblComputerGroup.Size = new System.Drawing.Size(84, 13);
            this.lblComputerGroup.TabIndex = 4;
            this.lblComputerGroup.Text = "Computer Group";
            // 
            // cboComputerGroup
            // 
            this.cboComputerGroup.FormattingEnabled = true;
            this.cboComputerGroup.Location = new System.Drawing.Point(13, 78);
            this.cboComputerGroup.Name = "cboComputerGroup";
            this.cboComputerGroup.Size = new System.Drawing.Size(176, 21);
            this.cboComputerGroup.TabIndex = 5;
            this.cboComputerGroup.SelectedIndexChanged += new System.EventHandler(this.cboComputerGroup_SelectedIndexChanged);
            this.cboComputerGroup.EnabledChanged += new System.EventHandler(this.cboComputerGroup_EnabledChanged);
            // 
            // lblParentGroup
            // 
            this.lblParentGroup.AutoSize = true;
            this.lblParentGroup.Location = new System.Drawing.Point(10, 156);
            this.lblParentGroup.Name = "lblParentGroup";
            this.lblParentGroup.Size = new System.Drawing.Size(118, 13);
            this.lblParentGroup.TabIndex = 8;
            this.lblParentGroup.Text = "Parent Computer Group";
            // 
            // lblParentInterval3
            // 
            this.lblParentInterval3.AutoSize = true;
            this.lblParentInterval3.Location = new System.Drawing.Point(12, 288);
            this.lblParentInterval3.MaximumSize = new System.Drawing.Size(176, 0);
            this.lblParentInterval3.Name = "lblParentInterval3";
            this.lblParentInterval3.Size = new System.Drawing.Size(174, 39);
            this.lblParentInterval3.TabIndex = 16;
            this.lblParentInterval3.Text = "...after update became available for parent when no computers in the parent group" +
    " require the update";
            // 
            // cboParentGroup
            // 
            this.cboParentGroup.FormattingEnabled = true;
            this.cboParentGroup.Location = new System.Drawing.Point(13, 172);
            this.cboParentGroup.Name = "cboParentGroup";
            this.cboParentGroup.Size = new System.Drawing.Size(176, 21);
            this.cboParentGroup.TabIndex = 9;
            this.cboParentGroup.SelectedIndexChanged += new System.EventHandler(this.cboParentGroup_SelectedIndexChanged);
            // 
            // cboNoApprovalInterval
            // 
            this.cboNoApprovalInterval.FormattingEnabled = true;
            this.cboNoApprovalInterval.Items.AddRange(new object[] {
            "Minutes",
            "Hours",
            "Days",
            "Weeks"});
            this.cboNoApprovalInterval.Location = new System.Drawing.Point(104, 264);
            this.cboNoApprovalInterval.Name = "cboNoApprovalInterval";
            this.cboNoApprovalInterval.Size = new System.Drawing.Size(85, 21);
            this.cboNoApprovalInterval.TabIndex = 15;
            this.cboNoApprovalInterval.SelectedIndexChanged += new System.EventHandler(this.cboNoApprovalInterval_SelectedIndexChanged);
            // 
            // lblParentInterval1
            // 
            this.lblParentInterval1.AutoSize = true;
            this.lblParentInterval1.Location = new System.Drawing.Point(10, 210);
            this.lblParentInterval1.Name = "lblParentInterval1";
            this.lblParentInterval1.Size = new System.Drawing.Size(96, 13);
            this.lblParentInterval1.TabIndex = 10;
            this.lblParentInterval1.Text = "Updates Allowed...";
            // 
            // numNoApprovalInterval
            // 
            this.numNoApprovalInterval.Location = new System.Drawing.Point(13, 266);
            this.numNoApprovalInterval.Maximum = new decimal(new int[] {
            120,
            0,
            0,
            0});
            this.numNoApprovalInterval.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numNoApprovalInterval.Name = "numNoApprovalInterval";
            this.numNoApprovalInterval.Size = new System.Drawing.Size(84, 20);
            this.numNoApprovalInterval.TabIndex = 14;
            this.numNoApprovalInterval.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numNoApprovalInterval.ValueChanged += new System.EventHandler(this.numNoApprovalInterval_ValueChanged);
            // 
            // numParentInterval
            // 
            this.numParentInterval.Location = new System.Drawing.Point(13, 226);
            this.numParentInterval.Maximum = new decimal(new int[] {
            120,
            0,
            0,
            0});
            this.numParentInterval.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numParentInterval.Name = "numParentInterval";
            this.numParentInterval.Size = new System.Drawing.Size(84, 20);
            this.numParentInterval.TabIndex = 11;
            this.numParentInterval.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numParentInterval.ValueChanged += new System.EventHandler(this.numParentInterval_ValueChanged);
            // 
            // lblParentInterval2
            // 
            this.lblParentInterval2.AutoSize = true;
            this.lblParentInterval2.Location = new System.Drawing.Point(10, 250);
            this.lblParentInterval2.MaximumSize = new System.Drawing.Size(176, 0);
            this.lblParentInterval2.Name = "lblParentInterval2";
            this.lblParentInterval2.Size = new System.Drawing.Size(150, 13);
            this.lblParentInterval2.TabIndex = 13;
            this.lblParentInterval2.Text = "...after approval for parent or...";
            this.lblParentInterval2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cboParentInterval
            // 
            this.cboParentInterval.FormattingEnabled = true;
            this.cboParentInterval.Items.AddRange(new object[] {
            "Minutes",
            "Hours",
            "Days",
            "Weeks"});
            this.cboParentInterval.Location = new System.Drawing.Point(104, 225);
            this.cboParentInterval.Name = "cboParentInterval";
            this.cboParentInterval.Size = new System.Drawing.Size(85, 21);
            this.cboParentInterval.TabIndex = 12;
            this.cboParentInterval.SelectedIndexChanged += new System.EventHandler(this.cboParentInterval_SelectedIndexChanged);
            // 
            // btnAdd
            // 
            this.btnAdd.Enabled = false;
            this.btnAdd.Location = new System.Drawing.Point(13, 3);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 0;
            this.btnAdd.Text = "&Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(114, 32);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Enabled = false;
            this.btnRemove.Location = new System.Drawing.Point(114, 3);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(75, 23);
            this.btnRemove.TabIndex = 2;
            this.btnRemove.Text = "&Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Enabled = false;
            this.btnEdit.Location = new System.Drawing.Point(13, 32);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(75, 23);
            this.btnEdit.TabIndex = 1;
            this.btnEdit.Text = "&Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // trvGroupRules
            // 
            this.trvGroupRules.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trvGroupRules.Location = new System.Drawing.Point(0, 0);
            this.trvGroupRules.Name = "trvGroupRules";
            this.trvGroupRules.Size = new System.Drawing.Size(344, 405);
            this.trvGroupRules.TabIndex = 0;
            this.trvGroupRules.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.trvGroupRules_AfterSelect);
            this.trvGroupRules.DoubleClick += new System.EventHandler(this.trvGroupRules_DoubleClick);
            // 
            // frmGroupUpdateRules
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(545, 405);
            this.Controls.Add(this.spl);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(560, 406);
            this.Name = "frmGroupUpdateRules";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Computer Group Update Approval Rules";
            this.Resize += new System.EventHandler(this.frmGroupUpdateRules_Resize);
            this.spl.Panel1.ResumeLayout(false);
            this.spl.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.spl)).EndInit();
            this.spl.ResumeLayout(false);
            this.splLeft.Panel1.ResumeLayout(false);
            this.splLeft.Panel1.PerformLayout();
            this.splLeft.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splLeft)).EndInit();
            this.splLeft.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numSortWeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDisplayOrder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numNoApprovalInterval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numParentInterval)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer spl;
        private System.Windows.Forms.TreeView trvGroupRules;
        private System.Windows.Forms.ComboBox cboComputerGroup;
        private System.Windows.Forms.Label lblComputerGroup;
        private System.Windows.Forms.NumericUpDown numDisplayOrder;
        private System.Windows.Forms.Label lblDisplayOrder;
        private System.Windows.Forms.Label lblParentGroup;
        private System.Windows.Forms.ComboBox cboParentGroup;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Label lblParentInterval3;
        private System.Windows.Forms.ComboBox cboNoApprovalInterval;
        private System.Windows.Forms.NumericUpDown numNoApprovalInterval;
        private System.Windows.Forms.Label lblParentInterval2;
        private System.Windows.Forms.ComboBox cboParentInterval;
        private System.Windows.Forms.NumericUpDown numParentInterval;
        private System.Windows.Forms.Label lblParentInterval1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.SplitContainer splLeft;
        private System.Windows.Forms.NumericUpDown numSortWeight;
        private System.Windows.Forms.Label lblSortWeight;
        private System.Windows.Forms.TextBox txtShortName;
        private System.Windows.Forms.Label lblShortName;
    }
}