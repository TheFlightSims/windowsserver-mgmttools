// Decompiled with JetBrains decompiler
// Type: Microsoft.Deployment.Compression.IPackStreamContext
// Assembly: Microsoft.Deployment.Compression, Version=3.0.0.0, Culture=neutral, PublicKeyToken=ce35f76fcda82bad
// MVID: 851118E5-1A5D-4354-8CEB-CAC13A1060E5
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\Microsoft.Deployment.Compression.dll

using System;
using System.IO;

namespace Microsoft.Deployment.Compression
{
  public interface IPackStreamContext
  {
    string GetArchiveName(int archiveNumber);

    Stream OpenArchiveWriteStream(
      int archiveNumber,
      string archiveName,
      bool truncate,
      CompressionEngine compressionEngine);

    void CloseArchiveWriteStream(int archiveNumber, string archiveName, Stream stream);

    Stream OpenFileReadStream(
      string path,
      out FileAttributes attributes,
      out DateTime lastWriteTime);

    void CloseFileReadStream(string path, Stream stream);

    object GetOption(string optionName, object[] parameters);
  }
}
