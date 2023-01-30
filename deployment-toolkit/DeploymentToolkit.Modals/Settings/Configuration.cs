using DeploymentToolkit.Modals.Settings.Install;
using DeploymentToolkit.Modals.Settings.Uninstall;

namespace DeploymentToolkit.Modals.Settings
{
    public class Configuration
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public DisplaySettings DisplaySettings { get; set; }
        public CustomVariables CustomVariables { get; set; }
        public InstallSettings InstallSettings { get; set; }
        public UninstallSettings UninstallSettings { get; set; }
    }
}
