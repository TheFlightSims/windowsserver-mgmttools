using System.Xml.Serialization;

namespace DeploymentToolkit.Modals.Settings.Tray
{
    public class DefaultPageSettings : IPageSettings
    {
        [XmlElement(ElementName = "DefaultLogoPosition", IsNullable = true)]
        public Alignment? LogoPosition { get; set; } = Alignment.Right;
        [XmlElement(ElementName = "DefaultLogoAlignment", IsNullable = true)]
        public Alignment? LogoAlignment { get; set; } = Alignment.Automatic;
        [XmlElement(ElementName = "DefaultTitleAlignment", IsNullable = true)]
        public Alignment? TitleAlignment { get; set; } = Alignment.Left;

        public PageSettings UpgradeSchedule { get; set; }
        public PageSettings Restart { get; set; }
    }
}
