// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.ErrorContext
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\Newtonsoft.Json.dll

using System;


#nullable enable
namespace Newtonsoft.Json.Serialization
{
  public class ErrorContext
  {
    internal ErrorContext(object? originalObject, object? member, string path, Exception error)
    {
      this.OriginalObject = originalObject;
      this.Member = member;
      this.Error = error;
      this.Path = path;
    }

    internal bool Traced { get; set; }

    public Exception Error { get; }

    public object? OriginalObject { get; }

    public object? Member { get; }

    public string Path { get; }

    public bool Handled { get; set; }
  }
}
