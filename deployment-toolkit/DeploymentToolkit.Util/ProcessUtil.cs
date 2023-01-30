
using DeploymentToolkit.ToolkitEnvironment;
using DeploymentToolkit.Util.Structs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace DeploymentToolkit.Util
{
    public static class ProcessUtil
    {
        private const int GENERIC_ALL_ACCESS = 0x10000000;

        private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        public static void StartTrayAppForAllLoggedOnUsers()
        {
            var trayApps = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(EnvironmentVariables.DeploymentToolkitTrayExeName));
            var trayAppSessions = trayApps.Select((p) => p.SessionId);

            var session = GetLoggedOnUserTokens(trayAppSessions);
            if(session.Count == 0)
            {
                _logger.Info("No session found to spawn Tray App");
                return;
            }

            _logger.Trace($"Trying to start Tray App in {session.Count} sessions ...");
            StartProcessInSessions(session, EnvironmentVariables.DeploymentToolkitTrayExePath, "--startup --requested");
        }

        private static List<IntPtr> GetLoggedOnUserTokens(IEnumerable<int> excludeSessions)
        {
            var result = new List<IntPtr>();

            var process = Process.GetProcessesByName("explorer");
            if(process == null || process.Length == 0)
            {
                _logger.Debug("No instances of explorer.exe found. Assuming no logged on users");
                return result;
            }

            var grouped = process.GroupBy((p) => p.SessionId);
            foreach(var group in grouped)
            {
                var sessionId = group.Key;
                if(excludeSessions.Contains(sessionId))
                {
                    _logger.Trace($"Skipping session {sessionId}");
                    continue;
                }

                var handle = group.First().Handle;
                var token = IntPtr.Zero;

                if(NativeMethod.OpenProcessToken(handle, TokenAdjuster.TOKEN_READ | TokenAdjuster.TOKEN_QUERY | TokenAdjuster.TOKEN_DUPLICATE | TokenAdjuster.TOKEN_ASSIGN_PRIMARY, ref token) == 0)
                {
                    _logger.Warn($"Failed to open token from {sessionId} ({Marshal.GetLastWin32Error()}). Skipping ...");
                    continue;
                }

                result.Add(token);
            }

            return result;
        }

        private static void StartProcessInSessions(List<IntPtr> tokens, string path, string arguments)
        {
            if(tokens == null || tokens.Count == 0)
            {
                throw new ArgumentNullException(nameof(tokens));
            }

            // Adjust token
            _logger.Trace("Adjusting token ...");
            if(!TokenAdjuster.EnablePrivilege("SeAssignPrimaryTokenPrivilege", true))
            {
                _logger.Error("Failed to enable required privilege (SeAssignPrimaryTokenPrivilege)");
                return;
            }

            _logger.Trace($"Trying to start '{path}' with arguments '{arguments}' for {tokens.Count} sessions ...");

            foreach(var token in tokens)
            {
                var duplicatedToken = IntPtr.Zero;
                var environment = IntPtr.Zero;
                var processInformation = new PROCESS_INFORMATION();

                try
                {
                    var securityAttributes = new SECURITY_ATTRIBUTES();
                    securityAttributes.Length = Marshal.SizeOf(securityAttributes);

                    if(!NativeMethod.DuplicateTokenEx(
                            token,
                            GENERIC_ALL_ACCESS,
                            ref securityAttributes,
                            (int)SECURITY_IMPERSONATION_LEVEL.SecurityImpersonation,
                            (int)TOKEN_TYPE.TokenPrimary,
                            ref duplicatedToken
                        )
                    )
                    {
                        _logger.Error($"Failed to duplicate token ({Marshal.GetLastWin32Error()})");
                        continue;
                    }

                    if(!NativeMethod.CreateEnvironmentBlock(out environment, duplicatedToken, false))
                    {
                        _logger.Error($"Failed to get environment ({Marshal.GetLastWin32Error()})");
                        continue;
                    }

                    var startupInfo = new STARTUPINFO();
                    startupInfo.cb = Marshal.SizeOf(startupInfo);
                    startupInfo.lpDesktop = @"winsta0\default";
                    startupInfo.wShowWindow = 5; // SW_SHOW

                    if(!NativeMethod.CreateProcessAsUser(
                            duplicatedToken,
                            path,
                            arguments,
                            ref securityAttributes,
                            ref securityAttributes,
                            false,
                            ProcessCreationFlags.NORMAL_PRIORITY_CLASS | ProcessCreationFlags.CREATE_UNICODE_ENVIRONMENT | ProcessCreationFlags.CREATE_NEW_CONSOLE | ProcessCreationFlags.CREATE_BREAKAWAY_FROM_JOB,
                            environment,
                            Path.GetDirectoryName(path),
                            ref startupInfo,
                            ref processInformation
                        )
                    )
                    {
                        _logger.Error($"Failed to start process ({Marshal.GetLastWin32Error()})");
                        continue;
                    }

                    _logger.Info($"Process started as {processInformation.dwProcessID} ({Marshal.GetLastWin32Error()})");
                }
                catch(Exception ex)
                {
                    _logger.Warn(ex, "Error while trying to start process as user");
                }
                finally
                {
                    if(processInformation.hProcess != IntPtr.Zero)
                    {
                        NativeMethod.CloseHandle(processInformation.hProcess);
                    }

                    if(processInformation.hThread != IntPtr.Zero)
                    {
                        NativeMethod.CloseHandle(processInformation.hThread);
                    }

                    if(duplicatedToken != IntPtr.Zero)
                    {
                        NativeMethod.CloseHandle(duplicatedToken);
                    }

                    if(environment != IntPtr.Zero)
                    {
                        NativeMethod.DestroyEnvironmentBlock(environment);
                    }

                    if(token != IntPtr.Zero)
                    {
                        NativeMethod.CloseHandle(token);
                    }
                }
            }
        }
    }
}
