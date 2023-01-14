namespace SUSWatcher
{
    partial class frmSUSWatch
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSUSWatch));
            this.tim = new System.Windows.Forms.Timer(this.components);
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lstLog = new System.Windows.Forms.ListBox();
            this.tls = new System.Windows.Forms.ToolStrip();
            this.btnSave = new System.Windows.Forms.ToolStripButton();
            this.grdSUSID = new System.Windows.Forms.DataGridView();
            this.susID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.susCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.susSource = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.prg = new System.Windows.Forms.ProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdSUSID)).BeginInit();
            this.SuspendLayout();
            // 
            // tim
            // 
            this.tim.Enabled = true;
            this.tim.Interval = 50;
            this.tim.Tick += new System.EventHandler(this.tim_Tick);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lstLog);
            this.splitContainer1.Panel1.Controls.Add(this.tls);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.grdSUSID);
            this.splitContainer1.Panel2.Controls.Add(this.prg);
            this.splitContainer1.Size = new System.Drawing.Size(659, 368);
            this.splitContainer1.SplitterDistance = 230;
            this.splitContainer1.TabIndex = 0;
            // 
            // lstLog
            // 
            this.lstLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstLog.FormattingEnabled = true;
            this.lstLog.Location = new System.Drawing.Point(0, 25);
            this.lstLog.Name = "lstLog";
            this.lstLog.Size = new System.Drawing.Size(659, 205);
            this.lstLog.TabIndex = 0;
            // 
            // tls
            // 
            this.tls.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnSave});
            this.tls.Location = new System.Drawing.Point(0, 0);
            this.tls.Name = "tls";
            this.tls.Size = new System.Drawing.Size(659, 25);
            this.tls.TabIndex = 1;
            this.tls.Text = "toolStrip1";
            // 
            // btnSave
            // 
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(51, 22);
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // grdSUSID
            // 
            this.grdSUSID.AllowUserToAddRows = false;
            this.grdSUSID.AllowUserToOrderColumns = true;
            this.grdSUSID.AllowUserToResizeColumns = false;
            this.grdSUSID.AllowUserToResizeRows = false;
            this.grdSUSID.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdSUSID.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.susID,
            this.susCount,
            this.susSource});
            this.grdSUSID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdSUSID.Location = new System.Drawing.Point(0, 0);
            this.grdSUSID.Name = "grdSUSID";
            this.grdSUSID.Size = new System.Drawing.Size(659, 111);
            this.grdSUSID.TabIndex = 2;
            // 
            // susID
            // 
            this.susID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.susID.HeaderText = "SUS ID";
            this.susID.Name = "susID";
            // 
            // susCount
            // 
            this.susCount.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.susCount.HeaderText = "Times Seen";
            this.susCount.Name = "susCount";
            this.susCount.Width = 81;
            // 
            // susSource
            // 
            this.susSource.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.susSource.HeaderText = "SUS ID Source";
            this.susSource.Name = "susSource";
            this.susSource.Width = 96;
            // 
            // prg
            // 
            this.prg.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.prg.Location = new System.Drawing.Point(0, 111);
            this.prg.Maximum = 2000;
            this.prg.Name = "prg";
            this.prg.Size = new System.Drawing.Size(659, 23);
            this.prg.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.prg.TabIndex = 1;
            // 
            // frmSUSWatch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(659, 368);
            this.Controls.Add(this.splitContainer1);
            this.Name = "frmSUSWatch";
            this.Text = "SUSWatch";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tls.ResumeLayout(false);
            this.tls.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdSUSID)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer tim;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ProgressBar prg;
        private System.Windows.Forms.ListBox lstLog;
        private System.Windows.Forms.ToolStrip tls;
        private System.Windows.Forms.ToolStripButton btnSave;
        private System.Windows.Forms.DataGridView grdSUSID;
        private System.Windows.Forms.DataGridViewTextBoxColumn susID;
        private System.Windows.Forms.DataGridViewTextBoxColumn susCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn susSource;
    }
}

