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
/// Represents integration tests for modify privilege functionality.
/// </summary>
public sealed class PrivilegeCommandTests : CliTestBase
{
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

        policy.ResetConnection();

        this.ServiceCollection.AddSingleton<ILsaUserRights>(policy);
        this.ServiceCollection.AddSingleton<IUserRightsManager, UserRightsManager>();
        this.ServiceCollection.AddSingleton<CliBuilder>();

        var cliBuilder = this.ServiceProvider.GetRequiredService<CliBuilder>();

        var commandLineBuilder = cliBuilder.Create();
        var parser = commandLineBuilder.Build();

        var args = new[]
        {
            "privilege",
            Privilege2,
            "--grant",
            PrincipalName1,
            "--revoke-others"
        };

        var rc = parser.Invoke(args);

        Assert.Equal(0, rc);
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
    public void GrantAndRevokePasses()
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

        policy.ResetConnection();

        this.ServiceCollection.AddSingleton<ILsaUserRights>(policy);
        this.ServiceCollection.AddSingleton<IUserRightsManager, UserRightsManager>();
        this.ServiceCollection.AddSingleton<CliBuilder>();

        var cliBuilder = this.ServiceProvider.GetRequiredService<CliBuilder>();

        var commandLineBuilder = cliBuilder.Create();
        var parser = commandLineBuilder.Build();

        var args = new[]
        {
            "privilege",
            Privilege1,
            "--grant",
            PrincipalName2,
            "--revoke",
            PrincipalName1
        };

        var rc = parser.Invoke(args);

        Assert.Equal(0, rc);
        Assert.Equal(new[] { PrincipalSid1, PrincipalSid2 }.OrderBy(p => p), policy.LsaEnumerateAccountsWithUserRight().OrderBy(p => p));
        Assert.Equal(new[] { Privilege2 }, policy.LsaEnumerateAccountRights(PrincipalSid1));
        Assert.Equal(new[] { Privilege1, Privilege2 }.OrderBy(p => p), policy.LsaEnumerateAccountRights(PrincipalSid2).OrderBy(p => p));
    }

    /// <summary>
    /// Verifies granting a principal to a privilege and revoking all principals matching a pattern is successful and does not modify other assignments.
    /// </summary>
    [Fact]
    public void GrantAndRevokePatternPasses()
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

        policy.ResetConnection();

        this.ServiceCollection.AddSingleton<ILsaUserRights>(policy);
        this.ServiceCollection.AddSingleton<IUserRightsManager, UserRightsManager>();
        this.ServiceCollection.AddSingleton<CliBuilder>();

        var cliBuilder = this.ServiceProvider.GetRequiredService<CliBuilder>();

        var commandLineBuilder = cliBuilder.Create();
        var parser = commandLineBuilder.Build();

        var args = new[]
        {
            "privilege",
            Privilege1,
            "--grant",
            PrincipalName1,
            "--revoke-pattern",
            "^S-1-5-21"
        };

        var rc = parser.Invoke(args);

        Assert.Equal(0, rc);
        Assert.Equal(new[] { PrincipalSid1, PrincipalSid2, PrincipalSid3 }.OrderBy(p => p), policy.LsaEnumerateAccountsWithUserRight().OrderBy(p => p));
        Assert.Equal(new[] { Privilege1, Privilege2 }.OrderBy(p => p), policy.LsaEnumerateAccountRights(PrincipalSid1).OrderBy(p => p));
        Assert.Equal(new[] { Privilege1, Privilege2 }, policy.LsaEnumerateAccountRights(PrincipalSid2).OrderBy(p => p));
        Assert.Equal(new[] { Privilege1, Privilege2 }, policy.LsaEnumerateAccountRights(PrincipalSid3).OrderBy(p => p));
    }

    /// <summary>
    /// Verifies a single grant is successful and does not modify other assignments.
    /// </summary>
    [Fact]
    public void GrantPasses()
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

        policy.ResetConnection();

        this.ServiceCollection.AddSingleton<ILsaUserRights>(policy);
        this.ServiceCollection.AddSingleton<IUserRightsManager, UserRightsManager>();
        this.ServiceCollection.AddSingleton<CliBuilder>();

        var cliBuilder = this.ServiceProvider.GetRequiredService<CliBuilder>();

        var commandLineBuilder = cliBuilder.Create();
        var parser = commandLineBuilder.Build();

        var args = new[]
        {
            "privilege",
            Privilege2,
            "--grant",
            PrincipalName1
        };

        var rc = parser.Invoke(args);

        Assert.Equal(0, rc);
        Assert.Equal(new[] { PrincipalSid1, PrincipalSid2 }.OrderBy(p => p), policy.LsaEnumerateAccountsWithUserRight().OrderBy(p => p));
        Assert.Equal(new[] { Privilege1, Privilege2 }.OrderBy(p => p), policy.LsaEnumerateAccountRights(PrincipalSid1).OrderBy(p => p));
        Assert.Equal(new[] { Privilege2 }, policy.LsaEnumerateAccountRights(PrincipalSid2));
    }

    /// <summary>
    /// Verifies revoking all principals for a privilege is successful and does not modify other assignments.
    /// </summary>
    [Fact]
    public void RevokeAllPasses()
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

        policy.ResetConnection();

        this.ServiceCollection.AddSingleton<ILsaUserRights>(policy);
        this.ServiceCollection.AddSingleton<IUserRightsManager, UserRightsManager>();
        this.ServiceCollection.AddSingleton<CliBuilder>();

        var cliBuilder = this.ServiceProvider.GetRequiredService<CliBuilder>();

        var commandLineBuilder = cliBuilder.Create();
        var parser = commandLineBuilder.Build();

        var args = new[]
        {
            "privilege",
            Privilege1,
            "--revoke-all"
        };

        var rc = parser.Invoke(args);

        Assert.Equal(0, rc);
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
    public void RevokePasses()
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
            "privilege",
            Privilege1,
            "--revoke",
            PrincipalName2
        };

        var rc = parser.Invoke(args);

        Assert.Equal(0, rc);
        Assert.Equal(new[] { PrincipalSid1, PrincipalSid2 }.OrderBy(p => p), policy.LsaEnumerateAccountsWithUserRight().OrderBy(p => p));
        Assert.Equal(new[] { Privilege1 }, policy.LsaEnumerateAccountRights(PrincipalSid1));
        Assert.Equal(new[] { Privilege2 }, policy.LsaEnumerateAccountRights(PrincipalSid2));
    }

    /// <summary>
    /// Verifies revoking all non builtin and virtual principals from a privilege is successful.
    /// </summary>
    [Fact]
    public void RevokePatternForAllButBuiltinAndVirtualPasses()
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

        policy.ResetConnection();

        this.ServiceCollection.AddSingleton<ILsaUserRights>(policy);
        this.ServiceCollection.AddSingleton<IUserRightsManager, UserRightsManager>();
        this.ServiceCollection.AddSingleton<CliBuilder>();

        var cliBuilder = this.ServiceProvider.GetRequiredService<CliBuilder>();

        var commandLineBuilder = cliBuilder.Create();
        var parser = commandLineBuilder.Build();

        var args = new[]
        {
            "privilege",
            Privilege1,
            "--revoke-pattern",
            "^S-1-5-21"
        };

        var rc = parser.Invoke(args);

        Assert.Equal(0, rc);
        Assert.Equal(new[] { PrincipalSid1, PrincipalSid2, PrincipalSid3 }.OrderBy(p => p), policy.LsaEnumerateAccountsWithUserRight().OrderBy(p => p));
        Assert.Equal(new[] { Privilege2 }, policy.LsaEnumerateAccountRights(PrincipalSid1));
        Assert.Equal(new[] { Privilege1, Privilege2 }, policy.LsaEnumerateAccountRights(PrincipalSid2).OrderBy(p => p));
        Assert.Equal(new[] { Privilege1, Privilege2 }, policy.LsaEnumerateAccountRights(PrincipalSid3).OrderBy(p => p));
    }
}