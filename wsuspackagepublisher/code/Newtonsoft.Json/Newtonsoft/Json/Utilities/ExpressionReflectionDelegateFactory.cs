// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Utilities.ExpressionReflectionDelegateFactory
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\Newtonsoft.Json.dll

using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;


#nullable enable
namespace Newtonsoft.Json.Utilities
{
  internal class ExpressionReflectionDelegateFactory : ReflectionDelegateFactory
  {
    private static readonly ExpressionReflectionDelegateFactory _instance = new ExpressionReflectionDelegateFactory();

    internal static ReflectionDelegateFactory Instance => (ReflectionDelegateFactory) ExpressionReflectionDelegateFactory._instance;

    public override ObjectConstructor<object> CreateParameterizedConstructor(MethodBase method)
    {
      ValidationUtils.ArgumentNotNull((object) method, nameof (method));
      Type type = typeof (object);
      ParameterExpression argsParameterExpression = Expression.Parameter(typeof (object[]), "args");
      return (ObjectConstructor<object>) Expression.Lambda(typeof (ObjectConstructor<object>), this.BuildMethodCall(method, type, (ParameterExpression) null, argsParameterExpression), argsParameterExpression).Compile();
    }

    public override MethodCall<T, object?> CreateMethodCall<T>(MethodBase method)
    {
      ValidationUtils.ArgumentNotNull((object) method, nameof (method));
      Type type = typeof (object);
      ParameterExpression targetParameterExpression = Expression.Parameter(type, "target");
      ParameterExpression argsParameterExpression = Expression.Parameter(typeof (object[]), "args");
      return (MethodCall<T, object>) Expression.Lambda(typeof (MethodCall<T, object>), this.BuildMethodCall(method, type, targetParameterExpression, argsParameterExpression), targetParameterExpression, argsParameterExpression).Compile();
    }

    private Expression BuildMethodCall(
      MethodBase method,
      Type type,
      ParameterExpression? targetParameterExpression,
      ParameterExpression argsParameterExpression)
    {
      ParameterInfo[] parameters = method.GetParameters();
      Expression[] expressionArray;
      IList<ExpressionReflectionDelegateFactory.ByRefParameter> byRefParameterList;
      if (parameters.Length == 0)
      {
        expressionArray = CollectionUtils.ArrayEmpty<Expression>();
        byRefParameterList = (IList<ExpressionReflectionDelegateFactory.ByRefParameter>) CollectionUtils.ArrayEmpty<ExpressionReflectionDelegateFactory.ByRefParameter>();
      }
      else
      {
        expressionArray = new Expression[parameters.Length];
        byRefParameterList = (IList<ExpressionReflectionDelegateFactory.ByRefParameter>) new List<ExpressionReflectionDelegateFactory.ByRefParameter>();
        for (int index1 = 0; index1 < parameters.Length; ++index1)
        {
          ParameterInfo parameterInfo = parameters[index1];
          Type type1 = parameterInfo.ParameterType;
          bool flag = false;
          if (type1.IsByRef)
          {
            type1 = type1.GetElementType();
            flag = true;
          }
          Expression index2 = (Expression) Expression.Constant((object) index1);
          Expression expression = this.EnsureCastExpression((Expression) Expression.ArrayIndex((Expression) argsParameterExpression, index2), type1, !flag);
          if (flag)
          {
            ParameterExpression variable = Expression.Variable(type1);
            byRefParameterList.Add(new ExpressionReflectionDelegateFactory.ByRefParameter(expression, variable, parameterInfo.IsOut));
            expression = (Expression) variable;
          }
          expressionArray[index1] = expression;
        }
      }
      Expression expression1 = !method.IsConstructor ? (!method.IsStatic ? (Expression) Expression.Call(this.EnsureCastExpression((Expression) targetParameterExpression, method.DeclaringType), (MethodInfo) method, expressionArray) : (Expression) Expression.Call((MethodInfo) method, expressionArray)) : (Expression) Expression.New((ConstructorInfo) method, expressionArray);
      MethodInfo methodInfo = method as MethodInfo;
      Expression expression2 = (object) methodInfo == null ? this.EnsureCastExpression(expression1, type) : (!(methodInfo.ReturnType != typeof (void)) ? (Expression) Expression.Block(expression1, (Expression) Expression.Constant((object) null)) : this.EnsureCastExpression(expression1, type));
      if (byRefParameterList.Count > 0)
      {
        IList<ParameterExpression> variables = (IList<ParameterExpression>) new List<ParameterExpression>();
        IList<Expression> expressionList = (IList<Expression>) new List<Expression>();
        foreach (ExpressionReflectionDelegateFactory.ByRefParameter byRefParameter in (IEnumerable<ExpressionReflectionDelegateFactory.ByRefParameter>) byRefParameterList)
        {
          if (!byRefParameter.IsOut)
            expressionList.Add((Expression) Expression.Assign((Expression) byRefParameter.Variable, byRefParameter.Value));
          variables.Add(byRefParameter.Variable);
        }
        expressionList.Add(expression2);
        expression2 = (Expression) Expression.Block((IEnumerable<ParameterExpression>) variables, (IEnumerable<Expression>) expressionList);
      }
      return expression2;
    }

    public override Func<T> CreateDefaultConstructor<T>(Type type)
    {
      ValidationUtils.ArgumentNotNull((object) type, nameof (type));
      if (type.IsAbstract())
        return (Func<T>) (() => (T) Activator.CreateInstance(type));
      try
      {
        Type targetType = typeof (T);
        return (Func<T>) Expression.Lambda(typeof (Func<T>), this.EnsureCastExpression((Expression) Expression.New(type), targetType)).Compile();
      }
      catch
      {
        return (Func<T>) (() => (T) Activator.CreateInstance(type));
      }
    }

    public override Func<T, object?> CreateGet<T>(PropertyInfo propertyInfo)
    {
      ValidationUtils.ArgumentNotNull((object) propertyInfo, nameof (propertyInfo));
      Type type = typeof (T);
      Type targetType = typeof (object);
      ParameterExpression parameterExpression = Expression.Parameter(type, "instance");
      MethodInfo getMethod = propertyInfo.GetGetMethod(true);
      if (getMethod == (MethodInfo) null)
        throw new ArgumentException("Property does not have a getter.");
      return (Func<T, object>) Expression.Lambda(typeof (Func<T, object>), this.EnsureCastExpression(!getMethod.IsStatic ? (Expression) Expression.MakeMemberAccess(this.EnsureCastExpression((Expression) parameterExpression, propertyInfo.DeclaringType), (MemberInfo) propertyInfo) : (Expression) Expression.MakeMemberAccess((Expression) null, (MemberInfo) propertyInfo), targetType), parameterExpression).Compile();
    }

    public override Func<T, object?> CreateGet<T>(FieldInfo fieldInfo)
    {
      ValidationUtils.ArgumentNotNull((object) fieldInfo, nameof (fieldInfo));
      ParameterExpression parameterExpression;
      return Expression.Lambda<Func<T, object>>(this.EnsureCastExpression(!fieldInfo.IsStatic ? (Expression) Expression.Field(this.EnsureCastExpression((Expression) parameterExpression, fieldInfo.DeclaringType), fieldInfo) : (Expression) Expression.Field((Expression) null, fieldInfo), typeof (object)), parameterExpression).Compile();
    }

    public override Action<T, object?> CreateSet<T>(FieldInfo fieldInfo)
    {
      ValidationUtils.ArgumentNotNull((object) fieldInfo, nameof (fieldInfo));
      if (fieldInfo.DeclaringType.IsValueType() || fieldInfo.IsInitOnly)
        return LateBoundReflectionDelegateFactory.Instance.CreateSet<T>(fieldInfo);
      ParameterExpression parameterExpression1 = Expression.Parameter(typeof (T), "source");
      ParameterExpression parameterExpression2 = Expression.Parameter(typeof (object), "value");
      Expression left = !fieldInfo.IsStatic ? (Expression) Expression.Field(this.EnsureCastExpression((Expression) parameterExpression1, fieldInfo.DeclaringType), fieldInfo) : (Expression) Expression.Field((Expression) null, fieldInfo);
      Expression right = this.EnsureCastExpression((Expression) parameterExpression2, left.Type);
      return (Action<T, object>) Expression.Lambda(typeof (Action<T, object>), (Expression) Expression.Assign(left, right), parameterExpression1, parameterExpression2).Compile();
    }

    public override Action<T, object?> CreateSet<T>(PropertyInfo propertyInfo)
    {
      ValidationUtils.ArgumentNotNull((object) propertyInfo, nameof (propertyInfo));
      if (propertyInfo.DeclaringType.IsValueType())
        return LateBoundReflectionDelegateFactory.Instance.CreateSet<T>(propertyInfo);
      Type type1 = typeof (T);
      Type type2 = typeof (object);
      ParameterExpression parameterExpression1 = Expression.Parameter(type1, "instance");
      ParameterExpression parameterExpression2 = Expression.Parameter(type2, "value");
      Expression expression = this.EnsureCastExpression((Expression) parameterExpression2, propertyInfo.PropertyType);
      MethodInfo setMethod = propertyInfo.GetSetMethod(true);
      if (setMethod == (MethodInfo) null)
        throw new ArgumentException("Property does not have a setter.");
      Expression body;
      if (setMethod.IsStatic)
        body = (Expression) Expression.Call(setMethod, expression);
      else
        body = (Expression) Expression.Call(this.EnsureCastExpression((Expression) parameterExpression1, propertyInfo.DeclaringType), setMethod, expression);
      return (Action<T, object>) Expression.Lambda(typeof (Action<T, object>), body, parameterExpression1, parameterExpression2).Compile();
    }

    private Expression EnsureCastExpression(
      Expression expression,
      Type targetType,
      bool allowWidening = false)
    {
      Type type = expression.Type;
      if (type == targetType || !type.IsValueType() && targetType.IsAssignableFrom(type))
        return expression;
      if (!targetType.IsValueType())
        return (Expression) Expression.Convert(expression, targetType);
      Expression expression1 = (Expression) Expression.Unbox(expression, targetType);
      if (allowWidening && targetType.IsPrimitive())
      {
        MethodInfo method = typeof (Convert).GetMethod("To" + targetType.Name, new Type[1]
        {
          typeof (object)
        });
        if (method != (MethodInfo) null)
          expression1 = (Expression) Expression.Condition((Expression) Expression.TypeIs(expression, targetType), expression1, (Expression) Expression.Call(method, expression));
      }
      return (Expression) Expression.Condition((Expression) Expression.Equal(expression, (Expression) Expression.Constant((object) null, typeof (object))), (Expression) Expression.Default(targetType), expression1);
    }

    private class ByRefParameter
    {
      public Expression Value;
      public ParameterExpression Variable;
      public bool IsOut;

      public ByRefParameter(Expression value, ParameterExpression variable, bool isOut)
      {
        this.Value = value;
        this.Variable = variable;
        this.IsOut = isOut;
      }
    }
  }
}
