namespace UserRights.Cli;

using System;
using System.Runtime.Serialization;

/// <summary>
/// Represents the exception that is thrown when a syntax error occurs.
/// </summary>
[Serializable]
public class SyntaxException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SyntaxException" /> class.
    /// </summary>
    public SyntaxException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SyntaxException" /> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    public SyntaxException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SyntaxException" /> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public SyntaxException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SyntaxException" /> class.
    /// </summary>
    /// <param name="info">The <see cref="SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
    /// <param name="context">The <see cref="StreamingContext" /> that contains contextual information about the source or destination.</param>
    protected SyntaxException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}