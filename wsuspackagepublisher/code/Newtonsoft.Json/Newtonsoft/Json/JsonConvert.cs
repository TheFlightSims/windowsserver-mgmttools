// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.JsonConvert
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\Newtonsoft.Json.dll

using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Numerics;
using System.Text;
using System.Xml;
using System.Xml.Linq;


#nullable enable
namespace Newtonsoft.Json
{
  public static class JsonConvert
  {
    public static readonly string True = "true";
    public static readonly string False = "false";
    public static readonly string Null = "null";
    public static readonly string Undefined = "undefined";
    public static readonly string PositiveInfinity = "Infinity";
    public static readonly string NegativeInfinity = "-Infinity";
    public static readonly string NaN = nameof (NaN);

    public static Func<JsonSerializerSettings>? DefaultSettings { get; set; }

    public static string ToString(DateTime value) => JsonConvert.ToString(value, DateFormatHandling.IsoDateFormat, DateTimeZoneHandling.RoundtripKind);

    public static string ToString(
      DateTime value,
      DateFormatHandling format,
      DateTimeZoneHandling timeZoneHandling)
    {
      DateTime dateTime = DateTimeUtils.EnsureDateTime(value, timeZoneHandling);
      using (StringWriter stringWriter = StringUtils.CreateStringWriter(64))
      {
        stringWriter.Write('"');
        DateTimeUtils.WriteDateTimeString((TextWriter) stringWriter, dateTime, format, (string) null, CultureInfo.InvariantCulture);
        stringWriter.Write('"');
        return stringWriter.ToString();
      }
    }

    public static string ToString(DateTimeOffset value) => JsonConvert.ToString(value, DateFormatHandling.IsoDateFormat);

    public static string ToString(DateTimeOffset value, DateFormatHandling format)
    {
      using (StringWriter stringWriter = StringUtils.CreateStringWriter(64))
      {
        stringWriter.Write('"');
        DateTimeUtils.WriteDateTimeOffsetString((TextWriter) stringWriter, value, format, (string) null, CultureInfo.InvariantCulture);
        stringWriter.Write('"');
        return stringWriter.ToString();
      }
    }

    public static string ToString(bool value) => !value ? JsonConvert.False : JsonConvert.True;

    public static string ToString(char value) => JsonConvert.ToString(char.ToString(value));

    public static string ToString(Enum value) => value.ToString("D");

    public static string ToString(int value) => value.ToString((string) null, (IFormatProvider) CultureInfo.InvariantCulture);

    public static string ToString(short value) => value.ToString((string) null, (IFormatProvider) CultureInfo.InvariantCulture);

    [CLSCompliant(false)]
    public static string ToString(ushort value) => value.ToString((string) null, (IFormatProvider) CultureInfo.InvariantCulture);

    [CLSCompliant(false)]
    public static string ToString(uint value) => value.ToString((string) null, (IFormatProvider) CultureInfo.InvariantCulture);

    public static string ToString(long value) => value.ToString((string) null, (IFormatProvider) CultureInfo.InvariantCulture);

    private static string ToStringInternal(BigInteger value) => value.ToString((string) null, (IFormatProvider) CultureInfo.InvariantCulture);

    [CLSCompliant(false)]
    public static string ToString(ulong value) => value.ToString((string) null, (IFormatProvider) CultureInfo.InvariantCulture);

    public static string ToString(float value) => JsonConvert.EnsureDecimalPlace((double) value, value.ToString("R", (IFormatProvider) CultureInfo.InvariantCulture));

    internal static string ToString(
      float value,
      FloatFormatHandling floatFormatHandling,
      char quoteChar,
      bool nullable)
    {
      return JsonConvert.EnsureFloatFormat((double) value, JsonConvert.EnsureDecimalPlace((double) value, value.ToString("R", (IFormatProvider) CultureInfo.InvariantCulture)), floatFormatHandling, quoteChar, nullable);
    }

    private static string EnsureFloatFormat(
      double value,
      string text,
      FloatFormatHandling floatFormatHandling,
      char quoteChar,
      bool nullable)
    {
      if (floatFormatHandling == FloatFormatHandling.Symbol || !double.IsInfinity(value) && !double.IsNaN(value))
        return text;
      if (floatFormatHandling != FloatFormatHandling.DefaultValue)
        return quoteChar.ToString() + text + quoteChar.ToString();
      return nullable ? JsonConvert.Null : "0.0";
    }

    public static string ToString(double value) => JsonConvert.EnsureDecimalPlace(value, value.ToString("R", (IFormatProvider) CultureInfo.InvariantCulture));

    internal static string ToString(
      double value,
      FloatFormatHandling floatFormatHandling,
      char quoteChar,
      bool nullable)
    {
      return JsonConvert.EnsureFloatFormat(value, JsonConvert.EnsureDecimalPlace(value, value.ToString("R", (IFormatProvider) CultureInfo.InvariantCulture)), floatFormatHandling, quoteChar, nullable);
    }

    private static string EnsureDecimalPlace(double value, string text) => double.IsNaN(value) || double.IsInfinity(value) || text.IndexOf('.') != -1 || text.IndexOf('E') != -1 || text.IndexOf('e') != -1 ? text : text + ".0";

    private static string EnsureDecimalPlace(string text) => text.IndexOf('.') != -1 ? text : text + ".0";

    public static string ToString(byte value) => value.ToString((string) null, (IFormatProvider) CultureInfo.InvariantCulture);

    [CLSCompliant(false)]
    public static string ToString(sbyte value) => value.ToString((string) null, (IFormatProvider) CultureInfo.InvariantCulture);

    public static string ToString(Decimal value) => JsonConvert.EnsureDecimalPlace(value.ToString((string) null, (IFormatProvider) CultureInfo.InvariantCulture));

    public static string ToString(Guid value) => JsonConvert.ToString(value, '"');

    internal static string ToString(Guid value, char quoteChar)
    {
      string str1 = value.ToString("D", (IFormatProvider) CultureInfo.InvariantCulture);
      string str2 = quoteChar.ToString((IFormatProvider) CultureInfo.InvariantCulture);
      return str2 + str1 + str2;
    }

    public static string ToString(TimeSpan value) => JsonConvert.ToString(value, '"');

    internal static string ToString(TimeSpan value, char quoteChar) => JsonConvert.ToString(value.ToString(), quoteChar);

    public static string ToString(Uri? value) => value == (Uri) null ? JsonConvert.Null : JsonConvert.ToString(value, '"');

    internal static string ToString(Uri value, char quoteChar) => JsonConvert.ToString(value.OriginalString, quoteChar);

    public static string ToString(string? value) => JsonConvert.ToString(value, '"');

    public static string ToString(string? value, char delimiter) => JsonConvert.ToString(value, delimiter, StringEscapeHandling.Default);

    public static string ToString(
      string? value,
      char delimiter,
      StringEscapeHandling stringEscapeHandling)
    {
      if (delimiter != '"' && delimiter != '\'')
        throw new ArgumentException("Delimiter must be a single or double quote.", nameof (delimiter));
      return JavaScriptUtils.ToEscapedJavaScriptString(value, delimiter, true, stringEscapeHandling);
    }

    public static string ToString(object? value)
    {
      if (value == null)
        return JsonConvert.Null;
      switch (ConvertUtils.GetTypeCode(value.GetType()))
      {
        case PrimitiveTypeCode.Char:
          return JsonConvert.ToString((char) value);
        case PrimitiveTypeCode.Boolean:
          return JsonConvert.ToString((bool) value);
        case PrimitiveTypeCode.SByte:
          return JsonConvert.ToString((sbyte) value);
        case PrimitiveTypeCode.Int16:
          return JsonConvert.ToString((short) value);
        case PrimitiveTypeCode.UInt16:
          return JsonConvert.ToString((ushort) value);
        case PrimitiveTypeCode.Int32:
          return JsonConvert.ToString((int) value);
        case PrimitiveTypeCode.Byte:
          return JsonConvert.ToString((byte) value);
        case PrimitiveTypeCode.UInt32:
          return JsonConvert.ToString((uint) value);
        case PrimitiveTypeCode.Int64:
          return JsonConvert.ToString((long) value);
        case PrimitiveTypeCode.UInt64:
          return JsonConvert.ToString((ulong) value);
        case PrimitiveTypeCode.Single:
          return JsonConvert.ToString((float) value);
        case PrimitiveTypeCode.Double:
          return JsonConvert.ToString((double) value);
        case PrimitiveTypeCode.DateTime:
          return JsonConvert.ToString((DateTime) value);
        case PrimitiveTypeCode.DateTimeOffset:
          return JsonConvert.ToString((DateTimeOffset) value);
        case PrimitiveTypeCode.Decimal:
          return JsonConvert.ToString((Decimal) value);
        case PrimitiveTypeCode.Guid:
          return JsonConvert.ToString((Guid) value);
        case PrimitiveTypeCode.TimeSpan:
          return JsonConvert.ToString((TimeSpan) value);
        case PrimitiveTypeCode.BigInteger:
          return JsonConvert.ToStringInternal((BigInteger) value);
        case PrimitiveTypeCode.Uri:
          return JsonConvert.ToString((Uri) value);
        case PrimitiveTypeCode.String:
          return JsonConvert.ToString((string) value);
        case PrimitiveTypeCode.DBNull:
          return JsonConvert.Null;
        default:
          throw new ArgumentException("Unsupported type: {0}. Use the JsonSerializer class to get the object's JSON representation.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) value.GetType()));
      }
    }

    [DebuggerStepThrough]
    public static string SerializeObject(object? value) => JsonConvert.SerializeObject(value, (Type) null, (JsonSerializerSettings) null);

    [DebuggerStepThrough]
    public static string SerializeObject(object? value, Formatting formatting) => JsonConvert.SerializeObject(value, formatting, (JsonSerializerSettings) null);

    [DebuggerStepThrough]
    public static string SerializeObject(object? value, params JsonConverter[] converters)
    {
      JsonSerializerSettings serializerSettings;
      if (converters == null || converters.Length == 0)
        serializerSettings = (JsonSerializerSettings) null;
      else
        serializerSettings = new JsonSerializerSettings()
        {
          Converters = (IList<JsonConverter>) converters
        };
      JsonSerializerSettings settings = serializerSettings;
      return JsonConvert.SerializeObject(value, (Type) null, settings);
    }

    [DebuggerStepThrough]
    public static string SerializeObject(
      object? value,
      Formatting formatting,
      params JsonConverter[] converters)
    {
      JsonSerializerSettings serializerSettings;
      if (converters == null || converters.Length == 0)
        serializerSettings = (JsonSerializerSettings) null;
      else
        serializerSettings = new JsonSerializerSettings()
        {
          Converters = (IList<JsonConverter>) converters
        };
      JsonSerializerSettings settings = serializerSettings;
      return JsonConvert.SerializeObject(value, (Type) null, formatting, settings);
    }

    [DebuggerStepThrough]
    public static string SerializeObject(object? value, JsonSerializerSettings settings) => JsonConvert.SerializeObject(value, (Type) null, settings);

    [DebuggerStepThrough]
    public static string SerializeObject(object? value, Type? type, JsonSerializerSettings? settings)
    {
      JsonSerializer jsonSerializer = JsonSerializer.CreateDefault(settings);
      return JsonConvert.SerializeObjectInternal(value, type, jsonSerializer);
    }

    [DebuggerStepThrough]
    public static string SerializeObject(
      object? value,
      Formatting formatting,
      JsonSerializerSettings? settings)
    {
      return JsonConvert.SerializeObject(value, (Type) null, formatting, settings);
    }

    [DebuggerStepThrough]
    public static string SerializeObject(
      object? value,
      Type? type,
      Formatting formatting,
      JsonSerializerSettings? settings)
    {
      JsonSerializer jsonSerializer = JsonSerializer.CreateDefault(settings);
      jsonSerializer.Formatting = formatting;
      return JsonConvert.SerializeObjectInternal(value, type, jsonSerializer);
    }

    private static string SerializeObjectInternal(
      object? value,
      Type? type,
      JsonSerializer jsonSerializer)
    {
      StringWriter stringWriter = new StringWriter(new StringBuilder(256), (IFormatProvider) CultureInfo.InvariantCulture);
      using (JsonTextWriter jsonTextWriter = new JsonTextWriter((TextWriter) stringWriter))
      {
        jsonTextWriter.Formatting = jsonSerializer.Formatting;
        jsonSerializer.Serialize((JsonWriter) jsonTextWriter, value, type);
      }
      return stringWriter.ToString();
    }

    [DebuggerStepThrough]
    public static object? DeserializeObject(string value) => JsonConvert.DeserializeObject(value, (Type) null, (JsonSerializerSettings) null);

    [DebuggerStepThrough]
    public static object? DeserializeObject(string value, JsonSerializerSettings settings) => JsonConvert.DeserializeObject(value, (Type) null, settings);

    [DebuggerStepThrough]
    public static object? DeserializeObject(string value, Type type) => JsonConvert.DeserializeObject(value, type, (JsonSerializerSettings) null);

    [DebuggerStepThrough]
    public static T DeserializeObject<T>(string value) => JsonConvert.DeserializeObject<T>(value, (JsonSerializerSettings) null);

    [DebuggerStepThrough]
    public static T DeserializeAnonymousType<T>(string value, T anonymousTypeObject) => JsonConvert.DeserializeObject<T>(value);

    [DebuggerStepThrough]
    public static T DeserializeAnonymousType<T>(
      string value,
      T anonymousTypeObject,
      JsonSerializerSettings settings)
    {
      return JsonConvert.DeserializeObject<T>(value, settings);
    }

    [DebuggerStepThrough]
    [return: MaybeNull]
    public static T DeserializeObject<T>(string value, params JsonConverter[] converters) => (T) JsonConvert.DeserializeObject(value, typeof (T), converters);

    [DebuggerStepThrough]
    [return: MaybeNull]
    public static T DeserializeObject<T>(string value, JsonSerializerSettings? settings) => (T) JsonConvert.DeserializeObject(value, typeof (T), settings);

    [DebuggerStepThrough]
    public static object? DeserializeObject(
      string value,
      Type type,
      params JsonConverter[] converters)
    {
      JsonSerializerSettings serializerSettings;
      if (converters == null || converters.Length == 0)
        serializerSettings = (JsonSerializerSettings) null;
      else
        serializerSettings = new JsonSerializerSettings()
        {
          Converters = (IList<JsonConverter>) converters
        };
      JsonSerializerSettings settings = serializerSettings;
      return JsonConvert.DeserializeObject(value, type, settings);
    }

    public static object? DeserializeObject(
      string value,
      Type? type,
      JsonSerializerSettings? settings)
    {
      ValidationUtils.ArgumentNotNull((object) value, nameof (value));
      JsonSerializer jsonSerializer = JsonSerializer.CreateDefault(settings);
      if (!jsonSerializer.IsCheckAdditionalContentSet())
        jsonSerializer.CheckAdditionalContent = true;
      using (JsonTextReader reader = new JsonTextReader((TextReader) new StringReader(value)))
        return jsonSerializer.Deserialize((JsonReader) reader, type);
    }

    [DebuggerStepThrough]
    public static void PopulateObject(string value, object target) => JsonConvert.PopulateObject(value, target, (JsonSerializerSettings) null);

    public static void PopulateObject(string value, object target, JsonSerializerSettings? settings)
    {
      using (JsonReader reader = (JsonReader) new JsonTextReader((TextReader) new StringReader(value)))
      {
        JsonSerializer.CreateDefault(settings).Populate(reader, target);
        if (settings == null || !settings.CheckAdditionalContent)
          return;
        while (reader.Read())
        {
          if (reader.TokenType != JsonToken.Comment)
            throw JsonSerializationException.Create(reader, "Additional text found in JSON string after finishing deserializing object.");
        }
      }
    }

    public static string SerializeXmlNode(XmlNode? node) => JsonConvert.SerializeXmlNode(node, Formatting.None);

    public static string SerializeXmlNode(XmlNode? node, Formatting formatting)
    {
      XmlNodeConverter xmlNodeConverter = new XmlNodeConverter();
      return JsonConvert.SerializeObject((object) node, formatting, (JsonConverter) xmlNodeConverter);
    }

    public static string SerializeXmlNode(XmlNode? node, Formatting formatting, bool omitRootObject)
    {
      XmlNodeConverter xmlNodeConverter = new XmlNodeConverter()
      {
        OmitRootObject = omitRootObject
      };
      return JsonConvert.SerializeObject((object) node, formatting, (JsonConverter) xmlNodeConverter);
    }

    public static XmlDocument? DeserializeXmlNode(string value) => JsonConvert.DeserializeXmlNode(value, (string) null);

    public static XmlDocument? DeserializeXmlNode(string value, string? deserializeRootElementName) => JsonConvert.DeserializeXmlNode(value, deserializeRootElementName, false);

    public static XmlDocument? DeserializeXmlNode(
      string value,
      string? deserializeRootElementName,
      bool writeArrayAttribute)
    {
      return JsonConvert.DeserializeXmlNode(value, deserializeRootElementName, writeArrayAttribute, false);
    }

    public static XmlDocument? DeserializeXmlNode(
      string value,
      string? deserializeRootElementName,
      bool writeArrayAttribute,
      bool encodeSpecialCharacters)
    {
      return (XmlDocument) JsonConvert.DeserializeObject(value, typeof (XmlDocument), (JsonConverter) new XmlNodeConverter()
      {
        DeserializeRootElementName = deserializeRootElementName,
        WriteArrayAttribute = writeArrayAttribute,
        EncodeSpecialCharacters = encodeSpecialCharacters
      });
    }

    public static string SerializeXNode(XObject? node) => JsonConvert.SerializeXNode(node, Formatting.None);

    public static string SerializeXNode(XObject? node, Formatting formatting) => JsonConvert.SerializeXNode(node, formatting, false);

    public static string SerializeXNode(XObject? node, Formatting formatting, bool omitRootObject)
    {
      XmlNodeConverter xmlNodeConverter = new XmlNodeConverter()
      {
        OmitRootObject = omitRootObject
      };
      return JsonConvert.SerializeObject((object) node, formatting, (JsonConverter) xmlNodeConverter);
    }

    public static XDocument? DeserializeXNode(string value) => JsonConvert.DeserializeXNode(value, (string) null);

    public static XDocument? DeserializeXNode(string value, string? deserializeRootElementName) => JsonConvert.DeserializeXNode(value, deserializeRootElementName, false);

    public static XDocument? DeserializeXNode(
      string value,
      string? deserializeRootElementName,
      bool writeArrayAttribute)
    {
      return JsonConvert.DeserializeXNode(value, deserializeRootElementName, writeArrayAttribute, false);
    }

    public static XDocument? DeserializeXNode(
      string value,
      string? deserializeRootElementName,
      bool writeArrayAttribute,
      bool encodeSpecialCharacters)
    {
      return (XDocument) JsonConvert.DeserializeObject(value, typeof (XDocument), (JsonConverter) new XmlNodeConverter()
      {
        DeserializeRootElementName = deserializeRootElementName,
        WriteArrayAttribute = writeArrayAttribute,
        EncodeSpecialCharacters = encodeSpecialCharacters
      });
    }
  }
}
