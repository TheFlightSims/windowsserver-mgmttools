// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Bson.BsonType
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\Newtonsoft.Json.dll

namespace Newtonsoft.Json.Bson
{
  internal enum BsonType : sbyte
  {
    MinKey = -1, // 0xFF
    Number = 1,
    String = 2,
    Object = 3,
    Array = 4,
    Binary = 5,
    Undefined = 6,
    Oid = 7,
    Boolean = 8,
    Date = 9,
    Null = 10, // 0x0A
    Regex = 11, // 0x0B
    Reference = 12, // 0x0C
    Code = 13, // 0x0D
    Symbol = 14, // 0x0E
    CodeWScope = 15, // 0x0F
    Integer = 16, // 0x10
    TimeStamp = 17, // 0x11
    Long = 18, // 0x12
    MaxKey = 127, // 0x7F
  }
}
