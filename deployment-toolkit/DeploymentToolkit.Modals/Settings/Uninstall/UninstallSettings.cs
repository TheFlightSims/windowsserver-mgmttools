using DeploymentToolkit.Modals.Settings.Install;
using System.Xml.Serialization;

namespace DeploymentToolkit.Modals.Settings.Uninstall
{
    public class UninstallSettings
    {
        public string CommandLine { get; set; }
        public string Parameters { get; set; }
        public DeferSettings DeferSettings { get; set; }
        public MSISettings MSISettings { get; set; }
        public CloseProgramsSettings CloseProgramsSettings { get; set; }
        public RestartSettings RestartSettings { get; set; }
        public LogoffSettings LogoffSettings { get; set; }
        public ActiveSetupSettings ActiveSetupSettings { get; set; }
        public CustomActions CustomActions { get; set; }

        [XmlIgnore]
        public string LogFileSuffix { get; set; }
    }
}
