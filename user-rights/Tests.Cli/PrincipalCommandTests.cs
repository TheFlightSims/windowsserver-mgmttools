namespace Tests.Cli;

using System;
using System.Collections.Generic;
using System.CommandLine.Parsing;
using System.Linq;
using System.Security.Principal;
using Microsoft.Extensions.DependencyInjection;
using UserRights.Application;
using UserRights.Cli;
using Xunit;
using static Tests.TestData;

/// <summary>
/// Represents integration tests for modify principal functionality.
/// </summary>
public sealed class PrincipalCommandTests : CliTestBase
{
    /// <summary>
    /// Verifies a granting a privilege to a principal and revoking their other privileges is successful and does not modify other assignments.
    /// </summary>
    [Fact]
    public void GrantAndRevokeOthersShouldWork()
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

        Assert.Equal(new[] { PrincipalSid1, PrincipalSid2 }, policy.LsaEnumerateAccountsWithUserRight().OrderBy(p => p));
        Assert.Equal(new[] { Privilege1 }, policy.LsaEnumerateAccountRights(PrincipalSid1));
        Assert.Equal(new[] { Privilege2 }, policy.LsaEnumerateAccountRights(PrincipalSid2));

        policy.ResetConnection();

        this.ServiceCollection.AddSingleton<ILsaUserRights>(policy);
        this.ServiceCollection.AddSingleton<IUserRightsManager, UserRightsManager>();
        this.ServiceCollection.AddSingleton<CliBuilder>();

        var cliBuilder = this.ServiceProvider.GetRequiredService<CliBuilder>();

        var commandLineBuilder = cliBuilder.Create();
        var parser = commandLineBuilder.Build();

        var args = new[]
        {
            "principal",
            PrincipalName1,
            "--grant",
            Privilege2,
            "--revoke-others"
        };

        var rc = parser.Invoke(args);

        Assert.Equal(0, rc);
        Assert.Equal(new[] { PrincipalSid1, PrincipalSid2 }, policy.LsaEnumerateAccountsWithUserRight().OrderBy(p => p));
        Assert.Equal(new[] { Privilege2 }, policy.LsaEnumerateAccountRights(PrincipalSid1));
        Assert.Equal(new[] { Privilege2 }, policy.LsaEnumerateAccountRights(PrincipalSid2));
    }

    /// <summary>
    /// Verifies a single grant with a single revoke is successful and does not modify other assignments.
    /// </summary>
    [Fact]
    public void GrantAndRevokeShouldWork()
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

        policy.ResetConnection();

        this.ServiceCollection.AddSingleton<ILsaUserRights>(policy);
        this.ServiceCollection.AddSingleton<IUserRightsManager, UserRightsManager>();
        this.ServiceCollection.AddSingleton<CliBuilder>();

        var cliBuilder = this.ServiceProvider.GetRequiredService<CliBuilder>();

        var commandLineBuilder = cliBuilder.Create();
        var parser = commandLineBuilder.Build();

        var args = new[]
        {
            "principal",
            PrincipalName1,
            "--grant",
            Privilege2,
            "--revoke",
            Privilege1
        };

        var rc = parser.Invoke(args);

        Assert.Equal(0, rc);
        Assert.Equal(new[] { PrincipalSid1, PrincipalSid2 }.OrderBy(p => p), policy.LsaEnumerateAccountsWithUserRight().OrderBy(p => p));
        Assert.Equal(new[] { Privilege2 }, policy.LsaEnumerateAccountRights(PrincipalSid1));
        Assert.Equal(new[] { Privilege1, Privilege2 }.OrderBy(p => p), policy.LsaEnumerateAccountRights(PrincipalSid2).OrderBy(p => p));
    }

    /// <summary>
    /// Verifies a single grant is successful and does not modify other assignments.
    /// </summary>
    [Fact]
    public void GrantShouldWork()
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

        policy.ResetConnection();

        this.ServiceCollection.AddSingleton<ILsaUserRights>(policy);
        this.ServiceCollection.AddSingleton<IUserRightsManager, UserRightsManager>();
        this.ServiceCollection.AddSingleton<CliBuilder>();

        var cliBuilder = this.ServiceProvider.GetRequiredService<CliBuilder>();

        var commandLineBuilder = cliBuilder.Create();
        var parser = commandLineBuilder.Build();

        var args = new[]
        {
            "principal",
            PrincipalName1,
            "--grant",
            Privilege2
        };

        var rc = parser.Invoke(args);

        Assert.Equal(0, rc);
        Assert.Equal(new[] { PrincipalSid1, PrincipalSid2 }.OrderBy(p => p), policy.LsaEnumerateAccountsWithUserRight().OrderBy(p => p));
        Assert.Equal(new[] { Privilege1, Privilege2 }.OrderBy(p => p), policy.LsaEnumerateAccountRights(PrincipalSid1).OrderBy(p => p));
        Assert.Equal(new[] { Privilege1, Privilege2 }.OrderBy(p => p), policy.LsaEnumerateAccountRights(PrincipalSid2).OrderBy(p => p));
    }

    /// <summary>
    /// Verifies a revoking all privileges for a principal is successful and does not modify other assignments.
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

        policy.ResetConnection();

        this.ServiceCollection.AddSingleton<ILsaUserRights>(policy);
        this.ServiceCollection.AddSingleton<IUserRightsManager, UserRightsManager>();
        this.ServiceCollection.AddSingleton<CliBuilder>();

        var cliBuilder = this.ServiceProvider.GetRequiredService<CliBuilder>();

        var commandLineBuilder = cliBuilder.Create();
        var parser = commandLineBuilder.Build();

        var args = new[]
        {
            "principal",
            PrincipalName1,
            "--revoke-all"
        };

        var rc = parser.Invoke(args);

        Assert.Equal(0, rc);
        Assert.Empty(policy.LsaEnumerateAccountRights(PrincipalSid1));
        Assert.Equal(new[] { PrincipalSid2 }, policy.LsaEnumerateAccountsWithUserRight());
        Assert.Equal(new[] { Privilege1, Privilege2 }.OrderBy(p => p), policy.LsaEnumerateAccountRights(PrincipalSid2).OrderBy(p => p));
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

        policy.ResetConnection();

        this.ServiceCollection.AddSingleton<ILsaUserRights>(policy);
        this.ServiceCollection.AddSingleton<IUserRightsManager, UserRightsManager>();
        this.ServiceCollection.AddSingleton<CliBuilder>();

        var cliBuilder = this.ServiceProvider.GetRequiredService<CliBuilder>();

        var commandLineBuilder = cliBuilder.Create();
        var parser = commandLineBuilder.Build();

        var args = new[]
        {
            "principal",
            PrincipalName2,
            "--revoke",
            Privilege2
        };

        var rc = parser.Invoke(args);

        Assert.Equal(0, rc);
        Assert.Equal(new[] { PrincipalSid1, PrincipalSid2 }.OrderBy(p => p), policy.LsaEnumerateAccountsWithUserRight().OrderBy(p => p));
        Assert.Equal(new[] { Privilege1 }, policy.LsaEnumerateAccountRights(PrincipalSid1));
        Assert.Equal(new[] { Privilege1 }, policy.LsaEnumerateAccountRights(PrincipalSid2));
    }
}