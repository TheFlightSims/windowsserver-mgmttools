namespace Tests.Cli;

using System;
using System.Collections.Generic;
using System.CommandLine.Parsing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Text.Json;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserRights.Application;
using UserRights.Cli;
using UserRights.Extensions.Security;
using Xunit;
using static Tests.TestData;

/// <summary>
/// Represents integration tests for list functionality.
/// </summary>
public sealed class ListCommandTests : CliTestBase
{
    /// <summary>
    /// Verifies listing user rights to a JSON file.
    /// </summary>
    [Fact]
    public void PathAndJsonShouldWork()
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

        var expected = database
            .SelectMany(kvp => kvp.Value.Select(p => new UserRightEntry(kvp.Key, p.Value, p.ToAccountName())))
            .OrderBy(p => p.Privilege)
            .ThenBy(p => p.SecurityId)
            .ToArray();

        var policy = new MockLsaUserRights(database);

        this.ServiceCollection.AddSingleton<ILsaUserRights>(policy);
        this.ServiceCollection.AddSingleton<IUserRightsManager, UserRightsManager>();
        this.ServiceCollection.AddSingleton<CliBuilder>();

        var cliBuilder = this.ServiceProvider.GetRequiredService<CliBuilder>();

        var commandLineBuilder = cliBuilder.Create();
        var parser = commandLineBuilder.Build();

        var file = Path.GetTempFileName();
        var args = new[]
        {
            "list",
            "--json",
            "--path",
            file
        };

        int rc;
        UserRightEntry[] actual;
        try
        {
            rc = parser.Invoke(args);

            var json = File.ReadAllText(file, Encoding.UTF8);

            actual = JsonSerializer.Deserialize<UserRightEntry[]>(json)
                ?.OrderBy(p => p.Privilege)
                .ThenBy(p => p.SecurityId)
                .ToArray() ?? Array.Empty<UserRightEntry>();
        }
        finally
        {
            File.Delete(file);
        }

        Assert.Equal(0, rc);
        Assert.Equal(expected, actual, new UserRightEntryEqualityComparer());
    }

    /// <summary>
    /// Verifies listing user rights to a CSV file.
    /// </summary>
    [Fact]
    public void PathShouldWork()
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

        var expected = database
            .SelectMany(kvp => kvp.Value.Select(p => new UserRightEntry(kvp.Key, p.Value, p.ToAccountName())))
            .OrderBy(p => p.Privilege)
            .ThenBy(p => p.SecurityId)
            .ToArray();

        this.ServiceCollection.AddSingleton<ILsaUserRights>(policy);
        this.ServiceCollection.AddSingleton<IUserRightsManager, UserRightsManager>();
        this.ServiceCollection.AddSingleton<CliBuilder>();

        var cliBuilder = this.ServiceProvider.GetRequiredService<CliBuilder>();

        var commandLineBuilder = cliBuilder.Create();
        var parser = commandLineBuilder.Build();

        var file = Path.GetTempFileName();
        var args = new[]
        {
            "list",
            "--path",
            file
        };

        int rc;
        UserRightEntry[] actual;
        try
        {
            rc = parser.Invoke(args);

            var configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                PrepareHeaderForMatch = a => a.Header.ToUpperInvariant()
            };

            using var streamReader = new StreamReader(file);
            using var csvReader = new CsvReader(streamReader, configuration);

            actual = csvReader.GetRecords<UserRightEntry>()
                .OrderBy(p => p.Privilege)
                .ThenBy(p => p.SecurityId)
                .ToArray();
        }
        finally
        {
            File.Delete(file);
        }

        Assert.Equal(0, rc);
        Assert.Equal(expected, actual, new UserRightEntryEqualityComparer());
    }
}