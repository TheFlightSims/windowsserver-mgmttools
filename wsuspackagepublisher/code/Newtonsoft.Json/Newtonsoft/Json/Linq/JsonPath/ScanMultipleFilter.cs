// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.JsonPath.ScanMultipleFilter
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\Newtonsoft.Json.dll

using System.Collections.Generic;


#nullable enable
namespace Newtonsoft.Json.Linq.JsonPath
{
  internal class ScanMultipleFilter : PathFilter
  {
    private List<string> _names;

    public ScanMultipleFilter(List<string> names) => this._names = names;

    public override IEnumerable<JToken> ExecuteFilter(
      JToken root,
      IEnumerable<JToken> current,
      bool errorWhenNoMatch)
    {
      foreach (JToken c in current)
      {
        JToken value = c;
        while (true)
        {
          value = PathFilter.GetNextScanValue(c, (JToken) (value as JContainer), value);
          if (value != null)
          {
            if (value is JProperty property)
            {
              foreach (string name in this._names)
              {
                if (property.Name == name)
                  yield return property.Value;
              }
            }
            property = (JProperty) null;
          }
          else
            break;
        }
        value = (JToken) null;
      }
    }
  }
}
