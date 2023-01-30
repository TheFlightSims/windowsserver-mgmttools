namespace DeploymentToolkit.Modals.Settings.Tray
{
    public interface IPageWindowSettings
    {
        public string PageName { get; }
        public int? RequestedHeight { get; }
        public int? RequestedWidth { get; }
        public bool AllowMinimize { get; }
        public bool AllowClose { get; }
    }
}
