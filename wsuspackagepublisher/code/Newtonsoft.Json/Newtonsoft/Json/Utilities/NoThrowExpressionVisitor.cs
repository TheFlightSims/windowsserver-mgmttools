// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Utilities.NoThrowExpressionVisitor
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\Newtonsoft.Json.dll

using System.Linq.Expressions;


#nullable enable
namespace Newtonsoft.Json.Utilities
{
  internal class NoThrowExpressionVisitor : ExpressionVisitor
  {
    internal static readonly object ErrorResult = new object();

    protected override Expression VisitConditional(ConditionalExpression node) => node.IfFalse.NodeType == ExpressionType.Throw ? (Expression) Expression.Condition(node.Test, node.IfTrue, (Expression) Expression.Constant(NoThrowExpressionVisitor.ErrorResult)) : base.VisitConditional(node);
  }
}
