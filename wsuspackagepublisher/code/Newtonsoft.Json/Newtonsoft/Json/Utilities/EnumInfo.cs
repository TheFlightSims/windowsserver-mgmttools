// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Utilities.EnumInfo
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\Newtonsoft.Json.dll


#nullable enable
namespace Newtonsoft.Json.Utilities
{
  internal class EnumInfo
  {
    public readonly bool IsFlags;
    public readonly ulong[] Values;
    public readonly string[] Names;
    public readonly string[] ResolvedNames;

    public EnumInfo(bool isFlags, ulong[] values, string[] names, string[] resolvedNames)
    {
      this.IsFlags = isFlags;
      this.Values = values;
      this.Names = names;
      this.ResolvedNames = resolvedNames;
    }
  }
}
