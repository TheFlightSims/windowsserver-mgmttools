// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Bson.BsonBinaryType
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\Newtonsoft.Json.dll

using System;

namespace Newtonsoft.Json.Bson
{
  internal enum BsonBinaryType : byte
  {
    Binary = 0,
    Function = 1,
    [Obsolete("This type has been deprecated in the BSON specification. Use Binary instead.")] BinaryOld = 2,
    [Obsolete("This type has been deprecated in the BSON specification. Use Uuid instead.")] UuidOld = 3,
    Uuid = 4,
    Md5 = 5,
    UserDefined = 128, // 0x80
  }
}
