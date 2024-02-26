namespace Yolu;

public static class DisposableUtils {
    /// <summary>
    /// Disposes many objects.
    /// </summary>
    /// <param name="objects">An array of objects to dispose.</param>
    public static void Dispose(IEnumerable<IDisposable?> objects) {
        foreach (var obj in objects) {
            obj?.Dispose();
        }
    }

    /// <summary>
    /// Disposes many objects.
    /// </summary>
    /// <param name="objects">An array of objects to dispose.</param>
    /// <returns>The task representing asynchronous execution of this method.</returns>
    public static async ValueTask DisposeAsync(IEnumerable<IAsyncDisposable?> objects) {
        foreach (var obj in objects) {
            if (obj is not null) {
                await obj.DisposeAsync().ConfigureAwait(false);
            }
        }
    }

    /// <summary>
    /// Disposes many objects in safe manner.
    /// </summary>
    /// <param name="objects">An array of objects to dispose.</param>
    public static void Dispose(ReadOnlySpan<IDisposable?> objects) {
        foreach (var obj in objects) {
            obj?.Dispose();
        }
    }

    /// <summary>
    /// Disposes many objects in safe manner.
    /// </summary>
    /// <param name="objects">An array of objects to dispose.</param>
    /// <returns>The task representing asynchronous execution of this method.</returns>
    public static ValueTask DisposeAsync(params IAsyncDisposable?[] objects)
        => DisposeAsync(objects.AsEnumerable());
}