// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Bson.BsonEmpty
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\Newtonsoft.Json.dll

namespace Newtonsoft.Json.Bson
{
  internal class BsonEmpty : BsonToken
  {
    public static readonly BsonToken Null = (BsonToken) new BsonEmpty(BsonType.Null);
    public static readonly BsonToken Undefined = (BsonToken) new BsonEmpty(BsonType.Undefined);

    private BsonEmpty(BsonType type) => this.Type = type;

    public override BsonType Type { get; }
  }
}
