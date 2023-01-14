// Decompiled with JetBrains decompiler
// Type: Microsoft.Deployment.Compression.Cab.NativeMethods
// Assembly: Microsoft.Deployment.Compression.Cab, Version=3.0.0.0, Culture=neutral, PublicKeyToken=ce35f76fcda82bad
// MVID: FD30E6CD-702E-43F8-86E3-5A8B35FB59B3
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\Microsoft.Deployment.Compression.Cab.dll

using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

namespace Microsoft.Deployment.Compression.Cab
{
  internal static class NativeMethods
  {
    internal static class FCI
    {
      internal const int MIN_DISK = 32768;
      internal const int MAX_DISK = 2147483647;
      internal const int MAX_FOLDER = 2147450880;
      internal const int MAX_FILENAME = 256;
      internal const int MAX_CABINET_NAME = 256;
      internal const int MAX_CAB_PATH = 256;
      internal const int MAX_DISK_NAME = 256;
      internal const int CPU_80386 = 1;

      [DllImport("cabinet.dll", EntryPoint = "FCICreate", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, ThrowOnUnmappableChar = true, BestFitMapping = false)]
      internal static extern NativeMethods.FCI.Handle Create(
        IntPtr perf,
        NativeMethods.FCI.PFNFILEPLACED pfnfcifp,
        NativeMethods.FCI.PFNALLOC pfna,
        NativeMethods.FCI.PFNFREE pfnf,
        NativeMethods.FCI.PFNOPEN pfnopen,
        NativeMethods.FCI.PFNREAD pfnread,
        NativeMethods.FCI.PFNWRITE pfnwrite,
        NativeMethods.FCI.PFNCLOSE pfnclose,
        NativeMethods.FCI.PFNSEEK pfnseek,
        NativeMethods.FCI.PFNDELETE pfndelete,
        NativeMethods.FCI.PFNGETTEMPFILE pfnfcigtf,
        [MarshalAs(UnmanagedType.LPStruct)] NativeMethods.FCI.CCAB pccab,
        IntPtr pv);

      [DllImport("cabinet.dll", EntryPoint = "FCIAddFile", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, ThrowOnUnmappableChar = true, BestFitMapping = false)]
      internal static extern int AddFile(
        NativeMethods.FCI.Handle hfci,
        string pszSourceFile,
        IntPtr pszFileName,
        [MarshalAs(UnmanagedType.Bool)] bool fExecute,
        NativeMethods.FCI.PFNGETNEXTCABINET pfnfcignc,
        NativeMethods.FCI.PFNSTATUS pfnfcis,
        NativeMethods.FCI.PFNGETOPENINFO pfnfcigoi,
        NativeMethods.FCI.TCOMP typeCompress);

      [DllImport("cabinet.dll", EntryPoint = "FCIFlushCabinet", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, ThrowOnUnmappableChar = true, BestFitMapping = false)]
      internal static extern int FlushCabinet(
        NativeMethods.FCI.Handle hfci,
        [MarshalAs(UnmanagedType.Bool)] bool fGetNextCab,
        NativeMethods.FCI.PFNGETNEXTCABINET pfnfcignc,
        NativeMethods.FCI.PFNSTATUS pfnfcis);

      [DllImport("cabinet.dll", EntryPoint = "FCIFlushFolder", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, ThrowOnUnmappableChar = true, BestFitMapping = false)]
      internal static extern int FlushFolder(
        NativeMethods.FCI.Handle hfci,
        NativeMethods.FCI.PFNGETNEXTCABINET pfnfcignc,
        NativeMethods.FCI.PFNSTATUS pfnfcis);

      [SuppressUnmanagedCodeSecurity]
      [DllImport("cabinet.dll", EntryPoint = "FCIDestroy", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, ThrowOnUnmappableChar = true, BestFitMapping = false)]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool Destroy(IntPtr hfci);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate IntPtr PFNALLOC(int cb);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void PFNFREE(IntPtr pv);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate int PFNOPEN(string path, int oflag, int pmode, out int err, IntPtr pv);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate int PFNREAD(
        int fileHandle,
        IntPtr memory,
        int cb,
        out int err,
        IntPtr pv);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate int PFNWRITE(
        int fileHandle,
        IntPtr memory,
        int cb,
        out int err,
        IntPtr pv);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate int PFNCLOSE(int fileHandle, out int err, IntPtr pv);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate int PFNSEEK(
        int fileHandle,
        int dist,
        int seekType,
        out int err,
        IntPtr pv);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate int PFNDELETE(string path, out int err, IntPtr pv);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate int PFNGETNEXTCABINET(IntPtr pccab, uint cbPrevCab, IntPtr pv);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate int PFNFILEPLACED(
        IntPtr pccab,
        string path,
        long fileSize,
        int continuation,
        IntPtr pv);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate int PFNGETOPENINFO(
        string path,
        out short date,
        out short time,
        out short pattribs,
        out int err,
        IntPtr pv);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate int PFNSTATUS(
        NativeMethods.FCI.STATUS typeStatus,
        uint cb1,
        uint cb2,
        IntPtr pv);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate int PFNGETTEMPFILE(IntPtr tempNamePtr, int tempNameSize, IntPtr pv);

      internal enum ERROR
      {
        NONE,
        OPEN_SRC,
        READ_SRC,
        ALLOC_FAIL,
        TEMP_FILE,
        BAD_COMPR_TYPE,
        CAB_FILE,
        USER_ABORT,
        MCI_FAIL,
      }

      internal enum TCOMP : ushort
      {
        TYPE_NONE = 0,
        TYPE_MSZIP = 1,
        TYPE_QUANTUM = 2,
        TYPE_LZX = 3,
        SHIFT_QUANTUM_LEVEL = 4,
        SHIFT_LZX_WINDOW = 8,
        SHIFT_QUANTUM_MEM = 8,
        BAD = 15, // 0x000F
        MASK_TYPE = 15, // 0x000F
        QUANTUM_LEVEL_LO = 16, // 0x0010
        QUANTUM_LEVEL_HI = 112, // 0x0070
        MASK_QUANTUM_LEVEL = 240, // 0x00F0
        QUANTUM_MEM_LO = 2560, // 0x0A00
        LZX_WINDOW_LO = 3840, // 0x0F00
        LZX_WINDOW_HI = 5376, // 0x1500
        QUANTUM_MEM_HI = 5376, // 0x1500
        MASK_LZX_WINDOW = 7936, // 0x1F00
        MASK_QUANTUM_MEM = 7936, // 0x1F00
        MASK_RESERVED = 57344, // 0xE000
      }

      internal enum STATUS : uint
      {
        FILE,
        FOLDER,
        CABINET,
      }

      [StructLayout(LayoutKind.Sequential)]
      internal class CCAB
      {
        internal int cb = int.MaxValue;
        internal int cbFolderThresh = 2147450880;
        internal int cbReserveCFHeader;
        internal int cbReserveCFFolder;
        internal int cbReserveCFData;
        internal int iCab;
        internal int iDisk;
        internal int fFailOnIncompressible;
        internal short setID;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        internal string szDisk = string.Empty;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        internal string szCab = string.Empty;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        internal string szCabPath = string.Empty;
      }

      internal class Handle : SafeHandle
      {
        internal Handle()
          : base(IntPtr.Zero, true)
        {
        }

        public override bool IsInvalid => this.handle == IntPtr.Zero;

        [SecurityPermission(SecurityAction.Assert, UnmanagedCode = true)]
        protected override bool ReleaseHandle() => NativeMethods.FCI.Destroy(this.handle);
      }
    }

    internal static class FDI
    {
      internal const int MAX_DISK = 2147483647;
      internal const int MAX_FILENAME = 256;
      internal const int MAX_CABINET_NAME = 256;
      internal const int MAX_CAB_PATH = 256;
      internal const int MAX_DISK_NAME = 256;
      internal const int CPU_80386 = 1;

      [DllImport("cabinet.dll", EntryPoint = "FDICreate", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, ThrowOnUnmappableChar = true, BestFitMapping = false)]
      internal static extern NativeMethods.FDI.Handle Create(
        [MarshalAs(UnmanagedType.FunctionPtr)] NativeMethods.FDI.PFNALLOC pfnalloc,
        [MarshalAs(UnmanagedType.FunctionPtr)] NativeMethods.FDI.PFNFREE pfnfree,
        NativeMethods.FDI.PFNOPEN pfnopen,
        NativeMethods.FDI.PFNREAD pfnread,
        NativeMethods.FDI.PFNWRITE pfnwrite,
        NativeMethods.FDI.PFNCLOSE pfnclose,
        NativeMethods.FDI.PFNSEEK pfnseek,
        int cpuType,
        IntPtr perf);

      [DllImport("cabinet.dll", EntryPoint = "FDICopy", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, ThrowOnUnmappableChar = true, BestFitMapping = false)]
      internal static extern int Copy(
        NativeMethods.FDI.Handle hfdi,
        string pszCabinet,
        string pszCabPath,
        int flags,
        NativeMethods.FDI.PFNNOTIFY pfnfdin,
        IntPtr pfnfdid,
        IntPtr pvUser);

      [SuppressUnmanagedCodeSecurity]
      [DllImport("cabinet.dll", EntryPoint = "FDIDestroy", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, ThrowOnUnmappableChar = true, BestFitMapping = false)]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool Destroy(IntPtr hfdi);

      [DllImport("cabinet.dll", EntryPoint = "FDIIsCabinet", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, ThrowOnUnmappableChar = true, BestFitMapping = false)]
      internal static extern int IsCabinet(
        NativeMethods.FDI.Handle hfdi,
        int hf,
        out NativeMethods.FDI.CABINFO pfdici);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate IntPtr PFNALLOC(int cb);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void PFNFREE(IntPtr pv);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate int PFNOPEN(string path, int oflag, int pmode);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate int PFNREAD(int hf, IntPtr pv, int cb);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate int PFNWRITE(int hf, IntPtr pv, int cb);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate int PFNCLOSE(int hf);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate int PFNSEEK(int hf, int dist, int seektype);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate int PFNNOTIFY(
        NativeMethods.FDI.NOTIFICATIONTYPE fdint,
        NativeMethods.FDI.NOTIFICATION fdin);

      internal enum ERROR
      {
        NONE,
        CABINET_NOT_FOUND,
        NOT_A_CABINET,
        UNKNOWN_CABINET_VERSION,
        CORRUPT_CABINET,
        ALLOC_FAIL,
        BAD_COMPR_TYPE,
        MDI_FAIL,
        TARGET_FILE,
        RESERVE_MISMATCH,
        WRONG_CABINET,
        USER_ABORT,
      }

      internal enum NOTIFICATIONTYPE
      {
        CABINET_INFO,
        PARTIAL_FILE,
        COPY_FILE,
        CLOSE_FILE_INFO,
        NEXT_CABINET,
        ENUMERATE,
      }

      internal struct CABINFO
      {
        internal int cbCabinet;
        internal short cFolders;
        internal short cFiles;
        internal short setID;
        internal short iCabinet;
        internal int fReserve;
        internal int hasprev;
        internal int hasnext;
      }

      [StructLayout(LayoutKind.Sequential)]
      internal class NOTIFICATION
      {
        internal int cb;
        internal IntPtr psz1;
        internal IntPtr psz2;
        internal IntPtr psz3;
        internal IntPtr pv;
        internal IntPtr hf_ptr;
        internal short date;
        internal short time;
        internal short attribs;
        internal short setID;
        internal short iCabinet;
        internal short iFolder;
        internal int fdie;

        internal int hf => (int) this.hf_ptr;
      }

      internal class Handle : SafeHandle
      {
        internal Handle()
          : base(IntPtr.Zero, true)
        {
        }

        public override bool IsInvalid => this.handle == IntPtr.Zero;

        protected override bool ReleaseHandle() => NativeMethods.FDI.Destroy(this.handle);
      }
    }

    [StructLayout(LayoutKind.Sequential)]
    internal class ERF
    {
      private int erfOper;
      private int erfType;
      private int fError;

      internal int Oper
      {
        get => this.erfOper;
        set => this.erfOper = value;
      }

      internal int Type
      {
        get => this.erfType;
        set => this.erfType = value;
      }

      internal bool Error
      {
        get => this.fError != 0;
        set => this.fError = value ? 1 : 0;
      }

      internal void Clear()
      {
        this.Oper = 0;
        this.Type = 0;
        this.Error = false;
      }
    }
  }
}
