// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.XElementWrapper
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System.Collections.Generic;
using System.Xml.Linq;


#nullable enable
namespace Newtonsoft.Json.Converters
{
  internal class XElementWrapper : XContainerWrapper, IXmlElement, IXmlNode
  {
    private List<IXmlNode>? _attributes;

    private XElement Element => (XElement) this.WrappedNode;

    public XElementWrapper(XElement element)
      : base((XContainer) element)
    {
    }

    public void SetAttributeNode(IXmlNode attribute)
    {
      this.Element.Add(((XObjectWrapper) attribute).WrappedNode);
      this._attributes = (List<IXmlNode>) null;
    }

    public override List<IXmlNode> Attributes
    {
      get
      {
        if (this._attributes == null)
        {
          if (!this.Element.HasAttributes && !this.HasImplicitNamespaceAttribute(this.NamespaceUri))
          {
            this._attributes = XmlNodeConverter.EmptyChildNodes;
          }
          else
          {
            this._attributes = new List<IXmlNode>();
            foreach (XAttribute attribute in this.Element.Attributes())
              this._attributes.Add((IXmlNode) new XAttributeWrapper(attribute));
            string namespaceUri = this.NamespaceUri;
            if (this.HasImplicitNamespaceAttribute(namespaceUri))
              this._attributes.Insert(0, (IXmlNode) new XAttributeWrapper(new XAttribute((XName) "xmlns", (object) namespaceUri)));
          }
        }
        return this._attributes;
      }
    }

    private bool HasImplicitNamespaceAttribute(string namespaceUri)
    {
      if (!StringUtils.IsNullOrEmpty(namespaceUri) && namespaceUri != this.ParentNode?.NamespaceUri && StringUtils.IsNullOrEmpty(this.GetPrefixOfNamespace(namespaceUri)))
      {
        bool flag = false;
        if (this.Element.HasAttributes)
        {
          foreach (XAttribute attribute in this.Element.Attributes())
          {
            if (attribute.Name.LocalName == "xmlns" && StringUtils.IsNullOrEmpty(attribute.Name.NamespaceName) && attribute.Value == namespaceUri)
              flag = true;
          }
        }
        if (!flag)
          return true;
      }
      return false;
    }

    public override IXmlNode AppendChild(IXmlNode newChild)
    {
      IXmlNode xmlNode = base.AppendChild(newChild);
      this._attributes = (List<IXmlNode>) null;
      return xmlNode;
    }

    public override string? Value
    {
      get => this.Element.Value;
      set => this.Element.Value = value;
    }

    public override string? LocalName => this.Element.Name.LocalName;

    public override string? NamespaceUri => this.Element.Name.NamespaceName;

    public string GetPrefixOfNamespace(string namespaceUri) => this.Element.GetPrefixOfNamespace((XNamespace) namespaceUri);

    public bool IsEmpty => this.Element.IsEmpty;
  }
}
