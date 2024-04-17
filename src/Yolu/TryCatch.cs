using System.Runtime.CompilerServices;

namespace Yolu;


public static class TryCatchUtils {
    /// <summary>
    /// Executes a function and returns its result, or the default value for the type if an exception is thrown.
    /// </summary>
    /// <typeparam name="T">The type of the return value.</typeparam>
    /// <param name="func">The function to execute.</param>
    /// <returns>The result of the function, or the default value for the type if an exception is thrown.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T? Try<T>(Func<T> func) {
        try {
            return func();
        }
        catch {
            return default!;
        }
    }

    /// <summary>
    /// Executes a function and returns its result, or a specified value if an exception is thrown.
    /// </summary>
    /// <typeparam name="T">The type of the return value.</typeparam>
    /// <param name="func">The function to execute.</param>
    /// <param name="whenCatch">The value to return if an exception is thrown.</param>
    /// <returns>The result of the function, or the specified value if an exception is thrown.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Try<T>(Func<T> func, T whenCatch) {
        try {
            return func();
        }
        catch {
            return whenCatch;
        }
    }

    /// <summary>
    /// Executes a function and returns its result, or the result of a second function if a specific exception is thrown.
    /// </summary>
    /// <typeparam name="T">The type of the return value.</typeparam>
    /// <typeparam name="TException">The type of the exception to catch.</typeparam>
    /// <param name="func">The function to execute.</param>
    /// <param name="whenCatch">The function to execute if the specified exception is thrown.</param>
    /// <returns>The result of the function, or the result of the second function if the specified exception is thrown.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Try<T, TException>(Func<T> func, Func<TException, T> whenCatch) where TException : Exception {
        try {
            return func();
        }
        catch (TException ex) {
            return whenCatch(ex);
        }
    }

    /// <summary>
    /// Executes an asynchronous function and returns its result, or the default value for the type if an exception is thrown.
    /// </summary>
    /// <typeparam name="T">The type of the return value.</typeparam>
    /// <param name="func">The asynchronous function to execute.</param>
    /// <returns>A task that represents the asynchronous operation. The task result is the result of the function, or the default value for the type if an exception is thrown.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<T?> Try<T>(Func<Task<T>> func) {
        try {
            return await func()!;
        }
        catch {
            return default;
        }
    }

    /// <summary>
    /// Executes an asynchronous function and returns its result, or a specified value if an exception is thrown.
    /// </summary>
    /// <typeparam name="T">The type of the return value.</typeparam>
    /// <param name="func">The asynchronous function to execute.</param>
    /// <param name="whenCatch">The value to return if an exception is thrown.</param>
    /// <returns>A task that represents the asynchronous operation. The task result is the result of the function, or the specified value if an exception is thrown.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<T> Try<T>(Func<Task<T>> func, T whenCatch) {
        try {
            return await func();
        }
        catch {
            return whenCatch;
        }
    }

    /// <summary>
    /// Executes an asynchronous function and returns its result, or the result of a second function if a specific exception is thrown.
    /// </summary>
    /// <typeparam name="T">The type of the return value.</typeparam>
    /// <typeparam name="TException">The type of the exception to catch.</typeparam>
    /// <param name="func">The asynchronous function to execute.</param>
    /// <param name="whenCatch">The function to execute if the specified exception is thrown.</param>
    /// <returns>A task that represents the asynchronous operation. The task result is the result of the function, or the result of the second function if the specified exception is thrown.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<T> Try<T, TException>(Func<Task<T>> func, Func<TException, T> whenCatch) where TException : Exception {
        try {
            return await func();
        }
        catch (TException ex) {
            return whenCatch(ex);
        }
    }
}
