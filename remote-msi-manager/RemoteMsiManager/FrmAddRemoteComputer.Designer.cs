namespace RemoteMsiManager
{
    partial class FrmAddRemoteComputer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmAddRemoteComputer));
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtBxComputerName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtBxUsername = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtBxPassword = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btnPing = new System.Windows.Forms.Button();
            this.btnShowPassword = new System.Windows.Forms.Button();
            this.btnTestCredential = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            resources.ApplyResources(this.btnOk, "btnOk");
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Name = "btnOk";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.BtnOk_Click);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // txtBxComputerName
            // 
            resources.ApplyResources(this.txtBxComputerName, "txtBxComputerName");
            this.txtBxComputerName.Name = "txtBxComputerName";
            this.txtBxComputerName.TextChanged += new System.EventHandler(this.TxtBxes_TextChanged);
            this.txtBxComputerName.Leave += new System.EventHandler(this.TxtBxComputerName_Leave);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // txtBxUsername
            // 
            resources.ApplyResources(this.txtBxUsername, "txtBxUsername");
            this.txtBxUsername.Name = "txtBxUsername";
            this.txtBxUsername.TextChanged += new System.EventHandler(this.TxtBxes_TextChanged);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // txtBxPassword
            // 
            resources.ApplyResources(this.txtBxPassword, "txtBxPassword");
            this.txtBxPassword.Name = "txtBxPassword";
            this.txtBxPassword.UseSystemPasswordChar = true;
            this.txtBxPassword.TextChanged += new System.EventHandler(this.TxtBxes_TextChanged);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // btnPing
            // 
            resources.ApplyResources(this.btnPing, "btnPing");
            this.btnPing.Image = global::RemoteMsiManager.Properties.Resources.Red16x16;
            this.btnPing.Name = "btnPing";
            this.btnPing.TabStop = false;
            this.toolTip1.SetToolTip(this.btnPing, resources.GetString("btnPing.ToolTip"));
            this.btnPing.UseVisualStyleBackColor = true;
            this.btnPing.Click += new System.EventHandler(this.BtnPing_Click);
            this.btnPing.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BtnShowPassword_MouseDown);
            this.btnPing.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BtnShowPassword_MouseUp);
            // 
            // btnShowPassword
            // 
            resources.ApplyResources(this.btnShowPassword, "btnShowPassword");
            this.btnShowPassword.Image = global::RemoteMsiManager.Properties.Resources.Eye16x16;
            this.btnShowPassword.Name = "btnShowPassword";
            this.btnShowPassword.TabStop = false;
            this.btnShowPassword.UseVisualStyleBackColor = true;
            this.btnShowPassword.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BtnShowPassword_MouseDown);
            this.btnShowPassword.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BtnShowPassword_MouseUp);
            // 
            // btnTestCredential
            // 
            resources.ApplyResources(this.btnTestCredential, "btnTestCredential");
            this.btnTestCredential.Name = "btnTestCredential";
            this.btnTestCredential.UseVisualStyleBackColor = false;
            this.btnTestCredential.Click += new System.EventHandler(this.BtnTestCredential_Click);
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // FrmAddRemoteComputer
            // 
            this.AcceptButton = this.btnOk;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnTestCredential);
            this.Controls.Add(this.btnPing);
            this.Controls.Add(this.btnShowPassword);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtBxPassword);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtBxUsername);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtBxComputerName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FrmAddRemoteComputer";
            this.ShowInTaskbar = false;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtBxComputerName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtBxUsername;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtBxPassword;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnShowPassword;
        private System.Windows.Forms.Button btnPing;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button btnTestCredential;
        private System.Windows.Forms.Label label5;
    }
}