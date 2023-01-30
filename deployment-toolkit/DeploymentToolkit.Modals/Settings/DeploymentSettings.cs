namespace DeploymentToolkit.Modals.Settings
{
    public class DeploymentSettings
    {
        public class MSISettings
        {
            public string DefaultSilentParameters { get; set; } = "/qn";
            public string DefaultInstallParameters { get; set; } = "/qn";
            public string DefaultUninstallParameters { get; set; } = "/qn";

            public string DefaultLoggingParameters { get; set; } = "/L*v";

            public string ActiveSetupParameters { get; set; } = "/fcu";
        }

        public MSISettings MSI = new MSISettings();
    }
}
