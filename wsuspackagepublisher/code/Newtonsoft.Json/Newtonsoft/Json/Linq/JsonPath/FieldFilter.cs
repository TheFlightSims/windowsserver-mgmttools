// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.JsonPath.FieldFilter
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
  internal class FieldFilter : PathFilter
  {
    internal string? Name;

    public FieldFilter(string? name) => this.Name = name;

    public override IEnumerable<JToken> ExecuteFilter(
      JToken root,
      IEnumerable<JToken> current,
      bool errorWhenNoMatch)
    {
      foreach (JToken jtoken1 in current)
      {
        if (jtoken1 is JObject jobject)
        {
          if (this.Name != null)
          {
            JToken jtoken2 = jobject[this.Name];
            if (jtoken2 != null)
              yield return jtoken2;
            else if (errorWhenNoMatch)
              throw new JsonException("Property '{0}' does not exist on JObject.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this.Name));
          }
          else
          {
            foreach (KeyValuePair<string, JToken> keyValuePair in jobject)
              yield return keyValuePair.Value;
          }
        }
        else if (errorWhenNoMatch)
          throw new JsonException("Property '{0}' not valid on {1}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) (this.Name ?? "*"), (object) jtoken1.GetType().Name));
      }
    }
  }
}
