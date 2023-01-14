namespace UserRights.Application;

/// <summary>
/// Represents the interface for a user right entry in the local security database.
/// </summary>
public interface IUserRightEntry
{
    /// <summary>
    /// Gets the privilege assigned to the principal.
    /// </summary>
    string Privilege { get; }

    /// <summary>
    /// Gets the security id of the principal.
    /// </summary>
    string SecurityId { get; }

    /// <summary>
    /// Gets the account name of the principal.
    /// </summary>
    /// <remarks>
    /// The account name may be empty if the query was performed remotely due to the translation possibly not working.
    /// </remarks>
    string? AccountName { get; }
}