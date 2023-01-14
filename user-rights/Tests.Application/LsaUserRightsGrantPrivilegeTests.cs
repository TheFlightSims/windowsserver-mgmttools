namespace Tests.Application;

using System;
using System.Security.Principal;
using UserRights.Application;
using Xunit;
using static Tests.PrivilegeConstants;
using static Tests.SecurityIdentifierConstants;

/// <summary>
/// Represents tests for <see cref="LsaUserRights"/> grant functionality.
/// </summary>
[Collection("lsa")]
public sealed class LsaUserRightsGrantPrivilegeTests : LsaUserRightsTestBase
{
    /// <summary>
    /// Tests granting a privilege.
    /// </summary>
    /// <remarks>
    /// We assume the BUILTIN\Users group is not granted the SeMachineAccountPrivilege privilege.
    /// </remarks>
    [AdminOnlyFact]
    public void GrantPrivilegeShouldWork()
    {
        var securityIdentifier = new SecurityIdentifier(Users);

        if (this.InitialState.TryGetValue(SeMachineAccountPrivilege, out var initial))
        {
            Assert.DoesNotContain(securityIdentifier, initial);
        }

        using var policy = new LsaUserRights();
        policy.Connect();
        policy.LsaAddAccountRights(securityIdentifier, SeMachineAccountPrivilege);

        var current = this.GetCurrentState();

        current.TryGetValue(SeMachineAccountPrivilege, out var collection);

        Assert.NotNull(collection);
        Assert.Contains(securityIdentifier, collection);
    }

    /// <summary>
    /// Tests granting a privilege without connecting throws an exception.
    /// </summary>
    [AdminOnlyFact]
    public void GrantPrivilegeWithoutConnectingThrowsException()
    {
        var securityIdentifier = new SecurityIdentifier(Users);

        using var policy = new LsaUserRights();

        Assert.Throws<InvalidOperationException>(() => policy.LsaAddAccountRights(securityIdentifier, SeMachineAccountPrivilege));
    }
}