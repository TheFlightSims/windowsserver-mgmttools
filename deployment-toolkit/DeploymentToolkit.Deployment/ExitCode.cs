namespace DeploymentToolkit.Deployment
{
    internal enum ExitCode
    {
        ExitOK = 0,
        FailedToInitilizeLogger = -1,
        SettingsFileMissing = -2,
        FailedToReadSettings = -3,
        MissingRequiredParameter = -4,
        DeploymentToolkitInstallPathNotFound = -5,
        InstallFileMissing = -6,
        UninstallFileMissing = -7,
        NotElevated = -8,

        InvalidCommandLineSpecified = -10,
        ErrorDuringInstallation = -11,
        ErrorDuringUninstallation = -12
    }
}
