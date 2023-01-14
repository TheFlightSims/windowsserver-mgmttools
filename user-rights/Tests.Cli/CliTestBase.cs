namespace Tests.Cli;

using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

/// <summary>
/// Represents a test configuration that provides command line infrastructure.
/// </summary>
public abstract class CliTestBase : IDisposable
{
    private readonly IServiceCollection serviceCollection;
    private readonly Lazy<ServiceProvider> serviceProvider;

    private bool disposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="CliTestBase"/> class.
    /// </summary>
    protected CliTestBase()
    {
        this.serviceCollection = new ServiceCollection()
            .AddLogging(builder => builder
                .ClearProviders()
                .SetMinimumLevel(LogLevel.Trace)
                .AddDebug());

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
            this.serviceProvider.Value.Dispose();
            this.disposed = true;
        }
    }
}