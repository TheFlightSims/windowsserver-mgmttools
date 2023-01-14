// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.RegexConverter
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\Newtonsoft.Json.dll

using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Utilities;
using System;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;


#nullable enable
namespace Newtonsoft.Json.Converters
{
  public class RegexConverter : JsonConverter
  {
    private const string PatternName = "Pattern";
    private const string OptionsName = "Options";

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
      if (value == null)
      {
        writer.WriteNull();
      }
      else
      {
        Regex regex = (Regex) value;
        if (writer is BsonWriter writer1)
          this.WriteBson(writer1, regex);
        else
          this.WriteJson(writer, regex, serializer);
      }
    }

    private bool HasFlag(RegexOptions options, RegexOptions flag) => (options & flag) == flag;

    private void WriteBson(BsonWriter writer, Regex regex)
    {
      string str = (string) null;
      if (this.HasFlag(regex.Options, RegexOptions.IgnoreCase))
        str += "i";
      if (this.HasFlag(regex.Options, RegexOptions.Multiline))
        str += "m";
      if (this.HasFlag(regex.Options, RegexOptions.Singleline))
        str += "s";
      string options = str + "u";
      if (this.HasFlag(regex.Options, RegexOptions.ExplicitCapture))
        options += "x";
      writer.WriteRegex(regex.ToString(), options);
    }

    private void WriteJson(JsonWriter writer, Regex regex, JsonSerializer serializer)
    {
      DefaultContractResolver contractResolver = serializer.ContractResolver as DefaultContractResolver;
      writer.WriteStartObject();
      writer.WritePropertyName(contractResolver != null ? contractResolver.GetResolvedPropertyName("Pattern") : "Pattern");
      writer.WriteValue(regex.ToString());
      writer.WritePropertyName(contractResolver != null ? contractResolver.GetResolvedPropertyName("Options") : "Options");
      serializer.Serialize(writer, (object) regex.Options);
      writer.WriteEndObject();
    }

    public override object? ReadJson(
      JsonReader reader,
      Type objectType,
      object? existingValue,
      JsonSerializer serializer)
    {
      switch (reader.TokenType)
      {
        case JsonToken.StartObject:
          return (object) this.ReadRegexObject(reader, serializer);
        case JsonToken.String:
          return this.ReadRegexString(reader);
        case JsonToken.Null:
          return (object) null;
        default:
          throw JsonSerializationException.Create(reader, "Unexpected token when reading Regex.");
      }
    }

    private object ReadRegexString(JsonReader reader)
    {
      string str = (string) reader.Value;
      int num = str.Length > 0 && str[0] == '/' ? str.LastIndexOf('/') : throw JsonSerializationException.Create(reader, "Regex pattern must be enclosed by slashes.");
      if (num > 0)
        return (object) new Regex(str.Substring(1, num - 1), MiscellaneousUtils.GetRegexOptions(str.Substring(num + 1)));
    }

    private Regex ReadRegexObject(JsonReader reader, JsonSerializer serializer)
    {
      string pattern = (string) null;
      RegexOptions? nullable = new RegexOptions?();
      while (reader.Read())
      {
        switch (reader.TokenType)
        {
          case JsonToken.PropertyName:
            string a = reader.Value.ToString();
            if (!reader.Read())
              throw JsonSerializationException.Create(reader, "Unexpected end when reading Regex.");
            if (string.Equals(a, "Pattern", StringComparison.OrdinalIgnoreCase))
            {
              pattern = (string) reader.Value;
              continue;
            }
            if (string.Equals(a, "Options", StringComparison.OrdinalIgnoreCase))
            {
              nullable = new RegexOptions?(serializer.Deserialize<RegexOptions>(reader));
              continue;
            }
            reader.Skip();
            continue;
          case JsonToken.EndObject:
            return pattern != null ? new Regex(pattern, nullable.GetValueOrDefault()) : throw JsonSerializationException.Create(reader, "Error deserializing Regex. No pattern found.");
          default:
            continue;
        }
      }
      throw JsonSerializationException.Create(reader, "Unexpected end when reading Regex.");
    }

    public override bool CanConvert(Type objectType) => objectType.Name == "Regex" && this.IsRegex(objectType);

    [MethodImpl(MethodImplOptions.NoInlining)]
    private bool IsRegex(Type objectType) => objectType == typeof (Regex);
  }
}
