using System.Collections.Generic;
using System.Xml.Serialization;

namespace DeploymentToolkit.Modals.Settings.Install
{
    [XmlRoot(ElementName = "UninstallSettings")]
    public class InstallerUninstallSettings
    {
        public bool IgnoreUninstallErrors { get; set; }
        [XmlArrayItem("Item", IsNullable = false)]
        public List<UninstallItem> Uninstall { get; set; }
    }
}
