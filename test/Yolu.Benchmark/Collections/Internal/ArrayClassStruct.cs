using Yolu.Collections;
using System.Collections;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace Yolu.Benchmark.Collections.Internal;

public static class ArrayStructBuilder {
    public static ArrayStruct<T> CreateArrayStruct<T>(ReadOnlySpan<T> span) => new(span);
}

[CollectionBuilder(typeof(ArrayStructBuilder), nameof(ArrayStructBuilder.CreateArrayStruct))]
public readonly struct ArrayStruct<T> : IArray<T> {
    public static readonly ArrayStruct<T> Empty = new(Array.Empty<T>());
    private readonly T[] _array;

    public T this[int index] {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get {
            // We intentionally do not check this.array != null, and throw NullReferenceException
            // if this is called while uninitialized.
            // The reason for this is perf.
            // Length and the indexer must be absolutely trivially implemented for the JIT optimization
            // of removing array bounds checking to work.
            return _array[index];
        }
    }

    public int Count {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get { return _array.Length; }
    }

    public ArrayStruct(T[] values) {
        _array = values;
    }

    public ArrayStruct(ReadOnlySpan<T> values) {
        _array = values.ToArray();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Pure]
    IEnumerator<T> IEnumerable<T>.GetEnumerator() {
        return EnumeratorObject.Create(_array);
    }


    IEnumerator IEnumerable.GetEnumerator() {
        return EnumeratorObject.Create(_array);
    }

    public Enumerator GetEnumerator() {
        return new(_array);
    }


    /// <summary>
    /// An array enumerator.
    /// </summary>
    /// <remarks>
    /// It is important that this enumerator does NOT implement <see cref="IDisposable"/>.
    /// We want the iterator to inline when we do foreach and to not result in
    /// a try/finally frame in the client.
    /// </remarks>
    public struct Enumerator {

        /// <summary> 
        /// The array being enumerated.
        /// </summary>
        private readonly T[] _array;

        /// <summary>
        /// The currently enumerated position.
        /// </summary>
        /// <value>
        /// -1 before the first call to <see cref="MoveNext"/>.
        /// >= this.array.Length after <see cref="MoveNext"/> returns false.
        /// </value>
        private int _index;

        /// <summary>
        /// Initializes a new instance of the <see cref="Enumerator"/> struct.
        /// </summary>
        /// <param name="array">The array to enumerate.</param>
        internal Enumerator(T[] array) {
            _array = array;
            _index = -1;
        }

        /// <summary>
        /// Gets the currently enumerated value.
        /// </summary>
        public T Current {
            get {
                // PERF: no need to do a range check, we already did in MoveNext.
                // if user did not call MoveNext or ignored its result (incorrect use)
                // he will still get an exception from the array access range check.
                return _array[_index];
            }
        }

        /// <summary>
        /// Advances to the next value to be enumerated.
        /// </summary>
        /// <returns><c>true</c> if another item exists in the array; <c>false</c> otherwise.</returns>
        public bool MoveNext() {
            return ++_index < _array.Length;
        }
    }

    /// <summary>
    /// An array enumerator that implements <see cref="IEnumerator{T}"/> pattern (including <see cref="IDisposable"/>).
    /// </summary>
    private class EnumeratorObject : IEnumerator<T> {
        /// <summary>
        /// A shareable singleton for enumerating empty arrays.
        /// </summary>
        private static readonly IEnumerator<T> s_EmptyEnumerator = new EnumeratorObject(ArrayStruct<T>.Empty._array);

        /// <summary>
        /// The array being enumerated.
        /// </summary>
        private readonly T[] _array;

        /// <summary>
        /// The currently enumerated position.
        /// </summary>
        /// <value>
        /// -1 before the first call to <see cref="MoveNext"/>.
        /// this.array.Length - 1 after MoveNext returns false.
        /// </value>
        private int _index;

        /// <summary>
        /// Initializes a new instance of the <see cref="Enumerator"/> class.
        /// </summary>
        private EnumeratorObject(T[] array) {
            _index = -1;
            _array = array;
        }

        /// <summary>
        /// Gets the currently enumerated value.
        /// </summary>
        public T Current {
            get {
                // this.index >= 0 && this.index < this.array.Length
                // unsigned compare performs the range check above in one compare
                if ((uint)_index < (uint)_array.Length) {
                    return _array[_index];
                }

                // Before first or after last MoveNext.
                throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets the currently enumerated value.
        /// </summary>
        object IEnumerator.Current {
            get { return Current; }
        }

        /// <summary>
        /// If another item exists in the array, advances to the next value to be enumerated.
        /// </summary>
        /// <returns><c>true</c> if another item exists in the array; <c>false</c> otherwise.</returns>
        public bool MoveNext() {
            var newIndex = _index + 1;
            var length = _array.Length;

            // unsigned math is used to prevent false positive if index + 1 overflows.
            if ((uint)newIndex <= (uint)length) {
                _index = newIndex;
                return (uint)newIndex < (uint)length;
            }

            return false;
        }

        /// <summary>
        /// Resets enumeration to the start of the array.
        /// </summary>
        void IEnumerator.Reset() {
            _index = -1;
        }

        /// <summary>
        /// Disposes this enumerator.
        /// </summary>
        /// <remarks>
        /// Currently has no action.
        /// </remarks>
        public void Dispose() {
            // we do not have any native or disposable resources.
            // nothing to do here.
        }

        /// <summary>
        /// Creates an enumerator for the specified array.
        /// </summary>
        internal static IEnumerator<T> Create(T[] array) {
            if (array.Length != 0) {
                return new EnumeratorObject(array);
            }
            else {
                return s_EmptyEnumerator;
            }
        }
    }
}
