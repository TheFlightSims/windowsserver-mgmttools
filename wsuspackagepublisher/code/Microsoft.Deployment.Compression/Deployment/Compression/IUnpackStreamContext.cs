// Decompiled with JetBrains decompiler
// Type: Microsoft.Deployment.Compression.IUnpackStreamContext
// Assembly: Microsoft.Deployment.Compression, Version=3.0.0.0, Culture=neutral, PublicKeyToken=ce35f76fcda82bad
// MVID: 851118E5-1A5D-4354-8CEB-CAC13A1060E5
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\Microsoft.Deployment.Compression.dll

using System;
using System.IO;

namespace Microsoft.Deployment.Compression
{
  public interface IUnpackStreamContext
  {
    Stream OpenArchiveReadStream(
      int archiveNumber,
      string archiveName,
      CompressionEngine compressionEngine);

    void CloseArchiveReadStream(int archiveNumber, string archiveName, Stream stream);

    Stream OpenFileWriteStream(string path, long fileSize, DateTime lastWriteTime);

    void CloseFileWriteStream(
      string path,
      Stream stream,
      FileAttributes attributes,
      DateTime lastWriteTime);
  }
}
