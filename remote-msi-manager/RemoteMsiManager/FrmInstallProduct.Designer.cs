namespace RemoteMsiManager
{
    partial class FrmInstallProduct
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmInstallProduct));
            this.label1 = new System.Windows.Forms.Label();
            this.txtBxOptions = new System.Windows.Forms.TextBox();
            this.txtBxResult = new System.Windows.Forms.TextBox();
            this.lblResult = new System.Windows.Forms.Label();
            this.txtBxTargetComputer = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtBxLocalPackage = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.chkLstFiles = new System.Windows.Forms.CheckedListBox();
            this.chklstFolders = new System.Windows.Forms.CheckedListBox();
            this.txtBxStatus = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnRemoveFolders = new System.Windows.Forms.Button();
            this.btnAddFolders = new System.Windows.Forms.Button();
            this.btnRemoveFiles = new System.Windows.Forms.Button();
            this.btnAddAdditionnalFiles = new System.Windows.Forms.Button();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.btnInstall = new System.Windows.Forms.Button();
            this.ChkBxNeverRestart = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // txtBxOptions
            // 
            resources.ApplyResources(this.txtBxOptions, "txtBxOptions");
            this.txtBxOptions.Name = "txtBxOptions";
            // 
            // txtBxResult
            // 
            resources.ApplyResources(this.txtBxResult, "txtBxResult");
            this.txtBxResult.Name = "txtBxResult";
            this.txtBxResult.ReadOnly = true;
            this.txtBxResult.TabStop = false;
            // 
            // lblResult
            // 
            resources.ApplyResources(this.lblResult, "lblResult");
            this.lblResult.Name = "lblResult";
            // 
            // txtBxTargetComputer
            // 
            resources.ApplyResources(this.txtBxTargetComputer, "txtBxTargetComputer");
            this.txtBxTargetComputer.Name = "txtBxTargetComputer";
            this.txtBxTargetComputer.ReadOnly = true;
            this.txtBxTargetComputer.TabStop = false;
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // txtBxLocalPackage
            // 
            resources.ApplyResources(this.txtBxLocalPackage, "txtBxLocalPackage");
            this.txtBxLocalPackage.Name = "txtBxLocalPackage";
            this.txtBxLocalPackage.TextChanged += new System.EventHandler(this.TxtBxLocation_TextChanged);
            this.txtBxLocalPackage.Leave += new System.EventHandler(this.TxtBxLocation_Leave);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // chkLstFiles
            // 
            resources.ApplyResources(this.chkLstFiles, "chkLstFiles");
            this.chkLstFiles.FormattingEnabled = true;
            this.chkLstFiles.Name = "chkLstFiles";
            this.chkLstFiles.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.ChkLstFiles_ItemCheck);
            // 
            // chklstFolders
            // 
            resources.ApplyResources(this.chklstFolders, "chklstFolders");
            this.chklstFolders.FormattingEnabled = true;
            this.chklstFolders.Name = "chklstFolders";
            this.chklstFolders.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.ChklstFolders_ItemCheck);
            // 
            // txtBxStatus
            // 
            resources.ApplyResources(this.txtBxStatus, "txtBxStatus");
            this.txtBxStatus.Name = "txtBxStatus";
            this.txtBxStatus.ReadOnly = true;
            this.txtBxStatus.TabStop = false;
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // btnClose
            // 
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Image = global::RemoteMsiManager.Properties.Resources.Exit24x24;
            this.btnClose.Name = "btnClose";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // btnRemoveFolders
            // 
            resources.ApplyResources(this.btnRemoveFolders, "btnRemoveFolders");
            this.btnRemoveFolders.Image = global::RemoteMsiManager.Properties.Resources.DeleteFolder32x32;
            this.btnRemoveFolders.Name = "btnRemoveFolders";
            this.btnRemoveFolders.UseVisualStyleBackColor = true;
            this.btnRemoveFolders.Click += new System.EventHandler(this.BtnRemoveFolders_Click);
            // 
            // btnAddFolders
            // 
            resources.ApplyResources(this.btnAddFolders, "btnAddFolders");
            this.btnAddFolders.Image = global::RemoteMsiManager.Properties.Resources.AddFolder32x32;
            this.btnAddFolders.Name = "btnAddFolders";
            this.btnAddFolders.UseVisualStyleBackColor = true;
            this.btnAddFolders.Click += new System.EventHandler(this.BtnAddFolders_Click);
            // 
            // btnRemoveFiles
            // 
            resources.ApplyResources(this.btnRemoveFiles, "btnRemoveFiles");
            this.btnRemoveFiles.Image = global::RemoteMsiManager.Properties.Resources.DeleteFile32x32;
            this.btnRemoveFiles.Name = "btnRemoveFiles";
            this.btnRemoveFiles.UseVisualStyleBackColor = true;
            this.btnRemoveFiles.Click += new System.EventHandler(this.BtnRemoveFiles_Click);
            // 
            // btnAddAdditionnalFiles
            // 
            resources.ApplyResources(this.btnAddAdditionnalFiles, "btnAddAdditionnalFiles");
            this.btnAddAdditionnalFiles.Image = global::RemoteMsiManager.Properties.Resources.AddFile32x32;
            this.btnAddAdditionnalFiles.Name = "btnAddAdditionnalFiles";
            this.btnAddAdditionnalFiles.UseVisualStyleBackColor = true;
            this.btnAddAdditionnalFiles.Click += new System.EventHandler(this.BtnAddAdditionnalFiles_Click);
            // 
            // btnBrowse
            // 
            resources.ApplyResources(this.btnBrowse, "btnBrowse");
            this.btnBrowse.Image = global::RemoteMsiManager.Properties.Resources.Browse32x32;
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Click += new System.EventHandler(this.BtnBrowse_Click);
            // 
            // btnInstall
            // 
            resources.ApplyResources(this.btnInstall, "btnInstall");
            this.btnInstall.Image = global::RemoteMsiManager.Properties.Resources.Install32x32;
            this.btnInstall.Name = "btnInstall";
            this.btnInstall.UseVisualStyleBackColor = true;
            this.btnInstall.Click += new System.EventHandler(this.BtnInstall_Click);
            // 
            // ChkBxNeverRestart
            // 
            resources.ApplyResources(this.ChkBxNeverRestart, "ChkBxNeverRestart");
            this.ChkBxNeverRestart.Checked = true;
            this.ChkBxNeverRestart.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ChkBxNeverRestart.Name = "ChkBxNeverRestart";
            this.ChkBxNeverRestart.UseVisualStyleBackColor = true;
            // 
            // FrmInstallProduct
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.Controls.Add(this.ChkBxNeverRestart);
            this.Controls.Add(this.chklstFolders);
            this.Controls.Add(this.chkLstFiles);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnRemoveFolders);
            this.Controls.Add(this.btnAddFolders);
            this.Controls.Add(this.btnRemoveFiles);
            this.Controls.Add(this.btnAddAdditionnalFiles);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtBxLocalPackage);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.lblResult);
            this.Controls.Add(this.txtBxTargetComputer);
            this.Controls.Add(this.txtBxStatus);
            this.Controls.Add(this.txtBxResult);
            this.Controls.Add(this.txtBxOptions);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnInstall);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmInstallProduct";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnInstall;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtBxOptions;
        private System.Windows.Forms.TextBox txtBxResult;
        private System.Windows.Forms.Label lblResult;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TextBox txtBxTargetComputer;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtBxLocalPackage;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnAddAdditionnalFiles;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnAddFolders;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckedListBox chkLstFiles;
        private System.Windows.Forms.CheckedListBox chklstFolders;
        private System.Windows.Forms.Button btnRemoveFiles;
        private System.Windows.Forms.Button btnRemoveFolders;
        private System.Windows.Forms.TextBox txtBxStatus;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox ChkBxNeverRestart;
    }
}