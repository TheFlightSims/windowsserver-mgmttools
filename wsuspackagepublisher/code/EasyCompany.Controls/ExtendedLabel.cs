// Decompiled with JetBrains decompiler
// Type: EasyCompany.Controls.ExtendedLabel
// Assembly: EasyCompany.Controls, Version=1.0.1410.17, Culture=neutral, PublicKeyToken=null
// MVID: 13A03E2B-FBF0-45F8-A0C3-D3E907F194C4
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\EasyCompany.Controls.dll

using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace EasyCompany.Controls
{
  public class ExtendedLabel : Label
  {
    private const int WM_LBUTTONDCLICK = 515;
    private string clipboardText;

    [DefaultValue(false)]
    [Description("Overrides default behavior of Label to copy label text to clipboard on double click")]
    public bool CopyTextOnDoubleClick { get; set; }

    protected override void OnDoubleClick(EventArgs e)
    {
      if (!string.IsNullOrEmpty(this.clipboardText))
        Clipboard.SetData(DataFormats.Text, (object) this.clipboardText);
      this.clipboardText = (string) null;
      base.OnDoubleClick(e);
    }

    protected override void WndProc(ref Message m)
    {
      if (!this.CopyTextOnDoubleClick && m.Msg == 515)
      {
        IDataObject dataObject = Clipboard.GetDataObject();
        if (dataObject.GetDataPresent(DataFormats.Text))
          this.clipboardText = (string) dataObject.GetData(DataFormats.Text);
      }
      base.WndProc(ref m);
    }
  }
}
