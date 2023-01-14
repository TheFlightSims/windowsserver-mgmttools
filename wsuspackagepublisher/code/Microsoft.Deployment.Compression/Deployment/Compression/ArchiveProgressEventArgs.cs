// Decompiled with JetBrains decompiler
// Type: Microsoft.Deployment.Compression.ArchiveProgressEventArgs
// Assembly: Microsoft.Deployment.Compression, Version=3.0.0.0, Culture=neutral, PublicKeyToken=ce35f76fcda82bad
// MVID: 851118E5-1A5D-4354-8CEB-CAC13A1060E5
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\Microsoft.Deployment.Compression.dll

using System;

namespace Microsoft.Deployment.Compression
{
  public class ArchiveProgressEventArgs : EventArgs
  {
    private ArchiveProgressType progressType;
    private string currentFileName;
    private int currentFileNumber;
    private int totalFiles;
    private long currentFileBytesProcessed;
    private long currentFileTotalBytes;
    private string currentArchiveName;
    private short currentArchiveNumber;
    private short totalArchives;
    private long currentArchiveBytesProcessed;
    private long currentArchiveTotalBytes;
    private long fileBytesProcessed;
    private long totalFileBytes;

    public ArchiveProgressEventArgs(
      ArchiveProgressType progressType,
      string currentFileName,
      int currentFileNumber,
      int totalFiles,
      long currentFileBytesProcessed,
      long currentFileTotalBytes,
      string currentArchiveName,
      int currentArchiveNumber,
      int totalArchives,
      long currentArchiveBytesProcessed,
      long currentArchiveTotalBytes,
      long fileBytesProcessed,
      long totalFileBytes)
    {
      this.progressType = progressType;
      this.currentFileName = currentFileName;
      this.currentFileNumber = currentFileNumber;
      this.totalFiles = totalFiles;
      this.currentFileBytesProcessed = currentFileBytesProcessed;
      this.currentFileTotalBytes = currentFileTotalBytes;
      this.currentArchiveName = currentArchiveName;
      this.currentArchiveNumber = checked ((short) currentArchiveNumber);
      this.totalArchives = checked ((short) totalArchives);
      this.currentArchiveBytesProcessed = currentArchiveBytesProcessed;
      this.currentArchiveTotalBytes = currentArchiveTotalBytes;
      this.fileBytesProcessed = fileBytesProcessed;
      this.totalFileBytes = totalFileBytes;
    }

    public ArchiveProgressType ProgressType => this.progressType;

    public string CurrentFileName => this.currentFileName;

    public int CurrentFileNumber => this.currentFileNumber;

    public int TotalFiles => this.totalFiles;

    public long CurrentFileBytesProcessed => this.currentFileBytesProcessed;

    public long CurrentFileTotalBytes => this.currentFileTotalBytes;

    public string CurrentArchiveName => this.currentArchiveName;

    public int CurrentArchiveNumber => (int) this.currentArchiveNumber;

    public int TotalArchives => (int) this.totalArchives;

    public long CurrentArchiveBytesProcessed => this.currentArchiveBytesProcessed;

    public long CurrentArchiveTotalBytes => this.currentArchiveTotalBytes;

    public long FileBytesProcessed => this.fileBytesProcessed;

    public long TotalFileBytes => this.totalFileBytes;
  }
}
