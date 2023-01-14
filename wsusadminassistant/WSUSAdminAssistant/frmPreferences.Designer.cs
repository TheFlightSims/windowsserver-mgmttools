namespace WSUSAdminAssistant
{
    partial class frmPreferences
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPreferences));
            this.tabPreferences = new System.Windows.Forms.TabControl();
            this.tabGeneral = new System.Windows.Forms.TabPage();
            this.btnGroupUpdate = new System.Windows.Forms.Button();
            this.txtGroupUpdate = new System.Windows.Forms.TextBox();
            this.lblGroupUpdate = new System.Windows.Forms.Label();
            this.btnDefaultSUS = new System.Windows.Forms.Button();
            this.txtDefaultSUS = new System.Windows.Forms.TextBox();
            this.lblDefaultSUS = new System.Windows.Forms.Label();
            this.btnCredentials = new System.Windows.Forms.Button();
            this.txtCredentials = new System.Windows.Forms.TextBox();
            this.lblCredentials = new System.Windows.Forms.Label();
            this.btnComputerRegEx = new System.Windows.Forms.Button();
            this.txtComputerRegEx = new System.Windows.Forms.TextBox();
            this.lblComputerRegEx = new System.Windows.Forms.Label();
            this.chkLocalCreds = new System.Windows.Forms.CheckBox();
            this.tabHelpers = new System.Windows.Forms.TabPage();
            this.lnkPSExec = new System.Windows.Forms.LinkLabel();
            this.btnPSExec = new System.Windows.Forms.Button();
            this.txtPSExec = new System.Windows.Forms.TextBox();
            this.lblPSExec = new System.Windows.Forms.Label();
            this.tlsPreferences = new System.Windows.Forms.ToolStrip();
            this.btnSave = new System.Windows.Forms.ToolStripButton();
            this.btnClose = new System.Windows.Forms.ToolStripButton();
            this.tabPreferences.SuspendLayout();
            this.tabGeneral.SuspendLayout();
            this.tabHelpers.SuspendLayout();
            this.tlsPreferences.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPreferences
            // 
            this.tabPreferences.Controls.Add(this.tabGeneral);
            this.tabPreferences.Controls.Add(this.tabHelpers);
            this.tabPreferences.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabPreferences.Location = new System.Drawing.Point(0, 25);
            this.tabPreferences.Multiline = true;
            this.tabPreferences.Name = "tabPreferences";
            this.tabPreferences.SelectedIndex = 0;
            this.tabPreferences.Size = new System.Drawing.Size(475, 433);
            this.tabPreferences.TabIndex = 0;
            // 
            // tabGeneral
            // 
            this.tabGeneral.Controls.Add(this.btnGroupUpdate);
            this.tabGeneral.Controls.Add(this.txtGroupUpdate);
            this.tabGeneral.Controls.Add(this.lblGroupUpdate);
            this.tabGeneral.Controls.Add(this.btnDefaultSUS);
            this.tabGeneral.Controls.Add(this.txtDefaultSUS);
            this.tabGeneral.Controls.Add(this.lblDefaultSUS);
            this.tabGeneral.Controls.Add(this.btnCredentials);
            this.tabGeneral.Controls.Add(this.txtCredentials);
            this.tabGeneral.Controls.Add(this.lblCredentials);
            this.tabGeneral.Controls.Add(this.btnComputerRegEx);
            this.tabGeneral.Controls.Add(this.txtComputerRegEx);
            this.tabGeneral.Controls.Add(this.lblComputerRegEx);
            this.tabGeneral.Controls.Add(this.chkLocalCreds);
            this.tabGeneral.Location = new System.Drawing.Point(4, 22);
            this.tabGeneral.Name = "tabGeneral";
            this.tabGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tabGeneral.Size = new System.Drawing.Size(467, 407);
            this.tabGeneral.TabIndex = 1;
            this.tabGeneral.Text = "General Options";
            this.tabGeneral.UseVisualStyleBackColor = true;
            // 
            // btnGroupUpdate
            // 
            this.btnGroupUpdate.Location = new System.Drawing.Point(433, 111);
            this.btnGroupUpdate.Name = "btnGroupUpdate";
            this.btnGroupUpdate.Size = new System.Drawing.Size(26, 23);
            this.btnGroupUpdate.TabIndex = 6;
            this.btnGroupUpdate.Text = "...";
            this.btnGroupUpdate.UseVisualStyleBackColor = true;
            this.btnGroupUpdate.Click += new System.EventHandler(this.btnGroupUpdate_Click);
            // 
            // txtGroupUpdate
            // 
            this.txtGroupUpdate.Location = new System.Drawing.Point(9, 113);
            this.txtGroupUpdate.Name = "txtGroupUpdate";
            this.txtGroupUpdate.ReadOnly = true;
            this.txtGroupUpdate.Size = new System.Drawing.Size(418, 20);
            this.txtGroupUpdate.TabIndex = 5;
            // 
            // lblGroupUpdate
            // 
            this.lblGroupUpdate.AutoSize = true;
            this.lblGroupUpdate.Location = new System.Drawing.Point(9, 96);
            this.lblGroupUpdate.Name = "lblGroupUpdate";
            this.lblGroupUpdate.Size = new System.Drawing.Size(148, 13);
            this.lblGroupUpdate.TabIndex = 4;
            this.lblGroupUpdate.Text = "Group Update Rules Location";
            // 
            // btnDefaultSUS
            // 
            this.btnDefaultSUS.Location = new System.Drawing.Point(433, 215);
            this.btnDefaultSUS.Name = "btnDefaultSUS";
            this.btnDefaultSUS.Size = new System.Drawing.Size(26, 23);
            this.btnDefaultSUS.TabIndex = 12;
            this.btnDefaultSUS.Text = "...";
            this.btnDefaultSUS.UseVisualStyleBackColor = true;
            this.btnDefaultSUS.Click += new System.EventHandler(this.btnDefaultSUS_Click);
            // 
            // txtDefaultSUS
            // 
            this.txtDefaultSUS.Location = new System.Drawing.Point(9, 217);
            this.txtDefaultSUS.Name = "txtDefaultSUS";
            this.txtDefaultSUS.ReadOnly = true;
            this.txtDefaultSUS.Size = new System.Drawing.Size(418, 20);
            this.txtDefaultSUS.TabIndex = 11;
            // 
            // lblDefaultSUS
            // 
            this.lblDefaultSUS.AutoSize = true;
            this.lblDefaultSUS.Location = new System.Drawing.Point(9, 200);
            this.lblDefaultSUS.Name = "lblDefaultSUS";
            this.lblDefaultSUS.Size = new System.Drawing.Size(124, 13);
            this.lblDefaultSUS.TabIndex = 10;
            this.lblDefaultSUS.Text = "Default SUS ID Location";
            // 
            // btnCredentials
            // 
            this.btnCredentials.Location = new System.Drawing.Point(433, 164);
            this.btnCredentials.Name = "btnCredentials";
            this.btnCredentials.Size = new System.Drawing.Size(26, 23);
            this.btnCredentials.TabIndex = 9;
            this.btnCredentials.Text = "...";
            this.btnCredentials.UseVisualStyleBackColor = true;
            this.btnCredentials.Click += new System.EventHandler(this.btnCredentials_Click);
            // 
            // txtCredentials
            // 
            this.txtCredentials.Location = new System.Drawing.Point(9, 166);
            this.txtCredentials.Name = "txtCredentials";
            this.txtCredentials.ReadOnly = true;
            this.txtCredentials.Size = new System.Drawing.Size(418, 20);
            this.txtCredentials.TabIndex = 8;
            // 
            // lblCredentials
            // 
            this.lblCredentials.AutoSize = true;
            this.lblCredentials.Location = new System.Drawing.Point(9, 149);
            this.lblCredentials.Name = "lblCredentials";
            this.lblCredentials.Size = new System.Drawing.Size(151, 13);
            this.lblCredentials.TabIndex = 7;
            this.lblCredentials.Text = "Computer Credentials Location";
            // 
            // btnComputerRegEx
            // 
            this.btnComputerRegEx.Location = new System.Drawing.Point(433, 62);
            this.btnComputerRegEx.Name = "btnComputerRegEx";
            this.btnComputerRegEx.Size = new System.Drawing.Size(26, 23);
            this.btnComputerRegEx.TabIndex = 3;
            this.btnComputerRegEx.Text = "...";
            this.btnComputerRegEx.UseVisualStyleBackColor = true;
            this.btnComputerRegEx.Click += new System.EventHandler(this.btnComputerRegEx_Click);
            // 
            // txtComputerRegEx
            // 
            this.txtComputerRegEx.Location = new System.Drawing.Point(9, 64);
            this.txtComputerRegEx.Name = "txtComputerRegEx";
            this.txtComputerRegEx.ReadOnly = true;
            this.txtComputerRegEx.Size = new System.Drawing.Size(418, 20);
            this.txtComputerRegEx.TabIndex = 2;
            // 
            // lblComputerRegEx
            // 
            this.lblComputerRegEx.AutoSize = true;
            this.lblComputerRegEx.Location = new System.Drawing.Point(9, 47);
            this.lblComputerRegEx.Name = "lblComputerRegEx";
            this.lblComputerRegEx.Size = new System.Drawing.Size(158, 13);
            this.lblComputerRegEx.TabIndex = 1;
            this.lblComputerRegEx.Text = "Computer Group Rules Location";
            // 
            // chkLocalCreds
            // 
            this.chkLocalCreds.AutoSize = true;
            this.chkLocalCreds.Location = new System.Drawing.Point(8, 6);
            this.chkLocalCreds.Name = "chkLocalCreds";
            this.chkLocalCreds.Size = new System.Drawing.Size(389, 17);
            this.chkLocalCreds.TabIndex = 0;
            this.chkLocalCreds.Text = "Supply current credentials if no other security credentials found for IP address";
            this.chkLocalCreds.UseVisualStyleBackColor = true;
            // 
            // tabHelpers
            // 
            this.tabHelpers.Controls.Add(this.lnkPSExec);
            this.tabHelpers.Controls.Add(this.btnPSExec);
            this.tabHelpers.Controls.Add(this.txtPSExec);
            this.tabHelpers.Controls.Add(this.lblPSExec);
            this.tabHelpers.Location = new System.Drawing.Point(4, 22);
            this.tabHelpers.Name = "tabHelpers";
            this.tabHelpers.Padding = new System.Windows.Forms.Padding(3);
            this.tabHelpers.Size = new System.Drawing.Size(467, 407);
            this.tabHelpers.TabIndex = 0;
            this.tabHelpers.Text = "Helper Applications";
            this.tabHelpers.UseVisualStyleBackColor = true;
            // 
            // lnkPSExec
            // 
            this.lnkPSExec.AutoSize = true;
            this.lnkPSExec.Location = new System.Drawing.Point(397, 10);
            this.lnkPSExec.Name = "lnkPSExec";
            this.lnkPSExec.Size = new System.Drawing.Size(65, 13);
            this.lnkPSExec.TabIndex = 3;
            this.lnkPSExec.TabStop = true;
            this.lnkPSExec.Text = "Get PSExec";
            this.lnkPSExec.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkPSExec_LinkClicked);
            // 
            // btnPSExec
            // 
            this.btnPSExec.Location = new System.Drawing.Point(367, 5);
            this.btnPSExec.Name = "btnPSExec";
            this.btnPSExec.Size = new System.Drawing.Size(24, 23);
            this.btnPSExec.TabIndex = 2;
            this.btnPSExec.Text = "...";
            this.btnPSExec.UseVisualStyleBackColor = true;
            this.btnPSExec.Click += new System.EventHandler(this.btnPSExec_Click);
            // 
            // txtPSExec
            // 
            this.txtPSExec.AcceptsReturn = true;
            this.txtPSExec.Location = new System.Drawing.Point(121, 7);
            this.txtPSExec.Name = "txtPSExec";
            this.txtPSExec.Size = new System.Drawing.Size(240, 20);
            this.txtPSExec.TabIndex = 1;
            // 
            // lblPSExec
            // 
            this.lblPSExec.AutoSize = true;
            this.lblPSExec.Location = new System.Drawing.Point(8, 10);
            this.lblPSExec.Name = "lblPSExec";
            this.lblPSExec.Size = new System.Drawing.Size(65, 13);
            this.lblPSExec.TabIndex = 0;
            this.lblPSExec.Text = "PSExec.exe";
            // 
            // tlsPreferences
            // 
            this.tlsPreferences.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnSave,
            this.btnClose});
            this.tlsPreferences.Location = new System.Drawing.Point(0, 0);
            this.tlsPreferences.Name = "tlsPreferences";
            this.tlsPreferences.Size = new System.Drawing.Size(475, 25);
            this.tlsPreferences.TabIndex = 1;
            this.tlsPreferences.Text = "toolStrip1";
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
            // btnClose
            // 
            this.btnClose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(40, 22);
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // frmPreferences
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(475, 458);
            this.Controls.Add(this.tabPreferences);
            this.Controls.Add(this.tlsPreferences);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmPreferences";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Preferences";
            this.tabPreferences.ResumeLayout(false);
            this.tabGeneral.ResumeLayout(false);
            this.tabGeneral.PerformLayout();
            this.tabHelpers.ResumeLayout(false);
            this.tabHelpers.PerformLayout();
            this.tlsPreferences.ResumeLayout(false);
            this.tlsPreferences.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabPreferences;
        private System.Windows.Forms.TabPage tabHelpers;
        private System.Windows.Forms.ToolStrip tlsPreferences;
        private System.Windows.Forms.Label lblPSExec;
        private System.Windows.Forms.ToolStripButton btnSave;
        private System.Windows.Forms.Button btnPSExec;
        private System.Windows.Forms.TextBox txtPSExec;
        private System.Windows.Forms.ToolStripButton btnClose;
        private System.Windows.Forms.TabPage tabGeneral;
        private System.Windows.Forms.CheckBox chkLocalCreds;
        private System.Windows.Forms.LinkLabel lnkPSExec;
        private System.Windows.Forms.Button btnComputerRegEx;
        private System.Windows.Forms.TextBox txtComputerRegEx;
        private System.Windows.Forms.Label lblComputerRegEx;
        private System.Windows.Forms.Button btnCredentials;
        private System.Windows.Forms.TextBox txtCredentials;
        private System.Windows.Forms.Label lblCredentials;
        private System.Windows.Forms.Button btnDefaultSUS;
        private System.Windows.Forms.TextBox txtDefaultSUS;
        private System.Windows.Forms.Label lblDefaultSUS;
        private System.Windows.Forms.Button btnGroupUpdate;
        private System.Windows.Forms.TextBox txtGroupUpdate;
        private System.Windows.Forms.Label lblGroupUpdate;
    }
}