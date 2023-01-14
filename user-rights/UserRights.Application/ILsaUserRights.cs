namespace UserRights.Application;

/// <summary>
/// Represents the interface to the local security authority.
/// </summary>
public interface ILsaUserRights : IUserRights
{
    /// <summary>
    /// Connects to the local security authority.
    /// </summary>
    /// <param name="systemName">The remote system name to execute the task on (default localhost).</param>
    void Connect(string? systemName = default);
}