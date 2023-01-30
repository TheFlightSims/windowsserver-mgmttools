using DeploymentToolkit.ToolkitEnvironment;
using System;

namespace DeploymentToolkit.Unblocker
{
    class Program
    {
        private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            try
            {
                Logging.LogManager.Initialize("Unblocker");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to initialize logger: {ex}");
                Environment.Exit(-1);
            }

            ProcessManager.UnblockAllDTBlockedApps();
        }
    }
}
