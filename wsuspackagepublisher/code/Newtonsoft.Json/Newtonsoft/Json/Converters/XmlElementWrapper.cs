// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.XmlElementWrapper
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\Newtonsoft.Json.dll

using System.Xml;


#nullable enable
namespace Newtonsoft.Json.Converters
{
  internal class XmlElementWrapper : XmlNodeWrapper, IXmlElement, IXmlNode
  {
    private readonly XmlElement _element;

    public XmlElementWrapper(XmlElement element)
      : base((XmlNode) element)
    {
      this._element = element;
    }

    public void SetAttributeNode(IXmlNode attribute) => this._element.SetAttributeNode((XmlAttribute) ((XmlNodeWrapper) attribute).WrappedNode);

    public string GetPrefixOfNamespace(string namespaceUri) => this._element.GetPrefixOfNamespace(namespaceUri);

    public bool IsEmpty => this._element.IsEmpty;
  }
}
