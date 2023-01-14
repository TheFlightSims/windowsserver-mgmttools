// Decompiled with JetBrains decompiler
// Type: Microsoft.Deployment.Compression.Cab.CabInfo
// Assembly: Microsoft.Deployment.Compression.Cab, Version=3.0.0.0, Culture=neutral, PublicKeyToken=ce35f76fcda82bad
// MVID: FD30E6CD-702E-43F8-86E3-5A8B35FB59B3
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\Microsoft.Deployment.Compression.Cab.dll

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Microsoft.Deployment.Compression.Cab
{
  [Serializable]
  public class CabInfo : ArchiveInfo
  {
    public CabInfo(string path)
      : base(path)
    {
    }

    protected CabInfo(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }

    protected override CompressionEngine CreateCompressionEngine() => (CompressionEngine) new CabEngine();

    public IList<CabFileInfo> GetFiles()
    {
      IList<ArchiveFileInfo> files = base.GetFiles();
      List<CabFileInfo> cabFileInfoList = new List<CabFileInfo>(files.Count);
      foreach (CabFileInfo cabFileInfo in (IEnumerable<ArchiveFileInfo>) files)
        cabFileInfoList.Add(cabFileInfo);
      return (IList<CabFileInfo>) cabFileInfoList.AsReadOnly();
    }

    public IList<CabFileInfo> GetFiles(string searchPattern)
    {
      IList<ArchiveFileInfo> files = base.GetFiles(searchPattern);
      List<CabFileInfo> cabFileInfoList = new List<CabFileInfo>(files.Count);
      foreach (CabFileInfo cabFileInfo in (IEnumerable<ArchiveFileInfo>) files)
        cabFileInfoList.Add(cabFileInfo);
      return (IList<CabFileInfo>) cabFileInfoList.AsReadOnly();
    }

        public void Unpack(string destinationFolder, EventHandler<ArchiveProgressEventArgs> unpackArchiveProgression)
        {
            throw new NotImplementedException();
        }

        public void Pack(string v1, bool v2, CompressionLevel none, EventHandler<ArchiveProgressEventArgs> exportProgressionEventHandler)
        {
            throw new NotImplementedException();
        }
    }
}
