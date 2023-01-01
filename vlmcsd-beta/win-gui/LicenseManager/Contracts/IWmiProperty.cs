using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace HGM.Hotbird64.LicenseManager.Contracts
{
    public interface IWmiProperty : INotifyPropertyChanged
    {
        ManagementObject Property { get; }
        bool ShowAllFields { get; }
        bool DeveloperMode { get; }
        LicenseMachine.LicenseProvider LicenseProvider { get; set; }
    }
}
