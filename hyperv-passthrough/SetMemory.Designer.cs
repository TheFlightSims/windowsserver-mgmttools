namespace HyperVpassthroughdev
{
    partial class SetMemory
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetMemory));
            this.labelhimem = new System.Windows.Forms.Label();
            this.HighMem = new System.Windows.Forms.TextBox();
            this.OK = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelhimem
            // 
            this.labelhimem.AutoSize = true;
            this.labelhimem.Location = new System.Drawing.Point(2, 9);
            this.labelhimem.Name = "labelhimem";
            this.labelhimem.Size = new System.Drawing.Size(69, 13);
            this.labelhimem.TabIndex = 0;
            this.labelhimem.Text = "High Memory";
            this.labelhimem.Click += new System.EventHandler(this.label1_Click);
            // 
            // HighMem
            // 
            this.HighMem.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.HighMem.Location = new System.Drawing.Point(78, 6);
            this.HighMem.Name = "HighMem";
            this.HighMem.Size = new System.Drawing.Size(156, 20);
            this.HighMem.TabIndex = 2;
            this.HighMem.TextChanged += new System.EventHandler(this.HighMemchanges);
            // 
            // OK
            // 
            this.OK.Location = new System.Drawing.Point(78, 58);
            this.OK.Name = "OK";
            this.OK.Size = new System.Drawing.Size(75, 26);
            this.OK.TabIndex = 4;
            this.OK.Text = "OK";
            this.OK.UseVisualStyleBackColor = true;
            this.OK.Click += new System.EventHandler(this.OKClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.DarkRed;
            this.label1.Location = new System.Drawing.Point(5, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(235, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "By default, Low Memory location is set to 512Mb";
            // 
            // SetMemory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(246, 96);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.OK);
            this.Controls.Add(this.HighMem);
            this.Controls.Add(this.labelhimem);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "SetMemory";
            this.Text = "Set Memory";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelhimem;
        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.TextBox HighMem;
        private System.Windows.Forms.Label label1;
    }
}