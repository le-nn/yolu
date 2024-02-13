namespace Hadron;

/// <summary>
/// Represents pipeline operator.
/// </summary>
/// <typeparam name="T">The value that you want to pipe.</typeparam>
/// <remarks>
/// Initializes a new instance of the <see cref="AsyncPipeline{T}" /> class.
/// </remarks>
/// <param name="func">The value selector.</param>
public readonly struct AsyncPipeline<T>(Func<Task<T>> func) {
    /// <summary>
    /// The value selector.
    /// </summary>
    private readonly Func<Task<T>>? _func = func;

    /// <summary>
    /// Connects and convolves a series of functions and return values.
    /// </summary>
    /// <typeparam name="U">Argument type</typeparam>
    /// <param name="func">To pipe function</param>
    /// <returns></returns>
    public AsyncPipeline<U> PipeAsync<U>(Func<T, Task<U>> func) {
        var self = this;
        return new AsyncPipeline<U>(async () => {
            if (self._func is null)
                throw Error.WithException(new InvalidOperationException("The pipeline is not initialized"));

            return await func(await self._func());
        });
    }

    /// <summary>
    /// Connects and convolves a series of functions and return values.
    /// </summary>
    /// <typeparam name="U">Argument type</typeparam>
    /// <param name="func">To pipe function</param>
    /// <returns></returns>
    public AsyncPipeline<U> PipeAsync<U>(Func<T, U> func) {
        var self = this;
        return new AsyncPipeline<U>(async () => {
            if (self._func is null)
                throw Error.WithException(new InvalidOperationException("The pipeline is not initialized"));

            return func(await self._func());
        });
    }

    /// <summary>
    /// Interrupt action.
    /// </summary>
    /// <typeparam name="T">Argument type</typeparam>
    /// <param name="func">To Interrupt async action</param>
    /// <returns></returns>
    public AsyncPipeline<T> ActionAsync(Func<T, Task> func) {
        var self = this;
        return new AsyncPipeline<T>(async () => {
            if (self._func is null)
                throw Error.WithException(new InvalidOperationException("The pipeline is not initialized"));

            var result = await self._func();
            await func(result);
            return result;
        });
    }

    /// <summary>
    /// Interrupt action.
    /// </summary>
    /// <typeparam name="T">Argument type</typeparam>
    /// <param name="func">To Interrupt async action</param>
    /// <returns></returns>
    public AsyncPipeline<T> ActionAsync(Action<T> func) {
        var self = this;
        return new AsyncPipeline<T>(async () => {
            if (self._func is null)
                throw Error.WithException(new InvalidOperationException("The pipeline is not initialized"));

            var result = await self._func();
            func(result);
            return result;
        });
    }

    /// <summary>
    /// Execute all piped actions async.
    /// </summary>
    /// <returns></returns>
    public async Task<T> ExecuteAsync() {
        var self = this;
        if (self._func is null)
            throw Error.WithException(new InvalidOperationException("The pipeline is not initialized"));

        return await self._func();
    }

    public Pipeline<Task<T>> AsPipeline() {
        var self = this;
        if (self._func is null)
            throw Error.WithException(new InvalidOperationException("The pipeline is not initialized"));

        return new Pipeline<Task<T>>(self._func);
    }
}