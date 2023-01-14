// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.JsonPath.QueryExpression
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\Newtonsoft.Json.dll


#nullable enable
namespace Newtonsoft.Json.Linq.JsonPath
{
  internal abstract class QueryExpression
  {
    internal QueryOperator Operator;

    public QueryExpression(QueryOperator @operator) => this.Operator = @operator;

    public abstract bool IsMatch(JToken root, JToken t);
  }
}
