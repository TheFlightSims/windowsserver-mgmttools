namespace GPOChecker
{
    partial class GPOChecker
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GPOChecker));
            this.PrevButton = new System.Windows.Forms.Button();
            this.NextButton = new System.Windows.Forms.Button();
            this.InfoBox = new System.Windows.Forms.TextBox();
            this.GuidanceButton = new System.Windows.Forms.Button();
            this.MarkAsDone = new System.Windows.Forms.CheckBox();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.SkipButton = new System.Windows.Forms.Button();
            this.NameLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // PrevButton
            // 
            this.PrevButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PrevButton.Location = new System.Drawing.Point(12, 376);
            this.PrevButton.Name = "PrevButton";
            this.PrevButton.Size = new System.Drawing.Size(75, 38);
            this.PrevButton.TabIndex = 1;
            this.PrevButton.Text = " Previous";
            this.PrevButton.UseVisualStyleBackColor = true;
            this.PrevButton.Visible = false;
            this.PrevButton.Click += new System.EventHandler(this.PrevButton_Click);
            // 
            // NextButton
            // 
            this.NextButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NextButton.Location = new System.Drawing.Point(410, 376);
            this.NextButton.Name = "NextButton";
            this.NextButton.Size = new System.Drawing.Size(75, 38);
            this.NextButton.TabIndex = 2;
            this.NextButton.Text = "Go";
            this.NextButton.UseVisualStyleBackColor = true;
            this.NextButton.Click += new System.EventHandler(this.NextButton_Click);
            // 
            // InfoBox
            // 
            this.InfoBox.Location = new System.Drawing.Point(12, 38);
            this.InfoBox.Multiline = true;
            this.InfoBox.Name = "InfoBox";
            this.InfoBox.ReadOnly = true;
            this.InfoBox.Size = new System.Drawing.Size(473, 309);
            this.InfoBox.TabIndex = 3;
            this.InfoBox.Text = resources.GetString("InfoBox.Text");
            // 
            // GuidanceButton
            // 
            this.GuidanceButton.Location = new System.Drawing.Point(221, 391);
            this.GuidanceButton.Name = "GuidanceButton";
            this.GuidanceButton.Size = new System.Drawing.Size(75, 23);
            this.GuidanceButton.TabIndex = 4;
            this.GuidanceButton.Text = "Guidance";
            this.GuidanceButton.UseVisualStyleBackColor = true;
            this.GuidanceButton.Visible = false;
            this.GuidanceButton.Click += new System.EventHandler(this.GuidanceButton_Click);
            // 
            // MarkAsDone
            // 
            this.MarkAsDone.AutoSize = true;
            this.MarkAsDone.Location = new System.Drawing.Point(214, 353);
            this.MarkAsDone.Name = "MarkAsDone";
            this.MarkAsDone.Size = new System.Drawing.Size(91, 17);
            this.MarkAsDone.TabIndex = 5;
            this.MarkAsDone.Text = "Mark as done";
            this.MarkAsDone.UseVisualStyleBackColor = true;
            this.MarkAsDone.Visible = false;
            this.MarkAsDone.CheckedChanged += new System.EventHandler(this.MarkAsDone_CheckedChanged);
            // 
            // SkipButton
            // 
            this.SkipButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SkipButton.Location = new System.Drawing.Point(221, 376);
            this.SkipButton.Name = "SkipButton";
            this.SkipButton.Size = new System.Drawing.Size(75, 38);
            this.SkipButton.TabIndex = 7;
            this.SkipButton.Text = "Skip";
            this.SkipButton.UseVisualStyleBackColor = true;
            this.SkipButton.Visible = false;
            this.SkipButton.Click += new System.EventHandler(this.SkipButton_Click);
            // 
            // NameLabel
            // 
            this.NameLabel.AutoSize = true;
            this.NameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NameLabel.Location = new System.Drawing.Point(12, 9);
            this.NameLabel.MaximumSize = new System.Drawing.Size(480, 0);
            this.NameLabel.Name = "NameLabel";
            this.NameLabel.Size = new System.Drawing.Size(194, 20);
            this.NameLabel.TabIndex = 8;
            this.NameLabel.Text = "Group Policy Analysis Tool";
            // 
            // GPOChecker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(497, 428);
            this.Controls.Add(this.NameLabel);
            this.Controls.Add(this.SkipButton);
            this.Controls.Add(this.MarkAsDone);
            this.Controls.Add(this.GuidanceButton);
            this.Controls.Add(this.InfoBox);
            this.Controls.Add(this.NextButton);
            this.Controls.Add(this.PrevButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "GPOChecker";
            this.Text = "GPO Checker";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button PrevButton;
        private System.Windows.Forms.Button NextButton;
        private System.Windows.Forms.TextBox InfoBox;
        private System.Windows.Forms.Button GuidanceButton;
        private System.Windows.Forms.CheckBox MarkAsDone;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button SkipButton;
        private System.Windows.Forms.Label NameLabel;
    }
}

