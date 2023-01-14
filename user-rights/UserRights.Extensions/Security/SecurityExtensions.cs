namespace UserRights.Extensions.Security;

using System;
using System.Security.Principal;

/// <summary>
/// Represents LSA user right and security extensions.
/// </summary>
public static class SecurityExtensions
{
    /// <summary>
    /// Gets the security identifier (SID) for specified account name.
    /// </summary>
    /// <param name="accountName">The account name to translate.</param>
    /// <returns>The security identifier (SID).</returns>
    public static SecurityIdentifier ToSecurityIdentifier(this string accountName)
    {
        if (string.IsNullOrWhiteSpace(accountName))
        {
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(accountName));
        }

        try
        {
            var account = new NTAccount(accountName);

            return (SecurityIdentifier)account.Translate(typeof(SecurityIdentifier));
        }
        catch (Exception e)
        {
            throw new SecurityTranslationException($"Error translating account name {accountName} to a security identifier (SID), the account may be unknown on the host.", e);
        }
    }

    /// <summary>
    /// Gets the account name for the specified security identifier (SID).
    /// </summary>
    /// <param name="securityIdentifier">The security identifier (SID) to translate.</param>
    /// <returns>The account name.</returns>
    public static string ToAccountName(this SecurityIdentifier securityIdentifier)
    {
        if (securityIdentifier is null)
        {
            throw new ArgumentNullException(nameof(securityIdentifier));
        }

        try
        {
            return securityIdentifier.Translate(typeof(NTAccount)).Value;
        }
        catch (Exception e)
        {
            throw new SecurityTranslationException($"Error translating security identifier (SID) {securityIdentifier.Value} to an account name, the SID may be unknown on the host.", e);
        }
    }
}