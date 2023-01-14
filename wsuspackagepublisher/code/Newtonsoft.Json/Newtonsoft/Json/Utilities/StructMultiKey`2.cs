// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Utilities.StructMultiKey`2
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\Newtonsoft.Json.dll

using System;


#nullable enable
namespace Newtonsoft.Json.Utilities
{
  internal readonly struct StructMultiKey<T1, T2> : IEquatable<StructMultiKey<T1, T2>>
  {
    public readonly T1 Value1;
    public readonly T2 Value2;

    public StructMultiKey(T1 v1, T2 v2)
    {
      this.Value1 = v1;
      this.Value2 = v2;
    }

    public override int GetHashCode()
    {
      T1 obj1 = this.Value1;
      ref T1 local1 = ref obj1;
      int num1;
      if ((object) default (T1) == null)
      {
        T1 obj2 = local1;
        ref T1 local2 = ref obj2;
        if ((object) obj2 == null)
        {
          num1 = 0;
          goto label_4;
        }
        else
          local1 = ref local2;
      }
      num1 = local1.GetHashCode();
label_4:
      T2 obj3 = this.Value2;
      ref T2 local3 = ref obj3;
      int num2;
      if ((object) default (T2) == null)
      {
        T2 obj4 = local3;
        ref T2 local4 = ref obj4;
        if ((object) obj4 == null)
        {
          num2 = 0;
          goto label_8;
        }
        else
          local3 = ref local4;
      }
      num2 = local3.GetHashCode();
label_8:
      return num1 ^ num2;
    }

    public override bool Equals(object obj) => obj is StructMultiKey<T1, T2> other && this.Equals(other);

    public bool Equals(StructMultiKey<T1, T2> other) => object.Equals((object) this.Value1, (object) other.Value1) && object.Equals((object) this.Value2, (object) other.Value2);
  }
}
