namespace Tests.Cli;

using System.CommandLine.Parsing;
using Microsoft.Extensions.DependencyInjection;
using UserRights.Application;
using UserRights.Cli;
using Xunit;

/// <summary>
/// Represents syntax tests for principal functionality.
/// </summary>
public sealed class PrincipalSyntaxTests : CliTestBase
{
    private readonly Parser parser;

    /// <summary>
    /// Initializes a new instance of the <see cref="PrincipalSyntaxTests"/> class.
    /// </summary>
    public PrincipalSyntaxTests()
    {
        this.ServiceCollection.AddSingleton<ILsaUserRights, MockLsaUserRights>();
        this.ServiceCollection.AddSingleton<IUserRightsManager, MockUserRightsManager>();
        this.ServiceCollection.AddSingleton<CliBuilder>();

        var cliBuilder = this.ServiceProvider.GetRequiredService<CliBuilder>();

        var commandLineBuilder = cliBuilder.Create();
        this.parser = commandLineBuilder.Build();
    }

    /// <summary>
    /// Ensures granting a privilege and revoking a different privilege is accepted.
    /// </summary>
    [Fact]
    public void GrantAndRevokeShouldWork()
    {
        var args = new[] { "principal", "DOMAIN\\UserOrGroup", "--grant", "SeServiceLogonRight", "--revoke", "SeBatchLogonRight" };

        var rc = this.parser.Invoke(args);

        Assert.Equal(0, rc);
    }

    /// <summary>
    /// Ensures granting multiple privileges is accepted.
    /// </summary>
    [Fact]
    public void GrantMultipleShouldWork()
    {
        var args = new[] { "principal", "DOMAIN\\UserOrGroup", "--grant", "SeServiceLogonRight", "--grant", "SeBatchLogonRight" };

        var rc = this.parser.Invoke(args);

        Assert.Equal(0, rc);
    }

    /// <summary>
    /// Ensures granting a privilege is accepted.
    /// </summary>
    [Fact]
    public void GrantShouldWork()
    {
        var args = new[] { "principal", "DOMAIN\\UserOrGroup", "--grant", "SeServiceLogonRight" };

        var rc = this.parser.Invoke(args);

        Assert.Equal(0, rc);
    }

    /// <summary>
    /// Ensures an empty or whitespace grant is rejected.
    /// </summary>
    /// <param name="args">The test arguments.</param>
    [Theory]
    [InlineData("principal", "DOMAIN\\UserOrGroup", "--grant", "")]
    [InlineData("principal", "DOMAIN\\UserOrGroup", "--grant", " ")]
    public void GrantWithInvalidStringThrowsException(params string[] args)
        => Assert.Throws<SyntaxException>(() => this.parser.Invoke(args));

    /// <summary>
    /// Ensures granting a privilege and revoking all other privileges is rejected.
    /// </summary>
    [Fact]
    public void GrantWithRevokeAllThrowsException()
    {
        var args = new[] { "principal", "DOMAIN\\UserOrGroup", "--grant", "SeServiceLogonRight", "--revoke-all" };

        Assert.Throws<SyntaxException>(() => this.parser.Invoke(args));
    }

    /// <summary>
    /// Ensures specifying no options is rejected.
    /// </summary>
    [Fact]
    public void NoOptionsThrowsException()
    {
        var args = new[] { "principal" };

        Assert.Throws<SyntaxException>(() => this.parser.Invoke(args));
    }

    /// <summary>
    /// Ensures overlapping or duplicate privileges is rejected.
    /// </summary>
    /// <param name="args">The test arguments.</param>
    [Theory]
    [InlineData("principal", "DOMAIN\\UserOrGroup", "--grant", "SeServiceLogonRight", "--revoke", "SeServiceLogonRight")]
    [InlineData("principal", "DOMAIN\\UserOrGroup", "--grant", "SeServiceLogonRight", "--grant", "SeServiceLogonRight")]
    [InlineData("principal", "DOMAIN\\UserOrGroup", "--revoke", "SeServiceLogonRight", "--revoke", "SeServiceLogonRight")]
    public void OverlappingGrantsAndRevokesThrowsException(params string[] args)
        => Assert.Throws<SyntaxException>(() => this.parser.Invoke(args));

    /// <summary>
    /// Ensures an empty or whitespace principal is rejected.
    /// </summary>
    /// <param name="args">The test arguments.</param>
    [Theory]
    [InlineData("principal", "", "--grant", "SeServiceLogonRight")]
    [InlineData("principal", " ", "--grant", "SeServiceLogonRight")]
    public void PrincipalWithInvalidStringThrowsException(params string[] args)
        => Assert.Throws<SyntaxException>(() => this.parser.Invoke(args));

    /// <summary>
    /// Ensures revoking all privileges is accepted.
    /// </summary>
    [Fact]
    public void RevokeAllShouldWork()
    {
        var args = new[] { "principal", "DOMAIN\\UserOrGroup", "--revoke-all" };

        var rc = this.parser.Invoke(args);

        Assert.Equal(0, rc);
    }

    /// <summary>
    /// Ensures granting a privilege and granting all privileges is rejected.
    /// </summary>
    [Fact]
    public void RevokeAllWithGrantsThrowsException()
    {
        var args = new[] { "principal", "DOMAIN\\UserOrGroup", "--revoke-all", "--grant", "SeServiceLogonRight" };

        Assert.Throws<SyntaxException>(() => this.parser.Invoke(args));
    }

    /// <summary>
    /// Ensures revoking a privilege and revoking all privileges is rejected.
    /// </summary>
    [Fact]
    public void RevokeAllWithRevocationsThrowsException()
    {
        var args = new[] { "principal", "DOMAIN\\UserOrGroup", "--revoke-all", "--revoke", "SeServiceLogonRight" };

        Assert.Throws<SyntaxException>(() => this.parser.Invoke(args));
    }

    /// <summary>
    /// Ensures revoking all privileges and revoking other privileges is rejected.
    /// </summary>
    [Fact]
    public void RevokeAllWithRevokeOthersThrowsException()
    {
        var args = new[] { "principal", "DOMAIN\\UserOrGroup", "--revoke-all", "--revoke-others" };

        Assert.Throws<SyntaxException>(() => this.parser.Invoke(args));
    }

    /// <summary>
    /// Ensures revoking multiple privileges is accepted.
    /// </summary>
    [Fact]
    public void RevokeMultipleShouldWork()
    {
        var args = new[] { "principal", "DOMAIN\\UserOrGroup", "--revoke", "SeServiceLogonRight", "--revoke", "SeBatchLogonRight" };

        var rc = this.parser.Invoke(args);

        Assert.Equal(0, rc);
    }

    /// <summary>
    /// Ensures revoke other privileges without granting a privilege is rejected.
    /// </summary>
    [Fact]
    public void RevokeOthersWithOutGrantsThrowsException()
    {
        var args = new[] { "principal", "DOMAIN\\UserOrGroup", "--revoke-others" };

        Assert.Throws<SyntaxException>(() => this.parser.Invoke(args));
    }

    /// <summary>
    /// Ensures revoke other privileges with revoking a privilege is rejected.
    /// </summary>
    [Fact]
    public void RevokeOthersWithRevocationsThrowsException()
    {
        var args = new[] { "principal", "DOMAIN\\UserOrGroup", "--revoke-others", "--revoke", "SeServiceLogonRight" };

        Assert.Throws<SyntaxException>(() => this.parser.Invoke(args));
    }

    /// <summary>
    /// Ensures revoking a privilege is accepted.
    /// </summary>
    [Fact]
    public void RevokeShouldWork()
    {
        var args = new[] { "principal", "DOMAIN\\UserOrGroup", "--revoke", "SeServiceLogonRight" };

        var rc = this.parser.Invoke(args);

        Assert.Equal(0, rc);
    }

    /// <summary>
    /// Ensures an empty or whitespace revocation is rejected.
    /// </summary>
    /// <param name="args">The test arguments.</param>
    [Theory]
    [InlineData("principal", "DOMAIN\\UserOrGroup", "--revoke", "")]
    [InlineData("principal", "DOMAIN\\UserOrGroup", "--revoke", " ")]
    public void RevokeWithInvalidStringThrowsException(params string[] args)
        => Assert.Throws<SyntaxException>(() => this.parser.Invoke(args));

    /// <summary>
    /// Ensures an empty or whitespace system name is rejected.
    /// </summary>
    /// <param name="args">The test arguments.</param>
    [Theory]
    [InlineData("principal", "DOMAIN\\UserOrGroup", "--grant", "SeServiceLogonRight", "--system-name", "")]
    [InlineData("principal", "DOMAIN\\UserOrGroup", "--grant", "SeServiceLogonRight", "--system-name", " ")]
    public void SystemNameWithInvalidStringThrowsException(params string[] args)
        => Assert.Throws<SyntaxException>(() => this.parser.Invoke(args));
}