namespace Hadron;

public readonly struct Observer<T>(
    Action<T> onNext,
    Action<Exception>? onError = null,
    Action? onCompleted = null
) : IObserver<T> {
    private readonly Action? _onCompleted = onCompleted;
    private readonly Action<Exception>? _onError = onError;
    private readonly Action<T> _onNext = onNext;

    /// <summary>
    /// Notifies the observer that the provider has finished sending push-based notifications.
    /// </summary>
    public void OnCompleted() {
        _onCompleted?.Invoke();
    }

    /// <summary>
    /// Notifies the observer that the provider has experienced an error condition.
    /// </summary>
    /// <param name="error">The error that occurred.</param>
    public void OnError(Exception error) {
        _onError?.Invoke(error);
    }

    /// <summary>
    /// Provides the observer with new data.
    /// </summary>
    /// <param name="value">The current notification information.</param>
    public void OnNext(T value) {
        _onNext?.Invoke(value);
    }
}
