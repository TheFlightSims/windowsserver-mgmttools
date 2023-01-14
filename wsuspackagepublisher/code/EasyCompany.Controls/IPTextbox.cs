// Decompiled with JetBrains decompiler
// Type: EasyCompany.Controls.IPTextbox
// Assembly: EasyCompany.Controls, Version=1.0.1410.17, Culture=neutral, PublicKeyToken=null
// MVID: 13A03E2B-FBF0-45F8-A0C3-D3E907F194C4
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\EasyCompany.Controls.dll

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace EasyCompany.Controls
{
  [Description("Control that allows to display an IPv4 address or a subnet range.")]
  [DefaultProperty("IPv4Address")]
  [ToolboxBitmap("C:\\Users\\Courtel\\Documents\\Visual Studio 2010\\Projects\\EasyCompany.Controls\\IPTextbox16x16.bmp")]
  [Designer(typeof (IpTextboxDesigner))]
  public class IPTextbox : Control
  {
    private const float round = 8f;
    private const int _width = 125;
    private const int _height = 25;
    private Color _innerBackColor = SystemColors.Window;
    private Color _borderColor = SystemColors.WindowText;
    private SolidBrush _innerBrush = new SolidBrush(SystemColors.Window);
    private SolidBrush _textBrush = new SolidBrush(SystemColors.WindowText);
    private Pen _borderPen = new Pen(SystemColors.WindowText);
    private IPAddress _ipV4Address = new IPAddress(new byte[4]);
    private IPTextbox.AddressTypes _addressType;
    private int _subnet = 32;
    private int _carretPosition;
    private IContainer components;

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool CreateCaret(IntPtr hWnd, IntPtr hBmp, int w, int h);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool SetCaretPos(int x, int y);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool ShowCaret(IntPtr hWnd);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool DestroyCaret();

    public IPTextbox()
    {
      this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
      this.SetStyle(ControlStyles.Opaque, false);
      this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
      this.SetStyle(ControlStyles.UserPaint, true);
      this.SetStyle(ControlStyles.FixedHeight, true);
      this.SetStyle(ControlStyles.FixedWidth, true);
      this.Size = new Size(120, 25);
      this.Cursor = Cursors.IBeam;
      this.IPv4Address = this._ipV4Address;
      this.SubnetMask = 32;
      this.Font = new Font("Microsoft Sans Serif", 10f);
      this.BackColor = Color.Transparent;
      this.ForeColor = SystemColors.WindowText;
      this.InitializeComponent();
    }

    [Category("Apparence")]
    [Description("Get or Set the IPv4Address.")]
    public IPAddress IPv4Address
    {
      get => this._ipV4Address;
      set
      {
        if (value == null || value.AddressFamily != AddressFamily.InterNetwork)
          return;
        this._ipV4Address = value;
        this.Invalidate();
      }
    }

    [Category("Comportement")]
    [DefaultValue(typeof (IPTextbox.AddressTypes), "Single")]
    [Description("Determine wheter this address is a signle IPv4 address or an IPv4 range.")]
    public IPTextbox.AddressTypes AddressType
    {
      get => this._addressType;
      set
      {
        this._addressType = value;
        this.Invalidate();
      }
    }

    [Category("Comportement")]
    [Description("Get or Set the Subnet Mask.")]
    [DefaultValue(32)]
    public int SubnetMask
    {
      get => this._subnet;
      set
      {
        if (value < 0 || value > 32)
          return;
        this._subnet = value;
        this.Invalidate();
      }
    }

    [Browsable(false)]
    public override Color BackColor
    {
      get => base.BackColor;
      set => base.BackColor = value;
    }

    [DefaultValue(typeof (Color), "WindowText")]
    public override Color ForeColor
    {
      get => base.ForeColor;
      set
      {
        this._textBrush = new SolidBrush(value);
        base.ForeColor = value;
      }
    }

    [Browsable(false)]
    public override Image BackgroundImage
    {
      get => base.BackgroundImage;
      set => base.BackgroundImage = value;
    }

    [Browsable(false)]
    public override ImageLayout BackgroundImageLayout
    {
      get => base.BackgroundImageLayout;
      set => base.BackgroundImageLayout = value;
    }

    [Browsable(false)]
    public override string Text
    {
      get => base.Text;
      set => base.Text = value;
    }

    [Browsable(false)]
    public override RightToLeft RightToLeft
    {
      get => base.RightToLeft;
      set => base.RightToLeft = value;
    }

    [DefaultValue(typeof (Cursor), "IBeam")]
    public override Cursor Cursor
    {
      get => base.Cursor;
      set
      {
        base.Cursor = value;
        this.Invalidate();
      }
    }

    [DefaultValue(typeof (Color), "Window")]
    [Category("Apparence")]
    [Description("Backcolor of the control")]
    public Color InnerBackColor
    {
      get => this._innerBackColor;
      set
      {
        this._innerBackColor = value;
        this._innerBrush = new SolidBrush(value);
        this.Invalidate();
      }
    }

    [Category("Apparence")]
    [DefaultValue(typeof (Color), "WindowText")]
    [Description("Color of the border")]
    public Color BorderColor
    {
      get => this._borderColor;
      set
      {
        this._borderColor = value;
        this._borderPen = new Pen(value);
        this.Invalidate();
      }
    }

    [Description("Position of the carret")]
    [DefaultValue(0)]
    [Category("Apparence")]
    public int CarretPosition
    {
      get => this._carretPosition;
      set
      {
        if (value >= 0 && value <= 125)
          this._carretPosition = value;
        this.Invalidate();
      }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
      base.OnPaint(e);
      Rectangle r1 = new Rectangle(0, 0, 125, 25);
      Rectangle r2 = new Rectangle(0, 0, 124, 24);
      GraphicsPath path1 = IPTextbox.RoundRect(r1, 8f);
      GraphicsPath path2 = IPTextbox.RoundRect(r2, 8f);
      if (this.Enabled)
        e.Graphics.FillPath((Brush) new SolidBrush(this._innerBackColor), path1);
      else
        e.Graphics.FillPath((Brush) new SolidBrush(ControlPaint.Dark(this._innerBackColor, 0.05f)), path1);
      e.Graphics.DrawPath(this._borderPen, path2);
      string textToRender = this.GetTextToRender();
      SizeF sizeF = e.Graphics.MeasureString(textToRender, this.Font);
      e.Graphics.DrawString(textToRender, this.Font, (Brush) this._textBrush, (float) (1.0 + (125.0 - (double) sizeF.Width) / 2.0), (float) (0.0 + (25.0 - (double) sizeF.Height) / 2.0));
    }

    protected override void OnSizeChanged(EventArgs e)
    {
      this.Width = 125;
      this.Height = 25;
    }

    protected override void OnGotFocus(EventArgs e)
    {
      this._borderPen.DashStyle = DashStyle.Dash;
      IPTextbox.CreateCaret(this.Handle, IntPtr.Zero, 1, this.Height - 8);
      IPTextbox.SetCaretPos(this.GetHorizontalCaretPosition(), 4);
      IPTextbox.ShowCaret(this.Handle);
      this.Invalidate();
    }

    protected override void OnLostFocus(EventArgs e)
    {
      this._borderPen.DashStyle = DashStyle.Solid;
      IPTextbox.DestroyCaret();
      this.Invalidate();
    }

    protected override void OnEnabledChanged(EventArgs e)
    {
      if (this.Enabled)
        this.Font = new Font(this.Font, FontStyle.Regular);
      else
        this.Font = new Font(this.Font, FontStyle.Italic);
      this.Invalidate();
    }

    protected override void OnClick(EventArgs e)
    {
      this.Focus();
      base.OnClick(e);
    }

    private static GraphicsPath RoundRect(Rectangle r, float r1)
    {
      float x = (float) r.X;
      float y = (float) r.Y;
      float width = (float) r.Width;
      float height = (float) r.Height;
      GraphicsPath graphicsPath = new GraphicsPath();
      graphicsPath.AddArc(x, y, 2f * r1, height, 90f, 180f);
      graphicsPath.AddLine(x + r1, y, (float) ((double) x + (double) width - 2.0 * (double) r1), y);
      graphicsPath.AddArc((float) ((double) x + (double) width - 2.0 * (double) r1), y, 2f * r1, height, -90f, 180f);
      graphicsPath.AddLine(x + r1, y + height, (float) ((double) x + (double) width - 2.0 * (double) r1), y + height);
      return graphicsPath;
    }

    private int GetHorizontalCaretPosition()
    {
      Graphics graphics = this.CreateGraphics();
      string textToRender = this.GetTextToRender();
      string text = textToRender.Substring(0, this.CarretPosition + 1);
      SizeF sizeF = graphics.MeasureString(text, this.Font);
      return (int) ((float) (0.0 + (125.0 - (double) graphics.MeasureString(textToRender, this.Font).Width) / 2.0) + sizeF.Width);
    }

    private string GetTextToRender()
    {
      string textToRender = this.IPv4Address.ToString();
      if (this.AddressType == IPTextbox.AddressTypes.Subnet)
        textToRender = textToRender + "/" + this.SubnetMask.ToString();
      return textToRender;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent() => this.components = (IContainer) new System.ComponentModel.Container();

    public enum AddressTypes
    {
      Single,
      Subnet,
    }
  }
}
