using System.Collections.Concurrent;
using System.Threading.Channels;

namespace Yolu.Executors;

/// <summary>
/// Provides a feature to wait for the end of asynchronous processing and connect to the next processing.
/// </summary>
public class SequentialExecutor : IDisposable, IAsyncDisposable {
    readonly Channel<Func<Task>> _taskQueue = Channel.CreateUnbounded<Func<Task>>();
    readonly Task _task;
    int _isDisposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="SequentialExecutor"/> class.
    /// </summary>
    public SequentialExecutor() {
        _task = Run();
    }

    /// <summary>
    /// Finalizes an instance of the <see cref="SequentialExecutor"/> class.
    /// </summary>
    ~SequentialExecutor() {
        Dispose(false);
    }

    /// <summary>
    /// Executes the specified asynchronous function sequentially.
    /// </summary>
    /// <param name="func">The asynchronous function to execute.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async ValueTask ExecuteAsync(Func<Task> func, CancellationToken cancellationToken = default) {
        var tcs = new TaskCompletionSource<object?>();

        await _taskQueue.Writer.WriteAsync(async () => {
            try {
                await func();
                tcs.SetResult(null);
            }
            catch (Exception ex) {
                tcs.SetException(ex);
            }
        }, cancellationToken);

        await tcs.Task;
    }

    /// <summary>
    /// Executes the specified asynchronous function with a return value sequentially.
    /// </summary>
    /// <typeparam name="T">The type of the return value.</typeparam>
    /// <param name="func">The asynchronous function to execute.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation and contains the result.</returns>
    public async Task<T> ExecuteAsync<T>(Func<Task<T>> func, CancellationToken cancellationToken = default) {
        var tcs = new TaskCompletionSource<T>();

        // Enqueue the task with a return value
        await _taskQueue.Writer.WriteAsync(async () => {
            try {
                var result = await func();
                tcs.SetResult(result);
            }
            catch (Exception ex) {
                tcs.SetException(ex);
            }
        }, cancellationToken);

        return await tcs.Task;
    }

    /// <summary>
    /// Runs the task queue and processes each task sequentially.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task Run() {
        await foreach (var item in _taskQueue.Reader.ReadAllAsync()) {
            await item();
        }
    }

    /// <summary>
    /// Releases the unmanaged resources used by the <see cref="SequentialExecutor"/> and optionally releases the managed resources.
    /// </summary>
    /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing) {
        if (Interlocked.Exchange(ref _isDisposed, 1) == 0) {
            if (disposing) {
                // Dispose managed resources
                _taskQueue.Writer.Complete();
            }
            // Dispose unmanaged resources
        }
    }

    /// <summary>
    /// Releases all resources used by the <see cref="SequentialExecutor"/>.
    /// </summary>
    public void Dispose() {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Asynchronously releases all resources used by the <see cref="SequentialExecutor"/>.
    /// </summary>
    /// <returns>A task that represents the asynchronous dispose operation.</returns>
    public async ValueTask DisposeAsync() {
        Dispose(true);
        await _task;
        GC.SuppressFinalize(this);
    }
}