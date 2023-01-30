using DeploymentToolkit.Util.Structs;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace DeploymentToolkit.Util
{
    public sealed class TokenAdjuster
    {
        private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        private const int SE_PRIVILEGE_ENABLED = 0x00000002;
        public const UInt32 STANDARD_RIGHTS_REQUIRED = 0x000F0000;
        public const UInt32 STANDARD_RIGHTS_READ = 0x00020000;
        public const UInt32 TOKEN_ASSIGN_PRIMARY = 0x0001;
        public const UInt32 TOKEN_DUPLICATE = 0x0002;
        public const UInt32 TOKEN_IMPERSONATE = 0x0004;
        public const UInt32 TOKEN_QUERY = 0x0008;
        public const UInt32 TOKEN_QUERY_SOURCE = 0x0010;
        public const UInt32 TOKEN_ADJUST_PRIVILEGES = 0x0020;
        public const UInt32 TOKEN_ADJUST_GROUPS = 0x0040;
        public const UInt32 TOKEN_ADJUST_DEFAULT = 0x0080;
        public const UInt32 TOKEN_ADJUST_SESSIONID = 0x0100;
        public const UInt32 TOKEN_READ = (STANDARD_RIGHTS_READ | TOKEN_QUERY);
        public const UInt32 TOKEN_ALL_ACCESS = (STANDARD_RIGHTS_REQUIRED | TOKEN_ASSIGN_PRIMARY | TOKEN_DUPLICATE | TOKEN_IMPERSONATE | TOKEN_QUERY | TOKEN_QUERY_SOURCE | TOKEN_ADJUST_PRIVILEGES | TOKEN_ADJUST_GROUPS | TOKEN_ADJUST_DEFAULT | TOKEN_ADJUST_SESSIONID);
        private const int PROCESS_QUERY_INFORMATION = 0X00000400;

        internal static bool EnablePrivilege(string privilegeName, bool bEnablePrivilege)
        {
            _logger.Trace($"Trying to enable {privilegeName}");

            int returnLength = 0;
            var token = IntPtr.Zero;
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

            var unmanagedTokenPrivileges = IntPtr.Zero;
            try
            {
                if(!NativeMethod.LookupPrivilegeValue(null, privilegeName, ref tLUID))
                {
                    _logger.Warn($"Failed to Lookup {privilegeName}");
                    return false;
                }

                var process = Process.GetCurrentProcess();
                if(process.Handle == IntPtr.Zero)
                {
                    _logger.Warn($"Failed to get process handle");
                    return false;
                }


                if(NativeMethod.OpenProcessToken(process.Handle, TOKEN_ALL_ACCESS, ref token) == 0)
                {
                    _logger.Warn($"Failed to open process token ({Marshal.GetLastWin32Error()})");
                    return false;
                }

                tokenPrivileges.PrivilegeCount = 1;
                tokenPrivileges.Privileges[2] = SE_PRIVILEGE_ENABLED;
                tokenPrivileges.Privileges[1] = tLUID.HighPart;
                tokenPrivileges.Privileges[0] = tLUID.LowPart;
                const int bufLength = 256;
                unmanagedTokenPrivileges = Marshal.AllocHGlobal(bufLength);
                Marshal.StructureToPtr(tokenPrivileges, unmanagedTokenPrivileges, true);
                if(NativeMethod.AdjustTokenPrivileges(token, 0, unmanagedTokenPrivileges, bufLength, IntPtr.Zero, ref returnLength) == 0)
                {
                    _logger.Warn($"Failed to adjust privileges ({Marshal.GetLastWin32Error()})");
                    _logger.Warn(new Win32Exception(Marshal.GetLastWin32Error()));
                    return false;
                }

                if(Marshal.GetLastWin32Error() != 0)
                {
                    _logger.Warn($"Failed to adjust privileges ({Marshal.GetLastWin32Error()})");
                    return false;
                }

                _logger.Debug($"Successfully enabled privilege {privilegeName}");
                return true;
            }
            catch(Exception ex)
            {
                _logger.Error(ex, $"Error while trying to enable {privilegeName}");
                return false;
            }
            finally
            {
                if(token != IntPtr.Zero)
                {
                    NativeMethod.CloseHandle(token);
                }

                if(unmanagedTokenPrivileges != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(unmanagedTokenPrivileges);
                }
            }
        }
    }
}
