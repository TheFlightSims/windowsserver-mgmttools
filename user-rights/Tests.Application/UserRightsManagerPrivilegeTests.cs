namespace Tests.Application;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text.RegularExpressions;
using Microsoft.Extensions.DependencyInjection;
using UserRights.Application;
using Xunit;
using static Tests.TestData;

/// <summary>
/// Represents integration tests for modify privilege functionality.
/// </summary>
public sealed class UserRightsManagerPrivilegeTests : UserRightsManagerTestBase
{
    /// <summary>
    /// Generates invalid method arguments for the <see cref="IUserRightsManager.ModifyPrivilege"/> method.
    /// </summary>
    /// <returns>A sequence of method arguments.</returns>
    public static IEnumerable<object[]> InvalidArguments()
    {
        var policy = new MockLsaUserRights();
        var pattern = new Regex(".*", RegexOptions.None, TimeSpan.FromSeconds(1));

        // Verify null policy instance.
        yield return new object[] { null!, Privilege1, new[] { PrincipalName1 }, Array.Empty<string>(), false, false, null!, false };

        // Verify null or empty privilege.
        yield return new object[] { policy, null!, new[] { PrincipalName1 }, Array.Empty<string>(), false, false, null!, false };
        yield return new object[] { policy, string.Empty, new[] { PrincipalName1 }, Array.Empty<string>(), false, false, null!, false };

        // Verify null grant collection.
        yield return new object[] { policy, Privilege1, null!, new[] { PrincipalName1 }, false, false, null!, false };

        // Verify null revocation collection.
        yield return new object[] { policy, Privilege1, new[] { PrincipalName1 }, null!, false, false, null!, false };

        // Verify RevokeAll requirements.
        yield return new object[] { policy, Privilege1, new[] { PrincipalName1 }, Array.Empty<string>(), true, false, null!, false };
        yield return new object[] { policy, Privilege1, Array.Empty<string>(), new[] { PrincipalName1 }, true, false, null!, false };
        yield return new object[] { policy, Privilege1, Array.Empty<string>(), Array.Empty<string>(), true, true, null!, false };
        yield return new object[] { policy, Privilege1, Array.Empty<string>(), Array.Empty<string>(), true, false, pattern, false };

        // Verify RevokeOthers requirements.
        yield return new object[] { policy, Privilege1, Array.Empty<string>(), Array.Empty<string>(), false, true, null!, false };
        yield return new object[] { policy, Privilege1, new[] { PrincipalName1 }, new[] { PrincipalName2 }, false, true, null!, false };
        yield return new object[] { policy, Privilege1, Array.Empty<string>(), Array.Empty<string>(), true, true, null!, false };
        yield return new object[] { policy, Privilege1, Array.Empty<string>(), Array.Empty<string>(), false, true, pattern, false };

        // Verify RevokePattern requirements.
        yield return new object[] { policy, Privilege1, Array.Empty<string>(), new[] { PrincipalName1 }, false, false, pattern, false };
        yield return new object[] { policy, Privilege1, Array.Empty<string>(), Array.Empty<string>(), true, false, pattern, false };
        yield return new object[] { policy, Privilege1, Array.Empty<string>(), Array.Empty<string>(), false, true, pattern, false };

        // Verify remaining requirements.
        yield return new object[] { policy, Privilege1, Array.Empty<string>(), Array.Empty<string>(), false, false, null!, false };

        // Verify grant and revocation set restrictions.
        yield return new object[] { policy, Privilege1, new[] { PrincipalName1 }, new[] { PrincipalName1 }, false, false, null!, false };
        yield return new object[] { policy, Privilege1, new[] { PrincipalName1, PrincipalName1 }, Array.Empty<string>(), false, false, null!, false };
        yield return new object[] { policy, Privilege1, Array.Empty<string>(), new[] { PrincipalName1, PrincipalName1 }, false, false, null!, false };
    }

    /// <summary>
    /// Verifies granting a principal to a privilege and revoking its other principals is successful and does not modify other assignments.
    /// </summary>
    [Fact]
    public void GrantAndRevokeOthersShouldWork()
    {
        var principals1 = new HashSet<SecurityIdentifier>
        {
            PrincipalSid1,
            PrincipalSid2
        };

        var principals2 = new HashSet<SecurityIdentifier>
        {
            PrincipalSid2
        };

        var database = new Dictionary<string, ICollection<SecurityIdentifier>>(StringComparer.Ordinal)
        {
            { Privilege1, principals1 },
            { Privilege2, principals2 }
        };

        var policy = new MockLsaUserRights(database);
        policy.Connect("SystemName");

        Assert.Equal(new[] { PrincipalSid1, PrincipalSid2 }.OrderBy(p => p), policy.LsaEnumerateAccountsWithUserRight().OrderBy(p => p));
        Assert.Equal(new[] { Privilege1 }, policy.LsaEnumerateAccountRights(PrincipalSid1));
        Assert.Equal(new[] { Privilege1, Privilege2 }.OrderBy(p => p), policy.LsaEnumerateAccountRights(PrincipalSid2).OrderBy(p => p));
        Assert.Equal(new[] { PrincipalSid1, PrincipalSid2 }.OrderBy(p => p), policy.LsaEnumerateAccountsWithUserRight(Privilege1).OrderBy(p => p));
        Assert.Equal(new[] { PrincipalSid2 }, policy.LsaEnumerateAccountsWithUserRight(Privilege2));

        var manager = this.ServiceProvider.GetRequiredService<IUserRightsManager>();
        manager.ModifyPrivilege(policy, Privilege2, new[] { PrincipalName1 }, Array.Empty<string>(), false, true, null!, false);

        Assert.Equal(new[] { PrincipalSid1, PrincipalSid2 }.OrderBy(p => p), policy.LsaEnumerateAccountsWithUserRight().OrderBy(p => p));
        Assert.Equal(new[] { Privilege1, Privilege2 }.OrderBy(p => p), policy.LsaEnumerateAccountRights(PrincipalSid1).OrderBy(p => p));
        Assert.Equal(new[] { Privilege1 }, policy.LsaEnumerateAccountRights(PrincipalSid2));
        Assert.Equal(new[] { PrincipalSid1, PrincipalSid2 }.OrderBy(p => p), policy.LsaEnumerateAccountsWithUserRight(Privilege1).OrderBy(p => p));
        Assert.Equal(new[] { PrincipalSid1 }, policy.LsaEnumerateAccountsWithUserRight(Privilege2));
    }

    /// <summary>
    /// Verifies a single grant with a single revoke is successful and does not modify other assignments.
    /// </summary>
    [Fact]
    public void GrantAndRevokeShouldWork()
    {
        var principals1 = new HashSet<SecurityIdentifier>
        {
            PrincipalSid1
        };

        var principals2 = new HashSet<SecurityIdentifier>
        {
            PrincipalSid1,
            PrincipalSid2
        };

        var database = new Dictionary<string, ICollection<SecurityIdentifier>>(StringComparer.Ordinal)
        {
            { Privilege1, principals1 },
            { Privilege2, principals2 }
        };

        var policy = new MockLsaUserRights(database);
        policy.Connect("SystemName");

        Assert.Equal(new[] { PrincipalSid1, PrincipalSid2 }.OrderBy(p => p), policy.LsaEnumerateAccountsWithUserRight().OrderBy(p => p));
        Assert.Equal(new[] { Privilege1, Privilege2 }.OrderBy(p => p), policy.LsaEnumerateAccountRights(PrincipalSid1).OrderBy(p => p));
        Assert.Equal(new[] { Privilege2 }, policy.LsaEnumerateAccountRights(PrincipalSid2));

        var manager = this.ServiceProvider.GetRequiredService<IUserRightsManager>();
        manager.ModifyPrivilege(policy, Privilege1, new[] { PrincipalName2 }, new[] { PrincipalName1 }, false, false, null!, false);

        Assert.Equal(new[] { PrincipalSid1, PrincipalSid2 }.OrderBy(p => p), policy.LsaEnumerateAccountsWithUserRight().OrderBy(p => p));
        Assert.Equal(new[] { Privilege2 }, policy.LsaEnumerateAccountRights(PrincipalSid1));
        Assert.Equal(new[] { Privilege1, Privilege2 }.OrderBy(p => p), policy.LsaEnumerateAccountRights(PrincipalSid2).OrderBy(p => p));
    }

    /// <summary>
    /// Verifies granting a principal to a privilege and revoking all principals matching a pattern is successful and does not modify other assignments.
    /// </summary>
    [Fact]
    public void GrantAndRevokePatternShouldWork()
    {
        var principals1 = new HashSet<SecurityIdentifier>
        {
            PrincipalSidCurrent,
            PrincipalSid2,
            PrincipalSid3
        };

        var principals2 = new HashSet<SecurityIdentifier>
        {
            PrincipalSid1,
            PrincipalSid2,
            PrincipalSid3
        };

        var database = new Dictionary<string, ICollection<SecurityIdentifier>>(StringComparer.Ordinal)
        {
            { Privilege1, principals1 },
            { Privilege2, principals2 }
        };

        var policy = new MockLsaUserRights(database);
        policy.Connect("SystemName");

        Assert.Equal(new[] { PrincipalSidCurrent, PrincipalSid1, PrincipalSid2, PrincipalSid3 }.OrderBy(p => p), policy.LsaEnumerateAccountsWithUserRight().OrderBy(p => p));
        Assert.Equal(new[] { Privilege1 }, policy.LsaEnumerateAccountRights(PrincipalSidCurrent));
        Assert.Equal(new[] { Privilege2 }, policy.LsaEnumerateAccountRights(PrincipalSid1));
        Assert.Equal(new[] { Privilege1, Privilege2 }.OrderBy(p => p), policy.LsaEnumerateAccountRights(PrincipalSid2).OrderBy(p => p));
        Assert.Equal(new[] { Privilege1, Privilege2 }.OrderBy(p => p), policy.LsaEnumerateAccountRights(PrincipalSid3).OrderBy(p => p));

        var manager = this.ServiceProvider.GetRequiredService<IUserRightsManager>();
        var pattern = new Regex("^S-1-5-21", RegexOptions.None, TimeSpan.FromSeconds(1));
        manager.ModifyPrivilege(policy, Privilege1, new[] { PrincipalName1 }, Array.Empty<string>(), false, false, pattern, false);

        Assert.Equal(new[] { PrincipalSid1, PrincipalSid2, PrincipalSid3 }.OrderBy(p => p), policy.LsaEnumerateAccountsWithUserRight().OrderBy(p => p));
        Assert.Equal(new[] { Privilege1, Privilege2 }.OrderBy(p => p), policy.LsaEnumerateAccountRights(PrincipalSid1).OrderBy(p => p));
        Assert.Equal(new[] { Privilege1, Privilege2 }, policy.LsaEnumerateAccountRights(PrincipalSid2).OrderBy(p => p));
        Assert.Equal(new[] { Privilege1, Privilege2 }, policy.LsaEnumerateAccountRights(PrincipalSid3).OrderBy(p => p));
    }

    /// <summary>
    /// Verifies a single grant is successful and does not modify other assignments.
    /// </summary>
    [Fact]
    public void GrantShouldWork()
    {
        var principals1 = new HashSet<SecurityIdentifier>
        {
            PrincipalSid1
        };

        var principals2 = new HashSet<SecurityIdentifier>
        {
            PrincipalSid2
        };

        var database = new Dictionary<string, ICollection<SecurityIdentifier>>(StringComparer.Ordinal)
        {
            { Privilege1, principals1 },
            { Privilege2, principals2 }
        };

        var policy = new MockLsaUserRights(database);
        policy.Connect("SystemName");

        Assert.Equal(new[] { PrincipalSid1, PrincipalSid2 }.OrderBy(p => p), policy.LsaEnumerateAccountsWithUserRight().OrderBy(p => p));
        Assert.Equal(new[] { Privilege1 }, policy.LsaEnumerateAccountRights(PrincipalSid1));
        Assert.Equal(new[] { Privilege2 }, policy.LsaEnumerateAccountRights(PrincipalSid2));

        var manager = this.ServiceProvider.GetRequiredService<IUserRightsManager>();
        manager.ModifyPrivilege(policy, Privilege2, new[] { PrincipalName1 }, Array.Empty<string>(), false, false, null!, false);

        Assert.Equal(new[] { PrincipalSid1, PrincipalSid2 }.OrderBy(p => p), policy.LsaEnumerateAccountsWithUserRight().OrderBy(p => p));
        Assert.Equal(new[] { Privilege1, Privilege2 }.OrderBy(p => p), policy.LsaEnumerateAccountRights(PrincipalSid1).OrderBy(p => p));
        Assert.Equal(new[] { Privilege2 }, policy.LsaEnumerateAccountRights(PrincipalSid2));
    }

    /// <summary>
    /// Verifies invalid arguments throw an instance of <see cref="ArgumentException"/>.
    /// </summary>
    /// <param name="policy">A connection to the local security authority.</param>
    /// <param name="privilege">The privilege to modify.</param>
    /// <param name="grants">The principals to grant the privilege to.</param>
    /// <param name="revocations">The principals to revoke the privilege from.</param>
    /// <param name="revokeAll">Revokes all principals from the privilege.</param>
    /// <param name="revokeOthers">Revokes all principals from the privilege excluding those being granted.</param>
    /// <param name="revokePattern">Revokes all principals whose SID matches the regular expression excluding those being granted.</param>
    /// <param name="dryRun">Enables dry-run mode.</param>
    [Theory]
    [MemberData(nameof(InvalidArguments))]
    public void InvalidArgumentsThrowsException(IUserRights policy, string privilege, string[] grants, string[] revocations, bool revokeAll, bool revokeOthers, Regex revokePattern, bool dryRun)
    {
        var manager = this.ServiceProvider.GetRequiredService<IUserRightsManager>();

        Assert.ThrowsAny<ArgumentException>(() => manager.ModifyPrivilege(policy, privilege, grants, revocations, revokeAll, revokeOthers, revokePattern, dryRun));
    }

    /// <summary>
    /// Verifies revoking all principals for a privilege is successful and does not modify other assignments.
    /// </summary>
    [Fact]
    public void RevokeAllShouldWork()
    {
        var principals1 = new HashSet<SecurityIdentifier>
        {
            PrincipalSid1,
            PrincipalSid2
        };

        var principals2 = new HashSet<SecurityIdentifier>
        {
            PrincipalSid1,
            PrincipalSid2
        };

        var database = new Dictionary<string, ICollection<SecurityIdentifier>>(StringComparer.Ordinal)
        {
            { Privilege1, principals1 },
            { Privilege2, principals2 }
        };

        var policy = new MockLsaUserRights(database);
        policy.Connect("SystemName");

        Assert.Equal(new[] { PrincipalSid1, PrincipalSid2 }.OrderBy(p => p), policy.LsaEnumerateAccountsWithUserRight().OrderBy(p => p));
        Assert.Equal(new[] { Privilege1, Privilege2 }.OrderBy(p => p), policy.LsaEnumerateAccountRights(PrincipalSid1).OrderBy(p => p));
        Assert.Equal(new[] { Privilege1, Privilege2 }.OrderBy(p => p), policy.LsaEnumerateAccountRights(PrincipalSid2).OrderBy(p => p));
        Assert.Equal(new[] { PrincipalSid1, PrincipalSid2 }.OrderBy(p => p), policy.LsaEnumerateAccountsWithUserRight(Privilege1).OrderBy(p => p));
        Assert.Equal(new[] { PrincipalSid1, PrincipalSid2 }.OrderBy(p => p), policy.LsaEnumerateAccountsWithUserRight(Privilege2).OrderBy(p => p));

        var manager = this.ServiceProvider.GetRequiredService<IUserRightsManager>();
        manager.ModifyPrivilege(policy, Privilege1, Array.Empty<string>(), Array.Empty<string>(), true, false, null!, false);

        Assert.Equal(new[] { PrincipalSid1, PrincipalSid2 }.OrderBy(p => p), policy.LsaEnumerateAccountsWithUserRight().OrderBy(p => p));
        Assert.Equal(new[] { Privilege2 }, policy.LsaEnumerateAccountRights(PrincipalSid1));
        Assert.Equal(new[] { Privilege2 }, policy.LsaEnumerateAccountRights(PrincipalSid2));
        Assert.Empty(policy.LsaEnumerateAccountsWithUserRight(Privilege1));
        Assert.Equal(new[] { PrincipalSid1, PrincipalSid2 }.OrderBy(p => p), policy.LsaEnumerateAccountsWithUserRight(Privilege2).OrderBy(p => p));
    }

    /// <summary>
    /// Verifies a single revocation is successful and does not modify other assignments.
    /// </summary>
    [Fact]
    public void RevokeShouldWork()
    {
        var principals1 = new HashSet<SecurityIdentifier>
        {
            PrincipalSid1,
            PrincipalSid2
        };

        var principals2 = new HashSet<SecurityIdentifier>
        {
            PrincipalSid2
        };

        var database = new Dictionary<string, ICollection<SecurityIdentifier>>(StringComparer.Ordinal)
        {
            { Privilege1, principals1 },
            { Privilege2, principals2 }
        };

        var policy = new MockLsaUserRights(database);
        policy.Connect("SystemName");

        Assert.Equal(new[] { PrincipalSid1, PrincipalSid2 }.OrderBy(p => p), policy.LsaEnumerateAccountsWithUserRight().OrderBy(p => p));
        Assert.Equal(new[] { Privilege1 }, policy.LsaEnumerateAccountRights(PrincipalSid1));
        Assert.Equal(new[] { Privilege1, Privilege2 }.OrderBy(p => p), policy.LsaEnumerateAccountRights(PrincipalSid2).OrderBy(p => p));

        var manager = this.ServiceProvider.GetRequiredService<IUserRightsManager>();
        manager.ModifyPrivilege(policy, Privilege1, Array.Empty<string>(), new[] { PrincipalName2 }, false, false, null!, false);

        Assert.Equal(new[] { PrincipalSid1, PrincipalSid2 }.OrderBy(p => p), policy.LsaEnumerateAccountsWithUserRight().OrderBy(p => p));
        Assert.Equal(new[] { Privilege1 }, policy.LsaEnumerateAccountRights(PrincipalSid1));
        Assert.Equal(new[] { Privilege2 }, policy.LsaEnumerateAccountRights(PrincipalSid2));
    }

    /// <summary>
    /// Verifies revoking all non builtin and virtual principals from a privilege is successful.
    /// </summary>
    [Fact]
    public void RevokePatternForAllButBuiltinAndVirtualShouldWork()
    {
        var principals1 = new HashSet<SecurityIdentifier>
        {
            PrincipalSidCurrent,
            PrincipalSid2,
            PrincipalSid3
        };

        var principals2 = new HashSet<SecurityIdentifier>
        {
            PrincipalSid1,
            PrincipalSid2,
            PrincipalSid3
        };

        var database = new Dictionary<string, ICollection<SecurityIdentifier>>(StringComparer.Ordinal)
        {
            { Privilege1, principals1 },
            { Privilege2, principals2 }
        };

        var policy = new MockLsaUserRights(database);
        policy.Connect("SystemName");

        Assert.Equal(new[] { PrincipalSidCurrent, PrincipalSid1, PrincipalSid2, PrincipalSid3 }.OrderBy(p => p), policy.LsaEnumerateAccountsWithUserRight().OrderBy(p => p));
        Assert.Equal(new[] { Privilege1 }, policy.LsaEnumerateAccountRights(PrincipalSidCurrent));
        Assert.Equal(new[] { Privilege2 }, policy.LsaEnumerateAccountRights(PrincipalSid1));
        Assert.Equal(new[] { Privilege1, Privilege2 }.OrderBy(p => p), policy.LsaEnumerateAccountRights(PrincipalSid2).OrderBy(p => p));
        Assert.Equal(new[] { Privilege1, Privilege2 }.OrderBy(p => p), policy.LsaEnumerateAccountRights(PrincipalSid3).OrderBy(p => p));

        var manager = this.ServiceProvider.GetRequiredService<IUserRightsManager>();
        var pattern = new Regex("^S-1-5-21", RegexOptions.None, TimeSpan.FromSeconds(1));
        manager.ModifyPrivilege(policy, Privilege1, Array.Empty<string>(), Array.Empty<string>(), false, false, pattern, false);

        Assert.Equal(new[] { PrincipalSid1, PrincipalSid2, PrincipalSid3 }.OrderBy(p => p), policy.LsaEnumerateAccountsWithUserRight().OrderBy(p => p));
        Assert.Equal(new[] { Privilege2 }, policy.LsaEnumerateAccountRights(PrincipalSid1));
        Assert.Equal(new[] { Privilege1, Privilege2 }, policy.LsaEnumerateAccountRights(PrincipalSid2).OrderBy(p => p));
        Assert.Equal(new[] { Privilege1, Privilege2 }, policy.LsaEnumerateAccountRights(PrincipalSid3).OrderBy(p => p));
    }
}