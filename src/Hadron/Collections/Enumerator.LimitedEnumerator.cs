﻿using System.Collections;
using System.Runtime.InteropServices;

namespace Hadron.Collections;

public static partial class Enumerator {
    /// <summary>
    /// Wrapped for the enumerator which is limited by count.
    /// </summary>
    /// <typeparam name="T">The type of elements returned by enumerator.</typeparam>
    [StructLayout(LayoutKind.Auto)]
    public struct LimitedEnumerator<T> : IEnumerator<T> {
        private readonly IEnumerator<T> enumerator;
        private readonly bool disposeEnumerator;
        private int count;

        internal LimitedEnumerator(IEnumerator<T> enumerator, int limit, bool leaveOpen) {
            this.enumerator = enumerator;
            disposeEnumerator = !leaveOpen;
            count = limit;
        }

        /// <summary>
        /// Advances the enumerator to the next element.
        /// </summary>
        /// <returns><see langword="true"/> if the enumerator was successfully advanced to the next element; <see langword="false"/> if
        /// the enumerator has passed the end of the collection.</returns>
        public bool MoveNext() => count-- > 0 && enumerator.MoveNext();

        /// <summary>
        /// Gets the element in the collection at the current position of the enumerator.
        /// </summary>
        public readonly T Current => enumerator.Current;

        /// <inheritdoc/>
        readonly object? IEnumerator.Current => Current;

        /// <summary>
        /// Sets the enumerator to its initial position.
        /// </summary>
        public readonly void Reset() => enumerator?.Reset();

        /// <summary>
        /// Releases all resources associated with this enumerator.
        /// </summary>
        public void Dispose() {
            if (disposeEnumerator)
                enumerator?.Dispose();
            this = default;
        }
    }
}