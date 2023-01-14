namespace Tests.Application;

using System;
using System.Linq;
using System.Security.Principal;
using UserRights.Application;
using Xunit;
using static Tests.PrivilegeConstants;
using static Tests.SecurityIdentifierConstants;

/// <summary>
/// Represents tests for <see cref="LsaUserRights"/> list functionality.
/// </summary>
[Collection("lsa")]
public sealed class LsaUserRightsGetPrincipalsTests : LsaUserRightsTestBase
{
    /// <summary>
    /// Tests listing all the principals assigned to all privileges.
    /// </summary>
    [AdminOnlyFact]
    public void GetPrincipalsShouldWork()
    {
        var expected = this.InitialState.Values
            .SelectMany(p => p)
            .Distinct()
            .OrderBy(p => p)
            .ToArray();

        using var policy = new LsaUserRights();
        policy.Connect();

        var actual = policy.LsaEnumerateAccountsWithUserRight()
            .OrderBy(p => p)
            .ToArray();

        Assert.Equal(expected, actual);
    }

    /// <summary>
    /// Tests listing the principals assigned to a single privilege.
    /// </summary>
    /// <remarks>
    /// We assume the BUILTIN\Administrators group is granted the SeTakeOwnershipPrivilege privilege.
    /// </remarks>
    [AdminOnlyFact]
    public void GetPrincipalsSinglePrivilegeShouldWork()
    {
        var securityIdentifier = new SecurityIdentifier(Administrators);

        using var policy = new LsaUserRights();
        policy.Connect();

        var collection = policy.LsaEnumerateAccountsWithUserRight(SeTakeOwnershipPrivilege).ToArray();

        Assert.Contains(securityIdentifier, collection);
    }

    /// <summary>
    /// Tests listing all the principals assigned to all privileges without connecting throws an exception.
    /// </summary>
    [AdminOnlyFact]
    public void GetPrincipalsWithoutConnectingThrowsException()
    {
        using var policy = new LsaUserRights();

        Assert.Throws<InvalidOperationException>(() => policy.LsaEnumerateAccountsWithUserRight().ToArray());
    }

    /// <summary>
    /// Tests listing the principals assigned to a single privilege without connecting throws an exception.
    /// </summary>
    [AdminOnlyFact]
    public void GetPrincipalsSinglePrivilegeWithoutConnectingThrowsException()
    {
        using var policy = new LsaUserRights();

        Assert.Throws<InvalidOperationException>(() => policy.LsaEnumerateAccountsWithUserRight(SeTakeOwnershipPrivilege).ToArray());
    }
}