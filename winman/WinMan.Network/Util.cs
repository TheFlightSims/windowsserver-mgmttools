using System;
using System.Collections.Generic;
using System.Management;
using WinMan.Network.Models;

namespace WinMan.Network.Utils
{
    public class Util
    {
        public static List<NICModel> GetNetworkCards()
        {
            var rtnVal = new List<NICModel>();
            var opt = new ObjectGetOptions(null, TimeSpan.MaxValue, true);
            using (var manClass = new ManagementClass(@"\\.\root\cimv2", "Win32_PerfFormattedData_Tcpip_NetworkInterface", opt))
            {
                manClass.Scope.Options.EnablePrivileges = true;
                manClass.Scope.Options.Impersonation = ImpersonationLevel.Impersonate;
                foreach (ManagementObject item in manClass.GetInstances())
                {
                    var nic = new NICModel();
                    nic.BytesReceivedPersec = Convert.ToUInt64(item.GetPropertyValue("BytesReceivedPersec"));
                    nic.BytesSentPersec = Convert.ToUInt64(item.GetPropertyValue("BytesSentPersec"));
                    nic.BytesTotalPersec = Convert.ToUInt64(item.GetPropertyValue("BytesTotalPersec"));
                    nic.Caption = item.GetPropertyValue("Caption") == null ? "" : item.GetPropertyValue("Caption").ToString();
                    nic.CurrentBandwidth = Convert.ToUInt64(item.GetPropertyValue("CurrentBandwidth"));
                    nic.Description = item.GetPropertyValue("Description") == null ? "" : item.GetPropertyValue("Description").ToString();
                    nic.Name = item.GetPropertyValue("Name").ToString();
                    nic.PacketsPersec = Convert.ToUInt64(item.GetPropertyValue("PacketsPersec"));
                    nic.PacketsReceivedPersec = Convert.ToUInt64(item.GetPropertyValue("PacketsReceivedPersec"));
                    nic.PacketsSentPersec = Convert.ToUInt64(item.GetPropertyValue("PacketsSentPersec"));
                    rtnVal.Add(nic);
                }
            }
            return rtnVal;
        }
    }
}
