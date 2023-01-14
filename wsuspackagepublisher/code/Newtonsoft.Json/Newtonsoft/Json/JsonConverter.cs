// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.JsonConverter
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\Newtonsoft.Json.dll

using System;


#nullable enable
namespace Newtonsoft.Json
{
  public abstract class JsonConverter
  {
    public abstract void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer);

    public abstract object? ReadJson(
      JsonReader reader,
      Type objectType,
      object? existingValue,
      JsonSerializer serializer);

    public abstract bool CanConvert(Type objectType);

    public virtual bool CanRead => true;

    public virtual bool CanWrite => true;
  }
}
