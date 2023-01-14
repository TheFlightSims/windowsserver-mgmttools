using System;
using System.IO;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Microsoft.Deployment.Compression
{
  [Serializable]
  public abstract class ArchiveFileInfo : FileSystemInfo
  {
    private ArchiveInfo archiveInfo;
    private string name;
    private string path;
    private bool initialized;
    private bool exists;
    private int archiveNumber;
    private FileAttributes attributes;
    private DateTime lastWriteTime;
    private long length;

    protected ArchiveFileInfo(ArchiveInfo archiveInfo, string filePath)
    {
      if (filePath == null)
        throw new ArgumentNullException(nameof (filePath));
      this.Archive = archiveInfo;
      this.name = System.IO.Path.GetFileName(filePath);
      this.path = System.IO.Path.GetDirectoryName(filePath);
      this.attributes = FileAttributes.Normal;
      this.lastWriteTime = DateTime.MinValue;
    }

    protected ArchiveFileInfo(
      string filePath,
      int archiveNumber,
      FileAttributes attributes,
      DateTime lastWriteTime,
      long length)
      : this((ArchiveInfo) null, filePath)
    {
      this.exists = true;
      this.archiveNumber = archiveNumber;
      this.attributes = attributes;
      this.lastWriteTime = lastWriteTime;
      this.length = length;
      this.initialized = true;
    }

    protected ArchiveFileInfo(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
      this.archiveInfo = (ArchiveInfo) info.GetValue(nameof (archiveInfo), typeof (ArchiveInfo));
      this.name = info.GetString(nameof (name));
      this.path = info.GetString(nameof (path));
      this.initialized = info.GetBoolean(nameof (initialized));
      this.exists = info.GetBoolean(nameof (exists));
      this.archiveNumber = info.GetInt32(nameof (archiveNumber));
      this.attributes = (FileAttributes) info.GetValue(nameof (attributes), typeof (FileAttributes));
      this.lastWriteTime = info.GetDateTime(nameof (lastWriteTime));
      this.length = info.GetInt64(nameof (length));
    }

    public override string Name => this.name;

    public string Path => this.path;

    public override string FullName
    {
      get
      {
        string path2 = System.IO.Path.Combine(this.Path, this.Name);
        if (this.Archive != null)
          path2 = System.IO.Path.Combine(this.ArchiveName, path2);
        return path2;
      }
    }

    public ArchiveInfo Archive
    {
      get => this.archiveInfo;
      internal set
      {
        this.archiveInfo = value;
        this.OriginalPath = value?.FullName;
        this.FullPath = this.OriginalPath;
      }
    }

    public string ArchiveName => this.Archive == null ? (string) null : this.Archive.FullName;

    public int ArchiveNumber => this.archiveNumber;

    public override bool Exists
    {
      get
      {
        if (!this.initialized)
          this.Refresh();
        return this.exists;
      }
    }

    public long Length
    {
      get
      {
        if (!this.initialized)
          this.Refresh();
        return this.length;
      }
    }

    public new FileAttributes Attributes
    {
      get
      {
        if (!this.initialized)
          this.Refresh();
        return this.attributes;
      }
    }

    public new DateTime LastWriteTime
    {
      get
      {
        if (!this.initialized)
          this.Refresh();
        return this.lastWriteTime;
      }
    }

    [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      base.GetObjectData(info, context);
      info.AddValue("archiveInfo", (object) this.archiveInfo);
      info.AddValue("name", (object) this.name);
      info.AddValue("path", (object) this.path);
      info.AddValue("initialized", this.initialized);
      info.AddValue("exists", this.exists);
      info.AddValue("archiveNumber", this.archiveNumber);
      info.AddValue("attributes", (object) this.attributes);
      info.AddValue("lastWriteTime", this.lastWriteTime);
      info.AddValue("length", this.length);
    }

    public override string ToString() => this.FullName;

    public override void Delete() => throw new NotSupportedException();

    public new void Refresh()
    {
      base.Refresh();
      if (this.Archive == null)
        return;
      string str = System.IO.Path.Combine(this.Path, this.Name);
      this.Refresh(this.Archive.GetFile(str) ?? throw new FileNotFoundException("File not found in archive.", str));
    }

    public void CopyTo(string destFileName) => this.CopyTo(destFileName, false);

    public void CopyTo(string destFileName, bool overwrite)
    {
      if (destFileName == null)
        throw new ArgumentNullException(nameof (destFileName));
      if (!overwrite && File.Exists(destFileName))
        throw new IOException();
      if (this.Archive == null)
        throw new InvalidOperationException();
      this.Archive.UnpackFile(System.IO.Path.Combine(this.Path, this.Name), destFileName);
    }

    public Stream OpenRead() => this.Archive.OpenRead(System.IO.Path.Combine(this.Path, this.Name));

    public StreamReader OpenText() => this.Archive.OpenText(System.IO.Path.Combine(this.Path, this.Name));

    protected virtual void Refresh(ArchiveFileInfo newFileInfo)
    {
      this.exists = newFileInfo.exists;
      this.length = newFileInfo.length;
      this.attributes = newFileInfo.attributes;
      this.lastWriteTime = newFileInfo.lastWriteTime;
    }
  }
}
