// Decompiled with JetBrains decompiler
// Type: Microsoft.Deployment.Compression.Cab.CabWorker
// Assembly: Microsoft.Deployment.Compression.Cab, Version=3.0.0.0, Culture=neutral, PublicKeyToken=ce35f76fcda82bad
// MVID: FD30E6CD-702E-43F8-86E3-5A8B35FB59B3
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\Microsoft.Deployment.Compression.Cab.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace Microsoft.Deployment.Compression.Cab
{
  internal abstract class CabWorker : IDisposable
  {
    internal const string CabStreamName = "%%CAB%%";
    private CabEngine cabEngine;
    private HandleManager<Stream> streamHandles;
    private Stream cabStream;
    private Stream fileStream;
    private NativeMethods.ERF erf;
    private GCHandle erfHandle;
    private IDictionary<string, short> cabNumbers;
    private string nextCabinetName;
    private bool suppressProgressEvents;
    private byte[] buf;
    protected string currentFileName;
    protected int currentFileNumber;
    protected int totalFiles;
    protected long currentFileBytesProcessed;
    protected long currentFileTotalBytes;
    protected short currentFolderNumber;
    protected long currentFolderTotalBytes;
    protected string currentArchiveName;
    protected short currentArchiveNumber;
    protected short totalArchives;
    protected long currentArchiveBytesProcessed;
    protected long currentArchiveTotalBytes;
    protected long fileBytesProcessed;
    protected long totalFileBytes;

    protected CabWorker(CabEngine cabEngine)
    {
      this.cabEngine = cabEngine;
      this.streamHandles = new HandleManager<Stream>();
      this.erf = new NativeMethods.ERF();
      this.erfHandle = GCHandle.Alloc((object) this.erf, GCHandleType.Pinned);
      this.cabNumbers = (IDictionary<string, short>) new Dictionary<string, short>(1);
      this.buf = new byte[32768];
    }

    ~CabWorker() => this.Dispose(false);

    public CabEngine CabEngine => this.cabEngine;

    internal NativeMethods.ERF Erf => this.erf;

    internal GCHandle ErfHandle => this.erfHandle;

    internal HandleManager<Stream> StreamHandles => this.streamHandles;

    internal bool SuppressProgressEvents
    {
      get => this.suppressProgressEvents;
      set => this.suppressProgressEvents = value;
    }

    internal IDictionary<string, short> CabNumbers => this.cabNumbers;

    internal string NextCabinetName
    {
      get => this.nextCabinetName;
      set => this.nextCabinetName = value;
    }

    internal Stream CabStream
    {
      get => this.cabStream;
      set => this.cabStream = value;
    }

    internal Stream FileStream
    {
      get => this.fileStream;
      set => this.fileStream = value;
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    protected void ResetProgressData()
    {
      this.currentFileName = (string) null;
      this.currentFileNumber = 0;
      this.totalFiles = 0;
      this.currentFileBytesProcessed = 0L;
      this.currentFileTotalBytes = 0L;
      this.currentFolderNumber = (short) 0;
      this.currentFolderTotalBytes = 0L;
      this.currentArchiveName = (string) null;
      this.currentArchiveNumber = (short) 0;
      this.totalArchives = (short) 0;
      this.currentArchiveBytesProcessed = 0L;
      this.currentArchiveTotalBytes = 0L;
      this.fileBytesProcessed = 0L;
      this.totalFileBytes = 0L;
    }

    protected void OnProgress(ArchiveProgressType progressType)
    {
      if (this.suppressProgressEvents)
        return;
      this.CabEngine.ReportProgress(new ArchiveProgressEventArgs(progressType, this.currentFileName, this.currentFileNumber >= 0 ? this.currentFileNumber : 0, this.totalFiles, this.currentFileBytesProcessed, this.currentFileTotalBytes, this.currentArchiveName, (int) this.currentArchiveNumber, (int) this.totalArchives, this.currentArchiveBytesProcessed, this.currentArchiveTotalBytes, this.fileBytesProcessed, this.totalFileBytes));
    }

    internal IntPtr CabAllocMem(int byteCount) => Marshal.AllocHGlobal((IntPtr) byteCount);

    internal void CabFreeMem(IntPtr memPointer) => Marshal.FreeHGlobal(memPointer);

    internal int CabOpenStream(string path, int openFlags, int shareMode) => this.CabOpenStreamEx(path, openFlags, shareMode, out int _, IntPtr.Zero);

    internal virtual int CabOpenStreamEx(
      string path,
      int openFlags,
      int shareMode,
      out int err,
      IntPtr pv)
    {
      path = path.Trim();
      Stream cabStream = this.cabStream;
      this.cabStream = (Stream) new DuplicateStream(cabStream);
      int num = this.streamHandles.AllocHandle(cabStream);
      err = 0;
      return num;
    }

    internal int CabReadStream(int streamHandle, IntPtr memory, int cb) => this.CabReadStreamEx(streamHandle, memory, cb, out int _, IntPtr.Zero);

    internal virtual int CabReadStreamEx(
      int streamHandle,
      IntPtr memory,
      int cb,
      out int err,
      IntPtr pv)
    {
      Stream streamHandle1 = this.streamHandles[streamHandle];
      int length1 = cb;
      if (length1 > this.buf.Length)
        this.buf = new byte[length1];
      byte[] buf = this.buf;
      int count = length1;
      int length2 = streamHandle1.Read(buf, 0, count);
      Marshal.Copy(this.buf, 0, memory, length2);
      err = 0;
      return length2;
    }

    internal int CabWriteStream(int streamHandle, IntPtr memory, int cb) => this.CabWriteStreamEx(streamHandle, memory, cb, out int _, IntPtr.Zero);

    internal virtual int CabWriteStreamEx(
      int streamHandle,
      IntPtr memory,
      int cb,
      out int err,
      IntPtr pv)
    {
      Stream streamHandle1 = this.streamHandles[streamHandle];
      int length = cb;
      if (length > this.buf.Length)
        this.buf = new byte[length];
      Marshal.Copy(memory, this.buf, 0, length);
      byte[] buf = this.buf;
      int count = length;
      streamHandle1.Write(buf, 0, count);
      err = 0;
      return cb;
    }

    internal int CabCloseStream(int streamHandle) => this.CabCloseStreamEx(streamHandle, out int _, IntPtr.Zero);

    internal virtual int CabCloseStreamEx(int streamHandle, out int err, IntPtr pv)
    {
      this.streamHandles.FreeHandle(streamHandle);
      err = 0;
      return 0;
    }

    internal int CabSeekStream(int streamHandle, int offset, int seekOrigin) => this.CabSeekStreamEx(streamHandle, offset, seekOrigin, out int _, IntPtr.Zero);

    internal virtual int CabSeekStreamEx(
      int streamHandle,
      int offset,
      int seekOrigin,
      out int err,
      IntPtr pv)
    {
      offset = checked ((int) this.streamHandles[streamHandle].Seek((long) offset, (SeekOrigin) seekOrigin));
      err = 0;
      return offset;
    }

    protected virtual void Dispose(bool disposing)
    {
      if (disposing)
      {
        if (this.cabStream != null)
        {
          this.cabStream.Close();
          this.cabStream = (Stream) null;
        }
        if (this.fileStream != null)
        {
          this.fileStream.Close();
          this.fileStream = (Stream) null;
        }
      }
      if (!this.erfHandle.IsAllocated)
        return;
      this.erfHandle.Free();
    }

    protected void CheckError(bool extracting)
    {
      if (this.Erf.Error)
        throw new CabException(this.Erf.Oper, this.Erf.Type, CabException.GetErrorMessage(this.Erf.Oper, this.Erf.Type, extracting));
    }
  }
}
