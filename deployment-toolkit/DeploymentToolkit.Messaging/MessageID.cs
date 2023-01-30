namespace DeploymentToolkit.Messaging
{
    public enum MessageId : byte
    {
        // Client = TrayApp
        // Server = DeploymentToolkit executable
        // IFEObl = Image File Execution Options Blocker

        // Server <-  Client
        InitialConnectMessage,
        // Server  -> Client
        DeploymentInformationMessage,

        // Server  -> Client
        DeploymentStarted,
        // Server  -> Client
        DeploymentSuccess,
        // Server  -> Client
        DeploymentError,
        // Server  -> Client
        DeploymentRestart,
        // Server  -> Client
        DeploymentLogoff,

        // IFEObl  -> Client
        ExecutionBlocked,

        // Server  -> Client
        ShowBalloonTip,

        // Server <-  Client
        ContinueDeployment,
        AbortDeployment,

        // Server  -> Client
        DeferDeployment,
        // Server  -> Client
        CloseApplications
    }
}
