using DeploymentToolkit.Deployment.Settings.Install;

namespace DeploymentToolkit.Deployment.Settings
{
    public class Configuration
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public DisplaySettings DisplaySettings { get; set; }
        public InstallSettings InstallSettings { get; set; }
    }
}
