namespace Tests;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using UserRights.Application;

/// <summary>
/// Represents a mock <see cref="ILsaUserRights"/> implementation.
/// </summary>
public class MockLsaUserRights : ILsaUserRights
{
    private readonly IDictionary<string, ICollection<SecurityIdentifier>> database;
    private bool connected;

    /// <summary>
    /// Initializes a new instance of the <see cref="MockLsaUserRights"/> class.
    /// </summary>
    public MockLsaUserRights() => this.database = new Dictionary<string, ICollection<SecurityIdentifier>>(StringComparer.InvariantCultureIgnoreCase);

    /// <summary>
    /// Initializes a new instance of the <see cref="MockLsaUserRights"/> class.
    /// </summary>
    /// <param name="database">A map of privilege to assigned principals.</param>
    public MockLsaUserRights(IDictionary<string, ICollection<SecurityIdentifier>> database)
        => this.database = database ?? throw new ArgumentNullException(nameof(database));

    /// <inheritdoc />
    public void Connect(string? systemName = default)
    {
        if (this.connected)
        {
            throw new InvalidOperationException("A connection to the policy database already exists.");
        }

        this.connected = true;
    }

    /// <inheritdoc />
    public void LsaAddAccountRights(SecurityIdentifier accountSid, params string[] userRights)
    {
        ArgumentNullException.ThrowIfNull(accountSid);
        ArgumentNullException.ThrowIfNull(userRights);

        if (userRights.Length == 0)
        {
            throw new ArgumentException("Value cannot be an empty collection.", nameof(userRights));
        }

        if (!this.connected)
        {
            throw new InvalidOperationException("A connection to the policy database is required.");
        }

        foreach (var userRight in userRights)
        {
            if (this.database.TryGetValue(userRight, out var accountSids))
            {
                if (!accountSids.Contains(accountSid))
                {
                    accountSids.Add(accountSid);
                }
            }
            else
            {
                accountSids = new HashSet<SecurityIdentifier>
                {
                    accountSid
                };

                this.database.Add(userRight, accountSids);
            }
        }
    }

    /// <inheritdoc />
    public string[] LsaEnumerateAccountRights(SecurityIdentifier accountSid)
    {
        if (accountSid is null)
        {
            throw new ArgumentNullException(nameof(accountSid));
        }

        if (!this.connected)
        {
            throw new InvalidOperationException("A connection to the policy database is required.");
        }

        return this.database
            .Where(p => p.Value.Contains(accountSid))
            .Select(p => p.Key)
            .ToArray();
    }

    /// <inheritdoc />
    public SecurityIdentifier[] LsaEnumerateAccountsWithUserRight(string? userRight = default)
    {
        if (!this.connected)
        {
            throw new InvalidOperationException("A connection to the policy database is required.");
        }

        if (string.IsNullOrWhiteSpace(userRight))
        {
            return this.database.Values.SelectMany(p => p).Distinct().ToArray();
        }

        if (this.database.TryGetValue(userRight, out var accountSids))
        {
            return accountSids.ToArray();
        }

        return Array.Empty<SecurityIdentifier>();
    }

    /// <inheritdoc />
    public void LsaRemoveAccountRights(SecurityIdentifier accountSid, params string[] userRights)
    {
        ArgumentNullException.ThrowIfNull(accountSid);
        ArgumentNullException.ThrowIfNull(userRights);

        if (userRights.Length == 0)
        {
            throw new ArgumentException("Value cannot be an empty collection.", nameof(userRights));
        }

        if (!this.connected)
        {
            throw new InvalidOperationException("A connection to the policy database is required.");
        }

        foreach (var userRight in userRights)
        {
            if (this.database.TryGetValue(userRight, out var principals) && principals.Contains(accountSid))
            {
                principals.Remove(accountSid);
            }
        }
    }

    /// <summary>
    /// Allow a test to assert the policy database before manipulating it.
    /// </summary>
    public void ResetConnection() => this.connected = false;
}