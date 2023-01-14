// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Utilities.ReflectionObject
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\Newtonsoft.Json.dll

using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;


#nullable enable
namespace Newtonsoft.Json.Utilities
{
  internal class ReflectionObject
  {
    public ObjectConstructor<object>? Creator { get; }

    public IDictionary<string, ReflectionMember> Members { get; }

    private ReflectionObject(ObjectConstructor<object>? creator)
    {
      this.Members = (IDictionary<string, ReflectionMember>) new Dictionary<string, ReflectionMember>();
      this.Creator = creator;
    }

    public object? GetValue(object target, string member) => this.Members[member].Getter(target);

    public void SetValue(object target, string member, object? value) => this.Members[member].Setter(target, value);

    public Type GetType(string member) => this.Members[member].MemberType;

    public static ReflectionObject Create(Type t, params string[] memberNames) => ReflectionObject.Create(t, (MethodBase) null, memberNames);

    public static ReflectionObject Create(Type t, MethodBase? creator, params string[] memberNames)
    {
      ReflectionDelegateFactory reflectionDelegateFactory = JsonTypeReflector.ReflectionDelegateFactory;
      ObjectConstructor<object> creator1 = (ObjectConstructor<object>) null;
      if (creator != (MethodBase) null)
        creator1 = reflectionDelegateFactory.CreateParameterizedConstructor(creator);
      else if (ReflectionUtils.HasDefaultConstructor(t, false))
      {
        Func<object> ctor = reflectionDelegateFactory.CreateDefaultConstructor<object>(t);
        creator1 = (ObjectConstructor<object>) (args => ctor());
      }
      ReflectionObject reflectionObject = new ReflectionObject(creator1);
      foreach (string memberName in memberNames)
      {
        MemberInfo[] member = t.GetMember(memberName, BindingFlags.Instance | BindingFlags.Public);
        MemberInfo memberInfo = member.Length == 1 ? ((IEnumerable<MemberInfo>) member).Single<MemberInfo>() : throw new ArgumentException("Expected a single member with the name '{0}'.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) memberName));
        ReflectionMember reflectionMember = new ReflectionMember();
        switch (memberInfo.MemberType())
        {
          case MemberTypes.Field:
          case MemberTypes.Property:
            if (ReflectionUtils.CanReadMemberValue(memberInfo, false))
              reflectionMember.Getter = reflectionDelegateFactory.CreateGet<object>(memberInfo);
            if (ReflectionUtils.CanSetMemberValue(memberInfo, false, false))
            {
              reflectionMember.Setter = reflectionDelegateFactory.CreateSet<object>(memberInfo);
              break;
            }
            break;
          case MemberTypes.Method:
            MethodInfo method = (MethodInfo) memberInfo;
            if (method.IsPublic)
            {
              ParameterInfo[] parameters = method.GetParameters();
              if (parameters.Length == 0 && method.ReturnType != typeof (void))
              {
                MethodCall<object, object> call = reflectionDelegateFactory.CreateMethodCall<object>((MethodBase) method);
                reflectionMember.Getter = (Func<object, object>) (target => call(target, new object[0]));
                break;
              }
              if (parameters.Length == 1 && method.ReturnType == typeof (void))
              {
                MethodCall<object, object> call = reflectionDelegateFactory.CreateMethodCall<object>((MethodBase) method);
                object obj;
                reflectionMember.Setter = (Action<object, object>) ((target, arg) => obj = call(target, new object[1]
                {
                  arg
                }));
                break;
              }
              break;
            }
            break;
          default:
            throw new ArgumentException("Unexpected member type '{0}' for member '{1}'.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) memberInfo.MemberType(), (object) memberInfo.Name));
        }
        reflectionMember.MemberType = ReflectionUtils.GetMemberUnderlyingType(memberInfo);
        reflectionObject.Members[memberName] = reflectionMember;
      }
      return reflectionObject;
    }
  }
}
