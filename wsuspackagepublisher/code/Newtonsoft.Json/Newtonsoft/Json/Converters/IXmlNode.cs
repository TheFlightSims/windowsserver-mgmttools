// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.IXmlNode
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\Newtonsoft.Json.dll

using System.Collections.Generic;
using System.Xml;


#nullable enable
namespace Newtonsoft.Json.Converters
{
  internal interface IXmlNode
  {
    XmlNodeType NodeType { get; }

    string? LocalName { get; }

    List<IXmlNode> ChildNodes { get; }

    List<IXmlNode> Attributes { get; }

    IXmlNode? ParentNode { get; }

    string? Value { get; set; }

    IXmlNode AppendChild(IXmlNode newChild);

    string? NamespaceUri { get; }

    object? WrappedNode { get; }
  }
}
