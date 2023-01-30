using System;
using System.Runtime.InteropServices;

namespace DeploymentToolkit.Util
{
    public static class PowerUtil
    {
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int ExitWindowsEx(ExitReason uFlags, ShutdownReason dwReason);

        private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        private static bool ExitWindows(ExitReason exitWindows, ShutdownReason reason, bool ajustToken)
        {
            try
            {
                if(ajustToken && !TokenAdjuster.EnablePrivilege("SeShutdownPrivilege", true))
                {
                    return false;
                }
            }
            catch(Exception ex)
            {
                _logger.Fatal(ex, "Failed to adjust token");
                return false;
            }

            return ExitWindowsEx(exitWindows, reason) != 0;
        }

        public static bool Logoff()
        {
            return ExitWindows(ExitReason.LogOff | ExitReason.Force, ShutdownReason.MajorApplication | ShutdownReason.FlagPlanned, false);
        }

        public static bool Restart()
        {
            return ExitWindows(ExitReason.Reboot | ExitReason.Force, ShutdownReason.MajorApplication | ShutdownReason.FlagPlanned, true);
        }
    }
}
