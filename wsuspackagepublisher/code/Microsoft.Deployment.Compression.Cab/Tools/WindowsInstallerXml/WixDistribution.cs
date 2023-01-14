// Decompiled with JetBrains decompiler
// Type: Microsoft.Tools.WindowsInstallerXml.WixDistribution
// Assembly: Microsoft.Deployment.Compression.Cab, Version=3.0.0.0, Culture=neutral, PublicKeyToken=ce35f76fcda82bad
// MVID: FD30E6CD-702E-43F8-86E3-5A8B35FB59B3
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\Microsoft.Deployment.Compression.Cab.dll

using System;
using System.Diagnostics;
using System.Reflection;

namespace Microsoft.Tools.WindowsInstallerXml
{
  internal static class WixDistribution
  {
    public static string NewsUrl = "http://wixtoolset.org/news/";
    public static string ShortProduct = "WiX Toolset";
    public static string SupportUrl = "http://wixtoolset.org/";
    public static string TelemetryUrlFormat = "http://wixtoolset.org/telemetry/v{0}/?r={1}";
    public static string VSExtensionsLandingUrl = "http://wixtoolset.org/releases/";

    public static string ReplacePlaceholders(string original, Assembly assembly)
    {
      if (assembly != null)
      {
        FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
        original = original.Replace("[FileComments]", versionInfo.Comments);
        original = original.Replace("[FileCopyright]", versionInfo.LegalCopyright);
        original = original.Replace("[FileProductName]", versionInfo.ProductName);
        original = original.Replace("[FileVersion]", versionInfo.FileVersion);
        if (original.Contains("[FileVersionMajorMinor]"))
        {
          Version version = new Version(versionInfo.FileVersion);
          original = original.Replace("[FileVersionMajorMinor]", version.Major.ToString() + "." + (object) version.Minor);
        }
        AssemblyCompanyAttribute attribute1;
        if (WixDistribution.TryGetAttribute<AssemblyCompanyAttribute>(assembly, out attribute1))
          original = original.Replace("[AssemblyCompany]", attribute1.Company);
        AssemblyCopyrightAttribute attribute2;
        if (WixDistribution.TryGetAttribute<AssemblyCopyrightAttribute>(assembly, out attribute2))
          original = original.Replace("[AssemblyCopyright]", attribute2.Copyright);
        AssemblyDescriptionAttribute attribute3;
        if (WixDistribution.TryGetAttribute<AssemblyDescriptionAttribute>(assembly, out attribute3))
          original = original.Replace("[AssemblyDescription]", attribute3.Description);
        AssemblyProductAttribute attribute4;
        if (WixDistribution.TryGetAttribute<AssemblyProductAttribute>(assembly, out attribute4))
          original = original.Replace("[AssemblyProduct]", attribute4.Product);
        AssemblyTitleAttribute attribute5;
        if (WixDistribution.TryGetAttribute<AssemblyTitleAttribute>(assembly, out attribute5))
          original = original.Replace("[AssemblyTitle]", attribute5.Title);
      }
      original = original.Replace("[NewsUrl]", WixDistribution.NewsUrl);
      original = original.Replace("[ShortProduct]", WixDistribution.ShortProduct);
      original = original.Replace("[SupportUrl]", WixDistribution.SupportUrl);
      return original;
    }

    private static bool TryGetAttribute<T>(Assembly assembly, out T attribute) where T : Attribute
    {
      attribute = default (T);
      object[] customAttributes = assembly.GetCustomAttributes(typeof (T), false);
      if (customAttributes != null && customAttributes.Length != 0)
        attribute = customAttributes[0] as T;
      return (object) attribute != null;
    }
  }
}
