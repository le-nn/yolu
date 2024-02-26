namespace Yolu;

/// <summary>
/// Represents pipeline operator with async operation.
/// </summary>
/// <typeparam name="T">The type of the value.</typeparam>
/// <remarks>
/// Initializes a new instance of the <see cref="Pipeline{T}" /> struct.
/// </remarks>
/// <param name="func">The value selector.</param>
public readonly struct Pipeline<T>(Func<T> func) {
    private readonly Func<T>? _func = func;

    /// <summary>
    /// Pipes the value through the specified function.
    /// </summary>
    /// <typeparam name="U">The type of the result.</typeparam>
    /// <param name="func">The function.</param>
    /// <returns>The pipeline instance.</returns>
    public Pipeline<U> Pipe<U>(Func<T, U> func) {
        var self = this;
        return new Pipeline<U>(() => {
            if (self._func is null)
                throw Error.WithException(new InvalidOperationException("The pipeline is not initialized"));

            return func(self._func());
        });
    }

    /// <summary>
    /// Executes the specified action on the value.
    /// </summary>
    /// <param name="func">The action.</param>
    /// <returns>The pipeline instance.</returns>
    public Pipeline<T> Action(Action<T> func) {
        var self = this;
        return new Pipeline<T>(() => {
            if (self._func is null)
                throw Error.WithException(new InvalidOperationException("The pipeline is not initialized"));

            var result = self._func();
            func(result);
            return result;
        });
    }

    /// <summary>
    /// Pipes the value through the specified async function.
    /// </summary>
    /// <typeparam name="U">The type of the result.</typeparam>
    /// <param name="func">The async function.</param>
    /// <returns>The async pipeline instance.</returns>
    public AsyncPipeline<U> PipeAsync<U>(Func<T, Task<U>> func) {
        var self = this;
        return new AsyncPipeline<U>(() => {
            if (self._func is null)
                throw Error.WithException(new InvalidOperationException("The pipeline is not initialized"));

            return func(self._func());
        });
    }

    /// <summary>
    /// Executes the pipeline and returns the result.
    /// </summary>
    /// <returns>The result of the pipeline.</returns>
    public T Execute() {
        if (_func is null)
            throw Error.WithException(new InvalidOperationException("The pipeline is not initialized"));

        return _func();
    }
}
