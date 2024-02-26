using System.Collections;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;

namespace Yolu.Collections;

public partial class CollectionUtils {
    /// <summary>
    /// Represents a wrapped for method <see cref="IProducerConsumerCollection{T}.TryTake(out T)"/>
    /// in the form of enumerable collection.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    [StructLayout(LayoutKind.Auto)]
    public readonly struct ConsumingEnumerable<T> : IEnumerable<T> {
        /// <summary>
        /// Represents consumer enumerator.
        /// </summary>
        [StructLayout(LayoutKind.Auto)]
        public struct Enumerator : IEnumerator<T> {
            private readonly IProducerConsumerCollection<T>? _collection;

            private T? _current;

            internal Enumerator(IProducerConsumerCollection<T>? collection) {
                this._collection = collection;
                _current = default!;
            }

            /// <summary>
            /// Gets consumed item from the underlying collection.
            /// </summary>
            public readonly T Current => _current!;

            /// <inheritdoc />
            readonly object? IEnumerator.Current => Current;

            /// <summary>
            /// Consumes the item from the underlying collection.
            /// </summary>
            /// <returns><see langword="true"/> if the item has been consumed successfully; <see langword="false"/> if underlying collection is empty.</returns>
            public bool MoveNext() => _collection?.TryTake(out _current) ?? false;

            /// <inheritdoc />
            readonly void IEnumerator.Reset() => throw new NotSupportedException();

            /// <inheritdoc />
            void IDisposable.Dispose() => this = default;
        }

        private readonly IProducerConsumerCollection<T>? _collection;

        internal ConsumingEnumerable(IProducerConsumerCollection<T> collection)
            => this._collection = collection;

        /// <summary>
        /// Gets consumer enumerator.
        /// </summary>
        /// <returns>The enumerator wrapping method <see cref="IProducerConsumerCollection{T}.TryTake(out T)"/>.</returns>
        public Enumerator GetEnumerator() => new(_collection);

        /// <inheritdoc />
        IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    /// <summary>
    /// Gets consumer of thread-safe concurrent collection.
    /// </summary>
    /// <param name="collection">The concurrent collection.</param>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <returns>The consumer in the form of enumerable collection.</returns>
    public static ConsumingEnumerable<T> GetConsumer<T>(this IProducerConsumerCollection<T> collection)
        => new(collection ?? throw new ArgumentNullException(nameof(collection)));
}
