namespace Tests;

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UserRights.Application;

/// <summary>
/// Represents a mock user rights manager with noop interface implementations.
/// </summary>
public class MockUserRightsManager : IUserRightsManager
{
    /// <inheritdoc />
    public IEnumerable<IUserRightEntry> GetUserRights(IUserRights policy) => Enumerable.Empty<IUserRightEntry>();

    /// <inheritdoc />
    public void ModifyPrincipal(IUserRights policy, string principal, string[] grants, string[] revocations, bool revokeAll, bool revokeOthers, bool dryRun)
    {
    }

    /// <inheritdoc />
    public void ModifyPrivilege(IUserRights policy, string privilege, string[] grants, string[] revocations, bool revokeAll, bool revokeOthers, Regex? revokePattern, bool dryRun)
    {
    }
}