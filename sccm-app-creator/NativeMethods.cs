using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace CM_App_Creator
{
    internal static class NativeMethods
    {
        [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true, CharSet = System.Runtime.InteropServices.CharSet.Unicode)]
        [System.Security.SuppressUnmanagedCodeSecurity]
        public static extern IntPtr LoadLibraryEx(string lpFileName, IntPtr hFile, uint dwFlags);

        [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
        [System.Security.SuppressUnmanagedCodeSecurity]
        public static extern bool FreeLibrary(IntPtr hModule);

        [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true, CharSet = System.Runtime.InteropServices.CharSet.Unicode)]
        [System.Security.SuppressUnmanagedCodeSecurity]
        public static extern bool EnumResourceNames(IntPtr hModule, IntPtr lpszType, ENUMRESNAMEPROCA lpEnumFunc, IntPtr lParam);

        [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true, CharSet = System.Runtime.InteropServices.CharSet.Unicode)]
        [System.Security.SuppressUnmanagedCodeSecurity]
        public static extern IntPtr FindResource(IntPtr hModule, IntPtr lpName, IntPtr lpType);

        [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
        [System.Security.SuppressUnmanagedCodeSecurity]
        public static extern IntPtr LoadResource(IntPtr hModule, IntPtr hResInfo);

        [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
        [System.Security.SuppressUnmanagedCodeSecurity]
        public static extern IntPtr LockResource(IntPtr hResData);

        [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
        [System.Security.SuppressUnmanagedCodeSecurity]
        public static extern uint SizeofResource(IntPtr hModule, IntPtr hResInfo);

        [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
        [System.Security.SuppressUnmanagedCodeSecurity]
        public static extern IntPtr GetCurrentProcess();

        [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true, CharSet = System.Runtime.InteropServices.CharSet.Unicode)]
        [System.Security.SuppressUnmanagedCodeSecurity]
        public static extern int QueryDosDevice(string lpDeviceName, StringBuilder lpTargetPath, int ucchMax);

        [System.Runtime.InteropServices.DllImport("psapi.dll", SetLastError = true, CharSet = System.Runtime.InteropServices.CharSet.Unicode)]
        [System.Security.SuppressUnmanagedCodeSecurity]
        public static extern int GetMappedFileName(IntPtr hProcess, IntPtr lpv, StringBuilder lpFilename, int nSize);
    }

    [System.Runtime.InteropServices.UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Winapi, SetLastError = true, CharSet = System.Runtime.InteropServices.CharSet.Unicode)]
    [System.Security.SuppressUnmanagedCodeSecurity]
    internal delegate bool ENUMRESNAMEPROCA(IntPtr hModule, IntPtr lpszType, IntPtr lpszName, IntPtr lParam);
}