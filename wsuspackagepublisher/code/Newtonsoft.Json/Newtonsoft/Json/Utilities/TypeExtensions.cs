// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Utilities.TypeExtensions
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\Newtonsoft.Json.dll

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;


#nullable enable
namespace Newtonsoft.Json.Utilities
{
  internal static class TypeExtensions
  {
    public static MethodInfo Method(this Delegate d) => d.Method;

    public static MemberTypes MemberType(this MemberInfo memberInfo) => memberInfo.MemberType;

    public static bool ContainsGenericParameters(this Type type) => type.ContainsGenericParameters;

    public static bool IsInterface(this Type type) => type.IsInterface;

    public static bool IsGenericType(this Type type) => type.IsGenericType;

    public static bool IsGenericTypeDefinition(this Type type) => type.IsGenericTypeDefinition;

    public static Type BaseType(this Type type) => type.BaseType;

    public static System.Reflection.Assembly Assembly(this Type type) => type.Assembly;

    public static bool IsEnum(this Type type) => type.IsEnum;

    public static bool IsClass(this Type type) => type.IsClass;

    public static bool IsSealed(this Type type) => type.IsSealed;

    public static bool IsAbstract(this Type type) => type.IsAbstract;

    public static bool IsVisible(this Type type) => type.IsVisible;

    public static bool IsValueType(this Type type) => type.IsValueType;

    public static bool IsPrimitive(this Type type) => type.IsPrimitive;

    public static bool AssignableToTypeName(
      this Type type,
      string fullTypeName,
      bool searchInterfaces,
      [NotNullWhen(true)] out Type? match)
    {
      for (Type type1 = type; type1 != (Type) null; type1 = type1.BaseType())
      {
        if (string.Equals(type1.FullName, fullTypeName, StringComparison.Ordinal))
        {
          match = type1;
          return true;
        }
      }
      if (searchInterfaces)
      {
        foreach (MemberInfo memberInfo in type.GetInterfaces())
        {
          if (string.Equals(memberInfo.Name, fullTypeName, StringComparison.Ordinal))
          {
            match = type;
            return true;
          }
        }
      }
      match = (Type) null;
      return false;
    }

    public static bool AssignableToTypeName(
      this Type type,
      string fullTypeName,
      bool searchInterfaces)
    {
      return type.AssignableToTypeName(fullTypeName, searchInterfaces, out Type _);
    }

    public static bool ImplementInterface(this Type type, Type interfaceType)
    {
      for (Type type1 = type; type1 != (Type) null; type1 = type1.BaseType())
      {
        foreach (Type type2 in (IEnumerable<Type>) type1.GetInterfaces())
        {
          if (type2 == interfaceType || type2 != (Type) null && type2.ImplementInterface(interfaceType))
            return true;
        }
      }
      return false;
    }
  }
}
