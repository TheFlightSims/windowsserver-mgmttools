// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.JsonPrimitiveContract
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;


#nullable enable
namespace Newtonsoft.Json.Serialization
{
  public class JsonPrimitiveContract : JsonContract
  {
    private static readonly Dictionary<Type, ReadType> ReadTypeMap = new Dictionary<Type, ReadType>()
    {
      [typeof (byte[])] = ReadType.ReadAsBytes,
      [typeof (byte)] = ReadType.ReadAsInt32,
      [typeof (short)] = ReadType.ReadAsInt32,
      [typeof (int)] = ReadType.ReadAsInt32,
      [typeof (Decimal)] = ReadType.ReadAsDecimal,
      [typeof (bool)] = ReadType.ReadAsBoolean,
      [typeof (string)] = ReadType.ReadAsString,
      [typeof (DateTime)] = ReadType.ReadAsDateTime,
      [typeof (DateTimeOffset)] = ReadType.ReadAsDateTimeOffset,
      [typeof (float)] = ReadType.ReadAsDouble,
      [typeof (double)] = ReadType.ReadAsDouble,
      [typeof (long)] = ReadType.ReadAsInt64
    };

    internal PrimitiveTypeCode TypeCode { get; set; }

    public JsonPrimitiveContract(Type underlyingType)
      : base(underlyingType)
    {
      this.ContractType = JsonContractType.Primitive;
      this.TypeCode = ConvertUtils.GetTypeCode(underlyingType);
      this.IsReadOnlyOrFixedSize = true;
      ReadType readType;
      if (!JsonPrimitiveContract.ReadTypeMap.TryGetValue(this.NonNullableUnderlyingType, out readType))
        return;
      this.InternalReadType = readType;
    }
  }
}
