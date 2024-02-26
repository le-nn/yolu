using Yolu.Buffers;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Yolu.Collections;

/// <summary>
/// Provides extension methods for type <see cref="Span{T}"/> and <see cref="ReadOnlySpan{T}"/>.
/// </summary>
public static partial class Span {
    /// <summary>
    /// Determines whether two memory blocks identified by the given spans contain the same set of elements.
    /// </summary>
    /// <remarks>
    /// This method performs bitwise equality between each pair of elements.
    /// </remarks>
    /// <typeparam name="T">The type of elements in the span.</typeparam>
    /// <param name="x">The first memory span to compare.</param>
    /// <param name="y">The second memory span to compare.</param>
    /// <returns><see langword="true"/>, if both memory blocks are equal; otherwise, <see langword="false"/>.</returns>
    public static unsafe bool BitwiseEquals<T>(this ReadOnlySpan<T> x, ReadOnlySpan<T> y)
        where T : unmanaged {
        if (x.Length == y.Length) {
            for (int maxSize = Array.MaxLength / sizeof(T), size; !x.IsEmpty; x = x.Slice(size), y = y.Slice(size)) {
                size = Math.Min(maxSize, x.Length);
                var sizeInBytes = size * sizeof(T);
                var partX = MemoryMarshal.CreateReadOnlySpan(ref Unsafe.As<T, byte>(ref MemoryMarshal.GetReference(x)), sizeInBytes);
                var partY = MemoryMarshal.CreateReadOnlySpan(ref Unsafe.As<T, byte>(ref MemoryMarshal.GetReference(y)), sizeInBytes);
                if (MemoryExtensions.SequenceEqual(partX, partY) is false)
                    return false;
            }

            return true;
        }

        return false;
    }

    internal static bool BitwiseEquals<T>(T[] x, T[] y)
        where T : unmanaged
        => BitwiseEquals(new ReadOnlySpan<T>(x), new ReadOnlySpan<T>(y));

    /// <summary>
    /// Compares content of the two memory blocks identified by the given spans.
    /// </summary>
    /// <typeparam name="T">The type of elements in the span.</typeparam>
    /// <param name="x">The first memory span to compare.</param>
    /// <param name="y">The second array to compare.</param>
    /// <returns>Comparison result.</returns>
    public static unsafe int BitwiseCompare<T>(this ReadOnlySpan<T> x, ReadOnlySpan<T> y)
        where T : unmanaged {
        var result = x.Length;
        result = result.CompareTo(y.Length);
        if (result is 0) {
            for (int maxSize = Array.MaxLength / sizeof(T), size; !x.IsEmpty; x = x.Slice(size), y = y.Slice(size)) {
                size = Math.Min(maxSize, x.Length);
                var sizeInBytes = size * sizeof(T);
                var partX = MemoryMarshal.CreateReadOnlySpan(ref Unsafe.As<T, byte>(ref MemoryMarshal.GetReference(x)), sizeInBytes);
                var partY = MemoryMarshal.CreateReadOnlySpan(ref Unsafe.As<T, byte>(ref MemoryMarshal.GetReference(y)), sizeInBytes);
                result = MemoryExtensions.SequenceCompareTo(partX, partY);
                if (result is not 0)
                    break;
            }
        }

        return result;
    }

    /// <summary>
    /// Trims the span to specified length if it exceeds it.
    /// If length is less that <paramref name="maxLength" /> then the original span returned.
    /// </summary>
    /// <typeparam name="T">The type of items in the span.</typeparam>
    /// <param name="span">A contiguous region of arbitrary memory.</param>
    /// <param name="maxLength">Maximum length.</param>
    /// <returns>Trimmed span.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="maxLength"/> is less than zero.</exception>
    public static Span<T> TrimLength<T>(this Span<T> span, int maxLength) {
        switch (maxLength) {
            case < 0:
                throw new ArgumentOutOfRangeException(nameof(maxLength));
            case 0:
                span = default;
                break;
            default:
                if (span.Length > maxLength)
                    span = MemoryMarshal.CreateSpan(ref MemoryMarshal.GetReference(span), maxLength);
                break;
        }

        return span;
    }

    /// <summary>
    /// Trims the span to specified length if it exceeds it.
    /// If length is less that <paramref name="maxLength" /> then the original span returned.
    /// </summary>
    /// <typeparam name="T">The type of items in the span.</typeparam>
    /// <param name="span">A contiguous region of arbitrary memory.</param>
    /// <param name="maxLength">Maximum length.</param>
    /// <returns>Trimmed span.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="maxLength"/> is less than zero.</exception>
    public static ReadOnlySpan<T> TrimLength<T>(this ReadOnlySpan<T> span, int maxLength)
        => TrimLength(MemoryMarshal.CreateSpan(ref MemoryMarshal.GetReference(span), span.Length), maxLength);

    /// <summary>
    /// Trims the span to specified length if it exceeds it.
    /// If length is less that <paramref name="maxLength" /> then the original span returned.
    /// </summary>
    /// <typeparam name="T">The type of items in the span.</typeparam>
    /// <param name="span">A contiguous region of arbitrary memory.</param>
    /// <param name="maxLength">Maximum length.</param>
    /// <param name="rest">The rest of <paramref name="span"/>.</param>
    /// <returns>Trimmed span.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="maxLength"/> is less than zero.</exception>
    public static Span<T> TrimLength<T>(this Span<T> span, int maxLength, out Span<T> rest) {
        switch (maxLength) {
            case < 0:
                throw new ArgumentOutOfRangeException(nameof(maxLength));
            case 0:
                rest = span;
                span = default;
                break;
            default:
                span = TrimLengthCore(span, maxLength, out rest);
                break;
        }

        return span;
    }

    private static Span<T> TrimLengthCore<T>(Span<T> span, int maxLength, out Span<T> rest) {
        var length = span.Length;
        if (length > maxLength) {
            ref var ptr = ref MemoryMarshal.GetReference(span);
            span = MemoryMarshal.CreateSpan(ref ptr, maxLength);
            rest = MemoryMarshal.CreateSpan(ref Unsafe.Add(ref ptr, maxLength), length - maxLength);
        }
        else {
            rest = default;
        }

        return span;
    }

    internal static void ForEach<T>(ReadOnlySpan<T> span, Action<T> action) {
        foreach (var item in span)
            action(item);
    }

    /// <summary>
    /// Iterates over elements of the span.
    /// </summary>
    /// <typeparam name="T">The type of the elements.</typeparam>
    /// <param name="span">The span to iterate.</param>
    /// <param name="action">The action to be applied for each element of the span.</param>
    public static void ForEach<T>(this Span<T> span, RefAction<T, int> action) {
        for (var i = 0; i < span.Length; i++)
            action(ref span[i], i);
    }

    /// <summary>
    /// Iterates over elements of the span.
    /// </summary>
    /// <typeparam name="T">The type of the elements.</typeparam>
    /// <typeparam name="TArg">The type of the argument to be passed to the action.</typeparam>
    /// <param name="span">The span to iterate.</param>
    /// <param name="action">The action to be applied for each element of the span.</param>
    /// <param name="arg">The argument to be passed to the action.</param>
    /// <exception cref="ArgumentNullException"><paramref name="action"/> is zero.</exception>
    public static unsafe void ForEach<T, TArg>(this Span<T> span, delegate*<ref T, TArg, void> action, TArg arg) {
        ArgumentNullException.ThrowIfNull(action);

        foreach (ref var item in span)
            action(ref item, arg);
    }

    /// <summary>
    /// Converts contiguous memory identified by the specified pointer
    /// into <see cref="Span{T}"/>.
    /// </summary>
    /// <param name="value">The managed pointer.</param>
    /// <typeparam name="T">The type of the pointer.</typeparam>
    /// <returns>The span of contiguous memory.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe Span<byte> AsBytes<T>(ref T value)
        where T : unmanaged
        => MemoryMarshal.CreateSpan(ref Unsafe.As<T, byte>(ref value), sizeof(T));

    /// <summary>
    /// Converts contiguous memory identified by the specified pointer
    /// into <see cref="ReadOnlySpan{T}"/>.
    /// </summary>
    /// <param name="value">The managed pointer.</param>
    /// <typeparam name="T">The type of the pointer.</typeparam>
    /// <returns>The span of contiguous memory.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<byte> AsReadOnlyBytes<T>(ref readonly T value)
        where T : unmanaged
        => AsBytes(ref Unsafe.AsRef(in value));

    /// <summary>
    /// Converts contiguous memory identified by the specified pointer
    /// into <see cref="Span{T}"/>.
    /// </summary>
    /// <param name="pointer">The typed pointer.</param>
    /// <typeparam name="T">The type of the pointer.</typeparam>
    /// <returns>The span of contiguous memory.</returns>
    [CLSCompliant(false)]
    public static unsafe Span<byte> AsBytes<T>(T* pointer)
        where T : unmanaged
        => AsBytes(ref pointer[0]);

    internal static T[] ConcatToArray<T>(ReadOnlySpan<T> x, ReadOnlySpan<T> y) {
        T[] result;
        if (x.IsEmpty && y.IsEmpty) {
            result = [];
        }
        else {
            result = GC.AllocateUninitializedArray<T>(x.Length + y.Length);
            x.CopyTo(result);
            y.CopyTo(result.AsSpan(x.Length));
        }

        return result;
    }

    /// <summary>
    /// Copies the contents from the source span into a destination span.
    /// </summary>
    /// <param name="source">Source memory.</param>
    /// <param name="destination">Destination memory.</param>
    /// <param name="writtenCount">The number of copied elements.</param>
    /// <typeparam name="T">The type of the elements in the span.</typeparam>
    public static void CopyTo<T>(this ReadOnlySpan<T> source, Span<T> destination, out int writtenCount) {
        if (source.Length > destination.Length) {
            source = MemoryMarshal.CreateReadOnlySpan(ref MemoryMarshal.GetReference(source), writtenCount = destination.Length);
        }
        else {
            writtenCount = source.Length;
        }

        source.CopyTo(destination);
    }

    /// <summary>
    /// Copies the contents from the source span into a destination span.
    /// </summary>
    /// <param name="source">Source memory.</param>
    /// <param name="destination">Destination memory.</param>
    /// <param name="writtenCount">The number of copied elements.</param>
    /// <typeparam name="T">The type of the elements in the span.</typeparam>
    public static void CopyTo<T>(this Span<T> source, Span<T> destination, out int writtenCount)
        => CopyTo((ReadOnlySpan<T>)source, destination, out writtenCount);

    internal static bool ElementAt<T>(ReadOnlySpan<T> span, int index, [MaybeNullWhen(false)] out T element) {
        if ((uint)index < (uint)span.Length) {
            element = span[index];
            return true;
        }

        element = default;
        return false;
    }

    /// <summary>
    /// Initializes each element in the span.
    /// </summary>
    /// <remarks>
    /// This method has the same behavior as <see cref="Array.Initialize"/> and supports reference types.
    /// </remarks>
    /// <typeparam name="T">The type of the element.</typeparam>
    /// <param name="span">The span of elements.</param>
    public static void Initialize<T>(this Span<T> span)
        where T : new() {
        foreach (ref var item in span)
            item = new T();
    }

    /// <summary>
    /// Upcasts the span.
    /// </summary>
    /// <typeparam name="T">The source type.</typeparam>
    /// <typeparam name="TBase">The target type.</typeparam>
    /// <param name="span">The span over elements.</param>
    /// <returns>The span pointing to the same memory as <paramref name="span"/>.</returns>
    public static ReadOnlySpan<TBase> Contravariance<T, TBase>(this ReadOnlySpan<T> span)
        where T : class?, TBase
        where TBase : class?
        => MemoryMarshal.CreateReadOnlySpan(ref Unsafe.As<T, TBase>(ref MemoryMarshal.GetReference(span)), span.Length);

    /// <summary>
    /// Moves the range within the span to the specified index.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the span.</typeparam>
    /// <param name="span">The span of elements to modify.</param>
    /// <param name="range">The range of elements within <paramref name="span"/> to move.</param>
    /// <param name="destinationIndex">The index of the element before which <paramref name="range"/> of elements will be placed.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destinationIndex"/> is not a valid index within <paramref name="span"/>.</exception>
    public static void Move<T>(this Span<T> span, Range range, Index destinationIndex) {
        var (sourceIndex, length) = range.GetOffsetAndLength(span.Length);

        if (length is not 0)
            MoveCore(span, sourceIndex, destinationIndex.GetOffset(span.Length), length);

        static void MoveCore(Span<T> span, int sourceIndex, int destinationIndex, int length) {
            // prepare buffers
            Span<T> source = span.Slice(sourceIndex, length),
                destination,
                sourceGap,
                destinationGap;
            if (sourceIndex > destinationIndex) {
                sourceGap = span[destinationIndex..sourceIndex];
                destinationGap = span.Slice(destinationIndex + length);

                destination = span.Slice(destinationIndex, length);
            }
            else {
                var endOfLeftSegment = sourceIndex + length;
                switch (endOfLeftSegment.CompareTo(destinationIndex)) {
                    case 0:
                        return;
                    case > 0:
                        throw new ArgumentOutOfRangeException(nameof(destinationIndex));
                    case < 0:
                        sourceGap = span[endOfLeftSegment..destinationIndex];
                        destinationGap = span.Slice(sourceIndex);

                        destination = span.Slice(destinationIndex - length, length);
                        break;
                }
            }

            // Perf: allocate buffer for smallest part of the span
            if (source.Length > sourceGap.Length) {
                length = sourceGap.Length;

                var temp = source;
                source = sourceGap;
                sourceGap = temp;

                temp = destination;
                destination = destinationGap;
                destinationGap = temp;
            }
            else {
                Debug.Assert(length == source.Length);
            }

            // prepare buffer
            SpanOwner<T> buffer;
            if (RuntimeHelpers.IsReferenceOrContainsReferences<T>() || length > SpanOwner<T>.StackallocThreshold) {
                buffer = new(length);
            }
            else {
                unsafe {
                    void* bufferPtr = stackalloc byte[checked(Unsafe.SizeOf<T>() * length)];
                    buffer = new Span<T>(bufferPtr, length);
                }
            }

            // rearrange buffers
            source.CopyTo(buffer.Span);
            sourceGap.CopyTo(destinationGap);
            buffer.Span.CopyTo(destination);

            buffer.Dispose();
        }
    }
}