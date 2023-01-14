// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Bson.BsonBoolean
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\Newtonsoft.Json.dll

namespace Newtonsoft.Json.Bson
{
  internal class BsonBoolean : BsonValue
  {
    public static readonly BsonBoolean False = new BsonBoolean(false);
    public static readonly BsonBoolean True = new BsonBoolean(true);

    private BsonBoolean(bool value)
      : base((object) value, BsonType.Boolean)
    {
    }
  }
}
