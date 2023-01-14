namespace WSUSAdminAssistant
{
    partial class frmDefaultSUS
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
            this.lblSusID = new System.Windows.Forms.Label();
            this.txtSusID = new System.Windows.Forms.MaskedTextBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.lstGUIDs = new System.Windows.Forms.ListBox();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblSusID
            // 
            this.lblSusID.AutoSize = true;
            this.lblSusID.Location = new System.Drawing.Point(12, 16);
            this.lblSusID.Name = "lblSusID";
            this.lblSusID.Size = new System.Drawing.Size(46, 13);
            this.lblSusID.TabIndex = 0;
            this.lblSusID.Text = "SUS ID:";
            // 
            // txtSusID
            // 
            this.txtSusID.Location = new System.Drawing.Point(66, 13);
            this.txtSusID.Mask = "AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAA";
            this.txtSusID.Name = "txtSusID";
            this.txtSusID.Size = new System.Drawing.Size(217, 20);
            this.txtSusID.TabIndex = 1;
            this.txtSusID.TextChanged += new System.EventHandler(this.txtSusID_TextChanged);
            this.txtSusID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSusID_KeyPress);
            // 
            // btnAdd
            // 
            this.btnAdd.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnAdd.Enabled = false;
            this.btnAdd.Location = new System.Drawing.Point(300, 12);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = "&Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // lstGUIDs
            // 
            this.lstGUIDs.FormattingEnabled = true;
            this.lstGUIDs.Location = new System.Drawing.Point(15, 58);
            this.lstGUIDs.Name = "lstGUIDs";
            this.lstGUIDs.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstGUIDs.Size = new System.Drawing.Size(360, 95);
            this.lstGUIDs.Sorted = true;
            this.lstGUIDs.TabIndex = 3;
            this.lstGUIDs.DoubleClick += new System.EventHandler(this.lstGUIDs_DoubleClick);
            this.lstGUIDs.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lstGUIDs_KeyDown);
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(15, 159);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(75, 23);
            this.btnEdit.TabIndex = 4;
            this.btnEdit.Text = "&Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(205, 160);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(300, 160);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(110, 160);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 5;
            this.btnDelete.Text = "&Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // frmDefaultSUS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(395, 195);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.lstGUIDs);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.txtSusID);
            this.Controls.Add(this.lblSusID);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmDefaultSUS";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Default SUS IDs";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblSusID;
        private System.Windows.Forms.MaskedTextBox txtSusID;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.ListBox lstGUIDs;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnDelete;
    }
}