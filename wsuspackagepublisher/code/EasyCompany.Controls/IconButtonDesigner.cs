// Decompiled with JetBrains decompiler
// Type: EasyCompany.Controls.IconButtonDesigner
// Assembly: EasyCompany.Controls, Version=1.0.1410.17, Culture=neutral, PublicKeyToken=null
// MVID: 13A03E2B-FBF0-45F8-A0C3-D3E907F194C4
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\EasyCompany.Controls.dll

using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms.Design;

namespace EasyCompany.Controls
{
  internal class IconButtonDesigner : ControlDesigner
  {
    private DesignerVerb[] verbs;

    internal IconButtonDesigner()
    {
    }

    public override DesignerVerbCollection Verbs
    {
      get
      {
        if (this.verbs == null)
        {
          this.verbs = new DesignerVerb[3];
          this.verbs[0] = new DesignerVerb("Red Text", new EventHandler(this.OnRedVerb));
          this.verbs[1] = new DesignerVerb("Green Text", new EventHandler(this.OnGreenVerb));
          this.verbs[2] = new DesignerVerb("Blue Text", new EventHandler(this.OnBlueVerb));
        }
        return new DesignerVerbCollection(this.verbs);
      }
    }

    protected override void PostFilterProperties(IDictionary properties)
    {
      properties.Remove((object) "BackgroundImage");
      properties.Remove((object) "BackgroundImageLayout");
      properties.Remove((object) "BackColor");
    }

    protected void OnRedVerb(object sender, EventArgs e) => TypeDescriptor.GetProperties((object) this.Control)["ForeColor"]?.SetValue((object) this.Control, (object) Color.Red);

    protected void OnGreenVerb(object sender, EventArgs e) => TypeDescriptor.GetProperties((object) this.Control)["ForeColor"]?.SetValue((object) this.Control, (object) Color.Green);

    protected void OnBlueVerb(object sender, EventArgs e) => TypeDescriptor.GetProperties((object) this.Control)["ForeColor"]?.SetValue((object) this.Control, (object) Color.Blue);
  }
}
