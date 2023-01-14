// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.KeyValuePairConverter
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\Newtonsoft.Json.dll

using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Reflection;


#nullable enable
namespace Newtonsoft.Json.Converters
{
  public class KeyValuePairConverter : JsonConverter
  {
    private const string KeyName = "Key";
    private const string ValueName = "Value";
    private static readonly ThreadSafeStore<Type, ReflectionObject> ReflectionObjectPerType = new ThreadSafeStore<Type, ReflectionObject>(new Func<Type, ReflectionObject>(KeyValuePairConverter.InitializeReflectionObject));

    private static ReflectionObject InitializeReflectionObject(Type t)
    {
      Type[] genericArguments = t.GetGenericArguments();
      Type type1 = ((IList<Type>) genericArguments)[0];
      Type type2 = ((IList<Type>) genericArguments)[1];
      return ReflectionObject.Create(t, (MethodBase) t.GetConstructor(new Type[2]
      {
        type1,
        type2
      }), "Key", "Value");
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
      if (value == null)
      {
        writer.WriteNull();
      }
      else
      {
        ReflectionObject reflectionObject = KeyValuePairConverter.ReflectionObjectPerType.Get(value.GetType());
        DefaultContractResolver contractResolver = serializer.ContractResolver as DefaultContractResolver;
        writer.WriteStartObject();
        writer.WritePropertyName(contractResolver != null ? contractResolver.GetResolvedPropertyName("Key") : "Key");
        serializer.Serialize(writer, reflectionObject.GetValue(value, "Key"), reflectionObject.GetType("Key"));
        writer.WritePropertyName(contractResolver != null ? contractResolver.GetResolvedPropertyName("Value") : "Value");
        serializer.Serialize(writer, reflectionObject.GetValue(value, "Value"), reflectionObject.GetType("Value"));
        writer.WriteEndObject();
      }
    }

    public override object? ReadJson(
      JsonReader reader,
      Type objectType,
      object? existingValue,
      JsonSerializer serializer)
    {
      if (reader.TokenType == JsonToken.Null)
      {
        if (!ReflectionUtils.IsNullableType(objectType))
          throw JsonSerializationException.Create(reader, "Cannot convert null value to KeyValuePair.");
        return (object) null;
      }
      object obj1 = (object) null;
      object obj2 = (object) null;
      reader.ReadAndAssert();
      Type key = ReflectionUtils.IsNullableType(objectType) ? Nullable.GetUnderlyingType(objectType) : objectType;
      ReflectionObject reflectionObject = KeyValuePairConverter.ReflectionObjectPerType.Get(key);
      JsonContract contract1 = serializer.ContractResolver.ResolveContract(reflectionObject.GetType("Key"));
      JsonContract contract2 = serializer.ContractResolver.ResolveContract(reflectionObject.GetType("Value"));
      while (reader.TokenType == JsonToken.PropertyName)
      {
        string a = reader.Value.ToString();
        if (string.Equals(a, "Key", StringComparison.OrdinalIgnoreCase))
        {
          reader.ReadForTypeAndAssert(contract1, false);
          obj1 = serializer.Deserialize(reader, contract1.UnderlyingType);
        }
        else if (string.Equals(a, "Value", StringComparison.OrdinalIgnoreCase))
        {
          reader.ReadForTypeAndAssert(contract2, false);
          obj2 = serializer.Deserialize(reader, contract2.UnderlyingType);
        }
        else
          reader.Skip();
        reader.ReadAndAssert();
      }
      return reflectionObject.Creator(new object[2]
      {
        obj1,
        obj2
      });
    }

    public override bool CanConvert(Type objectType)
    {
      Type type = ReflectionUtils.IsNullableType(objectType) ? Nullable.GetUnderlyingType(objectType) : objectType;
      return type.IsValueType() && type.IsGenericType() && type.GetGenericTypeDefinition() == typeof (KeyValuePair<,>);
    }
  }
}
