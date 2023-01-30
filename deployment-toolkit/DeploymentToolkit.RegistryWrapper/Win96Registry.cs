using Microsoft.Win32;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;

namespace DeploymentToolkit.RegistryWrapper
{
    public static class Win96Registry
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private static readonly Win32Registry _win32Registry = new Win32Registry();
        private static readonly Win64Registry _win64Registry = new Win64Registry();

        private static List<RegistryKey> GetBaseKey(RegistryHive hive)
        {
            switch(hive)
            {
                case RegistryHive.ClassesRoot:
                    return new List<RegistryKey>() {
                        _win32Registry.HKEY_CLASSES_ROOT,
                        _win64Registry.HKEY_CLASSES_ROOT
                    };
                case RegistryHive.CurrentConfig:
                    return new List<RegistryKey>() {
                        _win32Registry.HKEY_CURRENT_CONFIG,
                        _win64Registry.HKEY_CURRENT_CONFIG
                    };
                case RegistryHive.CurrentUser:
                    return new List<RegistryKey>() {
                        _win32Registry.HKEY_CURRENT_USER,
                        _win64Registry.HKEY_CURRENT_USER
                    };
                case RegistryHive.LocalMachine:
                    return new List<RegistryKey>() {
                        _win32Registry.HKEY_LOCAL_MACHINE,
                        _win64Registry.HKEY_LOCAL_MACHINE
                    };
                case RegistryHive.Users:
                    return new List<RegistryKey>() {
                        _win32Registry.HKEY_USERS,
                        _win64Registry.HKEY_USERS
                    };
                default:
                    throw new ArgumentOutOfRangeException(nameof(hive), "Unsupported hive");
            }
        }

        public static void CreateSubKey(RegistryHive hive, string path, string subKeyName)
        {
            _logger.Trace($"CreateSubKey({hive}, {path}, {subKeyName})");
            if(string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            if(string.IsNullOrEmpty(subKeyName))
            {
                throw new ArgumentNullException(nameof(subKeyName));
            }

            var hives = GetBaseKey(hive);
            foreach(var regHive in hives)
            {
                var subKey = regHive.OpenSubKey(path, true);
                if(subKey != null)
                {
                    subKey.CreateSubKey(subKeyName);
                }
                else
                {
                    _logger.Warn($"Failed to open {path}");
                }
            }
        }

        public static void DeleteSubKey(RegistryHive hive, string path, string subKeyName)
        {
            _logger.Trace($"DeleteSubKey({hive}, {path}, {subKeyName})");
            if(string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            if(string.IsNullOrEmpty(subKeyName))
            {
                throw new ArgumentNullException(nameof(subKeyName));
            }

            var hives = GetBaseKey(hive);
            foreach(var reghive in hives)
            {
                var subKey = reghive.OpenSubKey(path, true);
                if(subKey != null)
                {
                    subKey.DeleteSubKeyTree(subKeyName, false);
                }
            }
        }

        public static void SetValue(RegistryHive hive, string path, string subKeyName, string name, object value)
        {
            _logger.Trace($"SetValue({hive}, {path}, {subKeyName}, {name}, {value})");
            if(string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            if(string.IsNullOrEmpty(subKeyName))
            {
                throw new ArgumentNullException(nameof(subKeyName));
            }

            SetValue(hive, Path.Combine(path, subKeyName), name, value);
        }

        public static void SetValue(RegistryHive hive, string path, string name, object value)
        {
            _logger.Trace($"SetValue({hive}, {path}, {name}, {value})");
            if(string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            if(string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            var hives = GetBaseKey(hive);
            foreach(var regHive in hives)
            {
                var subKey = regHive.OpenSubKey(path, true);
                if(subKey != null)
                {
                    subKey.SetValue(name, value);
                }
                else
                {
                    _logger.Warn($"Can't set value as {path} does not exist");
                }
            }
        }
    }
}
