// Decompiled with JetBrains decompiler
// Type: EasyCompany.Controls.SearchAndReplace
// Assembly: EasyCompany.Controls, Version=1.0.1410.17, Culture=neutral, PublicKeyToken=null
// MVID: 13A03E2B-FBF0-45F8-A0C3-D3E907F194C4
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\EasyCompany.Controls.dll

using System.ComponentModel;
using System.Windows.Forms;

namespace EasyCompany.Controls
{
  [DefaultProperty("TextToEdit")]
  public class SearchAndReplace : Component
  {
    private FrmSearchAndReplace frmSearchAndReplace = new FrmSearchAndReplace();
    private IContainer components;

    public SearchAndReplace() => this.InitializeComponent();

    public SearchAndReplace(IContainer container)
    {
      container.Add((IComponent) this);
      this.InitializeComponent();
    }

    public DialogResult ShowDialog() => this.frmSearchAndReplace.ShowDialog();

    public DialogResult ShowDialog(IWin32Window owner) => this.frmSearchAndReplace.ShowDialog(owner);

    public void Reset()
    {
      this.frmSearchAndReplace.TextToEdit = string.Empty;
      this.frmSearchAndReplace.TextToSearchFor = string.Empty;
      this.frmSearchAndReplace.ReplacementText = string.Empty;
      this.frmSearchAndReplace.IgnoreCase = false;
    }

    [Description("This is the text where the search and replace will occurs.")]
    [DefaultValue("This is the text where to search for occurrences.")]
    [Category("Comportement")]
    public string TextToEdit
    {
      get => this.frmSearchAndReplace.TextToEdit;
      set
      {
        if (string.IsNullOrEmpty(value))
          return;
        this.frmSearchAndReplace.TextToEdit = value;
      }
    }

    [DefaultValue("")]
    [Category("Comportement")]
    [Description("This is the text to search in the 'Text to edit'.")]
    public string TextToSearchFor
    {
      get => this.frmSearchAndReplace.TextToSearchFor;
      set
      {
        if (string.IsNullOrEmpty(value))
          return;
        this.frmSearchAndReplace.TextToSearchFor = value;
      }
    }

    [Description("This is the text to insert instead of the found text.")]
    [Category("Comportement")]
    [DefaultValue("")]
    public string ReplacementText
    {
      get => this.frmSearchAndReplace.ReplacementText;
      set
      {
        if (value == null)
          return;
        this.frmSearchAndReplace.ReplacementText = value;
      }
    }

    [Category("Comportement")]
    [Description("Allows you to search ignoring case.")]
    [DefaultValue(false)]
    public bool IgnoreCase
    {
      get => this.frmSearchAndReplace.IgnoreCase;
      set => this.frmSearchAndReplace.IgnoreCase = value;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent() => this.components = (IContainer) new System.ComponentModel.Container();
  }
}
