using System.Diagnostics;

namespace DeploymentToolkit.Util
{
    public static class WindowsEventLog
    {
        private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        private static EventLog _eventLog;

        public static void Ensure()
        {
            if(!EventLog.SourceExists("DeploymentToolkit"))
            {
                _logger.Trace("Source does not exist. Creating ...");
                EventLog.CreateEventSource("DeploymentToolkit", "DeploymentToolkit");
            }

            _eventLog = new EventLog("DeploymentToolkit")
            {
                Source = "Deployment"
            };
        }

        public static void RequestTrayAppStart()
        {
            _logger.Trace("Requesting start of all tray apps ...");
            _eventLog.WriteEntry("Tray App start request", EventLogEntryType.Information, 42, 42);
        }
    }
}
