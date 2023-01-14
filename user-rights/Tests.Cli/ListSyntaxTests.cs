namespace Tests.Cli;

using System.CommandLine.Parsing;
using Microsoft.Extensions.DependencyInjection;
using UserRights.Application;
using UserRights.Cli;
using Xunit;

/// <summary>
/// Represents syntax tests for list functionality.
/// </summary>
public sealed class ListSyntaxTests : CliTestBase
{
    private readonly Parser parser;

    /// <summary>
    /// Initializes a new instance of the <see cref="ListSyntaxTests"/> class.
    /// </summary>
    public ListSyntaxTests()
    {
        this.ServiceCollection.AddSingleton<ILsaUserRights, MockLsaUserRights>();
        this.ServiceCollection.AddSingleton<IUserRightsManager, MockUserRightsManager>();
        this.ServiceCollection.AddSingleton<CliBuilder>();

        var cliBuilder = this.ServiceProvider.GetRequiredService<CliBuilder>();

        var commandLineBuilder = cliBuilder.Create();
        this.parser = commandLineBuilder.Build();
    }

    /// <summary>
    /// Verifies list mode with CSV formatted output sent to STDOUT is parsed successfully.
    /// </summary>
    [Fact]
    public void CsvToStdoutShouldWork()
    {
        var args = new[] { "list" };

        var rc = this.parser.Invoke(args);

        Assert.Equal(0, rc);
    }

    /// <summary>
    /// Verifies list mode with CSV formatted output sent to a file is parsed successfully.
    /// </summary>
    [Fact]
    public void CsvToPathShouldWork()
    {
        var args = new[] { "list", "--path", "file.csv" };

        var rc = this.parser.Invoke(args);

        Assert.Equal(0, rc);
    }

    /// <summary>
    /// Ensures an empty or whitespace path is rejected.
    /// </summary>
    /// <param name="args">The test arguments.</param>
    [Theory]
    [InlineData("list", "--path", "")]
    [InlineData("list", "--path", " ")]
    public void PathWithInvalidStringThrowsException(params string[] args)
        => Assert.Throws<SyntaxException>(() => this.parser.Invoke(args));

    /// <summary>
    /// Ensures an empty or whitespace system name is rejected.
    /// </summary>
    /// <param name="args">The test arguments.</param>
    [Theory]
    [InlineData("list", "--system-name", "")]
    [InlineData("list", "--system-name", " ")]
    public void SystemNameWithInvalidStringThrowsException(params string[] args)
        => Assert.Throws<SyntaxException>(() => this.parser.Invoke(args));

    /// <summary>
    /// Verifies list mode with JSON formatted output sent to STDOUT is parsed successfully.
    /// </summary>
    [Fact]
    public void JsonToStdoutShouldWork()
    {
        var args = new[] { "list", "--json" };

        var rc = this.parser.Invoke(args);

        Assert.Equal(0, rc);
    }

    /// <summary>
    /// Verifies list mode with JSON formatted output sent to a file is parsed successfully.
    /// </summary>
    [Fact]
    public void JsonToPathShouldWork()
    {
        var args = new[] { "list", "--json", "--path", "file.csv" };

        var rc = this.parser.Invoke(args);

        Assert.Equal(0, rc);
    }
}