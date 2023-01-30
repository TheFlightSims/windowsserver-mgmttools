using System;

namespace DeploymentToolkit.Actions.Utils
{
    [Flags]
    public enum ExitReason : uint
    {
        // ONE of the following:
        LogOff = 0x00,
        ShutDown = 0x01,
        Reboot = 0x02,
        PowerOff = 0x08,
        RestartApps = 0x40,
        // plus AT MOST ONE of the following two:
        Force = 0x04,
        ForceIfHung = 0x10,
    }
}
