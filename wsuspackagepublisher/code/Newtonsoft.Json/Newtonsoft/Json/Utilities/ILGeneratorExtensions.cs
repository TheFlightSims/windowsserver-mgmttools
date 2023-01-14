// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Utilities.ILGeneratorExtensions
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\Newtonsoft.Json.dll

using System;
using System.Reflection;
using System.Reflection.Emit;


#nullable enable
namespace Newtonsoft.Json.Utilities
{
  internal static class ILGeneratorExtensions
  {
    public static void PushInstance(this ILGenerator generator, Type type)
    {
      generator.Emit(OpCodes.Ldarg_0);
      if (type.IsValueType())
        generator.Emit(OpCodes.Unbox, type);
      else
        generator.Emit(OpCodes.Castclass, type);
    }

    public static void PushArrayInstance(this ILGenerator generator, int argsIndex, int arrayIndex)
    {
      generator.Emit(OpCodes.Ldarg, argsIndex);
      generator.Emit(OpCodes.Ldc_I4, arrayIndex);
      generator.Emit(OpCodes.Ldelem_Ref);
    }

    public static void BoxIfNeeded(this ILGenerator generator, Type type)
    {
      if (type.IsValueType())
        generator.Emit(OpCodes.Box, type);
      else
        generator.Emit(OpCodes.Castclass, type);
    }

    public static void UnboxIfNeeded(this ILGenerator generator, Type type)
    {
      if (type.IsValueType())
        generator.Emit(OpCodes.Unbox_Any, type);
      else
        generator.Emit(OpCodes.Castclass, type);
    }

    public static void CallMethod(this ILGenerator generator, MethodInfo methodInfo)
    {
      if (methodInfo.IsFinal || !methodInfo.IsVirtual)
        generator.Emit(OpCodes.Call, methodInfo);
      else
        generator.Emit(OpCodes.Callvirt, methodInfo);
    }

    public static void Return(this ILGenerator generator) => generator.Emit(OpCodes.Ret);
  }
}
