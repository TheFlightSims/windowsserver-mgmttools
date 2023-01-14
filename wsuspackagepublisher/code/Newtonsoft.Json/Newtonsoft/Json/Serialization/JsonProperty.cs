// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.JsonProperty
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System;


#nullable enable
namespace Newtonsoft.Json.Serialization
{
  public class JsonProperty
  {
    internal Required? _required;
    internal bool _hasExplicitDefaultValue;
    private object? _defaultValue;
    private bool _hasGeneratedDefaultValue;
    private string? _propertyName;
    internal bool _skipPropertyNameEscape;
    private Type? _propertyType;

    internal JsonContract? PropertyContract { get; set; }

    public string? PropertyName
    {
      get => this._propertyName;
      set
      {
        this._propertyName = value;
        this._skipPropertyNameEscape = !JavaScriptUtils.ShouldEscapeJavaScriptString(this._propertyName, JavaScriptUtils.HtmlCharEscapeFlags);
      }
    }

    public Type? DeclaringType { get; set; }

    public int? Order { get; set; }

    public string? UnderlyingName { get; set; }

    public IValueProvider? ValueProvider { get; set; }

    public IAttributeProvider? AttributeProvider { get; set; }

    public Type? PropertyType
    {
      get => this._propertyType;
      set
      {
        if (!(this._propertyType != value))
          return;
        this._propertyType = value;
        this._hasGeneratedDefaultValue = false;
      }
    }

    public JsonConverter? Converter { get; set; }

    [Obsolete("MemberConverter is obsolete. Use Converter instead.")]
    public JsonConverter? MemberConverter
    {
      get => this.Converter;
      set => this.Converter = value;
    }

    public bool Ignored { get; set; }

    public bool Readable { get; set; }

    public bool Writable { get; set; }

    public bool HasMemberAttribute { get; set; }

    public object? DefaultValue
    {
      get => !this._hasExplicitDefaultValue ? (object) null : this._defaultValue;
      set
      {
        this._hasExplicitDefaultValue = true;
        this._defaultValue = value;
      }
    }

    internal object? GetResolvedDefaultValue()
    {
      if (this._propertyType == (Type) null)
        return (object) null;
      if (!this._hasExplicitDefaultValue && !this._hasGeneratedDefaultValue)
      {
        this._defaultValue = ReflectionUtils.GetDefaultValue(this._propertyType);
        this._hasGeneratedDefaultValue = true;
      }
      return this._defaultValue;
    }

    public Required Required
    {
      get => this._required.GetValueOrDefault();
      set => this._required = new Required?(value);
    }

    public bool IsRequiredSpecified => this._required.HasValue;

    public bool? IsReference { get; set; }

    public Newtonsoft.Json.NullValueHandling? NullValueHandling { get; set; }

    public Newtonsoft.Json.DefaultValueHandling? DefaultValueHandling { get; set; }

    public Newtonsoft.Json.ReferenceLoopHandling? ReferenceLoopHandling { get; set; }

    public Newtonsoft.Json.ObjectCreationHandling? ObjectCreationHandling { get; set; }

    public Newtonsoft.Json.TypeNameHandling? TypeNameHandling { get; set; }

    public Predicate<object>? ShouldSerialize { get; set; }

    public Predicate<object>? ShouldDeserialize { get; set; }

    public Predicate<object>? GetIsSpecified { get; set; }

    public Action<object, object?>? SetIsSpecified { get; set; }

    public override string ToString() => this.PropertyName ?? string.Empty;

    public JsonConverter? ItemConverter { get; set; }

    public bool? ItemIsReference { get; set; }

    public Newtonsoft.Json.TypeNameHandling? ItemTypeNameHandling { get; set; }

    public Newtonsoft.Json.ReferenceLoopHandling? ItemReferenceLoopHandling { get; set; }

    internal void WritePropertyName(JsonWriter writer)
    {
      string propertyName = this.PropertyName;
      if (this._skipPropertyNameEscape)
        writer.WritePropertyName(propertyName, false);
      else
        writer.WritePropertyName(propertyName);
    }
  }
}
