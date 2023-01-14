namespace Tests.Application;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text.Json;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserRights.Application;
using UserRights.Extensions.Security;
using UserRights.Extensions.Serialization;
using Xunit;
using static Tests.TestData;

/// <summary>
/// Represents tests for <see cref="IUserRightsManager"/> list functionality.
/// </summary>
public sealed class UserRightsManagerListTests : UserRightsManagerTestBase
{
    /// <summary>
    /// Verifies invalid arguments throw an instance of <see cref="ArgumentException"/>.
    /// </summary>
    [Fact]
    public void InvalidArgumentsThrowsException()
    {
        var manager = this.ServiceProvider.GetRequiredService<IUserRightsManager>();

        Assert.ThrowsAny<ArgumentException>(() => manager.GetUserRights(null!));
    }

    /// <summary>
    /// Verifies listing user rights and serializing the output to a CSV.
    /// </summary>
    [Fact]
    public void SerializingToCsvShouldWork()
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
        policy.Connect("SystemName");

        Assert.Equal(new[] { PrincipalSid1, PrincipalSid2 }, policy.LsaEnumerateAccountsWithUserRight().OrderBy(p => p));
        Assert.Equal(new[] { Privilege1, Privilege2 }, policy.LsaEnumerateAccountRights(PrincipalSid1));
        Assert.Equal(new[] { Privilege1, Privilege2 }, policy.LsaEnumerateAccountRights(PrincipalSid2));

        var manager = this.ServiceProvider.GetRequiredService<IUserRightsManager>();
        var userRights = manager.GetUserRights(policy).ToArray();

        Assert.Equal(expected, userRights, new UserRightEntryEqualityComparer());

        var serialized = userRights.ToCsv();

        var configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            PrepareHeaderForMatch = a => a.Header.ToUpperInvariant()
        };

        using var stringReader = new StringReader(serialized);
        using var csvReader = new CsvReader(stringReader, configuration);

        var actual = csvReader.GetRecords<UserRightEntry>()
            .OrderBy(p => p.Privilege)
            .ThenBy(p => p.SecurityId)
            .ToArray();

        Assert.Equal(expected, actual, new UserRightEntryEqualityComparer());
    }

    /// <summary>
    /// Verifies listing user rights and serializing the output to a JSON.
    /// </summary>
    [Fact]
    public void SerializingToJsonShouldWork()
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
        policy.Connect("SystemName");

        Assert.Equal(new[] { PrincipalSid1, PrincipalSid2 }, policy.LsaEnumerateAccountsWithUserRight().OrderBy(p => p));
        Assert.Equal(new[] { Privilege1, Privilege2 }, policy.LsaEnumerateAccountRights(PrincipalSid1));
        Assert.Equal(new[] { Privilege1, Privilege2 }, policy.LsaEnumerateAccountRights(PrincipalSid2));

        var manager = this.ServiceProvider.GetRequiredService<IUserRightsManager>();
        var userRights = manager.GetUserRights(policy).ToArray();

        Assert.Equal(expected, userRights, new UserRightEntryEqualityComparer());

        var serialized = userRights.ToJson();

        var actual = JsonSerializer.Deserialize<UserRightEntry[]>(serialized)
            ?.OrderBy(p => p.Privilege)
            .ThenBy(p => p.SecurityId)
            .ToArray() ?? Array.Empty<UserRightEntry>();

        Assert.Equal(expected, actual, new UserRightEntryEqualityComparer());
    }
}