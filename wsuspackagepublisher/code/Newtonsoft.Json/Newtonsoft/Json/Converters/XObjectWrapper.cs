// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.XObjectWrapper
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\Newtonsoft.Json.dll

using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;


#nullable enable
namespace Newtonsoft.Json.Converters
{
  internal class XObjectWrapper : IXmlNode
  {
    private readonly XObject? _xmlObject;

    public XObjectWrapper(XObject? xmlObject) => this._xmlObject = xmlObject;

    public object? WrappedNode => (object) this._xmlObject;

    public virtual XmlNodeType NodeType
    {
      get
      {
        XObject xmlObject = this._xmlObject;
        return xmlObject == null ? XmlNodeType.None : xmlObject.NodeType;
      }
    }

    public virtual string? LocalName => (string) null;

    public virtual List<IXmlNode> ChildNodes => XmlNodeConverter.EmptyChildNodes;

    public virtual List<IXmlNode> Attributes => XmlNodeConverter.EmptyChildNodes;

    public virtual IXmlNode? ParentNode => (IXmlNode) null;

    public virtual string? Value
    {
      get => (string) null;
      set => throw new InvalidOperationException();
    }

    public virtual IXmlNode AppendChild(IXmlNode newChild) => throw new InvalidOperationException();

    public virtual string? NamespaceUri => (string) null;
  }
}
