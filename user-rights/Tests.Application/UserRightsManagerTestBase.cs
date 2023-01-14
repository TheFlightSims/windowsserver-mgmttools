namespace Tests.Application;

using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using UserRights.Application;

/// <summary>
/// Represents the test base for <see cref="UserRightsManager"/> application.
/// </summary>
public abstract class UserRightsManagerTestBase : IDisposable
{
    private readonly IServiceCollection serviceCollection;
    private readonly Lazy<ServiceProvider> serviceProvider;

    private bool disposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserRightsManagerTestBase"/> class.
    /// </summary>
    protected UserRightsManagerTestBase()
    {
        this.serviceCollection = new ServiceCollection()
            .AddSingleton<IUserRightsManager, UserRightsManager>()
            .AddLogging(builder => builder
                .ClearProviders()
                .SetMinimumLevel(LogLevel.Trace)
                .AddDebug());

        // Defer the creation until the instance is accessed to allow inheritors to modify the service collection.
        this.serviceProvider = new Lazy<ServiceProvider>(this.serviceCollection.BuildServiceProvider);
    }

    /// <summary>
    /// Gets the service collection.
    /// </summary>
    protected IServiceCollection ServiceCollection
    {
        get
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }

            return this.serviceCollection;
        }
    }

    /// <summary>
    /// Gets the service provider.
    /// </summary>
    protected ServiceProvider ServiceProvider
    {
        get
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }

            return this.serviceProvider.Value;
        }
    }

    /// <inheritdoc />
    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Releases resources when they are no longer required.
    /// </summary>
    /// <param name="disposing">A value indicating whether the method call comes from a dispose method (its value is <c>true</c>) or from a finalizer (its value is <c>false</c>).</param>
    protected virtual void Dispose(bool disposing)
    {
        if (this.disposed)
        {
            return;
        }

        if (disposing)
        {
            if (this.serviceProvider.IsValueCreated)
            {
                this.serviceProvider.Value.Dispose();
            }

            this.disposed = true;
        }
    }
}