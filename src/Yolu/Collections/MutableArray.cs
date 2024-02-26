namespace Yolu.Collections;

internal static class MutableArrayClassBuilder {
    /// <summary>
    /// Creates a new instance of <see cref="IMutableArray{T}"/> from the specified span.
    /// </summary>
    /// <typeparam name="T">The type of elements in the array.</typeparam>
    /// <param name="span">The span of elements.</param>
    /// <returns>A new instance of <see cref="IMutableArray{T}"/>.</returns>
    public static IMutableArray<T> CreateIMutableArray<T>(ReadOnlySpan<T> span) => new MutableArray<T>(span.ToArray());

    /// <summary>
    /// Creates a new instance of <see cref="MutableArray{T}"/> from the specified span.
    /// </summary>
    /// <typeparam name="T">The type of elements in the array.</typeparam>
    /// <param name="span">The span of elements.</param>
    /// <returns>A new instance of <see cref="MutableArray{T}"/>.</returns>
    public static MutableArray<T> CreateMutableArray<T>(ReadOnlySpan<T> span) => new(span.ToArray());
}

/// <summary>
/// Represents a mutable array collection and implements <see cref="IMutableArray{T}"/>.
/// </summary>
/// <typeparam name="T">The type of elements in the array.</typeparam>
public class MutableArray<T> : List<T>, IMutableArray<T> {
    /// <summary>
    /// Initializes a new instance of the <see cref="MutableArray{T}"/> class with the specified size and factory function.
    /// </summary>
    /// <param name="size">The size of the array.</param>
    /// <param name="factory">The factory function used to create elements of the array.</param>
    public MutableArray(int size, Func<int, T> factory) : base(Enumerable.Range(0, size).Select(factory)) {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MutableArray{T}"/> class with the specified size.
    /// </summary>
    /// <param name="size">The size of the array.</param>
    public MutableArray(int size) : base(size) {

    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MutableArray{T}"/> class with the specified items.
    /// </summary>
    /// <param name="items">The items to be added to the array.</param>
    public MutableArray(IEnumerable<T> items) : base(items) {

    }

    /// <inheritdoc/>
    public override string? ToString() {
        if (Count < 1000) {
            return $"MutableArray<{typeof(T).Name}>[{string.Join(", ", this)}]";
        }

        return base.ToString();
    }
}
