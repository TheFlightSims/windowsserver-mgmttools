// Decompiled with JetBrains decompiler
// Type: EasyCompany.Controls.FilteredTextbox
// Assembly: EasyCompany.Controls, Version=1.0.1410.17, Culture=neutral, PublicKeyToken=null
// MVID: 13A03E2B-FBF0-45F8-A0C3-D3E907F194C4
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\EasyCompany.Controls.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace EasyCompany.Controls
{
  [ToolboxBitmap("C:\\Users\\Courtel\\Documents\\Visual Studio 2010\\Projects\\EasyCompany.Controls\\FilteredTextbox.bmp")]
  [Description("Textbox that allows to filter inputs.")]
  [DefaultProperty("FilterMethod")]
  public class FilteredTextbox : TextBox
  {
    private List<char> _alwaysAllowedCharacters = new List<char>();
    private FilteredTextbox.FilterMethods _filterMethod = FilteredTextbox.FilterMethods.DisallowedCharacters;
    private string _regExprFilter = string.Empty;
    private List<char> _allowedCharacters = new List<char>();
    private List<char> _disallowedCharacters = new List<char>();
    private Regex _regExpr = new Regex(string.Empty);

    public FilteredTextbox()
    {
      this._alwaysAllowedCharacters.Add('\b');
      this._alwaysAllowedCharacters.Add('\u0003');
      this._alwaysAllowedCharacters.Add('\u0016');
      this._alwaysAllowedCharacters.Add('\u0018');
      this._alwaysAllowedCharacters.Add('\r');
    }

    [Category("Comportement")]
    [Description("Determine the method used to filter characters typed by the user.")]
    public FilteredTextbox.FilterMethods FilterMethod
    {
      get => this._filterMethod;
      set => this._filterMethod = value;
    }

    [Description("The Regular Expression used to filter characters when the Filter Method is set to 'RegExpr'.")]
    [Category("Comportement")]
    [DefaultValue(typeof (string), "")]
    public string RegExpr
    {
      get => this._regExprFilter;
      set
      {
        if (value == null)
          return;
        this._regExprFilter = value;
        try
        {
          this._regExpr = new Regex(value);
        }
        catch (Exception ex)
        {
        }
      }
    }

    [Description("Characters that are allowed when the Filter Method is set to 'AllowedCharacters'.")]
    [Category("Comportement")]
    public List<char> AllowedCharacters => this._allowedCharacters;

    [Category("Comportement")]
    [Description("Characters that are not allowed when the Filter Method is set to 'DisallowedCharacters'.")]
    public List<char> DisallowedCharacters => this._disallowedCharacters;

    public string FilterText(string textToFilter)
    {
      StringBuilder stringBuilder = new StringBuilder();
      try
      {
        foreach (char pressedKey in textToFilter)
        {
          if (this.IsAllowedCharacter(pressedKey))
            stringBuilder.Append(pressedKey);
        }
        return stringBuilder.ToString();
      }
      catch (Exception ex)
      {
      }
      return (string) null;
    }

    protected override void OnKeyPress(KeyPressEventArgs e)
    {
      char keyChar = e.KeyChar;
      if (!this._alwaysAllowedCharacters.Contains(keyChar) && !this.IsAllowedCharacter(keyChar))
        e.Handled = true;
      if (keyChar == '\u0016')
      {
        if (Clipboard.ContainsText())
        {
          try
          {
            string text = Clipboard.GetText();
            int selectionStart = this.SelectionStart;
            int startIndex = this.SelectionStart + this.SelectionLength;
            string str = this.FilterText(text);
            if (str != null)
              this.Text = this.Text.Substring(0, selectionStart) + str + this.Text.Substring(startIndex);
            this.Select(selectionStart + str.Length, 0);
          }
          catch (Exception ex)
          {
          }
          e.Handled = true;
        }
      }
      base.OnKeyPress(e);
    }

    private bool IsAllowedCharacter(char pressedKey)
    {
      switch (this.FilterMethod)
      {
        case FilteredTextbox.FilterMethods.RegExpr:
          try
          {
            if (this._regExpr.IsMatch(pressedKey.ToString()))
              return true;
            break;
          }
          catch (Exception ex)
          {
            break;
          }
        case FilteredTextbox.FilterMethods.AllowedCharacters:
          if (this.AllowedCharacters.Contains(pressedKey))
            return true;
          break;
        case FilteredTextbox.FilterMethods.DisallowedCharacters:
          if (!this.DisallowedCharacters.Contains(pressedKey))
            return true;
          break;
      }
      return false;
    }

    public enum FilterMethods
    {
      RegExpr,
      AllowedCharacters,
      DisallowedCharacters,
    }
  }
}
