namespace UserRights.Cli;

using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Help;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using UserRights.Application;
using UserRights.Extensions.Serialization;

/// <summary>
/// Represents the command line builder creator.
/// </summary>
public class CliBuilder
{
    private readonly string program = AppDomain.CurrentDomain.FriendlyName;
    private readonly Version version = Assembly.GetExecutingAssembly().GetName().Version!;

    private readonly ILogger logger;
    private readonly ILsaUserRights policy;
    private readonly IUserRightsManager manager;

    /// <summary>
    /// Initializes a new instance of the <see cref="CliBuilder"/> class.
    /// </summary>
    /// <param name="logger">The logging instance.</param>
    /// <param name="policy">The local security authority policy instance.</param>
    /// <param name="manager">The user rights application instance.</param>
    public CliBuilder(ILogger<CliBuilder> logger, ILsaUserRights policy, IUserRightsManager manager)
    {
        this.logger = logger;
        this.policy = policy;
        this.manager = manager;
    }

    /// <summary>
    /// Creates the command line builder.
    /// </summary>
    /// <returns>A configured command line builder.</returns>
    public CommandLineBuilder Create()
    {
        var rootCommand = new RootCommand("Windows User Rights Assignment Utility")
        {
            this.CreateListCommand(),
            this.CreatePrincipalCommand(),
            this.CreatePrivilegeCommand()
        };

        var builder = new CommandLineBuilder(rootCommand)
            .UseVersionOption()
            .UseHelp(context =>
            {
                string[] examples;
                switch (context.Command.Name)
                {
                    case "list":

                        examples = new[]
                        {
                            "list",
                            "list --json",
                            "list --path x:\\path\\file.csv"
                        };

                        break;

                    case "principal":

                        examples = new[]
                        {
                            "principal DOMAIN\\UserOrGroup --grant SeDenyServiceLogonRight",
                            "principal DOMAIN\\UserOrGroup --revoke SeDenyServiceLogonRight",
                            "principal DOMAIN\\UserOrGroup --grant SeServiceLogonRight --revoke SeDenyServiceLogonRight",
                            "principal DOMAIN\\UserOrGroup --grant SeServiceLogonRight --grant SeInteractiveLogonRight --revoke-others"
                        };

                        break;

                    case "privilege":

                        examples = new[]
                        {
                            "privilege SeServiceLogonRight --grant DOMAIN\\UserOrGroup --revoke DOMAIN\\Group",
                            "privilege SeServiceLogonRight --revoke DOMAIN\\UserOrGroup",
                            "privilege SeServiceLogonRight --grant DOMAIN\\UserOrGroup --revoke-pattern \"^S-1-5-21-\"",
                            "privilege SeServiceLogonRight --revoke-pattern \"^S-1-5-21-\"",
                            "privilege SeServiceLogonRight --revoke-all"
                        };

                        break;

                    default:

                        return;
                }

                var stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("Examples:");

                for (var i = 0; i < examples.Length; i++)
                {
                    var example = examples[i];
                    stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "  {0} ", this.program);
                    stringBuilder.Append(example);

                    if (i < examples.Length - 1)
                    {
                        stringBuilder.AppendLine();
                    }
                }

                var sections = HelpBuilder.Default.GetLayout().ToList();
                sections.Insert(2, element => element.Output.WriteLine(stringBuilder.ToString()));
                context.HelpBuilder.CustomizeLayout(_ => sections);
            })
            .UseParseDirective()
            .CancelOnProcessTermination();

        builder.AddMiddleware(async (context, next) =>
        {
            if (context.ParseResult.Errors.Count > 0)
            {
                var stringBuilder = new StringBuilder();

                for (var i = 0; i < context.ParseResult.Errors.Count; i++)
                {
                    var error = context.ParseResult.Errors[i];
                    stringBuilder.Append(error.Message);

                    if (i < context.ParseResult.Errors.Count - 1)
                    {
                        stringBuilder.AppendLine();
                    }
                }

                throw new SyntaxException(stringBuilder.ToString());
            }

            await next(context).ConfigureAwait(false);
        });

        return builder;
    }

    /// <summary>
    /// Creates the list command.
    /// </summary>
    /// <returns>The list command instance.</returns>
    private Command CreateListCommand()
    {
        var jsonOption = new Option<bool>(new[] { "--json", "-j" }, "Formats output in JSON instead of CSV.");
        var pathOption = new Option<string>(new[] { "--path", "-f" }, "Writes output to the specified path instead of STDOUT.");
        var systemNameOption = new Option<string>(new[] { "--system-name", "-s" }, "The name of the remote system to execute on (default localhost).");

        // Ensure the path is a valid string.
        pathOption.AddValidator(result =>
        {
            if (string.IsNullOrWhiteSpace(result.Tokens.Single().Value))
            {
                result.ErrorMessage = "Path cannot be empty or whitespace.";
            }
        });

        // Ensure the system name is a valid string.
        systemNameOption.AddValidator(result =>
        {
            if (string.IsNullOrWhiteSpace(result.Tokens.Single().Value))
            {
                result.ErrorMessage = "System name cannot be empty or whitespace.";
            }
        });

        var command = new Command("list", "Runs the utility in list mode.");

        command.AddOption(jsonOption);
        command.AddOption(pathOption);
        command.AddOption(systemNameOption);

        command.SetHandler(
            (json, path, systemName) =>
            {
                this.logger.LogInformation(OperationId.ListMode, "{Program:l} v{Version} executing in {Mode:l} mode.", this.program, this.version, command.Name);

                this.policy.Connect(systemName);

                var results = this.manager.GetUserRights(this.policy);

                var serialized = json
                    ? results.ToJson()
                    : results.ToCsv();

                if (string.IsNullOrWhiteSpace(path))
                {
                    using var streamWriter = new StreamWriter(Console.OpenStandardOutput());
                    streamWriter.Write(serialized);
                }
                else
                {
                    using var streamWriter = new StreamWriter(path, false, Encoding.UTF8);
                    streamWriter.Write(serialized);
                }
            },
            jsonOption,
            pathOption,
            systemNameOption);

        return command;
    }

    /// <summary>
    /// Creates the principal command.
    /// </summary>
    /// <returns>The principal command instance.</returns>
    private Command CreatePrincipalCommand()
    {
        var principalArgument = new Argument<string>("principal", "The principal to modify.");
        var grantsOption = new Option<string[]>(new[] { "--grant", "-g" }, "The privilege to grant to the principal.");
        var revocationsOption = new Option<string[]>(new[] { "--revoke", "-r" }, "The privilege to revoke from the principal.");
        var revokeAllOption = new Option<bool>(new[] { "--revoke-all", "-a" }, "Revokes all privileges from the principal.");
        var revokeOthersOption = new Option<bool>(new[] { "--revoke-others", "-o" }, "Revokes all privileges from the principal excluding those being granted.");
        var dryRunOption = new Option<bool>(new[] { "--dry-run", "-d" }, "Enables dry-run mode.");
        var systemNameOption = new Option<string>(new[] { "--system-name", "-s" }, "The name of the remote system to execute on (default localhost).");

        // Ensure the principal is a valid string.
        principalArgument.AddValidator(result =>
        {
            if (string.IsNullOrWhiteSpace(result.Tokens.Single().Value))
            {
                result.ErrorMessage = "Principal cannot be empty or whitespace.";
            }
        });

        // Ensure principal mode is used with at least one of grant, revoke, or revoke all.
        principalArgument.AddValidator(result =>
        {
            if (result.GetValueForOption(grantsOption) is not { Length: > 0 }
                && result.GetValueForOption(revocationsOption) is not { Length: > 0 }
                && !result.GetValueForOption(revokeAllOption))
            {
                result.ErrorMessage = "At least one option is required.";
            }
        });

        // Ensure the grants are valid strings.
        grantsOption.AddValidator(result =>
        {
            if (result.Tokens.Any(p => string.IsNullOrWhiteSpace(p.Value)))
            {
                result.ErrorMessage = "Grants cannot be empty or whitespace.";
            }
        });

        // Ensure the grants do not overlap with revocations or contain duplicates.
        grantsOption.AddValidator(result =>
        {
            var grantsCollection = result.GetValueForOption(grantsOption) ?? Array.Empty<string>();
            var revocationsCollection = result.GetValueForOption(revocationsOption) ?? Array.Empty<string>();

            var grantsSet = grantsCollection.ToHashSet(StringComparer.InvariantCultureIgnoreCase);
            var revocationsSet = revocationsCollection.ToHashSet(StringComparer.InvariantCultureIgnoreCase);

            if (grantsSet.Overlaps(revocationsSet))
            {
                result.ErrorMessage = "The grants and revocations cannot overlap.";
            }
            else if (grantsSet.Count != grantsCollection.Length)
            {
                result.ErrorMessage = "The grants cannot contain duplicates.";
            }
        });

        // Ensure the revocations are valid strings.
        revocationsOption.AddValidator(result =>
        {
            if (result.Tokens.Any(p => string.IsNullOrWhiteSpace(p.Value)))
            {
                result.ErrorMessage = "Revocations cannot be empty or whitespace.";
            }
        });

        // Ensure the revocations do not overlap with revocations or contain duplicates.
        revocationsOption.AddValidator(result =>
        {
            var grantsCollection = result.GetValueForOption(grantsOption) ?? Array.Empty<string>();
            var revocationsCollection = result.GetValueForOption(revocationsOption) ?? Array.Empty<string>();

            var grantsSet = grantsCollection.ToHashSet(StringComparer.InvariantCultureIgnoreCase);
            var revocationsSet = revocationsCollection.ToHashSet(StringComparer.InvariantCultureIgnoreCase);

            if (grantsSet.Overlaps(revocationsSet))
            {
                result.ErrorMessage = "The grants and revocations cannot overlap.";
            }
            else if (revocationsSet.Count != revocationsCollection.Length)
            {
                result.ErrorMessage = "The revocations cannot contain duplicates.";
            }
        });

        // Ensure revoke all is not used with any other option.
        revokeAllOption.AddValidator(result =>
        {
            if (result.GetValueForOption(revokeOthersOption)
                || result.GetValueForOption(grantsOption) is { Length: > 0 }
                || result.GetValueForOption(revocationsOption) is { Length: > 0 })
            {
                result.ErrorMessage = "Revoke all cannot be used with any other option.";
            }
        });

        // Ensure revoke others is only used with grant.
        revokeOthersOption.AddValidator(result =>
        {
            if (result.GetValueForOption(revokeAllOption)
                || result.GetValueForOption(grantsOption) is not { Length: > 0 }
                || result.GetValueForOption(revocationsOption) is { Length: > 0 })
            {
                result.ErrorMessage = "Revoke others is only valid with grants.";
            }
        });

        // Ensure the system name is a valid string.
        systemNameOption.AddValidator(result =>
        {
            if (string.IsNullOrWhiteSpace(result.Tokens.Single().Value))
            {
                result.ErrorMessage = "System name cannot be empty or whitespace.";
            }
        });

        // Create command.
        var command = new Command("principal", "Runs the utility in principal mode.");

        command.AddArgument(principalArgument);
        command.AddOption(grantsOption);
        command.AddOption(revocationsOption);
        command.AddOption(revokeAllOption);
        command.AddOption(revokeOthersOption);
        command.AddOption(dryRunOption);
        command.AddOption(systemNameOption);

        command.SetHandler(
            (principal, grants, revocations, revokeAll, revokeOthers, dryRun, systemName) =>
            {
                this.logger.BeginScope(new Dictionary<string, object>(StringComparer.Ordinal) { { "DryRun", dryRun } });

                this.logger.LogInformation(OperationId.PrincipalMode, "{Program:l} v{Version} executing in {Mode:l} mode.", this.program, this.version, command.Name);

                this.policy.Connect(systemName);

                this.manager.ModifyPrincipal(
                    this.policy,
                    principal,
                    grants,
                    revocations,
                    revokeAll,
                    revokeOthers,
                    dryRun);
            },
            principalArgument,
            grantsOption,
            revocationsOption,
            revokeAllOption,
            revokeOthersOption,
            dryRunOption,
            systemNameOption);

        return command;
    }

    /// <summary>
    /// Creates the privilege command.
    /// </summary>
    /// <returns>The privilege command instance.</returns>
    private Command CreatePrivilegeCommand()
    {
        var privilegeArgument = new Argument<string>("privilege", "The privilege to modify.");
        var grantsOption = new Option<string[]>(new[] { "--grant", "-g" }, "The principal to grant the privilege to.");
        var revocationsOption = new Option<string[]>(new[] { "--revoke", "-r" }, "The principal to revoke the privilege from.");
        var revokeAllOption = new Option<bool>(new[] { "--revoke-all", "-a" }, "Revokes all principals from the privilege.");
        var revokeOthersOption = new Option<bool>(new[] { "--revoke-others", "-o" }, "Revokes all principals from the privilege excluding those being granted.");
        var revokePatternOption = new Option<string>(new[] { "--revoke-pattern", "-t" }, description: "Revokes all principals whose SID matches the regular expression excluding those being granted.");
        var dryRunOption = new Option<bool>(new[] { "--dry-run", "-d" }, "Enables dry-run mode.");
        var systemNameOption = new Option<string>(new[] { "--system-name", "-s" }, "The name of the remote system to execute on (default localhost).");

        // Ensure the principal is a valid string.
        privilegeArgument.AddValidator(result =>
        {
            if (string.IsNullOrWhiteSpace(result.Tokens.Single().Value))
            {
                result.ErrorMessage = "Privilege cannot be empty or whitespace.";
            }
        });

        // Ensure privilege mode is used with at least one of grant, revoke, revoke all, or revoke pattern.
        privilegeArgument.AddValidator(result =>
        {
            if (result.GetValueForOption(grantsOption) is not { Length: > 0 }
                && result.GetValueForOption(revocationsOption) is not { Length: > 0 }
                && !result.GetValueForOption(revokeAllOption)
                && result.GetValueForOption(revokePatternOption) is null)
            {
                result.ErrorMessage = "At least one option is required.";
            }
        });

        // Ensure the grants are valid strings.
        grantsOption.AddValidator(result =>
        {
            if (result.Tokens.Any(p => string.IsNullOrWhiteSpace(p.Value)))
            {
                result.ErrorMessage = "Grants cannot be empty or whitespace.";
            }
        });

        // Ensure the grants do not overlap with revocations or contain duplicates.
        grantsOption.AddValidator(result =>
        {
            var grantsCollection = result.GetValueForOption(grantsOption) ?? Array.Empty<string>();
            var revocationsCollection = result.GetValueForOption(revocationsOption) ?? Array.Empty<string>();

            var grantsSet = grantsCollection.ToHashSet(StringComparer.InvariantCultureIgnoreCase);
            var revocationsSet = revocationsCollection.ToHashSet(StringComparer.InvariantCultureIgnoreCase);

            if (grantsSet.Overlaps(revocationsSet))
            {
                result.ErrorMessage = "Grants and revocations cannot overlap.";
            }
            else if (grantsSet.Count != grantsCollection.Length)
            {
                result.ErrorMessage = "Grants cannot contain duplicates.";
            }
        });

        // Ensure the revocations are valid strings.
        revocationsOption.AddValidator(result =>
        {
            if (result.Tokens.Any(p => string.IsNullOrWhiteSpace(p.Value)))
            {
                result.ErrorMessage = "Revocations cannot be empty or whitespace.";
            }
        });

        // Ensure the revocations do not overlap with revocations or contain duplicates.
        revocationsOption.AddValidator(result =>
        {
            var grantsCollection = result.GetValueForOption(grantsOption) ?? Array.Empty<string>();
            var revocationsCollection = result.GetValueForOption(revocationsOption) ?? Array.Empty<string>();

            var grantsSet = grantsCollection.ToHashSet(StringComparer.InvariantCultureIgnoreCase);
            var revocationsSet = revocationsCollection.ToHashSet(StringComparer.InvariantCultureIgnoreCase);

            if (grantsSet.Overlaps(revocationsSet))
            {
                result.ErrorMessage = "Grants and revocations cannot overlap.";
            }
            else if (revocationsSet.Count != revocationsCollection.Length)
            {
                result.ErrorMessage = "Revocations cannot contain duplicates.";
            }
        });

        // Ensure revoke all is not used with any other option.
        revokeAllOption.AddValidator(result =>
        {
            if (result.GetValueForOption(grantsOption) is { Length: > 0 }
                || result.GetValueForOption(revocationsOption) is { Length: > 0 }
                || result.GetValueForOption(revokeOthersOption)
                || result.GetValueForOption(revokePatternOption) is not null)
            {
                result.ErrorMessage = "Revoke all cannot be used with any other option.";
            }
        });

        // Ensure revoke others is only used with grant.
        revokeOthersOption.AddValidator(result =>
        {
            if (result.GetValueForOption(grantsOption) is not { Length: > 0 }
                || result.GetValueForOption(revocationsOption) is { Length: > 0 }
                || result.GetValueForOption(revokeAllOption)
                || result.GetValueForOption(revokePatternOption) is not null)
            {
                result.ErrorMessage = "Revoke others is only valid when used with grants.";
            }
        });

        // Ensure the revoke pattern is a valid string.
        revokePatternOption.AddValidator(result =>
        {
            if (string.IsNullOrWhiteSpace(result.Tokens.Single().Value))
            {
                result.ErrorMessage = "Revoke pattern cannot be empty or whitespace.";
            }
            else
            {
                try
                {
                    _ = new Regex(result.Tokens.Single().Value, RegexOptions.None, TimeSpan.FromSeconds(1));
                }
                catch (RegexParseException e)
                {
                    result.ErrorMessage = string.Format(CultureInfo.InvariantCulture, "Revoke pattern must be a valid regular expression. {0}", e.Message);
                }
            }
        });

        // Ensure revoke pattern is not used with revoke, revoke all, or revoke others.
        revokePatternOption.AddValidator(result =>
        {
            if (result.GetValueForOption(revocationsOption) is { Length: > 0 }
                || result.GetValueForOption(revokeAllOption)
                || result.GetValueForOption(revokeOthersOption))
            {
                result.ErrorMessage = "Revoke pattern is only valid when used alone or with grants.";
            }
        });

        // Ensure the system name is a valid string.
        systemNameOption.AddValidator(result =>
        {
            if (string.IsNullOrWhiteSpace(result.Tokens.Single().Value))
            {
                result.ErrorMessage = "System name cannot be empty or whitespace.";
            }
        });

        var command = new Command("privilege", "Runs the utility in privilege mode.");

        command.AddArgument(privilegeArgument);
        command.AddOption(grantsOption);
        command.AddOption(revocationsOption);
        command.AddOption(revokeAllOption);
        command.AddOption(revokeOthersOption);
        command.AddOption(revokePatternOption);
        command.AddOption(dryRunOption);
        command.AddOption(systemNameOption);

        command.SetHandler(
            (privilege, grants, revocations, revokeAll, revokeOthers, revokePattern, dryRun, systemName) =>
            {
                this.logger.BeginScope(new Dictionary<string, object>(StringComparer.Ordinal) { { "DryRun", dryRun } });

                this.logger.LogInformation(OperationId.PrivilegeMode, "{Program:l} v{Version} executing in {Mode:l} mode.", this.program, this.version, command.Name);

                var revokeRegex = string.IsNullOrWhiteSpace(revokePattern)
                    ? null
                    : new Regex(revokePattern, RegexOptions.None, TimeSpan.FromSeconds(1));

                this.policy.Connect(systemName);

                this.manager.ModifyPrivilege(
                    this.policy,
                    privilege,
                    grants,
                    revocations,
                    revokeAll,
                    revokeOthers,
                    revokeRegex,
                    dryRun);
            },
            privilegeArgument,
            grantsOption,
            revocationsOption,
            revokeAllOption,
            revokeOthersOption,
            revokePatternOption,
            dryRunOption,
            systemNameOption);

        return command;
    }
}