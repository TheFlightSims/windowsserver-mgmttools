// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Utilities.StringUtils
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\Newtonsoft.Json.dll

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;


#nullable enable
namespace Newtonsoft.Json.Utilities
{
  internal static class StringUtils
  {
    public const string CarriageReturnLineFeed = "\r\n";
    public const string Empty = "";
    public const char CarriageReturn = '\r';
    public const char LineFeed = '\n';
    public const char Tab = '\t';

    public static bool IsNullOrEmpty([NotNullWhen(false)] string? value) => string.IsNullOrEmpty(value);

    public static string FormatWith(this string format, IFormatProvider provider, object? arg0) => StringUtils.FormatWith(format, provider, new object[1]
    {
      arg0
    });

    public static string FormatWith(
      this string format,
      IFormatProvider provider,
      object? arg0,
      object? arg1)
    {
      return StringUtils.FormatWith(format, provider, new object[2]
      {
        arg0,
        arg1
      });
    }

    public static string FormatWith(
      this string format,
      IFormatProvider provider,
      object? arg0,
      object? arg1,
      object? arg2)
    {
      return StringUtils.FormatWith(format, provider, new object[3]
      {
        arg0,
        arg1,
        arg2
      });
    }

    public static string FormatWith(
      this string format,
      IFormatProvider provider,
      object? arg0,
      object? arg1,
      object? arg2,
      object? arg3)
    {
      return StringUtils.FormatWith(format, provider, new object[4]
      {
        arg0,
        arg1,
        arg2,
        arg3
      });
    }

    private static string FormatWith(
      this string format,
      IFormatProvider provider,
      params object?[] args)
    {
      ValidationUtils.ArgumentNotNull((object) format, nameof (format));
      return string.Format(provider, format, args);
    }

    public static bool IsWhiteSpace(string s)
    {
      switch (s)
      {
        case null:
          throw new ArgumentNullException(nameof (s));
        case "":
          return false;
        default:
          for (int index = 0; index < s.Length; ++index)
          {
            if (!char.IsWhiteSpace(s[index]))
              return false;
          }
          return true;
      }
    }

    public static StringWriter CreateStringWriter(int capacity) => new StringWriter(new StringBuilder(capacity), (IFormatProvider) CultureInfo.InvariantCulture);

    public static void ToCharAsUnicode(char c, char[] buffer)
    {
      buffer[0] = '\\';
      buffer[1] = 'u';
      buffer[2] = MathUtils.IntToHex((int) c >> 12 & 15);
      buffer[3] = MathUtils.IntToHex((int) c >> 8 & 15);
      buffer[4] = MathUtils.IntToHex((int) c >> 4 & 15);
      buffer[5] = MathUtils.IntToHex((int) c & 15);
    }

    public static TSource ForgivingCaseSensitiveFind<TSource>(
      this IEnumerable<TSource> source,
      Func<TSource, string> valueSelector,
      string testValue)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (valueSelector == null)
        throw new ArgumentNullException(nameof (valueSelector));
      IEnumerable<TSource> source1 = source.Where<TSource>((Func<TSource, bool>) (s => string.Equals(valueSelector(s), testValue, StringComparison.OrdinalIgnoreCase)));
      return source1.Count<TSource>() <= 1 ? source1.SingleOrDefault<TSource>() : source.Where<TSource>((Func<TSource, bool>) (s => string.Equals(valueSelector(s), testValue, StringComparison.Ordinal))).SingleOrDefault<TSource>();
    }

    public static string ToCamelCase(string s)
    {
      if (StringUtils.IsNullOrEmpty(s) || !char.IsUpper(s[0]))
        return s;
      char[] charArray = s.ToCharArray();
      for (int index = 0; index < charArray.Length && (index != 1 || char.IsUpper(charArray[index])); ++index)
      {
        bool flag = index + 1 < charArray.Length;
        if (index > 0 & flag && !char.IsUpper(charArray[index + 1]))
        {
          if (char.IsSeparator(charArray[index + 1]))
          {
            charArray[index] = StringUtils.ToLower(charArray[index]);
            break;
          }
          break;
        }
        charArray[index] = StringUtils.ToLower(charArray[index]);
      }
      return new string(charArray);
    }

    private static char ToLower(char c)
    {
      c = char.ToLower(c, CultureInfo.InvariantCulture);
      return c;
    }

    public static string ToSnakeCase(string s) => StringUtils.ToSeparatedCase(s, '_');

    public static string ToKebabCase(string s) => StringUtils.ToSeparatedCase(s, '-');

    private static string ToSeparatedCase(string s, char separator)
    {
      if (StringUtils.IsNullOrEmpty(s))
        return s;
      StringBuilder stringBuilder = new StringBuilder();
      StringUtils.SeparatedCaseState separatedCaseState = StringUtils.SeparatedCaseState.Start;
      for (int index = 0; index < s.Length; ++index)
      {
        if (s[index] == ' ')
        {
          if (separatedCaseState != StringUtils.SeparatedCaseState.Start)
            separatedCaseState = StringUtils.SeparatedCaseState.NewWord;
        }
        else if (char.IsUpper(s[index]))
        {
          switch (separatedCaseState)
          {
            case StringUtils.SeparatedCaseState.Lower:
            case StringUtils.SeparatedCaseState.NewWord:
              stringBuilder.Append(separator);
              break;
            case StringUtils.SeparatedCaseState.Upper:
              bool flag = index + 1 < s.Length;
              if (index > 0 & flag)
              {
                char c = s[index + 1];
                if (!char.IsUpper(c) && (int) c != (int) separator)
                {
                  stringBuilder.Append(separator);
                  break;
                }
                break;
              }
              break;
          }
          char lower = char.ToLower(s[index], CultureInfo.InvariantCulture);
          stringBuilder.Append(lower);
          separatedCaseState = StringUtils.SeparatedCaseState.Upper;
        }
        else if ((int) s[index] == (int) separator)
        {
          stringBuilder.Append(separator);
          separatedCaseState = StringUtils.SeparatedCaseState.Start;
        }
        else
        {
          if (separatedCaseState == StringUtils.SeparatedCaseState.NewWord)
            stringBuilder.Append(separator);
          stringBuilder.Append(s[index]);
          separatedCaseState = StringUtils.SeparatedCaseState.Lower;
        }
      }
      return stringBuilder.ToString();
    }

    public static bool IsHighSurrogate(char c) => char.IsHighSurrogate(c);

    public static bool IsLowSurrogate(char c) => char.IsLowSurrogate(c);

    public static bool StartsWith(this string source, char value) => source.Length > 0 && (int) source[0] == (int) value;

    public static bool EndsWith(this string source, char value) => source.Length > 0 && (int) source[source.Length - 1] == (int) value;

    public static string Trim(this string s, int start, int length)
    {
      if (s == null)
        throw new ArgumentNullException();
      if (start < 0)
        throw new ArgumentOutOfRangeException(nameof (start));
      if (length < 0)
        throw new ArgumentOutOfRangeException(nameof (length));
      int index = start + length - 1;
      if (index >= s.Length)
        throw new ArgumentOutOfRangeException(nameof (length));
      while (start < index && char.IsWhiteSpace(s[start]))
        ++start;
      while (index >= start && char.IsWhiteSpace(s[index]))
        --index;
      return s.Substring(start, index - start + 1);
    }

    private enum SeparatedCaseState
    {
      Start,
      Lower,
      Upper,
      NewWord,
    }
  }
}
