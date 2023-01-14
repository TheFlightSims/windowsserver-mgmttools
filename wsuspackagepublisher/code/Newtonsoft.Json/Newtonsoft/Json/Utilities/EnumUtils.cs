// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Utilities.EnumUtils
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\Newtonsoft.Json.dll

using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;


#nullable enable
namespace Newtonsoft.Json.Utilities
{
  internal static class EnumUtils
  {
    private const char EnumSeparatorChar = ',';
    private const string EnumSeparatorString = ", ";
    private static readonly ThreadSafeStore<StructMultiKey<Type, NamingStrategy?>, EnumInfo> ValuesAndNamesPerEnum = new ThreadSafeStore<StructMultiKey<Type, NamingStrategy>, EnumInfo>(new Func<StructMultiKey<Type, NamingStrategy>, EnumInfo>(EnumUtils.InitializeValuesAndNames));
    private static CamelCaseNamingStrategy _camelCaseNamingStrategy = new CamelCaseNamingStrategy();

    private static EnumInfo InitializeValuesAndNames(StructMultiKey<Type, NamingStrategy?> key)
    {
      Type enumType = key.Value1;
      string[] names = Enum.GetNames(enumType);
      string[] strArray = new string[names.Length];
      ulong[] values = new ulong[names.Length];
      for (int count = 0; count < names.Length; ++count)
      {
        string name1 = names[count];
        FieldInfo field = enumType.GetField(name1, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
        values[count] = EnumUtils.ToUInt64(field.GetValue((object) null));
        string str = field.GetCustomAttributes(typeof (EnumMemberAttribute), true).Cast<EnumMemberAttribute>().Select<EnumMemberAttribute, string>((Func<EnumMemberAttribute, string>) (a => a.Value)).SingleOrDefault<string>();
        bool hasSpecifiedName = str != null;
        if (str == null)
          str = name1;
        string name2 = str;
        if (Array.IndexOf<string>(strArray, name2, 0, count) != -1)
          throw new InvalidOperationException("Enum name '{0}' already exists on enum '{1}'.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) name2, (object) enumType.Name));
        strArray[count] = key.Value2 != null ? key.Value2.GetPropertyName(name2, hasSpecifiedName) : name2;
      }
      return new EnumInfo(enumType.IsDefined(typeof (FlagsAttribute), false), values, names, strArray);
    }

    public static IList<T> GetFlagsValues<T>(T value) where T : struct
    {
      Type enumType = typeof (T);
      if (!enumType.IsDefined(typeof (FlagsAttribute), false))
        throw new ArgumentException("Enum type {0} is not a set of flags.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) enumType));
      Type underlyingType = Enum.GetUnderlyingType(value.GetType());
      ulong uint64 = EnumUtils.ToUInt64((object) value);
      EnumInfo enumValuesAndNames = EnumUtils.GetEnumValuesAndNames(enumType);
      IList<T> flagsValues = (IList<T>) new List<T>();
      for (int index = 0; index < enumValuesAndNames.Values.Length; ++index)
      {
        ulong num = enumValuesAndNames.Values[index];
        if (((long) uint64 & (long) num) == (long) num && num != 0UL)
          flagsValues.Add((T) Convert.ChangeType((object) num, underlyingType, (IFormatProvider) CultureInfo.CurrentCulture));
      }
      if (flagsValues.Count == 0 && ((IEnumerable<ulong>) enumValuesAndNames.Values).Any<ulong>((Func<ulong, bool>) (v => v == 0UL)))
        flagsValues.Add(default (T));
      return flagsValues;
    }

    public static bool TryToString(Type enumType, object value, bool camelCase, [NotNullWhen(true)] out string? name) => EnumUtils.TryToString(enumType, value, camelCase ? (NamingStrategy) EnumUtils._camelCaseNamingStrategy : (NamingStrategy) null, out name);

    public static bool TryToString(
      Type enumType,
      object value,
      NamingStrategy? namingStrategy,
      [NotNullWhen(true)] out string? name)
    {
      EnumInfo entry = EnumUtils.ValuesAndNamesPerEnum.Get(new StructMultiKey<Type, NamingStrategy>(enumType, namingStrategy));
      ulong uint64 = EnumUtils.ToUInt64(value);
      if (!entry.IsFlags)
      {
        int index = Array.BinarySearch<ulong>(entry.Values, uint64);
        if (index >= 0)
        {
          name = entry.ResolvedNames[index];
          return true;
        }
        name = (string) null;
        return false;
      }
      name = EnumUtils.InternalFlagsFormat(entry, uint64);
      return name != null;
    }

    private static string? InternalFlagsFormat(EnumInfo entry, ulong result)
    {
      string[] resolvedNames = entry.ResolvedNames;
      ulong[] values = entry.Values;
      int index = values.Length - 1;
      StringBuilder stringBuilder = new StringBuilder();
      bool flag = true;
      ulong num = result;
      for (; index >= 0 && (index != 0 || values[index] != 0UL); --index)
      {
        if (((long) result & (long) values[index]) == (long) values[index])
        {
          result -= values[index];
          if (!flag)
            stringBuilder.Insert(0, ", ");
          string str = resolvedNames[index];
          stringBuilder.Insert(0, str);
          flag = false;
        }
      }
      return result == 0UL ? (num != 0UL ? stringBuilder.ToString() : (values.Length == 0 || values[0] != 0UL ? (string) null : resolvedNames[0])) : (string) null;
    }

    public static EnumInfo GetEnumValuesAndNames(Type enumType) => EnumUtils.ValuesAndNamesPerEnum.Get(new StructMultiKey<Type, NamingStrategy>(enumType, (NamingStrategy) null));

    private static ulong ToUInt64(object value)
    {
      switch (ConvertUtils.GetTypeCode(value.GetType(), out bool _))
      {
        case PrimitiveTypeCode.Char:
          return (ulong) (char) value;
        case PrimitiveTypeCode.Boolean:
          return (ulong) Convert.ToByte((bool) value);
        case PrimitiveTypeCode.SByte:
          return (ulong) (sbyte) value;
        case PrimitiveTypeCode.Int16:
          return (ulong) (short) value;
        case PrimitiveTypeCode.UInt16:
          return (ulong) (ushort) value;
        case PrimitiveTypeCode.Int32:
          return (ulong) (int) value;
        case PrimitiveTypeCode.Byte:
          return (ulong) (byte) value;
        case PrimitiveTypeCode.UInt32:
          return (ulong) (uint) value;
        case PrimitiveTypeCode.Int64:
          return (ulong) (long) value;
        case PrimitiveTypeCode.UInt64:
          return (ulong) value;
        default:
          throw new InvalidOperationException("Unknown enum type.");
      }
    }

    public static object ParseEnum(
      Type enumType,
      NamingStrategy? namingStrategy,
      string value,
      bool disallowNumber)
    {
      ValidationUtils.ArgumentNotNull((object) enumType, nameof (enumType));
      ValidationUtils.ArgumentNotNull((object) value, nameof (value));
      if (!enumType.IsEnum())
        throw new ArgumentException("Type provided must be an Enum.", nameof (enumType));
      EnumInfo enumInfo = EnumUtils.ValuesAndNamesPerEnum.Get(new StructMultiKey<Type, NamingStrategy>(enumType, namingStrategy));
      string[] names = enumInfo.Names;
      string[] resolvedNames = enumInfo.ResolvedNames;
      ulong[] values = enumInfo.Values;
      int? indexByName = EnumUtils.FindIndexByName(resolvedNames, value, 0, value.Length, StringComparison.Ordinal);
      if (indexByName.HasValue)
        return Enum.ToObject(enumType, values[indexByName.Value]);
      int index1 = -1;
      for (int index2 = 0; index2 < value.Length; ++index2)
      {
        if (!char.IsWhiteSpace(value[index2]))
        {
          index1 = index2;
          break;
        }
      }
      char c = index1 != -1 ? value[index1] : throw new ArgumentException("Must specify valid information for parsing in the string.");
      if (char.IsDigit(c) || c == '-' || c == '+')
      {
        Type underlyingType = Enum.GetUnderlyingType(enumType);
        value = value.Trim();
        object obj = (object) null;
        try
        {
          obj = Convert.ChangeType((object) value, underlyingType, (IFormatProvider) CultureInfo.InvariantCulture);
        }
        catch (FormatException ex)
        {
        }
        if (obj != null)
        {
          if (disallowNumber)
            throw new FormatException("Integer string '{0}' is not allowed.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) value));
          return Enum.ToObject(enumType, obj);
        }
      }
      ulong num1 = 0;
      int num2;
      for (int index3 = index1; index3 <= value.Length; index3 = num2 + 1)
      {
        num2 = value.IndexOf(',', index3);
        if (num2 == -1)
          num2 = value.Length;
        int num3 = num2;
        while (index3 < num2 && char.IsWhiteSpace(value[index3]))
          ++index3;
        while (num3 > index3 && char.IsWhiteSpace(value[num3 - 1]))
          --num3;
        int valueSubstringLength = num3 - index3;
        int? nullable = EnumUtils.MatchName(value, names, resolvedNames, index3, valueSubstringLength, StringComparison.Ordinal);
        if (!nullable.HasValue)
          nullable = EnumUtils.MatchName(value, names, resolvedNames, index3, valueSubstringLength, StringComparison.OrdinalIgnoreCase);
        if (!nullable.HasValue)
        {
          nullable = EnumUtils.FindIndexByName(resolvedNames, value, 0, value.Length, StringComparison.OrdinalIgnoreCase);
          if (nullable.HasValue)
            return Enum.ToObject(enumType, values[nullable.Value]);
          throw new ArgumentException("Requested value '{0}' was not found.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) value));
        }
        num1 |= values[nullable.Value];
      }
      return Enum.ToObject(enumType, num1);
    }

    private static int? MatchName(
      string value,
      string[] enumNames,
      string[] resolvedNames,
      int valueIndex,
      int valueSubstringLength,
      StringComparison comparison)
    {
      int? indexByName = EnumUtils.FindIndexByName(resolvedNames, value, valueIndex, valueSubstringLength, comparison);
      if (!indexByName.HasValue)
        indexByName = EnumUtils.FindIndexByName(enumNames, value, valueIndex, valueSubstringLength, comparison);
      return indexByName;
    }

    private static int? FindIndexByName(
      string[] enumNames,
      string value,
      int valueIndex,
      int valueSubstringLength,
      StringComparison comparison)
    {
      for (int index = 0; index < enumNames.Length; ++index)
      {
        if (enumNames[index].Length == valueSubstringLength && string.Compare(enumNames[index], 0, value, valueIndex, valueSubstringLength, comparison) == 0)
          return new int?(index);
      }
      return new int?();
    }
  }
}
