// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.IReferenceResolver
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\Newtonsoft.Json.dll


#nullable enable
namespace Newtonsoft.Json.Serialization
{
  public interface IReferenceResolver
  {
    object ResolveReference(object context, string reference);

    string GetReference(object context, object value);

    bool IsReferenced(object context, object value);

    void AddReference(object context, string reference, object value);
  }
}
