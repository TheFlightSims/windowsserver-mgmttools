// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Bson.BsonObjectId
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System;

namespace Newtonsoft.Json.Bson
{
  [Obsolete("BSON reading and writing has been moved to its own package. See https://www.nuget.org/packages/Newtonsoft.Json.Bson for more details.")]
  public class BsonObjectId
  {
    public byte[] Value { get; }

    public BsonObjectId(byte[] value)
    {
      ValidationUtils.ArgumentNotNull((object) value, nameof (value));
      this.Value = value.Length == 12 ? value : throw new ArgumentException("An ObjectId must be 12 bytes", nameof (value));
    }
  }
}
