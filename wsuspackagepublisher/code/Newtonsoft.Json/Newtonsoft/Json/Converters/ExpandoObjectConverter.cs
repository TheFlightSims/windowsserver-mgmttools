// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.ExpandoObjectConverter
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;


#nullable enable
namespace Newtonsoft.Json.Converters
{
  public class ExpandoObjectConverter : JsonConverter
  {
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
    }

    public override object? ReadJson(
      JsonReader reader,
      Type objectType,
      object? existingValue,
      JsonSerializer serializer)
    {
      return this.ReadValue(reader);
    }

    private object? ReadValue(JsonReader reader)
    {
      if (!reader.MoveToContent())
        throw JsonSerializationException.Create(reader, "Unexpected end when reading ExpandoObject.");
      switch (reader.TokenType)
      {
        case JsonToken.StartObject:
          return this.ReadObject(reader);
        case JsonToken.StartArray:
          return this.ReadList(reader);
        default:
          return JsonTokenUtils.IsPrimitiveToken(reader.TokenType) ? reader.Value : throw JsonSerializationException.Create(reader, "Unexpected token when converting ExpandoObject: {0}".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) reader.TokenType));
      }
    }

    private object ReadList(JsonReader reader)
    {
      IList<object> objectList = (IList<object>) new List<object>();
      while (reader.Read())
      {
        switch (reader.TokenType)
        {
          case JsonToken.Comment:
            continue;
          case JsonToken.EndArray:
            return (object) objectList;
          default:
            object obj = this.ReadValue(reader);
            objectList.Add(obj);
            continue;
        }
      }
      throw JsonSerializationException.Create(reader, "Unexpected end when reading ExpandoObject.");
    }

    private object ReadObject(JsonReader reader)
    {
      IDictionary<string, object> dictionary = (IDictionary<string, object>) new ExpandoObject();
      while (reader.Read())
      {
        switch (reader.TokenType)
        {
          case JsonToken.PropertyName:
            string key = reader.Value.ToString();
            object obj = reader.Read() ? this.ReadValue(reader) : throw JsonSerializationException.Create(reader, "Unexpected end when reading ExpandoObject.");
            dictionary[key] = obj;
            continue;
          case JsonToken.EndObject:
            return (object) dictionary;
          default:
            continue;
        }
      }
      throw JsonSerializationException.Create(reader, "Unexpected end when reading ExpandoObject.");
    }

    public override bool CanConvert(Type objectType) => objectType == typeof (ExpandoObject);

    public override bool CanWrite => false;
  }
}
