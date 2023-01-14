// Decompiled with JetBrains decompiler
// Type: EasyCompany.Controls.FrmSearchAndReplace
// Assembly: EasyCompany.Controls, Version=1.0.1410.17, Culture=neutral, PublicKeyToken=null
// MVID: 13A03E2B-FBF0-45F8-A0C3-D3E907F194C4
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\EasyCompany.Controls.dll

using System;
using System.ComponentModel;
using System.Media;
using System.Resources;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace EasyCompany.Controls
{
  internal class FrmSearchAndReplace : Form
  {
    private int currentPos;
    private ResourceManager resMan = new ResourceManager("EasyCompany.Controls.Localization.Localization", typeof (FrmSearchAndReplace).Assembly);
    private IContainer components;
    private Label lblIndex;
    private CheckBox chkBxIgnoreCase;
    private Button btnReplaceAll;
    private Button btnReplace;
    private Button btnPrevious;
    private Button btnNext;
    private TextBox txtBxSourceText;
    private TextBox txtBxReplacementText;
    private Label label2;
    private TextBox txtBxTextToSearch;
    private Label label1;
    private Button btnOk;
    private Button btnCancel;

    internal FrmSearchAndReplace()
    {
      this.InitializeComponent();
      this.SetButtonEnableness();
      this.txtBxTextToSearch.Focus();
    }

    internal string TextToEdit
    {
      get => this.txtBxSourceText.Text;
      set
      {
        if (value == null)
          return;
        this.CursorPosition = 0;
        this.txtBxSourceText.Text = value;
        this.txtBxSourceText.Refresh();
        this.SetButtonEnableness();
      }
    }

    internal string TextToSearchFor
    {
      get => this.txtBxTextToSearch.Text;
      set
      {
        if (value == null)
          return;
        this.txtBxTextToSearch.Text = value;
        this.SetButtonEnableness();
      }
    }

    internal string ReplacementText
    {
      get => this.txtBxReplacementText.Text;
      set
      {
        if (value == null)
          return;
        this.txtBxReplacementText.Text = value;
        this.SetButtonEnableness();
      }
    }

    internal bool IgnoreCase
    {
      get => this.chkBxIgnoreCase.Checked;
      set => this.chkBxIgnoreCase.Checked = value;
    }

    internal int CursorPosition
    {
      get => this.currentPos;
      set
      {
        if (value < 0 || value >= this.TextToEdit.Length)
          return;
        this.currentPos = value;
        this.lblIndex.Text = this.GetLocalizedString("Position") + value.ToString();
        if (this.txtBxSourceText.SelectionStart == value)
          return;
        this.txtBxSourceText.SelectionStart = value;
        this.txtBxSourceText.SelectionLength = 0;
      }
    }

    internal string SelectedText => this.txtBxSourceText.SelectedText;

    private int SearchPreviousOccurrence(int position)
    {
      int num = -1;
      try
      {
        num = this.txtBxSourceText.Text.LastIndexOf(this.txtBxTextToSearch.Text, position, this.IgnoreCase ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture);
      }
      catch (Exception ex)
      {
      }
      return num;
    }

    private int SearchNextOccurrence(int position)
    {
      int num = -1;
      try
      {
        num = this.txtBxSourceText.Text.IndexOf(this.txtBxTextToSearch.Text, position, this.IgnoreCase ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture);
      }
      catch (Exception ex)
      {
      }
      return num;
    }

    private void SelectText(int index)
    {
      this.txtBxSourceText.Select(index, this.TextToSearchFor.Length);
      this.txtBxSourceText.ScrollToCaret();
    }

    private string ReplaceOneOccurrence(
      string textToEdit,
      string replacementText,
      int position,
      int length)
    {
      try
      {
        StringBuilder stringBuilder = new StringBuilder(textToEdit.Substring(0, position));
        stringBuilder.Append(replacementText);
        stringBuilder.Append(textToEdit.Substring(position + length));
        textToEdit = stringBuilder.ToString();
      }
      catch (Exception ex)
      {
      }
      return textToEdit;
    }

    private string ReplaceAllOccurrences(
      string textToEdit,
      string textToReplace,
      string replacementText)
    {
      try
      {
        textToEdit = Regex.Replace(textToEdit, Regex.Escape(textToReplace), replacementText, this.IgnoreCase ? RegexOptions.IgnoreCase : RegexOptions.None);
      }
      catch (Exception ex)
      {
      }
      return textToEdit;
    }

    private void SetButtonEnableness()
    {
      bool flag1 = !string.IsNullOrEmpty(this.txtBxTextToSearch.Text);
      bool flag2 = !string.IsNullOrEmpty(this.txtBxReplacementText.Text);
      this.btnPrevious.Enabled = flag1;
      this.btnNext.Enabled = flag1;
      this.btnReplace.Enabled = flag2 && flag1;
      this.btnReplaceAll.Enabled = flag2 && flag1;
    }

    private string GetLocalizedString(string unlocalizedString)
    {
      string empty = string.Empty;
      try
      {
        string localizedString = this.resMan.GetString(unlocalizedString);
        if (!string.IsNullOrEmpty(localizedString))
          return localizedString;
      }
      catch (Exception ex)
      {
      }
      return "Missing_Localized_String_For(" + (unlocalizedString != null ? unlocalizedString : "null") + ")";
    }

    private void txtBxTextToSearch_TextChanged(object sender, EventArgs e) => this.SetButtonEnableness();

    private void txtBxReplacementText_TextChanged(object sender, EventArgs e) => this.SetButtonEnableness();

    private void btnPrevious_Click(object sender, EventArgs e)
    {
      try
      {
        if (string.IsNullOrEmpty(this.txtBxTextToSearch.Text))
          return;
        int index = this.SearchPreviousOccurrence(!string.IsNullOrEmpty(this.txtBxSourceText.SelectedText) ? Math.Max(0, this.CursorPosition - 1) : this.CursorPosition);
        if (index != -1 && this.CursorPosition != 0)
        {
          this.CursorPosition = index;
          this.SelectText(index);
          this.btnNext.Enabled = true;
        }
        else
        {
          this.btnPrevious.Enabled = false;
          this.txtBxSourceText.SelectionLength = 0;
          SystemSounds.Asterisk.Play();
        }
      }
      catch (Exception ex)
      {
      }
    }

    private void btnNext_Click(object sender, EventArgs e)
    {
      try
      {
        if (string.IsNullOrEmpty(this.txtBxTextToSearch.Text))
          return;
        int index = this.SearchNextOccurrence(!string.IsNullOrEmpty(this.txtBxSourceText.SelectedText) ? this.CursorPosition + 1 : this.CursorPosition);
        if (index != -1)
        {
          this.CursorPosition = index;
          this.SelectText(index);
          this.btnPrevious.Enabled = true;
        }
        else
        {
          this.btnNext.Enabled = false;
          if (!string.IsNullOrEmpty(this.txtBxSourceText.SelectedText))
            this.CursorPosition = this.txtBxSourceText.SelectionStart + this.txtBxSourceText.SelectionLength;
          this.txtBxSourceText.SelectionLength = 0;
          SystemSounds.Asterisk.Play();
        }
      }
      catch (Exception ex)
      {
      }
    }

    private void txtBxSourceText_Click(object sender, EventArgs e)
    {
      this.CursorPosition = this.txtBxSourceText.SelectionStart;
      this.SetButtonEnableness();
    }

    private void btnReplace_Click(object sender, EventArgs e)
    {
      if (string.Compare(this.txtBxSourceText.SelectedText, this.txtBxTextToSearch.Text, this.chkBxIgnoreCase.Checked) == 0)
        this.txtBxSourceText.Text = this.ReplaceOneOccurrence(this.txtBxSourceText.Text, this.txtBxReplacementText.Text, this.txtBxSourceText.SelectionStart, this.txtBxSourceText.SelectionLength);
      this.btnNext.PerformClick();
    }

    private void btnReplaceAll_Click(object sender, EventArgs e) => this.TextToEdit = this.ReplaceAllOccurrences(this.txtBxSourceText.Text, this.txtBxTextToSearch.Text, this.txtBxReplacementText.Text);

    private void txtBxSourceText_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.Alt)
        return;
      this.CursorPosition = this.txtBxSourceText.SelectionStart;
      this.SetButtonEnableness();
    }

    private void txtBxSourceText_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyData != (Keys.A | Keys.Control))
        return;
      this.txtBxSourceText.SelectAll();
      this.SetButtonEnableness();
      e.Handled = true;
      e.SuppressKeyPress = true;
    }

    private void btnOk_Click(object sender, EventArgs e) => this.DialogResult = DialogResult.OK;

    private void btnCancel_Click(object sender, EventArgs e) => this.DialogResult = DialogResult.Cancel;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmSearchAndReplace));
      this.lblIndex = new Label();
      this.chkBxIgnoreCase = new CheckBox();
      this.btnReplaceAll = new Button();
      this.btnReplace = new Button();
      this.btnPrevious = new Button();
      this.btnNext = new Button();
      this.txtBxSourceText = new TextBox();
      this.txtBxReplacementText = new TextBox();
      this.label2 = new Label();
      this.txtBxTextToSearch = new TextBox();
      this.label1 = new Label();
      this.btnOk = new Button();
      this.btnCancel = new Button();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.lblIndex, "lblIndex");
      this.lblIndex.Name = "lblIndex";
      componentResourceManager.ApplyResources((object) this.chkBxIgnoreCase, "chkBxIgnoreCase");
      this.chkBxIgnoreCase.Name = "chkBxIgnoreCase";
      this.chkBxIgnoreCase.UseVisualStyleBackColor = true;
      componentResourceManager.ApplyResources((object) this.btnReplaceAll, "btnReplaceAll");
      this.btnReplaceAll.Name = "btnReplaceAll";
      this.btnReplaceAll.UseVisualStyleBackColor = true;
      this.btnReplaceAll.Click += new EventHandler(this.btnReplaceAll_Click);
      componentResourceManager.ApplyResources((object) this.btnReplace, "btnReplace");
      this.btnReplace.Name = "btnReplace";
      this.btnReplace.UseVisualStyleBackColor = true;
      this.btnReplace.Click += new EventHandler(this.btnReplace_Click);
      componentResourceManager.ApplyResources((object) this.btnPrevious, "btnPrevious");
      this.btnPrevious.Name = "btnPrevious";
      this.btnPrevious.UseVisualStyleBackColor = true;
      this.btnPrevious.Click += new EventHandler(this.btnPrevious_Click);
      componentResourceManager.ApplyResources((object) this.btnNext, "btnNext");
      this.btnNext.Name = "btnNext";
      this.btnNext.UseVisualStyleBackColor = true;
      this.btnNext.Click += new EventHandler(this.btnNext_Click);
      this.txtBxSourceText.AcceptsReturn = true;
      componentResourceManager.ApplyResources((object) this.txtBxSourceText, "txtBxSourceText");
      this.txtBxSourceText.HideSelection = false;
      this.txtBxSourceText.Name = "txtBxSourceText";
      this.txtBxSourceText.Click += new EventHandler(this.txtBxSourceText_Click);
      this.txtBxSourceText.KeyDown += new KeyEventHandler(this.txtBxSourceText_KeyDown);
      this.txtBxSourceText.KeyUp += new KeyEventHandler(this.txtBxSourceText_KeyUp);
      componentResourceManager.ApplyResources((object) this.txtBxReplacementText, "txtBxReplacementText");
      this.txtBxReplacementText.Name = "txtBxReplacementText";
      this.txtBxReplacementText.TextChanged += new EventHandler(this.txtBxReplacementText_TextChanged);
      componentResourceManager.ApplyResources((object) this.label2, "label2");
      this.label2.Name = "label2";
      componentResourceManager.ApplyResources((object) this.txtBxTextToSearch, "txtBxTextToSearch");
      this.txtBxTextToSearch.Name = "txtBxTextToSearch";
      this.txtBxTextToSearch.TextChanged += new EventHandler(this.txtBxTextToSearch_TextChanged);
      componentResourceManager.ApplyResources((object) this.label1, "label1");
      this.label1.Name = "label1";
      componentResourceManager.ApplyResources((object) this.btnOk, "btnOk");
      this.btnOk.Name = "btnOk";
      this.btnOk.UseVisualStyleBackColor = true;
      this.btnOk.Click += new EventHandler(this.btnOk_Click);
      componentResourceManager.ApplyResources((object) this.btnCancel, "btnCancel");
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ControlBox = false;
      this.Controls.Add((Control) this.lblIndex);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOk);
      this.Controls.Add((Control) this.chkBxIgnoreCase);
      this.Controls.Add((Control) this.btnReplaceAll);
      this.Controls.Add((Control) this.btnReplace);
      this.Controls.Add((Control) this.btnPrevious);
      this.Controls.Add((Control) this.btnNext);
      this.Controls.Add((Control) this.txtBxSourceText);
      this.Controls.Add((Control) this.txtBxReplacementText);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.txtBxTextToSearch);
      this.Controls.Add((Control) this.label1);
      this.Name = nameof (FrmSearchAndReplace);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
