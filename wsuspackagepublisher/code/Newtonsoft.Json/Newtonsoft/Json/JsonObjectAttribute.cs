// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.JsonObjectAttribute
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\Newtonsoft.Json.dll

using System;


#nullable enable
namespace Newtonsoft.Json
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface, AllowMultiple = false)]
  public sealed class JsonObjectAttribute : JsonContainerAttribute
  {
    private MemberSerialization _memberSerialization;
    internal MissingMemberHandling? _missingMemberHandling;
    internal Required? _itemRequired;
    internal NullValueHandling? _itemNullValueHandling;

    public MemberSerialization MemberSerialization
    {
      get => this._memberSerialization;
      set => this._memberSerialization = value;
    }

    public MissingMemberHandling MissingMemberHandling
    {
      get => this._missingMemberHandling.GetValueOrDefault();
      set => this._missingMemberHandling = new MissingMemberHandling?(value);
    }

    public NullValueHandling ItemNullValueHandling
    {
      get => this._itemNullValueHandling.GetValueOrDefault();
      set => this._itemNullValueHandling = new NullValueHandling?(value);
    }

    public Required ItemRequired
    {
      get => this._itemRequired.GetValueOrDefault();
      set => this._itemRequired = new Required?(value);
    }

    public JsonObjectAttribute()
    {
    }

    public JsonObjectAttribute(MemberSerialization memberSerialization) => this.MemberSerialization = memberSerialization;

    public JsonObjectAttribute(string id)
      : base(id)
    {
    }
  }
}
