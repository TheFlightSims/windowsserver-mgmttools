// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Utilities.ImmutableCollectionsUtils
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\Newtonsoft.Json.dll

using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;


#nullable enable
namespace Newtonsoft.Json.Utilities
{
  internal static class ImmutableCollectionsUtils
  {
    private const string ImmutableListGenericInterfaceTypeName = "System.Collections.Immutable.IImmutableList`1";
    private const string ImmutableQueueGenericInterfaceTypeName = "System.Collections.Immutable.IImmutableQueue`1";
    private const string ImmutableStackGenericInterfaceTypeName = "System.Collections.Immutable.IImmutableStack`1";
    private const string ImmutableSetGenericInterfaceTypeName = "System.Collections.Immutable.IImmutableSet`1";
    private const string ImmutableArrayTypeName = "System.Collections.Immutable.ImmutableArray";
    private const string ImmutableArrayGenericTypeName = "System.Collections.Immutable.ImmutableArray`1";
    private const string ImmutableListTypeName = "System.Collections.Immutable.ImmutableList";
    private const string ImmutableListGenericTypeName = "System.Collections.Immutable.ImmutableList`1";
    private const string ImmutableQueueTypeName = "System.Collections.Immutable.ImmutableQueue";
    private const string ImmutableQueueGenericTypeName = "System.Collections.Immutable.ImmutableQueue`1";
    private const string ImmutableStackTypeName = "System.Collections.Immutable.ImmutableStack";
    private const string ImmutableStackGenericTypeName = "System.Collections.Immutable.ImmutableStack`1";
    private const string ImmutableSortedSetTypeName = "System.Collections.Immutable.ImmutableSortedSet";
    private const string ImmutableSortedSetGenericTypeName = "System.Collections.Immutable.ImmutableSortedSet`1";
    private const string ImmutableHashSetTypeName = "System.Collections.Immutable.ImmutableHashSet";
    private const string ImmutableHashSetGenericTypeName = "System.Collections.Immutable.ImmutableHashSet`1";
    private static readonly IList<ImmutableCollectionsUtils.ImmutableCollectionTypeInfo> ArrayContractImmutableCollectionDefinitions = (IList<ImmutableCollectionsUtils.ImmutableCollectionTypeInfo>) new List<ImmutableCollectionsUtils.ImmutableCollectionTypeInfo>()
    {
      new ImmutableCollectionsUtils.ImmutableCollectionTypeInfo("System.Collections.Immutable.IImmutableList`1", "System.Collections.Immutable.ImmutableList`1", "System.Collections.Immutable.ImmutableList"),
      new ImmutableCollectionsUtils.ImmutableCollectionTypeInfo("System.Collections.Immutable.ImmutableList`1", "System.Collections.Immutable.ImmutableList`1", "System.Collections.Immutable.ImmutableList"),
      new ImmutableCollectionsUtils.ImmutableCollectionTypeInfo("System.Collections.Immutable.IImmutableQueue`1", "System.Collections.Immutable.ImmutableQueue`1", "System.Collections.Immutable.ImmutableQueue"),
      new ImmutableCollectionsUtils.ImmutableCollectionTypeInfo("System.Collections.Immutable.ImmutableQueue`1", "System.Collections.Immutable.ImmutableQueue`1", "System.Collections.Immutable.ImmutableQueue"),
      new ImmutableCollectionsUtils.ImmutableCollectionTypeInfo("System.Collections.Immutable.IImmutableStack`1", "System.Collections.Immutable.ImmutableStack`1", "System.Collections.Immutable.ImmutableStack"),
      new ImmutableCollectionsUtils.ImmutableCollectionTypeInfo("System.Collections.Immutable.ImmutableStack`1", "System.Collections.Immutable.ImmutableStack`1", "System.Collections.Immutable.ImmutableStack"),
      new ImmutableCollectionsUtils.ImmutableCollectionTypeInfo("System.Collections.Immutable.IImmutableSet`1", "System.Collections.Immutable.ImmutableHashSet`1", "System.Collections.Immutable.ImmutableHashSet"),
      new ImmutableCollectionsUtils.ImmutableCollectionTypeInfo("System.Collections.Immutable.ImmutableSortedSet`1", "System.Collections.Immutable.ImmutableSortedSet`1", "System.Collections.Immutable.ImmutableSortedSet"),
      new ImmutableCollectionsUtils.ImmutableCollectionTypeInfo("System.Collections.Immutable.ImmutableHashSet`1", "System.Collections.Immutable.ImmutableHashSet`1", "System.Collections.Immutable.ImmutableHashSet"),
      new ImmutableCollectionsUtils.ImmutableCollectionTypeInfo("System.Collections.Immutable.ImmutableArray`1", "System.Collections.Immutable.ImmutableArray`1", "System.Collections.Immutable.ImmutableArray")
    };
    private const string ImmutableDictionaryGenericInterfaceTypeName = "System.Collections.Immutable.IImmutableDictionary`2";
    private const string ImmutableDictionaryTypeName = "System.Collections.Immutable.ImmutableDictionary";
    private const string ImmutableDictionaryGenericTypeName = "System.Collections.Immutable.ImmutableDictionary`2";
    private const string ImmutableSortedDictionaryTypeName = "System.Collections.Immutable.ImmutableSortedDictionary";
    private const string ImmutableSortedDictionaryGenericTypeName = "System.Collections.Immutable.ImmutableSortedDictionary`2";
    private static readonly IList<ImmutableCollectionsUtils.ImmutableCollectionTypeInfo> DictionaryContractImmutableCollectionDefinitions = (IList<ImmutableCollectionsUtils.ImmutableCollectionTypeInfo>) new List<ImmutableCollectionsUtils.ImmutableCollectionTypeInfo>()
    {
      new ImmutableCollectionsUtils.ImmutableCollectionTypeInfo("System.Collections.Immutable.IImmutableDictionary`2", "System.Collections.Immutable.ImmutableDictionary`2", "System.Collections.Immutable.ImmutableDictionary"),
      new ImmutableCollectionsUtils.ImmutableCollectionTypeInfo("System.Collections.Immutable.ImmutableSortedDictionary`2", "System.Collections.Immutable.ImmutableSortedDictionary`2", "System.Collections.Immutable.ImmutableSortedDictionary"),
      new ImmutableCollectionsUtils.ImmutableCollectionTypeInfo("System.Collections.Immutable.ImmutableDictionary`2", "System.Collections.Immutable.ImmutableDictionary`2", "System.Collections.Immutable.ImmutableDictionary")
    };

    internal static bool TryBuildImmutableForArrayContract(
      Type underlyingType,
      Type collectionItemType,
      [NotNullWhen(true)] out Type? createdType,
      [NotNullWhen(true)] out ObjectConstructor<object>? parameterizedCreator)
    {
      if (underlyingType.IsGenericType())
      {
        Type genericTypeDefinition = underlyingType.GetGenericTypeDefinition();
        string name = genericTypeDefinition.FullName;
        ImmutableCollectionsUtils.ImmutableCollectionTypeInfo collectionTypeInfo = ImmutableCollectionsUtils.ArrayContractImmutableCollectionDefinitions.FirstOrDefault<ImmutableCollectionsUtils.ImmutableCollectionTypeInfo>((Func<ImmutableCollectionsUtils.ImmutableCollectionTypeInfo, bool>) (d => d.ContractTypeName == name));
        if (collectionTypeInfo != null)
        {
          Type type1 = genericTypeDefinition.Assembly().GetType(collectionTypeInfo.CreatedTypeName);
          Type type2 = genericTypeDefinition.Assembly().GetType(collectionTypeInfo.BuilderTypeName);
          if (type1 != (Type) null && type2 != (Type) null)
          {
            MethodInfo methodInfo = ((IEnumerable<MethodInfo>) type2.GetMethods()).FirstOrDefault<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "CreateRange" && m.GetParameters().Length == 1));
            if (methodInfo != (MethodInfo) null)
            {
              createdType = type1.MakeGenericType(collectionItemType);
              MethodInfo method = methodInfo.MakeGenericMethod(collectionItemType);
              parameterizedCreator = JsonTypeReflector.ReflectionDelegateFactory.CreateParameterizedConstructor((MethodBase) method);
              return true;
            }
          }
        }
      }
      createdType = (Type) null;
      parameterizedCreator = (ObjectConstructor<object>) null;
      return false;
    }

    internal static bool TryBuildImmutableForDictionaryContract(
      Type underlyingType,
      Type keyItemType,
      Type valueItemType,
      [NotNullWhen(true)] out Type? createdType,
      [NotNullWhen(true)] out ObjectConstructor<object>? parameterizedCreator)
    {
      if (underlyingType.IsGenericType())
      {
        Type genericTypeDefinition = underlyingType.GetGenericTypeDefinition();
        string name = genericTypeDefinition.FullName;
        ImmutableCollectionsUtils.ImmutableCollectionTypeInfo collectionTypeInfo = ImmutableCollectionsUtils.DictionaryContractImmutableCollectionDefinitions.FirstOrDefault<ImmutableCollectionsUtils.ImmutableCollectionTypeInfo>((Func<ImmutableCollectionsUtils.ImmutableCollectionTypeInfo, bool>) (d => d.ContractTypeName == name));
        if (collectionTypeInfo != null)
        {
          Type type1 = genericTypeDefinition.Assembly().GetType(collectionTypeInfo.CreatedTypeName);
          Type type2 = genericTypeDefinition.Assembly().GetType(collectionTypeInfo.BuilderTypeName);
          if (type1 != (Type) null && type2 != (Type) null)
          {
            MethodInfo methodInfo = ((IEnumerable<MethodInfo>) type2.GetMethods()).FirstOrDefault<MethodInfo>((Func<MethodInfo, bool>) (m =>
            {
              ParameterInfo[] parameters = m.GetParameters();
              return m.Name == "CreateRange" && parameters.Length == 1 && parameters[0].ParameterType.IsGenericType() && parameters[0].ParameterType.GetGenericTypeDefinition() == typeof (IEnumerable<>);
            }));
            if (methodInfo != (MethodInfo) null)
            {
              createdType = type1.MakeGenericType(keyItemType, valueItemType);
              MethodInfo method = methodInfo.MakeGenericMethod(keyItemType, valueItemType);
              parameterizedCreator = JsonTypeReflector.ReflectionDelegateFactory.CreateParameterizedConstructor((MethodBase) method);
              return true;
            }
          }
        }
      }
      createdType = (Type) null;
      parameterizedCreator = (ObjectConstructor<object>) null;
      return false;
    }

    internal class ImmutableCollectionTypeInfo
    {
      public ImmutableCollectionTypeInfo(
        string contractTypeName,
        string createdTypeName,
        string builderTypeName)
      {
        this.ContractTypeName = contractTypeName;
        this.CreatedTypeName = createdTypeName;
        this.BuilderTypeName = builderTypeName;
      }

      public string ContractTypeName { get; set; }

      public string CreatedTypeName { get; set; }

      public string BuilderTypeName { get; set; }
    }
  }
}
