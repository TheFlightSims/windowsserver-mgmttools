// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.JsonSerializerProxy
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System;
using System.Collections;
using System.Globalization;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;


#nullable enable
namespace Newtonsoft.Json.Serialization
{
  internal class JsonSerializerProxy : JsonSerializer
  {
    private readonly JsonSerializerInternalReader? _serializerReader;
    private readonly JsonSerializerInternalWriter? _serializerWriter;
    private readonly JsonSerializer _serializer;

    public override event EventHandler<ErrorEventArgs>? Error
    {
      add => this._serializer.Error += value;
      remove => this._serializer.Error -= value;
    }

    public override IReferenceResolver? ReferenceResolver
    {
      get => this._serializer.ReferenceResolver;
      set => this._serializer.ReferenceResolver = value;
    }

    public override ITraceWriter? TraceWriter
    {
      get => this._serializer.TraceWriter;
      set => this._serializer.TraceWriter = value;
    }

    public override IEqualityComparer? EqualityComparer
    {
      get => this._serializer.EqualityComparer;
      set => this._serializer.EqualityComparer = value;
    }

    public override JsonConverterCollection Converters => this._serializer.Converters;

    public override DefaultValueHandling DefaultValueHandling
    {
      get => this._serializer.DefaultValueHandling;
      set => this._serializer.DefaultValueHandling = value;
    }

    public override IContractResolver ContractResolver
    {
      get => this._serializer.ContractResolver;
      set => this._serializer.ContractResolver = value;
    }

    public override MissingMemberHandling MissingMemberHandling
    {
      get => this._serializer.MissingMemberHandling;
      set => this._serializer.MissingMemberHandling = value;
    }

    public override NullValueHandling NullValueHandling
    {
      get => this._serializer.NullValueHandling;
      set => this._serializer.NullValueHandling = value;
    }

    public override ObjectCreationHandling ObjectCreationHandling
    {
      get => this._serializer.ObjectCreationHandling;
      set => this._serializer.ObjectCreationHandling = value;
    }

    public override ReferenceLoopHandling ReferenceLoopHandling
    {
      get => this._serializer.ReferenceLoopHandling;
      set => this._serializer.ReferenceLoopHandling = value;
    }

    public override PreserveReferencesHandling PreserveReferencesHandling
    {
      get => this._serializer.PreserveReferencesHandling;
      set => this._serializer.PreserveReferencesHandling = value;
    }

    public override TypeNameHandling TypeNameHandling
    {
      get => this._serializer.TypeNameHandling;
      set => this._serializer.TypeNameHandling = value;
    }

    public override MetadataPropertyHandling MetadataPropertyHandling
    {
      get => this._serializer.MetadataPropertyHandling;
      set => this._serializer.MetadataPropertyHandling = value;
    }

    [Obsolete("TypeNameAssemblyFormat is obsolete. Use TypeNameAssemblyFormatHandling instead.")]
    public override FormatterAssemblyStyle TypeNameAssemblyFormat
    {
      get => this._serializer.TypeNameAssemblyFormat;
      set => this._serializer.TypeNameAssemblyFormat = value;
    }

    public override TypeNameAssemblyFormatHandling TypeNameAssemblyFormatHandling
    {
      get => this._serializer.TypeNameAssemblyFormatHandling;
      set => this._serializer.TypeNameAssemblyFormatHandling = value;
    }

    public override ConstructorHandling ConstructorHandling
    {
      get => this._serializer.ConstructorHandling;
      set => this._serializer.ConstructorHandling = value;
    }

    [Obsolete("Binder is obsolete. Use SerializationBinder instead.")]
    public override System.Runtime.Serialization.SerializationBinder Binder
    {
      get => this._serializer.Binder;
      set => this._serializer.Binder = value;
    }

    public override ISerializationBinder SerializationBinder
    {
      get => this._serializer.SerializationBinder;
      set => this._serializer.SerializationBinder = value;
    }

    public override StreamingContext Context
    {
      get => this._serializer.Context;
      set => this._serializer.Context = value;
    }

    public override Formatting Formatting
    {
      get => this._serializer.Formatting;
      set => this._serializer.Formatting = value;
    }

    public override DateFormatHandling DateFormatHandling
    {
      get => this._serializer.DateFormatHandling;
      set => this._serializer.DateFormatHandling = value;
    }

    public override DateTimeZoneHandling DateTimeZoneHandling
    {
      get => this._serializer.DateTimeZoneHandling;
      set => this._serializer.DateTimeZoneHandling = value;
    }

    public override DateParseHandling DateParseHandling
    {
      get => this._serializer.DateParseHandling;
      set => this._serializer.DateParseHandling = value;
    }

    public override FloatFormatHandling FloatFormatHandling
    {
      get => this._serializer.FloatFormatHandling;
      set => this._serializer.FloatFormatHandling = value;
    }

    public override FloatParseHandling FloatParseHandling
    {
      get => this._serializer.FloatParseHandling;
      set => this._serializer.FloatParseHandling = value;
    }

    public override StringEscapeHandling StringEscapeHandling
    {
      get => this._serializer.StringEscapeHandling;
      set => this._serializer.StringEscapeHandling = value;
    }

    public override string DateFormatString
    {
      get => this._serializer.DateFormatString;
      set => this._serializer.DateFormatString = value;
    }

    public override CultureInfo Culture
    {
      get => this._serializer.Culture;
      set => this._serializer.Culture = value;
    }

    public override int? MaxDepth
    {
      get => this._serializer.MaxDepth;
      set => this._serializer.MaxDepth = value;
    }

    public override bool CheckAdditionalContent
    {
      get => this._serializer.CheckAdditionalContent;
      set => this._serializer.CheckAdditionalContent = value;
    }

    internal JsonSerializerInternalBase GetInternalSerializer() => this._serializerReader != null ? (JsonSerializerInternalBase) this._serializerReader : (JsonSerializerInternalBase) this._serializerWriter;

    public JsonSerializerProxy(JsonSerializerInternalReader serializerReader)
    {
      ValidationUtils.ArgumentNotNull((object) serializerReader, nameof (serializerReader));
      this._serializerReader = serializerReader;
      this._serializer = serializerReader.Serializer;
    }

    public JsonSerializerProxy(JsonSerializerInternalWriter serializerWriter)
    {
      ValidationUtils.ArgumentNotNull((object) serializerWriter, nameof (serializerWriter));
      this._serializerWriter = serializerWriter;
      this._serializer = serializerWriter.Serializer;
    }

    internal override object? DeserializeInternal(JsonReader reader, Type? objectType) => this._serializerReader != null ? this._serializerReader.Deserialize(reader, objectType, false) : this._serializer.Deserialize(reader, objectType);

    internal override void PopulateInternal(JsonReader reader, object target)
    {
      if (this._serializerReader != null)
        this._serializerReader.Populate(reader, target);
      else
        this._serializer.Populate(reader, target);
    }

    internal override void SerializeInternal(JsonWriter jsonWriter, object? value, Type? rootType)
    {
      if (this._serializerWriter != null)
        this._serializerWriter.Serialize(jsonWriter, value, rootType);
      else
        this._serializer.Serialize(jsonWriter, value);
    }
  }
}
