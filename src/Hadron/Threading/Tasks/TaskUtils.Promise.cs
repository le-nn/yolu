namespace Hadron.Tasks;

public static partial class TaskUtils {
    /// <summary>
    /// Creates a task with the specified initialization action and cancellation token.
    /// </summary>
    /// <param name="initialize">The initialization action that sets the result or exception of the task.</param>
    /// <param name="token">The cancellation token used to cancel the task.</param>
    /// <returns>The created task.</returns>
    public static Task CreateTask(Action<Action, Action<Exception>> initialize, CancellationToken token = default) {
        var source = new TaskCompletionSource();
        if (token != default) token.Register(() => source.TrySetCanceled(token));
        initialize(source.SetResult, source.SetException);
        return source.Task;
    }

    /// <summary>
    /// Creates a task with the specified initialization action, cancellation token, and result type.
    /// </summary>
    /// <typeparam name="T">The type of the task result.</typeparam>
    /// <param name="initialize">The initialization action that sets the result or exception of the task.</param>
    /// <param name="token">The cancellation token used to cancel the task.</param>
    /// <returns>The created task.</returns>
    public static Task<T> CreateTask<T>(Action<Action<T>, Action<Exception>> initialize, CancellationToken token = default) {
        var source = new TaskCompletionSource<T>();
        if (token != default) token.Register(() => source.TrySetCanceled(token));
        initialize(source.SetResult, source.SetException);

        return source.Task;
    }
}
