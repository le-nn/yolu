using System.Collections.Concurrent;

namespace Hadron.Executors;

/// <summary>
/// Provides a feature to wait for the end of asynchronous processing and connect to the next processing.
/// </summary>
public class ConcatAsyncOperationExecutor {
    private readonly ConcurrentQueue<IOperation> _operations = [];
    private volatile int _processingCount = 0;

    /// <summary>
    /// Waits for the end of asynchronous processing and cancats to the next processing.
    /// </summary>
    /// <param name="operation">The async operation.</param>
    /// <returns>The async oparation contains a result of processing. </returns>
    public Task<T> ExecuteAsync<T>(Func<Task<T>> operation) {
        var source = new TaskCompletionSource<T>();
        _operations.Enqueue(new Operation<T>(operation, source));

        Hadle();

        return source.Task;
    }

    /// <summary>
    /// Waits for the end of asynchronous processing and concat to the next processing.
    /// </summary>
    /// <param name="operation">The async operation.</param>
    /// <returns>The async operation contains a result of processing. </returns>
    public Task ExecuteAsync(Func<Task> operation) {
        var source = new TaskCompletionSource<byte>();
        _operations.Enqueue(new Operation<byte>(async () => {
            await operation.Invoke();
            return 0;
        }, source));

        Hadle();

        return source.Task;
    }

    private async void Hadle() {
        if (_processingCount is not 0) {
            return;
        }

        Interlocked.Increment(ref _processingCount);

        try {
            while (_operations.TryDequeue(out var operation)) {
                await operation.HandleAsync();
            }
        }
        finally {
            Interlocked.Decrement(ref _processingCount);
        }

    }

    private interface IOperation {
        Task HandleAsync();
    }

    private readonly struct Operation<T>(Func<Task<T>> func, TaskCompletionSource<T> taskSource) : IOperation {
        private readonly Func<Task<T>> _func = func;
        private readonly TaskCompletionSource<T> _taskSource = taskSource;

        public async Task HandleAsync() {
            try {
                var result = await _func.Invoke();
                _taskSource.SetResult(result);
            }
            catch (Exception ex) {
                _taskSource.SetException(ex);
            }
        }
    }
}