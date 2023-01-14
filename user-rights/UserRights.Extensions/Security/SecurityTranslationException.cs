namespace UserRights.Extensions.Security;

using System;
using System.Runtime.Serialization;

/// <summary>
/// Represents the exception thrown when an error occurs translating security contexts.
/// </summary>
[Serializable]
public class SecurityTranslationException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SecurityTranslationException" /> class.
    /// </summary>
    public SecurityTranslationException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SecurityTranslationException" /> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    public SecurityTranslationException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SecurityTranslationException" /> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public SecurityTranslationException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SecurityTranslationException" /> class.
    /// </summary>
    /// <param name="info">The <see cref="SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
    /// <param name="context">The <see cref="StreamingContext" /> that contains contextual information about the source or destination.</param>
    protected SecurityTranslationException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}