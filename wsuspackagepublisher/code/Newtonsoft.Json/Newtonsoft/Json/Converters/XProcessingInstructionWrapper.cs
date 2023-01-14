// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.XProcessingInstructionWrapper
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\Newtonsoft.Json.dll

using System.Xml.Linq;


#nullable enable
namespace Newtonsoft.Json.Converters
{
  internal class XProcessingInstructionWrapper : XObjectWrapper
  {
    private XProcessingInstruction ProcessingInstruction => (XProcessingInstruction) this.WrappedNode;

    public XProcessingInstructionWrapper(XProcessingInstruction processingInstruction)
      : base((XObject) processingInstruction)
    {
    }

    public override string? LocalName => this.ProcessingInstruction.Target;

    public override string? Value
    {
      get => this.ProcessingInstruction.Data;
      set => this.ProcessingInstruction.Data = value;
    }
  }
}
