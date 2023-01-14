// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.DefaultJsonNameTable
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\Newtonsoft.Json.dll

using System;


#nullable enable
namespace Newtonsoft.Json
{
  public class DefaultJsonNameTable : JsonNameTable
  {
    private static readonly int HashCodeRandomizer = Environment.TickCount;
    private int _count;
    private DefaultJsonNameTable.Entry[] _entries;
    private int _mask = 31;

    public DefaultJsonNameTable() => this._entries = new DefaultJsonNameTable.Entry[this._mask + 1];

    public override string? Get(char[] key, int start, int length)
    {
      if (length == 0)
        return string.Empty;
      int num1 = length + DefaultJsonNameTable.HashCodeRandomizer;
      int num2 = num1 + (num1 << 7 ^ (int) key[start]);
      int num3 = start + length;
      for (int index = start + 1; index < num3; ++index)
        num2 += num2 << 7 ^ (int) key[index];
      int num4 = num2 - (num2 >> 17);
      int num5 = num4 - (num4 >> 11);
      int num6 = num5 - (num5 >> 5);
      for (DefaultJsonNameTable.Entry entry = this._entries[num6 & this._mask]; entry != null; entry = entry.Next)
      {
        if (entry.HashCode == num6 && DefaultJsonNameTable.TextEquals(entry.Value, key, start, length))
          return entry.Value;
      }
      return (string) null;
    }

    public string Add(string key)
    {
      int num1 = key != null ? key.Length : throw new ArgumentNullException(nameof (key));
      if (num1 == 0)
        return string.Empty;
      int num2 = num1 + DefaultJsonNameTable.HashCodeRandomizer;
      for (int index = 0; index < key.Length; ++index)
        num2 += num2 << 7 ^ (int) key[index];
      int num3 = num2 - (num2 >> 17);
      int num4 = num3 - (num3 >> 11);
      int hashCode = num4 - (num4 >> 5);
      for (DefaultJsonNameTable.Entry entry = this._entries[hashCode & this._mask]; entry != null; entry = entry.Next)
      {
        if (entry.HashCode == hashCode && entry.Value.Equals(key, StringComparison.Ordinal))
          return entry.Value;
      }
      return this.AddEntry(key, hashCode);
    }

    private string AddEntry(string str, int hashCode)
    {
      int index = hashCode & this._mask;
      DefaultJsonNameTable.Entry entry = new DefaultJsonNameTable.Entry(str, hashCode, this._entries[index]);
      this._entries[index] = entry;
      if (this._count++ == this._mask)
        this.Grow();
      return entry.Value;
    }

    private void Grow()
    {
      DefaultJsonNameTable.Entry[] entries = this._entries;
      int num = this._mask * 2 + 1;
      DefaultJsonNameTable.Entry[] entryArray = new DefaultJsonNameTable.Entry[num + 1];
      DefaultJsonNameTable.Entry next;
      for (int index1 = 0; index1 < entries.Length; ++index1)
      {
        for (DefaultJsonNameTable.Entry entry = entries[index1]; entry != null; entry = next)
        {
          int index2 = entry.HashCode & num;
          next = entry.Next;
          entry.Next = entryArray[index2];
          entryArray[index2] = entry;
        }
      }
      this._entries = entryArray;
      this._mask = num;
    }

    private static bool TextEquals(string str1, char[] str2, int str2Start, int str2Length)
    {
      if (str1.Length != str2Length)
        return false;
      for (int index = 0; index < str1.Length; ++index)
      {
        if ((int) str1[index] != (int) str2[str2Start + index])
          return false;
      }
      return true;
    }

    private class Entry
    {
      internal readonly string Value;
      internal readonly int HashCode;
      internal DefaultJsonNameTable.Entry Next;

      internal Entry(string value, int hashCode, DefaultJsonNameTable.Entry next)
      {
        this.Value = value;
        this.HashCode = hashCode;
        this.Next = next;
      }
    }
  }
}
