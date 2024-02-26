using System.Collections;

namespace Yolu.Collections.Internal;
internal static class EnumerableExtension {
    /// <summary>
    /// Gets the number of elements in the specified sequence,
    /// while guaranteeing that the sequence is only enumerated once
    /// in total by this method and the caller.
    /// </summary>
    /// <typeparam name="T">The type of element in the collection.</typeparam>
    /// <param name="sequence">The sequence.</param>
    /// <returns>The number of elements in the sequence.</returns>
    internal static int GetCount<T>(ref IEnumerable<T> sequence) {
        if (!sequence.TryGetCount(out var count)) {
            // We cannot predict the length of the sequence. We must walk the entire sequence
            // to find the count. But avoid our caller also having to enumerate by capturing
            // the enumeration in a snapshot and passing that back to the caller.
            var list = sequence.ToList();
            count = list.Count;
            sequence = list;
        }

        return count;
    }

    /// <summary>
    /// Tries to divine the number of elements in a sequence without actually enumerating each element.
    /// </summary>
    /// <typeparam name="T">The type of elements in the sequence.</typeparam>
    /// <param name="sequence">The enumerable source.</param>
    /// <param name="count">Receives the number of elements in the enumeration, if it could be determined.</param>
    /// <returns><c>true</c> if the count could be determined; <c>false</c> otherwise.</returns>
    internal static bool TryGetCount<T>(this IEnumerable<T> sequence, out int count) {
        return TryGetCount<T>((IEnumerable)sequence, out count);
    }

    /// <summary>
    /// Tries to divine the number of elements in a sequence without actually enumerating each element.
    /// </summary>
    /// <typeparam name="T">The type of elements in the sequence.</typeparam>
    /// <param name="sequence">The enumerable source.</param>
    /// <param name="count">Receives the number of elements in the enumeration, if it could be determined.</param>
    /// <returns><c>true</c> if the count could be determined; <c>false</c> otherwise.</returns>
    internal static bool TryGetCount<T>(this IEnumerable sequence, out int count) {
        if (sequence is ICollection collection) {
            count = collection.Count;
            return true;
        }

        if (sequence is ICollection<T> collectionOfT) {
            count = collectionOfT.Count;
            return true;
        }

        if (sequence is IReadOnlyCollection<T> readOnlyCollection) {
            count = readOnlyCollection.Count;
            return true;
        }

        count = 0;
        return false;
    }

    /// <summary>Gets an enumerator singleton for an empty collection.</summary>
    internal static IEnumerator<T> GetEmptyEnumerator<T>() =>
        ((IEnumerable<T>)[]).GetEnumerator();

    /// <summary>Converts an enumerable to an array using the same logic as List{T}.</summary>
    /// <param name="source">The enumerable to convert.</param>
    /// <param name="length">The number of items stored in the resulting array, 0-indexed.</param>
    /// <returns>
    /// The resulting array.  The length of the array may be greater than <paramref name="length"/>,
    /// which is the actual number of elements in the array.
    /// </returns>
    internal static T[] ToArray<T>(IEnumerable<T> source, out int length) {
        if (source is ICollection<T> ic) {
            var count = ic.Count;
            if (count != 0) {
                // Allocate an array of the desired size, then copy the elements into it. Note that this has the same
                // issue regarding concurrency as other existing collections like List<T>. If the collection size
                // concurrently changes between the array allocation and the CopyTo, we could end up either getting an
                // exception from overrunning the array (if the size went up) or we could end up not filling as many
                // items as 'count' suggests (if the size went down).  This is only an issue for concurrent collections
                // that implement ICollection<T>, which as of .NET 4.6 is just ConcurrentDictionary<TKey, TValue>.
                var arr = new T[count];

                ic.CopyTo(arr, 0);
                length = count;
                return arr;
            }
        }
        else {
            using var en = source.GetEnumerator();
            if (en.MoveNext()) {
                const int DefaultCapacity = 4;
                var arr = new T[DefaultCapacity];
                arr[0] = en.Current;
                var count = 1;

                while (en.MoveNext()) {
                    if (count == arr.Length) {
                        // This is the same growth logic as in List<T>:
                        // If the array is currently empty, we make it a default size.  Otherwise, we attempt to
                        // double the size of the array.  Doubling will overflow once the size of the array reaches
                        // 2^30, since doubling to 2^31 is 1 larger than Int32.MaxValue.  In that case, we instead
                        // constrain the length to be Array.MaxLength (this overflow check works because of the
                        // cast to uint).
                        var newLength = count << 1;
                        if ((uint)newLength > Array.MaxLength) {
                            newLength = Array.MaxLength <= count ? count + 1 : Array.MaxLength;
                        }

                        Array.Resize(ref arr, newLength);
                    }

                    arr[count++] = en.Current;
                }

                length = count;
                return arr;
            }
        }

        length = 0;
        return [];
    }
}
