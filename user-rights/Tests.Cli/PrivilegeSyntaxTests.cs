namespace Tests.Cli;

using System.CommandLine.Parsing;
using Microsoft.Extensions.DependencyInjection;
using UserRights.Application;
using UserRights.Cli;
using Xunit;

/// <summary>
/// Represents syntax tests for privilege functionality.
/// </summary>
public sealed class PrivilegeSyntaxTests : CliTestBase
{
    private readonly Parser parser;

    /// <summary>
    /// Initializes a new instance of the <see cref="PrivilegeSyntaxTests"/> class.
    /// </summary>
    public PrivilegeSyntaxTests()
    {
        this.ServiceCollection.AddSingleton<ILsaUserRights, MockLsaUserRights>();
        this.ServiceCollection.AddSingleton<IUserRightsManager, MockUserRightsManager>();
        this.ServiceCollection.AddSingleton<CliBuilder>();

        var cliBuilder = this.ServiceProvider.GetRequiredService<CliBuilder>();

        var commandLineBuilder = cliBuilder.Create();
        this.parser = commandLineBuilder.Build();
    }

    /// <summary>
    /// Ensures granting a context and revoking a different context is accepted.
    /// </summary>
    [Fact]
    public void GrantAndRevokeShouldWork()
    {
        var args = new[] { "privilege", "SeServiceLogonRight", "--grant", "DOMAIN\\User", "--revoke", "DOMAIN\\Group" };

        var rc = this.parser.Invoke(args);

        Assert.Equal(0, rc);
    }

    /// <summary>
    /// Ensures granting multiple contexts is accepted.
    /// </summary>
    [Fact]
    public void GrantMultipleShouldWork()
    {
        var args = new[] { "privilege", "SeServiceLogonRight", "--grant", "DOMAIN\\User", "--grant", "DOMAIN\\Group" };

        var rc = this.parser.Invoke(args);

        Assert.Equal(0, rc);
    }

    /// <summary>
    /// Ensures granting a context is accepted.
    /// </summary>
    [Fact]
    public void GrantShouldWork()
    {
        var args = new[] { "privilege", "SeServiceLogonRight", "--grant", "DOMAIN\\User" };

        var rc = this.parser.Invoke(args);

        Assert.Equal(0, rc);
    }

    /// <summary>
    /// Ensures an empty or whitespace grant is rejected.
    /// </summary>
    /// <param name="args">The test arguments.</param>
    [Theory]
    [InlineData("privilege", "SeServiceLogonRight", "--grant", "")]
    [InlineData("privilege", "SeServiceLogonRight", "--grant", " ")]
    public void GrantWithInvalidStringThrowsException(params string[] args)
        => Assert.Throws<SyntaxException>(() => this.parser.Invoke(args));

    /// <summary>
    /// Ensures granting a context and revoking all contexts is rejected.
    /// </summary>
    [Fact]
    public void GrantWithRevokeAllThrowsException()
    {
        var args = new[] { "privilege", "SeServiceLogonRight", "--grant", "DOMAIN\\User", "--revoke-all" };

        Assert.Throws<SyntaxException>(() => this.parser.Invoke(args));
    }

    /// <summary>
    /// Ensures specifying no options is rejected.
    /// </summary>
    [Fact]
    public void NoOptionsThrowsException()
    {
        var args = new[] { "privilege" };

        Assert.Throws<SyntaxException>(() => this.parser.Invoke(args));
    }

    /// <summary>
    /// Ensures overlapping or duplicate contexts is rejected.
    /// </summary>
    /// <param name="args">The test arguments.</param>
    [Theory]
    [InlineData("privilege", "SeServiceLogonRight", "--grant", "DOMAIN\\UserOrGroup", "--revoke", "DOMAIN\\UserOrGroup")]
    [InlineData("privilege", "SeServiceLogonRight", "--grant", "DOMAIN\\UserOrGroup", "--grant", "DOMAIN\\UserOrGroup")]
    [InlineData("privilege", "SeServiceLogonRight", "--revoke", "DOMAIN\\UserOrGroup", "--revoke", "DOMAIN\\UserOrGroup")]
    public void OverlappingGrantsAndRevokesThrowsException(params string[] args)
        => Assert.Throws<SyntaxException>(() => this.parser.Invoke(args));

    /// <summary>
    /// Ensures an empty or whitespace principal is rejected.
    /// </summary>
    /// <param name="args">The test arguments.</param>
    [Theory]
    [InlineData("privilege", "", "--grant", "DOMAIN\\UserOrGroup")]
    [InlineData("privilege", " ", "--grant", "DOMAIN\\UserOrGroup")]
    public void PrivilegeWithInvalidStringThrowsException(params string[] args)
        => Assert.Throws<SyntaxException>(() => this.parser.Invoke(args));

    /// <summary>
    /// Ensures revoking all contexts is accepted.
    /// </summary>
    [Fact]
    public void RevokeAllShouldWork()
    {
        var args = new[] { "privilege", "SeServiceLogonRight", "--revoke-all" };

        int? rc = null;
        var exception = Record.Exception(() => rc = this.parser.Invoke(args));

        Assert.Null(exception);
        Assert.Equal(0, rc);
    }

    /// <summary>
    /// Ensures granting a context and revoking all contexts is rejected.
    /// </summary>
    [Fact]
    public void RevokeAllWithGrantsThrowsException()
    {
        var args = new[] { "privilege", "SeServiceLogonRight", "--revoke-all", "--grant", "DOMAIN\\UserOrGroup" };

        Assert.Throws<SyntaxException>(() => this.parser.Invoke(args));
    }

    /// <summary>
    /// Ensures revoking a context and revoking all contexts is rejected.
    /// </summary>
    [Fact]
    public void RevokeAllWithRevocationsThrowsException()
    {
        var args = new[] { "privilege", "SeServiceLogonRight", "--revoke-all", "--revoke", "DOMAIN\\UserOrGroup" };

        Assert.Throws<SyntaxException>(() => this.parser.Invoke(args));
    }

    /// <summary>
    /// Ensures revoking all contexts and revoking other contexts is rejected.
    /// </summary>
    [Fact]
    public void RevokeAllWithRevokeOthersThrowsException()
    {
        var args = new[] { "privilege", "SeServiceLogonRight", "--revoke-all", "--revoke-others" };

        Assert.Throws<SyntaxException>(() => this.parser.Invoke(args));
    }

    /// <summary>
    /// Ensures revoking multiple contexts is accepted.
    /// </summary>
    [Fact]
    public void RevokeMultipleShouldWork()
    {
        var args = new[] { "privilege", "SeServiceLogonRight", "--revoke", "DOMAIN\\User", "--revoke", "DOMAIN\\Group" };

        var rc = this.parser.Invoke(args);

        Assert.Equal(0, rc);
    }

    /// <summary>
    /// Ensures revoke other contexts without granting a context is rejected.
    /// </summary>
    [Fact]
    public void RevokeOthersWithOutGrantsThrowsException()
    {
        var args = new[] { "privilege", "SeServiceLogonRight", "--revoke-others" };

        Assert.Throws<SyntaxException>(() => this.parser.Invoke(args));
    }

    /// <summary>
    /// Ensures revoke other contexts with revoking a context is rejected.
    /// </summary>
    [Fact]
    public void RevokeOthersWithRevocationsThrowsException()
    {
        var args = new[] { "privilege", "SeServiceLogonRight", "--revoke-others", "--revoke", "DOMAIN\\UserOrGroup" };

        Assert.Throws<SyntaxException>(() => this.parser.Invoke(args));
    }

    /// <summary>
    /// Ensures revoking a valid pattern is accepted.
    /// </summary>
    [Fact]
    public void RevokePatternShouldWork()
    {
        var args = new[] { "privilege", "SeServiceLogonRight", "--revoke-pattern", "^S-1-5-21-" };

        var rc = this.parser.Invoke(args);

        Assert.Equal(0, rc);
    }

    /// <summary>
    /// Ensures granting a context and revoking a valid pattern is accepted.
    /// </summary>
    [Fact]
    public void RevokePatternWithGrantShouldWork()
    {
        var args = new[] { "privilege", "SeServiceLogonRight", "--grant", "DOMAIN\\UserOrGroup", "--revoke-pattern", "^S-1-5-21-" };

        var rc = this.parser.Invoke(args);

        Assert.Equal(0, rc);
    }

    /// <summary>
    /// Ensures revoking a valid regex is accepted.
    /// </summary>
    /// <param name="args">The test arguments.</param>
    [Theory]
    [InlineData("privilege", "SeServiceLogonRight", "--revoke-pattern", "^xyz.*")]
    [InlineData("privilege", "SeServiceLogonRight", "--revoke-pattern", "^S-1-5-21-")]
    [InlineData("privilege", "SeServiceLogonRight", "--revoke-pattern", "(?i)^[A-Z]+")]
    public void RevokePatternWithValidRegexShouldWork(params string[] args)
    {
        var rc = this.parser.Invoke(args);

        Assert.Equal(0, rc);
    }

    /// <summary>
    /// Ensures revoking a pattern and revoking all contexts is rejected.
    /// </summary>
    [Fact]
    public void RevokePatternWithRevokeAllThrowsException()
    {
        var args = new[] { "privilege", "SeServiceLogonRight", "--revoke-pattern", "^S-1-5-21-", "--revoke-all" };

        Assert.Throws<SyntaxException>(() => this.parser.Invoke(args));
    }

    /// <summary>
    /// Ensures revoking a pattern and revoking other contexts is rejected.
    /// </summary>
    [Fact]
    public void RevokePatternWithRevokeOthersThrowsException()
    {
        var args = new[] { "privilege", "SeServiceLogonRight", "--revoke-pattern", "^S-1-5-21-", "--revoke-others" };

        Assert.Throws<SyntaxException>(() => this.parser.Invoke(args));
    }

    /// <summary>
    /// Ensures revoking a pattern and revoking a context is rejected.
    /// </summary>
    [Fact]
    public void RevokePatternWithRevokeThrowsException()
    {
        var args = new[] { "privilege", "SeServiceLogonRight", "--revoke-pattern", "^S-1-5-21-", "--revoke", "DOMAIN\\UserOrGroup" };

        Assert.Throws<SyntaxException>(() => this.parser.Invoke(args));
    }

    /// <summary>
    /// Ensures revoking an invalid regex is rejected.
    /// </summary>
    /// <param name="args">The test arguments.</param>
    [Theory]
    [InlineData("privilege", "SeServiceLogonRight", "--revoke-pattern", "[0-9]{3,1}")]
    [InlineData("privilege", "SeServiceLogonRight", "--revoke-pattern", "^[S-1-5-21-")]
    public void RevokePatternWithInvalidRegexThrowsException(params string[] args)
        => Assert.Throws<SyntaxException>(() => this.parser.Invoke(args));

    /// <summary>
    /// Ensures an empty or whitespace revocation pattern is rejected.
    /// </summary>
    /// <param name="args">The test arguments.</param>
    [Theory]
    [InlineData("privilege", "SeServiceLogonRight", "--revoke-pattern", "")]
    [InlineData("privilege", "SeServiceLogonRight", "--revoke-pattern", " ")]
    public void RevokePatternWithInvalidStringThrowsException(params string[] args)
        => Assert.Throws<SyntaxException>(() => this.parser.Invoke(args));

    /// <summary>
    /// Ensures revoking a context is accepted.
    /// </summary>
    [Fact]
    public void RevokeShouldWork()
    {
        var args = new[] { "privilege", "SeServiceLogonRight", "--revoke", "DOMAIN\\UserOrGroup" };

        var rc = this.parser.Invoke(args);

        Assert.Equal(0, rc);
    }

    /// <summary>
    /// Ensures an empty or whitespace revocation is rejected.
    /// </summary>
    /// <param name="args">The test arguments.</param>
    [Theory]
    [InlineData("privilege", "SeServiceLogonRight", "--revoke", "")]
    [InlineData("privilege", "SeServiceLogonRight", "--revoke", " ")]
    public void RevokeWithInvalidStringThrowsException(params string[] args)
        => Assert.Throws<SyntaxException>(() => this.parser.Invoke(args));

    /// <summary>
    /// Ensures an empty or whitespace system name is rejected.
    /// </summary>
    /// <param name="args">The test arguments.</param>
    [Theory]
    [InlineData("privilege", "SeServiceLogonRight", "--grant", "DOMAIN\\UserOrGroup", "--system-name", "")]
    [InlineData("privilege", "SeServiceLogonRight", "--grant", "DOMAIN\\UserOrGroup", "--system-name", " ")]
    public void SystemNameWithInvalidStringThrowsException(params string[] args)
        => Assert.Throws<SyntaxException>(() => this.parser.Invoke(args));
}