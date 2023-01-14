// Decompiled with JetBrains decompiler
// Type: Microsoft.Deployment.Compression.Cab.CabPacker
// Assembly: Microsoft.Deployment.Compression.Cab, Version=3.0.0.0, Culture=neutral, PublicKeyToken=ce35f76fcda82bad
// MVID: FD30E6CD-702E-43F8-86E3-5A8B35FB59B3
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\Microsoft.Deployment.Compression.Cab.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;

namespace Microsoft.Deployment.Compression.Cab
{
  internal class CabPacker : CabWorker
  {
    private const string TempStreamName = "%%TEMP%%";
    private NativeMethods.FCI.Handle fciHandle;
    private NativeMethods.FCI.PFNALLOC fciAllocMemHandler;
    private NativeMethods.FCI.PFNFREE fciFreeMemHandler;
    private NativeMethods.FCI.PFNOPEN fciOpenStreamHandler;
    private NativeMethods.FCI.PFNREAD fciReadStreamHandler;
    private NativeMethods.FCI.PFNWRITE fciWriteStreamHandler;
    private NativeMethods.FCI.PFNCLOSE fciCloseStreamHandler;
    private NativeMethods.FCI.PFNSEEK fciSeekStreamHandler;
    private NativeMethods.FCI.PFNFILEPLACED fciFilePlacedHandler;
    private NativeMethods.FCI.PFNDELETE fciDeleteFileHandler;
    private NativeMethods.FCI.PFNGETTEMPFILE fciGetTempFileHandler;
    private NativeMethods.FCI.PFNGETNEXTCABINET fciGetNextCabinet;
    private NativeMethods.FCI.PFNSTATUS fciCreateStatus;
    private NativeMethods.FCI.PFNGETOPENINFO fciGetOpenInfo;
    private IPackStreamContext context;
    private FileAttributes fileAttributes;
    private DateTime fileLastWriteTime;
    private int maxCabBytes;
    private long totalFolderBytesProcessedInCurrentCab;
    private CompressionLevel compressionLevel;
    private bool dontUseTempFiles;
    private IList<Stream> tempStreams;

    public CabPacker(CabEngine cabEngine)
      : base(cabEngine)
    {
      this.fciAllocMemHandler = new NativeMethods.FCI.PFNALLOC(((CabWorker) this).CabAllocMem);
      this.fciFreeMemHandler = new NativeMethods.FCI.PFNFREE(((CabWorker) this).CabFreeMem);
      this.fciOpenStreamHandler = new NativeMethods.FCI.PFNOPEN(((CabWorker) this).CabOpenStreamEx);
      this.fciReadStreamHandler = new NativeMethods.FCI.PFNREAD(((CabWorker) this).CabReadStreamEx);
      this.fciWriteStreamHandler = new NativeMethods.FCI.PFNWRITE(((CabWorker) this).CabWriteStreamEx);
      this.fciCloseStreamHandler = new NativeMethods.FCI.PFNCLOSE(((CabWorker) this).CabCloseStreamEx);
      this.fciSeekStreamHandler = new NativeMethods.FCI.PFNSEEK(((CabWorker) this).CabSeekStreamEx);
      this.fciFilePlacedHandler = new NativeMethods.FCI.PFNFILEPLACED(this.CabFilePlaced);
      this.fciDeleteFileHandler = new NativeMethods.FCI.PFNDELETE(this.CabDeleteFile);
      this.fciGetTempFileHandler = new NativeMethods.FCI.PFNGETTEMPFILE(this.CabGetTempFile);
      this.fciGetNextCabinet = new NativeMethods.FCI.PFNGETNEXTCABINET(this.CabGetNextCabinet);
      this.fciCreateStatus = new NativeMethods.FCI.PFNSTATUS(this.CabCreateStatus);
      this.fciGetOpenInfo = new NativeMethods.FCI.PFNGETOPENINFO(this.CabGetOpenInfo);
      this.tempStreams = (IList<Stream>) new List<Stream>();
      this.compressionLevel = CompressionLevel.Normal;
    }

    public bool UseTempFiles
    {
      get => !this.dontUseTempFiles;
      set => this.dontUseTempFiles = !value;
    }

    public CompressionLevel CompressionLevel
    {
      get => this.compressionLevel;
      set => this.compressionLevel = value;
    }

    private void CreateFci(long maxArchiveSize)
    {
      NativeMethods.FCI.CCAB pccab = new NativeMethods.FCI.CCAB();
      if (maxArchiveSize > 0L && maxArchiveSize < (long) pccab.cb)
        pccab.cb = Math.Max(32768, checked ((int) maxArchiveSize));
      object option = this.context.GetOption("maxFolderSize", (object[]) null);
      if (option != null)
      {
        long int64 = Convert.ToInt64(option, (IFormatProvider) CultureInfo.InvariantCulture);
        if (int64 > 0L && int64 < (long) pccab.cbFolderThresh)
          pccab.cbFolderThresh = checked ((int) int64);
      }
      this.maxCabBytes = pccab.cb;
      pccab.szCab = this.context.GetArchiveName(0);
      if (pccab.szCab == null)
        throw new FileNotFoundException("Cabinet name not provided by stream context.");
      pccab.setID = checked ((short) new Random().Next((int) short.MinValue, 32768));
      this.CabNumbers[pccab.szCab] = (short) 0;
      this.currentArchiveName = pccab.szCab;
      this.totalArchives = (short) 1;
      this.CabStream = (Stream) null;
      this.Erf.Clear();
      this.fciHandle = NativeMethods.FCI.Create(this.ErfHandle.AddrOfPinnedObject(), this.fciFilePlacedHandler, this.fciAllocMemHandler, this.fciFreeMemHandler, this.fciOpenStreamHandler, this.fciReadStreamHandler, this.fciWriteStreamHandler, this.fciCloseStreamHandler, this.fciSeekStreamHandler, this.fciDeleteFileHandler, this.fciGetTempFileHandler, pccab, IntPtr.Zero);
      this.CheckError(false);
    }

    [SecurityPermission(SecurityAction.Assert, UnmanagedCode = true)]
    public void Pack(
      IPackStreamContext streamContext,
      IEnumerable<string> files,
      long maxArchiveSize)
    {
      if (streamContext == null)
        throw new ArgumentNullException(nameof (streamContext));
      if (files == null)
        throw new ArgumentNullException(nameof (files));
      lock (this)
      {
        try
        {
          this.context = streamContext;
          this.ResetProgressData();
          this.CreateFci(maxArchiveSize);
          foreach (string file in files)
          {
            Stream stream = this.context.OpenFileReadStream(file, out FileAttributes _, out DateTime _);
            if (stream != null)
            {
              checked { this.totalFileBytes += stream.Length; }
              checked { ++this.totalFiles; }
              this.context.CloseFileReadStream(file, stream);
            }
          }
          long num = 0;
          this.currentFileNumber = -1;
          foreach (string file in files)
          {
            FileAttributes attributes;
            DateTime lastWriteTime;
            Stream stream = this.context.OpenFileReadStream(file, out attributes, out lastWriteTime);
            if (stream != null)
            {
              if (stream.Length >= 2147450880L)
                throw new NotSupportedException(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "File {0} exceeds maximum file size for cabinet format.", (object) file));
              if (num > 0L)
              {
                bool flag = checked (num + stream.Length) >= 2147450880L;
                if (!flag)
                  flag = Convert.ToBoolean(streamContext.GetOption("nextFolder", new object[2]
                  {
                    (object) file,
                    (object) this.currentFolderNumber
                  }), (IFormatProvider) CultureInfo.InvariantCulture);
                if (flag)
                {
                  this.FlushFolder();
                  num = 0L;
                }
              }
              if (this.currentFolderTotalBytes > 0L)
              {
                this.currentFolderTotalBytes = 0L;
                checked { ++this.currentFolderNumber; }
                num = 0L;
              }
              this.currentFileName = file;
              checked { ++this.currentFileNumber; }
              this.currentFileTotalBytes = stream.Length;
              this.currentFileBytesProcessed = 0L;
              this.OnProgress(ArchiveProgressType.StartFile);
              checked { num += stream.Length; }
              this.AddFile(file, stream, attributes, lastWriteTime, false, this.CompressionLevel);
            }
          }
          this.FlushFolder();
          this.FlushCabinet();
        }
        finally
        {
          if (this.CabStream != null)
          {
            this.context.CloseArchiveWriteStream((int) this.currentArchiveNumber, this.currentArchiveName, this.CabStream);
            this.CabStream = (Stream) null;
          }
          if (this.FileStream != null)
          {
            this.context.CloseFileReadStream(this.currentFileName, this.FileStream);
            this.FileStream = (Stream) null;
          }
          this.context = (IPackStreamContext) null;
          if (this.fciHandle != null)
          {
            this.fciHandle.Dispose();
            this.fciHandle = (NativeMethods.FCI.Handle) null;
          }
        }
      }
    }

    internal override int CabOpenStreamEx(
      string path,
      int openFlags,
      int shareMode,
      out int err,
      IntPtr pv)
    {
      if (this.CabNumbers.ContainsKey(path))
      {
        if (this.CabStream == null)
        {
          short cabNumber = this.CabNumbers[path];
          this.currentFolderTotalBytes = 0L;
          Stream stream = this.context.OpenArchiveWriteStream((int) cabNumber, path, true, (CompressionEngine) this.CabEngine);
          if (stream == null)
            throw new FileNotFoundException(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "Cabinet {0} not provided.", (object) cabNumber));
          this.currentArchiveName = path;
          this.currentArchiveTotalBytes = Math.Min(this.totalFolderBytesProcessedInCurrentCab, (long) this.maxCabBytes);
          this.currentArchiveBytesProcessed = 0L;
          this.OnProgress(ArchiveProgressType.StartArchive);
          this.CabStream = stream;
        }
        path = "%%CAB%%";
      }
      else
      {
        if (path == "%%TEMP%%")
        {
          Stream stream = (Stream) new MemoryStream();
          this.tempStreams.Add(stream);
          int num = this.StreamHandles.AllocHandle(stream);
          err = 0;
          return num;
        }
        if (path != "%%CAB%%")
        {
          path = Path.Combine(Path.GetTempPath(), path);
          Stream source = (Stream) new System.IO.FileStream(path, FileMode.Open, FileAccess.ReadWrite);
          this.tempStreams.Add(source);
          int num = this.StreamHandles.AllocHandle((Stream) new DuplicateStream(source));
          err = 0;
          return num;
        }
      }
      return base.CabOpenStreamEx(path, openFlags, shareMode, out err, pv);
    }

    internal override int CabWriteStreamEx(
      int streamHandle,
      IntPtr memory,
      int cb,
      out int err,
      IntPtr pv)
    {
      int num = base.CabWriteStreamEx(streamHandle, memory, cb, out err, pv);
      if (num <= 0 || err != 0 || DuplicateStream.OriginalStream(this.StreamHandles[streamHandle]) != DuplicateStream.OriginalStream(this.CabStream))
        return num;
      checked { this.currentArchiveBytesProcessed += (long) cb; }
      if (this.currentArchiveBytesProcessed <= this.currentArchiveTotalBytes)
        return num;
      this.currentArchiveBytesProcessed = this.currentArchiveTotalBytes;
      return num;
    }

    internal override int CabCloseStreamEx(int streamHandle, out int err, IntPtr pv)
    {
      Stream stream = DuplicateStream.OriginalStream(this.StreamHandles[streamHandle]);
      if (stream == DuplicateStream.OriginalStream(this.FileStream))
      {
        this.context.CloseFileReadStream(this.currentFileName, stream);
        this.FileStream = (Stream) null;
        long num = checked (this.currentFileTotalBytes - this.currentFileBytesProcessed);
        checked { this.currentFileBytesProcessed += num; }
        checked { this.fileBytesProcessed += num; }
        this.OnProgress(ArchiveProgressType.FinishFile);
        this.currentFileTotalBytes = 0L;
        this.currentFileBytesProcessed = 0L;
        this.currentFileName = (string) null;
      }
      else if (stream == DuplicateStream.OriginalStream(this.CabStream))
      {
        if (stream.CanWrite)
          stream.Flush();
        this.currentArchiveBytesProcessed = this.currentArchiveTotalBytes;
        this.OnProgress(ArchiveProgressType.FinishArchive);
        checked { ++this.currentArchiveNumber; }
        checked { ++this.totalArchives; }
        this.context.CloseArchiveWriteStream((int) this.currentArchiveNumber, this.currentArchiveName, stream);
        this.currentArchiveName = this.NextCabinetName;
        this.currentArchiveBytesProcessed = this.currentArchiveTotalBytes = 0L;
        this.totalFolderBytesProcessedInCurrentCab = 0L;
        this.CabStream = (Stream) null;
      }
      else
      {
        stream.Close();
        this.tempStreams.Remove(stream);
      }
      return base.CabCloseStreamEx(streamHandle, out err, pv);
    }

    protected override void Dispose(bool disposing)
    {
      try
      {
        if (!disposing || this.fciHandle == null)
          return;
        this.fciHandle.Dispose();
        this.fciHandle = (NativeMethods.FCI.Handle) null;
      }
      finally
      {
        base.Dispose(disposing);
      }
    }

    private static NativeMethods.FCI.TCOMP GetCompressionType(CompressionLevel compLevel)
    {
      if (compLevel < CompressionLevel.Min)
        return NativeMethods.FCI.TCOMP.TYPE_NONE;
      if (compLevel > CompressionLevel.Max)
        compLevel = CompressionLevel.Max;
      return (NativeMethods.FCI.TCOMP) checked ((ushort) (3 | 3840 + (unchecked (checked (6 * (int) unchecked (compLevel - 1)) / 9) << 8)));
    }

    private void AddFile(
      string name,
      Stream stream,
      FileAttributes attributes,
      DateTime lastWriteTime,
      bool execute,
      CompressionLevel compLevel)
    {
      this.FileStream = stream;
      this.fileAttributes = attributes & (FileAttributes.ReadOnly | FileAttributes.Hidden | FileAttributes.System | FileAttributes.Archive);
      this.fileLastWriteTime = lastWriteTime;
      this.currentFileName = name;
      NativeMethods.FCI.TCOMP compressionType = CabPacker.GetCompressionType(compLevel);
      IntPtr num = IntPtr.Zero;
      try
      {
        Encoding encoding = Encoding.ASCII;
        if (Encoding.UTF8.GetByteCount(name) > name.Length)
        {
          encoding = Encoding.UTF8;
          this.fileAttributes |= FileAttributes.Normal;
        }
        byte[] bytes = encoding.GetBytes(name);
        num = Marshal.AllocHGlobal(checked (bytes.Length + 1));
        Marshal.Copy(bytes, 0, num, bytes.Length);
        Marshal.WriteByte(num, bytes.Length, (byte) 0);
        this.Erf.Clear();
        NativeMethods.FCI.AddFile(this.fciHandle, string.Empty, num, execute, this.fciGetNextCabinet, this.fciCreateStatus, this.fciGetOpenInfo, compressionType);
      }
      finally
      {
        if (num != IntPtr.Zero)
          Marshal.FreeHGlobal(num);
      }
      this.CheckError(false);
      this.FileStream = (Stream) null;
      this.currentFileName = (string) null;
    }

    private void FlushFolder()
    {
      this.Erf.Clear();
      NativeMethods.FCI.FlushFolder(this.fciHandle, this.fciGetNextCabinet, this.fciCreateStatus);
      this.CheckError(false);
    }

    private void FlushCabinet()
    {
      this.Erf.Clear();
      NativeMethods.FCI.FlushCabinet(this.fciHandle, false, this.fciGetNextCabinet, this.fciCreateStatus);
      this.CheckError(false);
    }

    private int CabGetOpenInfo(
      string path,
      out short date,
      out short time,
      out short attribs,
      out int err,
      IntPtr pv)
    {
      CompressionEngine.DateTimeToDosDateAndTime(this.fileLastWriteTime, out date, out time);
      attribs = checked ((short) this.fileAttributes);
      Stream fileStream = this.FileStream;
      this.FileStream = (Stream) new DuplicateStream(fileStream);
      int openInfo = this.StreamHandles.AllocHandle(fileStream);
      err = 0;
      return openInfo;
    }

    private int CabFilePlaced(
      IntPtr pccab,
      string filePath,
      long fileSize,
      int continuation,
      IntPtr pv)
    {
      return 0;
    }

    private int CabGetNextCabinet(IntPtr pccab, uint prevCabSize, IntPtr pv)
    {
      NativeMethods.FCI.CCAB structure = new NativeMethods.FCI.CCAB();
      Marshal.PtrToStructure(pccab, (object) structure);
      structure.szDisk = string.Empty;
      structure.szCab = this.context.GetArchiveName(structure.iCab);
      this.CabNumbers[structure.szCab] = checked ((short) structure.iCab);
      this.NextCabinetName = structure.szCab;
      Marshal.StructureToPtr((object) structure, pccab, false);
      return 1;
    }

    private int CabCreateStatus(
      NativeMethods.FCI.STATUS typeStatus,
      uint cb1,
      uint cb2,
      IntPtr pv)
    {
      switch (typeStatus)
      {
        case NativeMethods.FCI.STATUS.FILE:
          if (cb2 > 0U && this.currentFileBytesProcessed < this.currentFileTotalBytes)
          {
            if (checked (this.currentFileBytesProcessed + (long) cb2) > this.currentFileTotalBytes)
              cb2 = checked ((uint) this.currentFileTotalBytes - (uint) this.currentFileBytesProcessed);
            checked { this.currentFileBytesProcessed += (long) cb2; }
            checked { this.fileBytesProcessed += (long) cb2; }
            this.OnProgress(ArchiveProgressType.PartialFile);
            break;
          }
          break;
        case NativeMethods.FCI.STATUS.FOLDER:
          if (cb1 == 0U)
          {
            this.currentFolderTotalBytes = checked ((long) cb2 - this.totalFolderBytesProcessedInCurrentCab);
            this.totalFolderBytesProcessedInCurrentCab = (long) cb2;
            break;
          }
          if (this.currentFolderTotalBytes == 0L)
          {
            this.OnProgress(ArchiveProgressType.PartialArchive);
            break;
          }
          break;
      }
      return 0;
    }

    private int CabGetTempFile(IntPtr tempNamePtr, int tempNameSize, IntPtr pv)
    {
      byte[] bytes = Encoding.ASCII.GetBytes(!this.UseTempFiles ? "%%TEMP%%" : Path.GetFileName(Path.GetTempFileName()));
      if (bytes.Length >= tempNameSize)
        return -1;
      Marshal.Copy(bytes, 0, tempNamePtr, bytes.Length);
      Marshal.WriteByte(tempNamePtr, bytes.Length, (byte) 0);
      return 1;
    }

    private int CabDeleteFile(string path, out int err, IntPtr pv)
    {
      try
      {
        if (path != "%%TEMP%%")
        {
          path = Path.Combine(Path.GetTempPath(), path);
          File.Delete(path);
        }
      }
      catch (IOException ex)
      {
      }
      err = 0;
      return 1;
    }
  }
}
