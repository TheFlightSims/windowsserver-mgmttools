namespace DeploymentToolkit.Modals.Settings.Tray
{
    public interface IButtonSettings
    {
        public int? Height { get; set; }
        public int? Width { get; set; }
        public string FontColor { get; set; }
        public string FontWeight { get; set; }
        public string BackgroundColor { get; set; }
        public string BorderColor { get; set; }
    }
}
