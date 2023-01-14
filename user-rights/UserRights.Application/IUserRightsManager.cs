namespace UserRights.Application;

using System.Collections.Generic;
using System.Text.RegularExpressions;

/// <summary>
/// Represents the interface to the user rights application.
/// </summary>
public interface IUserRightsManager
{
    /// <summary>
    /// Gets all privileges for all principals.
    /// </summary>
    /// <param name="policy">A connection to the local security authority.</param>
    /// <returns>A sequence of all user rights.</returns>
    IEnumerable<IUserRightEntry> GetUserRights(IUserRights policy);

    /// <summary>
    /// Modifies the specified principal.
    /// </summary>
    /// <param name="policy">A connection to the local security authority.</param>
    /// <param name="principal">The principal to modify.</param>
    /// <param name="grants">The privileges to grant to the principal.</param>
    /// <param name="revocations">The privileges to revoke from the principal.</param>
    /// <param name="revokeAll">Revokes all privileges from the principal.</param>
    /// <param name="revokeOthers">Revokes all privileges from the principal excluding those being granted.</param>
    /// <param name="dryRun">Enables dry-run mode.</param>
    /// <remarks>
    /// <para>
    /// The following assumptions are expected from the input settings.
    /// </para>
    /// <list type="bullet">
    /// <item>
    /// The <c>revokeAll</c> parameter cannot be combined with any other parameter.
    /// </item>
    /// <item>
    /// The <c>revokeOthers</c> parameter requires the <c>grants</c> parameter, and revokes all principals provisioned except those being granted.
    /// </item>
    /// <item>
    /// The <c>revokeOthers</c> parameter cannot be combined with the <c>revocations</c>, or the <c>revokeAll</c> parameter.
    /// </item>
    /// <item>
    /// The <c>grants</c> and <c>revocations</c> parameters must be distinct without overlap, and can be combined.
    /// </item>
    /// <item>
    /// The <c>grants</c> parameter only grants deficit privileges.
    /// </item>
    /// <item>
    /// The <c>revocations</c> parameter only revokes provisioned privileges.
    /// </item>
    /// </list>
    /// </remarks>
    void ModifyPrincipal(IUserRights policy, string principal, string[] grants, string[] revocations, bool revokeAll, bool revokeOthers, bool dryRun);

    /// <summary>
    /// Modifies the specified privilege.
    /// </summary>
    /// <param name="policy">A connection to the local security authority.</param>
    /// <param name="privilege">The privilege to modify.</param>
    /// <param name="grants">The principals to grant the privilege to.</param>
    /// <param name="revocations">The principals to revoke the privilege from.</param>
    /// <param name="revokeAll">Revokes all principals from the privilege.</param>
    /// <param name="revokeOthers">Revokes all principals from the privilege excluding those being granted.</param>
    /// <param name="revokePattern">Revokes all principals whose SID matches the regular expression excluding those being granted.</param>
    /// <param name="dryRun">Enables dry-run mode.</param>
    /// <remarks>
    /// <para>
    /// The following assumptions are expected from the input options.
    /// </para>
    /// <list type="bullet">
    /// <item>
    /// The <c>revokeAll</c> parameter cannot be combined with any other parameter.
    /// </item>
    /// <item>
    /// The <c>revokeOthers</c> parameter requires the <c>grants</c> parameter, and revokes all principals provisioned except those being granted.
    /// </item>
    /// <item>
    /// The <c>revokeOthers</c> parameter cannot be combined with the <c>revocations</c>, <c>revokeAll</c>, or the <c>revokePattern</c> parameter.
    /// </item>
    /// <item>
    /// The <c>revokePattern</c> parameter cannot be combined with the <c>revocations</c>, <c>revokeAll</c>, or the <c>revokeOthers</c> parameter.
    /// </item>
    /// <item>
    /// The <c>grants</c> and <c>revocations</c> parameters must be distinct without overlap, and can be combined.
    /// </item>
    /// <item>
    /// The <c>grants</c> parameter only grants principals in deficit.
    /// </item>
    /// <item>
    /// The <c>revocations</c> parameter only revokes principals that are provisioned.
    /// </item>
    /// </list>
    /// </remarks>
    void ModifyPrivilege(IUserRights policy, string privilege, string[] grants, string[] revocations, bool revokeAll, bool revokeOthers, Regex? revokePattern, bool dryRun);
}