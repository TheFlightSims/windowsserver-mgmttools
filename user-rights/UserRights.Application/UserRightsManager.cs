namespace UserRights.Application;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using UserRights.Extensions.Security;

/// <summary>
/// Represents the applications logic.
/// </summary>
public class UserRightsManager : IUserRightsManager
{
    private readonly ILogger logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserRightsManager"/> class.
    /// </summary>
    /// <param name="logger">The logging instance.</param>
    public UserRightsManager(ILogger<UserRightsManager> logger) => this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

    /// <inheritdoc />
    public IEnumerable<IUserRightEntry> GetUserRights(IUserRights policy)
    {
        if (policy is null)
        {
            throw new ArgumentNullException(nameof(policy));
        }

        // Enumerate the principals with privileges.
        var principals = policy.LsaEnumerateAccountsWithUserRight();

        // Enumerate the privileges for each principal.
        var entries = new List<UserRightEntry>();
        foreach (var principal in principals)
        {
            string? accountName = default;
            try
            {
                accountName = principal.ToAccountName();
            }
            catch (SecurityTranslationException)
            {
                // Account may be unknown locally if querying a remote host, or it may have been deleted.
            }

            var privileges = policy.LsaEnumerateAccountRights(principal);
            foreach (var privilege in privileges)
            {
                var record = new UserRightEntry(privilege, principal.Value, accountName);
                entries.Add(record);
            }
        }

        // Sort the data.
        var sorted = entries
            .OrderBy(p => p.Privilege)
            .ThenBy(p => p.SecurityId)
            .ToArray();

        return sorted;
    }

    /// <inheritdoc />
    public void ModifyPrincipal(IUserRights policy, string principal, string[] grants, string[] revocations, bool revokeAll, bool revokeOthers, bool dryRun)
    {
        if (policy is null)
        {
            throw new ArgumentNullException(nameof(policy));
        }

        if (string.IsNullOrWhiteSpace(principal))
        {
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(principal));
        }

        if (grants is null)
        {
            throw new ArgumentNullException(nameof(grants));
        }

        if (revocations is null)
        {
            throw new ArgumentNullException(nameof(revocations));
        }

        if (revokeAll && (revokeOthers || grants.Length > 0 || revocations.Length > 0))
        {
            throw new ArgumentException($"The {nameof(revokeAll)} parameter cannot be used with any other option.", nameof(revokeAll));
        }

        if (revokeOthers && (revokeAll || grants.Length == 0 || revocations.Length > 0))
        {
            throw new ArgumentException($"The {nameof(revokeOthers)} parameter is only valid with {nameof(grants)}.", nameof(revokeAll));
        }

        if (grants.Length == 0 && revocations.Length == 0 && !revokeAll)
        {
            throw new ArgumentException("The parameter combination is invalid.");
        }

        // Convert the grants to a set.
        var grantSet = grants.ToHashSet(StringComparer.InvariantCultureIgnoreCase);

        // Convert the revocations to a set.
        var revocationSet = revocations.ToHashSet(StringComparer.InvariantCultureIgnoreCase);

        if (grantSet.Overlaps(revocationSet))
        {
            throw new ArgumentException("The grants and revocations cannot overlap.");
        }

        if (grants.Length != grantSet.Count)
        {
            throw new ArgumentException("The grants cannot contain duplicates.", nameof(grants));
        }

        if (revocations.Length != revocationSet.Count)
        {
            throw new ArgumentException("The revocations cannot contain duplicates.", nameof(revocations));
        }

        // Convert the principal to a security identifier.
        var securityIdentifier = principal.ToSecurityIdentifier();

        // Get the privileges currently assigned to the principal.
        var privileges = policy.LsaEnumerateAccountRights(securityIdentifier).ToHashSet(StringComparer.InvariantCultureIgnoreCase);

        // Perform a full revocation of all privileges if required.
        if (revokeAll)
        {
            foreach (var privilege in privileges)
            {
                this.RevokePrivilege(policy, securityIdentifier, privilege, dryRun);
            }

            // Ignore any further processing.
            return;
        }

        // Determine the deficit grants.
        var deficit = grantSet.Where(p => !privileges.Contains(p));

        // Determine the surplus grants.
        var surplus = revocationSet.Where(privileges.Contains);

        // Perform a revocation of all privileges except those being granted if required.
        if (revokeOthers)
        {
            var others = privileges.Where(p => !grantSet.Contains(p));
            foreach (var privilege in others)
            {
                this.RevokePrivilege(policy, securityIdentifier, privilege, dryRun);
            }
        }

        // Grant any deficit privileges.
        foreach (var privilege in deficit)
        {
            this.GrantPrivilege(policy, securityIdentifier, privilege, dryRun);
        }

        // Revoke any surplus privileges.
        foreach (var privilege in surplus)
        {
            this.RevokePrivilege(policy, securityIdentifier, privilege, dryRun);
        }
    }

    /// <inheritdoc />
    public void ModifyPrivilege(IUserRights policy, string privilege, string[] grants, string[] revocations, bool revokeAll, bool revokeOthers, Regex? revokePattern, bool dryRun)
    {
        if (policy is null)
        {
            throw new ArgumentNullException(nameof(policy));
        }

        if (grants is null)
        {
            throw new ArgumentNullException(nameof(grants));
        }

        if (revocations is null)
        {
            throw new ArgumentNullException(nameof(revocations));
        }

        if (string.IsNullOrWhiteSpace(privilege))
        {
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(privilege));
        }

        if (revokeAll && (grants.Length > 0 || revocations.Length > 0 || revokeOthers || revokePattern != null))
        {
            throw new ArgumentException($"The {nameof(revokeAll)} parameter cannot be used with any other option.", nameof(revokeAll));
        }

        if (revokeOthers && (grants.Length == 0 || revocations.Length > 0 || revokeAll || revokePattern != null))
        {
            throw new ArgumentException($"The {nameof(revokeOthers)} parameter is only valid with {nameof(grants)}.", nameof(revokeOthers));
        }

        if (revokePattern != null && (revocations.Length > 0 || revokeAll || revokeOthers))
        {
            throw new ArgumentException($"The {nameof(revokePattern)} parameter cannot be used with {nameof(revocations)}, {nameof(revokeAll)}, or {nameof(revokeOthers)}.", nameof(revokePattern));
        }

        if (grants.Length == 0 && revocations.Length == 0 && !revokeAll && revokePattern == null)
        {
            throw new ArgumentException("The parameter combination is invalid.");
        }

        // Translate the principal for each grant to a security identifier.
        var grantSet = grants.Select(p => p.ToSecurityIdentifier()).ToHashSet();

        // Translate the principal for each revocation to a security identifier.
        var revocationSet = revocations.Select(p => p.ToSecurityIdentifier()).ToHashSet();

        if (grantSet.Overlaps(revocationSet))
        {
            throw new ArgumentException("The grants and revocations cannot overlap.");
        }

        if (grants.Length != grantSet.Count)
        {
            throw new ArgumentException("The grants cannot contain duplicates.", nameof(grants));
        }

        if (revocations.Length != revocationSet.Count)
        {
            throw new ArgumentException("The revocations cannot contain duplicates.", nameof(revocations));
        }

        // Get the principals with the privilege currently assigned.
        var principals = policy.LsaEnumerateAccountsWithUserRight(privilege).ToHashSet();

        // Perform a full revocation of the privilege from each principal if required.
        if (revokeAll)
        {
            foreach (var principal in principals)
            {
                this.RevokePrivilege(policy, principal, privilege, dryRun);
            }

            // Ignore any further processing.
            return;
        }

        // Determine the deficit grants.
        var deficit = grantSet.Where(p => !principals.Contains(p));

        // Determine the surplus grants.
        var surplus = revocationSet.Where(principals.Contains);

        // Perform a revocation of the privilege from each principal except those being granted if required.
        if (revokeOthers)
        {
            var others = principals.Where(p => !grantSet.Contains(p));
            foreach (var principal in others)
            {
                this.RevokePrivilege(policy, principal, privilege, dryRun);
            }
        }

        // Perform a revocation of the privilege from each principal matching the regex pattern if required.
        if (revokePattern != null)
        {
            var matches = principals.Where(p => !grantSet.Contains(p) && revokePattern.IsMatch(p.Value));
            foreach (var principal in matches)
            {
                this.RevokePrivilege(policy, principal, privilege, dryRun);
            }
        }

        // Grant any deficit principals.
        foreach (var sid in deficit)
        {
            this.GrantPrivilege(policy, sid, privilege, dryRun);
        }

        // Revoke any surplus principals.
        foreach (var sid in surplus)
        {
            this.RevokePrivilege(policy, sid, privilege, dryRun);
        }
    }

    /// <summary>
    /// Grants a privilege to a principal.
    /// </summary>
    /// <param name="policy">A connection to the local security authority.</param>
    /// <param name="principal">The principal to grant the privilege to.</param>
    /// <param name="privilege">The privilege to grant.</param>
    /// <param name="dryRun">A value indicating whether to process the action or just instrument it.</param>
    private void GrantPrivilege(IUserRights policy, SecurityIdentifier principal, string privilege, bool dryRun)
    {
        if (principal is null)
        {
            throw new ArgumentNullException(nameof(principal));
        }

        if (string.IsNullOrWhiteSpace(privilege))
        {
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(privilege));
        }

        if (dryRun)
        {
            this.logger.LogInformation(OperationId.PrivilegeGrantDryrun, "Granting {Privilege:l} to {Principal}.", privilege, principal);

            return;
        }

        try
        {
            policy.LsaAddAccountRights(principal, privilege);
        }
        catch
        {
            this.logger.LogError(OperationId.PrivilegeGrantFailure, "Failed to grant {Privilege:l} to {Principal}.", privilege, principal);

            throw;
        }

        this.logger.LogInformation(OperationId.PrivilegeGrantSuccess, "Successfully granted {Privilege:l} to {Principal}.", privilege, principal);
    }

    /// <summary>
    /// Revokes a privilege from a principal.
    /// </summary>
    /// <param name="policy">A connection to the local security authority.</param>
    /// <param name="principal">The principal to revoke the privilege from.</param>
    /// <param name="privilege">The privilege to revoke.</param>
    /// <param name="dryRun">A value indicating whether to process the action or just instrument it.</param>
    private void RevokePrivilege(IUserRights policy, SecurityIdentifier principal, string privilege, bool dryRun)
    {
        if (principal is null)
        {
            throw new ArgumentNullException(nameof(principal));
        }

        if (string.IsNullOrWhiteSpace(privilege))
        {
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(privilege));
        }

        if (dryRun)
        {
            this.logger.LogInformation(OperationId.PrivilegeRevokeDryrun, "Revoking {Privilege:l} from {Principal}.", privilege, principal);

            return;
        }

        try
        {
            policy.LsaRemoveAccountRights(principal, privilege);
        }
        catch
        {
            this.logger.LogError(OperationId.PrivilegeRevokeFailure, "Failed to revoke {Privilege:l} from {Principal}.", privilege, principal);

            throw;
        }

        this.logger.LogInformation(OperationId.PrivilegeRevokeSuccess, "Successfully revoked {Privilege:l} from {Principal}.", privilege, principal);
    }
}