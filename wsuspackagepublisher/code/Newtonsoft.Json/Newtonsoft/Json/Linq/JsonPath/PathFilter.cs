// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.JsonPath.PathFilter
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;


#nullable enable
namespace Newtonsoft.Json.Linq.JsonPath
{
  internal abstract class PathFilter
  {
    public abstract IEnumerable<JToken> ExecuteFilter(
      JToken root,
      IEnumerable<JToken> current,
      bool errorWhenNoMatch);

    protected static JToken? GetTokenIndex(JToken t, bool errorWhenNoMatch, int index)
    {
      if (t is JArray jarray)
      {
        if (jarray.Count > index)
          return jarray[index];
        if (errorWhenNoMatch)
          throw new JsonException("Index {0} outside the bounds of JArray.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) index));
        return (JToken) null;
      }
      if (t is JConstructor jconstructor)
      {
        if (jconstructor.Count > index)
          return jconstructor[(object) index];
        if (errorWhenNoMatch)
          throw new JsonException("Index {0} outside the bounds of JConstructor.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) index));
        return (JToken) null;
      }
      if (errorWhenNoMatch)
        throw new JsonException("Index {0} not valid on {1}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) index, (object) t.GetType().Name));
      return (JToken) null;
    }

    protected static JToken? GetNextScanValue(
      JToken originalParent,
      JToken? container,
      JToken? value)
    {
      if (container != null && container.HasValues)
      {
        value = container.First;
      }
      else
      {
        while (value != null && value != originalParent && value == value.Parent.Last)
          value = (JToken) value.Parent;
        if (value == null || value == originalParent)
          return (JToken) null;
        value = value.Next;
      }
      return value;
    }
  }
}
