namespace DatabaseManager
{
    partial class frmDataFilterCondition
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
            this.btnOK = new System.Windows.Forms.Button();
            this.gbRange = new System.Windows.Forms.GroupBox();
            this.panelRange = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtFrom = new System.Windows.Forms.TextBox();
            this.txtTo = new System.Windows.Forms.TextBox();
            this.rbRange = new System.Windows.Forms.RadioButton();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.gbSeries = new System.Windows.Forms.GroupBox();
            this.panelSeries = new System.Windows.Forms.Panel();
            this.txtValues = new System.Windows.Forms.TextBox();
            this.gbSingle = new System.Windows.Forms.GroupBox();
            this.panelSingle = new System.Windows.Forms.Panel();
            this.txtValue = new System.Windows.Forms.TextBox();
            this.cboOperator = new System.Windows.Forms.ComboBox();
            this.rbSingle = new System.Windows.Forms.RadioButton();
            this.rbSeries = new System.Windows.Forms.RadioButton();
            this.gbRange.SuspendLayout();
            this.panelRange.SuspendLayout();
            this.gbSeries.SuspendLayout();
            this.panelSeries.SuspendLayout();
            this.gbSingle.SuspendLayout();
            this.panelSingle.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.Location = new System.Drawing.Point(331, 359);
            this.btnOK.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(82, 28);
            this.btnOK.TabIndex = 12;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // gbRange
            // 
            this.gbRange.Controls.Add(this.panelRange);
            this.gbRange.Location = new System.Drawing.Point(14, 131);
            this.gbRange.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gbRange.Name = "gbRange";
            this.gbRange.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gbRange.Size = new System.Drawing.Size(486, 70);
            this.gbRange.TabIndex = 10;
            this.gbRange.TabStop = false;
            // 
            // panelRange
            // 
            this.panelRange.Controls.Add(this.label2);
            this.panelRange.Controls.Add(this.label3);
            this.panelRange.Controls.Add(this.txtFrom);
            this.panelRange.Controls.Add(this.txtTo);
            this.panelRange.Enabled = false;
            this.panelRange.Location = new System.Drawing.Point(13, 25);
            this.panelRange.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panelRange.Name = "panelRange";
            this.panelRange.Size = new System.Drawing.Size(472, 38);
            this.panelRange.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 11);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 15);
            this.label2.TabIndex = 0;
            this.label2.Text = "Between";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(248, 11);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(27, 15);
            this.label3.TabIndex = 2;
            this.label3.Text = "and";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtFrom
            // 
            this.txtFrom.Location = new System.Drawing.Point(77, 8);
            this.txtFrom.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtFrom.Name = "txtFrom";
            this.txtFrom.Size = new System.Drawing.Size(164, 23);
            this.txtFrom.TabIndex = 1;
            // 
            // txtTo
            // 
            this.txtTo.Location = new System.Drawing.Point(282, 8);
            this.txtTo.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtTo.Name = "txtTo";
            this.txtTo.Size = new System.Drawing.Size(182, 23);
            this.txtTo.TabIndex = 3;
            // 
            // rbRange
            // 
            this.rbRange.AutoSize = true;
            this.rbRange.Location = new System.Drawing.Point(20, 109);
            this.rbRange.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rbRange.Name = "rbRange";
            this.rbRange.Size = new System.Drawing.Size(103, 19);
            this.rbRange.TabIndex = 4;
            this.rbRange.TabStop = true;
            this.rbRange.Text = "Interval criteria";
            this.rbRange.UseVisualStyleBackColor = true;
            this.rbRange.CheckedChanged += new System.EventHandler(this.rbRange_CheckedChanged);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Location = new System.Drawing.Point(420, 359);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(82, 28);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnClear
            // 
            this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnClear.Location = new System.Drawing.Point(26, 359);
            this.btnClear.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(82, 28);
            this.btnClear.TabIndex = 11;
            this.btnClear.Text = "Clear";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // gbSeries
            // 
            this.gbSeries.Controls.Add(this.panelSeries);
            this.gbSeries.Location = new System.Drawing.Point(14, 244);
            this.gbSeries.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gbSeries.Name = "gbSeries";
            this.gbSeries.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gbSeries.Size = new System.Drawing.Size(486, 70);
            this.gbSeries.TabIndex = 14;
            this.gbSeries.TabStop = false;
            // 
            // panelSeries
            // 
            this.panelSeries.Controls.Add(this.txtValues);
            this.panelSeries.Enabled = false;
            this.panelSeries.Location = new System.Drawing.Point(13, 25);
            this.panelSeries.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panelSeries.Name = "panelSeries";
            this.panelSeries.Size = new System.Drawing.Size(472, 38);
            this.panelSeries.TabIndex = 3;
            // 
            // txtValues
            // 
            this.txtValues.Location = new System.Drawing.Point(6, 8);
            this.txtValues.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtValues.Name = "txtValues";
            this.txtValues.Size = new System.Drawing.Size(459, 23);
            this.txtValues.TabIndex = 1;
            // 
            // gbSingle
            // 
            this.gbSingle.Controls.Add(this.panelSingle);
            this.gbSingle.Location = new System.Drawing.Point(14, 25);
            this.gbSingle.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gbSingle.Name = "gbSingle";
            this.gbSingle.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gbSingle.Size = new System.Drawing.Size(486, 70);
            this.gbSingle.TabIndex = 8;
            this.gbSingle.TabStop = false;
            // 
            // panelSingle
            // 
            this.panelSingle.Controls.Add(this.txtValue);
            this.panelSingle.Controls.Add(this.cboOperator);
            this.panelSingle.Location = new System.Drawing.Point(7, 24);
            this.panelSingle.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panelSingle.Name = "panelSingle";
            this.panelSingle.Size = new System.Drawing.Size(478, 38);
            this.panelSingle.TabIndex = 3;
            // 
            // txtValue
            // 
            this.txtValue.Location = new System.Drawing.Point(84, 4);
            this.txtValue.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtValue.Name = "txtValue";
            this.txtValue.Size = new System.Drawing.Size(388, 23);
            this.txtValue.TabIndex = 1;
            // 
            // cboOperator
            // 
            this.cboOperator.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboOperator.Items.AddRange(new object[] {
            "=",
            ">",
            ">=",
            "<",
            "<=",
            "<>",
            "LIKE",
            "NOT LIKE"});
            this.cboOperator.Location = new System.Drawing.Point(12, 4);
            this.cboOperator.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cboOperator.Name = "cboOperator";
            this.cboOperator.Size = new System.Drawing.Size(65, 23);
            this.cboOperator.TabIndex = 0;
            // 
            // rbSingle
            // 
            this.rbSingle.AutoSize = true;
            this.rbSingle.Checked = true;
            this.rbSingle.Location = new System.Drawing.Point(20, 8);
            this.rbSingle.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rbSingle.Name = "rbSingle";
            this.rbSingle.Size = new System.Drawing.Size(127, 19);
            this.rbSingle.TabIndex = 2;
            this.rbSingle.TabStop = true;
            this.rbSingle.Text = "Single value criteria";
            this.rbSingle.UseVisualStyleBackColor = true;
            this.rbSingle.CheckedChanged += new System.EventHandler(this.rbSingle_CheckedChanged);
            // 
            // rbSeries
            // 
            this.rbSeries.AutoSize = true;
            this.rbSeries.Location = new System.Drawing.Point(20, 229);
            this.rbSeries.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rbSeries.Name = "rbSeries";
            this.rbSeries.Size = new System.Drawing.Size(55, 19);
            this.rbSeries.TabIndex = 15;
            this.rbSeries.TabStop = true;
            this.rbSeries.Text = "Series";
            this.rbSeries.UseVisualStyleBackColor = true;
            // 
            // frmDataFilterCondition
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(509, 410);
            this.Controls.Add(this.rbSeries);
            this.Controls.Add(this.gbSeries);
            this.Controls.Add(this.rbRange);
            this.Controls.Add(this.rbSingle);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.gbSingle);
            this.Controls.Add(this.gbRange);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnClear);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.Name = "frmDataFilterCondition";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Query Condition";
            this.Load += new System.EventHandler(this.frmDataFilterCondition_Load);
            this.gbRange.ResumeLayout(false);
            this.panelRange.ResumeLayout(false);
            this.panelRange.PerformLayout();
            this.gbSeries.ResumeLayout(false);
            this.panelSeries.ResumeLayout(false);
            this.panelSeries.PerformLayout();
            this.gbSingle.ResumeLayout(false);
            this.panelSingle.ResumeLayout(false);
            this.panelSingle.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.GroupBox gbRange;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtFrom;
        private System.Windows.Forms.TextBox txtTo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.GroupBox gbSeries;
        private System.Windows.Forms.TextBox txtValues;
        private System.Windows.Forms.RadioButton rbRange;
        private System.Windows.Forms.Panel panelRange;
        private System.Windows.Forms.Panel panelSeries;
        private System.Windows.Forms.GroupBox gbSingle;
        private System.Windows.Forms.Panel panelSingle;
        private System.Windows.Forms.TextBox txtValue;
        private System.Windows.Forms.ComboBox cboOperator;
        private System.Windows.Forms.RadioButton rbSingle;
        private System.Windows.Forms.RadioButton rbSeries;
    }
}