using System.Runtime.CompilerServices;

namespace Yolu.Tasks;

public static partial class TaskUtils {
    /// <summary>
    /// Converts the specified object to a <see cref="Task{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the object.</typeparam>
    /// <param name="obj">The object to convert.</param>
    /// <returns>A <see cref="Task{T}"/> representing the specified object.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Task<T> AsTask<T>(this T obj) {
        return Task.FromResult(obj);
    }

    /// <summary>
    /// Converts the specified object to a <see cref="ValueTask{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the object.</typeparam>
    /// <param name="obj">The object to convert.</param>
    /// <returns>A <see cref="ValueTask{T}"/> representing the specified object.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ValueTask<T> AsValueTask<T>(this T obj) {
        return ValueTask.FromResult(obj);
    }
}
