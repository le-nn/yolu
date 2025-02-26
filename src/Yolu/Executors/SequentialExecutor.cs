using System.Collections.Concurrent;
using System.Threading.Channels;

namespace Yolu.Executors;

/// <summary>
/// Provides a feature to wait for the end of asynchronous processing and connect to the next processing.
/// </summary>
public class SequentialExecutor {
    readonly Channel<Func<Task>> _taskQueue = Channel.CreateUnbounded<Func<Task>>();
    readonly Task _task;

    public SequentialExecutor() {
        _task = Run();
    }

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

    private async Task Run() {
        await foreach (var item in _taskQueue.Reader.ReadAllAsync()) {
            await item();
        }
    }

    public void Dispose() {
        // Signal completion and wait for the thread to finish
        _taskQueue.Writer.Complete();
    }

    public async ValueTask DisposeAsync() {
        _taskQueue.Writer.Complete();
        await _task;
    }
}