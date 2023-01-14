namespace RemoteMsiManager
{
    partial class FrmRemoteMsiManager
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmRemoteMsiManager));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.dgvComputers = new System.Windows.Forms.DataGridView();
            this.Computer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ComputerName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UserName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Total = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Displayed = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvProducts = new System.Windows.Forms.DataGridView();
            this.Product = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.identifyingNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProductName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Version = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.installDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.txtBxPattern = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtBxExceptions = new System.Windows.Forms.TextBox();
            this.ctxMnuProducts = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tlStrCopyMsiProductCode = new System.Windows.Forms.ToolStripMenuItem();
            this.tlStrCopyEntireLine = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tlStrShowDetails = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tlStrSetAsPattern = new System.Windows.Forms.ToolStripMenuItem();
            this.tlStrSetAsException = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tlStrUninstallProducts = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.btnDeleteExceptionFilter = new System.Windows.Forms.Button();
            this.btnDeletePatternFilter = new System.Windows.Forms.Button();
            this.btnRefreshFilter = new System.Windows.Forms.Button();
            this.btnRemoveComputers = new System.Windows.Forms.Button();
            this.btnAddRemoteComputer = new System.Windows.Forms.Button();
            this.btnQueryComputer = new System.Windows.Forms.Button();
            this.btnInstallProduct = new System.Windows.Forms.Button();
            this.btnUninstall = new System.Windows.Forms.Button();
            this.btnQuit = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvComputers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProducts)).BeginInit();
            this.ctxMnuProducts.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dgvComputers);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dgvProducts);
            // 
            // dgvComputers
            // 
            this.dgvComputers.AllowUserToAddRows = false;
            this.dgvComputers.AllowUserToDeleteRows = false;
            this.dgvComputers.AllowUserToOrderColumns = true;
            this.dgvComputers.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.SteelBlue;
            this.dgvComputers.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Khaki;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvComputers.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvComputers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvComputers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Computer,
            this.ComputerName,
            this.UserName,
            this.Total,
            this.Displayed});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvComputers.DefaultCellStyle = dataGridViewCellStyle3;
            resources.ApplyResources(this.dgvComputers, "dgvComputers");
            this.dgvComputers.EnableHeadersVisualStyles = false;
            this.dgvComputers.Name = "dgvComputers";
            this.dgvComputers.ReadOnly = true;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvComputers.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvComputers.RowHeadersVisible = false;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.SteelBlue;
            this.dgvComputers.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvComputers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvComputers.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvComputers_CellDoubleClick);
            this.dgvComputers.SelectionChanged += new System.EventHandler(this.dgvComputers_SelectionChanged);
            // 
            // Computer
            // 
            resources.ApplyResources(this.Computer, "Computer");
            this.Computer.Name = "Computer";
            this.Computer.ReadOnly = true;
            // 
            // ComputerName
            // 
            this.ComputerName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.ComputerName.FillWeight = 50F;
            resources.ApplyResources(this.ComputerName, "ComputerName");
            this.ComputerName.Name = "ComputerName";
            this.ComputerName.ReadOnly = true;
            // 
            // UserName
            // 
            this.UserName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.UserName.FillWeight = 50F;
            resources.ApplyResources(this.UserName, "UserName");
            this.UserName.Name = "UserName";
            this.UserName.ReadOnly = true;
            // 
            // Total
            // 
            this.Total.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            resources.ApplyResources(this.Total, "Total");
            this.Total.Name = "Total";
            this.Total.ReadOnly = true;
            // 
            // Displayed
            // 
            this.Displayed.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            resources.ApplyResources(this.Displayed, "Displayed");
            this.Displayed.Name = "Displayed";
            this.Displayed.ReadOnly = true;
            // 
            // dgvProducts
            // 
            this.dgvProducts.AllowUserToAddRows = false;
            this.dgvProducts.AllowUserToDeleteRows = false;
            this.dgvProducts.AllowUserToOrderColumns = true;
            this.dgvProducts.AllowUserToResizeRows = false;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.SteelBlue;
            this.dgvProducts.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle6;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.Khaki;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvProducts.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.dgvProducts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProducts.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Product,
            this.identifyingNumber,
            this.ProductName,
            this.Version,
            this.installDate});
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvProducts.DefaultCellStyle = dataGridViewCellStyle9;
            resources.ApplyResources(this.dgvProducts, "dgvProducts");
            this.dgvProducts.EnableHeadersVisualStyles = false;
            this.dgvProducts.Name = "dgvProducts";
            this.dgvProducts.ReadOnly = true;
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.Color.SteelBlue;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvProducts.RowHeadersDefaultCellStyle = dataGridViewCellStyle10;
            this.dgvProducts.RowHeadersVisible = false;
            dataGridViewCellStyle11.SelectionBackColor = System.Drawing.Color.SteelBlue;
            this.dgvProducts.RowsDefaultCellStyle = dataGridViewCellStyle11;
            this.dgvProducts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvProducts.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProducts_CellContentDoubleClick);
            this.dgvProducts.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvProducts_CellMouseClick);
            this.dgvProducts.SelectionChanged += new System.EventHandler(this.dgvProducts_SelectionChanged);
            this.dgvProducts.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.dgvProducts_SortCompare);
            // 
            // Product
            // 
            resources.ApplyResources(this.Product, "Product");
            this.Product.Name = "Product";
            this.Product.ReadOnly = true;
            // 
            // identifyingNumber
            // 
            this.identifyingNumber.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.identifyingNumber.DefaultCellStyle = dataGridViewCellStyle8;
            resources.ApplyResources(this.identifyingNumber, "identifyingNumber");
            this.identifyingNumber.Name = "identifyingNumber";
            this.identifyingNumber.ReadOnly = true;
            // 
            // ProductName
            // 
            this.ProductName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            resources.ApplyResources(this.ProductName, "ProductName");
            this.ProductName.Name = "ProductName";
            this.ProductName.ReadOnly = true;
            // 
            // Version
            // 
            this.Version.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            resources.ApplyResources(this.Version, "Version");
            this.Version.Name = "Version";
            this.Version.ReadOnly = true;
            // 
            // installDate
            // 
            this.installDate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            resources.ApplyResources(this.installDate, "installDate");
            this.installDate.Name = "installDate";
            this.installDate.ReadOnly = true;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // txtBxPattern
            // 
            resources.ApplyResources(this.txtBxPattern, "txtBxPattern");
            this.txtBxPattern.Name = "txtBxPattern";
            this.txtBxPattern.TextChanged += new System.EventHandler(this.txtBxPattern_TextChanged);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // txtBxExceptions
            // 
            resources.ApplyResources(this.txtBxExceptions, "txtBxExceptions");
            this.txtBxExceptions.BackColor = System.Drawing.SystemColors.Window;
            this.txtBxExceptions.Name = "txtBxExceptions";
            this.txtBxExceptions.TextChanged += new System.EventHandler(this.txtBxExceptions_TextChanged);
            // 
            // ctxMnuProducts
            // 
            this.ctxMnuProducts.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tlStrCopyMsiProductCode,
            this.tlStrCopyEntireLine,
            this.toolStripSeparator1,
            this.tlStrShowDetails,
            this.toolStripSeparator3,
            this.tlStrSetAsPattern,
            this.tlStrSetAsException,
            this.toolStripSeparator2,
            this.tlStrUninstallProducts,
            this.toolStripSeparator4});
            this.ctxMnuProducts.Name = "ctxMnuProducts";
            resources.ApplyResources(this.ctxMnuProducts, "ctxMnuProducts");
            // 
            // tlStrCopyMsiProductCode
            // 
            this.tlStrCopyMsiProductCode.Name = "tlStrCopyMsiProductCode";
            resources.ApplyResources(this.tlStrCopyMsiProductCode, "tlStrCopyMsiProductCode");
            this.tlStrCopyMsiProductCode.Click += new System.EventHandler(this.tlStrCopyMsiProductCode_Click);
            // 
            // tlStrCopyEntireLine
            // 
            this.tlStrCopyEntireLine.Name = "tlStrCopyEntireLine";
            resources.ApplyResources(this.tlStrCopyEntireLine, "tlStrCopyEntireLine");
            this.tlStrCopyEntireLine.Click += new System.EventHandler(this.tlStrCopyEntireLine_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // tlStrShowDetails
            // 
            this.tlStrShowDetails.Image = global::RemoteMsiManager.Properties.Resources.Eye16x16;
            this.tlStrShowDetails.Name = "tlStrShowDetails";
            resources.ApplyResources(this.tlStrShowDetails, "tlStrShowDetails");
            this.tlStrShowDetails.Click += new System.EventHandler(this.tlStrShowDetails_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            // 
            // tlStrSetAsPattern
            // 
            this.tlStrSetAsPattern.Image = global::RemoteMsiManager.Properties.Resources.Add16x16;
            this.tlStrSetAsPattern.Name = "tlStrSetAsPattern";
            resources.ApplyResources(this.tlStrSetAsPattern, "tlStrSetAsPattern");
            this.tlStrSetAsPattern.Click += new System.EventHandler(this.tlStrSetAsPattern_Click);
            // 
            // tlStrSetAsException
            // 
            this.tlStrSetAsException.Image = global::RemoteMsiManager.Properties.Resources.Minus16x16;
            this.tlStrSetAsException.Name = "tlStrSetAsException";
            resources.ApplyResources(this.tlStrSetAsException, "tlStrSetAsException");
            this.tlStrSetAsException.Click += new System.EventHandler(this.tlStrSetAsException_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            // 
            // tlStrUninstallProducts
            // 
            this.tlStrUninstallProducts.Image = global::RemoteMsiManager.Properties.Resources.Uninstall24x24;
            this.tlStrUninstallProducts.Name = "tlStrUninstallProducts";
            resources.ApplyResources(this.tlStrUninstallProducts, "tlStrUninstallProducts");
            this.tlStrUninstallProducts.Click += new System.EventHandler(this.tlStrUninstallProducts_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            resources.ApplyResources(this.toolStripSeparator4, "toolStripSeparator4");
            // 
            // btnDeleteExceptionFilter
            // 
            resources.ApplyResources(this.btnDeleteExceptionFilter, "btnDeleteExceptionFilter");
            this.btnDeleteExceptionFilter.Image = global::RemoteMsiManager.Properties.Resources.Delete16x16;
            this.btnDeleteExceptionFilter.Name = "btnDeleteExceptionFilter";
            this.btnDeleteExceptionFilter.UseVisualStyleBackColor = true;
            this.btnDeleteExceptionFilter.Click += new System.EventHandler(this.btnDeleteExceptionFilter_Click);
            // 
            // btnDeletePatternFilter
            // 
            resources.ApplyResources(this.btnDeletePatternFilter, "btnDeletePatternFilter");
            this.btnDeletePatternFilter.Image = global::RemoteMsiManager.Properties.Resources.Delete16x16;
            this.btnDeletePatternFilter.Name = "btnDeletePatternFilter";
            this.btnDeletePatternFilter.UseVisualStyleBackColor = true;
            this.btnDeletePatternFilter.Click += new System.EventHandler(this.btnDeletePatternFilter_Click);
            // 
            // btnRefreshFilter
            // 
            resources.ApplyResources(this.btnRefreshFilter, "btnRefreshFilter");
            this.btnRefreshFilter.Image = global::RemoteMsiManager.Properties.Resources.Refresh48x48;
            this.btnRefreshFilter.Name = "btnRefreshFilter";
            this.btnRefreshFilter.UseVisualStyleBackColor = true;
            this.btnRefreshFilter.Click += new System.EventHandler(this.btnRefreshFilter_Click);
            // 
            // btnRemoveComputers
            // 
            resources.ApplyResources(this.btnRemoveComputers, "btnRemoveComputers");
            this.btnRemoveComputers.Image = global::RemoteMsiManager.Properties.Resources.Remove24x24;
            this.btnRemoveComputers.Name = "btnRemoveComputers";
            this.btnRemoveComputers.UseVisualStyleBackColor = true;
            this.btnRemoveComputers.Click += new System.EventHandler(this.btnRemoveComputers_Click);
            // 
            // btnAddRemoteComputer
            // 
            resources.ApplyResources(this.btnAddRemoteComputer, "btnAddRemoteComputer");
            this.btnAddRemoteComputer.Image = global::RemoteMsiManager.Properties.Resources.Add24x24;
            this.btnAddRemoteComputer.Name = "btnAddRemoteComputer";
            this.btnAddRemoteComputer.UseVisualStyleBackColor = true;
            this.btnAddRemoteComputer.Click += new System.EventHandler(this.btnAddRemoteComputer_Click);
            // 
            // btnQueryComputer
            // 
            resources.ApplyResources(this.btnQueryComputer, "btnQueryComputer");
            this.btnQueryComputer.Image = global::RemoteMsiManager.Properties.Resources.Search24x24;
            this.btnQueryComputer.Name = "btnQueryComputer";
            this.btnQueryComputer.UseVisualStyleBackColor = true;
            this.btnQueryComputer.Click += new System.EventHandler(this.btnQueryComputer_Click);
            // 
            // btnInstallProduct
            // 
            resources.ApplyResources(this.btnInstallProduct, "btnInstallProduct");
            this.btnInstallProduct.Image = global::RemoteMsiManager.Properties.Resources.Install24x24;
            this.btnInstallProduct.Name = "btnInstallProduct";
            this.btnInstallProduct.UseVisualStyleBackColor = true;
            this.btnInstallProduct.Click += new System.EventHandler(this.btnInstall_Click);
            // 
            // btnUninstall
            // 
            resources.ApplyResources(this.btnUninstall, "btnUninstall");
            this.btnUninstall.Image = global::RemoteMsiManager.Properties.Resources.Uninstall24x24;
            this.btnUninstall.Name = "btnUninstall";
            this.btnUninstall.UseVisualStyleBackColor = true;
            this.btnUninstall.Click += new System.EventHandler(this.btnUninstall_Click);
            // 
            // btnQuit
            // 
            resources.ApplyResources(this.btnQuit, "btnQuit");
            this.btnQuit.Image = global::RemoteMsiManager.Properties.Resources.Exit24x24;
            this.btnQuit.Name = "btnQuit";
            this.btnQuit.UseVisualStyleBackColor = true;
            this.btnQuit.Click += new System.EventHandler(this.btnQuit_Click);
            // 
            // FrmRemoteMsiManager
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnDeleteExceptionFilter);
            this.Controls.Add(this.btnDeletePatternFilter);
            this.Controls.Add(this.btnRefreshFilter);
            this.Controls.Add(this.btnRemoveComputers);
            this.Controls.Add(this.txtBxExceptions);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtBxPattern);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnAddRemoteComputer);
            this.Controls.Add(this.btnQueryComputer);
            this.Controls.Add(this.btnInstallProduct);
            this.Controls.Add(this.btnUninstall);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.btnQuit);
            this.KeyPreview = true;
            this.Name = "FrmRemoteMsiManager";
            this.Shown += new System.EventHandler(this.FrmMain_Shown);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvComputers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProducts)).EndInit();
            this.ctxMnuProducts.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnQuit;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView dgvComputers;
        private System.Windows.Forms.DataGridView dgvProducts;
        private System.Windows.Forms.Button btnUninstall;
        private System.Windows.Forms.Button btnQueryComputer;
        private System.Windows.Forms.Button btnAddRemoteComputer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtBxPattern;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtBxExceptions;
        private System.Windows.Forms.Button btnRemoveComputers;
        private System.Windows.Forms.Button btnInstallProduct;
        private System.Windows.Forms.Button btnRefreshFilter;
        private System.Windows.Forms.Button btnDeletePatternFilter;
        private System.Windows.Forms.Button btnDeleteExceptionFilter;
        private System.Windows.Forms.ContextMenuStrip ctxMnuProducts;
        private System.Windows.Forms.ToolStripMenuItem tlStrCopyMsiProductCode;
        private System.Windows.Forms.ToolStripMenuItem tlStrCopyEntireLine;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem tlStrUninstallProducts;
        private System.Windows.Forms.ToolStripMenuItem tlStrShowDetails;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem tlStrSetAsException;
        private System.Windows.Forms.ToolStripMenuItem tlStrSetAsPattern;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Computer;
        private System.Windows.Forms.DataGridViewTextBoxColumn ComputerName;
        private System.Windows.Forms.DataGridViewTextBoxColumn UserName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Total;
        private System.Windows.Forms.DataGridViewTextBoxColumn Displayed;
        private System.Windows.Forms.DataGridViewTextBoxColumn Product;
        private System.Windows.Forms.DataGridViewTextBoxColumn identifyingNumber;
        private new System.Windows.Forms.DataGridViewTextBoxColumn ProductName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Version;
        private System.Windows.Forms.DataGridViewTextBoxColumn installDate;
    }
}

