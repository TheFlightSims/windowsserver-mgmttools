using System.Xml.Serialization;

namespace DeploymentToolkit.Modals.Settings.Tray
{
    public class ButtonSettings : IButtonSettings
    {
        [XmlElement(IsNullable = true)]
        public int? Height { get; set; }
        [XmlElement(IsNullable = true)]
        public int? Width { get; set; }
        public string FontColor { get; set; }
        public string FontWeight { get; set; }
        public string BackgroundColor { get; set; }
        public string BorderColor { get; set; }
    }
}
