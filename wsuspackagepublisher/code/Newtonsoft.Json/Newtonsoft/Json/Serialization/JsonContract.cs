// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.JsonContract
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;


#nullable enable
namespace Newtonsoft.Json.Serialization
{
  public abstract class JsonContract
  {
    internal bool IsNullable;
    internal bool IsConvertable;
    internal bool IsEnum;
    internal Type NonNullableUnderlyingType;
    internal ReadType InternalReadType;
    internal JsonContractType ContractType;
    internal bool IsReadOnlyOrFixedSize;
    internal bool IsSealed;
    internal bool IsInstantiable;
    private List<SerializationCallback>? _onDeserializedCallbacks;
    private List<SerializationCallback>? _onDeserializingCallbacks;
    private List<SerializationCallback>? _onSerializedCallbacks;
    private List<SerializationCallback>? _onSerializingCallbacks;
    private List<SerializationErrorCallback>? _onErrorCallbacks;
    private Type _createdType;

    public Type UnderlyingType { get; }

    public Type CreatedType
    {
      get => this._createdType;
      set
      {
        ValidationUtils.ArgumentNotNull((object) value, nameof (value));
        this._createdType = value;
        this.IsSealed = this._createdType.IsSealed();
        this.IsInstantiable = !this._createdType.IsInterface() && !this._createdType.IsAbstract();
      }
    }

    public bool? IsReference { get; set; }

    public JsonConverter? Converter { get; set; }

    public JsonConverter? InternalConverter { get; internal set; }

    public IList<SerializationCallback> OnDeserializedCallbacks
    {
      get
      {
        if (this._onDeserializedCallbacks == null)
          this._onDeserializedCallbacks = new List<SerializationCallback>();
        return (IList<SerializationCallback>) this._onDeserializedCallbacks;
      }
    }

    public IList<SerializationCallback> OnDeserializingCallbacks
    {
      get
      {
        if (this._onDeserializingCallbacks == null)
          this._onDeserializingCallbacks = new List<SerializationCallback>();
        return (IList<SerializationCallback>) this._onDeserializingCallbacks;
      }
    }

    public IList<SerializationCallback> OnSerializedCallbacks
    {
      get
      {
        if (this._onSerializedCallbacks == null)
          this._onSerializedCallbacks = new List<SerializationCallback>();
        return (IList<SerializationCallback>) this._onSerializedCallbacks;
      }
    }

    public IList<SerializationCallback> OnSerializingCallbacks
    {
      get
      {
        if (this._onSerializingCallbacks == null)
          this._onSerializingCallbacks = new List<SerializationCallback>();
        return (IList<SerializationCallback>) this._onSerializingCallbacks;
      }
    }

    public IList<SerializationErrorCallback> OnErrorCallbacks
    {
      get
      {
        if (this._onErrorCallbacks == null)
          this._onErrorCallbacks = new List<SerializationErrorCallback>();
        return (IList<SerializationErrorCallback>) this._onErrorCallbacks;
      }
    }

    public Func<object>? DefaultCreator { get; set; }

    public bool DefaultCreatorNonPublic { get; set; }

    internal JsonContract(Type underlyingType)
    {
      ValidationUtils.ArgumentNotNull((object) underlyingType, nameof (underlyingType));
      this.UnderlyingType = underlyingType;
      underlyingType = ReflectionUtils.EnsureNotByRefType(underlyingType);
      this.IsNullable = ReflectionUtils.IsNullable(underlyingType);
      this.NonNullableUnderlyingType = !this.IsNullable || !ReflectionUtils.IsNullableType(underlyingType) ? underlyingType : Nullable.GetUnderlyingType(underlyingType);
      this._createdType = this.CreatedType = this.NonNullableUnderlyingType;
      this.IsConvertable = ConvertUtils.IsConvertible(this.NonNullableUnderlyingType);
      this.IsEnum = this.NonNullableUnderlyingType.IsEnum();
      this.InternalReadType = ReadType.Read;
    }

    internal void InvokeOnSerializing(object o, StreamingContext context)
    {
      if (this._onSerializingCallbacks == null)
        return;
      foreach (SerializationCallback serializingCallback in this._onSerializingCallbacks)
        serializingCallback(o, context);
    }

    internal void InvokeOnSerialized(object o, StreamingContext context)
    {
      if (this._onSerializedCallbacks == null)
        return;
      foreach (SerializationCallback serializedCallback in this._onSerializedCallbacks)
        serializedCallback(o, context);
    }

    internal void InvokeOnDeserializing(object o, StreamingContext context)
    {
      if (this._onDeserializingCallbacks == null)
        return;
      foreach (SerializationCallback deserializingCallback in this._onDeserializingCallbacks)
        deserializingCallback(o, context);
    }

    internal void InvokeOnDeserialized(object o, StreamingContext context)
    {
      if (this._onDeserializedCallbacks == null)
        return;
      foreach (SerializationCallback deserializedCallback in this._onDeserializedCallbacks)
        deserializedCallback(o, context);
    }

    internal void InvokeOnError(object o, StreamingContext context, ErrorContext errorContext)
    {
      if (this._onErrorCallbacks == null)
        return;
      foreach (SerializationErrorCallback onErrorCallback in this._onErrorCallbacks)
        onErrorCallback(o, context, errorContext);
    }

    internal static SerializationCallback CreateSerializationCallback(MethodInfo callbackMethodInfo) => (SerializationCallback) ((o, context) => callbackMethodInfo.Invoke(o, new object[1]
    {
      (object) context
    }));

    internal static SerializationErrorCallback CreateSerializationErrorCallback(
      MethodInfo callbackMethodInfo)
    {
      return (SerializationErrorCallback) ((o, context, econtext) => callbackMethodInfo.Invoke(o, new object[2]
      {
        (object) context,
        (object) econtext
      }));
    }
  }
}
