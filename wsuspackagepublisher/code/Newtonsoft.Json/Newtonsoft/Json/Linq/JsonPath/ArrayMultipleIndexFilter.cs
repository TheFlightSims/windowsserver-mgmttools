// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.JsonPath.ArrayMultipleIndexFilter
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\Newtonsoft.Json.dll

using System.Collections.Generic;


#nullable enable
namespace Newtonsoft.Json.Linq.JsonPath
{
  internal class ArrayMultipleIndexFilter : PathFilter
  {
    internal List<int> Indexes;

    public ArrayMultipleIndexFilter(List<int> indexes) => this.Indexes = indexes;

    public override IEnumerable<JToken> ExecuteFilter(
      JToken root,
      IEnumerable<JToken> current,
      bool errorWhenNoMatch)
    {
      foreach (JToken t in current)
      {
        foreach (int index in this.Indexes)
        {
          JToken tokenIndex = PathFilter.GetTokenIndex(t, errorWhenNoMatch, index);
          if (tokenIndex != null)
            yield return tokenIndex;
        }
      }
    }
  }
}
