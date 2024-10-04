using System.Collections.Concurrent;

namespace Yolu.Executors;

/// <summary>
/// Provides a feature to wait for the end of asynchronous processing and connect to the next processing.
/// </summary>
public class SequentialExecutor {
    private readonly Thread _thread;
    private readonly BlockingCollection<Func<Task>> _taskQueue = [];

    public SequentialExecutor() {
        // Initialize and start the dedicated thread
        _thread = new(Run) {
            IsBackground = true,
        };
        _thread.Start();
    }

    public Task ExecuteAsync(Func<Task> func) {
        var tcs = new TaskCompletionSource();

        // Enqueue the task to be executed
        _taskQueue.Add(async () => {
            try {
                await func();
                tcs.SetResult();
            }
            catch (Exception ex) {
                tcs.SetException(ex);
            }
        });

        return tcs.Task;
    }

    public Task<T> ExecuteAsync<T>(Func<Task<T>> func) {
        var tcs = new TaskCompletionSource<T>();

        // Enqueue the task with a return value
        _taskQueue.Add(async () => {
            try {
                var result = await func();
                tcs.SetResult(result);
            }
            catch (Exception ex) {
                tcs.SetException(ex);
            }
        });

        return tcs.Task;
    }

    private void Run() {
        // Process tasks sequentially on the dedicated thread
        foreach (var taskFunc in _taskQueue.GetConsumingEnumerable()) {
            // Execute the task synchronously
            taskFunc().Wait();
        }
    }

    public void Dispose() {
        // Signal completion and wait for the thread to finish
        _taskQueue.CompleteAdding();
        _thread.Join();
        _taskQueue.Dispose();
    }
}