// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.PreserveReferencesHandling
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\Newtonsoft.Json.dll

using System;

namespace Newtonsoft.Json
{
  [Flags]
  public enum PreserveReferencesHandling
  {
    None = 0,
    Objects = 1,
    Arrays = 2,
    All = Arrays | Objects, // 0x00000003
  }
}
