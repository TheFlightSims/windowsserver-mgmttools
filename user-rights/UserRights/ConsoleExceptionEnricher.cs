namespace UserRights;

using System;
using System.Globalization;
using System.Text;
using Serilog.Core;
using Serilog.Events;

/// <summary>
/// Represents a exception formatter for console log events.
/// </summary>
public class ConsoleExceptionEnricher : ILogEventEnricher
{
    /// <inheritdoc/>
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        ArgumentNullException.ThrowIfNull(logEvent);
        ArgumentNullException.ThrowIfNull(propertyFactory);

        if (logEvent.Exception is null)
        {
            return;
        }

        var stringBuilder = new StringBuilder(logEvent.Exception.Message);

        var innerException = logEvent.Exception.InnerException;
        while (innerException is not null)
        {
            stringBuilder.AppendFormat(CultureInfo.InvariantCulture, " ({0})", innerException.Message);
            innerException = innerException.InnerException;
        }

        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("ConsoleException", stringBuilder.ToString()));
    }
}