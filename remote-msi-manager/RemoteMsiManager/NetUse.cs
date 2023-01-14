using System;
using System.Runtime.InteropServices;

using DWORD = System.UInt32;
using LPWSTR = System.String;
using NET_API_STATUS = System.UInt32;

namespace RemoteMsiManager
{
    class NetUse
    {
        [DllImport("NetApi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern NET_API_STATUS NetUseAdd(
            LPWSTR UncServerName,
            DWORD Level,
            ref USE_INFO_2 Buf,
            out DWORD ParmError);

        [DllImport("NetApi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern NET_API_STATUS NetUseDel(
            LPWSTR UncServerName,
            LPWSTR UseName,
            DWORD ForceCond);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr LocalFree(IntPtr hMem);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern int FormatMessage(FormatMessageFlags dwFlags, IntPtr lpSource, uint dwMessageId, uint dwLanguageId, ref IntPtr lpBuffer, uint nSize, IntPtr Arguments);

        [Flags]
        private enum FormatMessageFlags : uint
        {
            FORMAT_MESSAGE_ALLOCATE_BUFFER = 0x00000100,
            FORMAT_MESSAGE_IGNORE_INSERTS = 0x00000200,
            FORMAT_MESSAGE_FROM_SYSTEM = 0x00001000,
            FORMAT_MESSAGE_ARGUMENT_ARRAY = 0x00002000,
            FORMAT_MESSAGE_FROM_HMODULE = 0x00000800,
            FORMAT_MESSAGE_FROM_STRING = 0x00000400,
        }


        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct USE_INFO_2
        {
            internal LPWSTR ui2_local;
            internal LPWSTR ui2_remote;
            internal LPWSTR ui2_password;
            internal DWORD ui2_status;
            internal DWORD ui2_asg_type;
            internal DWORD ui2_refcount;
            internal DWORD ui2_usecount;
            internal LPWSTR ui2_username;
            internal LPWSTR ui2_domainname;
        }


        internal static bool Mount(string drive, string networkPath, string username, string password)
        {
            if (!string.IsNullOrEmpty(drive))
            {
                NetUseDel("", drive, 2);
            }
            else
            {
                NetUseDel("", networkPath, 2);
            }

            USE_INFO_2 useInfo = new USE_INFO_2
            {
                ui2_local = drive,
                ui2_remote = networkPath,
                ui2_asg_type = 0,    //disk drive
                ui2_usecount = 1
            };
            if (!String.IsNullOrEmpty(username))
            {
                useInfo.ui2_username = username;
                useInfo.ui2_password = password;
            }

            uint paramErrorIndex;
            uint returnCode = NetUseAdd(null, 2, ref useInfo, out paramErrorIndex);
            if (returnCode != 0)
            {
                throw new Exception(GetSystemMessage((int)returnCode));
            }
            return true;
        }

        internal static void UnMount(string networkPath_or_Drive)
        {
            try
            {
                NetUseDel("", networkPath_or_Drive, 2);
            }
            catch (Exception) { }
        }

        internal static string GetSystemMessage(int errorCode)
        {
            try
            {
                IntPtr lpMsgBuf = IntPtr.Zero;

                int dwChars = FormatMessage(
                    FormatMessageFlags.FORMAT_MESSAGE_ALLOCATE_BUFFER | FormatMessageFlags.FORMAT_MESSAGE_FROM_SYSTEM | FormatMessageFlags.FORMAT_MESSAGE_IGNORE_INSERTS,
                    IntPtr.Zero,
                    (uint)errorCode,
                    0, // Default language
                    ref lpMsgBuf,
                    0,
                    IntPtr.Zero);
                if (dwChars == 0)
                {
                    // Handle the error.
                    int le = Marshal.GetLastWin32Error();
                    return "Unable to get error code string from System - Error " + le.ToString();
                }

                string sRet = Marshal.PtrToStringAnsi(lpMsgBuf);

                // Free the buffer.
                lpMsgBuf = LocalFree(lpMsgBuf);
                return sRet;
            }
            catch (Exception e)
            {
                return "Unable to get error code string from System -> " + e.ToString();
            }
        }


    }
}
