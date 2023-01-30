namespace DeploymentToolkit.Modals.Settings.Install
{
    public class InstallSettings
    {
        public string CommandLine { get; set; }
        public string Parameters { get; set; }
        public DeferSettings DeferSettings { get; set; }
        public MSISettings MSISettings { get; set; }
        public CloseProgramsSettings CloseProgramsSettings { get; set; }
        public InstallerUninstallSettings UninstallSettings { get; set; }
        public RestartSettings RestartSettings { get; set; }
        public LogoffSettings LogoffSettings { get; set; }
        public ActiveSetupSettings ActiveSetupSettings { get; set; }
        public CustomActions CustomActions { get; set; }
    }
}
