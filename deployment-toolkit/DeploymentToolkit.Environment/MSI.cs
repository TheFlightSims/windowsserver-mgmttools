using DeploymentToolkit.Modals;
using Microsoft.Deployment.WindowsInstaller;
using System.IO;

namespace DeploymentToolkit.ToolkitEnvironment
{
    public static class MSI
    {
        public const string SuppressReboot = "/norestart REBOOT=ReallySuppress";

        public static string DefaultSilentParameters = "/qn";
        public static string DefaultInstallParameters = "/qn";
        public static string DefaultUninstallParameters = "/qn";

        public static string DefaultLoggingParameters = "/L*v";

        public static string ActiveSetupParameters = "/fcu";
        public const string ActiveSetupPath = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Active Setup\Installed Components";
        public const string ActiveSetupUserPath = @"HKEY_USERS\{0}\SOFTWARE\Microsoft\Active Setup\Installed Components";

        private static string _productCode;
        public static string ProductCode
        {
            get
            {
                if(string.IsNullOrEmpty(_productCode))
                {
                    ReadMSI();
                }

                return _productCode;
            }
            private set
            {
                _logger.Trace($"Product Code set from '{_productCode}' to '{value}'");
                _productCode = value;
            }
        }

        private static string _productVersion;
        public static string ProductVersion
        {
            get
            {
                if(string.IsNullOrEmpty(_productVersion))
                {
                    ReadMSI();
                }

                return _productVersion;
            }
            private set
            {
                _logger.Trace($"ProductVersion set from '{_productVersion}' to '{value}'");
                _productVersion = value;
            }
        }

        private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        private static void ReadMSI()
        {
            if(EnvironmentVariables.ActiveSequenceType == SequenceType.Installation)
            {
                ReadMSI(EnvironmentVariables.InstallSettings.CommandLine);
            }
            else
            {
                ReadMSI(EnvironmentVariables.UninstallSettings.CommandLine);
            }
        }

        public static void ReadMSI(string msiPath)
        {
            if(!File.Exists(msiPath))
            {
                throw new FileNotFoundException(nameof(msiPath));
            }

            _logger.Trace($"Reading msi properties from {msiPath}");

            using(var database = new Database(msiPath, DatabaseOpenMode.ReadOnly))
            {
                _logger.Debug($"[{database.SummaryInfo.RevisionNumber}]{database.SummaryInfo.Title} from {database.SummaryInfo.Author} successfully opened");

                ProductCode = database.ExecutePropertyQuery("ProductCode");
                ProductVersion = database.ExecutePropertyQuery("ProductVersion");
            }
        }
    }
}
