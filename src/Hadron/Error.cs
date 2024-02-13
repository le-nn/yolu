using Hadron.Collections;
using Hadron.Identifier;

namespace Hadron;

public record ErrorId : PrefixedUlid {
    public const int TotalLength = 36;
    public const string Prefix = "error";
    public const string Separator = "_";

    public override string PrefixWithSeparator => "error_";

    public static ErrorId CreateNew() => NewPrefixedUlid<ErrorId>();

    public static ErrorId Parse(string id) {
        return Parse<ErrorId>(id);
    }
}

/// <summary>
///  Represents the general error.
/// </summary>
public class Error : Exception {
    public string? DisplayMessage { get; }

    public PrefixedUlid EventId { get; }

    public object? Payload { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Error"/> class.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="displayMessage"></param>
    /// <param name="payload"></param>
    /// <param name="eventId"></param>
    /// <param name="exception"></param>
    public Error(
        string message,
        string? displayMessage = null,
        object? payload = null,
        PrefixedUlid? eventId = null,
        Exception? exception = null
    ) : base(
        message,
        exception
    ) {
        Payload = payload;
        DisplayMessage = displayMessage;
        EventId = eventId ?? ErrorId.CreateNew();

        // Additional data
        Data.Add(nameof(EventId), EventId.ToString());
        Data.Add(nameof(DisplayMessage), displayMessage);
        Data.Add(nameof(Payload), payload);
    }

    public static Error WithDisplayMessage(
        string displayMessage,
        PrefixedUlid? eventId = null
    ) {
        return new Error<object>(
            null,
            displayMessage,
            displayMessage,
            eventId ?? ErrorId.CreateNew()
        );
    }

    public static Error<TPayload> WithDisplayMessage<TPayload>(
        TPayload exceptionType,
        string displayMessage,
        PrefixedUlid? eventId = null
    ) {
        return new(
            exceptionType,
            displayMessage,
            displayMessage,
            eventId ?? ErrorId.CreateNew()
        );
    }

    public static Error WithMessage(
        string message,
        string? displayMessage,
        Exception? ex = null,
        PrefixedUlid? eventId = null
    ) {
        return new Error<object>(
            null,
            message,
            displayMessage,
            eventId ?? ErrorId.CreateNew(),
            ex!
        );
    }

    public static Error<TPayload> WithMessage<TPayload>(
        TPayload exceptionType,
        string message,
        string? displayMessage,
        Exception? ex = null,
        PrefixedUlid? eventId = null
    ) {
        return new(
            exceptionType,
            message,
            displayMessage,
            eventId ?? ErrorId.CreateNew(),
            ex!
        );
    }

    public static Error WithException(
        Exception ex,
        string? message = null,
        string? displayMessage = null
    ) {
        return ex switch {
            Error ge => WithChild(
                ge,
                ge.Message,
                ge.DisplayMessage
            ),
            _ => WithMessage(
                message ?? ex.Message,
                displayMessage,
                ex
            )
        };
    }


    public static Error WithChild(
        Error ex,
        string message,
        string? displayMessage = null
    ) {
        return (message, displayMessage) is (null, null)
            ? ex
            : new Error<object>(
                null,
                message ?? ex.Message,
                displayMessage,
                ex.EventId,
                ex
            );
    }

    public static Error<TPayload> WithChild<TPayload>(
        TPayload payload,
        Error ex,
        string message,
        string? displayMessage = null
    ) {
        return new(
            payload,
            message,
            displayMessage,
            ex.EventId,
            ex
        );
    }

    public Array<string> FlattenDisplayMessages() {
        var messages = new List<string>();

        Exception? ex = this;
        while (ex is not null) {
            if (ex is Error { DisplayMessage: { } message and not "" }) {
                messages.Add(message);
            }

            ex = ex.InnerException;
        }

        return new(messages);
    }
}

/// <summary>
/// Represents the general error.
/// </summary>
/// <typeparam name="TError">Error info.</typeparam>
public class Error<TPayload>(
    TPayload? payload,
    string message,
    string? displayMessage = null,
    PrefixedUlid? eventId = null,
    Exception? error = null
    ) : Error(
    message,
    displayMessage,
    payload,
    eventId,
    error
    ) {
    public new TPayload? Payload => (TPayload?)base.Payload;
}
