﻿// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.JsonSerializerInternalWriter
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\Newtonsoft.Json.dll

using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Security;


#nullable enable
namespace Newtonsoft.Json.Serialization
{
  internal class JsonSerializerInternalWriter : JsonSerializerInternalBase
  {
    private Type? _rootType;
    private int _rootLevel;
    private readonly List<object> _serializeStack = new List<object>();

    public JsonSerializerInternalWriter(JsonSerializer serializer)
      : base(serializer)
    {
    }

    public void Serialize(JsonWriter jsonWriter, object? value, Type? objectType)
    {
      if (jsonWriter == null)
        throw new ArgumentNullException(nameof (jsonWriter));
      this._rootType = objectType;
      this._rootLevel = this._serializeStack.Count + 1;
      JsonContract contractSafe = this.GetContractSafe(value);
      try
      {
        if (this.ShouldWriteReference(value, (JsonProperty) null, contractSafe, (JsonContainerContract) null, (JsonProperty) null))
          this.WriteReference(jsonWriter, value);
        else
          this.SerializeValue(jsonWriter, value, contractSafe, (JsonProperty) null, (JsonContainerContract) null, (JsonProperty) null);
      }
      catch (Exception ex)
      {
        if (this.IsErrorHandled((object) null, contractSafe, (object) null, (IJsonLineInfo) null, jsonWriter.Path, ex))
        {
          this.HandleError(jsonWriter, 0);
        }
        else
        {
          this.ClearErrorContext();
          throw;
        }
      }
      finally
      {
        this._rootType = (Type) null;
      }
    }

    private JsonSerializerProxy GetInternalSerializer()
    {
      if (this.InternalSerializer == null)
        this.InternalSerializer = new JsonSerializerProxy(this);
      return this.InternalSerializer;
    }

    private JsonContract? GetContractSafe(object? value) => value == null ? (JsonContract) null : this.GetContract(value);

    private JsonContract GetContract(object value) => this.Serializer._contractResolver.ResolveContract(value.GetType());

    private void SerializePrimitive(
      JsonWriter writer,
      object value,
      JsonPrimitiveContract contract,
      JsonProperty? member,
      JsonContainerContract? containerContract,
      JsonProperty? containerProperty)
    {
      if (contract.TypeCode == PrimitiveTypeCode.Bytes && this.ShouldWriteType(TypeNameHandling.Objects, (JsonContract) contract, member, containerContract, containerProperty))
      {
        writer.WriteStartObject();
        this.WriteTypeProperty(writer, contract.CreatedType);
        writer.WritePropertyName("$value", false);
        JsonWriter.WriteValue(writer, contract.TypeCode, value);
        writer.WriteEndObject();
      }
      else
        JsonWriter.WriteValue(writer, contract.TypeCode, value);
    }

    private void SerializeValue(
      JsonWriter writer,
      object? value,
      JsonContract? valueContract,
      JsonProperty? member,
      JsonContainerContract? containerContract,
      JsonProperty? containerProperty)
    {
      if (value == null)
      {
        writer.WriteNull();
      }
      else
      {
        JsonConverter converter = member?.Converter ?? containerProperty?.ItemConverter ?? containerContract?.ItemConverter ?? valueContract.Converter ?? this.Serializer.GetMatchingConverter(valueContract.UnderlyingType) ?? valueContract.InternalConverter;
        if (converter != null && converter.CanWrite)
        {
          this.SerializeConvertable(writer, converter, value, valueContract, containerContract, containerProperty);
        }
        else
        {
          switch (valueContract.ContractType)
          {
            case JsonContractType.Object:
              this.SerializeObject(writer, value, (JsonObjectContract) valueContract, member, containerContract, containerProperty);
              break;
            case JsonContractType.Array:
              JsonArrayContract contract1 = (JsonArrayContract) valueContract;
              if (!contract1.IsMultidimensionalArray)
              {
                this.SerializeList(writer, (IEnumerable) value, contract1, member, containerContract, containerProperty);
                break;
              }
              this.SerializeMultidimensionalArray(writer, (Array) value, contract1, member, containerContract, containerProperty);
              break;
            case JsonContractType.Primitive:
              this.SerializePrimitive(writer, value, (JsonPrimitiveContract) valueContract, member, containerContract, containerProperty);
              break;
            case JsonContractType.String:
              this.SerializeString(writer, value, (JsonStringContract) valueContract);
              break;
            case JsonContractType.Dictionary:
              JsonDictionaryContract contract2 = (JsonDictionaryContract) valueContract;
              this.SerializeDictionary(writer, value is IDictionary dictionary ? dictionary : (IDictionary) contract2.CreateWrapper(value), contract2, member, containerContract, containerProperty);
              break;
            case JsonContractType.Dynamic:
              this.SerializeDynamic(writer, (IDynamicMetaObjectProvider) value, (JsonDynamicContract) valueContract, member, containerContract, containerProperty);
              break;
            case JsonContractType.Serializable:
              this.SerializeISerializable(writer, (ISerializable) value, (JsonISerializableContract) valueContract, member, containerContract, containerProperty);
              break;
            case JsonContractType.Linq:
              ((JToken) value).WriteTo(writer, this.Serializer.Converters.ToArray<JsonConverter>());
              break;
          }
        }
      }
    }

    private bool? ResolveIsReference(
      JsonContract contract,
      JsonProperty? property,
      JsonContainerContract? collectionContract,
      JsonProperty? containerProperty)
    {
      bool? nullable = new bool?();
      if (property != null)
        nullable = property.IsReference;
      if (!nullable.HasValue && containerProperty != null)
        nullable = containerProperty.ItemIsReference;
      if (!nullable.HasValue && collectionContract != null)
        nullable = collectionContract.ItemIsReference;
      if (!nullable.HasValue)
        nullable = contract.IsReference;
      return nullable;
    }

    private bool ShouldWriteReference(
      object? value,
      JsonProperty? property,
      JsonContract? valueContract,
      JsonContainerContract? collectionContract,
      JsonProperty? containerProperty)
    {
      if (value == null || valueContract.ContractType == JsonContractType.Primitive || valueContract.ContractType == JsonContractType.String)
        return false;
      bool? nullable = this.ResolveIsReference(valueContract, property, collectionContract, containerProperty);
      if (!nullable.HasValue)
        nullable = valueContract.ContractType != JsonContractType.Array ? new bool?(this.HasFlag(this.Serializer._preserveReferencesHandling, PreserveReferencesHandling.Objects)) : new bool?(this.HasFlag(this.Serializer._preserveReferencesHandling, PreserveReferencesHandling.Arrays));
      return nullable.GetValueOrDefault() && this.Serializer.GetReferenceResolver().IsReferenced((object) this, value);
    }

    private bool ShouldWriteProperty(
      object? memberValue,
      JsonObjectContract? containerContract,
      JsonProperty property)
    {
      return (memberValue != null || this.ResolvedNullValueHandling(containerContract, property) != NullValueHandling.Ignore) && (!this.HasFlag(property.DefaultValueHandling.GetValueOrDefault(this.Serializer._defaultValueHandling), DefaultValueHandling.Ignore) || !MiscellaneousUtils.ValueEquals(memberValue, property.GetResolvedDefaultValue()));
    }

    private bool CheckForCircularReference(
      JsonWriter writer,
      object? value,
      JsonProperty? property,
      JsonContract? contract,
      JsonContainerContract? containerContract,
      JsonProperty? containerProperty)
    {
      if (value == null || contract.ContractType == JsonContractType.Primitive || contract.ContractType == JsonContractType.String)
        return true;
      ReferenceLoopHandling? nullable = new ReferenceLoopHandling?();
      if (property != null)
        nullable = property.ReferenceLoopHandling;
      if (!nullable.HasValue && containerProperty != null)
        nullable = containerProperty.ItemReferenceLoopHandling;
      if (!nullable.HasValue && containerContract != null)
        nullable = containerContract.ItemReferenceLoopHandling;
      if ((this.Serializer._equalityComparer != null ? (this._serializeStack.Contains<object>(value, this.Serializer._equalityComparer) ? 1 : 0) : (this._serializeStack.Contains(value) ? 1 : 0)) != 0)
      {
        string str = "Self referencing loop detected";
        if (property != null)
          str += " for property '{0}'".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) property.PropertyName);
        string message = str + " with type '{0}'.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) value.GetType());
        switch (nullable.GetValueOrDefault(this.Serializer._referenceLoopHandling))
        {
          case ReferenceLoopHandling.Error:
            throw JsonSerializationException.Create((IJsonLineInfo) null, writer.ContainerPath, message, (Exception) null);
          case ReferenceLoopHandling.Ignore:
            if (this.TraceWriter != null && this.TraceWriter.LevelFilter >= TraceLevel.Verbose)
              this.TraceWriter.Trace(TraceLevel.Verbose, JsonPosition.FormatMessage((IJsonLineInfo) null, writer.Path, message + ". Skipping serializing self referenced value."), (Exception) null);
            return false;
          case ReferenceLoopHandling.Serialize:
            if (this.TraceWriter != null && this.TraceWriter.LevelFilter >= TraceLevel.Verbose)
              this.TraceWriter.Trace(TraceLevel.Verbose, JsonPosition.FormatMessage((IJsonLineInfo) null, writer.Path, message + ". Serializing self referenced value."), (Exception) null);
            return true;
        }
      }
      return true;
    }

    private void WriteReference(JsonWriter writer, object value)
    {
      string reference = this.GetReference(writer, value);
      if (this.TraceWriter != null && this.TraceWriter.LevelFilter >= TraceLevel.Info)
        this.TraceWriter.Trace(TraceLevel.Info, JsonPosition.FormatMessage((IJsonLineInfo) null, writer.Path, "Writing object reference to Id '{0}' for {1}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) reference, (object) value.GetType())), (Exception) null);
      writer.WriteStartObject();
      writer.WritePropertyName("$ref", false);
      writer.WriteValue(reference);
      writer.WriteEndObject();
    }

    private string GetReference(JsonWriter writer, object value)
    {
      try
      {
        return this.Serializer.GetReferenceResolver().GetReference((object) this, value);
      }
      catch (Exception ex)
      {
        throw JsonSerializationException.Create((IJsonLineInfo) null, writer.ContainerPath, "Error writing object reference for '{0}'.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) value.GetType()), ex);
      }
    }

    internal static bool TryConvertToString(object value, Type type, [NotNullWhen(true)] out string? s)
    {
      TypeConverter typeConverter;
      if (JsonTypeReflector.CanTypeDescriptorConvertString(type, out typeConverter))
      {
        s = typeConverter.ConvertToInvariantString(value);
        return true;
      }
      Type type1 = value as Type;
      if ((object) type1 != null)
      {
        s = type1.AssemblyQualifiedName;
        return true;
      }
      s = (string) null;
      return false;
    }

    private void SerializeString(JsonWriter writer, object value, JsonStringContract contract)
    {
      this.OnSerializing(writer, (JsonContract) contract, value);
      string s;
      JsonSerializerInternalWriter.TryConvertToString(value, contract.UnderlyingType, out s);
      writer.WriteValue(s);
      this.OnSerialized(writer, (JsonContract) contract, value);
    }

    private void OnSerializing(JsonWriter writer, JsonContract contract, object value)
    {
      if (this.TraceWriter != null && this.TraceWriter.LevelFilter >= TraceLevel.Info)
        this.TraceWriter.Trace(TraceLevel.Info, JsonPosition.FormatMessage((IJsonLineInfo) null, writer.Path, "Started serializing {0}".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) contract.UnderlyingType)), (Exception) null);
      contract.InvokeOnSerializing(value, this.Serializer._context);
    }

    private void OnSerialized(JsonWriter writer, JsonContract contract, object value)
    {
      if (this.TraceWriter != null && this.TraceWriter.LevelFilter >= TraceLevel.Info)
        this.TraceWriter.Trace(TraceLevel.Info, JsonPosition.FormatMessage((IJsonLineInfo) null, writer.Path, "Finished serializing {0}".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) contract.UnderlyingType)), (Exception) null);
      contract.InvokeOnSerialized(value, this.Serializer._context);
    }

    private void SerializeObject(
      JsonWriter writer,
      object value,
      JsonObjectContract contract,
      JsonProperty? member,
      JsonContainerContract? collectionContract,
      JsonProperty? containerProperty)
    {
      this.OnSerializing(writer, (JsonContract) contract, value);
      this._serializeStack.Add(value);
      this.WriteObjectStart(writer, value, (JsonContract) contract, member, collectionContract, containerProperty);
      int top = writer.Top;
      for (int index = 0; index < contract.Properties.Count; ++index)
      {
        JsonProperty property = contract.Properties[index];
        try
        {
          JsonContract memberContract;
          object memberValue;
          if (this.CalculatePropertyValues(writer, value, (JsonContainerContract) contract, member, property, out memberContract, out memberValue))
          {
            property.WritePropertyName(writer);
            this.SerializeValue(writer, memberValue, memberContract, property, (JsonContainerContract) contract, member);
          }
        }
        catch (Exception ex)
        {
          if (this.IsErrorHandled(value, (JsonContract) contract, (object) property.PropertyName, (IJsonLineInfo) null, writer.ContainerPath, ex))
            this.HandleError(writer, top);
          else
            throw;
        }
      }
      ExtensionDataGetter extensionDataGetter = contract.ExtensionDataGetter;
      IEnumerable<KeyValuePair<object, object>> keyValuePairs = extensionDataGetter != null ? extensionDataGetter(value) : (IEnumerable<KeyValuePair<object, object>>) null;
      if (keyValuePairs != null)
      {
        foreach (KeyValuePair<object, object> keyValuePair in keyValuePairs)
        {
          JsonContract contract1 = this.GetContract(keyValuePair.Key);
          JsonContract contractSafe = this.GetContractSafe(keyValuePair.Value);
          string propertyName = this.GetPropertyName(writer, keyValuePair.Key, contract1, out bool _);
          string name = contract.ExtensionDataNameResolver != null ? contract.ExtensionDataNameResolver(propertyName) : propertyName;
          if (this.ShouldWriteReference(keyValuePair.Value, (JsonProperty) null, contractSafe, (JsonContainerContract) contract, member))
          {
            writer.WritePropertyName(name);
            this.WriteReference(writer, keyValuePair.Value);
          }
          else if (this.CheckForCircularReference(writer, keyValuePair.Value, (JsonProperty) null, contractSafe, (JsonContainerContract) contract, member))
          {
            writer.WritePropertyName(name);
            this.SerializeValue(writer, keyValuePair.Value, contractSafe, (JsonProperty) null, (JsonContainerContract) contract, member);
          }
        }
      }
      writer.WriteEndObject();
      this._serializeStack.RemoveAt(this._serializeStack.Count - 1);
      this.OnSerialized(writer, (JsonContract) contract, value);
    }

    private bool CalculatePropertyValues(
      JsonWriter writer,
      object value,
      JsonContainerContract contract,
      JsonProperty? member,
      JsonProperty property,
      [NotNullWhen(true)] out JsonContract? memberContract,
      [NotNullWhen(true)] out object? memberValue)
    {
      if (!property.Ignored && property.Readable && this.ShouldSerialize(writer, property, value) && this.IsSpecified(writer, property, value))
      {
        if (property.PropertyContract == null)
          property.PropertyContract = this.Serializer._contractResolver.ResolveContract(property.PropertyType);
        memberValue = property.ValueProvider.GetValue(value);
        memberContract = property.PropertyContract.IsSealed ? property.PropertyContract : this.GetContractSafe(memberValue);
        if (this.ShouldWriteProperty(memberValue, contract as JsonObjectContract, property))
        {
          if (this.ShouldWriteReference(memberValue, property, memberContract, contract, member))
          {
            property.WritePropertyName(writer);
            this.WriteReference(writer, memberValue);
            return false;
          }
          if (!this.CheckForCircularReference(writer, memberValue, property, memberContract, contract, member))
            return false;
          if (memberValue == null)
          {
            JsonObjectContract jsonObjectContract = contract as JsonObjectContract;
            switch ((int) property._required ?? (int) ((Required?) jsonObjectContract?.ItemRequired).GetValueOrDefault())
            {
              case 2:
                throw JsonSerializationException.Create((IJsonLineInfo) null, writer.ContainerPath, "Cannot write a null value for property '{0}'. Property requires a value.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) property.PropertyName), (Exception) null);
              case 3:
                throw JsonSerializationException.Create((IJsonLineInfo) null, writer.ContainerPath, "Cannot write a null value for property '{0}'. Property requires a non-null value.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) property.PropertyName), (Exception) null);
            }
          }
          return true;
        }
      }
      memberContract = (JsonContract) null;
      memberValue = (object) null;
      return false;
    }

    private void WriteObjectStart(
      JsonWriter writer,
      object value,
      JsonContract contract,
      JsonProperty? member,
      JsonContainerContract? collectionContract,
      JsonProperty? containerProperty)
    {
      writer.WriteStartObject();
      if (((int) this.ResolveIsReference(contract, member, collectionContract, containerProperty) ?? (this.HasFlag(this.Serializer._preserveReferencesHandling, PreserveReferencesHandling.Objects) ? 1 : 0)) != 0 && (member == null || member.Writable || this.HasCreatorParameter(collectionContract, member)))
        this.WriteReferenceIdProperty(writer, contract.UnderlyingType, value);
      if (!this.ShouldWriteType(TypeNameHandling.Objects, contract, member, collectionContract, containerProperty))
        return;
      this.WriteTypeProperty(writer, contract.UnderlyingType);
    }

    private bool HasCreatorParameter(JsonContainerContract? contract, JsonProperty property) => contract is JsonObjectContract jsonObjectContract && jsonObjectContract.CreatorParameters.Contains(property.PropertyName);

    private void WriteReferenceIdProperty(JsonWriter writer, Type type, object value)
    {
      string reference = this.GetReference(writer, value);
      if (this.TraceWriter != null && this.TraceWriter.LevelFilter >= TraceLevel.Verbose)
        this.TraceWriter.Trace(TraceLevel.Verbose, JsonPosition.FormatMessage((IJsonLineInfo) null, writer.Path, "Writing object reference Id '{0}' for {1}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) reference, (object) type)), (Exception) null);
      writer.WritePropertyName("$id", false);
      writer.WriteValue(reference);
    }

    private void WriteTypeProperty(JsonWriter writer, Type type)
    {
      string typeName = ReflectionUtils.GetTypeName(type, this.Serializer._typeNameAssemblyFormatHandling, this.Serializer._serializationBinder);
      if (this.TraceWriter != null && this.TraceWriter.LevelFilter >= TraceLevel.Verbose)
        this.TraceWriter.Trace(TraceLevel.Verbose, JsonPosition.FormatMessage((IJsonLineInfo) null, writer.Path, "Writing type name '{0}' for {1}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) typeName, (object) type)), (Exception) null);
      writer.WritePropertyName("$type", false);
      writer.WriteValue(typeName);
    }

    private bool HasFlag(DefaultValueHandling value, DefaultValueHandling flag) => (value & flag) == flag;

    private bool HasFlag(PreserveReferencesHandling value, PreserveReferencesHandling flag) => (value & flag) == flag;

    private bool HasFlag(TypeNameHandling value, TypeNameHandling flag) => (value & flag) == flag;

    private void SerializeConvertable(
      JsonWriter writer,
      JsonConverter converter,
      object value,
      JsonContract contract,
      JsonContainerContract? collectionContract,
      JsonProperty? containerProperty)
    {
      if (this.ShouldWriteReference(value, (JsonProperty) null, contract, collectionContract, containerProperty))
      {
        this.WriteReference(writer, value);
      }
      else
      {
        if (!this.CheckForCircularReference(writer, value, (JsonProperty) null, contract, collectionContract, containerProperty))
          return;
        this._serializeStack.Add(value);
        if (this.TraceWriter != null && this.TraceWriter.LevelFilter >= TraceLevel.Info)
          this.TraceWriter.Trace(TraceLevel.Info, JsonPosition.FormatMessage((IJsonLineInfo) null, writer.Path, "Started serializing {0} with converter {1}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) value.GetType(), (object) converter.GetType())), (Exception) null);
        converter.WriteJson(writer, value, (JsonSerializer) this.GetInternalSerializer());
        if (this.TraceWriter != null && this.TraceWriter.LevelFilter >= TraceLevel.Info)
          this.TraceWriter.Trace(TraceLevel.Info, JsonPosition.FormatMessage((IJsonLineInfo) null, writer.Path, "Finished serializing {0} with converter {1}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) value.GetType(), (object) converter.GetType())), (Exception) null);
        this._serializeStack.RemoveAt(this._serializeStack.Count - 1);
      }
    }

    private void SerializeList(
      JsonWriter writer,
      IEnumerable values,
      JsonArrayContract contract,
      JsonProperty? member,
      JsonContainerContract? collectionContract,
      JsonProperty? containerProperty)
    {
      object obj1 = values is IWrappedCollection wrappedCollection ? wrappedCollection.UnderlyingCollection : (object) values;
      this.OnSerializing(writer, (JsonContract) contract, obj1);
      this._serializeStack.Add(obj1);
      bool flag = this.WriteStartArray(writer, obj1, contract, member, collectionContract, containerProperty);
      writer.WriteStartArray();
      int top = writer.Top;
      int keyValue = 0;
      foreach (object obj2 in values)
      {
        try
        {
          JsonContract jsonContract = contract.FinalItemContract ?? this.GetContractSafe(obj2);
          if (this.ShouldWriteReference(obj2, (JsonProperty) null, jsonContract, (JsonContainerContract) contract, member))
            this.WriteReference(writer, obj2);
          else if (this.CheckForCircularReference(writer, obj2, (JsonProperty) null, jsonContract, (JsonContainerContract) contract, member))
            this.SerializeValue(writer, obj2, jsonContract, (JsonProperty) null, (JsonContainerContract) contract, member);
        }
        catch (Exception ex)
        {
          if (this.IsErrorHandled(obj1, (JsonContract) contract, (object) keyValue, (IJsonLineInfo) null, writer.ContainerPath, ex))
            this.HandleError(writer, top);
          else
            throw;
        }
        finally
        {
          ++keyValue;
        }
      }
      writer.WriteEndArray();
      if (flag)
        writer.WriteEndObject();
      this._serializeStack.RemoveAt(this._serializeStack.Count - 1);
      this.OnSerialized(writer, (JsonContract) contract, obj1);
    }

    private void SerializeMultidimensionalArray(
      JsonWriter writer,
      Array values,
      JsonArrayContract contract,
      JsonProperty? member,
      JsonContainerContract? collectionContract,
      JsonProperty? containerProperty)
    {
      this.OnSerializing(writer, (JsonContract) contract, (object) values);
      this._serializeStack.Add((object) values);
      int num = this.WriteStartArray(writer, (object) values, contract, member, collectionContract, containerProperty) ? 1 : 0;
      this.SerializeMultidimensionalArray(writer, values, contract, member, writer.Top, CollectionUtils.ArrayEmpty<int>());
      if (num != 0)
        writer.WriteEndObject();
      this._serializeStack.RemoveAt(this._serializeStack.Count - 1);
      this.OnSerialized(writer, (JsonContract) contract, (object) values);
    }

    private void SerializeMultidimensionalArray(
      JsonWriter writer,
      Array values,
      JsonArrayContract contract,
      JsonProperty? member,
      int initialDepth,
      int[] indices)
    {
      int length = indices.Length;
      int[] indices1 = new int[length + 1];
      for (int index = 0; index < length; ++index)
        indices1[index] = indices[index];
      writer.WriteStartArray();
      for (int lowerBound = values.GetLowerBound(length); lowerBound <= values.GetUpperBound(length); ++lowerBound)
      {
        indices1[length] = lowerBound;
        if (indices1.Length == values.Rank)
        {
          object obj = values.GetValue(indices1);
          try
          {
            JsonContract jsonContract = contract.FinalItemContract ?? this.GetContractSafe(obj);
            if (this.ShouldWriteReference(obj, (JsonProperty) null, jsonContract, (JsonContainerContract) contract, member))
              this.WriteReference(writer, obj);
            else if (this.CheckForCircularReference(writer, obj, (JsonProperty) null, jsonContract, (JsonContainerContract) contract, member))
              this.SerializeValue(writer, obj, jsonContract, (JsonProperty) null, (JsonContainerContract) contract, member);
          }
          catch (Exception ex)
          {
            if (this.IsErrorHandled((object) values, (JsonContract) contract, (object) lowerBound, (IJsonLineInfo) null, writer.ContainerPath, ex))
              this.HandleError(writer, initialDepth + 1);
            else
              throw;
          }
        }
        else
          this.SerializeMultidimensionalArray(writer, values, contract, member, initialDepth + 1, indices1);
      }
      writer.WriteEndArray();
    }

    private bool WriteStartArray(
      JsonWriter writer,
      object values,
      JsonArrayContract contract,
      JsonProperty? member,
      JsonContainerContract? containerContract,
      JsonProperty? containerProperty)
    {
      bool flag1 = ((int) this.ResolveIsReference((JsonContract) contract, member, containerContract, containerProperty) ?? (this.HasFlag(this.Serializer._preserveReferencesHandling, PreserveReferencesHandling.Arrays) ? 1 : 0)) != 0 && (member == null || member.Writable || this.HasCreatorParameter(containerContract, member));
      bool flag2 = this.ShouldWriteType(TypeNameHandling.Arrays, (JsonContract) contract, member, containerContract, containerProperty);
      int num = flag1 | flag2 ? 1 : 0;
      if (num != 0)
      {
        writer.WriteStartObject();
        if (flag1)
          this.WriteReferenceIdProperty(writer, contract.UnderlyingType, values);
        if (flag2)
          this.WriteTypeProperty(writer, values.GetType());
        writer.WritePropertyName("$values", false);
      }
      if (contract.ItemContract != null)
        return num != 0;
      JsonArrayContract jsonArrayContract = contract;
      IContractResolver contractResolver = this.Serializer._contractResolver;
      Type type = contract.CollectionItemType;
      if ((object) type == null)
        type = typeof (object);
      JsonContract jsonContract = contractResolver.ResolveContract(type);
      jsonArrayContract.ItemContract = jsonContract;
      return num != 0;
    }

    [SecuritySafeCritical]
    private void SerializeISerializable(
      JsonWriter writer,
      ISerializable value,
      JsonISerializableContract contract,
      JsonProperty? member,
      JsonContainerContract? collectionContract,
      JsonProperty? containerProperty)
    {
      if (!JsonTypeReflector.FullyTrusted)
      {
        string message = ("Type '{0}' implements ISerializable but cannot be serialized using the ISerializable interface because the current application is not fully trusted and ISerializable can expose secure data." + Environment.NewLine + "To fix this error either change the environment to be fully trusted, change the application to not deserialize the type, add JsonObjectAttribute to the type or change the JsonSerializer setting ContractResolver to use a new DefaultContractResolver with IgnoreSerializableInterface set to true." + Environment.NewLine).FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) value.GetType());
        throw JsonSerializationException.Create((IJsonLineInfo) null, writer.ContainerPath, message, (Exception) null);
      }
      this.OnSerializing(writer, (JsonContract) contract, (object) value);
      this._serializeStack.Add((object) value);
      this.WriteObjectStart(writer, (object) value, (JsonContract) contract, member, collectionContract, containerProperty);
      SerializationInfo info = new SerializationInfo(contract.UnderlyingType, (IFormatterConverter) new FormatterConverter());
      value.GetObjectData(info, this.Serializer._context);
      foreach (SerializationEntry serializationEntry in info)
      {
        JsonContract contractSafe = this.GetContractSafe(serializationEntry.Value);
        if (this.ShouldWriteReference(serializationEntry.Value, (JsonProperty) null, contractSafe, (JsonContainerContract) contract, member))
        {
          writer.WritePropertyName(serializationEntry.Name);
          this.WriteReference(writer, serializationEntry.Value);
        }
        else if (this.CheckForCircularReference(writer, serializationEntry.Value, (JsonProperty) null, contractSafe, (JsonContainerContract) contract, member))
        {
          writer.WritePropertyName(serializationEntry.Name);
          this.SerializeValue(writer, serializationEntry.Value, contractSafe, (JsonProperty) null, (JsonContainerContract) contract, member);
        }
      }
      writer.WriteEndObject();
      this._serializeStack.RemoveAt(this._serializeStack.Count - 1);
      this.OnSerialized(writer, (JsonContract) contract, (object) value);
    }

    private void SerializeDynamic(
      JsonWriter writer,
      IDynamicMetaObjectProvider value,
      JsonDynamicContract contract,
      JsonProperty? member,
      JsonContainerContract? collectionContract,
      JsonProperty? containerProperty)
    {
      this.OnSerializing(writer, (JsonContract) contract, (object) value);
      this._serializeStack.Add((object) value);
      this.WriteObjectStart(writer, (object) value, (JsonContract) contract, member, collectionContract, containerProperty);
      int top = writer.Top;
      for (int index = 0; index < contract.Properties.Count; ++index)
      {
        JsonProperty property = contract.Properties[index];
        if (property.HasMemberAttribute)
        {
          try
          {
            JsonContract memberContract;
            object memberValue;
            if (this.CalculatePropertyValues(writer, (object) value, (JsonContainerContract) contract, member, property, out memberContract, out memberValue))
            {
              property.WritePropertyName(writer);
              this.SerializeValue(writer, memberValue, memberContract, property, (JsonContainerContract) contract, member);
            }
          }
          catch (Exception ex)
          {
            if (this.IsErrorHandled((object) value, (JsonContract) contract, (object) property.PropertyName, (IJsonLineInfo) null, writer.ContainerPath, ex))
              this.HandleError(writer, top);
            else
              throw;
          }
        }
      }
      foreach (string dynamicMemberName in value.GetDynamicMemberNames())
      {
        object memberValue;
        if (contract.TryGetMember(value, dynamicMemberName, out memberValue))
        {
          try
          {
            JsonContract contractSafe = this.GetContractSafe(memberValue);
            if (this.ShouldWriteDynamicProperty(memberValue))
            {
              if (this.CheckForCircularReference(writer, memberValue, (JsonProperty) null, contractSafe, (JsonContainerContract) contract, member))
              {
                string name = contract.PropertyNameResolver != null ? contract.PropertyNameResolver(dynamicMemberName) : dynamicMemberName;
                writer.WritePropertyName(name);
                this.SerializeValue(writer, memberValue, contractSafe, (JsonProperty) null, (JsonContainerContract) contract, member);
              }
            }
          }
          catch (Exception ex)
          {
            if (this.IsErrorHandled((object) value, (JsonContract) contract, (object) dynamicMemberName, (IJsonLineInfo) null, writer.ContainerPath, ex))
              this.HandleError(writer, top);
            else
              throw;
          }
        }
      }
      writer.WriteEndObject();
      this._serializeStack.RemoveAt(this._serializeStack.Count - 1);
      this.OnSerialized(writer, (JsonContract) contract, (object) value);
    }

    private bool ShouldWriteDynamicProperty(object? memberValue) => (this.Serializer._nullValueHandling != NullValueHandling.Ignore || memberValue != null) && (!this.HasFlag(this.Serializer._defaultValueHandling, DefaultValueHandling.Ignore) || memberValue != null && !MiscellaneousUtils.ValueEquals(memberValue, ReflectionUtils.GetDefaultValue(memberValue.GetType())));

    private bool ShouldWriteType(
      TypeNameHandling typeNameHandlingFlag,
      JsonContract contract,
      JsonProperty? member,
      JsonContainerContract? containerContract,
      JsonProperty? containerProperty)
    {
      TypeNameHandling typeNameHandling = (TypeNameHandling) ((int) member?.TypeNameHandling ?? (int) containerProperty?.ItemTypeNameHandling ?? (int) containerContract?.ItemTypeNameHandling ?? (int) this.Serializer._typeNameHandling);
      if (this.HasFlag(typeNameHandling, typeNameHandlingFlag))
        return true;
      if (this.HasFlag(typeNameHandling, TypeNameHandling.Auto))
      {
        if (member != null)
        {
          if (contract.NonNullableUnderlyingType != member.PropertyContract.CreatedType)
            return true;
        }
        else if (containerContract != null)
        {
          if (containerContract.ItemContract == null || contract.NonNullableUnderlyingType != containerContract.ItemContract.CreatedType)
            return true;
        }
        else if (this._rootType != (Type) null && this._serializeStack.Count == this._rootLevel)
        {
          JsonContract jsonContract = this.Serializer._contractResolver.ResolveContract(this._rootType);
          if (contract.NonNullableUnderlyingType != jsonContract.CreatedType)
            return true;
        }
      }
      return false;
    }

    private void SerializeDictionary(
      JsonWriter writer,
      IDictionary values,
      JsonDictionaryContract contract,
      JsonProperty? member,
      JsonContainerContract? collectionContract,
      JsonProperty? containerProperty)
    {
      object currentObject = values is IWrappedDictionary wrappedDictionary ? wrappedDictionary.UnderlyingDictionary : (object) values;
      this.OnSerializing(writer, (JsonContract) contract, currentObject);
      this._serializeStack.Add(currentObject);
      this.WriteObjectStart(writer, currentObject, (JsonContract) contract, member, collectionContract, containerProperty);
      if (contract.ItemContract == null)
      {
        JsonDictionaryContract dictionaryContract = contract;
        IContractResolver contractResolver = this.Serializer._contractResolver;
        Type type = contract.DictionaryValueType;
        if ((object) type == null)
          type = typeof (object);
        JsonContract jsonContract = contractResolver.ResolveContract(type);
        dictionaryContract.ItemContract = jsonContract;
      }
      if (contract.KeyContract == null)
      {
        JsonDictionaryContract dictionaryContract = contract;
        IContractResolver contractResolver = this.Serializer._contractResolver;
        Type type = contract.DictionaryKeyType;
        if ((object) type == null)
          type = typeof (object);
        JsonContract jsonContract = contractResolver.ResolveContract(type);
        dictionaryContract.KeyContract = jsonContract;
      }
      int top = writer.Top;
      IDictionaryEnumerator enumerator = values.GetEnumerator();
      try
      {
        while (enumerator.MoveNext())
        {
          DictionaryEntry entry = enumerator.Entry;
          bool escape;
          string propertyName = this.GetPropertyName(writer, entry.Key, contract.KeyContract, out escape);
          string str = contract.DictionaryKeyResolver != null ? contract.DictionaryKeyResolver(propertyName) : propertyName;
          try
          {
            object obj = entry.Value;
            JsonContract jsonContract = contract.FinalItemContract ?? this.GetContractSafe(obj);
            if (this.ShouldWriteReference(obj, (JsonProperty) null, jsonContract, (JsonContainerContract) contract, member))
            {
              writer.WritePropertyName(str, escape);
              this.WriteReference(writer, obj);
            }
            else if (this.CheckForCircularReference(writer, obj, (JsonProperty) null, jsonContract, (JsonContainerContract) contract, member))
            {
              writer.WritePropertyName(str, escape);
              this.SerializeValue(writer, obj, jsonContract, (JsonProperty) null, (JsonContainerContract) contract, member);
            }
          }
          catch (Exception ex)
          {
            if (this.IsErrorHandled(currentObject, (JsonContract) contract, (object) str, (IJsonLineInfo) null, writer.ContainerPath, ex))
              this.HandleError(writer, top);
            else
              throw;
          }
        }
      }
      finally
      {
        if (enumerator is IDisposable disposable)
          disposable.Dispose();
      }
      writer.WriteEndObject();
      this._serializeStack.RemoveAt(this._serializeStack.Count - 1);
      this.OnSerialized(writer, (JsonContract) contract, currentObject);
    }

    private string GetPropertyName(
      JsonWriter writer,
      object name,
      JsonContract contract,
      out bool escape)
    {
      if (contract.ContractType == JsonContractType.Primitive)
      {
        JsonPrimitiveContract primitiveContract = (JsonPrimitiveContract) contract;
        switch (primitiveContract.TypeCode)
        {
          case PrimitiveTypeCode.Single:
          case PrimitiveTypeCode.SingleNullable:
            float num1 = (float) name;
            escape = false;
            return num1.ToString("R", (IFormatProvider) CultureInfo.InvariantCulture);
          case PrimitiveTypeCode.Double:
          case PrimitiveTypeCode.DoubleNullable:
            double num2 = (double) name;
            escape = false;
            return num2.ToString("R", (IFormatProvider) CultureInfo.InvariantCulture);
          case PrimitiveTypeCode.DateTime:
          case PrimitiveTypeCode.DateTimeNullable:
            DateTime dateTime = DateTimeUtils.EnsureDateTime((DateTime) name, writer.DateTimeZoneHandling);
            escape = false;
            StringWriter writer1 = new StringWriter((IFormatProvider) CultureInfo.InvariantCulture);
            DateTimeUtils.WriteDateTimeString((TextWriter) writer1, dateTime, writer.DateFormatHandling, writer.DateFormatString, writer.Culture);
            return writer1.ToString();
          case PrimitiveTypeCode.DateTimeOffset:
          case PrimitiveTypeCode.DateTimeOffsetNullable:
            escape = false;
            StringWriter writer2 = new StringWriter((IFormatProvider) CultureInfo.InvariantCulture);
            DateTimeUtils.WriteDateTimeOffsetString((TextWriter) writer2, (DateTimeOffset) name, writer.DateFormatHandling, writer.DateFormatString, writer.Culture);
            return writer2.ToString();
          default:
            escape = true;
            string name1;
            return primitiveContract.IsEnum && EnumUtils.TryToString(primitiveContract.NonNullableUnderlyingType, name, (NamingStrategy) null, out name1) ? name1 : Convert.ToString(name, (IFormatProvider) CultureInfo.InvariantCulture);
        }
      }
      else
      {
        string s;
        if (JsonSerializerInternalWriter.TryConvertToString(name, name.GetType(), out s))
        {
          escape = true;
          return s;
        }
        escape = true;
        return name.ToString();
      }
    }

    private void HandleError(JsonWriter writer, int initialDepth)
    {
      this.ClearErrorContext();
      if (writer.WriteState == WriteState.Property)
        writer.WriteNull();
      while (writer.Top > initialDepth)
        writer.WriteEnd();
    }

    private bool ShouldSerialize(JsonWriter writer, JsonProperty property, object target)
    {
      if (property.ShouldSerialize == null)
        return true;
      bool flag = property.ShouldSerialize(target);
      if (this.TraceWriter != null && this.TraceWriter.LevelFilter >= TraceLevel.Verbose)
        this.TraceWriter.Trace(TraceLevel.Verbose, JsonPosition.FormatMessage((IJsonLineInfo) null, writer.Path, "ShouldSerialize result for property '{0}' on {1}: {2}".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) property.PropertyName, (object) property.DeclaringType, (object) flag)), (Exception) null);
      return flag;
    }

    private bool IsSpecified(JsonWriter writer, JsonProperty property, object target)
    {
      if (property.GetIsSpecified == null)
        return true;
      bool flag = property.GetIsSpecified(target);
      if (this.TraceWriter != null && this.TraceWriter.LevelFilter >= TraceLevel.Verbose)
        this.TraceWriter.Trace(TraceLevel.Verbose, JsonPosition.FormatMessage((IJsonLineInfo) null, writer.Path, "IsSpecified result for property '{0}' on {1}: {2}".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) property.PropertyName, (object) property.DeclaringType, (object) flag)), (Exception) null);
      return flag;
    }
  }
}
