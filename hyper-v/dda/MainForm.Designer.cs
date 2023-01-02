namespace DiscreteDeviceAssigner
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeaderDevice = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderClass = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderLocation = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.其它toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lowMemoryMappedIoSpaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LMMIOtoolStripTextBox = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.highMemoryMappedIoSpaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.HMMIOtoolStripTextBox = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.GCCTtoolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.添加设备ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.移除设备ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.复制地址toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.刷新列表toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderDevice,
            this.columnHeaderClass,
            this.columnHeaderLocation});
            this.listView1.FullRowSelect = true;
            this.listView1.HideSelection = false;
            resources.ApplyResources(this.listView1, "listView1");
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            this.listView1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseClick);
            // 
            // columnHeaderDevice
            // 
            resources.ApplyResources(this.columnHeaderDevice, "columnHeaderDevice");
            // 
            // columnHeaderClass
            // 
            resources.ApplyResources(this.columnHeaderClass, "columnHeaderClass");
            // 
            // columnHeaderLocation
            // 
            resources.ApplyResources(this.columnHeaderLocation, "columnHeaderLocation");
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.其它toolStripMenuItem,
            this.toolStripSeparator2,
            this.添加设备ToolStripMenuItem,
            this.移除设备ToolStripMenuItem,
            this.复制地址toolStripMenuItem,
            this.toolStripSeparator1,
            this.刷新列表toolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip";
            resources.ApplyResources(this.contextMenuStrip, "contextMenuStrip");
            this.contextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip_Opening);
            // 
            // 其它toolStripMenuItem
            // 
            this.其它toolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lowMemoryMappedIoSpaceToolStripMenuItem,
            this.LMMIOtoolStripTextBox,
            this.toolStripSeparator4,
            this.highMemoryMappedIoSpaceToolStripMenuItem,
            this.HMMIOtoolStripTextBox,
            this.toolStripSeparator5,
            this.GCCTtoolStripMenuItem});
            this.其它toolStripMenuItem.Name = "其它toolStripMenuItem";
            resources.ApplyResources(this.其它toolStripMenuItem, "其它toolStripMenuItem");
            this.其它toolStripMenuItem.Click += new System.EventHandler(this.其它toolStripMenuItem_Click);
            // 
            // lowMemoryMappedIoSpaceToolStripMenuItem
            // 
            resources.ApplyResources(this.lowMemoryMappedIoSpaceToolStripMenuItem, "lowMemoryMappedIoSpaceToolStripMenuItem");
            this.lowMemoryMappedIoSpaceToolStripMenuItem.Name = "lowMemoryMappedIoSpaceToolStripMenuItem";
            // 
            // LMMIOtoolStripTextBox
            // 
            resources.ApplyResources(this.LMMIOtoolStripTextBox, "LMMIOtoolStripTextBox");
            this.LMMIOtoolStripTextBox.Name = "LMMIOtoolStripTextBox";
            this.LMMIOtoolStripTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.LMMIOtoolStripTextBox_KeyDown);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            resources.ApplyResources(this.toolStripSeparator4, "toolStripSeparator4");
            // 
            // highMemoryMappedIoSpaceToolStripMenuItem
            // 
            resources.ApplyResources(this.highMemoryMappedIoSpaceToolStripMenuItem, "highMemoryMappedIoSpaceToolStripMenuItem");
            this.highMemoryMappedIoSpaceToolStripMenuItem.Name = "highMemoryMappedIoSpaceToolStripMenuItem";
            this.highMemoryMappedIoSpaceToolStripMenuItem.Click += new System.EventHandler(this.highMemoryMappedIoSpaceToolStripMenuItem_Click);
            // 
            // HMMIOtoolStripTextBox
            // 
            resources.ApplyResources(this.HMMIOtoolStripTextBox, "HMMIOtoolStripTextBox");
            this.HMMIOtoolStripTextBox.Name = "HMMIOtoolStripTextBox";
            this.HMMIOtoolStripTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.HMMIOtoolStripTextBox_KeyDown);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            resources.ApplyResources(this.toolStripSeparator5, "toolStripSeparator5");
            // 
            // GCCTtoolStripMenuItem
            // 
            this.GCCTtoolStripMenuItem.Checked = true;
            this.GCCTtoolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.GCCTtoolStripMenuItem.Name = "GCCTtoolStripMenuItem";
            resources.ApplyResources(this.GCCTtoolStripMenuItem, "GCCTtoolStripMenuItem");
            this.GCCTtoolStripMenuItem.Click += new System.EventHandler(this.GCCTtoolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            // 
            // 添加设备ToolStripMenuItem
            // 
            this.添加设备ToolStripMenuItem.Name = "添加设备ToolStripMenuItem";
            resources.ApplyResources(this.添加设备ToolStripMenuItem, "添加设备ToolStripMenuItem");
            this.添加设备ToolStripMenuItem.Click += new System.EventHandler(this.添加设备ToolStripMenuItem_Click);
            // 
            // 移除设备ToolStripMenuItem
            // 
            this.移除设备ToolStripMenuItem.Name = "移除设备ToolStripMenuItem";
            resources.ApplyResources(this.移除设备ToolStripMenuItem, "移除设备ToolStripMenuItem");
            this.移除设备ToolStripMenuItem.Click += new System.EventHandler(this.移除设备ToolStripMenuItem_Click);
            // 
            // 复制地址toolStripMenuItem
            // 
            this.复制地址toolStripMenuItem.Name = "复制地址toolStripMenuItem";
            resources.ApplyResources(this.复制地址toolStripMenuItem, "复制地址toolStripMenuItem");
            this.复制地址toolStripMenuItem.Click += new System.EventHandler(this.复制地址ToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // 刷新列表toolStripMenuItem
            // 
            this.刷新列表toolStripMenuItem.Name = "刷新列表toolStripMenuItem";
            resources.ApplyResources(this.刷新列表toolStripMenuItem, "刷新列表toolStripMenuItem");
            this.刷新列表toolStripMenuItem.Click += new System.EventHandler(this.刷新列表ToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AllowDrop = true;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.Controls.Add(this.listView1);
            this.HelpButton = true;
            this.KeyPreview = true;
            this.Name = "MainForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeaderDevice;
        private System.Windows.Forms.ColumnHeader columnHeaderLocation;
        private System.Windows.Forms.ColumnHeader columnHeaderClass;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem 添加设备ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 移除设备ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem 刷新列表toolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem 复制地址toolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 其它toolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lowMemoryMappedIoSpaceToolStripMenuItem;
        private System.Windows.Forms.ToolStripTextBox LMMIOtoolStripTextBox;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem highMemoryMappedIoSpaceToolStripMenuItem;
        private System.Windows.Forms.ToolStripTextBox HMMIOtoolStripTextBox;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem GCCTtoolStripMenuItem;
    }
}

