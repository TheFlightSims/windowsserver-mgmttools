// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.DefaultContractResolver
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\Newtonsoft.Json.dll

using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;


#nullable enable
namespace Newtonsoft.Json.Serialization
{
  public class DefaultContractResolver : IContractResolver
  {
    private static readonly IContractResolver _instance = (IContractResolver) new DefaultContractResolver();
    private static readonly string[] BlacklistedTypeNames = new string[3]
    {
      "System.IO.DriveInfo",
      "System.IO.FileInfo",
      "System.IO.DirectoryInfo"
    };
    private static readonly JsonConverter[] BuiltInConverters = new JsonConverter[10]
    {
      (JsonConverter) new EntityKeyMemberConverter(),
      (JsonConverter) new ExpandoObjectConverter(),
      (JsonConverter) new XmlNodeConverter(),
      (JsonConverter) new BinaryConverter(),
      (JsonConverter) new DataSetConverter(),
      (JsonConverter) new DataTableConverter(),
      (JsonConverter) new DiscriminatedUnionConverter(),
      (JsonConverter) new KeyValuePairConverter(),
      (JsonConverter) new BsonObjectIdConverter(),
      (JsonConverter) new RegexConverter()
    };
    private readonly DefaultJsonNameTable _nameTable = new DefaultJsonNameTable();
    private readonly ThreadSafeStore<Type, JsonContract> _contractCache;

    internal static IContractResolver Instance => DefaultContractResolver._instance;

    public bool DynamicCodeGeneration => JsonTypeReflector.DynamicCodeGeneration;

    [Obsolete("DefaultMembersSearchFlags is obsolete. To modify the members serialized inherit from DefaultContractResolver and override the GetSerializableMembers method instead.")]
    public BindingFlags DefaultMembersSearchFlags { get; set; }

    public bool SerializeCompilerGeneratedMembers { get; set; }

    public bool IgnoreSerializableInterface { get; set; }

    public bool IgnoreSerializableAttribute { get; set; }

    public bool IgnoreIsSpecifiedMembers { get; set; }

    public bool IgnoreShouldSerializeMembers { get; set; }

    public NamingStrategy? NamingStrategy { get; set; }

    public DefaultContractResolver()
    {
      this.IgnoreSerializableAttribute = true;
      this.DefaultMembersSearchFlags = BindingFlags.Instance | BindingFlags.Public;
      this._contractCache = new ThreadSafeStore<Type, JsonContract>(new Func<Type, JsonContract>(this.CreateContract));
    }

    public virtual JsonContract ResolveContract(Type type)
    {
      ValidationUtils.ArgumentNotNull((object) type, nameof (type));
      return this._contractCache.Get(type);
    }

    private static bool FilterMembers(MemberInfo member)
    {
      PropertyInfo property = member as PropertyInfo;
      if ((object) property != null)
        return !ReflectionUtils.IsIndexedProperty(property) && !ReflectionUtils.IsByRefLikeType(property.PropertyType);
      FieldInfo fieldInfo = member as FieldInfo;
      return (object) fieldInfo == null || !ReflectionUtils.IsByRefLikeType(fieldInfo.FieldType);
    }

    protected virtual List<MemberInfo> GetSerializableMembers(Type objectType)
    {
      bool serializableAttribute = this.IgnoreSerializableAttribute;
      MemberSerialization memberSerialization = JsonTypeReflector.GetObjectMemberSerialization(objectType, serializableAttribute);
      IEnumerable<MemberInfo> memberInfos = ReflectionUtils.GetFieldsAndProperties(objectType, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).Where<MemberInfo>((Func<MemberInfo, bool>) (m =>
      {
        PropertyInfo property = m as PropertyInfo;
        return (object) property == null || !ReflectionUtils.IsIndexedProperty(property);
      }));
      List<MemberInfo> source = new List<MemberInfo>();
      if (memberSerialization != MemberSerialization.Fields)
      {
        DataContractAttribute contractAttribute = JsonTypeReflector.GetDataContractAttribute(objectType);
        List<MemberInfo> list = ReflectionUtils.GetFieldsAndProperties(objectType, this.DefaultMembersSearchFlags).Where<MemberInfo>(new Func<MemberInfo, bool>(DefaultContractResolver.FilterMembers)).ToList<MemberInfo>();
        foreach (MemberInfo memberInfo in memberInfos)
        {
          if (this.SerializeCompilerGeneratedMembers || !memberInfo.IsDefined(typeof (CompilerGeneratedAttribute), true))
          {
            if (list.Contains(memberInfo))
              source.Add(memberInfo);
            else if (JsonTypeReflector.GetAttribute<JsonPropertyAttribute>((object) memberInfo) != null)
              source.Add(memberInfo);
            else if (JsonTypeReflector.GetAttribute<JsonRequiredAttribute>((object) memberInfo) != null)
              source.Add(memberInfo);
            else if (contractAttribute != null && JsonTypeReflector.GetAttribute<DataMemberAttribute>((object) memberInfo) != null)
              source.Add(memberInfo);
            else if (memberSerialization == MemberSerialization.Fields && memberInfo.MemberType() == MemberTypes.Field)
              source.Add(memberInfo);
          }
        }
        if (objectType.AssignableToTypeName("System.Data.Objects.DataClasses.EntityObject", false, out Type _))
          source = source.Where<MemberInfo>(new Func<MemberInfo, bool>(this.ShouldSerializeEntityMember)).ToList<MemberInfo>();
        if (typeof (Exception).IsAssignableFrom(objectType))
          source = source.Where<MemberInfo>((Func<MemberInfo, bool>) (m => !string.Equals(m.Name, "TargetSite", StringComparison.Ordinal))).ToList<MemberInfo>();
      }
      else
      {
        foreach (MemberInfo memberInfo in memberInfos)
        {
          FieldInfo fieldInfo = memberInfo as FieldInfo;
          if ((object) fieldInfo != null && !fieldInfo.IsStatic)
            source.Add(memberInfo);
        }
      }
      return source;
    }

    private bool ShouldSerializeEntityMember(MemberInfo memberInfo)
    {
      PropertyInfo propertyInfo = memberInfo as PropertyInfo;
      return (object) propertyInfo == null || !propertyInfo.PropertyType.IsGenericType() || !(propertyInfo.PropertyType.GetGenericTypeDefinition().FullName == "System.Data.Objects.DataClasses.EntityReference`1");
    }

    protected virtual JsonObjectContract CreateObjectContract(Type objectType)
    {
      JsonObjectContract contract = new JsonObjectContract(objectType);
      this.InitializeContract((JsonContract) contract);
      bool serializableAttribute = this.IgnoreSerializableAttribute;
      contract.MemberSerialization = JsonTypeReflector.GetObjectMemberSerialization(contract.NonNullableUnderlyingType, serializableAttribute);
      contract.Properties.AddRange<JsonProperty>((IEnumerable<JsonProperty>) this.CreateProperties(contract.NonNullableUnderlyingType, contract.MemberSerialization));
      Func<string, string> func = (Func<string, string>) null;
      JsonObjectAttribute cachedAttribute = JsonTypeReflector.GetCachedAttribute<JsonObjectAttribute>((object) contract.NonNullableUnderlyingType);
      if (cachedAttribute != null)
      {
        contract.ItemRequired = cachedAttribute._itemRequired;
        contract.ItemNullValueHandling = cachedAttribute._itemNullValueHandling;
        contract.MissingMemberHandling = cachedAttribute._missingMemberHandling;
        if (cachedAttribute.NamingStrategyType != (Type) null)
        {
          NamingStrategy namingStrategy = JsonTypeReflector.GetContainerNamingStrategy((JsonContainerAttribute) cachedAttribute);
          func = (Func<string, string>) (s => namingStrategy.GetDictionaryKey(s));
        }
      }
      if (func == null)
        func = new Func<string, string>(this.ResolveExtensionDataName);
      contract.ExtensionDataNameResolver = func;
      if (contract.IsInstantiable)
      {
        ConstructorInfo attributeConstructor = this.GetAttributeConstructor(contract.NonNullableUnderlyingType);
        if (attributeConstructor != (ConstructorInfo) null)
        {
          contract.OverrideCreator = JsonTypeReflector.ReflectionDelegateFactory.CreateParameterizedConstructor((MethodBase) attributeConstructor);
          contract.CreatorParameters.AddRange<JsonProperty>((IEnumerable<JsonProperty>) this.CreateConstructorParameters(attributeConstructor, contract.Properties));
        }
        else if (contract.MemberSerialization == MemberSerialization.Fields)
        {
          if (JsonTypeReflector.FullyTrusted)
            contract.DefaultCreator = new Func<object>(contract.GetUninitializedObject);
        }
        else if (contract.DefaultCreator == null || contract.DefaultCreatorNonPublic)
        {
          ConstructorInfo parameterizedConstructor = this.GetParameterizedConstructor(contract.NonNullableUnderlyingType);
          if (parameterizedConstructor != (ConstructorInfo) null)
          {
            contract.ParameterizedCreator = JsonTypeReflector.ReflectionDelegateFactory.CreateParameterizedConstructor((MethodBase) parameterizedConstructor);
            contract.CreatorParameters.AddRange<JsonProperty>((IEnumerable<JsonProperty>) this.CreateConstructorParameters(parameterizedConstructor, contract.Properties));
          }
        }
        else if (contract.NonNullableUnderlyingType.IsValueType())
        {
          ConstructorInfo immutableConstructor = this.GetImmutableConstructor(contract.NonNullableUnderlyingType, contract.Properties);
          if (immutableConstructor != (ConstructorInfo) null)
          {
            contract.OverrideCreator = JsonTypeReflector.ReflectionDelegateFactory.CreateParameterizedConstructor((MethodBase) immutableConstructor);
            contract.CreatorParameters.AddRange<JsonProperty>((IEnumerable<JsonProperty>) this.CreateConstructorParameters(immutableConstructor, contract.Properties));
          }
        }
      }
      MemberInfo dataMemberForType = this.GetExtensionDataMemberForType(contract.NonNullableUnderlyingType);
      if (dataMemberForType != (MemberInfo) null)
        DefaultContractResolver.SetExtensionDataDelegates(contract, dataMemberForType);
      if (Array.IndexOf<string>(DefaultContractResolver.BlacklistedTypeNames, objectType.FullName) != -1)
        contract.OnSerializingCallbacks.Add(new SerializationCallback(DefaultContractResolver.ThrowUnableToSerializeError));
      return contract;
    }

    private static void ThrowUnableToSerializeError(object o, StreamingContext context) => throw new JsonSerializationException("Unable to serialize instance of '{0}'.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) o.GetType()));

    private MemberInfo GetExtensionDataMemberForType(Type type) => this.GetClassHierarchyForType(type).SelectMany<Type, MemberInfo>((Func<Type, IEnumerable<MemberInfo>>) (baseType =>
    {
      List<MemberInfo> initial = new List<MemberInfo>();
      initial.AddRange<MemberInfo>((IEnumerable<MemberInfo>) baseType.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic));
      initial.AddRange<MemberInfo>((IEnumerable<MemberInfo>) baseType.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic));
      return (IEnumerable<MemberInfo>) initial;
    })).LastOrDefault<MemberInfo>((Func<MemberInfo, bool>) (m =>
    {
      switch (m.MemberType())
      {
        case MemberTypes.Field:
        case MemberTypes.Property:
          if (!m.IsDefined(typeof (JsonExtensionDataAttribute), false))
            return false;
          if (!ReflectionUtils.CanReadMemberValue(m, true))
            throw new JsonException("Invalid extension data attribute on '{0}'. Member '{1}' must have a getter.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) DefaultContractResolver.GetClrTypeFullName(m.DeclaringType), (object) m.Name));
          Type implementingType;
          if (ReflectionUtils.ImplementsGenericDefinition(ReflectionUtils.GetMemberUnderlyingType(m), typeof (IDictionary<,>), out implementingType))
          {
            Type genericArgument1 = implementingType.GetGenericArguments()[0];
            Type genericArgument2 = implementingType.GetGenericArguments()[1];
            Type c = typeof (string);
            if (genericArgument1.IsAssignableFrom(c) && genericArgument2.IsAssignableFrom(typeof (JToken)))
              return true;
          }
          throw new JsonException("Invalid extension data attribute on '{0}'. Member '{1}' type must implement IDictionary<string, JToken>.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) DefaultContractResolver.GetClrTypeFullName(m.DeclaringType), (object) m.Name));
        default:
          return false;
      }
    }));

    private static void SetExtensionDataDelegates(JsonObjectContract contract, MemberInfo member)
    {
      JsonExtensionDataAttribute attribute = ReflectionUtils.GetAttribute<JsonExtensionDataAttribute>((object) member);
      if (attribute == null)
        return;
      Type memberUnderlyingType = ReflectionUtils.GetMemberUnderlyingType(member);
      Type implementingType;
      ReflectionUtils.ImplementsGenericDefinition(memberUnderlyingType, typeof (IDictionary<,>), out implementingType);
      Type genericArgument1 = implementingType.GetGenericArguments()[0];
      Type genericArgument2 = implementingType.GetGenericArguments()[1];
      Type type;
      if (ReflectionUtils.IsGenericDefinition(memberUnderlyingType, typeof (IDictionary<,>)))
        type = typeof (Dictionary<,>).MakeGenericType(genericArgument1, genericArgument2);
      else
        type = memberUnderlyingType;
      Func<object, object> getExtensionDataDictionary = JsonTypeReflector.ReflectionDelegateFactory.CreateGet<object>(member);
      if (attribute.ReadData)
      {
        Action<object, object> setExtensionDataDictionary = ReflectionUtils.CanSetMemberValue(member, true, false) ? JsonTypeReflector.ReflectionDelegateFactory.CreateSet<object>(member) : (Action<object, object>) null;
        Func<object> createExtensionDataDictionary = JsonTypeReflector.ReflectionDelegateFactory.CreateDefaultConstructor<object>(type);
        MethodInfo setMethod = memberUnderlyingType.GetProperty("Item", BindingFlags.Instance | BindingFlags.Public, (Binder) null, genericArgument2, new Type[1]
        {
          genericArgument1
        }, (ParameterModifier[]) null)?.GetSetMethod();
        if (setMethod == (MethodInfo) null)
          setMethod = implementingType.GetProperty("Item", BindingFlags.Instance | BindingFlags.Public, (Binder) null, genericArgument2, new Type[1]
          {
            genericArgument1
          }, (ParameterModifier[]) null)?.GetSetMethod();
        MethodCall<object, object> setExtensionDataDictionaryValue = JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>((MethodBase) setMethod);
        ExtensionDataSetter extensionDataSetter = (ExtensionDataSetter) ((o, key, value) =>
        {
          object target = getExtensionDataDictionary(o);
          if (target == null)
          {
            if (setExtensionDataDictionary == null)
              throw new JsonSerializationException("Cannot set value onto extension data member '{0}'. The extension data collection is null and it cannot be set.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) member.Name));
            target = createExtensionDataDictionary();
            setExtensionDataDictionary(o, target);
          }
          object obj = setExtensionDataDictionaryValue(target, new object[2]
          {
            (object) key,
            value
          });
        });
        contract.ExtensionDataSetter = extensionDataSetter;
      }
      if (attribute.WriteData)
      {
        ObjectConstructor<object> createEnumerableWrapper = JsonTypeReflector.ReflectionDelegateFactory.CreateParameterizedConstructor((MethodBase) ((IEnumerable<ConstructorInfo>) typeof (DefaultContractResolver.EnumerableDictionaryWrapper<,>).MakeGenericType(genericArgument1, genericArgument2).GetConstructors()).First<ConstructorInfo>());
        ExtensionDataGetter extensionDataGetter = (ExtensionDataGetter) (o =>
        {
          object obj = getExtensionDataDictionary(o);
          if (obj == null)
            return (IEnumerable<KeyValuePair<object, object>>) null;
          return (IEnumerable<KeyValuePair<object, object>>) createEnumerableWrapper(new object[1]
          {
            obj
          });
        });
        contract.ExtensionDataGetter = extensionDataGetter;
      }
      contract.ExtensionDataValueType = genericArgument2;
    }

    private ConstructorInfo? GetAttributeConstructor(Type objectType)
    {
      IEnumerator<ConstructorInfo> enumerator = ((IEnumerable<ConstructorInfo>) objectType.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)).Where<ConstructorInfo>((Func<ConstructorInfo, bool>) (c => c.IsDefined(typeof (JsonConstructorAttribute), true))).GetEnumerator();
      if (enumerator.MoveNext())
      {
        ConstructorInfo current = enumerator.Current;
        if (enumerator.MoveNext())
          throw new JsonException("Multiple constructors with the JsonConstructorAttribute.");
        return current;
      }
      if (!(objectType == typeof (Version)))
        return (ConstructorInfo) null;
      return objectType.GetConstructor(new Type[4]
      {
        typeof (int),
        typeof (int),
        typeof (int),
        typeof (int)
      });
    }

    private ConstructorInfo? GetImmutableConstructor(
      Type objectType,
      JsonPropertyCollection memberProperties)
    {
      IEnumerator<ConstructorInfo> enumerator = ((IEnumerable<ConstructorInfo>) objectType.GetConstructors()).GetEnumerator();
      if (enumerator.MoveNext())
      {
        ConstructorInfo current = enumerator.Current;
        if (!enumerator.MoveNext())
        {
          ParameterInfo[] parameters = current.GetParameters();
          if (parameters.Length != 0)
          {
            foreach (ParameterInfo parameterInfo in parameters)
            {
              JsonProperty jsonProperty = this.MatchProperty(memberProperties, parameterInfo.Name, parameterInfo.ParameterType);
              if (jsonProperty == null || jsonProperty.Writable)
                return (ConstructorInfo) null;
            }
            return current;
          }
        }
      }
      return (ConstructorInfo) null;
    }

    private ConstructorInfo? GetParameterizedConstructor(Type objectType)
    {
      ConstructorInfo[] constructors = objectType.GetConstructors(BindingFlags.Instance | BindingFlags.Public);
      return constructors.Length == 1 ? constructors[0] : (ConstructorInfo) null;
    }

    protected virtual IList<JsonProperty> CreateConstructorParameters(
      ConstructorInfo constructor,
      JsonPropertyCollection memberProperties)
    {
      ParameterInfo[] parameters = constructor.GetParameters();
      JsonPropertyCollection constructorParameters = new JsonPropertyCollection(constructor.DeclaringType);
      foreach (ParameterInfo parameterInfo in parameters)
      {
        if (parameterInfo.Name != null)
        {
          JsonProperty matchingMemberProperty = this.MatchProperty(memberProperties, parameterInfo.Name, parameterInfo.ParameterType);
          if (matchingMemberProperty != null || parameterInfo.Name != null)
          {
            JsonProperty constructorParameter = this.CreatePropertyFromConstructorParameter(matchingMemberProperty, parameterInfo);
            if (constructorParameter != null)
              constructorParameters.AddProperty(constructorParameter);
          }
        }
      }
      return (IList<JsonProperty>) constructorParameters;
    }

    private JsonProperty? MatchProperty(JsonPropertyCollection properties, string name, Type type)
    {
      if (name == null)
        return (JsonProperty) null;
      JsonProperty closestMatchProperty = properties.GetClosestMatchProperty(name);
      return closestMatchProperty == null || closestMatchProperty.PropertyType != type ? (JsonProperty) null : closestMatchProperty;
    }

    protected virtual JsonProperty CreatePropertyFromConstructorParameter(
      JsonProperty? matchingMemberProperty,
      ParameterInfo parameterInfo)
    {
      JsonProperty property = new JsonProperty();
      property.PropertyType = parameterInfo.ParameterType;
      property.AttributeProvider = (IAttributeProvider) new ReflectionAttributeProvider((object) parameterInfo);
      this.SetPropertySettingsFromAttributes(property, (object) parameterInfo, parameterInfo.Name, parameterInfo.Member.DeclaringType, MemberSerialization.OptOut, out bool _);
      property.Readable = false;
      property.Writable = true;
      if (matchingMemberProperty != null)
      {
        property.PropertyName = property.PropertyName != parameterInfo.Name ? property.PropertyName : matchingMemberProperty.PropertyName;
        property.Converter = property.Converter ?? matchingMemberProperty.Converter;
        if (!property._hasExplicitDefaultValue && matchingMemberProperty._hasExplicitDefaultValue)
          property.DefaultValue = matchingMemberProperty.DefaultValue;
        property._required = property._required ?? matchingMemberProperty._required;
        property.IsReference = property.IsReference ?? matchingMemberProperty.IsReference;
        property.NullValueHandling = property.NullValueHandling ?? matchingMemberProperty.NullValueHandling;
        property.DefaultValueHandling = property.DefaultValueHandling ?? matchingMemberProperty.DefaultValueHandling;
        property.ReferenceLoopHandling = property.ReferenceLoopHandling ?? matchingMemberProperty.ReferenceLoopHandling;
        property.ObjectCreationHandling = property.ObjectCreationHandling ?? matchingMemberProperty.ObjectCreationHandling;
        property.TypeNameHandling = property.TypeNameHandling ?? matchingMemberProperty.TypeNameHandling;
      }
      return property;
    }

    protected virtual JsonConverter? ResolveContractConverter(Type objectType) => JsonTypeReflector.GetJsonConverter((object) objectType);

    private Func<object> GetDefaultCreator(Type createdType) => JsonTypeReflector.ReflectionDelegateFactory.CreateDefaultConstructor<object>(createdType);

    private void InitializeContract(JsonContract contract)
    {
      JsonContainerAttribute cachedAttribute = JsonTypeReflector.GetCachedAttribute<JsonContainerAttribute>((object) contract.NonNullableUnderlyingType);
      if (cachedAttribute != null)
      {
        contract.IsReference = cachedAttribute._isReference;
      }
      else
      {
        DataContractAttribute contractAttribute = JsonTypeReflector.GetDataContractAttribute(contract.NonNullableUnderlyingType);
        if (contractAttribute != null && contractAttribute.IsReference)
          contract.IsReference = new bool?(true);
      }
      contract.Converter = this.ResolveContractConverter(contract.NonNullableUnderlyingType);
      contract.InternalConverter = JsonSerializer.GetMatchingConverter((IList<JsonConverter>) DefaultContractResolver.BuiltInConverters, contract.NonNullableUnderlyingType);
      if (contract.IsInstantiable && (ReflectionUtils.HasDefaultConstructor(contract.CreatedType, true) || contract.CreatedType.IsValueType()))
      {
        contract.DefaultCreator = this.GetDefaultCreator(contract.CreatedType);
        contract.DefaultCreatorNonPublic = !contract.CreatedType.IsValueType() && ReflectionUtils.GetDefaultConstructor(contract.CreatedType) == (ConstructorInfo) null;
      }
      this.ResolveCallbackMethods(contract, contract.NonNullableUnderlyingType);
    }

    private void ResolveCallbackMethods(JsonContract contract, Type t)
    {
      List<SerializationCallback> onSerializing;
      List<SerializationCallback> onSerialized;
      List<SerializationCallback> onDeserializing;
      List<SerializationCallback> onDeserialized;
      List<SerializationErrorCallback> onError;
      this.GetCallbackMethodsForType(t, out onSerializing, out onSerialized, out onDeserializing, out onDeserialized, out onError);
      if (onSerializing != null)
        contract.OnSerializingCallbacks.AddRange<SerializationCallback>((IEnumerable<SerializationCallback>) onSerializing);
      if (onSerialized != null)
        contract.OnSerializedCallbacks.AddRange<SerializationCallback>((IEnumerable<SerializationCallback>) onSerialized);
      if (onDeserializing != null)
        contract.OnDeserializingCallbacks.AddRange<SerializationCallback>((IEnumerable<SerializationCallback>) onDeserializing);
      if (onDeserialized != null)
        contract.OnDeserializedCallbacks.AddRange<SerializationCallback>((IEnumerable<SerializationCallback>) onDeserialized);
      if (onError == null)
        return;
      contract.OnErrorCallbacks.AddRange<SerializationErrorCallback>((IEnumerable<SerializationErrorCallback>) onError);
    }

    private void GetCallbackMethodsForType(
      Type type,
      out List<SerializationCallback>? onSerializing,
      out List<SerializationCallback>? onSerialized,
      out List<SerializationCallback>? onDeserializing,
      out List<SerializationCallback>? onDeserialized,
      out List<SerializationErrorCallback>? onError)
    {
      onSerializing = (List<SerializationCallback>) null;
      onSerialized = (List<SerializationCallback>) null;
      onDeserializing = (List<SerializationCallback>) null;
      onDeserialized = (List<SerializationCallback>) null;
      onError = (List<SerializationErrorCallback>) null;
      foreach (Type t in this.GetClassHierarchyForType(type))
      {
        MethodInfo currentCallback1 = (MethodInfo) null;
        MethodInfo currentCallback2 = (MethodInfo) null;
        MethodInfo currentCallback3 = (MethodInfo) null;
        MethodInfo currentCallback4 = (MethodInfo) null;
        MethodInfo currentCallback5 = (MethodInfo) null;
        bool flag1 = DefaultContractResolver.ShouldSkipSerializing(t);
        bool flag2 = DefaultContractResolver.ShouldSkipDeserialized(t);
        foreach (MethodInfo method in t.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
        {
          if (!method.ContainsGenericParameters)
          {
            Type prevAttributeType = (Type) null;
            ParameterInfo[] parameters = method.GetParameters();
            if (!flag1 && DefaultContractResolver.IsValidCallback(method, parameters, typeof (OnSerializingAttribute), currentCallback1, ref prevAttributeType))
            {
              onSerializing = onSerializing ?? new List<SerializationCallback>();
              onSerializing.Add(JsonContract.CreateSerializationCallback(method));
              currentCallback1 = method;
            }
            if (DefaultContractResolver.IsValidCallback(method, parameters, typeof (OnSerializedAttribute), currentCallback2, ref prevAttributeType))
            {
              onSerialized = onSerialized ?? new List<SerializationCallback>();
              onSerialized.Add(JsonContract.CreateSerializationCallback(method));
              currentCallback2 = method;
            }
            if (DefaultContractResolver.IsValidCallback(method, parameters, typeof (OnDeserializingAttribute), currentCallback3, ref prevAttributeType))
            {
              onDeserializing = onDeserializing ?? new List<SerializationCallback>();
              onDeserializing.Add(JsonContract.CreateSerializationCallback(method));
              currentCallback3 = method;
            }
            if (!flag2 && DefaultContractResolver.IsValidCallback(method, parameters, typeof (OnDeserializedAttribute), currentCallback4, ref prevAttributeType))
            {
              onDeserialized = onDeserialized ?? new List<SerializationCallback>();
              onDeserialized.Add(JsonContract.CreateSerializationCallback(method));
              currentCallback4 = method;
            }
            if (DefaultContractResolver.IsValidCallback(method, parameters, typeof (OnErrorAttribute), currentCallback5, ref prevAttributeType))
            {
              onError = onError ?? new List<SerializationErrorCallback>();
              onError.Add(JsonContract.CreateSerializationErrorCallback(method));
              currentCallback5 = method;
            }
          }
        }
      }
    }

    private static bool IsConcurrentOrObservableCollection(Type t)
    {
      if (t.IsGenericType())
      {
        switch (t.GetGenericTypeDefinition().FullName)
        {
          case "System.Collections.Concurrent.ConcurrentQueue`1":
          case "System.Collections.Concurrent.ConcurrentStack`1":
          case "System.Collections.Concurrent.ConcurrentBag`1":
          case "System.Collections.Concurrent.ConcurrentDictionary`2":
          case "System.Collections.ObjectModel.ObservableCollection`1":
            return true;
        }
      }
      return false;
    }

    private static bool ShouldSkipDeserialized(Type t) => DefaultContractResolver.IsConcurrentOrObservableCollection(t) || t.Name == "FSharpSet`1" || t.Name == "FSharpMap`2";

    private static bool ShouldSkipSerializing(Type t) => DefaultContractResolver.IsConcurrentOrObservableCollection(t) || t.Name == "FSharpSet`1" || t.Name == "FSharpMap`2";

    private List<Type> GetClassHierarchyForType(Type type)
    {
      List<Type> hierarchyForType = new List<Type>();
      for (Type type1 = type; type1 != (Type) null && type1 != typeof (object); type1 = type1.BaseType())
        hierarchyForType.Add(type1);
      hierarchyForType.Reverse();
      return hierarchyForType;
    }

    protected virtual JsonDictionaryContract CreateDictionaryContract(Type objectType)
    {
      JsonDictionaryContract contract = new JsonDictionaryContract(objectType);
      this.InitializeContract((JsonContract) contract);
      JsonContainerAttribute attribute = JsonTypeReflector.GetAttribute<JsonContainerAttribute>((object) objectType);
      if (attribute?.NamingStrategyType != (Type) null)
      {
        NamingStrategy namingStrategy = JsonTypeReflector.GetContainerNamingStrategy(attribute);
        contract.DictionaryKeyResolver = (Func<string, string>) (s => namingStrategy.GetDictionaryKey(s));
      }
      else
        contract.DictionaryKeyResolver = new Func<string, string>(this.ResolveDictionaryKey);
      ConstructorInfo attributeConstructor = this.GetAttributeConstructor(contract.NonNullableUnderlyingType);
      if (attributeConstructor != (ConstructorInfo) null)
      {
        ParameterInfo[] parameters = attributeConstructor.GetParameters();
        Type type1;
        if (!(contract.DictionaryKeyType != (Type) null) || !(contract.DictionaryValueType != (Type) null))
          type1 = typeof (IDictionary);
        else
          type1 = typeof (IEnumerable<>).MakeGenericType(typeof (KeyValuePair<,>).MakeGenericType(contract.DictionaryKeyType, contract.DictionaryValueType));
        Type type2 = type1;
        if (parameters.Length == 0)
        {
          contract.HasParameterizedCreator = false;
        }
        else
        {
          if (parameters.Length != 1 || !type2.IsAssignableFrom(parameters[0].ParameterType))
            throw new JsonException("Constructor for '{0}' must have no parameters or a single parameter that implements '{1}'.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) contract.UnderlyingType, (object) type2));
          contract.HasParameterizedCreator = true;
        }
        contract.OverrideCreator = JsonTypeReflector.ReflectionDelegateFactory.CreateParameterizedConstructor((MethodBase) attributeConstructor);
      }
      return contract;
    }

    protected virtual JsonArrayContract CreateArrayContract(Type objectType)
    {
      JsonArrayContract contract = new JsonArrayContract(objectType);
      this.InitializeContract((JsonContract) contract);
      ConstructorInfo attributeConstructor = this.GetAttributeConstructor(contract.NonNullableUnderlyingType);
      if (attributeConstructor != (ConstructorInfo) null)
      {
        ParameterInfo[] parameters = attributeConstructor.GetParameters();
        Type type1;
        if (!(contract.CollectionItemType != (Type) null))
          type1 = typeof (IEnumerable);
        else
          type1 = typeof (IEnumerable<>).MakeGenericType(contract.CollectionItemType);
        Type type2 = type1;
        if (parameters.Length == 0)
        {
          contract.HasParameterizedCreator = false;
        }
        else
        {
          if (parameters.Length != 1 || !type2.IsAssignableFrom(parameters[0].ParameterType))
            throw new JsonException("Constructor for '{0}' must have no parameters or a single parameter that implements '{1}'.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) contract.UnderlyingType, (object) type2));
          contract.HasParameterizedCreator = true;
        }
        contract.OverrideCreator = JsonTypeReflector.ReflectionDelegateFactory.CreateParameterizedConstructor((MethodBase) attributeConstructor);
      }
      return contract;
    }

    protected virtual JsonPrimitiveContract CreatePrimitiveContract(Type objectType)
    {
      JsonPrimitiveContract contract = new JsonPrimitiveContract(objectType);
      this.InitializeContract((JsonContract) contract);
      return contract;
    }

    protected virtual JsonLinqContract CreateLinqContract(Type objectType)
    {
      JsonLinqContract contract = new JsonLinqContract(objectType);
      this.InitializeContract((JsonContract) contract);
      return contract;
    }

    protected virtual JsonISerializableContract CreateISerializableContract(Type objectType)
    {
      JsonISerializableContract contract = new JsonISerializableContract(objectType);
      this.InitializeContract((JsonContract) contract);
      if (contract.IsInstantiable)
      {
        ConstructorInfo constructor = contract.NonNullableUnderlyingType.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, new Type[2]
        {
          typeof (SerializationInfo),
          typeof (StreamingContext)
        }, (ParameterModifier[]) null);
        if (constructor != (ConstructorInfo) null)
        {
          ObjectConstructor<object> parameterizedConstructor = JsonTypeReflector.ReflectionDelegateFactory.CreateParameterizedConstructor((MethodBase) constructor);
          contract.ISerializableCreator = parameterizedConstructor;
        }
      }
      return contract;
    }

    protected virtual JsonDynamicContract CreateDynamicContract(Type objectType)
    {
      JsonDynamicContract contract = new JsonDynamicContract(objectType);
      this.InitializeContract((JsonContract) contract);
      JsonContainerAttribute attribute = JsonTypeReflector.GetAttribute<JsonContainerAttribute>((object) objectType);
      if (attribute?.NamingStrategyType != (Type) null)
      {
        NamingStrategy namingStrategy = JsonTypeReflector.GetContainerNamingStrategy(attribute);
        contract.PropertyNameResolver = (Func<string, string>) (s => namingStrategy.GetDictionaryKey(s));
      }
      else
        contract.PropertyNameResolver = new Func<string, string>(this.ResolveDictionaryKey);
      contract.Properties.AddRange<JsonProperty>((IEnumerable<JsonProperty>) this.CreateProperties(objectType, MemberSerialization.OptOut));
      return contract;
    }

    protected virtual JsonStringContract CreateStringContract(Type objectType)
    {
      JsonStringContract contract = new JsonStringContract(objectType);
      this.InitializeContract((JsonContract) contract);
      return contract;
    }

    protected virtual JsonContract CreateContract(Type objectType)
    {
      Type t = ReflectionUtils.EnsureNotByRefType(objectType);
      if (DefaultContractResolver.IsJsonPrimitiveType(t))
        return (JsonContract) this.CreatePrimitiveContract(objectType);
      Type type = ReflectionUtils.EnsureNotNullableType(t);
      switch (JsonTypeReflector.GetCachedAttribute<JsonContainerAttribute>((object) type))
      {
        case JsonObjectAttribute _:
          return (JsonContract) this.CreateObjectContract(objectType);
        case JsonArrayAttribute _:
          return (JsonContract) this.CreateArrayContract(objectType);
        case JsonDictionaryAttribute _:
          return (JsonContract) this.CreateDictionaryContract(objectType);
        default:
          if (type == typeof (JToken) || type.IsSubclassOf(typeof (JToken)))
            return (JsonContract) this.CreateLinqContract(objectType);
          if (CollectionUtils.IsDictionaryType(type))
            return (JsonContract) this.CreateDictionaryContract(objectType);
          if (typeof (IEnumerable).IsAssignableFrom(type))
            return (JsonContract) this.CreateArrayContract(objectType);
          if (DefaultContractResolver.CanConvertToString(type))
            return (JsonContract) this.CreateStringContract(objectType);
          if (!this.IgnoreSerializableInterface && typeof (ISerializable).IsAssignableFrom(type) && JsonTypeReflector.IsSerializable((object) type))
            return (JsonContract) this.CreateISerializableContract(objectType);
          if (typeof (IDynamicMetaObjectProvider).IsAssignableFrom(type))
            return (JsonContract) this.CreateDynamicContract(objectType);
          return DefaultContractResolver.IsIConvertible(type) ? (JsonContract) this.CreatePrimitiveContract(type) : (JsonContract) this.CreateObjectContract(objectType);
      }
    }

    internal static bool IsJsonPrimitiveType(Type t)
    {
      PrimitiveTypeCode typeCode = ConvertUtils.GetTypeCode(t);
      return typeCode != PrimitiveTypeCode.Empty && typeCode != PrimitiveTypeCode.Object;
    }

    internal static bool IsIConvertible(Type t) => (typeof (IConvertible).IsAssignableFrom(t) || ReflectionUtils.IsNullableType(t) && typeof (IConvertible).IsAssignableFrom(Nullable.GetUnderlyingType(t))) && !typeof (JToken).IsAssignableFrom(t);

    internal static bool CanConvertToString(Type type) => JsonTypeReflector.CanTypeDescriptorConvertString(type, out TypeConverter _) || type == typeof (Type) || type.IsSubclassOf(typeof (Type));

    private static bool IsValidCallback(
      MethodInfo method,
      ParameterInfo[] parameters,
      Type attributeType,
      MethodInfo? currentCallback,
      ref Type? prevAttributeType)
    {
      if (!method.IsDefined(attributeType, false))
        return false;
      if (currentCallback != (MethodInfo) null)
        throw new JsonException("Invalid attribute. Both '{0}' and '{1}' in type '{2}' have '{3}'.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) method, (object) currentCallback, (object) DefaultContractResolver.GetClrTypeFullName(method.DeclaringType), (object) attributeType));
      if (prevAttributeType != (Type) null)
        throw new JsonException("Invalid Callback. Method '{3}' in type '{2}' has both '{0}' and '{1}'.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) prevAttributeType, (object) attributeType, (object) DefaultContractResolver.GetClrTypeFullName(method.DeclaringType), (object) method));
      if (method.IsVirtual)
        throw new JsonException("Virtual Method '{0}' of type '{1}' cannot be marked with '{2}' attribute.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) method, (object) DefaultContractResolver.GetClrTypeFullName(method.DeclaringType), (object) attributeType));
      if (method.ReturnType != typeof (void))
        throw new JsonException("Serialization Callback '{1}' in type '{0}' must return void.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) DefaultContractResolver.GetClrTypeFullName(method.DeclaringType), (object) method));
      if (attributeType == typeof (OnErrorAttribute))
      {
        if (parameters == null || parameters.Length != 2 || parameters[0].ParameterType != typeof (StreamingContext) || parameters[1].ParameterType != typeof (ErrorContext))
          throw new JsonException("Serialization Error Callback '{1}' in type '{0}' must have two parameters of type '{2}' and '{3}'.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) DefaultContractResolver.GetClrTypeFullName(method.DeclaringType), (object) method, (object) typeof (StreamingContext), (object) typeof (ErrorContext)));
      }
      else if (parameters == null || parameters.Length != 1 || parameters[0].ParameterType != typeof (StreamingContext))
        throw new JsonException("Serialization Callback '{1}' in type '{0}' must have a single parameter of type '{2}'.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) DefaultContractResolver.GetClrTypeFullName(method.DeclaringType), (object) method, (object) typeof (StreamingContext)));
      prevAttributeType = attributeType;
      return true;
    }

    internal static string GetClrTypeFullName(Type type) => type.IsGenericTypeDefinition() || !type.ContainsGenericParameters() ? type.FullName : "{0}.{1}".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) type.Namespace, (object) type.Name);

    protected virtual IList<JsonProperty> CreateProperties(
      Type type,
      MemberSerialization memberSerialization)
    {
      List<MemberInfo> serializableMembers = this.GetSerializableMembers(type);
      if (serializableMembers == null)
        throw new JsonSerializationException("Null collection of serializable members returned.");
      DefaultJsonNameTable nameTable = this.GetNameTable();
      JsonPropertyCollection source = new JsonPropertyCollection(type);
      foreach (MemberInfo member in serializableMembers)
      {
        JsonProperty property = this.CreateProperty(member, memberSerialization);
        if (property != null)
        {
          lock (nameTable)
            property.PropertyName = nameTable.Add(property.PropertyName);
          source.AddProperty(property);
        }
      }
      return (IList<JsonProperty>) source.OrderBy<JsonProperty, int>((Func<JsonProperty, int>) (p => p.Order ?? -1)).ToList<JsonProperty>();
    }

    internal virtual DefaultJsonNameTable GetNameTable() => this._nameTable;

    protected virtual IValueProvider CreateMemberValueProvider(MemberInfo member) => !this.DynamicCodeGeneration ? (IValueProvider) new ReflectionValueProvider(member) : (IValueProvider) new DynamicValueProvider(member);

    protected virtual JsonProperty CreateProperty(
      MemberInfo member,
      MemberSerialization memberSerialization)
    {
      JsonProperty property = new JsonProperty();
      property.PropertyType = ReflectionUtils.GetMemberUnderlyingType(member);
      property.DeclaringType = member.DeclaringType;
      property.ValueProvider = this.CreateMemberValueProvider(member);
      property.AttributeProvider = (IAttributeProvider) new ReflectionAttributeProvider((object) member);
      bool allowNonPublicAccess;
      this.SetPropertySettingsFromAttributes(property, (object) member, member.Name, member.DeclaringType, memberSerialization, out allowNonPublicAccess);
      if (memberSerialization != MemberSerialization.Fields)
      {
        property.Readable = ReflectionUtils.CanReadMemberValue(member, allowNonPublicAccess);
        property.Writable = ReflectionUtils.CanSetMemberValue(member, allowNonPublicAccess, property.HasMemberAttribute);
      }
      else
      {
        property.Readable = true;
        property.Writable = true;
      }
      if (!this.IgnoreShouldSerializeMembers)
        property.ShouldSerialize = this.CreateShouldSerializeTest(member);
      if (!this.IgnoreIsSpecifiedMembers)
        this.SetIsSpecifiedActions(property, member, allowNonPublicAccess);
      return property;
    }

    private void SetPropertySettingsFromAttributes(
      JsonProperty property,
      object attributeProvider,
      string name,
      Type declaringType,
      MemberSerialization memberSerialization,
      out bool allowNonPublicAccess)
    {
      DataContractAttribute contractAttribute = JsonTypeReflector.GetDataContractAttribute(declaringType);
      MemberInfo memberInfo = attributeProvider as MemberInfo;
      DataMemberAttribute dataMemberAttribute = contractAttribute == null || !(memberInfo != (MemberInfo) null) ? (DataMemberAttribute) null : JsonTypeReflector.GetDataMemberAttribute(memberInfo);
      JsonPropertyAttribute attribute1 = JsonTypeReflector.GetAttribute<JsonPropertyAttribute>(attributeProvider);
      JsonRequiredAttribute attribute2 = JsonTypeReflector.GetAttribute<JsonRequiredAttribute>(attributeProvider);
      string str;
      bool hasSpecifiedName;
      if (attribute1 != null && attribute1.PropertyName != null)
      {
        str = attribute1.PropertyName;
        hasSpecifiedName = true;
      }
      else if (dataMemberAttribute != null && dataMemberAttribute.Name != null)
      {
        str = dataMemberAttribute.Name;
        hasSpecifiedName = true;
      }
      else
      {
        str = name;
        hasSpecifiedName = false;
      }
      JsonContainerAttribute attribute3 = JsonTypeReflector.GetAttribute<JsonContainerAttribute>((object) declaringType);
      NamingStrategy namingStrategy = !(attribute1?.NamingStrategyType != (Type) null) ? (!(attribute3?.NamingStrategyType != (Type) null) ? this.NamingStrategy : JsonTypeReflector.GetContainerNamingStrategy(attribute3)) : JsonTypeReflector.CreateNamingStrategyInstance(attribute1.NamingStrategyType, attribute1.NamingStrategyParameters);
      property.PropertyName = namingStrategy == null ? this.ResolvePropertyName(str) : namingStrategy.GetPropertyName(str, hasSpecifiedName);
      property.UnderlyingName = name;
      bool flag1 = false;
      if (attribute1 != null)
      {
        property._required = attribute1._required;
        property.Order = attribute1._order;
        property.DefaultValueHandling = attribute1._defaultValueHandling;
        flag1 = true;
        property.NullValueHandling = attribute1._nullValueHandling;
        property.ReferenceLoopHandling = attribute1._referenceLoopHandling;
        property.ObjectCreationHandling = attribute1._objectCreationHandling;
        property.TypeNameHandling = attribute1._typeNameHandling;
        property.IsReference = attribute1._isReference;
        property.ItemIsReference = attribute1._itemIsReference;
        property.ItemConverter = attribute1.ItemConverterType != (Type) null ? JsonTypeReflector.CreateJsonConverterInstance(attribute1.ItemConverterType, attribute1.ItemConverterParameters) : (JsonConverter) null;
        property.ItemReferenceLoopHandling = attribute1._itemReferenceLoopHandling;
        property.ItemTypeNameHandling = attribute1._itemTypeNameHandling;
      }
      else
      {
        property.NullValueHandling = new NullValueHandling?();
        property.ReferenceLoopHandling = new ReferenceLoopHandling?();
        property.ObjectCreationHandling = new ObjectCreationHandling?();
        property.TypeNameHandling = new TypeNameHandling?();
        property.IsReference = new bool?();
        property.ItemIsReference = new bool?();
        property.ItemConverter = (JsonConverter) null;
        property.ItemReferenceLoopHandling = new ReferenceLoopHandling?();
        property.ItemTypeNameHandling = new TypeNameHandling?();
        if (dataMemberAttribute != null)
        {
          property._required = new Required?(dataMemberAttribute.IsRequired ? Required.AllowNull : Required.Default);
          property.Order = dataMemberAttribute.Order != -1 ? new int?(dataMemberAttribute.Order) : new int?();
          property.DefaultValueHandling = !dataMemberAttribute.EmitDefaultValue ? new DefaultValueHandling?(DefaultValueHandling.Ignore) : new DefaultValueHandling?();
          flag1 = true;
        }
      }
      if (attribute2 != null)
      {
        property._required = new Required?(Required.Always);
        flag1 = true;
      }
      property.HasMemberAttribute = flag1;
      bool flag2 = JsonTypeReflector.GetAttribute<JsonIgnoreAttribute>(attributeProvider) != null || JsonTypeReflector.GetAttribute<JsonExtensionDataAttribute>(attributeProvider) != null || JsonTypeReflector.IsNonSerializable(attributeProvider);
      if (memberSerialization != MemberSerialization.OptIn)
      {
        bool flag3 = JsonTypeReflector.GetAttribute<IgnoreDataMemberAttribute>(attributeProvider) != null;
        property.Ignored = flag2 | flag3;
      }
      else
        property.Ignored = flag2 || !flag1;
      property.Converter = JsonTypeReflector.GetJsonConverter(attributeProvider);
      DefaultValueAttribute attribute4 = JsonTypeReflector.GetAttribute<DefaultValueAttribute>(attributeProvider);
      if (attribute4 != null)
        property.DefaultValue = attribute4.Value;
      allowNonPublicAccess = false;
      if ((this.DefaultMembersSearchFlags & BindingFlags.NonPublic) == BindingFlags.NonPublic)
        allowNonPublicAccess = true;
      if (flag1)
        allowNonPublicAccess = true;
      if (memberSerialization != MemberSerialization.Fields)
        return;
      allowNonPublicAccess = true;
    }

    private Predicate<object>? CreateShouldSerializeTest(MemberInfo member)
    {
      MethodInfo method = member.DeclaringType.GetMethod("ShouldSerialize" + member.Name, ReflectionUtils.EmptyTypes);
      if (method == (MethodInfo) null || method.ReturnType != typeof (bool))
        return (Predicate<object>) null;
      MethodCall<object, object> shouldSerializeCall = JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>((MethodBase) method);
      return (Predicate<object>) (o => (bool) shouldSerializeCall(o, new object[0]));
    }

    private void SetIsSpecifiedActions(
      JsonProperty property,
      MemberInfo member,
      bool allowNonPublicAccess)
    {
      MemberInfo memberInfo = (MemberInfo) member.DeclaringType.GetProperty(member.Name + "Specified", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
      if (memberInfo == (MemberInfo) null)
        memberInfo = (MemberInfo) member.DeclaringType.GetField(member.Name + "Specified", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
      if (memberInfo == (MemberInfo) null || ReflectionUtils.GetMemberUnderlyingType(memberInfo) != typeof (bool))
        return;
      Func<object, object> specifiedPropertyGet = JsonTypeReflector.ReflectionDelegateFactory.CreateGet<object>(memberInfo);
      property.GetIsSpecified = (Predicate<object>) (o => (bool) specifiedPropertyGet(o));
      if (!ReflectionUtils.CanSetMemberValue(memberInfo, allowNonPublicAccess, false))
        return;
      property.SetIsSpecified = JsonTypeReflector.ReflectionDelegateFactory.CreateSet<object>(memberInfo);
    }

    protected virtual string ResolvePropertyName(string propertyName) => this.NamingStrategy != null ? this.NamingStrategy.GetPropertyName(propertyName, false) : propertyName;

    protected virtual string ResolveExtensionDataName(string extensionDataName) => this.NamingStrategy != null ? this.NamingStrategy.GetExtensionDataName(extensionDataName) : extensionDataName;

    protected virtual string ResolveDictionaryKey(string dictionaryKey) => this.NamingStrategy != null ? this.NamingStrategy.GetDictionaryKey(dictionaryKey) : this.ResolvePropertyName(dictionaryKey);

    public string GetResolvedPropertyName(string propertyName) => this.ResolvePropertyName(propertyName);

    internal class EnumerableDictionaryWrapper<TEnumeratorKey, TEnumeratorValue> : 
      IEnumerable<KeyValuePair<object, object>>,
      IEnumerable
    {
      private readonly IEnumerable<KeyValuePair<TEnumeratorKey, TEnumeratorValue>> _e;

      public EnumerableDictionaryWrapper(
        IEnumerable<KeyValuePair<TEnumeratorKey, TEnumeratorValue>> e)
      {
        ValidationUtils.ArgumentNotNull((object) e, nameof (e));
        this._e = e;
      }

      public IEnumerator<KeyValuePair<object, object>> GetEnumerator()
      {
        foreach (KeyValuePair<TEnumeratorKey, TEnumeratorValue> keyValuePair in this._e)
          yield return new KeyValuePair<object, object>((object) keyValuePair.Key, (object) keyValuePair.Value);
      }

      IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();
    }
  }
}
