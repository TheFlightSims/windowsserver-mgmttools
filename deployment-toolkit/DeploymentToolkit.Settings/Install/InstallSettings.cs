using DeploymentToolkit.Settings;

namespace DeploymentToolkit.Deployment.Settings.Install
{
    public class InstallSettings
    {
        public string CommandLine { get; set; }
        public string Parameters { get; set; }
        public DeferSettings DeferSettings { get; set; }
        public MSISettings MSISettings { get; set; }
        public CloseProgramsSettings CloseProgramsSettings { get; set; }
        public UninstallSettings UninstallSettings { get; set; }
        public RestartSettings RestartSettings { get; set; }
        public LogoffSettings LogoffSettings { get; set; }
        public ActiveSetupSettings ActiveSetupSettings { get; set; }
        public CustomActions CustomActions { get; set; }
    }
}
