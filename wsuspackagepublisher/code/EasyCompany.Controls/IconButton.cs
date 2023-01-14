// Decompiled with JetBrains decompiler
// Type: EasyCompany.Controls.IconButton
// Assembly: EasyCompany.Controls, Version=1.0.1410.17, Culture=neutral, PublicKeyToken=null
// MVID: 13A03E2B-FBF0-45F8-A0C3-D3E907F194C4
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\EasyCompany.Controls.dll

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace EasyCompany.Controls
{
  [Description("A button that can display an image at left of the text.")]
  [Designer(typeof (IconButtonDesigner))]
  [ToolboxBitmap("C:\\Users\\Courtel\\Documents\\Visual Studio 2010\\Projects\\EasyCompany.Controls\\IconButton16x16.bmp")]
  public class IconButton : Control
  {
    protected static int EDGE_PADDING = 4;
    protected static int round = 4;
    protected ButtonState buttonState;
    protected Icon buttonIcon;
    protected int iconDrawWidth;
    protected bool mousePressed;
    protected bool mouseOnControl;

    [Category("Appearance")]
    [DefaultValue(null)]
    [Description("The Icon to be displayed in the button")]
    public Icon Icon
    {
      get => this.buttonIcon;
      set
      {
        this.buttonIcon = value;
        this.Invalidate();
        this.Update();
      }
    }

    public IconButton() => this.InitializeComponent();

    private void InitializeComponent()
    {
      this.buttonIcon = (Icon) null;
      this.buttonState = ButtonState.Normal;
      this.mousePressed = false;
      this.mouseOnControl = false;
    }

    protected override void OnGotFocus(EventArgs e)
    {
      this.Invalidate();
      base.OnGotFocus(e);
    }

    protected override void OnLostFocus(EventArgs e)
    {
      this.Invalidate();
      base.OnLostFocus(e);
    }

    protected override void OnTextChanged(EventArgs e)
    {
      this.Invalidate();
      this.Update();
      base.OnTextChanged(e);
    }

    protected override void OnSizeChanged(EventArgs e)
    {
      base.OnSizeChanged(e);
      this.Invalidate();
      this.Update();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
      base.OnPaint(e);
      this.Draw(e.Graphics);
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
      if (e.Button == MouseButtons.Left)
      {
        this.Focus();
        this.Capture = true;
        this.buttonState = ButtonState.Pushed;
        this.mousePressed = true;
        this.Invalidate();
        this.Update();
      }
      else
        base.OnMouseDown(e);
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
      if (this.mousePressed && e.Button == MouseButtons.Left)
      {
        this.Capture = false;
        this.buttonState = ButtonState.Normal;
        this.Invalidate();
        this.Update();
      }
      else
        base.OnMouseUp(e);
      this.mousePressed = false;
    }

    protected override void OnMouseEnter(EventArgs e)
    {
      base.OnMouseEnter(e);
      this.mouseOnControl = true;
      this.Invalidate();
    }

    protected override void OnMouseLeave(EventArgs e)
    {
      base.OnMouseLeave(e);
      this.mouseOnControl = false;
      this.Invalidate();
    }

    protected virtual void Draw(Graphics g)
    {
      this.DrawButton(g);
      if (this.buttonIcon != null)
        this.DrawIcon(g);
      this.DrawText(g);
      if (!this.Focused)
        return;
      this.DrawFocusClues(g);
    }

    protected virtual void DrawButton(Graphics g)
    {
      Rectangle rectangle = new Rectangle(0, 0, this.Width, this.Height);
      Rectangle r = new Rectangle(0, 0, this.Width - 1, this.Height - 1);
      if (this.Focused)
        rectangle.Inflate(-1, -1);
      GraphicsPath path1 = IconButton.RoundRect(rectangle, (float) IconButton.round, (float) IconButton.round, (float) IconButton.round, (float) IconButton.round);
      GraphicsPath path2 = IconButton.RoundRect(r, (float) IconButton.round, (float) IconButton.round, (float) IconButton.round, (float) IconButton.round);
      LinearGradientBrush linearGradientBrush = this.Enabled ? (this.buttonState != ButtonState.Normal ? new LinearGradientBrush(rectangle, Color.AliceBlue, Color.SteelBlue, LinearGradientMode.Vertical) : (!this.mouseOnControl ? new LinearGradientBrush(rectangle, Color.White, Color.AliceBlue, LinearGradientMode.Vertical) : new LinearGradientBrush(rectangle, Color.AliceBlue, Color.LightSteelBlue, LinearGradientMode.Vertical))) : new LinearGradientBrush(rectangle, Color.Silver, Color.LightGray, LinearGradientMode.Vertical);
      Pen pen = !this.Focused ? new Pen(Color.Black, 1f) : (this.buttonState != ButtonState.Normal ? new Pen(Color.DarkOrange, 1f) : new Pen(Color.DeepSkyBlue, 1f));
      g.FillPath((Brush) linearGradientBrush, path1);
      g.DrawPath(pen, path2);
    }

    private static GraphicsPath RoundRect(Rectangle r, float r1, float r2, float r3, float r4)
    {
      float x = (float) r.X;
      float y = (float) r.Y;
      float width = (float) r.Width;
      float height = (float) r.Height;
      GraphicsPath graphicsPath = new GraphicsPath();
      graphicsPath.AddBezier(x, y + r1, x, y, x + r1, y, x + r1, y);
      graphicsPath.AddLine(x + r1, y, x + width - r2, y);
      graphicsPath.AddBezier(x + width - r2, y, x + width, y, x + width, y + r2, x + width, y + r2);
      graphicsPath.AddLine(x + width, y + r2, x + width, y + height - r3);
      graphicsPath.AddBezier(x + width, y + height - r3, x + width, y + height, x + width - r3, y + height, x + width - r3, y + height);
      graphicsPath.AddLine(x + width - r3, y + height, x + r4, y + height);
      graphicsPath.AddBezier(x + r4, y + height, x, y + height, x, y + height - r4, x, y + height - r4);
      graphicsPath.AddLine(x, y + height - r4, x, y + r1);
      return graphicsPath;
    }

    protected virtual void DrawText(Graphics g)
    {
      int x = this.buttonIcon == null ? IconButton.EDGE_PADDING : this.iconDrawWidth + IconButton.EDGE_PADDING;
      int width = this.Width - x;
      int edgePadding = IconButton.EDGE_PADDING;
      int height = this.Height - 2 * IconButton.EDGE_PADDING;
      RectangleF layoutRectangle = new RectangleF((float) x, (float) edgePadding, (float) width, (float) height);
      if (ButtonState.Pushed == this.buttonState)
        layoutRectangle.Offset(1f, 1f);
      StringFormat format = new StringFormat();
      format.Alignment = StringAlignment.Center;
      format.LineAlignment = StringAlignment.Center;
      SolidBrush solidBrush = !this.Enabled ? new SolidBrush(Color.Gray) : new SolidBrush(this.ForeColor);
      g.DrawString(this.Text, this.Font, (Brush) solidBrush, layoutRectangle, format);
      solidBrush.Dispose();
    }

    protected virtual void DrawIcon(Graphics g)
    {
      int y = this.Height / 2 - this.buttonIcon.Height / 2;
      int height = this.buttonIcon.Height;
      int width = this.buttonIcon.Width;
      if (y + height >= this.Height)
      {
        y = IconButton.EDGE_PADDING;
        int num1 = this.Height - 2 * IconButton.EDGE_PADDING;
        float num2 = (float) num1 / (float) height;
        width = (int) ((double) width * (double) num2);
        height = num1;
      }
      Rectangle targetRect = new Rectangle(IconButton.EDGE_PADDING, y, width, height);
      if (this.buttonState == ButtonState.Pushed)
        targetRect.Offset(1, 1);
      g.DrawIcon(this.buttonIcon, targetRect);
      this.iconDrawWidth = targetRect.Width;
    }

    protected virtual void DrawFocusClues(Graphics g)
    {
      int num = 1;
      Pen pen = new Pen(Color.Black, 1f);
      GraphicsPath path = IconButton.RoundRect(new Rectangle(num, num, this.Width - 2 * num - 1, this.Height - 2 * num - 1), (float) IconButton.round, (float) IconButton.round, (float) IconButton.round, (float) IconButton.round);
      pen.DashStyle = DashStyle.Dot;
      g.DrawPath(pen, path);
      pen.Dispose();
    }
  }
}
