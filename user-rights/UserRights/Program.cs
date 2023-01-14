namespace UserRights;

using System;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using Serilog.Events;
using Serilog.Templates;
using Serilog.Templates.Themes;
using UserRights.Application;
using UserRights.Cli;

/// <summary>
/// Implements the user right utility.
/// </summary>
internal static class Program
{
    /// <summary>
    /// Represents the programs main entry point.
    /// </summary>
    /// <param name="args">The command line arguments.</param>
    /// <returns>A value representing the operation status code.</returns>
    private static async Task<int> Main(string[] args)
    {
        int rc;

        // Wrap the execution with error handling.
        try
        {
            rc = await Run(args).ConfigureAwait(false);
        }
        catch (SyntaxException e)
        {
            // Log syntax errors to assist with instrumenting automation.
            using (LogContext.PushProperty("EventId", OperationId.SyntaxError))
            {
                Log.Fatal(e, "Syntax error.");
            }

            rc = 1;
        }
        catch (Exception e)
        {
            // Log all other errors as execution failures.
            using (LogContext.PushProperty("EventId", OperationId.FatalError))
            {
                Log.Fatal(e, "Execution failed.");
            }

            rc = 2;
        }
        finally
        {
            await Log.CloseAndFlushAsync().ConfigureAwait(false);
        }

        return rc;
    }

    /// <summary>
    /// Formats command line arguments.
    /// </summary>
    /// <param name="args">The command line arguments.</param>
    /// <returns>A formatted string representation of the arguments.</returns>
    private static string FormatArguments(string[] args)
    {
        if (args is null)
        {
            throw new ArgumentNullException(nameof(args));
        }

        var stringBuilder = new StringBuilder();

        stringBuilder.Append('[');

        foreach (var arg in args)
        {
            stringBuilder.AppendFormat(CultureInfo.InvariantCulture, " \"{0}\"", arg);
        }

        stringBuilder.Append(" ]");

        return stringBuilder.ToString();
    }

    /// <summary>
    /// Represents the programs execution logic.
    /// </summary>
    /// <param name="args">The command line arguments.</param>
    private static async Task<int> Run(string[] args)
    {
        if (args is null)
        {
            throw new ArgumentNullException(nameof(args));
        }

        // Use Coalesce to accomodate EventIds from Microsoft.Extensions.Logging and Serilog.
        const string consoleTemplate =
            "[{@t:yyyy-MM-dd HH:mm:ss.fff}] " +
            "[{@l}] " +
            "[{Coalesce(EventId.Id, EventId)}] " +
            "{#if DryRun = true}[DryRun] {#end}" +
            "{@m:l}\n" +
            "{#if ConsoleException is not null}{ConsoleException}\n{#end}";

        const string eventLogTemplate =
            "{@m:l}\n\n" +
            "Context: {EnvironmentUserName}\n" +
            "Process Id: {ProcessId}\n" +
            "Correlation Id: {CorrelationId}\n" +
            "Arguments: {Arguments}" +
            "{#if @x is not null}\n\n{@x}{#end}";

        // Configure the initial logging state with the console sink only enabled for warning level.
        var levelSwitch = new LoggingLevelSwitch { MinimumLevel = LogEventLevel.Warning };

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .Enrich.FromGlobalLogContext()
            .Enrich.FromLogContext()
            .Enrich.WithEnvironmentUserName()
            .Enrich.WithProcessId()
            .Enrich.WithProperty("Arguments", FormatArguments(args))
            .Enrich.WithProperty("CorrelationId", Guid.NewGuid())
            .Enrich.With<ConsoleExceptionEnricher>()
            .WriteTo.Console(
                new ExpressionTemplate(consoleTemplate, theme: TemplateTheme.Literate),
                levelSwitch: levelSwitch,
                standardErrorFromLevel: LogEventLevel.Verbose)
            .WriteTo.EventLog(
                new ExpressionTemplate(eventLogTemplate),
                nameof(UserRights),
                manageEventSource: true,
                eventIdProvider: new EventIdProvider())
            .CreateLogger();

        var serviceProvider = new ServiceCollection()
            .AddSingleton<ILsaUserRights, LsaUserRights>()
            .AddSingleton<IUserRightsManager, UserRightsManager>()
            .AddSingleton<CliBuilder>()
            .AddLogging(logging => logging.AddSerilog())
            .BuildServiceProvider();

        await using var _ = serviceProvider.ConfigureAwait(false);

        var cliBuilder = serviceProvider.GetRequiredService<CliBuilder>();

        var commandLineBuilder = cliBuilder.Create();
        commandLineBuilder.AddMiddleware(
            async (context, next) =>
            {
                if (!string.Equals(context.ParseResult.CommandResult.Command.Name, "list", StringComparison.Ordinal))
                {
                    levelSwitch.MinimumLevel = LogEventLevel.Verbose;
                }

                await next(context).ConfigureAwait(false);
            });

        var parser = commandLineBuilder.Build();

        return await parser.InvokeAsync(args).ConfigureAwait(false);
    }
}