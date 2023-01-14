// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Bson.BsonArray
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\Newtonsoft.Json.dll

using System.Collections;
using System.Collections.Generic;

namespace Newtonsoft.Json.Bson
{
  internal class BsonArray : BsonToken, IEnumerable<BsonToken>, IEnumerable
  {
    private readonly List<BsonToken> _children = new List<BsonToken>();

    public void Add(BsonToken token)
    {
      this._children.Add(token);
      token.Parent = (BsonToken) this;
    }

    public override BsonType Type => BsonType.Array;

    public IEnumerator<BsonToken> GetEnumerator() => (IEnumerator<BsonToken>) this._children.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();
  }
}
