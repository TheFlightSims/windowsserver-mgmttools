using Microsoft.Win32;
using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace DeploymentToolkit.Registry.Modals
{
    public class WinRegistryKey : IDisposable
    {
        [DllImport("advapi32.dll")]
        private static extern int RegQueryValueEx(
            UIntPtr hKey,
            string lpValueName,
            int lpReserved,
            ref RegistryValueKind lpType,
            IntPtr lpData,
            ref int lpcbData
        );

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern int RegSetValueEx(
            UIntPtr hKey,
            [MarshalAs(UnmanagedType.LPStr)] string lpValueName,
            int Reserved,
            RegistryValueKind dwType,
            IntPtr lpData,
            int cbData
        );

        [DllImport("advapi32.dll")]
        private static extern int RegDeleteValue(
            UIntPtr hKey,
            [MarshalAs(UnmanagedType.VBByRefStr)] ref string lpValueName
        );

        [DllImport("advapi32.dll")]
        private static extern int RegQueryInfoKey(
            UIntPtr hkey,
            StringBuilder lpClass,
            ref uint lpcbClass,
            IntPtr lpReserved,
            ref uint lpcSubKeys,
            IntPtr lpcbMaxSubKeyLen,
            IntPtr lpcbMaxClassLen,
            IntPtr lpcValues,
            IntPtr lpcbMaxValueNameLen,
            IntPtr lpcbMaxValueLen,
            IntPtr lpcbSecurityDescriptor,
            IntPtr lpftLastWriteTime
        );

        [DllImport("advapi32.dll")]
        private static extern int RegEnumKeyEx(
            UIntPtr hKey,
            uint dwIndex,
            StringBuilder lpName,
            ref uint lpcchName,
            IntPtr lpReserved,
            IntPtr lpClass,
            IntPtr lpcchClass,
            System.Runtime.InteropServices.ComTypes.FILETIME lpftLastWriteTime
        );

        public string Key { get; }

        internal UIntPtr RegPointer { get; }

        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private WinRegistryBase _winRegistryBase { get; }
        private RegistryHive _hive { get; }

        public WinRegistryKey(WinRegistryBase winRegistryBase, UIntPtr regPointer, string Key, RegistryHive hive)
        {
            this._winRegistryBase = winRegistryBase;
            this.Key = Key;
            this.RegPointer = regPointer;
            this._hive = hive;
        }

        public bool DeleteValue(string key)
        {
            _logger.Trace($"DeleteValue({key})");
            var error = RegDeleteValue(RegPointer, ref key);
            _logger.Trace($"Errorlevel: {error}");

            if(error == 0)
            {
                return true;
            }

            throw new Win32Exception(error);
        }

        public bool SetValue(string key, object value, RegistryValueKind type)
        {
            _logger.Trace($"SetValue({key}, {value}, {type})");
            var size = 0;
            var data = IntPtr.Zero;
            try
            {
                switch(type)
                {
                    case RegistryValueKind.String:
                    {
                        _logger.Trace("Allocating string ...");
                        size = ((string)value).Length + 1;
                        data = Marshal.StringToHGlobalAnsi((string)value);
                    }
                    break;

                    case RegistryValueKind.DWord:
                    {
                        _logger.Trace("Allocating int ...");
                        size = Marshal.SizeOf(typeof(int));
                        data = Marshal.AllocHGlobal(size);
                        Marshal.WriteInt32(data, (int)value);
                    }
                    break;

                    case RegistryValueKind.QWord:
                    {
                        _logger.Trace("Allocating long ...");
                        size = Marshal.SizeOf(typeof(long));
                        data = Marshal.AllocHGlobal(size);
                        Marshal.WriteInt64(data, (int)value);
                    }
                    break;
                }

                var error = RegSetValueEx(RegPointer, key, 0, type, data, size);
                _logger.Trace($"Errorlevel: {error}");

                if(error == 0)
                {
                    return true;
                }

                throw new Win32Exception(error);
            }
            finally
            {
                if(data != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(data);
                }
            }
        }

        public T GetValue<T>(string key, RegistryValueKind type)
        {
            _logger.Trace($"GetValue({key}, {type})");
            int size = 0;
            var result = IntPtr.Zero;
            try
            {
                var error = RegQueryValueEx(RegPointer, key, 0, ref type, IntPtr.Zero, ref size);
                _logger.Trace($"Errorlevel: {error}");
                if(error != 0)
                {
                    throw new Win32Exception(error);
                }

                result = Marshal.AllocHGlobal(size);
                error = RegQueryValueEx(RegPointer, key, 0, ref type, result, ref size);
                _logger.Trace($"Errorlevel: {error}");
                if(error == 0)
                {
                    var resultObject = default(T);
                    switch(type)
                    {
                        case RegistryValueKind.String:
                            _logger.Trace("Fetching string ...");
                            resultObject = (T)Convert.ChangeType(Marshal.PtrToStringAnsi(result), typeof(T));
                            break;
                        case RegistryValueKind.DWord:
                            _logger.Trace("Fetching int ...");
                            resultObject = (T)Convert.ChangeType(Marshal.ReadInt32(result), typeof(T));
                            break;
                        case RegistryValueKind.QWord:
                            _logger.Trace("Fetching long ...");
                            resultObject = (T)Convert.ChangeType(Marshal.ReadInt64(result), typeof(T));
                            break;

                        default:
                            throw new ArgumentOutOfRangeException(nameof(type), "Unsupported Reg type");
                    }

                    return resultObject;
                }
                throw new Win32Exception(error);
            }
            finally
            {
                if(result != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(result);
                }
            }
        }

        public WinRegistryKey CreateSubKey(string name)
        {
            _logger.Trace($"CreateSubKey({name})");
            return _winRegistryBase.InternalCreateOrOpenKey(
                Path.Combine(
                    Key,
                    name
                ),
                _hive
            );
        }

        public List<WinRegistryKey> GetSubKeys()
        {
            _logger.Trace("GetSubKeys()");
            var subKeys = new List<WinRegistryKey>();
            try
            {
                var classSize = 255u;
                var className = new StringBuilder((int)classSize);
                var subKeyCount = 0u;
                var error = RegQueryInfoKey(
                    RegPointer,
                    className,
                    ref classSize,
                    IntPtr.Zero,
                    ref subKeyCount,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero
                );
                if(error == 0)
                {
                    //var subKeyCount = Marshal.ReadInt32(subKeyCountAddress);
                    _logger.Trace($"Found {subKeyCount} subkeys");

                    if(subKeyCount > 0)
                    {
                        for(uint i = 0; i < subKeyCount; i++)
                        {
                            _logger.Trace($"Getting info for {i}");
                            var name = new StringBuilder((int)classSize);
                            error = RegEnumKeyEx(
                                RegPointer,
                                i,
                                name,
                                ref classSize,
                                IntPtr.Zero,
                                IntPtr.Zero,
                                IntPtr.Zero,
                                new System.Runtime.InteropServices.ComTypes.FILETIME()
                            );

                            if(error == 0)
                            {
                                var path = Path.Combine(Key, name.ToString());
                                try
                                {
                                    var key = _winRegistryBase.OpenKey(path);
                                    subKeys.Add(key);
                                }
                                catch(Win32Exception ex)
                                {
                                    _logger.Warn(ex, "Failed to open key");
                                }
                                catch(Exception ex)
                                {
                                    _logger.Error(ex, "Failed to open key");
                                }
                            }
                            else
                            {
                                var exception = new Win32Exception(error);
                                _logger.Warn(exception, $"Failed to get SubKey ({i})");
#if DEBUG
                                throw exception;
#endif
                            }
                        }
                    }
                }
                else
                {
                    var exception = new Win32Exception(error);
                    _logger.Error(exception, "Failed to get RegInfo");
                }
            }
            finally
            {

            }
            return subKeys;
        }

        public bool DeleteSubKey(string name)
        {
            _logger.Trace($"DeleteSubKey({name})");
            return _winRegistryBase.DeleteKey(this, name);
        }

        public void Dispose()
        {
            _winRegistryBase.CloseKey(this);
        }
    }
}
