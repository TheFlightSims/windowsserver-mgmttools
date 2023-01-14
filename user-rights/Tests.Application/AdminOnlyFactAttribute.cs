namespace Tests.Application;

using Xunit;

/// <summary>
/// Represents a test fact that signals the runner to skip the test for non administrative users.
/// </summary>
public sealed class AdminOnlyFactAttribute : FactAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AdminOnlyFactAttribute"/> class.
    /// </summary>
    public AdminOnlyFactAttribute()
    {
        using var identity = System.Security.Principal.WindowsIdentity.GetCurrent();
        var principal = new System.Security.Principal.WindowsPrincipal(identity);

        if (principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator))
        {
            return;
        }

        this.Skip = "Current user is not an administrator.";
    }
}