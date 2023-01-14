// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.JsonPath.QueryScanFilter
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\Newtonsoft.Json.dll

using System.Collections.Generic;


#nullable enable
namespace Newtonsoft.Json.Linq.JsonPath
{
  internal class QueryScanFilter : PathFilter
  {
    internal QueryExpression Expression;

    public QueryScanFilter(QueryExpression expression) => this.Expression = expression;

    public override IEnumerable<JToken> ExecuteFilter(
      JToken root,
      IEnumerable<JToken> current,
      bool errorWhenNoMatch)
    {
      foreach (JToken t1 in current)
      {
        if (t1 is JContainer jcontainer)
        {
          foreach (JToken t2 in jcontainer.DescendantsAndSelf())
          {
            if (this.Expression.IsMatch(root, t2))
              yield return t2;
          }
        }
        else if (this.Expression.IsMatch(root, t1))
          yield return t1;
      }
    }
  }
}
