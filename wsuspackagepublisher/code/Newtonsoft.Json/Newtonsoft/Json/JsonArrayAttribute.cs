// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.JsonArrayAttribute
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\Newtonsoft.Json.dll

using System;


#nullable enable
namespace Newtonsoft.Json
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false)]
  public sealed class JsonArrayAttribute : JsonContainerAttribute
  {
    private bool _allowNullItems;

    public bool AllowNullItems
    {
      get => this._allowNullItems;
      set => this._allowNullItems = value;
    }

    public JsonArrayAttribute()
    {
    }

    public JsonArrayAttribute(bool allowNullItems) => this._allowNullItems = allowNullItems;

    public JsonArrayAttribute(string id)
      : base(id)
    {
    }
  }
}
