using DeploymentToolkit.RegistryWrapper;
using Microsoft.Win32;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace DeploymentToolkit.ToolkitEnvironment
{
    public static class ProcessManager
    {
        #region Imports
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        #endregion

        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private const string BlockExecutionKey = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options";
        private const string RunOnceKey = @"HKEY_LOCAL_MACHINE\Software\Microsoft\Windows\CurrentVersion\RunOnce";

        public static bool CheckPrograms(string[] applications, out List<string> openProcesses)
        {
            openProcesses = new List<string>();

            if(applications.Length == 0)
            {
                _logger.Trace("No executables specified to close");
                return false;
            }

            foreach(var executable in applications)
            {
                var executableName = executable.ToLower();
                if(executableName.EndsWith(".exe"))
                {
                    executableName = executableName.Substring(0, executableName.Length - 4);
                }

                _logger.Trace($"Searching for a process named {executableName}");
                var processes = Process.GetProcessesByName(executableName);
                if(processes.Length > 0)
                {
                    openProcesses.AddRange(
                        processes.Select(process => process.ProcessName)
                    );
                }
            }

            if(openProcesses.Count > 0)
            {
                return true;
            }

            return false;
        }

        public static bool ClosePrograms(string[] applications)
        {
            if(applications.Length == 0)
            {
                _logger.Trace("No executables specified to close");
                return true;
            }

            foreach(var executable in applications)
            {
                try
                {
                    var executableName = executable.ToLower();
                    if(executableName.EndsWith(".exe"))
                    {
                        executableName = executableName.Substring(0, executableName.Length - 4);
                    }

                    _logger.Trace($"Searching for a process named {executableName}");
                    var processes = Process.GetProcessesByName(executableName);
                    if(processes.Length > 0)
                    {
                        foreach(var process in processes)
                        {
                            _logger.Trace($"Trying to close [{process.Id}]{process.ProcessName}");
                            // Send a WM_CLOSE and wait for a gracefull exit
                            PostMessage(process.Handle, 0x0010, IntPtr.Zero, IntPtr.Zero);
                            var exited = process.WaitForExit(5000);
                            if(!exited)
                            {
                                _logger.Trace($"Process did not close after close message. Killing...");
                                process.Kill(); // If it does not exit gracefully then just kill it
                            }
                        }
                    }
                }
                catch(Exception ex)
                {
                    _logger.Error(ex, $"Failed to process {executable}");
                }
            }

            return true;
        }

        public static bool BlockExecution(string[] applications)
        {
            if(applications.Length == 0)
            {
                _logger.Trace("No executables specified to block");
                return true;
            }

            try
            {
                var startBlocked = false;
                var executableNames = applications.Distinct();
                foreach(var executable in executableNames)
                {
                    try
                    {
                        var executableName = executable.ToLower();
                        if(!executableName.EndsWith(".exe"))
                        {
                            executableName += ".exe";
                        }

                        _logger.Trace($"Blocking execution of {executableName}");
                        Win96Registry.CreateSubKey(RegistryHive.LocalMachine, BlockExecutionKey, executableName);
                        Win96Registry.SetValue(RegistryHive.LocalMachine, BlockExecutionKey, executableName, "Debugger", EnvironmentVariables.DeploymentToolkitBlockerExePath);
                        Win96Registry.SetValue(RegistryHive.LocalMachine, BlockExecutionKey, executableName, "DT_BLOCK", 0x01);

                        startBlocked = true;
                    }
                    catch(Exception ex)
                    {
                        _logger.Error(ex, $"Failed to process {executable}");
                    }
                }

                if(startBlocked)
                {
                    var registry = new Win64Registry();
                    var subKey = registry.OpenSubKey(RunOnceKey);

                    subKey.SetValue("Deployment Toolkit Unblocker", EnvironmentVariables.DeploymentToolkitUnblockerExePath, RegistryValueKind.String);

                    _logger.Debug($"Set unblocker startup key");
                }

                return true;
            }
            catch(Exception ex)
            {
                _logger.Error(ex, "Failed to block execution of executables");
                return false;
            }
        }

        public static bool UnblockExecution(string[] applications)
        {
            if(applications.Length == 0)
            {
                _logger.Trace("No executables specified to block");
                return true;
            }

            try
            {
                var executableNames = applications.Distinct();
                foreach(var executable in executableNames)
                {
                    try
                    {
                        var executableName = executable.ToLower();
                        if(!executableName.EndsWith(".exe"))
                        {
                            executableName += ".exe";
                        }

                        _logger.Trace($"Unblocking execution of {executableName}");
                        Win96Registry.DeleteSubKey(RegistryHive.LocalMachine, BlockExecutionKey, executableName);
                    }
                    catch(Exception ex)
                    {
                        _logger.Error(ex, $"Failed to process {executable}");
                    }
                }

                return true;
            }
            catch(Exception ex)
            {
                _logger.Error(ex, "Failed to unblock exection of executables");
                return false;
            }
        }

        public static void UnblockAllDTBlockedApps()
        {
            var apps = GetAllDTBlockedApps();
            if(apps.Count == 0)
            {
                _logger.Info("No apps to unblock");
                return;
            }

            _logger.Info($"Unblocking {apps.Count} apps ...");
            try
            {
                UnblockExecution(apps.ToArray());

                _logger.Info("Successfully unblocked");
            }
            catch(Exception ex)
            {
                _logger.Error(ex, $"Failed to unblock execution of apps");
            }
        }

        private static List<string> GetAllDTBlockedApps()
        {
            var result = new List<string>();
            try
            {
                var fullPath = Path.Combine("HKEY_LOCAL_MACHINE", BlockExecutionKey);
                var win32Registry = new Win32Registry();
                var subKeys = win32Registry.GetSubKeys(fullPath);

                foreach(var key in subKeys)
                {
                    if(IsKeyBlockedByDT(win32Registry, fullPath, key))
                    {
                        result.Add(key);
                    }
                }

                if(Environment.Is64BitOperatingSystem)
                {
                    var win64Registry = new Win64Registry();
                    var win64SubKeys = win64Registry.GetSubKeys(fullPath);

                    foreach(var key in subKeys)
                    {
                        if(!IsKeyBlockedByDT(win64Registry, fullPath, key))
                        {
                            continue;
                        }

                        if(result.Contains(key))
                        {
                            continue;
                        }

                        result.Add(key);
                    }
                }
            }
            catch(Exception ex)
            {
                _logger.Error(ex, "Failed to get blocked apps");
            }
            return result;
        }

        private static bool IsKeyBlockedByDT(WinRegistryBase registry, string path, string subKeyName)
        {
            var subKey = registry.OpenSubKey(path, subKeyName);
            if(subKey != null)
            {
                return subKey.GetValue("DT_BLOCK", null) != null;
            }
            return false;
        }
    }
}
