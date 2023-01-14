// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.SnakeCaseNamingStrategy
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;


#nullable enable
namespace Newtonsoft.Json.Serialization
{
  public class SnakeCaseNamingStrategy : NamingStrategy
  {
    public SnakeCaseNamingStrategy(bool processDictionaryKeys, bool overrideSpecifiedNames)
    {
      this.ProcessDictionaryKeys = processDictionaryKeys;
      this.OverrideSpecifiedNames = overrideSpecifiedNames;
    }

    public SnakeCaseNamingStrategy(
      bool processDictionaryKeys,
      bool overrideSpecifiedNames,
      bool processExtensionDataNames)
      : this(processDictionaryKeys, overrideSpecifiedNames)
    {
      this.ProcessExtensionDataNames = processExtensionDataNames;
    }

    public SnakeCaseNamingStrategy()
    {
    }

    protected override string ResolvePropertyName(string name) => StringUtils.ToSnakeCase(name);
  }
}
