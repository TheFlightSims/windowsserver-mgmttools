namespace GPOChecker
{
    partial class Update
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
            this.radioEnabled = new System.Windows.Forms.RadioButton();
            this.radioDisabled = new System.Windows.Forms.RadioButton();
            this.UnitLabel = new System.Windows.Forms.Label();
            this.HeaderLabel = new System.Windows.Forms.Label();
            this.DescriptionLabel = new System.Windows.Forms.Label();
            this.AdviceLabel = new System.Windows.Forms.Label();
            this.OKButton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            this.ValueSelect = new System.Windows.Forms.NumericUpDown();
            this.setReccom = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.ValueSelect)).BeginInit();
            this.SuspendLayout();
            // 
            // radioEnabled
            // 
            this.radioEnabled.AutoSize = true;
            this.radioEnabled.Location = new System.Drawing.Point(40, 70);
            this.radioEnabled.Name = "radioEnabled";
            this.radioEnabled.Size = new System.Drawing.Size(64, 17);
            this.radioEnabled.TabIndex = 1;
            this.radioEnabled.TabStop = true;
            this.radioEnabled.Text = "Enabled";
            this.radioEnabled.UseVisualStyleBackColor = true;
            // 
            // radioDisabled
            // 
            this.radioDisabled.AutoSize = true;
            this.radioDisabled.Location = new System.Drawing.Point(40, 96);
            this.radioDisabled.Name = "radioDisabled";
            this.radioDisabled.Size = new System.Drawing.Size(66, 17);
            this.radioDisabled.TabIndex = 2;
            this.radioDisabled.TabStop = true;
            this.radioDisabled.Text = "Disabled";
            this.radioDisabled.UseVisualStyleBackColor = true;
            // 
            // UnitLabel
            // 
            this.UnitLabel.AutoSize = true;
            this.UnitLabel.Location = new System.Drawing.Point(105, 84);
            this.UnitLabel.Name = "UnitLabel";
            this.UnitLabel.Size = new System.Drawing.Size(0, 13);
            this.UnitLabel.TabIndex = 3;
            // 
            // HeaderLabel
            // 
            this.HeaderLabel.AutoSize = true;
            this.HeaderLabel.Location = new System.Drawing.Point(17, 62);
            this.HeaderLabel.Name = "HeaderLabel";
            this.HeaderLabel.Size = new System.Drawing.Size(0, 13);
            this.HeaderLabel.TabIndex = 4;
            // 
            // DescriptionLabel
            // 
            this.DescriptionLabel.AutoSize = true;
            this.DescriptionLabel.Location = new System.Drawing.Point(34, 24);
            this.DescriptionLabel.MaximumSize = new System.Drawing.Size(200, 0);
            this.DescriptionLabel.Name = "DescriptionLabel";
            this.DescriptionLabel.Size = new System.Drawing.Size(32, 13);
            this.DescriptionLabel.TabIndex = 5;
            this.DescriptionLabel.Text = "Desc";
            // 
            // AdviceLabel
            // 
            this.AdviceLabel.AutoSize = true;
            this.AdviceLabel.ForeColor = System.Drawing.Color.DarkOrange;
            this.AdviceLabel.Location = new System.Drawing.Point(44, 164);
            this.AdviceLabel.MaximumSize = new System.Drawing.Size(200, 0);
            this.AdviceLabel.Name = "AdviceLabel";
            this.AdviceLabel.Size = new System.Drawing.Size(32, 13);
            this.AdviceLabel.TabIndex = 6;
            this.AdviceLabel.Text = "Desc";
            // 
            // OKButton
            // 
            this.OKButton.Location = new System.Drawing.Point(100, 331);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(75, 23);
            this.OKButton.TabIndex = 7;
            this.OKButton.Text = "OK";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // CancelButton
            // 
            this.CancelButton.Location = new System.Drawing.Point(185, 332);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(75, 23);
            this.CancelButton.TabIndex = 8;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // ValueSelect
            // 
            this.ValueSelect.Location = new System.Drawing.Point(40, 92);
            this.ValueSelect.Name = "ValueSelect";
            this.ValueSelect.Size = new System.Drawing.Size(74, 20);
            this.ValueSelect.TabIndex = 9;
            this.ValueSelect.ValueChanged += new System.EventHandler(this.ValueSelect_ValueChanged);
            // 
            // setReccom
            // 
            this.setReccom.Location = new System.Drawing.Point(185, 116);
            this.setReccom.Name = "setReccom";
            this.setReccom.Size = new System.Drawing.Size(75, 38);
            this.setReccom.TabIndex = 10;
            this.setReccom.Text = "Set to recommended";
            this.setReccom.UseVisualStyleBackColor = true;
            this.setReccom.Click += new System.EventHandler(this.setReccom_Click);
            // 
            // Update
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(272, 367);
            this.Controls.Add(this.setReccom);
            this.Controls.Add(this.ValueSelect);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.AdviceLabel);
            this.Controls.Add(this.DescriptionLabel);
            this.Controls.Add(this.HeaderLabel);
            this.Controls.Add(this.UnitLabel);
            this.Controls.Add(this.radioDisabled);
            this.Controls.Add(this.radioEnabled);
            this.Name = "Update";
            this.Text = "Update";
            this.Load += new System.EventHandler(this.Update_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ValueSelect)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.RadioButton radioEnabled;
        private System.Windows.Forms.RadioButton radioDisabled;
        private System.Windows.Forms.Label UnitLabel;
        private System.Windows.Forms.Label HeaderLabel;
        private System.Windows.Forms.Label DescriptionLabel;
        private System.Windows.Forms.Label AdviceLabel;
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.NumericUpDown ValueSelect;
        private System.Windows.Forms.Button setReccom;
    }
}