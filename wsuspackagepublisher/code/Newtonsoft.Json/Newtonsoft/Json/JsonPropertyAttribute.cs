// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.JsonPropertyAttribute
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\Newtonsoft.Json.dll

using System;


#nullable enable
namespace Newtonsoft.Json
{
  [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
  public sealed class JsonPropertyAttribute : Attribute
  {
    internal NullValueHandling? _nullValueHandling;
    internal DefaultValueHandling? _defaultValueHandling;
    internal ReferenceLoopHandling? _referenceLoopHandling;
    internal ObjectCreationHandling? _objectCreationHandling;
    internal TypeNameHandling? _typeNameHandling;
    internal bool? _isReference;
    internal int? _order;
    internal Required? _required;
    internal bool? _itemIsReference;
    internal ReferenceLoopHandling? _itemReferenceLoopHandling;
    internal TypeNameHandling? _itemTypeNameHandling;

    public Type? ItemConverterType { get; set; }

    public object[]? ItemConverterParameters { get; set; }

    public Type? NamingStrategyType { get; set; }

    public object[]? NamingStrategyParameters { get; set; }

    public NullValueHandling NullValueHandling
    {
      get => this._nullValueHandling.GetValueOrDefault();
      set => this._nullValueHandling = new NullValueHandling?(value);
    }

    public DefaultValueHandling DefaultValueHandling
    {
      get => this._defaultValueHandling.GetValueOrDefault();
      set => this._defaultValueHandling = new DefaultValueHandling?(value);
    }

    public ReferenceLoopHandling ReferenceLoopHandling
    {
      get => this._referenceLoopHandling.GetValueOrDefault();
      set => this._referenceLoopHandling = new ReferenceLoopHandling?(value);
    }

    public ObjectCreationHandling ObjectCreationHandling
    {
      get => this._objectCreationHandling.GetValueOrDefault();
      set => this._objectCreationHandling = new ObjectCreationHandling?(value);
    }

    public TypeNameHandling TypeNameHandling
    {
      get => this._typeNameHandling.GetValueOrDefault();
      set => this._typeNameHandling = new TypeNameHandling?(value);
    }

    public bool IsReference
    {
      get => this._isReference.GetValueOrDefault();
      set => this._isReference = new bool?(value);
    }

    public int Order
    {
      get => this._order.GetValueOrDefault();
      set => this._order = new int?(value);
    }

    public Required Required
    {
      get => this._required.GetValueOrDefault();
      set => this._required = new Required?(value);
    }

    public string? PropertyName { get; set; }

    public ReferenceLoopHandling ItemReferenceLoopHandling
    {
      get => this._itemReferenceLoopHandling.GetValueOrDefault();
      set => this._itemReferenceLoopHandling = new ReferenceLoopHandling?(value);
    }

    public TypeNameHandling ItemTypeNameHandling
    {
      get => this._itemTypeNameHandling.GetValueOrDefault();
      set => this._itemTypeNameHandling = new TypeNameHandling?(value);
    }

    public bool ItemIsReference
    {
      get => this._itemIsReference.GetValueOrDefault();
      set => this._itemIsReference = new bool?(value);
    }

    public JsonPropertyAttribute()
    {
    }

    public JsonPropertyAttribute(string propertyName) => this.PropertyName = propertyName;
  }
}
