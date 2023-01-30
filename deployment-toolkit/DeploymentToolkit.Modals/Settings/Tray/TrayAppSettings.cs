namespace DeploymentToolkit.Modals.Settings.Tray
{


    public class TrayAppSettings
    {
        public bool EnableAppList { get; set; }
        public string BackgroundColor { get; set; } = string.Empty;
        public string ForegroundColor { get; set; } = string.Empty;

        public DefaultPageSettings PageSettings { get; set; } = new();
        public BrandingSettings BrandingSettings { get; set; } = new();
    }
}
