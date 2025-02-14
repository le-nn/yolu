namespace Yolu;

/// <summary>
/// Represents a non-recoverable error in the program.
/// Used to indicate abnormal or unexpected conditions that should terminate the execution.
/// </summary>
public class Panic : Exception {
    /// <summary>
    /// Initializes a new instance of the <see cref="Panic"/> class.
    /// </summary>
    public Panic() : base() {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Panic"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public Panic(string message) : base(message) {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Panic"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public Panic(string message, Exception innerException) : base(message, innerException) {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Panic"/> class with serialized data.
    /// </summary>
    /// <param name="info">The SerializationInfo that holds the serialized object data about the exception being thrown.</param>
    /// <param name="context">The StreamingContext that contains contextual information about the source or destination.</param>
    protected Panic(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        : base(info, context) {
    }
}
