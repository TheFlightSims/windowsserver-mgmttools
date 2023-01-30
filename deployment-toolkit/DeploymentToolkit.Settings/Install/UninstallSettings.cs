using System.Xml.Serialization;

namespace DeploymentToolkit.Deployment.Settings.Install
{
    public class UninstallSettings
    {
        public bool IgnoreUninstallErrors { get; set; }
        [XmlArrayItemAttribute("Item", IsNullable = false)]
        public string[] Uninstall { get; set; }
    }
}
