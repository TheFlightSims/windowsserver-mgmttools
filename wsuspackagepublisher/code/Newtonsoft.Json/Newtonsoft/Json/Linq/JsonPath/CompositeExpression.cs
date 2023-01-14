// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.JsonPath.CompositeExpression
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\Newtonsoft.Json.dll

using System;
using System.Collections.Generic;


#nullable enable
namespace Newtonsoft.Json.Linq.JsonPath
{
  internal class CompositeExpression : QueryExpression
  {
    public List<QueryExpression> Expressions { get; set; }

    public CompositeExpression(QueryOperator @operator)
      : base(@operator)
    {
      this.Expressions = new List<QueryExpression>();
    }

    public override bool IsMatch(JToken root, JToken t)
    {
      switch (this.Operator)
      {
        case QueryOperator.And:
          foreach (QueryExpression expression in this.Expressions)
          {
            if (!expression.IsMatch(root, t))
              return false;
          }
          return true;
        case QueryOperator.Or:
          foreach (QueryExpression expression in this.Expressions)
          {
            if (expression.IsMatch(root, t))
              return true;
          }
          return false;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
  }
}
