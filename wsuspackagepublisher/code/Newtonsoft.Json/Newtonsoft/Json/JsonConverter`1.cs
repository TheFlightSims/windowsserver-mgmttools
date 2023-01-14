// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.JsonConverter`1
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;


#nullable enable
namespace Newtonsoft.Json
{
  public abstract class JsonConverter<T> : JsonConverter
  {
    public override sealed void WriteJson(
      JsonWriter writer,
      object? value,
      JsonSerializer serializer)
    {
      if ((value != null ? (value is T ? 1 : 0) : (ReflectionUtils.IsNullable(typeof (T)) ? 1 : 0)) == 0)
        throw new JsonSerializationException("Converter cannot write specified value to JSON. {0} is required.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) typeof (T)));
      this.WriteJson(writer, (T) value, serializer);
    }

    public abstract void WriteJson(JsonWriter writer, [AllowNull] T value, JsonSerializer serializer);

    public override sealed object? ReadJson(
      JsonReader reader,
      Type objectType,
      object? existingValue,
      JsonSerializer serializer)
    {
      bool flag = existingValue == null;
      if (!flag && !(existingValue is T))
        throw new JsonSerializationException("Converter cannot read JSON with the specified existing value. {0} is required.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) typeof (T)));
      return (object) this.ReadJson(reader, objectType, flag ? default (T) : (T) existingValue, !flag, serializer);
    }

    public abstract T ReadJson(
      JsonReader reader,
      Type objectType,
      [AllowNull] T existingValue,
      bool hasExistingValue,
      JsonSerializer serializer);

    public override sealed bool CanConvert(Type objectType) => typeof (T).IsAssignableFrom(objectType);
  }
}
