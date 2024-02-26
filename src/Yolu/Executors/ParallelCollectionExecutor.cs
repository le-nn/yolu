using System.Collections.Concurrent;

namespace Yolu.Executors;

/// <summary>
/// Executes a collection of tasks in parallel.
/// </summary>
/// <typeparam name="T">The type of tasks in the collection.</typeparam>
/// <remarks>
/// Initializes a new instance of the <see cref="ParallelCollectionExecutor{T}"/> class.
/// </remarks>
/// <param name="tasks">The collection of tasks to execute.</param>
/// <param name="parallelCount">The number of tasks to execute in parallel.</param>
public class ParallelCollectionExecutor<T>(IEnumerable<T> tasks, int parallelCount = 100) {
    private readonly ConcurrentQueue<T> _tasks = new ConcurrentQueue<T>(tasks);
    private readonly int _parallelCount = parallelCount;
    private int _progress = 0;
    private bool _isInvoked = false;

    /// <summary>
    /// Gets the progress of the execution.
    /// </summary>
    public int Progress => _progress;

    /// <summary>
    /// Gets the number of tasks in the collection.
    /// </summary>
    public int Count => _tasks.Count;

    /// <summary>
    /// Gets or sets the delay per startup for each task.
    /// </summary>
    public TimeSpan DelayPerStartup { get; set; } = TimeSpan.FromMilliseconds(100);

    /// <summary>
    /// Runs the tasks asynchronously using the specified handler.
    /// </summary>
    /// <param name="handler">The handler to execute for each task.</param>
    /// <param name="onError">The error handler to execute when an error occurs.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="Error">Thrown if the handler is already invoked.</exception>
    public async Task RunAsync(Func<T, Task> handler, Action<Exception>? onError = null) {
        if (_isInvoked) {
            throw Error.WithDisplayMessage("This handler is already invoked.");
        }

        _isInvoked = true;
        await Task.WhenAll(Run());

        IEnumerable<Task> Run() {
            for (var i = 0; i < _parallelCount; i++) {
                yield return Task.Run(async () => {
                    await Task.Delay(DelayPerStartup.Milliseconds * i);
                    while (_tasks.TryDequeue(out var t)) {
                        try {
                            await handler(t);
                            Interlocked.Increment(ref _progress);
                        }
                        catch (Exception ex) {
                            onError?.Invoke(ex);
                        }
                    }
                });
            }
        }
    }
}
