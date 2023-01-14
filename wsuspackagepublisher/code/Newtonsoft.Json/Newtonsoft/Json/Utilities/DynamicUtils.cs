// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Utilities.DynamicUtils
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\Newtonsoft.Json.dll

using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;


#nullable enable
namespace Newtonsoft.Json.Utilities
{
  internal static class DynamicUtils
  {
    public static IEnumerable<string> GetDynamicMemberNames(
      this IDynamicMetaObjectProvider dynamicProvider)
    {
      return dynamicProvider.GetMetaObject((Expression) Expression.Constant((object) dynamicProvider)).GetDynamicMemberNames();
    }

    internal static class BinderWrapper
    {
      public const string CSharpAssemblyName = "Microsoft.CSharp, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";
      private const string BinderTypeName = "Microsoft.CSharp.RuntimeBinder.Binder, Microsoft.CSharp, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";
      private const string CSharpArgumentInfoTypeName = "Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo, Microsoft.CSharp, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";
      private const string CSharpArgumentInfoFlagsTypeName = "Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfoFlags, Microsoft.CSharp, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";
      private const string CSharpBinderFlagsTypeName = "Microsoft.CSharp.RuntimeBinder.CSharpBinderFlags, Microsoft.CSharp, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";
      private static object? _getCSharpArgumentInfoArray;
      private static object? _setCSharpArgumentInfoArray;
      private static MethodCall<object?, object?>? _getMemberCall;
      private static MethodCall<object?, object?>? _setMemberCall;
      private static bool _init;

      private static void Init()
      {
        if (DynamicUtils.BinderWrapper._init)
          return;
        if (Type.GetType("Microsoft.CSharp.RuntimeBinder.Binder, Microsoft.CSharp, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false) == (Type) null)
          throw new InvalidOperationException("Could not resolve type '{0}'. You may need to add a reference to Microsoft.CSharp.dll to work with dynamic types.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) "Microsoft.CSharp.RuntimeBinder.Binder, Microsoft.CSharp, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"));
        DynamicUtils.BinderWrapper._getCSharpArgumentInfoArray = DynamicUtils.BinderWrapper.CreateSharpArgumentInfoArray(new int[1]);
        DynamicUtils.BinderWrapper._setCSharpArgumentInfoArray = DynamicUtils.BinderWrapper.CreateSharpArgumentInfoArray(0, 3);
        DynamicUtils.BinderWrapper.CreateMemberCalls();
        DynamicUtils.BinderWrapper._init = true;
      }

      private static object CreateSharpArgumentInfoArray(params int[] values)
      {
        Type type1 = Type.GetType("Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo, Microsoft.CSharp, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
        Type type2 = Type.GetType("Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfoFlags, Microsoft.CSharp, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
        Array instance = Array.CreateInstance(type1, values.Length);
        for (int index = 0; index < values.Length; ++index)
        {
          object obj = type1.GetMethod("Create", new Type[2]
          {
            type2,
            typeof (string)
          }).Invoke((object) null, new object[2]
          {
            (object) 0,
            null
          });
          instance.SetValue(obj, index);
        }
        return (object) instance;
      }

      private static void CreateMemberCalls()
      {
        Type type1 = Type.GetType("Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo, Microsoft.CSharp, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", true);
        Type type2 = Type.GetType("Microsoft.CSharp.RuntimeBinder.CSharpBinderFlags, Microsoft.CSharp, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", true);
        Type type3 = Type.GetType("Microsoft.CSharp.RuntimeBinder.Binder, Microsoft.CSharp, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", true);
        Type type4 = typeof (IEnumerable<>).MakeGenericType(type1);
        DynamicUtils.BinderWrapper._getMemberCall = JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>((MethodBase) type3.GetMethod("GetMember", new Type[4]
        {
          type2,
          typeof (string),
          typeof (Type),
          type4
        }));
        DynamicUtils.BinderWrapper._setMemberCall = JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>((MethodBase) type3.GetMethod("SetMember", new Type[4]
        {
          type2,
          typeof (string),
          typeof (Type),
          type4
        }));
      }

      public static CallSiteBinder GetMember(string name, Type context)
      {
        DynamicUtils.BinderWrapper.Init();
        return (CallSiteBinder) DynamicUtils.BinderWrapper._getMemberCall((object) null, new object[4]
        {
          (object) 0,
          (object) name,
          (object) context,
          DynamicUtils.BinderWrapper._getCSharpArgumentInfoArray
        });
      }

      public static CallSiteBinder SetMember(string name, Type context)
      {
        DynamicUtils.BinderWrapper.Init();
        return (CallSiteBinder) DynamicUtils.BinderWrapper._setMemberCall((object) null, new object[4]
        {
          (object) 0,
          (object) name,
          (object) context,
          DynamicUtils.BinderWrapper._setCSharpArgumentInfoArray
        });
      }
    }
  }
}
