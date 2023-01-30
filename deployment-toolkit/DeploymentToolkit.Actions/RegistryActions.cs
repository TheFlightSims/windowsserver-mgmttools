using DeploymentToolkit.Actions.Utils;
using DeploymentToolkit.RegistryWrapper;
using Microsoft.Win32;
using NLog;
using System;
using System.IO;

namespace DeploymentToolkit.Actions
{
    public static class RegistryActions
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private static readonly Win32Registry _win32Registry = new Win32Registry();
        private static readonly Win64Registry _win64Registry = new Win64Registry();

        public enum Architecture : byte
        {
            Win32,
            Win64
        }

        private static WinRegistryBase GetRegistry(Architecture architecture)
        {
            if(architecture == Architecture.Win32)
            {
                return _win32Registry;
            }
            else if(architecture == Architecture.Win64)
            {
                return _win64Registry;
            }
            throw new Exception("Invalid architecture");
        }

        public static bool KeyExists(Architecture architecture, string path, string keyName)
        {
            _logger.Trace($"KeyExists({architecture}, {path}, {keyName})");
            if(string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            if(string.IsNullOrEmpty(keyName))
            {
                throw new ArgumentNullException(nameof(keyName));
            }

            try
            {
                var registry = GetRegistry(architecture);
                return registry.SubKeyExists(path, keyName);
            }
            catch(Exception ex)
            {
                _logger.Error(ex, $"Failed to check for existence of {path}");
                return false;
            }
        }

        public static bool CreateKey(Architecture architecture, string path, string keyName)
        {
            _logger.Trace($"CreateKey({architecture}, {path}, {keyName})");
            if(string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            if(string.IsNullOrEmpty(keyName))
            {
                throw new ArgumentNullException(nameof(keyName));
            }

            try
            {
                var registry = GetRegistry(architecture);
                return registry.CreateSubKey(path, keyName);
            }
            catch(Exception ex)
            {
                _logger.Error(ex, $"Failed to create key {keyName} in {path}");
                return false;
            }
        }

        public static bool CreateKeyForAllUsers(Architecture architecture, string path, string keyName, bool includeDefaultProfile, bool includeSpecialProfiles)
        {
            _logger.Trace($"CreateKeyForAllUsers({architecture}, {path}, {keyName}, {includeDefaultProfile}, {includeSpecialProfiles})");

            foreach(var user in User.GetUserRegistry(includeDefaultProfile, includeSpecialProfiles))
            {
                try
                {
                    var userPath = Path.Combine(user, path);
                    CreateKey(architecture, userPath, keyName);
                }
                catch(Exception ex)
                {
                    _logger.Error(ex, $"Failed to process '{user}'");
                    return false;
                }
            }

            return true;
        }

        public static bool DeleteKey(Architecture architecture, string path, string keyName)
        {
            _logger.Trace($"DeleteKey({architecture}, {path}, {keyName})");
            if(string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            if(string.IsNullOrEmpty(keyName))
            {
                throw new ArgumentNullException(nameof(keyName));
            }

            try
            {
                var registry = GetRegistry(architecture);
                return registry.DeleteSubKey(path, keyName);
            }
            catch(Exception ex)
            {
                _logger.Error(ex, $"Failed to delete key {keyName} in {path}");
                return false;
            }
        }

        public static bool DeleteKeyForAllUsers(Architecture architecture, string path, string keyName, bool includeDefaultProfile, bool includeSpecialProfiles)
        {
            _logger.Trace($"DeleteKeyForAllUsers({architecture}, {path}, {keyName}, {includeDefaultProfile}, {includeSpecialProfiles})");

            foreach(var user in User.GetUserRegistry(includeDefaultProfile, includeSpecialProfiles))
            {
                try
                {
                    var userPath = Path.Combine(user, path);
                    DeleteKey(architecture, userPath, keyName);
                }
                catch(Exception ex)
                {
                    _logger.Error(ex, $"Failed to process '{user}'");
                    return false;
                }
            }

            return true;
        }

        public static bool ValueExists(Architecture architecture, string path, string valueName)
        {
            _logger.Trace($"ValueExists({architecture}, {path}, {valueName})");
            if(string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            if(string.IsNullOrEmpty(valueName))
            {
                throw new ArgumentNullException(nameof(valueName));
            }

            try
            {
                var registry = GetRegistry(architecture);
                return registry.HasValue(path, valueName);
            }
            catch(Exception ex)
            {
                _logger.Error(ex, $"Failed to get {valueName} in {path}");
                return false;
            }
        }

        public static string GetValue(Architecture architecture, string path, string valueName)
        {
            _logger.Trace($"GetValue({architecture}, {path}, {valueName})");
            if(string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            if(string.IsNullOrEmpty(valueName))
            {
                throw new ArgumentNullException(nameof(valueName));
            }

            try
            {
                var registry = GetRegistry(architecture);
                return registry.GetValue(path, valueName)?.ToString() ?? string.Empty;
            }
            catch(Exception ex)
            {
                _logger.Error(ex, $"Failed to get value of {valueName} in {path}");
                return null;
            }
        }

        public static bool SetValue(Architecture architecture, string path, string valueName, object value, RegistryValueKind valueType)
        {
            _logger.Trace($"SetValue({architecture}, {path}, {valueName}, {value}, {valueType})");
            if(string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            if(string.IsNullOrEmpty(valueName))
            {
                throw new ArgumentNullException(nameof(valueName));
            }

            try
            {
                var registry = GetRegistry(architecture);
                return registry.SetValue(path, valueName, value, valueType);
            }
            catch(Exception ex)
            {
                _logger.Error(ex, $"Failed to set value of {valueName}");
                return false;
            }
        }

        public static bool SetValueForAllUsers(Architecture architecture, string path, string valueName, string value, RegistryValueKind valueType, bool includeDefaultProfile, bool includeSpecialProfiles)
        {

            _logger.Trace($"SetValueForAllUsers({architecture}, {path}, {valueName}, {value}, {valueType}, {includeDefaultProfile}, {includeSpecialProfiles})");
            foreach(var user in User.GetUserRegistry(includeDefaultProfile, includeSpecialProfiles))
            {
                try
                {
                    var userPath = Path.Combine(user, path);
                    SetValue(architecture, userPath, valueName, value, valueType);
                }
                catch(Exception ex)
                {
                    _logger.Error(ex, $"Failed to process '{user}'");
                    return false;
                }
            }

            return true;
        }

        public static bool DeleteValue(Architecture architecture, string path, string valueName)
        {
            _logger.Trace($"DeleteValue({architecture}, {path}, {valueName})");
            if(string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            if(string.IsNullOrEmpty(valueName))
            {
                throw new ArgumentNullException(nameof(valueName));
            }

            try
            {
                var registry = GetRegistry(architecture);
                return registry.DeleteValue(path, valueName);
            }
            catch(Exception ex)
            {
                _logger.Error(ex, $"Failed to delete {valueName} in {path}");
                return false;
            }
        }

        public static bool DeleteValueForAllUsers(Architecture architecture, string path, string valueName, bool includeDefaultProfile, bool includeSpecialProfiles)
        {
            _logger.Trace($"DeleteValueForAllUsers({architecture}, {path}, {valueName}, {includeDefaultProfile}, {includeSpecialProfiles})");
            foreach(var user in User.GetUserRegistry(includeDefaultProfile, includeSpecialProfiles))
            {
                try
                {
                    var userPath = Path.Combine(user, path);
                    DeleteValue(architecture, userPath, valueName);
                }
                catch(Exception ex)
                {
                    _logger.Error(ex, $"Failed to process '{user}'");
                    return false;
                }
            }

            return true;
        }
    }
}
