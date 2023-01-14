namespace Tests.Application;

using System;
using System.Security.Principal;
using UserRights.Application;
using Xunit;
using static Tests.PrivilegeConstants;
using static Tests.SecurityIdentifierConstants;

/// <summary>
/// Represents tests for <see cref="LsaUserRights"/> revoke functionality.
/// </summary>
[Collection("lsa")]
public sealed class LsaUserRightsRevokePrivilegeTests : LsaUserRightsTestBase
{
    /// <summary>
    /// Tests revoking a privilege.
    /// </summary>
    /// <remarks>
    /// We assume the BUILTIN\Backup Operators is granted the SeBackupPrivilege privilege.
    /// </remarks>
    [AdminOnlyFact]
    public void RevokePrivilegeShouldWork()
    {
        var securityIdentifier = new SecurityIdentifier(BackupOperators);

        this.InitialState.TryGetValue(SeBackupPrivilege, out var initial);

        Assert.NotNull(initial);
        Assert.Contains(securityIdentifier, initial);

        using var policy = new LsaUserRights();
        policy.Connect();
        policy.LsaRemoveAccountRights(securityIdentifier, SeBackupPrivilege);

        var current = this.GetCurrentState();

        if (current.TryGetValue(SeBackupPrivilege, out var collection))
        {
            Assert.DoesNotContain(securityIdentifier, collection);
        }
    }

    /// <summary>
    /// Tests revoking a privilege without connecting throws an exception.
    /// </summary>
    [AdminOnlyFact]
    public void RevokePrivilegeWithoutConnectingThrowsException()
    {
        var securityIdentifier = new SecurityIdentifier(BackupOperators);

        using var policy = new LsaUserRights();

        Assert.Throws<InvalidOperationException>(() => policy.LsaRemoveAccountRights(securityIdentifier, SeBackupPrivilege));
    }
}