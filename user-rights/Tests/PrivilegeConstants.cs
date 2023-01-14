namespace Tests;

/// <summary>
/// Represents Windows user right privileges.
/// </summary>
public static class PrivilegeConstants
{
    /// <summary>
    /// Replace a process-level token.
    /// </summary>
    public const string SeAssignPrimaryTokenPrivilege = "SeAssignPrimaryTokenPrivilege";

    /// <summary>
    /// Generate security audits.
    /// </summary>
    public const string SeAuditPrivilege = "SeAuditPrivilege";

    /// <summary>
    /// Back up files and directories.
    /// </summary>
    public const string SeBackupPrivilege = "SeBackupPrivilege";

    /// <summary>
    /// Required for an account to log on using the batch logon type.
    /// </summary>
    public const string SeBatchLogonRight = "SeBatchLogonRight";

    /// <summary>
    /// Bypass traverse checking.
    /// </summary>
    public const string SeChangeNotifyPrivilege = "SeChangeNotifyPrivilege";

    /// <summary>
    /// Create global objects.
    /// </summary>
    public const string SeCreateGlobalPrivilege = "SeCreateGlobalPrivilege";

    /// <summary>
    /// Create a pagefile.
    /// </summary>
    public const string SeCreatePagefilePrivilege = "SeCreatePagefilePrivilege";

    /// <summary>
    /// Create permanent shared objects.
    /// </summary>
    public const string SeCreatePermanentPrivilege = "SeCreatePermanentPrivilege";

    /// <summary>
    /// Create symbolic links.
    /// </summary>
    public const string SeCreateSymbolicLinkPrivilege = "SeCreateSymbolicLinkPrivilege";

    /// <summary>
    /// Create a token object.
    /// </summary>
    public const string SeCreateTokenPrivilege = "SeCreateTokenPrivilege";

    /// <summary>
    /// Debug programs.
    /// </summary>
    public const string SeDebugPrivilege = "SeDebugPrivilege";

    /// <summary>
    /// Impersonate other users.
    /// </summary>
    public const string SeDelegateSessionUserImpersonatePrivilege = "SeDelegateSessionUserImpersonatePrivilege";

    /// <summary>
    /// Explicitly denies an account the right to log on using the batch logon type.
    /// </summary>
    public const string SeDenyBatchLogonRight = "SeDenyBatchLogonRight";

    /// <summary>
    /// Explicitly denies an account the right to log on using the interactive logon type.
    /// </summary>
    public const string SeDenyInteractiveLogonRight = "SeDenyInteractiveLogonRight";

    /// <summary>
    /// Explicitly denies an account the right to log on using the network logon type.
    /// </summary>
    public const string SeDenyNetworkLogonRight = "SeDenyNetworkLogonRight";

    /// <summary>
    /// Explicitly denies an account the right to log on remotely using the interactive logon type.
    /// </summary>
    public const string SeDenyRemoteInteractiveLogonRight = "SeDenyRemoteInteractiveLogonRight";

    /// <summary>
    /// Explicitly denies an account the right to log on using the service logon type.
    /// </summary>
    public const string SeDenyServiceLogonRight = "SeDenyServiceLogonRight";

    /// <summary>
    /// Enable computer and user accounts to be trusted for delegation.
    /// </summary>
    public const string SeEnableDelegationPrivilege = "SeEnableDelegationPrivilege";

    /// <summary>
    /// Impersonate a client after authentication.
    /// </summary>
    public const string SeImpersonatePrivilege = "SeImpersonatePrivilege";

    /// <summary>
    /// Increase scheduling priority.
    /// </summary>
    public const string SeIncreaseBasePriorityPrivilege = "SeIncreaseBasePriorityPrivilege";

    /// <summary>
    /// Adjust memory quotas for a process.
    /// </summary>
    public const string SeIncreaseQuotaPrivilege = "SeIncreaseQuotaPrivilege";

    /// <summary>
    /// Increase a process working set.
    /// </summary>
    public const string SeIncreaseWorkingSetPrivilege = "SeIncreaseWorkingSetPrivilege";

    /// <summary>
    /// Required for an account to log on using the interactive logon type.
    /// </summary>
    public const string SeInteractiveLogonRight = "SeInteractiveLogonRight";

    /// <summary>
    /// Load and unload device drivers.
    /// </summary>
    public const string SeLoadDriverPrivilege = "SeLoadDriverPrivilege";

    /// <summary>
    /// Lock pages in memory.
    /// </summary>
    public const string SeLockMemoryPrivilege = "SeLockMemoryPrivilege";

    /// <summary>
    /// Add workstations to domain.
    /// </summary>
    public const string SeMachineAccountPrivilege = "SeMachineAccountPrivilege";

    /// <summary>
    /// Manage the files on a volume.
    /// </summary>
    public const string SeManageVolumePrivilege = "SeManageVolumePrivilege";

    /// <summary>
    /// Required for an account to log on using the network logon type.
    /// </summary>
    public const string SeNetworkLogonRight = "SeNetworkLogonRight";

    /// <summary>
    /// Profile single process.
    /// </summary>
    public const string SeProfileSingleProcessPrivilege = "SeProfileSingleProcessPrivilege";

    /// <summary>
    /// Modify an object label.
    /// </summary>
    public const string SeRelabelPrivilege = "SeRelabelPrivilege";

    /// <summary>
    /// Required for an account to log on remotely using the interactive logon type.
    /// </summary>
    public const string SeRemoteInteractiveLogonRight = "SeRemoteInteractiveLogonRight";

    /// <summary>
    /// Force shutdown from a remote system.
    /// </summary>
    public const string SeRemoteShutdownPrivilege = "SeRemoteShutdownPrivilege";

    /// <summary>
    /// Restore files and directories.
    /// </summary>
    public const string SeRestorePrivilege = "SeRestorePrivilege";

    /// <summary>
    /// Manage auditing and security log.
    /// </summary>
    public const string SeSecurityPrivilege = "SeSecurityPrivilege";

    /// <summary>
    /// Required for an account to log on using the service logon type.
    /// </summary>
    public const string SeServiceLogonRight = "SeServiceLogonRight";

    /// <summary>
    /// Shut down the system.
    /// </summary>
    public const string SeShutdownPrivilege = "SeShutdownPrivilege";

    /// <summary>
    /// Synchronize directory service data.
    /// </summary>
    public const string SeSyncAgentPrivilege = "SeSyncAgentPrivilege";

    /// <summary>
    /// Modify firmware environment values.
    /// </summary>
    public const string SeSystemEnvironmentPrivilege = "SeSystemEnvironmentPrivilege";

    /// <summary>
    /// Profile system performance.
    /// </summary>
    public const string SeSystemProfilePrivilege = "SeSystemProfilePrivilege";

    /// <summary>
    /// Change the system time.
    /// </summary>
    public const string SeSystemtimePrivilege = "SeSystemtimePrivilege";

    /// <summary>
    /// Take ownership of files or other objects.
    /// </summary>
    public const string SeTakeOwnershipPrivilege = "SeTakeOwnershipPrivilege";

    /// <summary>
    /// Act as part of the operating system.
    /// </summary>
    public const string SeTcbPrivilege = "SeTcbPrivilege";

    /// <summary>
    /// Change the time zone.
    /// </summary>
    public const string SeTimeZonePrivilege = "SeTimeZonePrivilege";

    /// <summary>
    /// Access Credential Manager as a trusted caller.
    /// </summary>
    public const string SeTrustedCredManAccessPrivilege = "SeTrustedCredManAccessPrivilege";

    /// <summary>
    /// Remove computer from docking station.
    /// </summary>
    public const string SeUndockPrivilege = "SeUndockPrivilege";
}