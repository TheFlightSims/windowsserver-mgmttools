// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.XTextWrapper
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\Newtonsoft.Json.dll

using System.Xml.Linq;


#nullable enable
namespace Newtonsoft.Json.Converters
{
  internal class XTextWrapper : XObjectWrapper
  {
    private XText Text => (XText) this.WrappedNode;

    public XTextWrapper(XText text)
      : base((XObject) text)
    {
    }

    public override string? Value
    {
      get => this.Text.Value;
      set => this.Text.Value = value;
    }

    public override IXmlNode? ParentNode => this.Text.Parent == null ? (IXmlNode) null : XContainerWrapper.WrapNode((XObject) this.Text.Parent);
  }
}
