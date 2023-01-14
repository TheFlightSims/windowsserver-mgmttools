// Decompiled with JetBrains decompiler
// Type: EasyCompany.Controls.Localization.Localization
// Assembly: EasyCompany.Controls, Version=1.0.1410.17, Culture=neutral, PublicKeyToken=null
// MVID: 13A03E2B-FBF0-45F8-A0C3-D3E907F194C4
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\EasyCompany.Controls.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace EasyCompany.Controls.Localization
{
  [CompilerGenerated]
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
  [DebuggerNonUserCode]
  internal class Localization
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    internal Localization()
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static ResourceManager ResourceManager
    {
      get
      {
        if (object.ReferenceEquals((object) EasyCompany.Controls.Localization.Localization.resourceMan, (object) null))
          EasyCompany.Controls.Localization.Localization.resourceMan = new ResourceManager("EasyCompany.Controls.Localization.Localization", typeof (EasyCompany.Controls.Localization.Localization).Assembly);
        return EasyCompany.Controls.Localization.Localization.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get => EasyCompany.Controls.Localization.Localization.resourceCulture;
      set => EasyCompany.Controls.Localization.Localization.resourceCulture = value;
    }

    internal static string Position => EasyCompany.Controls.Localization.Localization.ResourceManager.GetString(nameof (Position), EasyCompany.Controls.Localization.Localization.resourceCulture);
  }
}
