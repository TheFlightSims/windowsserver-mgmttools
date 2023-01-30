using System.Xml.Serialization;

namespace DeploymentToolkit.Modals.Settings.Tray
{
    public class DefaultTextBlockSettings : ITextBlockSettings
    {
        [XmlElement(ElementName = "DefaultFontColor", IsNullable = true)]
        public string FontColor { get; set; } = "Black";
        [XmlElement(ElementName = "DefaultFontWeight", IsNullable = true)]
        public string FontWeight { get; set; } = "Normal";
        [XmlElement(ElementName = "DefaultBackgroundColor", IsNullable = true)]
        public string BackgroundColor { get; set; } = "White";

        public TextBlockSettings DateTimePickerTitle { get; set; }
        public TextBlockSettings DateTimePickerReminder { get; set; }

        public TextBlockSettings RestartTopTextBlock { get; set; }
        public TextBlockSettings RestartMiddleTextBlock { get; set; }
        public TextBlockSettings RestartBottomTextBlock { get; set; }

        public TextBlockSettings DeferalTopTextBlock { get; set; }
        public TextBlockSettings DeferalMiddleTextBlock { get; set; }
        public TextBlockSettings DeferalBottomTextBlock { get; set; }

        public TextBlockSettings CloseApplicationsTopTextBlock { get; set; }
        public TextBlockSettings CloseApplicationsBottomTextBlock { get; set; }
    }
}
