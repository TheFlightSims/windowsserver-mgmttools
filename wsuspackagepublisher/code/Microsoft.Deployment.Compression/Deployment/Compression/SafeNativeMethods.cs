// Decompiled with JetBrains decompiler
// Type: Microsoft.Deployment.Compression.SafeNativeMethods
// Assembly: Microsoft.Deployment.Compression, Version=3.0.0.0, Culture=neutral, PublicKeyToken=ce35f76fcda82bad
// MVID: 851118E5-1A5D-4354-8CEB-CAC13A1060E5
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\Microsoft.Deployment.Compression.dll

using System.Runtime.InteropServices;
using System.Security;

namespace Microsoft.Deployment.Compression
{
  [SuppressUnmanagedCodeSecurity]
  internal static class SafeNativeMethods
  {
    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool DosDateTimeToFileTime(
      short wFatDate,
      short wFatTime,
      out long fileTime);

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool FileTimeToDosDateTime(
      ref long fileTime,
      out short wFatDate,
      out short wFatTime);
  }
}
