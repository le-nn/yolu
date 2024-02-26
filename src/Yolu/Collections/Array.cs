using System.Buffers;
using System.Collections;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace Yolu.Collections;

public static class ArrayClassBuilder {
    /// <summary>
    /// Creates a new instance of the <see cref="Array{T}"/> class with the specified span.
    /// </summary>
    /// <typeparam name="T">The type of elements in the array.</typeparam>
    /// <param name="span">The span of elements.</param>
    /// <returns>A new instance of the <see cref="Array{T}"/> class.</returns>
    public static IArray<T> CreateIArray<T>(ReadOnlySpan<T> span) => new Array<T>(span);

    /// <summary>
    /// Creates a new instance of the <see cref="Array{T}"/> class with the specified span.
    /// </summary>
    /// <typeparam name="T">The type of elements in the array.</typeparam>
    /// <param name="span">The span of elements.</param>
    /// <returns>A new instance of the <see cref="Array{T}"/> class.</returns>
    public static Array<T> CreateArray<T>(ReadOnlySpan<T> span) => new(span);
}

/// <summary>
/// Represents a generic immutable array collection.
/// </summary>
/// <typeparam name="T">The type of elements in the array.</typeparam>
[CollectionBuilder(typeof(ArrayClassBuilder), nameof(ArrayClassBuilder.CreateArray))]
public partial class Array<T> : IArray<T> {
    /// <summary>
    /// Gets an empty array.
    /// </summary>
    public static Array<T> Empty { get; } = new Array<T>(Array.Empty<T>());

    private readonly T[] _array;

    /// <summary>
    /// Gets or sets the element at the specified index in the read-only list.
    /// </summary>
    /// <param name="index">The zero-based index of the element to get.</param>
    /// <returns>The element at the specified index in the read-only list.</returns>
    /// <exception cref="NotSupportedException">Always thrown from the setter.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the <see cref="IsDefault"/> property returns true.</exception>
    public T this[int index] {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get {
            // We intentionally do not check this._array != null, and throw NullReferenceException
            // if this is called while uninitialized.
            // The reason for this is perf.
            // Length and the indexer must be absolutely trivially implemented for the JIT optimization
            // of removing array bounds checking to work.
            return _array[index];
        }
    }

    /// <summary>
    /// Gets the number of elements in the array.
    /// </summary>
    public int Count {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _array.Length;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Array{T}"/> class with the specified array.
    /// </summary>
    /// <param name="values">The array of elements.</param>
    public Array(T[]? values) {
        _array = values ?? [];
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Array{T}"/> class with the elements from the specified <see cref="ReadOnlySpan{T}"/>.
    /// </summary>
    /// <param name="values">The <see cref="ReadOnlySpan{T}"/> containing the elements.</param>
    public Array(ReadOnlySpan<T> values) {
        _array = values.ToArray();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Array{T}"/> class with the elements from the specified collection.
    /// </summary>
    /// <param name="values">The collection of elements.</param>
    public Array(IEnumerable<T>? values) {
        _array = values?.ToArray() ?? [];
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Array{T}"/> class with the specified size.
    /// </summary>
    /// <param name="size">The size of the array.</param>
    public Array(int size) {
        if (size is 0) {
            _array = [];
        }
        else {
            _array = new T[size];
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Array{T}"/> class with the specified size and factory function.
    /// </summary>
    /// <param name="size">The size of the array.</param>
    /// <param name="factory">The factory function used to create each element of the array.</param>
    public Array(int size, Func<int, T> factory) {
        var arr = _array = new T[size];
        for (var i = 0; i < size; i++) {
            arr[i] = factory(i);
        }
    }

    /// <summary>
    /// Returns an enumerator that iterates through the array.
    /// </summary>
    /// <returns>An enumerator that can be used to iterate through the array.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Pure]
    IEnumerator<T> IEnumerable<T>.GetEnumerator() {
        return EnumeratorObject.Create(_array);
    }

    /// <summary>
    /// Returns an enumerator that iterates through the array.
    /// </summary>
    /// <returns>An enumerator that can be used to iterate through the array.</returns>
    IEnumerator IEnumerable.GetEnumerator() {
        return EnumeratorObject.Create(_array);
    }

    /// <summary>
    /// Returns an enumerator that iterates through the array.
    /// </summary>
    /// <returns>An enumerator that can be used to iterate through the array.</returns>
    public Enumerator GetEnumerator() {
        return new Enumerator(_array);
    }

    /// <summary>
    /// Checks equality between two instances.
    /// </summary>
    /// <param name="left">The instance to the left of the operator.</param>
    /// <param name="right">The instance to the right of the operator.</param>
    /// <returns><c>true</c> if the values' underlying arrays are reference equal; <c>false</c> otherwise.</returns>
    public static bool operator ==(Array<T>? left, Array<T>? right) {
        if (left is null && right is null) {
            return true;
        }

        return left?.Equals(right) ?? false;
    }

    /// <summary>
    /// Checks inequality between two instances.
    /// </summary>
    /// <param name="left">The instance to the left of the operator.</param>
    /// <param name="right">The instance to the right of the operator.</param>
    /// <returns><c>true</c> if the values' underlying arrays are reference not equal; <c>false</c> otherwise.</returns>
    public static bool operator !=(Array<T>? left, Array<T>? right) {
        if (left is null && right is null) {
            return true;
        }

        return left?.Equals(right) ?? false;
    }

    /// <summary>
    /// Gets or sets the element at the specified index in the read-only list.
    /// </summary>
    /// <param name="index">The zero-based index of the element to get.</param>
    /// <returns>The element at the specified index in the read-only list.</returns>
    /// <exception cref="NotSupportedException">Always thrown from the setter.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the <see cref="IsDefault"/> property returns true.</exception>
    T IReadOnlyList<T>.this[int index] {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get {
            var self = this;
            self.ThrowInvalidOperationIfNotInitialized();
            return self[index];
        }
    }

    /// <inheritdoc/>
    public override string? ToString() {
        if (Count < 1000) {
            return $"Array<{typeof(T).Name}>[{string.Join(", ", _array)}]";
        }

        return base.ToString();
    }

    /// <summary>
    /// Performs the specified action on each element of the array.
    /// </summary>
    /// <param name="action">The action to perform on each element.</param>
    public void ForEach(Action<T> action) {
        ArgumentNullException.ThrowIfNull(action);

        var array = _array;
        for (var i = 0; i < array.Length; i++) {
            action(array[i]);
        }
    }

    /// <summary>
    /// Gets a value indicating whether this collection is empty.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsEmpty {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get { return this.Length == 0; }
    }

    /// <summary>
    /// Gets the number of elements in the collection.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int Length {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get {
            // We intentionally do not check this._array != null, and throw NullReferenceException
            // if this is called while uninitialized.
            // The reason for this is perf.
            // Length and the indexer must be absolutely trivially implemented for the JIT optimization
            // of removing array bounds checking to work.
            return this._array.Length;
        }
    }

    /// <summary>
    /// Gets the number of elements in the collection.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if the <see cref="IsDefault"/> property returns true.</exception>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    int IReadOnlyCollection<T>.Count {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get {
            var self = this;
            self.ThrowInvalidOperationIfNotInitialized();
            return self.Length;
        }
    }

    /// <summary>
    /// Gets a value indicating whether this struct was initialized without an actual array instance.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsDefault {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _array is [];
    }

    /// <summary>
    /// Gets an untyped reference to the array.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Array SystemArray {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _array;
    }

    /// <summary>
    /// Gets the string to display in the debugger watches window for this instance.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get {
            var self = this;
            return self.IsDefault ? "Uninitialized" : string.Format(CultureInfo.CurrentCulture, "Length = {0}", self.Length);
        }
    }

    /// <summary>
    /// Throws a <see cref="NullReferenceException"/> if the specified array is uninitialized.
    /// </summary>
    private static void ThrowNullRefIfNotInitialized(Array<T> array) {
        array.ThrowNullRefIfNotInitialized();
    }

    /// <summary>
    /// Throws a null reference exception if the array field is null.
    /// </summary>
    internal void ThrowNullRefIfNotInitialized() {
        // Force NullReferenceException if array is null by touching its Length.
        // This way of checking has a nice property of requiring very little code
        // and not having any conditions/branches. 
        // In a faulting scenario we are relying on hardware to generate the fault.
        // And in the non-faulting scenario (most common) the check is virtually free since 
        // if we are going to do anything with the array, we will need Length anyways
        // so touching it, and potentially causing a cache miss, is not going to be an 
        // extra expense.
        var unused = this._array.Length;

        // This line is a workaround for a bug in C# compiler
        // The code in this line will not be emitted, but it will prevent incorrect 
        // optimizing away of "Length" call above in Release builds.
        // TODO: remove the workaround when building with Roslyn which does not have this bug.
        var unused2 = unused;
    }
}
