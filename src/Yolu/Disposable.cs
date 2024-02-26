using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Yolu;

public class Disposable : IDisposable {
    private const int _notDisposedState = 0;
    private const int _disposingState = 1;
    private const int _disposedState = 2;
    private volatile int _state;

    /// <summary>
    /// Indicates that this object is disposed.
    /// </summary>
    protected bool IsDisposed => _state is _disposedState;

    /// <summary>
    /// Indicates that <see cref="DisposeAsync()"/> is called but not yet completed.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    protected bool IsDisposing => _state is _disposingState;

    /// <summary>
    /// Indicates that <see cref="DisposeAsync()"/> is called.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    protected bool IsDisposingOrDisposed => _state is not _notDisposedState;

    private string ObjectName => GetType().Name;

    /// <summary>
    /// Gets a task representing <see cref="ObjectDisposedException"/> exception.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    protected Task DisposedTask => Task.FromException(new ObjectDisposedException(ObjectName));

    /// <summary>
    /// Returns a task representing <see cref="ObjectDisposedException"/> exception.
    /// </summary>
    /// <typeparam name="T">The type of the task.</typeparam>
    /// <returns>The task representing <see cref="ObjectDisposedException"/> exception.</returns>
    protected Task<T> GetDisposedTask<T>()
        => Task.FromException<T>(new ObjectDisposedException(ObjectName));

    /// <summary>
    /// Attempts to complete the task with <see cref="ObjectDisposedException"/> exception.
    /// </summary>
    /// <param name="source">The task completion source.</param>
    /// <typeparam name="T">The type of the task.</typeparam>
    /// <returns><see langword="true"/> if operation was successful; otherwise, <see langword="false"/>.</returns>
    protected bool TrySetDisposedException<T>(TaskCompletionSource<T> source)
        => source.TrySetException(new ObjectDisposedException(ObjectName));

    /// <summary>
    /// Attempts to complete the task with <see cref="ObjectDisposedException"/> exception.
    /// </summary>
    /// <param name="source">The task completion source.</param>
    /// <returns><see langword="true"/> if operation was successful; otherwise, <see langword="false"/>.</returns>
    protected bool TrySetDisposedException(TaskCompletionSource source)
        => source.TrySetException(new ObjectDisposedException(ObjectName));

    /// <summary>
    /// Releases managed and unmanaged resources associated with this object.
    /// </summary>
    /// <param name="disposing"><see langword="true"/> if called from <see cref="Dispose()"/>; <see langword="false"/> if called from finalizer <see cref="Finalize()"/>.</param>
    protected virtual void Dispose(bool disposing)
        => _state = _disposedState;

    /// <summary>
    /// Releases managed resources associated with this object asynchronously.
    /// </summary>
    /// <remarks>
    /// This method makes sense only if derived class implements <see cref="IAsyncDisposable"/> interface.
    /// </remarks>
    /// <returns>The task representing asynchronous execution of this method.</returns>
    protected virtual ValueTask DisposeAsyncCore() {
        Dispose(true);
        return ValueTask.CompletedTask;
    }

    private async ValueTask DisposeAsyncImpl() {
        await DisposeAsyncCore().ConfigureAwait(false);
        Dispose(false);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Releases managed resources associated with this object asynchronously.
    /// </summary>
    /// <remarks>
    /// If derived class implements <see cref="IAsyncDisposable"/> then <see cref="IAsyncDisposable.DisposeAsync"/>
    /// can be trivially implemented through delegation of the call to this method.
    /// </remarks>
    /// <returns>The task representing asynchronous execution of this method.</returns>
    protected ValueTask DisposeAsync() => Interlocked.CompareExchange(ref _state, _disposingState, _notDisposedState) switch {
        _notDisposedState => DisposeAsyncImpl(),
        _disposingState => DisposeAsyncCore(),
        _ => ValueTask.CompletedTask,
    };

    /// <summary>
    /// Starts disposing this object.
    /// </summary>
    /// <returns><see langword="true"/> if cleanup operations can be performed; <see langword="false"/> if the object is already disposing.</returns>
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    protected bool TryBeginDispose()
        => Interlocked.CompareExchange(ref _state, _disposingState, _notDisposedState) is _notDisposedState;

    /// <summary>
    /// Releases all resources associated with this object.
    /// </summary>
    [SuppressMessage("Design", "CA1063", Justification = "No need to call Dispose(true) multiple times")]
    public void Dispose() {
        Dispose(TryBeginDispose());
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Throws an exception if this object is collected without being disposed
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if the object is collected without being disposed</exception>
    ~Disposable() {
        if (IsDisposed) {
            throw new InvalidOperationException($"{nameof(Disposable)} was not disposed. ");
        }
    }
}