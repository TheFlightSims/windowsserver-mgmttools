// Decompiled with JetBrains decompiler
// Type: Microsoft.Deployment.Compression.ArchiveFileStreamContext
// Assembly: Microsoft.Deployment.Compression, Version=3.0.0.0, Culture=neutral, PublicKeyToken=ce35f76fcda82bad
// MVID: 851118E5-1A5D-4354-8CEB-CAC13A1060E5
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\Microsoft.Deployment.Compression.dll

using System;
using System.Collections.Generic;
using System.IO;

namespace Microsoft.Deployment.Compression
{
  public class ArchiveFileStreamContext : IPackStreamContext, IUnpackStreamContext
  {
    private IList<string> archiveFiles;
    private string directory;
    private IDictionary<string, string> files;
    private bool extractOnlyNewerFiles;
    private bool enableOffsetOpen;

    public ArchiveFileStreamContext(string archiveFile)
      : this(archiveFile, (string) null, (IDictionary<string, string>) null)
    {
    }

    public ArchiveFileStreamContext(
      string archiveFile,
      string directory,
      IDictionary<string, string> files)
      : this((IList<string>) new string[1]{ archiveFile }, directory, files)
    {
      if (archiveFile == null)
        throw new ArgumentNullException(nameof (archiveFile));
    }

    public ArchiveFileStreamContext(
      IList<string> archiveFiles,
      string directory,
      IDictionary<string, string> files)
    {
      this.archiveFiles = archiveFiles != null && archiveFiles.Count != 0 ? archiveFiles : throw new ArgumentNullException(nameof (archiveFiles));
      this.directory = directory;
      this.files = files;
    }

    public IList<string> ArchiveFiles => this.archiveFiles;

    public string Directory => this.directory;

    public IDictionary<string, string> Files => this.files;

    public bool ExtractOnlyNewerFiles
    {
      get => this.extractOnlyNewerFiles;
      set => this.extractOnlyNewerFiles = value;
    }

    public bool EnableOffsetOpen
    {
      get => this.enableOffsetOpen;
      set => this.enableOffsetOpen = value;
    }

    public virtual string GetArchiveName(int archiveNumber) => archiveNumber < this.archiveFiles.Count ? Path.GetFileName(this.archiveFiles[archiveNumber]) : string.Empty;

    public virtual Stream OpenArchiveWriteStream(
      int archiveNumber,
      string archiveName,
      bool truncate,
      CompressionEngine compressionEngine)
    {
      if (archiveNumber >= this.archiveFiles.Count)
        return (Stream) null;
      if (string.IsNullOrEmpty(archiveName))
        throw new ArgumentNullException(nameof (archiveName));
      Stream source = (Stream) File.Open(Path.Combine(Path.GetDirectoryName(this.archiveFiles[0]), archiveName), truncate ? FileMode.OpenOrCreate : FileMode.Open, FileAccess.ReadWrite);
      if (this.enableOffsetOpen)
      {
        long offset = compressionEngine.FindArchiveOffset((Stream) new DuplicateStream(source));
        if (offset < 0L)
          offset = source.Length;
        if (offset > 0L)
          source = (Stream) new OffsetStream(source, offset);
        source.Seek(0L, SeekOrigin.Begin);
      }
      if (truncate)
        source.SetLength(0L);
      return source;
    }

    public virtual void CloseArchiveWriteStream(
      int archiveNumber,
      string archiveName,
      Stream stream)
    {
      if (stream == null)
        return;
      stream.Close();
      if (!(stream is FileStream fileStream))
        return;
      string name = fileStream.Name;
      if (string.IsNullOrEmpty(archiveName) || !(archiveName != Path.GetFileName(name)))
        return;
      string str = Path.Combine(Path.GetDirectoryName(this.archiveFiles[0]), archiveName);
      if (File.Exists(str))
        File.Delete(str);
      File.Move(name, str);
    }

    public virtual Stream OpenFileReadStream(
      string path,
      out FileAttributes attributes,
      out DateTime lastWriteTime)
    {
      string path1 = this.TranslateFilePath(path);
      if (path1 == null)
      {
        attributes = FileAttributes.Normal;
        lastWriteTime = DateTime.Now;
        return (Stream) null;
      }
      attributes = File.GetAttributes(path1);
      lastWriteTime = File.GetLastWriteTime(path1);
      return (Stream) File.Open(path1, FileMode.Open, FileAccess.Read, FileShare.Read);
    }

    public virtual void CloseFileReadStream(string path, Stream stream) => stream?.Close();

    public virtual object GetOption(string optionName, object[] parameters) => (object) null;

    public virtual Stream OpenArchiveReadStream(
      int archiveNumber,
      string archiveName,
      CompressionEngine compressionEngine)
    {
      if (archiveNumber >= this.archiveFiles.Count)
        return (Stream) null;
      Stream source = (Stream) File.Open(this.archiveFiles[archiveNumber], FileMode.Open, FileAccess.Read, FileShare.Read);
      if (this.enableOffsetOpen)
      {
        long archiveOffset = compressionEngine.FindArchiveOffset((Stream) new DuplicateStream(source));
        if (archiveOffset > 0L)
          source = (Stream) new OffsetStream(source, archiveOffset);
        else
          source.Seek(0L, SeekOrigin.Begin);
      }
      return source;
    }

    public virtual void CloseArchiveReadStream(
      int archiveNumber,
      string archiveName,
      Stream stream)
    {
      stream?.Close();
    }

    public virtual Stream OpenFileWriteStream(string path, long fileSize, DateTime lastWriteTime)
    {
      string str = this.TranslateFilePath(path);
      if (str == null)
        return (Stream) null;
      FileInfo fileInfo = new FileInfo(str);
      if (fileInfo.Exists)
      {
        if (this.extractOnlyNewerFiles && lastWriteTime != DateTime.MinValue && fileInfo.LastWriteTime >= lastWriteTime)
          return (Stream) null;
        FileAttributes fileAttributes = FileAttributes.ReadOnly | FileAttributes.Hidden | FileAttributes.System;
        if ((fileInfo.Attributes & fileAttributes) != (FileAttributes) 0)
          fileInfo.Attributes &= ~fileAttributes;
      }
      if (!fileInfo.Directory.Exists)
        fileInfo.Directory.Create();
      return (Stream) File.Open(str, FileMode.Create, FileAccess.Write, FileShare.None);
    }

    public virtual void CloseFileWriteStream(
      string path,
      Stream stream,
      FileAttributes attributes,
      DateTime lastWriteTime)
    {
      stream?.Close();
      string fileName = this.TranslateFilePath(path);
      if (fileName == null)
        return;
      FileInfo fileInfo = new FileInfo(fileName);
      if (lastWriteTime != DateTime.MinValue)
      {
        try
        {
          fileInfo.LastWriteTime = lastWriteTime;
        }
        catch (ArgumentException ex)
        {
        }
        catch (IOException ex)
        {
        }
      }
      try
      {
        fileInfo.Attributes = attributes;
      }
      catch (IOException ex)
      {
      }
    }

    private string TranslateFilePath(string path)
    {
      string path2;
      if (this.files != null)
      {
        path2 = this.files[path];
      }
      else
      {
        this.ValidateArchivePath(path);
        path2 = path;
      }
      if (path2 != null && this.directory != null)
        path2 = Path.Combine(this.directory, path2);
      return path2;
    }

    private void ValidateArchivePath(string filePath)
    {
      string fullPath = Path.GetFullPath(string.IsNullOrEmpty(this.directory) ? Environment.CurrentDirectory : this.directory);
      if (!Path.GetFullPath(Path.Combine(fullPath, filePath)).StartsWith(fullPath, StringComparison.InvariantCultureIgnoreCase))
        throw new InvalidDataException("Archive cannot contain files with absolute or traversal paths.");
    }
  }
}
