// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Utilities.FSharpUtils
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\Newtonsoft.Json.dll

using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


#nullable enable
namespace Newtonsoft.Json.Utilities
{
  internal class FSharpUtils
  {
    private static readonly object Lock = new object();
    private static FSharpUtils? _instance;
    private MethodInfo _ofSeq;
    private Type _mapType;
    public const string FSharpSetTypeName = "FSharpSet`1";
    public const string FSharpListTypeName = "FSharpList`1";
    public const string FSharpMapTypeName = "FSharpMap`2";

    private FSharpUtils(Assembly fsharpCoreAssembly)
    {
      this.FSharpCoreAssembly = fsharpCoreAssembly;
      Type type1 = fsharpCoreAssembly.GetType("Microsoft.FSharp.Reflection.FSharpType");
      this.IsUnion = JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>((MethodBase) FSharpUtils.GetMethodWithNonPublicFallback(type1, nameof (IsUnion), BindingFlags.Static | BindingFlags.Public));
      this.GetUnionCases = JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>((MethodBase) FSharpUtils.GetMethodWithNonPublicFallback(type1, nameof (GetUnionCases), BindingFlags.Static | BindingFlags.Public));
      Type type2 = fsharpCoreAssembly.GetType("Microsoft.FSharp.Reflection.FSharpValue");
      this.PreComputeUnionTagReader = FSharpUtils.CreateFSharpFuncCall(type2, nameof (PreComputeUnionTagReader));
      this.PreComputeUnionReader = FSharpUtils.CreateFSharpFuncCall(type2, nameof (PreComputeUnionReader));
      this.PreComputeUnionConstructor = FSharpUtils.CreateFSharpFuncCall(type2, nameof (PreComputeUnionConstructor));
      Type type3 = fsharpCoreAssembly.GetType("Microsoft.FSharp.Reflection.UnionCaseInfo");
      this.GetUnionCaseInfoName = JsonTypeReflector.ReflectionDelegateFactory.CreateGet<object>(type3.GetProperty("Name"));
      this.GetUnionCaseInfoTag = JsonTypeReflector.ReflectionDelegateFactory.CreateGet<object>(type3.GetProperty("Tag"));
      this.GetUnionCaseInfoDeclaringType = JsonTypeReflector.ReflectionDelegateFactory.CreateGet<object>(type3.GetProperty("DeclaringType"));
      this.GetUnionCaseInfoFields = JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>((MethodBase) type3.GetMethod("GetFields"));
      this._ofSeq = fsharpCoreAssembly.GetType("Microsoft.FSharp.Collections.ListModule").GetMethod("OfSeq");
      this._mapType = fsharpCoreAssembly.GetType("Microsoft.FSharp.Collections.FSharpMap`2");
    }

    public static FSharpUtils Instance => FSharpUtils._instance;

    public Assembly FSharpCoreAssembly { get; private set; }

    public MethodCall<object?, object> IsUnion { get; private set; }

    public MethodCall<object?, object> GetUnionCases { get; private set; }

    public MethodCall<object?, object> PreComputeUnionTagReader { get; private set; }

    public MethodCall<object?, object> PreComputeUnionReader { get; private set; }

    public MethodCall<object?, object> PreComputeUnionConstructor { get; private set; }

    public Func<object, object> GetUnionCaseInfoDeclaringType { get; private set; }

    public Func<object, object> GetUnionCaseInfoName { get; private set; }

    public Func<object, object> GetUnionCaseInfoTag { get; private set; }

    public MethodCall<object, object?> GetUnionCaseInfoFields { get; private set; }

    public static void EnsureInitialized(Assembly fsharpCoreAssembly)
    {
      if (FSharpUtils._instance != null)
        return;
      lock (FSharpUtils.Lock)
      {
        if (FSharpUtils._instance != null)
          return;
        FSharpUtils._instance = new FSharpUtils(fsharpCoreAssembly);
      }
    }

    private static MethodInfo GetMethodWithNonPublicFallback(
      Type type,
      string methodName,
      BindingFlags bindingFlags)
    {
      MethodInfo method = type.GetMethod(methodName, bindingFlags);
      if (method == (MethodInfo) null && (bindingFlags & BindingFlags.NonPublic) != BindingFlags.NonPublic)
        method = type.GetMethod(methodName, bindingFlags | BindingFlags.NonPublic);
      return method;
    }

    private static MethodCall<object?, object> CreateFSharpFuncCall(Type type, string methodName)
    {
      MethodInfo nonPublicFallback = FSharpUtils.GetMethodWithNonPublicFallback(type, methodName, BindingFlags.Static | BindingFlags.Public);
      MethodInfo method = nonPublicFallback.ReturnType.GetMethod("Invoke", BindingFlags.Instance | BindingFlags.Public);
      MethodCall<object, object> call = JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>((MethodBase) nonPublicFallback);
      MethodCall<object, object> invoke = JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>((MethodBase) method);
      return (MethodCall<object, object>) ((target, args) => (object) new FSharpFunction(call(target, args), invoke));
    }

    public ObjectConstructor<object> CreateSeq(Type t) => JsonTypeReflector.ReflectionDelegateFactory.CreateParameterizedConstructor((MethodBase) this._ofSeq.MakeGenericMethod(t));

    public ObjectConstructor<object> CreateMap(Type keyType, Type valueType) => (ObjectConstructor<object>) typeof (FSharpUtils).GetMethod("BuildMapCreator").MakeGenericMethod(keyType, valueType).Invoke((object) this, (object[]) null);

    public ObjectConstructor<object> BuildMapCreator<TKey, TValue>()
    {
      ObjectConstructor<object> ctorDelegate = JsonTypeReflector.ReflectionDelegateFactory.CreateParameterizedConstructor((MethodBase) this._mapType.MakeGenericType(typeof (TKey), typeof (TValue)).GetConstructor(new Type[1]
      {
        typeof (IEnumerable<Tuple<TKey, TValue>>)
      }));
      return (ObjectConstructor<object>) (args =>
      {
        IEnumerable<Tuple<TKey, TValue>> tuples = ((IEnumerable<KeyValuePair<TKey, TValue>>) args[0]).Select<KeyValuePair<TKey, TValue>, Tuple<TKey, TValue>>((Func<KeyValuePair<TKey, TValue>, Tuple<TKey, TValue>>) (kv => new Tuple<TKey, TValue>(kv.Key, kv.Value)));
        return ctorDelegate(new object[1]{ (object) tuples });
      });
    }
  }
}
