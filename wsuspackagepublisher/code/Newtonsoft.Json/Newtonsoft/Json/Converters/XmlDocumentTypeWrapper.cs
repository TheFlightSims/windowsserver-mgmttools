// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.XmlDocumentTypeWrapper
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\Newtonsoft.Json.dll

using System.Xml;


#nullable enable
namespace Newtonsoft.Json.Converters
{
  internal class XmlDocumentTypeWrapper : XmlNodeWrapper, IXmlDocumentType, IXmlNode
  {
    private readonly XmlDocumentType _documentType;

    public XmlDocumentTypeWrapper(XmlDocumentType documentType)
      : base((XmlNode) documentType)
    {
      this._documentType = documentType;
    }

    public string Name => this._documentType.Name;

    public string System => this._documentType.SystemId;

    public string Public => this._documentType.PublicId;

    public string InternalSubset => this._documentType.InternalSubset;

    public override string? LocalName => "DOCTYPE";
  }
}
