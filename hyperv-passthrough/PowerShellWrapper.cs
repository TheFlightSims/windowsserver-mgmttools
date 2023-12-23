using Microsoft.HyperV.PowerShell;
using Microsoft.Management.Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Windows.Forms;

namespace DiscreteDeviceAssigner
{
    class PowerShellWrapper
    {
        private static Collection<PSObject> RunScript(string scriptText)
        {
            using (Runspace runspace = RunspaceFactory.CreateRunspace())
            {
                runspace.Open();
                Pipeline pipeline = runspace.CreatePipeline();
                pipeline.Commands.AddScript(scriptText);
                return pipeline.Invoke();
            }
        }

        private static Collection<string> GetPnpDeviceLocationPath(string instanceId)
        {
            Collection<string> results = new Collection<string>();
            foreach (var dev in RunScript("Get-PnpDeviceProperty -InstanceId \"" + instanceId + "\" DEVPKEY_Device_LocationPaths"))
            {
                CimInstance ci = dev.BaseObject as CimInstance; if (ci == null) continue;
                var data = ci.CimInstanceProperties["Data"]; if (data == null) continue;
                var data2 = data.Value as IEnumerable<string>; if (data2 == null) continue;
                foreach (var d in data2)
                {
                    results.Add(d);
                }
            }
            return results;
        }

        public static Collection<VirtualMachine> GetVM()
        {
            Collection<VirtualMachine> results = new Collection<VirtualMachine>();
            foreach (var vm in RunScript("Get-VM"))
            {
                if (vm.BaseObject is VirtualMachine)
                {
                    results.Add(vm.BaseObject as VirtualMachine);
                }
            }
            return results;
        }

        public static Collection<VMAssignedDevice> GetVMAssignableDevice(VirtualMachine vm)
        {
            Collection<VMAssignedDevice> results = new Collection<VMAssignedDevice>();
            foreach (var vmad in RunScript("Get-VMAssignableDevice -VMName \"" + vm.Name + "\""))
            {
                if (vmad.BaseObject is VMAssignedDevice)
                {
                    results.Add(vmad.BaseObject as VMAssignedDevice);
                }
            }
            return results;
        }

        public static CimInstance GetPnpDevice(string instanceId)
        {
            foreach (var dev in RunScript("Get-PnpDevice -InstanceId \"" + instanceId + "\""))
            {
                if (dev.BaseObject is CimInstance)
                {
                    return dev.BaseObject as CimInstance;
                }
            }
            return null;
        }

        public static Collection<CimInstance> GetPnpDevice()
        {
            Collection<CimInstance> results = new Collection<CimInstance>();
            foreach (var dev in RunScript("Get-PnpDevice"))
            {
                if (dev.BaseObject is CimInstance)
                {
                    results.Add(dev.BaseObject as CimInstance);
                }
            }
            return results;
        }

        public static void SetGuestControlledCacheTypes(VirtualMachine vm, bool value)
        {
            if (value)
            {
                RunScript("Set-VM \"" + vm.Name + "\" -GuestControlledCacheTypes $true");
            }
            else
            {
                RunScript("Set-VM \"" + vm.Name + "\" -GuestControlledCacheTypes $false");
            }
        }

        public static void SetLowMemoryMappedIoSpace(VirtualMachine vm)
        {
            RunScript("Set-VM \"" + vm.Name + "\" -LowMemoryMappedIoSpace " + Convert.ToInt32("512") * 1024 * 1024);
        }

        public static void SetHighMemoryMappedIoSpace(VirtualMachine vm, uint bytes)
        {
            RunScript("Set-VM \"" + vm.Name + "\" -HighMemoryMappedIoSpace " + bytes);
        }

        public static void RemoveVMAssignableDevice(VirtualMachine vm, VMAssignedDevice device)
        {
            RunScript("Remove-VMAssignableDevice -LocationPath \"" + device.LocationPath + "\" -VMName \"" + vm.Name + "\"");
            try
            {
                RunScript("Mount-VmHostAssignableDevice -LocationPath \"" + device.LocationPath + "\"");
            }
            catch { }
            try
            {
                RunScript("Enable-PnpDevice -InstanceId \"" + device.InstanceID + "\" -Confirm:$false");
            }
            catch { }
        }

        public static void GpuPartitioning(VirtualMachine vm, CimInstance device)
        {
            string DeviceIDInstance = device.CimInstanceProperties["DeviceId"] != null ? device.CimInstanceProperties["DeviceId"].Value as string : null;
            var locationPath = GetPnpDeviceLocationPath(DeviceIDInstance)[0];
            
            //securing devices from crashing
            try 
            {
                RunScript("Set-VM -Name \"" + vm.Name + "\" -AutomaticStopAction TurnOff");
                RunScript("Set-VM -GuestControlledCacheTypes $true -VMName \"" + vm.Name + "\"");
                RunScript("Set-VM -LowMemoryMappedIoSpace 3Gb -VMName \"" + vm.Name + "\"");
                RunScript("Set-VM -HighMemoryMappedIoSpace 33280Mb -VMName \"" + vm.Name + "\"");
            } catch 
            {
                MessageBox.Show($"Setting VM is failed, failing the application"); //Securing the devices
                throw new InvalidOperationException("Operation failed! Raise function GpuPartitioning (line 129), with the debug DeviceIDInstance: " + DeviceIDInstance);
            }

            try
            {
                RunScript("Disable-PnpDevice -InstanceId \"" + DeviceIDInstance + "\" -Confirm:$false");
                RunScript("Dismount-VMHostAssignableDevice -Force -LocationPath \"" + locationPath + "\"");
                RunScript("Add-VMAssignableDevice -LocationPath \"" + locationPath + "\" -VMName \"" + vm.Name + "\"");
            } catch
            {
                MessageBox.Show($"Disable device is failed, failing the application"); //Securing the devices and VMs
                throw new InvalidOperationException("Operation failed! Raise function GpuPartitioning (line 129), with the debug locationPath: " + locationPath);
            }
        }

        public static void RemoveGpuPartitioning(VirtualMachine vm)
        {
            RunScript("Remove-VMGpuPartitionAdapter -VMName \"" + vm.Name + "\"");
        }

        public static void AddVMAssignableDevice(VirtualMachine vm, CimInstance device)
        {
            string id = device.CimInstanceProperties["DeviceId"] != null ? device.CimInstanceProperties["DeviceId"].Value as string : null;

            var locationPaths = GetPnpDeviceLocationPath(id);
            if (locationPaths.Count == 0) throw new InvalidOperationException("Cannot do action specified device to the VM");

            try
            {
                if (vm.AutomaticStopAction != StopAction.TurnOff)
                {
                    RunScript("Set-VM -AutomaticStopAction:TurnOff -VMName \"" + vm.Name + "\"");
                }
            }
            catch { }
            try
            {
                if (vm.DynamicMemoryEnabled && vm.MemoryStartup != vm.MemoryMinimum)
                {
                    RunScript("Set-VM -MemoryStartupBytes:" + vm.MemoryMinimum + " -VMName \"" + vm.Name + "\"");
                }
            }
            catch { }
            try
            {
                if (!vm.GuestControlledCacheTypes)
                {
                    SetGuestControlledCacheTypes(vm, true);
                }
            }
            catch { }

            try
            {
                RunScript("Disable-PnpDevice -InstanceId \"" + id + "\" -Confirm:$false");
            }
            catch { }
            try
            {
                RunScript("Dismount-VmHostAssignableDevice -LocationPath \"" + locationPaths[0] + "\" -force");
            }
            catch { }
            RunScript("Add-VMAssignableDevice -LocationPath \"" + locationPaths[0] + "\" -VMName \"" + vm.Name + "\"");
        }

        internal static void GpuPartitioning(VirtualMachine item1, VMAssignedDevice item2)
        {
            throw new NotImplementedException();
        }
    }
}
