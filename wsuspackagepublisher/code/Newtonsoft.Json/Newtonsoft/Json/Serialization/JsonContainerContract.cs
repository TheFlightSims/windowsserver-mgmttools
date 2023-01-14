// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.JsonContainerContract
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System;


#nullable enable
namespace Newtonsoft.Json.Serialization
{
  public class JsonContainerContract : JsonContract
  {
    private JsonContract? _itemContract;
    private JsonContract? _finalItemContract;

    internal JsonContract? ItemContract
    {
      get => this._itemContract;
      set
      {
        this._itemContract = value;
        if (this._itemContract != null)
          this._finalItemContract = this._itemContract.UnderlyingType.IsSealed() ? this._itemContract : (JsonContract) null;
        else
          this._finalItemContract = (JsonContract) null;
      }
    }

    internal JsonContract? FinalItemContract => this._finalItemContract;

    public JsonConverter? ItemConverter { get; set; }

    public bool? ItemIsReference { get; set; }

    public ReferenceLoopHandling? ItemReferenceLoopHandling { get; set; }

    public TypeNameHandling? ItemTypeNameHandling { get; set; }

    internal JsonContainerContract(Type underlyingType)
      : base(underlyingType)
    {
      JsonContainerAttribute cachedAttribute = JsonTypeReflector.GetCachedAttribute<JsonContainerAttribute>((object) underlyingType);
      if (cachedAttribute == null)
        return;
      if (cachedAttribute.ItemConverterType != (Type) null)
        this.ItemConverter = JsonTypeReflector.CreateJsonConverterInstance(cachedAttribute.ItemConverterType, cachedAttribute.ItemConverterParameters);
      this.ItemIsReference = cachedAttribute._itemIsReference;
      this.ItemReferenceLoopHandling = cachedAttribute._itemReferenceLoopHandling;
      this.ItemTypeNameHandling = cachedAttribute._itemTypeNameHandling;
    }
  }
}
