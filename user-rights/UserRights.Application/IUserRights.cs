namespace UserRights.Application;

using System.Security.Principal;

/// <summary>
/// Represents the interface to the local security authority user right functions.
/// </summary>
public interface IUserRights
{
    /// <summary>
    /// Assigns one or more privileges to an account.
    /// </summary>
    /// <param name="accountSid">The security identifier (SID) of the account to add the privileges to.</param>
    /// <param name="userRights">The names of the privileges to add to the account.</param>
    void LsaAddAccountRights(SecurityIdentifier accountSid, params string[] userRights);

    /// <summary>
    /// Gets the privileges assigned to an account.
    /// </summary>
    /// <param name="accountSid">The SID of the account for which to enumerate privileges.</param>
    /// <returns>The names of the assigned privileges.</returns>
    string[] LsaEnumerateAccountRights(SecurityIdentifier accountSid);

    /// <summary>
    /// Gets the accounts in the database of a Local Security Authority (LSA) Policy object that hold a specified privilege.
    /// </summary>
    /// <param name="userRight">The name of a privilege.</param>
    /// <returns>The security identifier (SID) of each account that holds the specified privilege.</returns>
    SecurityIdentifier[] LsaEnumerateAccountsWithUserRight(string? userRight = default);

    /// <summary>
    /// Removes one or more privileges from an account.
    /// </summary>
    /// <param name="accountSid">The security identifier (SID) of the account to remove the privileges from.</param>
    /// <param name="userRights">The names of the privileges to remove from the account.</param>
    void LsaRemoveAccountRights(SecurityIdentifier accountSid, params string[] userRights);
}