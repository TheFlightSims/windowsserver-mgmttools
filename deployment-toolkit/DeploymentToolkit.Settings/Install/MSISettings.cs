namespace DeploymentToolkit.Deployment.Settings.Install
{
    public class MSISettings
    {
        public bool SuppressReboot { get; set; }
        public bool UseDefaultMSIParameters { get; set; }
        public bool SupressMSIRestartReturnCode { get; set; }
    }
}
