// Decompiled with JetBrains decompiler
// Type: Microsoft.Deployment.Compression.ArchiveException
// Assembly: Microsoft.Deployment.Compression, Version=3.0.0.0, Culture=neutral, PublicKeyToken=ce35f76fcda82bad
// MVID: 851118E5-1A5D-4354-8CEB-CAC13A1060E5
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\Microsoft.Deployment.Compression.dll

using System;
using System.IO;
using System.Runtime.Serialization;

namespace Microsoft.Deployment.Compression
{
  [Serializable]
  public class ArchiveException : IOException
  {
    public ArchiveException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    public ArchiveException(string message)
      : this(message, (Exception) null)
    {
    }

    public ArchiveException()
      : this((string) null, (Exception) null)
    {
    }

    protected ArchiveException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
