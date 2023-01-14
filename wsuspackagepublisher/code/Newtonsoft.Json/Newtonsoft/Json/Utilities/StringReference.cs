// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Utilities.StringReference
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\Newtonsoft.Json.dll


#nullable enable
namespace Newtonsoft.Json.Utilities
{
  internal readonly struct StringReference
  {
    private readonly char[] _chars;
    private readonly int _startIndex;
    private readonly int _length;

    public char this[int i] => this._chars[i];

    public char[] Chars => this._chars;

    public int StartIndex => this._startIndex;

    public int Length => this._length;

    public StringReference(char[] chars, int startIndex, int length)
    {
      this._chars = chars;
      this._startIndex = startIndex;
      this._length = length;
    }

    public override string ToString() => new string(this._chars, this._startIndex, this._length);
  }
}
