namespace UserRights.Extensions.Serialization;

using System;
using System.Runtime.Serialization;

/// <summary>
/// Represents the exception thrown when an error occurs serializing data.
/// </summary>
[Serializable]
public class SerializationException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SerializationException" /> class.
    /// </summary>
    public SerializationException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SerializationException" /> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    public SerializationException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SerializationException" /> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public SerializationException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SerializationException" /> class.
    /// </summary>
    /// <param name="info">The <see cref="SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
    /// <param name="context">The <see cref="StreamingContext" /> that contains contextual information about the source or destination.</param>
    protected SerializationException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}