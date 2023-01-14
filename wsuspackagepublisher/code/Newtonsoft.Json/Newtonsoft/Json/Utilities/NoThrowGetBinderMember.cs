// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Utilities.NoThrowGetBinderMember
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\Newtonsoft.Json.dll

using System.Dynamic;


#nullable enable
namespace Newtonsoft.Json.Utilities
{
  internal class NoThrowGetBinderMember : GetMemberBinder
  {
    private readonly GetMemberBinder _innerBinder;

    public NoThrowGetBinderMember(GetMemberBinder innerBinder)
      : base(innerBinder.Name, innerBinder.IgnoreCase)
    {
      this._innerBinder = innerBinder;
    }

    public override DynamicMetaObject FallbackGetMember(
      DynamicMetaObject target,
      DynamicMetaObject errorSuggestion)
    {
      DynamicMetaObject dynamicMetaObject = this._innerBinder.Bind(target, CollectionUtils.ArrayEmpty<DynamicMetaObject>());
      return new DynamicMetaObject(new NoThrowExpressionVisitor().Visit(dynamicMetaObject.Expression), dynamicMetaObject.Restrictions);
    }
  }
}
