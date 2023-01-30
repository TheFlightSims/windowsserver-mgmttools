using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security;

namespace DeploymentToolkit.Actions.Utils
{
    public sealed class TokenAdjuster
    {
        // PInvoke stuff required to set/enable security privileges
        private const int SE_PRIVILEGE_ENABLED = 0x00000002;
        private const int TOKEN_ADJUST_PRIVILEGES = 0X00000020;
        private const int TOKEN_QUERY = 0X00000008;
        private const int TOKEN_ALL_ACCESS = 0X001f01ff;
        private const int PROCESS_QUERY_INFORMATION = 0X00000400;

        [DllImport("advapi32", SetLastError = true), SuppressUnmanagedCodeSecurity]
        private static extern int OpenProcessToken(
            IntPtr ProcessHandle, // handle to process
            int DesiredAccess, // desired access to process
            ref IntPtr TokenHandle // handle to open access token
        );

        [DllImport("kernel32", SetLastError = true), SuppressUnmanagedCodeSecurity]
        private static extern bool CloseHandle(
            IntPtr handle
        );

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int AdjustTokenPrivileges(
            IntPtr TokenHandle,
            int DisableAllPrivileges,
            IntPtr NewState,
            int BufferLength,
            IntPtr PreviousState,
            ref int ReturnLength
        );

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool LookupPrivilegeValue(
            string lpSystemName,
            string lpName,
            ref LUID lpLuid
        );

        public static bool EnablePrivilege(string lpszPrivilege, bool bEnablePrivilege)
        {
            var retval = false;
            int ltkpOld = 0;
            var hToken = IntPtr.Zero;
            var tokenPrivileges = new TOKEN_PRIVILEGES
            {
                Privileges = new int[3]
            };
            var oldTokenPrivileges = new TOKEN_PRIVILEGES
            {
                Privileges = new int[3]
            };
            var tLUID = new LUID();
            tokenPrivileges.PrivilegeCount = 1;
            if(bEnablePrivilege)
            {
                tokenPrivileges.Privileges[2] = SE_PRIVILEGE_ENABLED;
            }
            else
            {
                tokenPrivileges.Privileges[2] = 0;
            }

            if(LookupPrivilegeValue(null, lpszPrivilege, ref tLUID))
            {
                var proc = Process.GetCurrentProcess();
                if(proc.Handle != IntPtr.Zero)
                {
                    if(OpenProcessToken(proc.Handle, TOKEN_ADJUST_PRIVILEGES | TOKEN_QUERY,
                        ref hToken) != 0)
                    {
                        tokenPrivileges.PrivilegeCount = 1;
                        tokenPrivileges.Privileges[2] = SE_PRIVILEGE_ENABLED;
                        tokenPrivileges.Privileges[1] = tLUID.HighPart;
                        tokenPrivileges.Privileges[0] = tLUID.LowPart;
                        const int bufLength = 256;
                        var tu = Marshal.AllocHGlobal(bufLength);
                        Marshal.StructureToPtr(tokenPrivileges, tu, true);
                        if(AdjustTokenPrivileges(hToken, 0, tu, bufLength, IntPtr.Zero, ref ltkpOld) != 0)
                        {
                            // successful AdjustTokenPrivileges doesn't mean privilege could be changed
                            if(Marshal.GetLastWin32Error() == 0)
                            {
                                retval = true; // Token changed
                            }
                        }
                        Marshal.FreeHGlobal(tu);
                    }
                }
            }
            if(hToken != IntPtr.Zero)
            {
                CloseHandle(hToken);
            }
            return retval;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct LUID
        {
            internal int LowPart;
            internal int HighPart;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct LUID_AND_ATTRIBUTES
        {
            private LUID Luid;
            private readonly int Attributes;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct TOKEN_PRIVILEGES
        {
            internal int PrivilegeCount;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            internal int[] Privileges;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct _PRIVILEGE_SET
        {
            private readonly int PrivilegeCount;
            private readonly int Control;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)] // ANYSIZE_ARRAY = 1
            private readonly LUID_AND_ATTRIBUTES[] Privileges;
        }
    }
}
