namespace WSUSAdminAssistant
{
    partial class frmCredentials
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
            this.tls = new System.Windows.Forms.ToolStrip();
            this.btnSave = new System.Windows.Forms.ToolStripButton();
            this.grdCredentials = new System.Windows.Forms.DataGridView();
            this.crNetwork = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.crNetmask = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.crDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.crDomain = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.crUser = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.crPassword = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdCredentials)).BeginInit();
            this.SuspendLayout();
            // 
            // tls
            // 
            this.tls.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnSave});
            this.tls.Location = new System.Drawing.Point(0, 0);
            this.tls.Name = "tls";
            this.tls.Size = new System.Drawing.Size(843, 25);
            this.tls.TabIndex = 0;
            this.tls.Text = "toolStrip1";
            // 
            // btnSave
            // 
            this.btnSave.Image = global::WSUSAdminAssistant.Properties.Resources.Save;
            this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(51, 22);
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // grdCredentials
            // 
            this.grdCredentials.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdCredentials.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.crNetwork,
            this.crNetmask,
            this.crDescription,
            this.crDomain,
            this.crUser,
            this.crPassword});
            this.grdCredentials.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdCredentials.Location = new System.Drawing.Point(0, 25);
            this.grdCredentials.Name = "grdCredentials";
            this.grdCredentials.Size = new System.Drawing.Size(843, 407);
            this.grdCredentials.TabIndex = 1;
            this.grdCredentials.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.grdCredentials_CellFormatting);
            this.grdCredentials.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.grdCredentials_ColumnHeaderMouseClick);
            this.grdCredentials.RowValidating += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.grdCredentials_RowValidating);
            // 
            // crNetwork
            // 
            this.crNetwork.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.crNetwork.HeaderText = "Network Address";
            this.crNetwork.Name = "crNetwork";
            this.crNetwork.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.crNetwork.Width = 104;
            // 
            // crNetmask
            // 
            this.crNetmask.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.crNetmask.HeaderText = "Network Mask (bits)";
            this.crNetmask.Name = "crNetmask";
            this.crNetmask.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.crNetmask.Width = 96;
            // 
            // crDescription
            // 
            this.crDescription.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.crDescription.HeaderText = "Network Description";
            this.crDescription.Name = "crDescription";
            // 
            // crDomain
            // 
            this.crDomain.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.crDomain.HeaderText = "Domain";
            this.crDomain.Name = "crDomain";
            this.crDomain.Width = 68;
            // 
            // crUser
            // 
            this.crUser.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.crUser.HeaderText = "Username";
            this.crUser.Name = "crUser";
            this.crUser.Width = 80;
            // 
            // crPassword
            // 
            this.crPassword.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.crPassword.HeaderText = "Password";
            this.crPassword.Name = "crPassword";
            this.crPassword.Width = 78;
            // 
            // frmCredentials
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(843, 432);
            this.Controls.Add(this.grdCredentials);
            this.Controls.Add(this.tls);
            this.Name = "frmCredentials";
            this.Text = "Security Credentials";
            this.Load += new System.EventHandler(this.frmCredentials_Load);
            this.tls.ResumeLayout(false);
            this.tls.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdCredentials)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip tls;
        private System.Windows.Forms.DataGridView grdCredentials;
        private System.Windows.Forms.ToolStripButton btnSave;
        private System.Windows.Forms.DataGridViewTextBoxColumn crNetwork;
        private System.Windows.Forms.DataGridViewTextBoxColumn crNetmask;
        private System.Windows.Forms.DataGridViewTextBoxColumn crDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn crDomain;
        private System.Windows.Forms.DataGridViewTextBoxColumn crUser;
        private System.Windows.Forms.DataGridViewTextBoxColumn crPassword;
    }
}