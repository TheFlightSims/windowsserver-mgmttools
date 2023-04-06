namespace DatabaseManager
{
    partial class frmAccountInfo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAccountInfo));
            btnConfirm = new System.Windows.Forms.Button();
            ucAccountInfo = new Controls.UC_DbAccountInfo();
            btnCancel = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // btnConfirm
            // 
            btnConfirm.Location = new System.Drawing.Point(135, 245);
            btnConfirm.Margin = new System.Windows.Forms.Padding(4);
            btnConfirm.Name = "btnConfirm";
            btnConfirm.Size = new System.Drawing.Size(88, 29);
            btnConfirm.TabIndex = 11;
            btnConfirm.Text = "OK";
            btnConfirm.UseVisualStyleBackColor = true;
            btnConfirm.Click += btnConfirm_Click;
            // 
            // ucAccountInfo
            // 
            ucAccountInfo.DatabaseType = DatabaseInterpreter.Model.DatabaseType.SqlServer;
            ucAccountInfo.Location = new System.Drawing.Point(3, 14);
            ucAccountInfo.Margin = new System.Windows.Forms.Padding(5);
            ucAccountInfo.Name = "ucAccountInfo";
            ucAccountInfo.Size = new System.Drawing.Size(444, 222);
            ucAccountInfo.TabIndex = 0;
            ucAccountInfo.Load += ucAccountInfo_Load;
            // 
            // btnCancel
            // 
            btnCancel.Location = new System.Drawing.Point(250, 245);
            btnCancel.Margin = new System.Windows.Forms.Padding(4);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(88, 29);
            btnCancel.TabIndex = 12;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // frmAccountInfo
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(450, 290);
            Controls.Add(btnCancel);
            Controls.Add(btnConfirm);
            Controls.Add(ucAccountInfo);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(4);
            MaximizeBox = false;
            Name = "frmAccountInfo";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Database Account";
            Activated += frmAccountInfo_Activated;
            Load += frmAccountInfo_Load;
            ResumeLayout(false);
        }

        #endregion

        private Controls.UC_DbAccountInfo ucAccountInfo;
        private System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.Button btnCancel;
    }
}