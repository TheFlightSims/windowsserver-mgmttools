using System;
using System.Management;
using WinMan.OS.Models;

namespace WinMan.OS.Utils
{
    public class Util
    {
        public static OsModel GetInfo()
        {
            var rtnVal = new OsModel();
            rtnVal.Memory = GetMemory();

            var manScope = new ManagementScope(@"\\.\root\cimv2");
            manScope.Options.EnablePrivileges = true;
            manScope.Connect();
            var query = new ObjectQuery("SELECT * FROM Win32_OperatingSystem");
            using (ManagementObjectSearcher mos = new ManagementObjectSearcher(manScope, query))
            {
                ManagementObjectCollection queryCollection = mos.Get();
                foreach (ManagementObject mo in queryCollection)
                {
                    rtnVal.MachineName = mo["CSName"].ToString();
                    rtnVal.OsName = mo["Caption"].ToString();
                    rtnVal.Architecture = mo["OSArchitecture"].ToString();
                    rtnVal.InstallDate = mo["InstallDate"].ToString();
                    rtnVal.LastBootupTime = mo["LastBootupTime"].ToString();
                }

            }

            query = new ObjectQuery("SELECT * FROM Win32_Processor");
            using (ManagementObjectSearcher mp = new ManagementObjectSearcher(manScope, query))
            {
                ManagementObjectCollection queryCollection = mp.Get();
                foreach (ManagementObject mo in queryCollection)
                {
                    rtnVal.Processor = mo["Name"].ToString();
                }
            }
            return rtnVal;
        }

        private static ulong GetMemory()
        {
            var opt = new ObjectGetOptions(null, TimeSpan.MaxValue, true);
            using (var manClass = new ManagementClass(@"\\.\root\cimv2", "Win32_PhysicalMemory", opt))
            {
                manClass.Scope.Options.EnablePrivileges = true;
                ulong rtnVal = 0;
                foreach (ManagementObject inst in manClass.GetInstances())
                {
                    rtnVal += Convert.ToUInt64(inst.GetPropertyValue("Capacity"));
                }
                return rtnVal;
            }
        }
    }
}
