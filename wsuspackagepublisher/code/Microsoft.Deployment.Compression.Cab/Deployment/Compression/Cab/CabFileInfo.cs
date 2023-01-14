// Decompiled with JetBrains decompiler
// Type: Microsoft.Deployment.Compression.Cab.CabFileInfo
// Assembly: Microsoft.Deployment.Compression.Cab, Version=3.0.0.0, Culture=neutral, PublicKeyToken=ce35f76fcda82bad
// MVID: FD30E6CD-702E-43F8-86E3-5A8B35FB59B3
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\Microsoft.Deployment.Compression.Cab.dll

using System;
using System.IO;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Microsoft.Deployment.Compression.Cab
{
  [Serializable]
  public class CabFileInfo : ArchiveFileInfo
  {
    private int cabFolder;

    public CabFileInfo(CabInfo cabinetInfo, string filePath)
      : base((ArchiveInfo) cabinetInfo, filePath)
    {
      if (cabinetInfo == null)
        throw new ArgumentNullException(nameof (cabinetInfo));
      this.cabFolder = -1;
    }

    internal CabFileInfo(
      string filePath,
      int cabFolder,
      int cabNumber,
      FileAttributes attributes,
      DateTime lastWriteTime,
      long length)
      : base(filePath, cabNumber, attributes, lastWriteTime, length)
    {
      this.cabFolder = cabFolder;
    }

    protected CabFileInfo(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
      this.cabFolder = info.GetInt32(nameof (cabFolder));
    }

    [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      base.GetObjectData(info, context);
      info.AddValue("cabFolder", this.cabFolder);
    }

    public CabInfo Cabinet => (CabInfo) this.Archive;

    public string CabinetName => this.ArchiveName;

    public int CabinetFolderNumber
    {
      get
      {
        if (this.cabFolder < 0)
          this.Refresh();
        return this.cabFolder;
      }
    }

    protected override void Refresh(ArchiveFileInfo newFileInfo)
    {
      base.Refresh(newFileInfo);
      this.cabFolder = ((CabFileInfo) newFileInfo).cabFolder;
    }
  }
}
