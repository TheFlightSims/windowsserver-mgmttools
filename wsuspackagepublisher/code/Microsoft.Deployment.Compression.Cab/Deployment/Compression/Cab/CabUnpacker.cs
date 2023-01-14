// Decompiled with JetBrains decompiler
// Type: Microsoft.Deployment.Compression.Cab.CabUnpacker
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
  internal class CabUnpacker : CabWorker
  {
    private NativeMethods.FDI.Handle fdiHandle;
    private NativeMethods.FDI.PFNALLOC fdiAllocMemHandler;
    private NativeMethods.FDI.PFNFREE fdiFreeMemHandler;
    private NativeMethods.FDI.PFNOPEN fdiOpenStreamHandler;
    private NativeMethods.FDI.PFNREAD fdiReadStreamHandler;
    private NativeMethods.FDI.PFNWRITE fdiWriteStreamHandler;
    private NativeMethods.FDI.PFNCLOSE fdiCloseStreamHandler;
    private NativeMethods.FDI.PFNSEEK fdiSeekStreamHandler;
    private IUnpackStreamContext context;
    private List<ArchiveFileInfo> fileList;
    private int folderId;
    private Predicate<string> filter;

    [SecurityPermission(SecurityAction.Assert, UnmanagedCode = true)]
    public CabUnpacker(CabEngine cabEngine)
      : base(cabEngine)
    {
      this.fdiAllocMemHandler = new NativeMethods.FDI.PFNALLOC(((CabWorker) this).CabAllocMem);
      this.fdiFreeMemHandler = new NativeMethods.FDI.PFNFREE(((CabWorker) this).CabFreeMem);
      this.fdiOpenStreamHandler = new NativeMethods.FDI.PFNOPEN(((CabWorker) this).CabOpenStream);
      this.fdiReadStreamHandler = new NativeMethods.FDI.PFNREAD(((CabWorker) this).CabReadStream);
      this.fdiWriteStreamHandler = new NativeMethods.FDI.PFNWRITE(((CabWorker) this).CabWriteStream);
      this.fdiCloseStreamHandler = new NativeMethods.FDI.PFNCLOSE(((CabWorker) this).CabCloseStream);
      this.fdiSeekStreamHandler = new NativeMethods.FDI.PFNSEEK(((CabWorker) this).CabSeekStream);
      this.fdiHandle = NativeMethods.FDI.Create(this.fdiAllocMemHandler, this.fdiFreeMemHandler, this.fdiOpenStreamHandler, this.fdiReadStreamHandler, this.fdiWriteStreamHandler, this.fdiCloseStreamHandler, this.fdiSeekStreamHandler, 1, this.ErfHandle.AddrOfPinnedObject());
      if (this.Erf.Error)
      {
        int oper = this.Erf.Oper;
        int type = this.Erf.Type;
        this.ErfHandle.Free();
        throw new CabException(oper, type, CabException.GetErrorMessage(oper, type, true));
      }
    }

    [SecurityPermission(SecurityAction.Assert, UnmanagedCode = true)]
    public bool IsArchive(Stream stream)
    {
      if (stream == null)
        throw new ArgumentNullException(nameof (stream));
      lock (this)
        return this.IsCabinet(stream, out short _, out int _, out int _);
    }

    [SecurityPermission(SecurityAction.Assert, UnmanagedCode = true)]
    public IList<ArchiveFileInfo> GetFileInfo(
      IUnpackStreamContext streamContext,
      Predicate<string> fileFilter)
    {
      if (streamContext == null)
        throw new ArgumentNullException(nameof (streamContext));
      lock (this)
      {
        this.context = streamContext;
        this.filter = fileFilter;
        this.NextCabinetName = string.Empty;
        this.fileList = new List<ArchiveFileInfo>();
        bool suppressProgressEvents = this.SuppressProgressEvents;
        this.SuppressProgressEvents = true;
        try
        {
          short num = 0;
          while (this.NextCabinetName != null)
          {
            this.Erf.Clear();
            this.CabNumbers[this.NextCabinetName] = num;
            NativeMethods.FDI.Copy(this.fdiHandle, this.NextCabinetName, string.Empty, 0, new NativeMethods.FDI.PFNNOTIFY(this.CabListNotify), IntPtr.Zero, IntPtr.Zero);
            this.CheckError(true);
            checked { ++num; }
          }
          List<ArchiveFileInfo> fileList = this.fileList;
          this.fileList = (List<ArchiveFileInfo>) null;
          return (IList<ArchiveFileInfo>) fileList.AsReadOnly();
        }
        finally
        {
          this.SuppressProgressEvents = suppressProgressEvents;
          if (this.CabStream != null)
          {
            this.context.CloseArchiveReadStream((int) this.currentArchiveNumber, this.currentArchiveName, this.CabStream);
            this.CabStream = (Stream) null;
          }
          this.context = (IUnpackStreamContext) null;
        }
      }
    }

    [SecurityPermission(SecurityAction.Assert, UnmanagedCode = true)]
    public void Unpack(IUnpackStreamContext streamContext, Predicate<string> fileFilter)
    {
      lock (this)
      {
        IList<ArchiveFileInfo> fileInfo = this.GetFileInfo(streamContext, fileFilter);
        this.ResetProgressData();
        if (fileInfo != null)
        {
          this.totalFiles = fileInfo.Count;
          int index = 0;
          while (index < fileInfo.Count)
          {
            checked { this.totalFileBytes += fileInfo[index].Length; }
            if (fileInfo[index].ArchiveNumber >= (int) this.totalArchives)
              this.totalArchives = checked ((short) (fileInfo[index].ArchiveNumber + 1));
            checked { ++index; }
          }
        }
        this.context = streamContext;
        this.fileList = (List<ArchiveFileInfo>) null;
        this.NextCabinetName = string.Empty;
        this.folderId = -1;
        this.currentFileNumber = -1;
        try
        {
          short num = 0;
          while (this.NextCabinetName != null)
          {
            this.Erf.Clear();
            this.CabNumbers[this.NextCabinetName] = num;
            NativeMethods.FDI.Copy(this.fdiHandle, this.NextCabinetName, string.Empty, 0, new NativeMethods.FDI.PFNNOTIFY(this.CabExtractNotify), IntPtr.Zero, IntPtr.Zero);
            this.CheckError(true);
            checked { ++num; }
          }
        }
        finally
        {
          if (this.CabStream != null)
          {
            this.context.CloseArchiveReadStream((int) this.currentArchiveNumber, this.currentArchiveName, this.CabStream);
            this.CabStream = (Stream) null;
          }
          if (this.FileStream != null)
          {
            this.context.CloseFileWriteStream(this.currentFileName, this.FileStream, FileAttributes.Normal, DateTime.Now);
            this.FileStream = (Stream) null;
          }
          this.context = (IUnpackStreamContext) null;
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
          Stream stream = this.context.OpenArchiveReadStream((int) cabNumber, path, (CompressionEngine) this.CabEngine);
          if (stream == null)
            throw new FileNotFoundException(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "Cabinet {0} not provided.", (object) cabNumber));
          this.currentArchiveName = path;
          this.currentArchiveNumber = cabNumber;
          if ((int) this.totalArchives <= (int) this.currentArchiveNumber)
            this.totalArchives = checked ((short) ((int) this.currentArchiveNumber + 1));
          this.currentArchiveTotalBytes = stream.Length;
          this.currentArchiveBytesProcessed = 0L;
          if (this.folderId != -3)
            this.OnProgress(ArchiveProgressType.StartArchive);
          this.CabStream = stream;
        }
        path = "%%CAB%%";
      }
      return base.CabOpenStreamEx(path, openFlags, shareMode, out err, pv);
    }

    internal override int CabReadStreamEx(
      int streamHandle,
      IntPtr memory,
      int cb,
      out int err,
      IntPtr pv)
    {
      int num = base.CabReadStreamEx(streamHandle, memory, cb, out err, pv);
      if (err != 0 || this.CabStream == null || this.fileList != null || DuplicateStream.OriginalStream(this.StreamHandles[streamHandle]) != DuplicateStream.OriginalStream(this.CabStream))
        return num;
      checked { this.currentArchiveBytesProcessed += (long) cb; }
      if (this.currentArchiveBytesProcessed <= this.currentArchiveTotalBytes)
        return num;
      this.currentArchiveBytesProcessed = this.currentArchiveTotalBytes;
      return num;
    }

    internal override int CabWriteStreamEx(
      int streamHandle,
      IntPtr memory,
      int cb,
      out int err,
      IntPtr pv)
    {
      int num = base.CabWriteStreamEx(streamHandle, memory, cb, out err, pv);
      if (num <= 0 || err != 0)
        return num;
      checked { this.currentFileBytesProcessed += (long) cb; }
      checked { this.fileBytesProcessed += (long) cb; }
      this.OnProgress(ArchiveProgressType.PartialFile);
      return num;
    }

    internal override int CabCloseStreamEx(int streamHandle, out int err, IntPtr pv)
    {
      Stream stream = DuplicateStream.OriginalStream(this.StreamHandles[streamHandle]);
      if (stream == DuplicateStream.OriginalStream(this.CabStream))
      {
        if (this.folderId != -3)
          this.OnProgress(ArchiveProgressType.FinishArchive);
        this.context.CloseArchiveReadStream((int) this.currentArchiveNumber, this.currentArchiveName, stream);
        this.currentArchiveName = this.NextCabinetName;
        this.currentArchiveBytesProcessed = this.currentArchiveTotalBytes = 0L;
        this.CabStream = (Stream) null;
      }
      return base.CabCloseStreamEx(streamHandle, out err, pv);
    }

    protected override void Dispose(bool disposing)
    {
      try
      {
        if (!disposing || this.fdiHandle == null)
          return;
        this.fdiHandle.Dispose();
        this.fdiHandle = (NativeMethods.FDI.Handle) null;
      }
      finally
      {
        base.Dispose(disposing);
      }
    }

    private static string GetFileName(NativeMethods.FDI.NOTIFICATION notification)
    {
      Encoding encoding = ((uint) notification.attribs & 128U) > 0U ? Encoding.UTF8 : Encoding.Default;
      int length = 0;
      while (Marshal.ReadByte(notification.psz1, length) != (byte) 0)
        checked { ++length; }
      byte[] numArray = new byte[length];
      Marshal.Copy(notification.psz1, numArray, 0, length);
      string path = encoding.GetString(numArray);
      if (Path.IsPathRooted(path))
        path = path.Replace(Path.VolumeSeparatorChar.ToString() ?? "", "");
      return path;
    }

    private bool IsCabinet(
      Stream cabStream,
      out short id,
      out int cabFolderCount,
      out int fileCount)
    {
      int num = this.StreamHandles.AllocHandle(cabStream);
      try
      {
        this.Erf.Clear();
        NativeMethods.FDI.CABINFO pfdici;
        bool flag = NativeMethods.FDI.IsCabinet(this.fdiHandle, num, out pfdici) != 0;
        if (this.Erf.Error)
        {
          if (this.Erf.Oper != 3)
            throw new CabException(this.Erf.Oper, this.Erf.Type, CabException.GetErrorMessage(this.Erf.Oper, this.Erf.Type, true));
          flag = false;
        }
        id = pfdici.setID;
        cabFolderCount = (int) pfdici.cFolders;
        fileCount = (int) pfdici.cFiles;
        return flag;
      }
      finally
      {
        this.StreamHandles.FreeHandle(num);
      }
    }

    private int CabListNotify(
      NativeMethods.FDI.NOTIFICATIONTYPE notificationType,
      NativeMethods.FDI.NOTIFICATION notification)
    {
      switch (notificationType)
      {
        case NativeMethods.FDI.NOTIFICATIONTYPE.CABINET_INFO:
          string stringAnsi = Marshal.PtrToStringAnsi(notification.psz1);
          this.NextCabinetName = stringAnsi.Length != 0 ? stringAnsi : (string) null;
          return 0;
        case NativeMethods.FDI.NOTIFICATIONTYPE.PARTIAL_FILE:
          return 0;
        case NativeMethods.FDI.NOTIFICATIONTYPE.COPY_FILE:
          string fileName = CabUnpacker.GetFileName(notification);
          if ((this.filter == null || this.filter(fileName)) && this.fileList != null)
          {
            FileAttributes attributes = (FileAttributes) ((int) notification.attribs & 39);
            if (attributes == (FileAttributes) 0)
              attributes = FileAttributes.Normal;
            DateTime dateTime;
            CompressionEngine.DosDateAndTimeToDateTime(notification.date, notification.time, out dateTime);
            long cb = (long) notification.cb;
            this.fileList.Add((ArchiveFileInfo) new CabFileInfo(fileName, (int) notification.iFolder, (int) notification.iCabinet, attributes, dateTime, cb));
            this.currentFileNumber = checked (this.fileList.Count - 1);
            checked { this.fileBytesProcessed += (long) notification.cb; }
          }
          checked { ++this.totalFiles; }
          checked { this.totalFileBytes += (long) notification.cb; }
          return 0;
        default:
          return 0;
      }
    }

    private int CabExtractNotify(
      NativeMethods.FDI.NOTIFICATIONTYPE notificationType,
      NativeMethods.FDI.NOTIFICATION notification)
    {
      switch (notificationType)
      {
        case NativeMethods.FDI.NOTIFICATIONTYPE.CABINET_INFO:
          if (this.NextCabinetName != null && this.NextCabinetName.StartsWith("?", StringComparison.Ordinal))
          {
            this.NextCabinetName = this.NextCabinetName.Substring(1);
          }
          else
          {
            string stringAnsi = Marshal.PtrToStringAnsi(notification.psz1);
            this.NextCabinetName = stringAnsi.Length != 0 ? stringAnsi : (string) null;
          }
          return 0;
        case NativeMethods.FDI.NOTIFICATIONTYPE.COPY_FILE:
          return this.CabExtractCopyFile(notification);
        case NativeMethods.FDI.NOTIFICATIONTYPE.CLOSE_FILE_INFO:
          return this.CabExtractCloseFile(notification);
        case NativeMethods.FDI.NOTIFICATIONTYPE.NEXT_CABINET:
          this.CabNumbers[Marshal.PtrToStringAnsi(notification.psz1)] = notification.iCabinet;
          this.NextCabinetName = "?" + this.NextCabinetName;
          return 0;
        default:
          return 0;
      }
    }

    private int CabExtractCopyFile(NativeMethods.FDI.NOTIFICATION notification)
    {
      if ((int) notification.iFolder != this.folderId)
      {
        if (notification.iFolder != (short) -3 && this.folderId != -1)
          checked { ++this.currentFolderNumber; }
        this.folderId = (int) notification.iFolder;
      }
      string fileName = CabUnpacker.GetFileName(notification);
      if (this.filter == null || this.filter(fileName))
      {
        checked { ++this.currentFileNumber; }
        this.currentFileName = fileName;
        this.currentFileBytesProcessed = 0L;
        this.currentFileTotalBytes = (long) notification.cb;
        this.OnProgress(ArchiveProgressType.StartFile);
        DateTime dateTime;
        CompressionEngine.DosDateAndTimeToDateTime(notification.date, notification.time, out dateTime);
        Stream stream = this.context.OpenFileWriteStream(fileName, (long) notification.cb, dateTime);
        if (stream != null)
        {
          this.FileStream = stream;
          return this.StreamHandles.AllocHandle(stream);
        }
        checked { this.fileBytesProcessed += (long) notification.cb; }
        this.OnProgress(ArchiveProgressType.FinishFile);
        this.currentFileName = (string) null;
      }
      return 0;
    }

    private int CabExtractCloseFile(NativeMethods.FDI.NOTIFICATION notification)
    {
      Stream streamHandle = this.StreamHandles[notification.hf];
      this.StreamHandles.FreeHandle(notification.hf);
      string fileName = CabUnpacker.GetFileName(notification);
      FileAttributes attributes = (FileAttributes) ((int) notification.attribs & 39);
      if (attributes == (FileAttributes) 0)
        attributes = FileAttributes.Normal;
      DateTime dateTime;
      CompressionEngine.DosDateAndTimeToDateTime(notification.date, notification.time, out dateTime);
      streamHandle.Flush();
      this.context.CloseFileWriteStream(fileName, streamHandle, attributes, dateTime);
      this.FileStream = (Stream) null;
      long num = checked (this.currentFileTotalBytes - this.currentFileBytesProcessed);
      checked { this.currentFileBytesProcessed += num; }
      checked { this.fileBytesProcessed += num; }
      this.OnProgress(ArchiveProgressType.FinishFile);
      this.currentFileName = (string) null;
      return 1;
    }
  }
}
