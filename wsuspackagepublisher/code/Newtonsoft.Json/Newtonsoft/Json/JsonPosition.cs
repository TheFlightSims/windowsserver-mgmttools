// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.JsonPosition
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;


#nullable enable
namespace Newtonsoft.Json
{
  internal struct JsonPosition
  {
    private static readonly char[] SpecialCharacters = new char[18]
    {
      '.',
      ' ',
      '\'',
      '/',
      '"',
      '[',
      ']',
      '(',
      ')',
      '\t',
      '\n',
      '\r',
      '\f',
      '\b',
      '\\',
      '\u0085',
      '\u2028',
      '\u2029'
    };
    internal JsonContainerType Type;
    internal int Position;
    internal string? PropertyName;
    internal bool HasIndex;

    public JsonPosition(JsonContainerType type)
    {
      this.Type = type;
      this.HasIndex = JsonPosition.TypeHasIndex(type);
      this.Position = -1;
      this.PropertyName = (string) null;
    }

    internal int CalculateLength()
    {
      switch (this.Type)
      {
        case JsonContainerType.Object:
          return this.PropertyName.Length + 5;
        case JsonContainerType.Array:
        case JsonContainerType.Constructor:
          return MathUtils.IntLength((ulong) this.Position) + 2;
        default:
          throw new ArgumentOutOfRangeException("Type");
      }
    }

    internal void WriteTo(StringBuilder sb, ref StringWriter? writer, ref char[]? buffer)
    {
      switch (this.Type)
      {
        case JsonContainerType.Object:
          string propertyName = this.PropertyName;
          if (propertyName.IndexOfAny(JsonPosition.SpecialCharacters) != -1)
          {
            sb.Append("['");
            if (writer == null)
              writer = new StringWriter(sb);
            JavaScriptUtils.WriteEscapedJavaScriptString((TextWriter) writer, propertyName, '\'', false, JavaScriptUtils.SingleQuoteCharEscapeFlags, StringEscapeHandling.Default, (IArrayPool<char>) null, ref buffer);
            sb.Append("']");
            break;
          }
          if (sb.Length > 0)
            sb.Append('.');
          sb.Append(propertyName);
          break;
        case JsonContainerType.Array:
        case JsonContainerType.Constructor:
          sb.Append('[');
          sb.Append(this.Position);
          sb.Append(']');
          break;
      }
    }

    internal static bool TypeHasIndex(JsonContainerType type) => type == JsonContainerType.Array || type == JsonContainerType.Constructor;

    internal static string BuildPath(List<JsonPosition> positions, JsonPosition? currentPosition)
    {
      int capacity = 0;
      JsonPosition jsonPosition;
      if (positions != null)
      {
        for (int index = 0; index < positions.Count; ++index)
        {
          int num = capacity;
          jsonPosition = positions[index];
          int length = jsonPosition.CalculateLength();
          capacity = num + length;
        }
      }
      if (currentPosition.HasValue)
      {
        int num = capacity;
        jsonPosition = currentPosition.GetValueOrDefault();
        int length = jsonPosition.CalculateLength();
        capacity = num + length;
      }
      StringBuilder sb = new StringBuilder(capacity);
      StringWriter writer = (StringWriter) null;
      char[] buffer = (char[]) null;
      if (positions != null)
      {
        foreach (JsonPosition position in positions)
          position.WriteTo(sb, ref writer, ref buffer);
      }
      if (currentPosition.HasValue)
      {
        jsonPosition = currentPosition.GetValueOrDefault();
        jsonPosition.WriteTo(sb, ref writer, ref buffer);
      }
      return sb.ToString();
    }

    internal static string FormatMessage(IJsonLineInfo? lineInfo, string path, string message)
    {
      if (!message.EndsWith(Environment.NewLine, StringComparison.Ordinal))
      {
        message = message.Trim();
        if (!message.EndsWith('.'))
          message += ".";
        message += " ";
      }
      message += "Path '{0}'".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) path);
      if (lineInfo != null && lineInfo.HasLineInfo())
        message += ", line {0}, position {1}".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) lineInfo.LineNumber, (object) lineInfo.LinePosition);
      message += ".";
      return message;
    }
  }
}
