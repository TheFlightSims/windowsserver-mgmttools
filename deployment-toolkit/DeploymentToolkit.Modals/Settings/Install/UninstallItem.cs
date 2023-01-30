using System.Xml.Serialization;

namespace DeploymentToolkit.Modals.Settings.Install
{
    [XmlRoot(ElementName = "Item")]
    public class UninstallItem
    {
        [XmlAttribute(AttributeName = "Exact")]
        public bool Exact { get; set; }
        [XmlText]
        public string Text { get; set; }
    }
}
