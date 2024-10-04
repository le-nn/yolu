using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Yolu.Collections;

public static class MutableArrayBuilder {
    /// <summary>
    /// Creates a new instance of <see cref="IMutableArray{T}"/> from the specified span.
    /// </summary>
    /// <typeparam name="T">The type of elements in the array.</typeparam>
    /// <param name="span">The span of elements.</param>
    /// <returns>A new instance of <see cref="IMutableArray{T}"/>.</returns>
    public static IMutableArray<T> CreateIMutableArray<T>(ReadOnlySpan<T> span) => new MutableArray<T>(span);

    /// <summary>
    /// Creates a new instance of <see cref="MutableArray{T}"/> from the specified span.
    /// </summary>
    /// <typeparam name="T">The type of elements in the array.</typeparam>
    /// <param name="span">The span of elements.</param>
    /// <returns>A new instance of <see cref="MutableArray{T}"/>.</returns>
    public static MutableArray<T> CreateMutableArray<T>(ReadOnlySpan<T> span) => new(span);
}

internal sealed class ICollectionDebugView<T> {
    private readonly ICollection<T> _collection;

    public ICollectionDebugView(ICollection<T> collection) {
        ArgumentNullException.ThrowIfNull(collection);

        _collection = collection;
    }

    [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
    public T[] Items {
        get {
            var items = new T[_collection.Count];
            _collection.CopyTo(items, 0);
            return items;
        }
    }
}

/// <summary>
/// Represents a mutable array collection and implements <see cref="IMutableArray{T}"/>.
/// </summary>
/// <typeparam name="T">The type of elements in the array.</typeparam>
[CollectionBuilder(typeof(MutableArrayBuilder), nameof(MutableArrayBuilder.CreateMutableArray))]
[DebuggerTypeProxy(typeof(ICollectionDebugView<>))]
public class MutableArray<T> : List<T>, IMutableArray<T> {
    public static MutableArray<T> Empty { get; } = [];

    public bool IsEmpty => this is [];

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
    public MutableArray(int size = 0) : base(size) {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MutableArray{T}"/> class with the specified items.
    /// </summary>
    /// <param name="items">The items to be added to the array.</param>
    public MutableArray(IEnumerable<T> items) : base(items) {

    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MutableArray{T}"/> class with the specified items.
    /// </summary>
    /// <param name="items">The items to be added to the array.</param>
    public MutableArray(T[] items) : base(items) {

    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MutableArray{T}"/> class with the specified items.
    /// </summary>
    /// <param name="items">The items to be added to the span.</param>
    public MutableArray(ReadOnlySpan<T> items) : base(items.ToArray()) {

    }

    /// <summary>
    /// Gets the number of elements in the collection.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int Length {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get =>this.Count;
        
    }

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override string? ToString() {
        return this.ToString();
    }

    /// <summary>
    /// Returns a string that represents the current object.
    /// </summary>
    /// <param name="isTypeOnly"> If set true MutableArray<T>, otherwise MutableArray<T>[1, 2, 3, 4 ...]. </param>
    /// <returns>A string that represents the current object.</returns>
    public string? ToString(bool isTypeOnly = false ) {
        if (isTypeOnly) {
            return base.ToString();
        }

        if (Count < 1000) {
            return $"MutableArray<{typeof(T).Name}>[{string.Join(", ", this)}]";
        }

        return base.ToString();
    }
}
