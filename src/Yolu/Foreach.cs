using System.Collections;

namespace Yolu;

/// <summary>
/// Provides a foreach loop with a specified range using the <see cref="Range" /> type.
/// </summary>
/// <example>
/// This example shows how to use the for loop with the <see cref="Range" /> type.
/// <code>
/// foreach(var i in 0..^1000) {
///     // Your code here.
/// }
/// </code>
/// </example>
public static class Foreach {
    /// <summary>
    /// Provides a foreach loop with a specified range using the <see cref="Range" /> type.
    /// </summary>
    /// <example>
    /// This example shows how to use the GetEnumerator method.
    /// <code>
    /// foreach(var i in 0..^1000) {
    ///     // Your code here.
    /// }
    /// </code>
    /// </example>
    /// <param name="range">The range to be used in the foreach loop.</param>
    /// <returns>A <see cref="RangeEnumerator" /> that can be used to iterate through the specified range.</returns>
    public static RangeEnumerator GetEnumerator(this Range range) {
        return new(range);
    }

    public struct RangeEnumerator : IEnumerator<int> {
        private readonly int _max;
        private readonly int _step;

        /// <summary>
        /// Gets the current position in the range.
        /// </summary>
        public int Current { get; private set; }

        /// <summary>
        /// Gets the current item in the range.
        /// </summary>
        object IEnumerator.Current => Current;

        /// <summary>
        /// Advances the enumerator to the next item in the range.
        /// </summary>
        /// <returns>true if the enumerator was successfully advanced to the next item; false if the enumerator has passed the end of the range.</returns>
        public bool MoveNext() {
            if (Current != _max) {
                Current += _step;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose() {
            // noop
        }

        /// <summary>
        /// Sets the enumerator to its initial position, which is before the first item in the range.
        /// </summary>
        /// <exception cref="NotSupportedException">The <see cref="Reset"/> method is not supported.</exception>
        public void Reset() {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RangeEnumerator" /> struct.
        /// </summary>
        /// <param name="range">The range to be enumerated.</param>
        public RangeEnumerator(Range range) {
            var step = range.End.Value < range.Start.Value ? -1 : 1;
            Current = range.Start.Value - (range.Start.IsFromEnd ? 0 : step);
            _max = range.End.Value - (range.End.IsFromEnd ? step : 0);
            _step = step;
        }
    }
}