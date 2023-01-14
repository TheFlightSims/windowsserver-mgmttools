﻿// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Schema.JsonSchemaGenerator
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\Newtonsoft.Json.dll

using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace Newtonsoft.Json.Schema
{
  [Obsolete("JSON Schema validation has been moved to its own package. See https://www.newtonsoft.com/jsonschema for more details.")]
  public class JsonSchemaGenerator
  {
    private IContractResolver _contractResolver;
    private JsonSchemaResolver _resolver;
    private readonly IList<JsonSchemaGenerator.TypeSchema> _stack = (IList<JsonSchemaGenerator.TypeSchema>) new List<JsonSchemaGenerator.TypeSchema>();
    private JsonSchema _currentSchema;

    public UndefinedSchemaIdHandling UndefinedSchemaIdHandling { get; set; }

    public IContractResolver ContractResolver
    {
      get => this._contractResolver == null ? DefaultContractResolver.Instance : this._contractResolver;
      set => this._contractResolver = value;
    }

    private JsonSchema CurrentSchema => this._currentSchema;

    private void Push(JsonSchemaGenerator.TypeSchema typeSchema)
    {
      this._currentSchema = typeSchema.Schema;
      this._stack.Add(typeSchema);
      this._resolver.LoadedSchemas.Add(typeSchema.Schema);
    }

    private JsonSchemaGenerator.TypeSchema Pop()
    {
      JsonSchemaGenerator.TypeSchema typeSchema1 = this._stack[this._stack.Count - 1];
      this._stack.RemoveAt(this._stack.Count - 1);
      JsonSchemaGenerator.TypeSchema typeSchema2 = this._stack.LastOrDefault<JsonSchemaGenerator.TypeSchema>();
      if (typeSchema2 != null)
      {
        this._currentSchema = typeSchema2.Schema;
        return typeSchema1;
      }
      this._currentSchema = (JsonSchema) null;
      return typeSchema1;
    }

    public JsonSchema Generate(Type type) => this.Generate(type, new JsonSchemaResolver(), false);

    public JsonSchema Generate(Type type, JsonSchemaResolver resolver) => this.Generate(type, resolver, false);

    public JsonSchema Generate(Type type, bool rootSchemaNullable) => this.Generate(type, new JsonSchemaResolver(), rootSchemaNullable);

    public JsonSchema Generate(Type type, JsonSchemaResolver resolver, bool rootSchemaNullable)
    {
      ValidationUtils.ArgumentNotNull((object) type, nameof (type));
      ValidationUtils.ArgumentNotNull((object) resolver, nameof (resolver));
      this._resolver = resolver;
      return this.GenerateInternal(type, !rootSchemaNullable ? Required.Always : Required.Default, false);
    }

    private string GetTitle(Type type)
    {
      JsonContainerAttribute cachedAttribute = JsonTypeReflector.GetCachedAttribute<JsonContainerAttribute>((object) type);
      return !StringUtils.IsNullOrEmpty(cachedAttribute?.Title) ? cachedAttribute.Title : (string) null;
    }

    private string GetDescription(Type type)
    {
      JsonContainerAttribute cachedAttribute = JsonTypeReflector.GetCachedAttribute<JsonContainerAttribute>((object) type);
      if (!StringUtils.IsNullOrEmpty(cachedAttribute?.Description))
        return cachedAttribute.Description;
      return ReflectionUtils.GetAttribute<DescriptionAttribute>((object) type)?.Description;
    }

    private string GetTypeId(Type type, bool explicitOnly)
    {
      JsonContainerAttribute cachedAttribute = JsonTypeReflector.GetCachedAttribute<JsonContainerAttribute>((object) type);
      if (!StringUtils.IsNullOrEmpty(cachedAttribute?.Id))
        return cachedAttribute.Id;
      if (explicitOnly)
        return (string) null;
      switch (this.UndefinedSchemaIdHandling)
      {
        case UndefinedSchemaIdHandling.UseTypeName:
          return type.FullName;
        case UndefinedSchemaIdHandling.UseAssemblyQualifiedName:
          return type.AssemblyQualifiedName;
        default:
          return (string) null;
      }
    }

    private JsonSchema GenerateInternal(Type type, Required valueRequired, bool required)
    {
      ValidationUtils.ArgumentNotNull((object) type, nameof (type));
      string typeId1 = this.GetTypeId(type, false);
      string typeId2 = this.GetTypeId(type, true);
      if (!StringUtils.IsNullOrEmpty(typeId1))
      {
        JsonSchema schema = this._resolver.GetSchema(typeId1);
        if (schema != null)
        {
          if (valueRequired != Required.Always && !JsonSchemaGenerator.HasFlag(schema.Type, JsonSchemaType.Null))
          {
            JsonSchema jsonSchema = schema;
            JsonSchemaType? type1 = jsonSchema.Type;
            jsonSchema.Type = type1.HasValue ? new JsonSchemaType?(type1.GetValueOrDefault() | JsonSchemaType.Null) : new JsonSchemaType?();
          }
          if (required)
          {
            bool? required1 = schema.Required;
            bool flag = true;
            if (!(required1.GetValueOrDefault() == flag & required1.HasValue))
              schema.Required = new bool?(true);
          }
          return schema;
        }
      }
      if (this._stack.Any<JsonSchemaGenerator.TypeSchema>((Func<JsonSchemaGenerator.TypeSchema, bool>) (tc => tc.Type == type)))
        throw new JsonException("Unresolved circular reference for type '{0}'. Explicitly define an Id for the type using a JsonObject/JsonArray attribute or automatically generate a type Id using the UndefinedSchemaIdHandling property.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) type));
      JsonContract contract = this.ContractResolver.ResolveContract(type);
      JsonConverter jsonConverter = contract.Converter ?? contract.InternalConverter;
      this.Push(new JsonSchemaGenerator.TypeSchema(type, new JsonSchema()));
      if (typeId2 != null)
        this.CurrentSchema.Id = typeId2;
      if (required)
        this.CurrentSchema.Required = new bool?(true);
      this.CurrentSchema.Title = this.GetTitle(type);
      this.CurrentSchema.Description = this.GetDescription(type);
      if (jsonConverter != null)
      {
        this.CurrentSchema.Type = new JsonSchemaType?(JsonSchemaType.Any);
      }
      else
      {
        switch (contract.ContractType)
        {
          case JsonContractType.Object:
            this.CurrentSchema.Type = new JsonSchemaType?(this.AddNullType(JsonSchemaType.Object, valueRequired));
            this.CurrentSchema.Id = this.GetTypeId(type, false);
            this.GenerateObjectSchema(type, (JsonObjectContract) contract);
            break;
          case JsonContractType.Array:
            this.CurrentSchema.Type = new JsonSchemaType?(this.AddNullType(JsonSchemaType.Array, valueRequired));
            this.CurrentSchema.Id = this.GetTypeId(type, false);
            JsonArrayAttribute cachedAttribute = JsonTypeReflector.GetCachedAttribute<JsonArrayAttribute>((object) type);
            bool flag1 = cachedAttribute == null || cachedAttribute.AllowNullItems;
            Type collectionItemType = ReflectionUtils.GetCollectionItemType(type);
            if (collectionItemType != (Type) null)
            {
              this.CurrentSchema.Items = (IList<JsonSchema>) new List<JsonSchema>();
              this.CurrentSchema.Items.Add(this.GenerateInternal(collectionItemType, !flag1 ? Required.Always : Required.Default, false));
              break;
            }
            break;
          case JsonContractType.Primitive:
            this.CurrentSchema.Type = new JsonSchemaType?(this.GetJsonSchemaType(type, valueRequired));
            JsonSchemaType? type2 = this.CurrentSchema.Type;
            JsonSchemaType jsonSchemaType = JsonSchemaType.Integer;
            if (type2.GetValueOrDefault() == jsonSchemaType & type2.HasValue && type.IsEnum() && !type.IsDefined(typeof (FlagsAttribute), true))
            {
              this.CurrentSchema.Enum = (IList<JToken>) new List<JToken>();
              EnumInfo enumValuesAndNames = EnumUtils.GetEnumValuesAndNames(type);
              for (int index = 0; index < enumValuesAndNames.Names.Length; ++index)
              {
                ulong num = enumValuesAndNames.Values[index];
                this.CurrentSchema.Enum.Add(JToken.FromObject(Enum.ToObject(type, num)));
              }
              break;
            }
            break;
          case JsonContractType.String:
            this.CurrentSchema.Type = new JsonSchemaType?(!ReflectionUtils.IsNullable(contract.UnderlyingType) ? JsonSchemaType.String : this.AddNullType(JsonSchemaType.String, valueRequired));
            break;
          case JsonContractType.Dictionary:
            this.CurrentSchema.Type = new JsonSchemaType?(this.AddNullType(JsonSchemaType.Object, valueRequired));
            Type keyType;
            Type valueType;
            ReflectionUtils.GetDictionaryKeyValueTypes(type, out keyType, out valueType);
            if (keyType != (Type) null && this.ContractResolver.ResolveContract(keyType).ContractType == JsonContractType.Primitive)
            {
              this.CurrentSchema.AdditionalProperties = this.GenerateInternal(valueType, Required.Default, false);
              break;
            }
            break;
          case JsonContractType.Dynamic:
          case JsonContractType.Linq:
            this.CurrentSchema.Type = new JsonSchemaType?(JsonSchemaType.Any);
            break;
          case JsonContractType.Serializable:
            this.CurrentSchema.Type = new JsonSchemaType?(this.AddNullType(JsonSchemaType.Object, valueRequired));
            this.CurrentSchema.Id = this.GetTypeId(type, false);
            this.GenerateISerializableContract(type, (JsonISerializableContract) contract);
            break;
          default:
            throw new JsonException("Unexpected contract type: {0}".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) contract));
        }
      }
      return this.Pop().Schema;
    }

    private JsonSchemaType AddNullType(JsonSchemaType type, Required valueRequired) => valueRequired != Required.Always ? type | JsonSchemaType.Null : type;

    private bool HasFlag(DefaultValueHandling value, DefaultValueHandling flag) => (value & flag) == flag;

    private void GenerateObjectSchema(Type type, JsonObjectContract contract)
    {
      this.CurrentSchema.Properties = (IDictionary<string, JsonSchema>) new Dictionary<string, JsonSchema>();
      foreach (JsonProperty property in (Collection<JsonProperty>) contract.Properties)
      {
        if (!property.Ignored)
        {
          NullValueHandling? nullValueHandling1 = property.NullValueHandling;
          NullValueHandling nullValueHandling2 = NullValueHandling.Ignore;
          bool flag = nullValueHandling1.GetValueOrDefault() == nullValueHandling2 & nullValueHandling1.HasValue || this.HasFlag(property.DefaultValueHandling.GetValueOrDefault(), DefaultValueHandling.Ignore) || property.ShouldSerialize != null || property.GetIsSpecified != null;
          JsonSchema jsonSchema = this.GenerateInternal(property.PropertyType, property.Required, !flag);
          if (property.DefaultValue != null)
            jsonSchema.Default = JToken.FromObject(property.DefaultValue);
          this.CurrentSchema.Properties.Add(property.PropertyName, jsonSchema);
        }
      }
      if (!type.IsSealed())
        return;
      this.CurrentSchema.AllowAdditionalProperties = false;
    }

    private void GenerateISerializableContract(Type type, JsonISerializableContract contract) => this.CurrentSchema.AllowAdditionalProperties = true;

    internal static bool HasFlag(JsonSchemaType? value, JsonSchemaType flag)
    {
      if (!value.HasValue)
        return true;
      JsonSchemaType? nullable1 = value;
      JsonSchemaType jsonSchemaType1 = flag;
      JsonSchemaType? nullable2 = nullable1.HasValue ? new JsonSchemaType?(nullable1.GetValueOrDefault() & jsonSchemaType1) : new JsonSchemaType?();
      JsonSchemaType jsonSchemaType2 = flag;
      if (nullable2.GetValueOrDefault() == jsonSchemaType2 & nullable2.HasValue)
        return true;
      if (flag == JsonSchemaType.Integer)
      {
        nullable1 = value;
        JsonSchemaType? nullable3 = nullable1.HasValue ? new JsonSchemaType?(nullable1.GetValueOrDefault() & JsonSchemaType.Float) : new JsonSchemaType?();
        JsonSchemaType jsonSchemaType3 = JsonSchemaType.Float;
        if (nullable3.GetValueOrDefault() == jsonSchemaType3 & nullable3.HasValue)
          return true;
      }
      return false;
    }

    private JsonSchemaType GetJsonSchemaType(Type type, Required valueRequired)
    {
      JsonSchemaType jsonSchemaType = JsonSchemaType.None;
      if (valueRequired != Required.Always && ReflectionUtils.IsNullable(type))
      {
        jsonSchemaType = JsonSchemaType.Null;
        if (ReflectionUtils.IsNullableType(type))
          type = Nullable.GetUnderlyingType(type);
      }
      PrimitiveTypeCode typeCode = ConvertUtils.GetTypeCode(type);
      switch (typeCode)
      {
        case PrimitiveTypeCode.Empty:
        case PrimitiveTypeCode.Object:
          return jsonSchemaType | JsonSchemaType.String;
        case PrimitiveTypeCode.Char:
          return jsonSchemaType | JsonSchemaType.String;
        case PrimitiveTypeCode.Boolean:
          return jsonSchemaType | JsonSchemaType.Boolean;
        case PrimitiveTypeCode.SByte:
        case PrimitiveTypeCode.Int16:
        case PrimitiveTypeCode.UInt16:
        case PrimitiveTypeCode.Int32:
        case PrimitiveTypeCode.Byte:
        case PrimitiveTypeCode.UInt32:
        case PrimitiveTypeCode.Int64:
        case PrimitiveTypeCode.UInt64:
        case PrimitiveTypeCode.BigInteger:
          return jsonSchemaType | JsonSchemaType.Integer;
        case PrimitiveTypeCode.Single:
        case PrimitiveTypeCode.Double:
        case PrimitiveTypeCode.Decimal:
          return jsonSchemaType | JsonSchemaType.Float;
        case PrimitiveTypeCode.DateTime:
        case PrimitiveTypeCode.DateTimeOffset:
          return jsonSchemaType | JsonSchemaType.String;
        case PrimitiveTypeCode.Guid:
        case PrimitiveTypeCode.TimeSpan:
        case PrimitiveTypeCode.Uri:
        case PrimitiveTypeCode.String:
        case PrimitiveTypeCode.Bytes:
          return jsonSchemaType | JsonSchemaType.String;
        case PrimitiveTypeCode.DBNull:
          return jsonSchemaType | JsonSchemaType.Null;
        default:
          throw new JsonException("Unexpected type code '{0}' for type '{1}'.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) typeCode, (object) type));
      }
    }

    private class TypeSchema
    {
      public Type Type { get; }

      public JsonSchema Schema { get; }

      public TypeSchema(Type type, JsonSchema schema)
      {
        ValidationUtils.ArgumentNotNull((object) type, nameof (type));
        ValidationUtils.ArgumentNotNull((object) schema, nameof (schema));
        this.Type = type;
        this.Schema = schema;
      }
    }
  }
}
