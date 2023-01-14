namespace WSUSAdminAssistant
{
    partial class frmComputerGroupRules
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
            this.grdRegEx = new System.Windows.Forms.DataGridView();
            this.rxPriority = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rxComputerRegEx = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rxIPRegEx = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rxComputerGroup = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.rxComment = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rxEnabled = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.spl = new System.Windows.Forms.SplitContainer();
            this.lstResults = new System.Windows.Forms.ListBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnSave = new System.Windows.Forms.ToolStripButton();
            this.btnTestRule = new System.Windows.Forms.ToolStripButton();
            this.btnPCsNotCovered = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.grdRegEx)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spl)).BeginInit();
            this.spl.Panel1.SuspendLayout();
            this.spl.Panel2.SuspendLayout();
            this.spl.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // grdRegEx
            // 
            this.grdRegEx.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdRegEx.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.rxPriority,
            this.rxComputerRegEx,
            this.rxIPRegEx,
            this.rxComputerGroup,
            this.rxComment,
            this.rxEnabled});
            this.grdRegEx.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdRegEx.Location = new System.Drawing.Point(0, 0);
            this.grdRegEx.Name = "grdRegEx";
            this.grdRegEx.Size = new System.Drawing.Size(1240, 263);
            this.grdRegEx.TabIndex = 0;
            this.grdRegEx.RowLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdRegEx_RowLeave);
            // 
            // rxPriority
            // 
            this.rxPriority.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.rxPriority.HeaderText = "Rule Priority";
            this.rxPriority.Name = "rxPriority";
            this.rxPriority.Width = 81;
            // 
            // rxComputerRegEx
            // 
            this.rxComputerRegEx.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.rxComputerRegEx.HeaderText = "Computer Name RegEx Rule";
            this.rxComputerRegEx.Name = "rxComputerRegEx";
            this.rxComputerRegEx.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.rxComputerRegEx.Width = 114;
            // 
            // rxIPRegEx
            // 
            this.rxIPRegEx.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.rxIPRegEx.HeaderText = "IP RegEx Rule";
            this.rxIPRegEx.Name = "rxIPRegEx";
            this.rxIPRegEx.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.rxIPRegEx.Width = 75;
            // 
            // rxComputerGroup
            // 
            this.rxComputerGroup.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.rxComputerGroup.HeaderText = "Computer Group";
            this.rxComputerGroup.Name = "rxComputerGroup";
            this.rxComputerGroup.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // rxComment
            // 
            this.rxComment.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.rxComment.HeaderText = "Comment";
            this.rxComment.Name = "rxComment";
            // 
            // rxEnabled
            // 
            this.rxEnabled.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.rxEnabled.HeaderText = "Rule Enabled";
            this.rxEnabled.Name = "rxEnabled";
            this.rxEnabled.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.rxEnabled.Width = 88;
            // 
            // spl
            // 
            this.spl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spl.Location = new System.Drawing.Point(0, 0);
            this.spl.Name = "spl";
            this.spl.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // spl.Panel1
            // 
            this.spl.Panel1.Controls.Add(this.grdRegEx);
            this.spl.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            // 
            // spl.Panel2
            // 
            this.spl.Panel2.Controls.Add(this.lstResults);
            this.spl.Panel2.Controls.Add(this.toolStrip1);
            this.spl.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.spl.Size = new System.Drawing.Size(1240, 523);
            this.spl.SplitterDistance = 263;
            this.spl.TabIndex = 1;
            // 
            // lstResults
            // 
            this.lstResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstResults.FormattingEnabled = true;
            this.lstResults.Location = new System.Drawing.Point(0, 25);
            this.lstResults.Name = "lstResults";
            this.lstResults.Size = new System.Drawing.Size(1240, 231);
            this.lstResults.TabIndex = 1;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnSave,
            this.btnTestRule,
            this.btnPCsNotCovered});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1240, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnSave
            // 
            this.btnSave.Image = global::WSUSAdminAssistant.Properties.Resources.Save;
            this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(82, 22);
            this.btnSave.Text = "Save Rules";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnTestRule
            // 
            this.btnTestRule.Image = global::WSUSAdminAssistant.Properties.Resources.Zoom;
            this.btnTestRule.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnTestRule.Name = "btnTestRule";
            this.btnTestRule.Size = new System.Drawing.Size(75, 22);
            this.btnTestRule.Text = "Test Rule";
            this.btnTestRule.Click += new System.EventHandler(this.btnTestRule_Click);
            // 
            // btnPCsNotCovered
            // 
            this.btnPCsNotCovered.Image = global::WSUSAdminAssistant.Properties.Resources.Legend;
            this.btnPCsNotCovered.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPCsNotCovered.Name = "btnPCsNotCovered";
            this.btnPCsNotCovered.Size = new System.Drawing.Size(164, 22);
            this.btnPCsNotCovered.Text = "PCs Not Covered by Rules";
            this.btnPCsNotCovered.Click += new System.EventHandler(this.btnPCsNotCovered_Click);
            // 
            // frmComputerGroupRules
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1240, 523);
            this.Controls.Add(this.spl);
            this.Name = "frmComputerGroupRules";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Computer Group Membership Rules";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmComputerGroupRules_FormClosing);
            this.Load += new System.EventHandler(this.frmComputerGroupRules_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grdRegEx)).EndInit();
            this.spl.Panel1.ResumeLayout(false);
            this.spl.Panel2.ResumeLayout(false);
            this.spl.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spl)).EndInit();
            this.spl.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView grdRegEx;
        private System.Windows.Forms.SplitContainer spl;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnSave;
        private System.Windows.Forms.ToolStripButton btnTestRule;
        private System.Windows.Forms.ListBox lstResults;
        private System.Windows.Forms.ToolStripButton btnPCsNotCovered;
        private System.Windows.Forms.DataGridViewTextBoxColumn rxPriority;
        private System.Windows.Forms.DataGridViewTextBoxColumn rxComputerRegEx;
        private System.Windows.Forms.DataGridViewTextBoxColumn rxIPRegEx;
        private System.Windows.Forms.DataGridViewComboBoxColumn rxComputerGroup;
        private System.Windows.Forms.DataGridViewTextBoxColumn rxComment;
        private System.Windows.Forms.DataGridViewCheckBoxColumn rxEnabled;
    }
}