using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace DeploymentToolkit.ToolkitEnvironment
{
    public static partial class EnvironmentVariables
    {
        #region DLLImports
        [DllImport("advapi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool OpenProcessToken(IntPtr ProcessHandle, uint DesiredAccess, out IntPtr TokenHandle);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CloseHandle(IntPtr hObject);

        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern bool GetTokenInformation(IntPtr TokenHandle, TOKEN_INFORMATION_CLASS TokenInformationClass, IntPtr TokenInformation, uint TokenInformationLength, out uint ReturnLength);
        #endregion

        #region Native-Variables
        public const uint STANDARD_RIGHTS_REQUIRED = 0x000F0000;
        public const uint STANDARD_RIGHTS_READ = 0x00020000;
        public const uint TOKEN_ASSIGN_PRIMARY = 0x0001;
        public const uint TOKEN_DUPLICATE = 0x0002;
        public const uint TOKEN_IMPERSONATE = 0x0004;
        public const uint TOKEN_QUERY = 0x0008;
        public const uint TOKEN_QUERY_SOURCE = 0x0010;
        public const uint TOKEN_ADJUST_PRIVILEGES = 0x0020;
        public const uint TOKEN_ADJUST_GROUPS = 0x0040;
        public const uint TOKEN_ADJUST_DEFAULT = 0x0080;
        public const uint TOKEN_ADJUST_SESSIONID = 0x0100;
        public const uint TOKEN_READ = (STANDARD_RIGHTS_READ | TOKEN_QUERY);
        public const uint TOKEN_ALL_ACCESS = (STANDARD_RIGHTS_REQUIRED | TOKEN_ASSIGN_PRIMARY |
            TOKEN_DUPLICATE | TOKEN_IMPERSONATE | TOKEN_QUERY | TOKEN_QUERY_SOURCE |
            TOKEN_ADJUST_PRIVILEGES | TOKEN_ADJUST_GROUPS | TOKEN_ADJUST_DEFAULT |
            TOKEN_ADJUST_SESSIONID);

        public enum TOKEN_INFORMATION_CLASS
        {
            TokenUser = 1,
            TokenGroups,
            TokenPrivileges,
            TokenOwner,
            TokenPrimaryGroup,
            TokenDefaultDacl,
            TokenSource,
            TokenType,
            TokenImpersonationLevel,
            TokenStatistics,
            TokenRestrictedSids,
            TokenSessionId,
            TokenGroupsAndPrivileges,
            TokenSessionReference,
            TokenSandBoxInert,
            TokenAuditPolicy,
            TokenOrigin,
            TokenElevationType,
            TokenLinkedToken,
            TokenElevation,
            TokenHasRestrictions,
            TokenAccessInformation,
            TokenVirtualizationAllowed,
            TokenVirtualizationEnabled,
            TokenIntegrityLevel,
            TokenUIAccess,
            TokenMandatoryPolicy,
            TokenLogonSid,
            MaxTokenInfoClass
        }

        public enum TOKEN_ELEVATION_TYPE
        {
            TokenElevationTypeDefault = 1,
            TokenElevationTypeFull,
            TokenElevationTypeLimited
        }
        #endregion

        private static bool? _isAdministrator;
        public static bool IsAdministrator
        {
            get
            {
                if(_isAdministrator.HasValue)
                {
                    return _isAdministrator.Value;
                }

                var identity = WindowsIdentity.GetCurrent();
                var principal = new WindowsPrincipal(identity);
                _isAdministrator = principal.IsInRole(WindowsBuiltInRole.Administrator);
                return _isAdministrator.Value;
            }
        }

        private static bool? _isElevated = null;
        public static bool IsElevated
        {
            get
            {
                if(_isElevated.HasValue)
                {
                    return _isElevated.Value;
                }

                var tokenHandle = IntPtr.Zero;
                try
                {
                    if(!OpenProcessToken(Process.GetCurrentProcess().Handle, TOKEN_ALL_ACCESS, out tokenHandle))
                    {
                        _logger.Error($"Failed to get process token. Win32 error: {Marshal.GetLastWin32Error()}");
                        _isElevated = false;
                        return false;
                    }

                    try
                    {
                        var identity = new WindowsIdentity(tokenHandle);
                        var principal = new WindowsPrincipal(identity);

                        _isElevated = principal.IsInRole(WindowsBuiltInRole.Administrator) || principal.IsInRole(0x200);

                        if(_isElevated.Value)
                        {
                            return true;
                        }
                    }
                    catch(Exception ex)
                    {
                        _logger.Debug(ex, "Failed to check for elevation. Using method 2");
                    }

                    try
                    {

                        var elevationResult = TOKEN_ELEVATION_TYPE.TokenElevationTypeDefault;
                        var resultSize = Marshal.SizeOf((int)elevationResult); //Marshal.SizeOf(typeof(TOKEN_ELEVATION_TYPE));
                        uint returnedSize = 0;

                        var elevationPointer = Marshal.AllocHGlobal(resultSize);
                        try
                        {
                            var success = GetTokenInformation(tokenHandle, TOKEN_INFORMATION_CLASS.TokenElevationType, elevationPointer, (uint)resultSize, out returnedSize);
                            if(success)
                            {
                                elevationResult = (TOKEN_ELEVATION_TYPE)Marshal.ReadInt32(elevationPointer);
                                _isElevated = elevationResult == TOKEN_ELEVATION_TYPE.TokenElevationTypeFull;
                                return _isElevated.Value;
                            }
                            else
                            {
                                _logger.Error($"Failed to get token information Win32 error: {Marshal.GetLastWin32Error()}");
                            }
                        }
                        catch(Exception ex)
                        {
                            _logger.Error(ex, $"Failed to process token. Win32 error: {Marshal.GetLastWin32Error()}");
                        }
                        finally
                        {
                            if(elevationPointer != IntPtr.Zero)
                            {
                                Marshal.FreeHGlobal(elevationPointer);
                            }
                        }
                    }
                    catch(Exception ex)
                    {
                        _logger.Error(ex);
                    }

                    _isElevated = false;
                    return false;
                }
                catch(Exception ex)
                {
                    _logger.Error(ex, "Failed to process IsElevated");
                    return false;
                }
                finally
                {
                    if(tokenHandle != IntPtr.Zero)
                    {
                        CloseHandle(tokenHandle);
                    }
                }
            }
        }
    }
}
