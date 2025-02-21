namespace Yolu.Threading.Tasks;

/// <summary>
/// Manages the execution of asynchronous tasks, ensuring that only one task runs at a time.
/// If a new task is started, the previous one is cancelled.
/// </summary>
public class SerialCancellationTaskManager : IDisposable {
    private CancellationTokenSource? _cancellationTokenSource;
    private readonly Lock _syncLock = new();

    /// <summary>
    /// Executes an asynchronous function, canceling any previously running task.
    /// </summary>
    /// <param name="func">A function that takes a <see cref="CancellationToken"/> and returns a <see cref="Task"/>.</param>
    /// <returns>A task that represents the execution of the provided function.</returns>
    public async Task ExecuteAsync(Func<CancellationToken, Task> func) {
        var token = CancelPreviousAndCreateNewToken();
        await func(token.Token).ConfigureAwait(false);
    }

    /// <summary>
    /// Executes an asynchronous function with a return value, canceling any previously running task.
    /// </summary>
    /// <typeparam name="T">The return type of the function.</typeparam>
    /// <param name="func">A function that takes a <see cref="CancellationToken"/> and returns a <see cref="Task{T}"/>.</param>
    /// <returns>A task that represents the execution of the provided function and returns a result of type <typeparamref name="T"/>.</returns>
    public async Task<T> ExecuteAsync<T>(Func<CancellationToken, Task<T>> func) {
        var token = CancelPreviousAndCreateNewToken();
        return await func(token.Token).ConfigureAwait(false);
    }

    /// <summary>
    /// Cancels the previously running task and creates a new <see cref="CancellationToken"/> for the next task.
    /// </summary>
    /// <returns>A new <see cref="CancellationTokenSource"/>.</returns>
    public CancellationTokenSource CancelPreviousAndCreateNewToken() {
        lock (_syncLock) {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = new CancellationTokenSource();
            return _cancellationTokenSource;
        }
    }

    /// <summary>
    /// Cancels the currently running task.
    /// </summary>
    public void Cancel() {
        lock (_syncLock) {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
        }
    }

    /// <summary>
    /// Disposes of the <see cref="SerialCancellationTaskManager"/> and cancels the currently running task.
    /// </summary>
    public void Dispose() {
        Cancel();
    }
}
