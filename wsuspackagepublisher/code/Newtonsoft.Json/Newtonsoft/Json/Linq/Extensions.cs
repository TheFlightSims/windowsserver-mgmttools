// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.Extensions
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;


#nullable enable
namespace Newtonsoft.Json.Linq
{
  public static class Extensions
  {
    public static IJEnumerable<JToken> Ancestors<T>(this IEnumerable<T> source) where T : JToken
    {
      ValidationUtils.ArgumentNotNull((object) source, nameof (source));
      return source.SelectMany<T, JToken>((Func<T, IEnumerable<JToken>>) (j => j.Ancestors())).AsJEnumerable();
    }

    public static IJEnumerable<JToken> AncestorsAndSelf<T>(this IEnumerable<T> source) where T : JToken
    {
      ValidationUtils.ArgumentNotNull((object) source, nameof (source));
      return source.SelectMany<T, JToken>((Func<T, IEnumerable<JToken>>) (j => j.AncestorsAndSelf())).AsJEnumerable();
    }

    public static IJEnumerable<JToken> Descendants<T>(this IEnumerable<T> source) where T : JContainer
    {
      ValidationUtils.ArgumentNotNull((object) source, nameof (source));
      return source.SelectMany<T, JToken>((Func<T, IEnumerable<JToken>>) (j => j.Descendants())).AsJEnumerable();
    }

    public static IJEnumerable<JToken> DescendantsAndSelf<T>(this IEnumerable<T> source) where T : JContainer
    {
      ValidationUtils.ArgumentNotNull((object) source, nameof (source));
      return source.SelectMany<T, JToken>((Func<T, IEnumerable<JToken>>) (j => j.DescendantsAndSelf())).AsJEnumerable();
    }

    public static IJEnumerable<JProperty> Properties(this IEnumerable<JObject> source)
    {
      ValidationUtils.ArgumentNotNull((object) source, nameof (source));
      return source.SelectMany<JObject, JProperty>((Func<JObject, IEnumerable<JProperty>>) (d => d.Properties())).AsJEnumerable<JProperty>();
    }

    public static IJEnumerable<JToken> Values(this IEnumerable<JToken> source, object? key) => source.Values<JToken, JToken>(key).AsJEnumerable();

    public static IJEnumerable<JToken> Values(this IEnumerable<JToken> source) => source.Values((object) null);

    public static IEnumerable<U> Values<U>(this IEnumerable<JToken> source, object key) => source.Values<JToken, U>(key);

    public static IEnumerable<U> Values<U>(this IEnumerable<JToken> source) => source.Values<JToken, U>((object) null);

    public static U Value<U>(this IEnumerable<JToken> value) => value.Value<JToken, U>();

    public static U Value<T, U>(this IEnumerable<T> value) where T : JToken
    {
      ValidationUtils.ArgumentNotNull((object) value, nameof (value));
      return value is JToken token ? token.Convert<JToken, U>() : throw new ArgumentException("Source value must be a JToken.");
    }

    internal static IEnumerable<U> Values<T, U>(this IEnumerable<T> source, object? key) where T : JToken
    {
      ValidationUtils.ArgumentNotNull((object) source, nameof (source));
      if (key == null)
      {
        foreach (T obj in source)
        {
          if (obj is JValue token)
          {
            yield return token.Convert<JValue, U>();
          }
          else
          {
            foreach (JToken child in obj.Children())
              yield return child.Convert<JToken, U>();
          }
        }
      }
      else
      {
        foreach (T obj in source)
        {
          JToken token = obj[key];
          if (token != null)
            yield return token.Convert<JToken, U>();
        }
      }
    }

    public static IJEnumerable<JToken> Children<T>(this IEnumerable<T> source) where T : JToken => source.Children<T, JToken>().AsJEnumerable();

    public static IEnumerable<U> Children<T, U>(this IEnumerable<T> source) where T : JToken
    {
      ValidationUtils.ArgumentNotNull((object) source, nameof (source));
      return source.SelectMany<T, JToken>((Func<T, IEnumerable<JToken>>) (c => (IEnumerable<JToken>) c.Children())).Convert<JToken, U>();
    }

    internal static IEnumerable<U> Convert<T, U>(this IEnumerable<T> source) where T : JToken
    {
      ValidationUtils.ArgumentNotNull((object) source, nameof (source));
      foreach (T token in source)
        yield return token.Convert<JToken, U>();
    }

    [return: MaybeNull]
    internal static U Convert<T, U>(this T token) where T : JToken?
    {
      if ((object) token == null)
        return default (U);
      switch (token)
      {
        case U u2 when typeof (U) != typeof (IComparable) && typeof (U) != typeof (IFormattable):
          return u2;
        case JValue jvalue:
          if (jvalue.Value is U u1)
            return u1;
          Type type = typeof (U);
          if (ReflectionUtils.IsNullableType(type))
          {
            if (jvalue.Value == null)
              return default (U);
            type = Nullable.GetUnderlyingType(type);
          }
          return (U) System.Convert.ChangeType(jvalue.Value, type, (IFormatProvider) CultureInfo.InvariantCulture);
        default:
          throw new InvalidCastException("Cannot cast {0} to {1}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) token.GetType(), (object) typeof (T)));
      }
    }

    public static IJEnumerable<JToken> AsJEnumerable(this IEnumerable<JToken> source) => source.AsJEnumerable<JToken>();

    public static IJEnumerable<T> AsJEnumerable<T>(this IEnumerable<T> source) where T : JToken
    {
      if (source == null)
        return (IJEnumerable<T>) null;
      return source is IJEnumerable<T> jenumerable ? jenumerable : (IJEnumerable<T>) new JEnumerable<T>(source);
    }
  }
}
