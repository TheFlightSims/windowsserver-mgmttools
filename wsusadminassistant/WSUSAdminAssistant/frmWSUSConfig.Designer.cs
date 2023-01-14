namespace WSUSAdminAssistant
{
    partial class frmWSUSConfig
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
            this.lblSQL = new System.Windows.Forms.Label();
            this.txtSQL = new System.Windows.Forms.TextBox();
            this.lblUser = new System.Windows.Forms.Label();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblConfirm = new System.Windows.Forms.Label();
            this.txtConfirm = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.lblWSUS = new System.Windows.Forms.Label();
            this.txtWSUS = new System.Windows.Forms.TextBox();
            this.lblDB = new System.Windows.Forms.Label();
            this.txtDB = new System.Windows.Forms.TextBox();
            this.chkSecure = new System.Windows.Forms.CheckBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.chkIntegratedSecurity = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // lblSQL
            // 
            this.lblSQL.AutoSize = true;
            this.lblSQL.Location = new System.Drawing.Point(13, 13);
            this.lblSQL.Name = "lblSQL";
            this.lblSQL.Size = new System.Drawing.Size(101, 13);
            this.lblSQL.TabIndex = 0;
            this.lblSQL.Text = "WSUS SQL Server:";
            // 
            // txtSQL
            // 
            this.txtSQL.Location = new System.Drawing.Point(138, 13);
            this.txtSQL.Name = "txtSQL";
            this.txtSQL.Size = new System.Drawing.Size(100, 20);
            this.txtSQL.TabIndex = 1;
            // 
            // lblUser
            // 
            this.lblUser.AutoSize = true;
            this.lblUser.Location = new System.Drawing.Point(13, 91);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(82, 13);
            this.lblUser.TabIndex = 5;
            this.lblUser.Text = "SQL Username:";
            // 
            // txtUser
            // 
            this.txtUser.Location = new System.Drawing.Point(138, 88);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(100, 20);
            this.txtUser.TabIndex = 6;
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(13, 118);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(80, 13);
            this.lblPassword.TabIndex = 7;
            this.lblPassword.Text = "SQL Password:";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(138, 115);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(100, 20);
            this.txtPassword.TabIndex = 8;
            this.txtPassword.TextChanged += new System.EventHandler(this.txtPassword_TextChanged);
            // 
            // lblConfirm
            // 
            this.lblConfirm.AutoSize = true;
            this.lblConfirm.Location = new System.Drawing.Point(41, 144);
            this.lblConfirm.Name = "lblConfirm";
            this.lblConfirm.Size = new System.Drawing.Size(49, 13);
            this.lblConfirm.TabIndex = 9;
            this.lblConfirm.Text = "(re-enter)";
            // 
            // txtConfirm
            // 
            this.txtConfirm.Location = new System.Drawing.Point(138, 141);
            this.txtConfirm.Name = "txtConfirm";
            this.txtConfirm.PasswordChar = '*';
            this.txtConfirm.Size = new System.Drawing.Size(100, 20);
            this.txtConfirm.TabIndex = 10;
            this.txtConfirm.TextChanged += new System.EventHandler(this.txtConfirm_TextChanged);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(16, 240);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 23);
            this.btnSave.TabIndex = 14;
            this.btnSave.Text = "&Save and Restart";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lblWSUS
            // 
            this.lblWSUS.AutoSize = true;
            this.lblWSUS.Location = new System.Drawing.Point(12, 183);
            this.lblWSUS.Name = "lblWSUS";
            this.lblWSUS.Size = new System.Drawing.Size(77, 13);
            this.lblWSUS.TabIndex = 11;
            this.lblWSUS.Text = "WSUS Server:";
            // 
            // txtWSUS
            // 
            this.txtWSUS.Location = new System.Drawing.Point(138, 180);
            this.txtWSUS.Name = "txtWSUS";
            this.txtWSUS.Size = new System.Drawing.Size(100, 20);
            this.txtWSUS.TabIndex = 12;
            // 
            // lblDB
            // 
            this.lblDB.AutoSize = true;
            this.lblDB.Location = new System.Drawing.Point(13, 39);
            this.lblDB.Name = "lblDB";
            this.lblDB.Size = new System.Drawing.Size(92, 13);
            this.lblDB.TabIndex = 2;
            this.lblDB.Text = "WSUS Database:";
            // 
            // txtDB
            // 
            this.txtDB.Location = new System.Drawing.Point(138, 39);
            this.txtDB.Name = "txtDB";
            this.txtDB.Size = new System.Drawing.Size(100, 20);
            this.txtDB.TabIndex = 3;
            // 
            // chkSecure
            // 
            this.chkSecure.AutoSize = true;
            this.chkSecure.Location = new System.Drawing.Point(134, 206);
            this.chkSecure.Name = "chkSecure";
            this.chkSecure.Size = new System.Drawing.Size(117, 17);
            this.chkSecure.TabIndex = 13;
            this.chkSecure.Text = "Secure Connection";
            this.chkSecure.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(138, 240);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 23);
            this.btnCancel.TabIndex = 15;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // chkIntegratedSecurity
            // 
            this.chkIntegratedSecurity.AutoSize = true;
            this.chkIntegratedSecurity.Location = new System.Drawing.Point(123, 65);
            this.chkIntegratedSecurity.Name = "chkIntegratedSecurity";
            this.chkIntegratedSecurity.Size = new System.Drawing.Size(115, 17);
            this.chkIntegratedSecurity.TabIndex = 4;
            this.chkIntegratedSecurity.Text = "Integrated Security";
            this.chkIntegratedSecurity.UseVisualStyleBackColor = true;
            this.chkIntegratedSecurity.CheckedChanged += new System.EventHandler(this.chkIntegratedSecurity_CheckedChanged);
            // 
            // frmWSUSConfig
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(263, 288);
            this.Controls.Add(this.chkIntegratedSecurity);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.chkSecure);
            this.Controls.Add(this.txtDB);
            this.Controls.Add(this.lblDB);
            this.Controls.Add(this.txtWSUS);
            this.Controls.Add(this.lblWSUS);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtConfirm);
            this.Controls.Add(this.lblConfirm);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.txtUser);
            this.Controls.Add(this.lblUser);
            this.Controls.Add(this.txtSQL);
            this.Controls.Add(this.lblSQL);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmWSUSConfig";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "WSUS Server Configuration";
            this.Load += new System.EventHandler(this.frmWSUSConfig_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblSQL;
        private System.Windows.Forms.TextBox txtSQL;
        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label lblConfirm;
        private System.Windows.Forms.TextBox txtConfirm;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label lblWSUS;
        private System.Windows.Forms.TextBox txtWSUS;
        private System.Windows.Forms.Label lblDB;
        private System.Windows.Forms.TextBox txtDB;
        private System.Windows.Forms.CheckBox chkSecure;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckBox chkIntegratedSecurity;
    }
}