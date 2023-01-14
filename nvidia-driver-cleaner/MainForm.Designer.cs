namespace NvidiaCleaner
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabMain = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.freeUpSpaceButton = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.tabSettings = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cRepository = new System.Windows.Forms.CheckBox();
            this.cGenerateLog = new System.Windows.Forms.CheckBox();
            this.basicSettingsGroup = new System.Windows.Forms.GroupBox();
            this.cDownloaded = new System.Windows.Forms.CheckBox();
            this.cExtracted = new System.Windows.Forms.CheckBox();
            this.tabControl1.SuspendLayout();
            this.tabMain.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabSettings.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.basicSettingsGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.AllowDrop = true;
            this.tabControl1.Controls.Add(this.tabMain);
            this.tabControl1.Controls.Add(this.tabSettings);
            resources.ApplyResources(this.tabControl1, "tabControl1");
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            // 
            // tabMain
            // 
            this.tabMain.Controls.Add(this.groupBox2);
            resources.ApplyResources(this.tabMain, "tabMain");
            this.tabMain.Name = "tabMain";
            this.tabMain.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.freeUpSpaceButton);
            this.groupBox2.Controls.Add(this.progressBar);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            this.groupBox2.Enter += new System.EventHandler(this.groupBox2_Enter);
            // 
            // freeUpSpaceButton
            // 
            resources.ApplyResources(this.freeUpSpaceButton, "freeUpSpaceButton");
            this.freeUpSpaceButton.Name = "freeUpSpaceButton";
            this.freeUpSpaceButton.UseVisualStyleBackColor = true;
            this.freeUpSpaceButton.Click += new System.EventHandler(this.FreeUpSpaceButton_Click);
            // 
            // progressBar
            // 
            resources.ApplyResources(this.progressBar, "progressBar");
            this.progressBar.Name = "progressBar";
            // 
            // tabSettings
            // 
            this.tabSettings.Controls.Add(this.groupBox3);
            this.tabSettings.Controls.Add(this.basicSettingsGroup);
            resources.ApplyResources(this.tabSettings, "tabSettings");
            this.tabSettings.Name = "tabSettings";
            this.tabSettings.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cRepository);
            this.groupBox3.Controls.Add(this.cGenerateLog);
            this.groupBox3.ForeColor = System.Drawing.Color.Maroon;
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            this.groupBox3.Enter += new System.EventHandler(this.groupBox3_Enter);
            // 
            // cRepository
            // 
            resources.ApplyResources(this.cRepository, "cRepository");
            this.cRepository.Name = "cRepository";
            this.cRepository.UseVisualStyleBackColor = true;
            // 
            // cGenerateLog
            // 
            resources.ApplyResources(this.cGenerateLog, "cGenerateLog");
            this.cGenerateLog.ForeColor = System.Drawing.Color.Maroon;
            this.cGenerateLog.Name = "cGenerateLog";
            this.cGenerateLog.UseVisualStyleBackColor = true;
            // 
            // basicSettingsGroup
            // 
            this.basicSettingsGroup.Controls.Add(this.cDownloaded);
            this.basicSettingsGroup.Controls.Add(this.cExtracted);
            resources.ApplyResources(this.basicSettingsGroup, "basicSettingsGroup");
            this.basicSettingsGroup.Name = "basicSettingsGroup";
            this.basicSettingsGroup.TabStop = false;
            // 
            // cDownloaded
            // 
            resources.ApplyResources(this.cDownloaded, "cDownloaded");
            this.cDownloaded.Name = "cDownloaded";
            this.cDownloaded.UseVisualStyleBackColor = true;
            // 
            // cExtracted
            // 
            resources.ApplyResources(this.cExtracted, "cExtracted");
            this.cExtracted.Name = "cExtracted";
            this.cExtracted.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.tabControl1.ResumeLayout(false);
            this.tabMain.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.tabSettings.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.basicSettingsGroup.ResumeLayout(false);
            this.basicSettingsGroup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabMain;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Button freeUpSpaceButton;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox cRepository;
        private System.Windows.Forms.CheckBox cGenerateLog;
        private System.Windows.Forms.GroupBox basicSettingsGroup;
        private System.Windows.Forms.CheckBox cDownloaded;
        private System.Windows.Forms.CheckBox cExtracted;
        public System.Windows.Forms.TabPage tabSettings;
    }
}

