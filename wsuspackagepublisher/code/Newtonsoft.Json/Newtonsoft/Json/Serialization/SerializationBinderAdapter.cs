// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.SerializationBinderAdapter
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\Newtonsoft.Json.dll

using System;
using System.Runtime.Serialization;


#nullable enable
namespace Newtonsoft.Json.Serialization
{
  internal class SerializationBinderAdapter : ISerializationBinder
  {
    public readonly SerializationBinder SerializationBinder;

    public SerializationBinderAdapter(SerializationBinder serializationBinder) => this.SerializationBinder = serializationBinder;

    public Type BindToType(string? assemblyName, string typeName) => this.SerializationBinder.BindToType(assemblyName, typeName);

    public void BindToName(Type serializedType, out string? assemblyName, out string? typeName) => this.SerializationBinder.BindToName(serializedType, out assemblyName, out typeName);
  }
}
