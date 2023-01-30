using System.Xml.Serialization;

namespace DeploymentToolkit.Deployment.Settings
{
    public class CloseProgramsSettings
    {
        public int TimeUntilForcedClose { get; set; }
        public bool DisableStartDuringInstallation { get; set; }
        [XmlArrayItem("Item", IsNullable = false)]
        public string[] Close { get; set; } = new string[0];
    }
}
