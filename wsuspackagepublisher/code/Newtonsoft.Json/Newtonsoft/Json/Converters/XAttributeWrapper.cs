// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.XAttributeWrapper
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\Newtonsoft.Json.dll

using System.Xml.Linq;


#nullable enable
namespace Newtonsoft.Json.Converters
{
  internal class XAttributeWrapper : XObjectWrapper
  {
    private XAttribute Attribute => (XAttribute) this.WrappedNode;

    public XAttributeWrapper(XAttribute attribute)
      : base((XObject) attribute)
    {
    }

    public override string? Value
    {
      get => this.Attribute.Value;
      set => this.Attribute.Value = value;
    }

    public override string? LocalName => this.Attribute.Name.LocalName;

    public override string? NamespaceUri => this.Attribute.Name.NamespaceName;

    public override IXmlNode? ParentNode => this.Attribute.Parent == null ? (IXmlNode) null : XContainerWrapper.WrapNode((XObject) this.Attribute.Parent);
  }
}
