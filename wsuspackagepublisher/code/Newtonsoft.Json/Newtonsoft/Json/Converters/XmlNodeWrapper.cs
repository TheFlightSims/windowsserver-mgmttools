// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.XmlNodeWrapper
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\Newtonsoft.Json.dll

using System.Collections.Generic;
using System.Xml;


#nullable enable
namespace Newtonsoft.Json.Converters
{
  internal class XmlNodeWrapper : IXmlNode
  {
    private readonly XmlNode _node;
    private List<IXmlNode>? _childNodes;
    private List<IXmlNode>? _attributes;

    public XmlNodeWrapper(XmlNode node) => this._node = node;

    public object? WrappedNode => (object) this._node;

    public XmlNodeType NodeType => this._node.NodeType;

    public virtual string? LocalName => this._node.LocalName;

    public List<IXmlNode> ChildNodes
    {
      get
      {
        if (this._childNodes == null)
        {
          if (!this._node.HasChildNodes)
          {
            this._childNodes = XmlNodeConverter.EmptyChildNodes;
          }
          else
          {
            this._childNodes = new List<IXmlNode>(this._node.ChildNodes.Count);
            foreach (XmlNode childNode in this._node.ChildNodes)
              this._childNodes.Add(XmlNodeWrapper.WrapNode(childNode));
          }
        }
        return this._childNodes;
      }
    }

    protected virtual bool HasChildNodes => this._node.HasChildNodes;

    internal static IXmlNode WrapNode(XmlNode node)
    {
      switch (node.NodeType)
      {
        case XmlNodeType.Element:
          return (IXmlNode) new XmlElementWrapper((XmlElement) node);
        case XmlNodeType.DocumentType:
          return (IXmlNode) new XmlDocumentTypeWrapper((XmlDocumentType) node);
        case XmlNodeType.XmlDeclaration:
          return (IXmlNode) new XmlDeclarationWrapper((XmlDeclaration) node);
        default:
          return (IXmlNode) new XmlNodeWrapper(node);
      }
    }

    public List<IXmlNode> Attributes
    {
      get
      {
        if (this._attributes == null)
        {
          if (!this.HasAttributes)
          {
            this._attributes = XmlNodeConverter.EmptyChildNodes;
          }
          else
          {
            this._attributes = new List<IXmlNode>(this._node.Attributes.Count);
            foreach (XmlNode attribute in (XmlNamedNodeMap) this._node.Attributes)
              this._attributes.Add(XmlNodeWrapper.WrapNode(attribute));
          }
        }
        return this._attributes;
      }
    }

    private bool HasAttributes
    {
      get
      {
        if (this._node is XmlElement node)
          return node.HasAttributes;
        XmlAttributeCollection attributes = this._node.Attributes;
        return attributes != null && attributes.Count > 0;
      }
    }

    public IXmlNode? ParentNode
    {
      get
      {
        XmlNode node1 = this._node is XmlAttribute node2 ? (XmlNode) node2.OwnerElement : this._node.ParentNode;
        return node1 == null ? (IXmlNode) null : XmlNodeWrapper.WrapNode(node1);
      }
    }

    public string? Value
    {
      get => this._node.Value;
      set => this._node.Value = value;
    }

    public IXmlNode AppendChild(IXmlNode newChild)
    {
      this._node.AppendChild(((XmlNodeWrapper) newChild)._node);
      this._childNodes = (List<IXmlNode>) null;
      this._attributes = (List<IXmlNode>) null;
      return newChild;
    }

    public string? NamespaceUri => this._node.NamespaceURI;
  }
}
