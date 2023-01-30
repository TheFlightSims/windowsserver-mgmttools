using DeploymentToolkit.Registry.Modals;
using Microsoft.Win32;
using NLog;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;

namespace DeploymentToolkit.Registry
{
    public abstract class WinRegistryBase
    {
        [DllImport("advapi32.dll")]
        private static extern int RegOpenKeyEx(
            RegistryHive hKey,
            [MarshalAs(UnmanagedType.VBByRefStr)] ref string subKey,
            int options,
            RegAccess sam,
            out UIntPtr phkResult
        );

        [DllImport("advapi32.dll")]
        private static extern int RegCloseKey(
            UIntPtr hKey
        );

        [DllImport("advapi32.dll")]
        private static extern int RegCreateKeyEx(
                RegistryHive hKey,
                string lpSubKey,
                int Reserved,
                string lpClass,
                RegOption dwOptions,
                RegSAM samDesired,
                SECURITY_ATTRIBUTES lpSecurityAttributes,
                out UIntPtr phkResult,
                out RegDisposition lpdwDisposition
        );

        [DllImport("advapi32.dll")]
        public static extern int RegDeleteKeyEx(
            UIntPtr hKey,
            string lpSubKey,
            RegSAM samDesired,
            uint Reserved
        );

        protected abstract RegAccess RegAccess { get; }

        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public WinRegistryKey OpenKey(string path, bool write = false)
        {
            _logger.Trace($"OpenKey({path}, {write})");
            if(string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            var hive = GetHiveFromString(path, out var newPath);
            _logger.Trace($"Hive: {hive}");
            var error = RegOpenKeyEx(hive, ref newPath, 0, write ? RegAccess.KEY_ALL_ACCESS : RegAccess.KEY_READ | RegAccess, out var key);
            _logger.Trace($"Errorlevel: {error}");

            if(error == 0)
            {
                return new WinRegistryKey(this, key, newPath, hive);
            }

            throw new Win32Exception(error);
        }

        public WinRegistryKey CreateOrOpenKey(string path)
        {
            _logger.Trace($"CreateKey({path})");
            if(string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            var hive = GetHiveFromString(path, out var newPath);
            return InternalCreateOrOpenKey(newPath, hive);
        }

        internal WinRegistryKey InternalCreateOrOpenKey(string path, RegistryHive hive)
        {
            _logger.Trace($"InternalCreateOrOpenKey({path}, {hive})");
            var error = RegCreateKeyEx(
                hive,
                path,
                0,
                string.Empty,
                RegOption.NonVolatile,
                RegSAM.Write | (RegAccess == RegAccess.KEY_WOW64_64KEY ? RegSAM.WOW64_64Key : RegSAM.WOW64_32Key),
                new SECURITY_ATTRIBUTES(),
                out UIntPtr key,
                out RegDisposition result
            );
            _logger.Trace($"Errorlevel: {error}");

            if(error == 0)
            {
                return new WinRegistryKey(this, key, path, hive);
            }

            throw new Win32Exception(error);
        }

        public bool DeleteKey(WinRegistryKey key, string subKey)
        {
            _logger.Trace($"DeleteKey({key.Key}, {subKey})");
            if(string.IsNullOrEmpty(subKey))
            {
                throw new ArgumentNullException(nameof(subKey));
            }

            var error = RegDeleteKeyEx(
                key.RegPointer,
                subKey,
                RegAccess == RegAccess.KEY_WOW64_64KEY ? RegSAM.WOW64_64Key : RegSAM.WOW64_32Key,
                0
            );
            _logger.Trace($"Errorlevel: {error}");

            if(error == 0)
            {
                return true;
            }

            throw new Win32Exception(error);
        }

        public bool CloseKey(WinRegistryKey key)
        {
            _logger.Trace($"CloseKey({key.Key})");
            var error = RegCloseKey(key.RegPointer);
            _logger.Trace($"Errorlevel: {error}");

            if(error == 0)
            {
                return true;
            }

            throw new Win32Exception(error);
        }

        private RegistryHive GetHiveFromString(string path, out string newPath)
        {
            _logger.Trace($"GetHiveFromString({path})");
            var split = path.Split('\\');
            if(split.Length == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(path));
            }

            var hive = split[0].ToUpper();
            _logger.Trace($"Hive: {hive}");
            newPath = split.Length > 2 ? split.Skip(1).Aggregate((i, j) => i + "\\" + j) : split[1];
            _logger.Trace($"NewPath: {newPath}");
            switch(hive)
            {
                case "HKEY_LOCAL_MACHINE":
                    return RegistryHive.LocalMachine;
                case "HKEY_CURRENT_USER":
                    return RegistryHive.CurrentUser;
                case "HKEY_CURRENT_CONFIG":
                    return RegistryHive.CurrentConfig;
                case "HKEY_CLASSES_ROOT":
                    return RegistryHive.ClassesRoot;
                case "HKEY_USERS":
                    return RegistryHive.Users;

                default:
                    throw new ArgumentOutOfRangeException(nameof(path), "Invalid RegHive");
            }
        }
    }
}
