namespace Tests;

using System;
using System.Security.Principal;

/// <summary>
/// Represents test data.
/// </summary>
public static class TestData
{
    /// <summary>
    /// Represents the allow log on locally security policy setting.
    /// </summary>
    public static readonly string Privilege1 = "SeInteractiveLogonRight";

    /// <summary>
    /// Represents the log on as a service security policy setting.
    /// </summary>
    public static readonly string Privilege2 = "SeServiceLogonRight";

    /// <summary>
    /// Represents the security identity for the currently logged on user.
    /// </summary>
    public static readonly SecurityIdentifier PrincipalSidCurrent = WindowsIdentity.GetCurrent().User ?? throw new ArgumentNullException();

    /// <summary>
    /// Represents the default local Administrator account.
    /// </summary>
    public static readonly string PrincipalName1 = "BUILTIN\\Administrators";

    /// <summary>
    /// Represents the security identity for the default local Administrator account.
    /// </summary>
    public static readonly SecurityIdentifier PrincipalSid1 = new("S-1-5-32-544");

    /// <summary>
    /// Represents the builtin group containing only the <b>Authenticated Users</b> group by default.
    /// </summary>
    public static readonly string PrincipalName2 = "BUILTIN\\Users";

    /// <summary>
    /// Represents the security identity for the built in local users group.
    /// </summary>
    public static readonly SecurityIdentifier PrincipalSid2 = new("S-1-5-32-545");

    /// <summary>
    /// Represents the builtin group containing only the <b>Guest</b> user by default.
    /// </summary>
    public static readonly string PrincipalName3 = "BUILTIN\\Guests";

    /// <summary>
    /// Represents the security identity for the built in local guests group.
    /// </summary>
    public static readonly SecurityIdentifier PrincipalSid3 = new("S-1-5-32-546");
}