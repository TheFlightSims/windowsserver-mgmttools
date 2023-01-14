namespace Tests;

using System;
using System.Collections.Generic;
using UserRights.Application;

/// <summary>
/// Represents the equality comparison for <see cref="IUserRightEntry"/> objects.
/// </summary>
public sealed class UserRightEntryEqualityComparer : IEqualityComparer<IUserRightEntry>
{
    /// <inheritdoc />
    public bool Equals(IUserRightEntry? x, IUserRightEntry? y)
    {
        if (ReferenceEquals(x, y))
        {
            return true;
        }

        if (x is null)
        {
            return false;
        }

        if (y is null)
        {
            return false;
        }

        if (x.GetType() != y.GetType())
        {
            return false;
        }

        return string.Equals(x.Privilege, y.Privilege, StringComparison.Ordinal)
            && string.Equals(x.SecurityId, y.SecurityId, StringComparison.Ordinal)
            && string.Equals(x.AccountName, y.AccountName, StringComparison.Ordinal);
    }

    /// <inheritdoc />
    public int GetHashCode(IUserRightEntry obj)
    {
        if (obj is null)
        {
            throw new ArgumentNullException(nameof(obj));
        }

        unchecked
        {
            var hashCode = StringComparer.Ordinal.GetHashCode(obj.Privilege);
            hashCode = (hashCode * 397) ^ StringComparer.Ordinal.GetHashCode(obj.SecurityId);

            if (obj.AccountName is not null)
            {
                hashCode = (hashCode * 397) ^ StringComparer.Ordinal.GetHashCode(obj.AccountName);
            }

            return hashCode;
        }
    }
}