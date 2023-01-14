// Decompiled with JetBrains decompiler
// Type: Microsoft.Deployment.Compression.BasicUnpackStreamContext
// Assembly: Microsoft.Deployment.Compression, Version=3.0.0.0, Culture=neutral, PublicKeyToken=ce35f76fcda82bad
// MVID: 851118E5-1A5D-4354-8CEB-CAC13A1060E5
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\Microsoft.Deployment.Compression.dll

using System;
using System.IO;

namespace Microsoft.Deployment.Compression
{
  public class BasicUnpackStreamContext : IUnpackStreamContext
  {
    private Stream archiveStream;
    private Stream fileStream;

    public BasicUnpackStreamContext(Stream archiveStream) => this.archiveStream = archiveStream;

    public Stream FileStream => this.fileStream;

    public Stream OpenArchiveReadStream(
      int archiveNumber,
      string archiveName,
      CompressionEngine compressionEngine)
    {
      return (Stream) new DuplicateStream(this.archiveStream);
    }

    public void CloseArchiveReadStream(int archiveNumber, string archiveName, Stream stream)
    {
    }

    public Stream OpenFileWriteStream(string path, long fileSize, DateTime lastWriteTime)
    {
      this.fileStream = (Stream) new MemoryStream(new byte[fileSize], 0, checked ((int) fileSize), true, true);
      return this.fileStream;
    }

    public void CloseFileWriteStream(
      string path,
      Stream stream,
      FileAttributes attributes,
      DateTime lastWriteTime)
    {
    }
  }
}
