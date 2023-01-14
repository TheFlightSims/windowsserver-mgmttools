// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.JsonArrayContract
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;


#nullable enable
namespace Newtonsoft.Json.Serialization
{
  public class JsonArrayContract : JsonContainerContract
  {
    private readonly Type? _genericCollectionDefinitionType;
    private Type? _genericWrapperType;
    private ObjectConstructor<object>? _genericWrapperCreator;
    private Func<object>? _genericTemporaryCollectionCreator;
    private readonly ConstructorInfo? _parameterizedConstructor;
    private ObjectConstructor<object>? _parameterizedCreator;
    private ObjectConstructor<object>? _overrideCreator;

    public Type? CollectionItemType { get; }

    public bool IsMultidimensionalArray { get; }

    internal bool IsArray { get; }

    internal bool ShouldCreateWrapper { get; }

    internal bool CanDeserialize { get; private set; }

    internal ObjectConstructor<object>? ParameterizedCreator
    {
      get
      {
        if (this._parameterizedCreator == null && this._parameterizedConstructor != (ConstructorInfo) null)
          this._parameterizedCreator = JsonTypeReflector.ReflectionDelegateFactory.CreateParameterizedConstructor((MethodBase) this._parameterizedConstructor);
        return this._parameterizedCreator;
      }
    }

    public ObjectConstructor<object>? OverrideCreator
    {
      get => this._overrideCreator;
      set
      {
        this._overrideCreator = value;
        this.CanDeserialize = true;
      }
    }

    public bool HasParameterizedCreator { get; set; }

    internal bool HasParameterizedCreatorInternal => this.HasParameterizedCreator || this._parameterizedCreator != null || this._parameterizedConstructor != (ConstructorInfo) null;

    public JsonArrayContract(Type underlyingType)
      : base(underlyingType)
    {
      this.ContractType = JsonContractType.Array;
      this.IsArray = this.CreatedType.IsArray || this.NonNullableUnderlyingType.IsGenericType() && this.NonNullableUnderlyingType.GetGenericTypeDefinition().FullName == "System.Linq.EmptyPartition`1";
      bool flag;
      if (this.IsArray)
      {
        this.CollectionItemType = ReflectionUtils.GetCollectionItemType(this.UnderlyingType);
        this.IsReadOnlyOrFixedSize = true;
        this._genericCollectionDefinitionType = typeof (List<>).MakeGenericType(this.CollectionItemType);
        flag = true;
        this.IsMultidimensionalArray = this.CreatedType.IsArray && this.UnderlyingType.GetArrayRank() > 1;
      }
      else if (typeof (IList).IsAssignableFrom(this.NonNullableUnderlyingType))
      {
        this.CollectionItemType = !ReflectionUtils.ImplementsGenericDefinition(this.NonNullableUnderlyingType, typeof (ICollection<>), out this._genericCollectionDefinitionType) ? ReflectionUtils.GetCollectionItemType(this.NonNullableUnderlyingType) : this._genericCollectionDefinitionType.GetGenericArguments()[0];
        if (this.NonNullableUnderlyingType == typeof (IList))
          this.CreatedType = typeof (List<object>);
        if (this.CollectionItemType != (Type) null)
          this._parameterizedConstructor = CollectionUtils.ResolveEnumerableCollectionConstructor(this.NonNullableUnderlyingType, this.CollectionItemType);
        this.IsReadOnlyOrFixedSize = ReflectionUtils.InheritsGenericDefinition(this.NonNullableUnderlyingType, typeof (ReadOnlyCollection<>));
        flag = true;
      }
      else if (ReflectionUtils.ImplementsGenericDefinition(this.NonNullableUnderlyingType, typeof (ICollection<>), out this._genericCollectionDefinitionType))
      {
        this.CollectionItemType = this._genericCollectionDefinitionType.GetGenericArguments()[0];
        if (ReflectionUtils.IsGenericDefinition(this.NonNullableUnderlyingType, typeof (ICollection<>)) || ReflectionUtils.IsGenericDefinition(this.NonNullableUnderlyingType, typeof (IList<>)))
          this.CreatedType = typeof (List<>).MakeGenericType(this.CollectionItemType);
        if (ReflectionUtils.IsGenericDefinition(this.NonNullableUnderlyingType, typeof (ISet<>)))
          this.CreatedType = typeof (HashSet<>).MakeGenericType(this.CollectionItemType);
        this._parameterizedConstructor = CollectionUtils.ResolveEnumerableCollectionConstructor(this.NonNullableUnderlyingType, this.CollectionItemType);
        flag = true;
        this.ShouldCreateWrapper = true;
      }
      else
      {
        Type implementingType;
        if (ReflectionUtils.ImplementsGenericDefinition(this.NonNullableUnderlyingType, typeof (IReadOnlyCollection<>), out implementingType))
        {
          this.CollectionItemType = implementingType.GetGenericArguments()[0];
          if (ReflectionUtils.IsGenericDefinition(this.NonNullableUnderlyingType, typeof (IReadOnlyCollection<>)) || ReflectionUtils.IsGenericDefinition(this.NonNullableUnderlyingType, typeof (IReadOnlyList<>)))
            this.CreatedType = typeof (ReadOnlyCollection<>).MakeGenericType(this.CollectionItemType);
          this._genericCollectionDefinitionType = typeof (List<>).MakeGenericType(this.CollectionItemType);
          this._parameterizedConstructor = CollectionUtils.ResolveEnumerableCollectionConstructor(this.CreatedType, this.CollectionItemType);
          this.StoreFSharpListCreatorIfNecessary(this.NonNullableUnderlyingType);
          this.IsReadOnlyOrFixedSize = true;
          flag = this.HasParameterizedCreatorInternal;
        }
        else if (ReflectionUtils.ImplementsGenericDefinition(this.NonNullableUnderlyingType, typeof (IEnumerable<>), out implementingType))
        {
          this.CollectionItemType = implementingType.GetGenericArguments()[0];
          if (ReflectionUtils.IsGenericDefinition(this.UnderlyingType, typeof (IEnumerable<>)))
            this.CreatedType = typeof (List<>).MakeGenericType(this.CollectionItemType);
          this._parameterizedConstructor = CollectionUtils.ResolveEnumerableCollectionConstructor(this.NonNullableUnderlyingType, this.CollectionItemType);
          this.StoreFSharpListCreatorIfNecessary(this.NonNullableUnderlyingType);
          if (this.NonNullableUnderlyingType.IsGenericType() && this.NonNullableUnderlyingType.GetGenericTypeDefinition() == typeof (IEnumerable<>))
          {
            this._genericCollectionDefinitionType = implementingType;
            this.IsReadOnlyOrFixedSize = false;
            this.ShouldCreateWrapper = false;
            flag = true;
          }
          else
          {
            this._genericCollectionDefinitionType = typeof (List<>).MakeGenericType(this.CollectionItemType);
            this.IsReadOnlyOrFixedSize = true;
            this.ShouldCreateWrapper = true;
            flag = this.HasParameterizedCreatorInternal;
          }
        }
        else
        {
          flag = false;
          this.ShouldCreateWrapper = true;
        }
      }
      this.CanDeserialize = flag;
      Type createdType;
      ObjectConstructor<object> parameterizedCreator;
      if (!(this.CollectionItemType != (Type) null) || !ImmutableCollectionsUtils.TryBuildImmutableForArrayContract(this.NonNullableUnderlyingType, this.CollectionItemType, out createdType, out parameterizedCreator))
        return;
      this.CreatedType = createdType;
      this._parameterizedCreator = parameterizedCreator;
      this.IsReadOnlyOrFixedSize = true;
      this.CanDeserialize = true;
    }

    internal IWrappedCollection CreateWrapper(object list)
    {
      if (this._genericWrapperCreator == null)
      {
        this._genericWrapperType = typeof (CollectionWrapper<>).MakeGenericType(this.CollectionItemType);
        Type type;
        if (ReflectionUtils.InheritsGenericDefinition(this._genericCollectionDefinitionType, typeof (List<>)) || this._genericCollectionDefinitionType.GetGenericTypeDefinition() == typeof (IEnumerable<>))
          type = typeof (ICollection<>).MakeGenericType(this.CollectionItemType);
        else
          type = this._genericCollectionDefinitionType;
        this._genericWrapperCreator = JsonTypeReflector.ReflectionDelegateFactory.CreateParameterizedConstructor((MethodBase) this._genericWrapperType.GetConstructor(new Type[1]
        {
          type
        }));
      }
      return (IWrappedCollection) this._genericWrapperCreator(new object[1]
      {
        list
      });
    }

    internal IList CreateTemporaryCollection()
    {
      if (this._genericTemporaryCollectionCreator == null)
        this._genericTemporaryCollectionCreator = JsonTypeReflector.ReflectionDelegateFactory.CreateDefaultConstructor<object>(typeof (List<>).MakeGenericType(this.IsMultidimensionalArray || this.CollectionItemType == (Type) null ? typeof (object) : this.CollectionItemType));
      return (IList) this._genericTemporaryCollectionCreator();
    }

    private void StoreFSharpListCreatorIfNecessary(Type underlyingType)
    {
      if (this.HasParameterizedCreatorInternal || !(underlyingType.Name == "FSharpList`1"))
        return;
      FSharpUtils.EnsureInitialized(underlyingType.Assembly());
      this._parameterizedCreator = FSharpUtils.Instance.CreateSeq(this.CollectionItemType);
    }
  }
}
