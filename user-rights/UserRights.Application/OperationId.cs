namespace UserRights.Application;

/// <summary>
/// Represents operational Windows event ids.
/// </summary>
public static class OperationId
{
    /// <summary>
    /// Gets a value indicating the application is executing in privilege mode.
    /// </summary>
    public static int PrivilegeMode => 1001;

    /// <summary>
    /// Gets a value indicating the application is executing in principal mode.
    /// </summary>
    public static int PrincipalMode => 1002;

    /// <summary>
    /// Gets a value indicating the application is executing in list mode.
    /// </summary>
    public static int ListMode => 1003;

    /// <summary>
    /// Gets a value indicating a privilege was successfully granted.
    /// </summary>
    public static int PrivilegeGrantSuccess => 2001;

    /// <summary>
    /// Gets a value indicating a privilege has failed to be granted.
    /// </summary>
    public static int PrivilegeGrantFailure => 2002;

    /// <summary>
    /// Gets a value indicating a privilege is being granted in dryrun mode.
    /// </summary>
    public static int PrivilegeGrantDryrun => 2003;

    /// <summary>
    /// Gets a value indicating a privilege was successfully revoked.
    /// </summary>
    public static int PrivilegeRevokeSuccess => 3001;

    /// <summary>
    /// Gets a value indicating a privilege has failed to be revoked.
    /// </summary>
    public static int PrivilegeRevokeFailure => 3002;

    /// <summary>
    /// Gets a value indicating a privilege is being revoked in dryrun mode.
    /// </summary>
    public static int PrivilegeRevokeDryrun => 3003;

    /// <summary>
    /// Gets a value indicating a fatal error has occurred.
    /// </summary>
    public static int FatalError => 4001;

    /// <summary>
    /// Gets a value indicating a syntax error has occurred.
    /// </summary>
    public static int SyntaxError => 4002;
}