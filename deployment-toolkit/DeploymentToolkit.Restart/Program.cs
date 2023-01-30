using DeploymentToolkit.Util;
using System;
using System.Threading;

namespace DeploymentToolkit.Restart
{
    internal class Program
    {
        private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        private static void Main(string[] args)
        {
            try
            {
                Logging.LogManager.Initialize("Restart");
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Failed to initialize logger: {ex}");
                Environment.Exit(-1);
            }

            _logger.Info("Restarting in 10 Seconds ...");
            for(var i = 10; i != 0; i--)
            {
                _logger.Info($"{i} ...");
                Thread.Sleep(1000);
            }
            _logger.Info("Restarting ...");

            PowerUtil.Restart();
        }
    }
}
