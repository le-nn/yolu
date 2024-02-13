using System.Buffers;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Hadron.Buffers;

/// <summary>
/// Represents the memory obtained from the pool or allocated
/// on the stack or heap.
/// </summary>
/// <remarks>
/// This type is aimed to be compatible with memory allocated using <c>stackalloc</c> operator.
/// If stack allocation threshold is reached (e.g. <see cref="StackallocThreshold"/>) then it's possible to use pooled memory from
/// arbitrary <see cref="MemoryPool{T}"/> or <see cref="ArrayPool{T}.Shared"/>. Custom
/// <see cref="ArrayPool{T}"/> is not supported because default <see cref="ArrayPool{T}.Shared"/>
/// is optimized for per-CPU core allocation which is perfect when the same
/// thread is responsible for renting and releasing the array.
/// </remarks>
/// <example>
/// <code>
/// const int stackallocThreshold = 20;
/// var memory = size &lt;=stackallocThreshold ? new SpanOwner&lt;byte&gt;(stackalloc byte[stackallocThreshold], size) : new SpanOwner&lt;byte&gt;(size);
/// </code>
/// </example>
/// <typeparam name="T">The type of the elements in the rented memory.</typeparam>
[StructLayout(LayoutKind.Auto)]
public ref struct SpanOwner<T> {
    /// <summary>
    /// Global recommended number of elements that can be allocated on the stack.
    /// </summary>
    /// <remarks>
    /// This property is for internal purposes only and should not be referenced
    /// directly in your code.
    /// </remarks>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [CLSCompliant(false)]
    public static int StackallocThreshold { get; } = 1 + (LibrarySettings.StackallocThreshold / Unsafe.SizeOf<T>());

    private readonly object? _owner;
    private readonly Span<T> _memory;

    /// <summary>
    /// Rents the memory referenced by the span.
    /// </summary>
    /// <param name="span">The span that references the memory to rent.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public SpanOwner(Span<T> span)
        => _memory = span;

    /// <summary>
    /// Rents the memory referenced by the span.
    /// </summary>
    /// <param name="span">The span that references the memory to rent.</param>
    /// <param name="length">The actual length of the data.</param>
    public SpanOwner(Span<T> span, int length)
        : this(span.Slice(0, length)) {
    }

    /// <summary>
    /// Rents the memory from the pool.
    /// </summary>
    /// <param name="pool">The memory pool.</param>
    /// <param name="minBufferSize">The minimum size of the memory to rent.</param>
    /// <param name="exactSize"><see langword="true"/> to return the buffer of <paramref name="minBufferSize"/> length; otherwise, the returned buffer is at least of <paramref name="minBufferSize"/>.</param>
    /// <exception cref="ArgumentNullException"><paramref name="pool"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="minBufferSize"/> is less than or equal to zero.</exception>
    public SpanOwner(MemoryPool<T> pool, int minBufferSize, bool exactSize = true) {
        ArgumentNullException.ThrowIfNull(pool);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(minBufferSize);

        var owner = pool.Rent(minBufferSize);
        _memory = owner.Memory.Span;
        if (exactSize)
            _memory = _memory.Slice(0, minBufferSize);
        this._owner = owner;
    }

    /// <summary>
    /// Rents the memory from the pool.
    /// </summary>
    /// <param name="pool">The memory pool.</param>
    /// <exception cref="ArgumentNullException"><paramref name="pool"/> is <see langword="null"/>.</exception>
    public SpanOwner(MemoryPool<T> pool) {
        ArgumentNullException.ThrowIfNull(pool);
        var owner = pool.Rent();
        _memory = owner.Memory.Span;
        this._owner = owner;
    }

    /// <summary>
    /// Rents the memory from <see cref="ArrayPool{T}.Shared"/>, if <typeparamref name="T"/>
    /// contains at least one field of reference type; or use <see cref="NativeMemory"/>.
    /// </summary>
    /// <param name="minBufferSize">The minimum size of the memory to rent.</param>
    /// <param name="exactSize"><see langword="true"/> to return the buffer of <paramref name="minBufferSize"/> length; otherwise, the returned buffer is at least of <paramref name="minBufferSize"/>.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="minBufferSize"/> is less than or equal to zero.</exception>
    public SpanOwner(int minBufferSize, bool exactSize = true) {
        if (UseNativeAllocation) {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(minBufferSize);

            unsafe {
                var ptr = NativeMemory.Alloc((uint)minBufferSize, (uint)Unsafe.SizeOf<T>());
                _memory = new(ptr, minBufferSize);
            }

            _owner = Sentinel.Instance;
        }
        else {
            var owner = ArrayPool<T>.Shared.Rent(minBufferSize);
            _memory = exactSize ? new(owner, 0, minBufferSize) : new(owner);
            this._owner = owner;
        }
    }

    private static bool UseNativeAllocation
        => !LibrarySettings.DisableNativeAllocation && !RuntimeHelpers.IsReferenceOrContainsReferences<T>() && AlignOf<T>() <= nuint.Size;
    /// <summary>
    /// Gets the alignment requirement for type <typeparamref name="T"/>, in bytes.
    /// </summary>
    /// <typeparam name="T">The target type.</typeparam>
    /// <returns>The alignment of the type <typeparamref name="T"/>.</returns>
    /// <seealso href="https://en.cppreference.com/w/c/language/_Alignof">_Alignof operator in C++</seealso>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int AlignOf<U>()
        => Unsafe.SizeOf<AlignmentHelperType<U>>() - Unsafe.SizeOf<U>();

    [StructLayout(LayoutKind.Sequential)]
    [ExcludeFromCodeCoverage]
    private readonly struct AlignmentHelperType<U> {
        private readonly byte field1;
        private readonly U field2;
    }

    /// <summary>
    /// Gets the rented memory.
    /// </summary>
    public readonly Span<T> Span => _memory;

    /// <summary>
    /// Gets a value indicating that this object
    /// doesn't reference rented memory.
    /// </summary>
    public readonly bool IsEmpty => _memory.IsEmpty;

    /// <summary>
    /// Converts the reference to the already allocated memory
    /// into the rental object.
    /// </summary>
    /// <param name="span">The allocated memory to convert.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator SpanOwner<T>(Span<T> span)
        => new(span);

    /// <summary>
    /// Gets length of the rented memory.
    /// </summary>
    public readonly int Length => _memory.Length;

    /// <summary>
    /// Gets the memory element by its index.
    /// </summary>
    /// <param name="index">The index of the memory element.</param>
    /// <returns>The managed pointer to the memory element.</returns>
    public readonly ref T this[int index] => ref _memory[index];

    /// <summary>
    /// Obtains managed pointer to the first element of the rented array.
    /// </summary>
    /// <returns>The managed pointer to the first element of the rented array.</returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly ref T GetPinnableReference() => ref _memory.GetPinnableReference();

    /// <summary>
    /// Gets textual representation of the rented memory.
    /// </summary>
    /// <returns>The textual representation of the rented memory.</returns>
    public override readonly string ToString() => _memory.ToString();

    /// <summary>
    /// Returns the memory back to the pool.
    /// </summary>
    public void Dispose() {
        if (_owner is T[] array) {
            ArrayPool<T>.Shared.Return(array, RuntimeHelpers.IsReferenceOrContainsReferences<T>());
        }
        else if (ReferenceEquals(_owner, Sentinel.Instance)) {
            unsafe {
                NativeMemory.Free(Unsafe.AsPointer(ref MemoryMarshal.GetReference(_memory)));
            }
        }
        else {
            Unsafe.As<IDisposable>(_owner)?.Dispose();
        }

        this = default;
    }
}