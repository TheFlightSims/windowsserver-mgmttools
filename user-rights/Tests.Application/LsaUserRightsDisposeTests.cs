namespace Tests.Application;

using UserRights.Application;
using Xunit;

/// <summary>
/// Represents tests for <see cref="LsaUserRights"/> disposal functionality.
/// </summary>
[Collection("lsa")]
public class LsaUserRightsDisposeTests
{
    /// <summary>
    /// Tests whether dispose can be successfully called multiple times.
    /// </summary>
    [Fact]
    public void CanBeDisposedMultipleTimes()
    {
        using var policy = new LsaUserRights();

        policy.Dispose();

        var exception = Record.Exception(policy.Dispose);

        Assert.Null(exception);
    }
}