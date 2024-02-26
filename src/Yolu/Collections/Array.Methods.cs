using Yolu.Collections.Internal;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace Yolu.Collections;

public partial class Array<T> {
    /// <summary>
    /// Searches the array for the specified item.
    /// </summary>
    /// <param name="item">The item to search for.</param>
    /// <returns>The 0-based index into the array where the item was found; or -1 if it could not be found.</returns>
    [Pure]
    public int IndexOf(T item) {
        var self = this;
        return self.IndexOf(item, 0, self.Length, EqualityComparer<T>.Default);
    }

    /// <summary>
    /// Searches for the specified object and returns the zero-based index of the
    /// first occurrence within the range of elements in the <see cref="ImmutableList{T}"/>
    /// that starts at the specified index and contains the specified number of elements.
    /// </summary>
    /// <param name="item">
    /// The object to locate in the <see cref="ImmutableList{T}"/>. The value
    /// can be null for reference types.
    /// </param>
    /// <param name="equalityComparer">The equality comparer to use for testing the match of two elements.</param>
    /// <returns>
    /// The zero-based index of the first occurrence of <paramref name="item"/> within the entire
    /// <see cref="ImmutableList{T}"/>, if found; otherwise, -1.
    /// </returns>
    [Pure]
    internal int IndexOf(T item, IEqualityComparer<T> equalityComparer) {
        return this.IndexOf(item, 0, this.Count, equalityComparer);
    }


    /// <summary>
    /// Searches the array for the specified item.
    /// </summary>
    /// <param name="item">The item to search for.</param>
    /// <param name="startIndex">The index at which to begin the search.</param>
    /// <param name="equalityComparer">The equality comparer to use in the search.</param>
    /// <returns>The 0-based index into the array where the item was found; or -1 if it could not be found.</returns>
    [Pure]
    public int IndexOf(T item, int startIndex, IEqualityComparer<T> equalityComparer) {
        var self = this;
        return self.IndexOf(item, startIndex, self.Length - startIndex, equalityComparer);
    }

    /// <summary>
    /// Searches the array for the specified item.
    /// </summary>
    /// <param name="item">The item to search for.</param>
    /// <param name="startIndex">The index at which to begin the search.</param>
    /// <returns>The 0-based index into the array where the item was found; or -1 if it could not be found.</returns>
    [Pure]
    public int IndexOf(T item, int startIndex) {
        var self = this;
        return self.IndexOf(item, startIndex, self.Length - startIndex, EqualityComparer<T>.Default);
    }

    /// <summary>
    /// Searches the array for the specified item.
    /// </summary>
    /// <param name="item">The item to search for.</param>
    /// <param name="startIndex">The index at which to begin the search.</param>
    /// <param name="count">The number of elements to search.</param>
    /// <returns>The 0-based index into the array where the item was found; or -1 if it could not be found.</returns>
    [Pure]
    public int IndexOf(T item, int startIndex, int count) {
        return this.IndexOf(item, startIndex, count, EqualityComparer<T>.Default);
    }

    /// <summary>
    /// Searches the array for the specified item.
    /// </summary>
    /// <param name="item">The item to search for.</param>
    /// <param name="startIndex">The index at which to begin the search.</param>
    /// <param name="count">The number of elements to search.</param>
    /// <param name="equalityComparer">The equality comparer to use in the search.</param>
    /// <returns>The 0-based index into the array where the item was found; or -1 if it could not be found.</returns>
    [Pure]
    public int IndexOf(T item, int startIndex, int count, IEqualityComparer<T> equalityComparer) {
        var self = this;
        self.ThrowNullRefIfNotInitialized();
        Requires.NotNull(equalityComparer, "equalityComparer");

        if (count == 0 && startIndex == 0) {
            return -1;
        }

        Requires.Range(startIndex >= 0 && startIndex < self.Length, "startIndex");
        Requires.Range(count >= 0 && startIndex + count <= self.Length, "count");

        if (equalityComparer == EqualityComparer<T>.Default) {
            return Array.IndexOf(self._array, item, startIndex, count);
        }
        else {
            for (var i = startIndex; i < startIndex + count; i++) {
                if (equalityComparer.Equals(self._array[i], item)) {
                    return i;
                }
            }

            return -1;
        }
    }

    /// <summary>
    /// Searches the array for the specified item in reverse.
    /// </summary>
    /// <param name="item">The item to search for.</param>
    /// <returns>The 0-based index into the array where the item was found; or -1 if it could not be found.</returns>
    [Pure]
    public int LastIndexOf(T item) {
        var self = this;
        if (self.Length == 0) {
            return -1;
        }

        return self.LastIndexOf(item, self.Length - 1, self.Length, EqualityComparer<T>.Default);
    }

    /// <summary>
    /// Searches the array for the specified item in reverse.
    /// </summary>
    /// <param name="item">The item to search for.</param>
    /// <param name="startIndex">The index at which to begin the search.</param>
    /// <returns>The 0-based index into the array where the item was found; or -1 if it could not be found.</returns>
    [Pure]
    public int LastIndexOf(T item, int startIndex) {
        var self = this;
        if (self.Length == 0 && startIndex == 0) {
            return -1;
        }

        return self.LastIndexOf(item, startIndex, startIndex + 1, EqualityComparer<T>.Default);
    }

    /// <summary>
    /// Searches the array for the specified item in reverse.
    /// </summary>
    /// <param name="item">The item to search for.</param>
    /// <param name="startIndex">The index at which to begin the search.</param>
    /// <param name="count">The number of elements to search.</param>
    /// <returns>The 0-based index into the array where the item was found; or -1 if it could not be found.</returns>
    [Pure]
    public int LastIndexOf(T item, int startIndex, int count) {
        return this.LastIndexOf(item, startIndex, count, EqualityComparer<T>.Default);
    }

    /// <summary>
    /// Searches the array for the specified item in reverse.
    /// </summary>
    /// <param name="item">The item to search for.</param>
    /// <param name="startIndex">The index at which to begin the search.</param>
    /// <param name="count">The number of elements to search.</param>
    /// <param name="equalityComparer">The equality comparer to use in the search.</param>
    /// <returns>The 0-based index into the array where the item was found; or -1 if it could not be found.</returns>
    [Pure]
    public int LastIndexOf(T item, int startIndex, int count, IEqualityComparer<T> equalityComparer) {
        var self = this;
        self.ThrowNullRefIfNotInitialized();
        Requires.NotNull(equalityComparer, "equalityComparer");

        if (startIndex == 0 && count == 0) {
            return -1;
        }

        Requires.Range(startIndex >= 0 && startIndex < self.Length, "startIndex");
        Requires.Range(count >= 0 && startIndex - count + 1 >= 0, "count");

        if (equalityComparer == EqualityComparer<T>.Default) {
            return Array.LastIndexOf(self._array, item, startIndex, count);
        }
        else {
            for (var i = startIndex; i >= startIndex - count + 1; i--) {
                if (equalityComparer.Equals(item, self._array[i])) {
                    return i;
                }
            }

            return -1;
        }
    }

    /// <summary>
    /// Determines whether the specified item exists in the array.
    /// </summary>
    /// <param name="item">The item to search for.</param>
    /// <returns><c>true</c> if an equal value was found in the array; <c>false</c> otherwise.</returns>
    [Pure]
    public bool Contains(T item) {
        return this.IndexOf(item) >= 0;
    }

    /// <summary>
    /// Copies the contents of this array to the specified array.
    /// </summary>
    /// <param name="destination">The array to copy to.</param>
    [Pure]
    public void CopyTo(T[] destination) {
        var self = this;
        self.ThrowNullRefIfNotInitialized();
        Array.Copy(self._array, 0, destination, 0, self.Length);
    }

    /// <summary>
    /// Copies the contents of this array to the specified array.
    /// </summary>
    /// <param name="destination">The array to copy to.</param>
    /// <param name="destinationIndex">The index into the destination array to which the first copied element is written.</param>
    [Pure]
    public void CopyTo(T[] destination, int destinationIndex) {
        var self = this;
        self.ThrowNullRefIfNotInitialized();
        Array.Copy(self._array, 0, destination, destinationIndex, self.Length);
    }

    /// <summary>
    /// Copies the contents of this array to the specified array.
    /// </summary>
    /// <param name="sourceIndex">The index into this collection of the first element to copy.</param>
    /// <param name="destination">The array to copy to.</param>
    /// <param name="destinationIndex">The index into the destination array to which the first copied element is written.</param>
    /// <param name="length">The number of elements to copy.</param>
    [Pure]
    public void CopyTo(int sourceIndex, T[] destination, int destinationIndex, int length) {
        var self = this;
        self.ThrowNullRefIfNotInitialized();
        Array.Copy(self._array, sourceIndex, destination, destinationIndex, length);
    }

    /// <summary>
    /// Returns a new array with the specified value inserted at the specified position.
    /// </summary>
    /// <param name="index">The 0-based index into the array at which the new item should be added.</param>
    /// <param name="item">The item to insert at the start of the array.</param>
    /// <returns>A new array.</returns>
    [Pure]
    public Array<T> Insert(int index, T item) {
        var self = this;
        self.ThrowNullRefIfNotInitialized();
        Requires.Range(index >= 0 && index <= self.Length, "index");

        if (self.Length == 0) {
            return [item];
        }

        var tmp = new T[self.Length + 1];
        Array.Copy(self._array, 0, tmp, 0, index);
        tmp[index] = item;
        Array.Copy(self._array, index, tmp, index + 1, self.Length - index);
        return new Array<T>(tmp);
    }

    /// <summary>
    /// Inserts the specified values at the specified index.
    /// </summary>
    /// <param name="index">The index at which to insert the value.</param>
    /// <param name="items">The elements to insert.</param>
    /// <returns>The new immutable collection.</returns>
    [Pure]
    public Array<T> InsertRange(int index, IEnumerable<T> items) {
        var self = this;
        self.ThrowNullRefIfNotInitialized();
        Requires.Range(index >= 0 && index <= self.Length, "index");
        Requires.NotNull(items, "items");

        if (self.Length == 0) {
            return [.. items];
        }

        var count = EnumerableExtension.GetCount(ref items);
        if (count == 0) {
            return self;
        }

        var tmp = new T[self.Length + count];
        Array.Copy(self._array, 0, tmp, 0, index);
        var sequenceIndex = index;
        foreach (var item in items) {
            tmp[sequenceIndex++] = item;
        }

        Array.Copy(self._array, index, tmp, index + count, self.Length - index);
        return new Array<T>(tmp);
    }

    /// <summary>
    /// Inserts the specified values at the specified index.
    /// </summary>
    /// <param name="index">The index at which to insert the value.</param>
    /// <param name="items">The elements to insert.</param>
    /// <returns>The new immutable collection.</returns>
    [Pure]
    public Array<T> InsertRange(int index, Array<T> items) {
        var self = this;
        self.ThrowNullRefIfNotInitialized();
        ThrowNullRefIfNotInitialized(items);
        Requires.Range(index >= 0 && index <= self.Length, "index");

        if (self.IsEmpty) {
            return new Array<T>(items._array);
        }
        else if (items.IsEmpty) {
            return new Array<T>(self._array);
        }

        return self.InsertRange(index, items._array);
    }

    /// <summary>
    /// Returns a new array with the specified value inserted at the end.
    /// </summary>
    /// <param name="item">The item to insert at the end of the array.</param>
    /// <returns>A new array.</returns>
    [Pure]
    public Array<T> Add(T item) {
        var self = this;
        if (self.Length == 0) {
            return [item];
        }

        return self.Insert(self.Length, item);
    }

    /// <summary>
    /// Adds the specified values to this list.
    /// </summary>
    /// <param name="items">The values to add.</param>
    /// <returns>A new list with the elements added.</returns>
    [Pure]
    public Array<T> AddRange(IEnumerable<T> items) {
        var self = this;
        return self.InsertRange(self.Length, items);
    }

    /// <summary>
    /// Adds the specified values to this list.
    /// </summary>
    /// <param name="items">The values to add.</param>
    /// <returns>A new list with the elements added.</returns>
    [Pure]
    public Array<T> AddRange(Array<T> items) {
        var self = this;
        self.ThrowNullRefIfNotInitialized();
        ThrowNullRefIfNotInitialized(items);
        if (self.IsEmpty) {
            // Be sure what we return is marked as initialized.
            return new Array<T>(items._array);
        }
        else if (items.IsEmpty) {
            return self;
        }

        return self.AddRange(items._array);
    }

    /// <summary>
    /// Returns an array with the item at the specified position replaced.
    /// </summary>
    /// <param name="index">The index of the item to replace.</param>
    /// <param name="item">The new item.</param>
    /// <returns>The new array.</returns>
    [Pure]
    public Array<T> SetItem(int index, T item) {
        var self = this;
        Requires.Range(index >= 0 && index < self.Length, "index");

        var tmp = new T[self.Length];
        Array.Copy(self._array, tmp, self.Length);
        tmp[index] = item;
        return new Array<T>(tmp);
    }

    /// <summary>
    /// Replaces the first equal element in the list with the specified element.
    /// </summary>
    /// <param name="oldValue">The element to replace.</param>
    /// <param name="newValue">The element to replace the old element with.</param>
    /// <returns>The new list -- even if the value being replaced is equal to the new value for that position.</returns>
    /// <exception cref="ArgumentException">Thrown when the old value does not exist in the list.</exception>
    [Pure]
    public Array<T> Replace(T oldValue, T newValue) {
        return this.Replace(oldValue, newValue, EqualityComparer<T>.Default);
    }

    /// <summary>
    /// Replaces the first equal element in the list with the specified element.
    /// </summary>
    /// <param name="oldValue">The element to replace.</param>
    /// <param name="newValue">The element to replace the old element with.</param>
    /// <param name="equalityComparer">
    /// The equality comparer to use in the search.
    /// </param>
    /// <returns>The new list -- even if the value being replaced is equal to the new value for that position.</returns>
    /// <exception cref="ArgumentException">Thrown when the old value does not exist in the list.</exception>
    [Pure]
    public Array<T> Replace(T oldValue, T newValue, IEqualityComparer<T> equalityComparer) {
        var self = this;
        var index = self.IndexOf(oldValue, equalityComparer);
        if (index < 0) {
            throw new ArgumentException("Cannot find the old value", "oldValue");
        }

        return self.SetItem(index, newValue);
    }

    /// <summary>
    /// Returns an array with the first occurrence of the specified element removed from the array.
    /// If no match is found, the current array is returned.
    /// </summary>
    /// <param name="item">The item to remove.</param>
    /// <returns>The new array.</returns>
    [Pure]
    public Array<T> Remove(T item) {
        return this.Remove(item, EqualityComparer<T>.Default);
    }

    /// <summary>
    /// Returns an array with the first occurrence of the specified element removed from the array.
    /// If no match is found, the current array is returned.
    /// </summary>
    /// <param name="item">The item to remove.</param>
    /// <param name="equalityComparer">
    /// The equality comparer to use in the search.
    /// </param>
    /// <returns>The new array.</returns>
    [Pure]
    public Array<T> Remove(T item, IEqualityComparer<T> equalityComparer) {
        var self = this;
        self.ThrowNullRefIfNotInitialized();
        var index = self.IndexOf(item, equalityComparer);
        return index < 0
            ? new Array<T>(self._array)
            : self.RemoveAt(index);
    }

    /// <summary>
    /// Returns an array with the element at the specified position removed.
    /// </summary>
    /// <param name="index">The 0-based index into the array for the element to omit from the returned array.</param>
    /// <returns>The new array.</returns>
    [Pure]
    public Array<T> RemoveAt(int index) {
        return this.RemoveRange(index, 1);
    }

    /// <summary>
    /// Returns an array with the elements at the specified position removed.
    /// </summary>
    /// <param name="index">The 0-based index into the array for the element to omit from the returned array.</param>
    /// <param name="length">The number of elements to remove.</param>
    /// <returns>The new array.</returns>
    [Pure]
    public Array<T> RemoveRange(int index, int length) {
        var self = this;
        Requires.Range(index >= 0 && index < self.Length, "index");
        Requires.Range(length >= 0 && index + length <= self.Length, "length");

        var tmp = new T[self.Length - length];
        Array.Copy(self._array, 0, tmp, 0, index);
        Array.Copy(self._array, index + length, tmp, index, self.Length - index - length);
        return new Array<T>(tmp);
    }

    /// <summary>
    /// Removes the specified values from this list.
    /// </summary>
    /// <param name="items">The items to remove if matches are found in this list.</param>
    /// <returns>
    /// A new list with the elements removed.
    /// </returns>
    [Pure]
    public Array<T> RemoveRange(IEnumerable<T> items) {
        return this.RemoveRange(items, EqualityComparer<T>.Default);
    }

    /// <summary>
    /// Removes the specified values from this list.
    /// </summary>
    /// <param name="items">The items to remove if matches are found in this list.</param>
    /// <param name="equalityComparer">
    /// The equality comparer to use in the search.
    /// </param>
    /// <returns>
    /// A new list with the elements removed.
    /// </returns>
    [Pure]
    public Array<T> RemoveRange(IEnumerable<T> items, IEqualityComparer<T> equalityComparer) {
        var self = this;
        self.ThrowNullRefIfNotInitialized();
        Requires.NotNull(items, "items");
        Requires.NotNull(equalityComparer, "equalityComparer");

        var indexesToRemove = new SortedSet<int>();
        foreach (var item in items) {
            var index = self.IndexOf(item, equalityComparer);
            while (index >= 0 && !indexesToRemove.Add(index) && index + 1 < self.Length) {
                // This is a duplicate of one we've found. Try hard to find another instance in the list to remove.
                index = self.IndexOf(item, index + 1, equalityComparer);
            }
        }

        return self.RemoveAtRange(indexesToRemove);
    }

    /// <summary>
    /// Removes the specified values from this list.
    /// </summary>
    /// <param name="items">The items to remove if matches are found in this list.</param>
    /// <returns>
    /// A new list with the elements removed.
    /// </returns>
    [Pure]
    public Array<T> RemoveRange(Array<T> items) {
        return this.RemoveRange(items._array);
    }

    /// <summary>
    /// Removes the specified values from this list.
    /// </summary>
    /// <param name="items">The items to remove if matches are found in this list.</param>
    /// <param name="equalityComparer">
    /// The equality comparer to use in the search.
    /// </param>
    /// <returns>
    /// A new list with the elements removed.
    /// </returns>
    [Pure]
    public Array<T> RemoveRange(Array<T> items, IEqualityComparer<T> equalityComparer) {
        return this.RemoveRange(items._array, equalityComparer);
    }

    /// <summary>
    /// Removes all the elements that match the conditions defined by the specified
    /// predicate.
    /// </summary>
    /// <param name="match">
    /// The <see cref="Predicate{T}"/> delegate that defines the conditions of the elements
    /// to remove.
    /// </param>
    /// <returns>
    /// The new list.
    /// </returns>
    [Pure]
    public Array<T> RemoveAll(Predicate<T> match) {
        var self = this;
        self.ThrowNullRefIfNotInitialized();
        Requires.NotNull(match, "match");

        if (self.IsEmpty) {
            return new Array<T>(self._array);
        }

        List<int>? removeIndexes = null;
        for (var i = 0; i < self._array.Length; i++) {
            if (match(self._array[i])) {
                removeIndexes ??= [];

                removeIndexes.Add(i);
            }
        }

        return removeIndexes != null ?
            self.RemoveAtRange(removeIndexes) :
            self;
    }

    /// <summary>
    /// Returns a sorted instance of this array.
    /// </summary>
    [Pure]
    public Array<T> Sort() {
        var self = this;
        return self.Sort(0, self.Length, Comparer<T>.Default);
    }

    /// <summary>
    /// Returns a sorted instance of this array.
    /// </summary>
    /// <param name="comparer">The comparer to use in sorting. If <c>null</c>, the default comparer is used.</param>
    [Pure]
    public Array<T> Sort(IComparer<T> comparer) {
        var self = this;
        return self.Sort(0, self.Length, comparer);
    }

    /// <summary>
    /// Returns a sorted instance of this array.
    /// </summary>
    /// <param name="index">The index of the first element to consider in the sort.</param>
    /// <param name="count">The number of elements to include in the sort.</param>
    /// <param name="comparer">The comparer to use in sorting. If <c>null</c>, the default comparer is used.</param>
    [Pure]
    public Array<T> Sort(int index, int count, IComparer<T> comparer) {
        var self = this;
        self.ThrowNullRefIfNotInitialized();
        Requires.Range(index >= 0, "index");
        Requires.Range(count >= 0 && index + count <= self.Length, "count");

        comparer ??= Comparer<T>.Default;

        // 0 and 1 element arrays don't need to be sorted.
        if (count > 1) {
            // Avoid copying the entire array when the array is already sorted.
            var outOfOrder = false;
            for (var i = index + 1; i < index + count; i++) {
                if (comparer.Compare(self._array[i - 1], self._array[i]) > 0) {
                    outOfOrder = true;
                    break;
                }
            }

            if (outOfOrder) {
                var tmp = new T[self.Length];
                Array.Copy(self._array, tmp, self.Length);
                Array.Sort(tmp, index, count, comparer);
                return new Array<T>(tmp);
            }
        }

        return new Array<T>(self._array);
    }

    /// <summary>
    /// Returns a hash code for this instance.
    /// </summary>
    /// <returns>
    /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
    /// </returns>
    [Pure]
    public override int GetHashCode() {
        var self = this;
        return self._array == null ? 0 : self._array.GetHashCode();
    }

    /// <summary>
    /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
    /// </summary>
    /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
    /// <returns>
    ///   <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
    /// </returns>
    [Pure]
    public override bool Equals(object? obj) {
        if (obj is Array<T> array) {
            return this.Equals(array);
        }

        return false;
    }

    /// <summary>
    /// Indicates whether the current object is equal to another object of the same type.
    /// </summary>
    /// <param name="other">An object to compare with this object.</param>
    /// <returns>
    /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
    /// </returns>
    [Pure]
    public bool Equals(Array<T>? other) {
        return this._array == other?._array;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Array{T}"/> struct based on the contents
    /// of an existing instance, allowing a covariant static cast to efficiently reuse the existing array.
    /// </summary>
    /// <param name="items">The array to initialize the array with. No copy is made.</param>
    /// <remarks>
    /// Covariant upcasts from this method may be reversed by calling the
    /// <see cref="Array{T}.As{TOther}"/>  or <see cref="Array{T}.CastArray{TOther}"/>method.
    /// </remarks>
    [Pure]
    public static Array<T> CastUp<TDerived>(Array<TDerived> items)
        where TDerived : class, T {
        return new Array<T>(items._array);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Array{T}"/> struct by casting the underlying
    /// array to an array of type <typeparam name="TOther"/>.
    /// </summary>
    /// <exception cref="InvalidCastException">Thrown if the cast is illegal.</exception>
    [Pure]
    public Array<TOther> CastArray<TOther>() where TOther : class {
        return new Array<TOther>((TOther[])(object)_array);
    }

    /// <summary>
    /// Creates an immutable array for this array, cast to a different element type.
    /// </summary>
    /// <typeparam name="TOther">The type of array element to return.</typeparam>
    /// <returns>
    /// A struct typed for the base element type. If the cast fails, an instance
    /// is returned whose <see cref="IsDefault"/> property returns <c>true</c>.
    /// </returns>
    /// <remarks>
    /// Arrays of derived elements types can be cast to arrays of base element types
    /// without reallocating the array.
    /// These upcasts can be reversed via this same method, casting an array of base
    /// element types to their derived types. However, downcasting is only successful
    /// when it reverses a prior upcasting operation.
    /// </remarks>
    [Pure]
    public Array<TOther> As<TOther>() where TOther : class {
        return new Array<TOther>(_array as TOther[]);
    }

    /// <summary>
    /// Filters the elements of this array to those assignable to the specified type.
    /// </summary>
    /// <typeparam name="TResult">The type to filter the elements of the sequence on.</typeparam>
    /// <returns>
    /// An <see cref="IEnumerable{T}"/> that contains elements from
    /// the input sequence of type <typeparamref name="TResult"/>.
    /// </returns>
    [Pure]
    public IEnumerable<TResult> OfType<TResult>() {
        var self = this;
        if (self._array == null || self._array.Length == 0) {
            return Enumerable.Empty<TResult>();
        }

        return self._array.OfType<TResult>();
    }

    /// <summary>
    /// Throws an <see cref="InvalidOperationException"/> if the <see cref="array"/> field is null, i.e. the
    /// <see cref="IsDefault"/> property returns true.  The
    /// <see cref="InvalidOperationException"/> message specifies that the operation cannot be performed
    /// on a default instance of <see cref="Array{T}"/>.
    /// 
    /// This is intended for explicitly implemented interface method and property implementations.
    /// </summary>
    private void ThrowInvalidOperationIfNotInitialized() {
        if (this.IsDefault) {
            throw new InvalidOperationException("InvalidOperationOnDefaultArray");
        }
    }

    /// <summary>
    /// Returns an array with items at the specified indexes removed.
    /// </summary>
    /// <param name="indexesToRemove">A **sorted set** of indexes to elements that should be omitted from the returned array.</param>
    /// <returns>The new array.</returns>
    private Array<T> RemoveAtRange(ICollection<int> indexesToRemove) {
        var self = this;
        self.ThrowNullRefIfNotInitialized();
        _ = indexesToRemove ?? throw new ArgumentNullException(nameof(indexesToRemove));

        if (indexesToRemove.Count == 0) {
            // Be sure to return a !IsDefault instance.
            return new Array<T>(self._array);
        }

        var newArray = new T[self.Length - indexesToRemove.Count];
        var copied = 0;
        var removed = 0;
        var lastIndexRemoved = -1;
        foreach (var indexToRemove in indexesToRemove) {
            var copyLength = lastIndexRemoved == -1 ? indexToRemove : (indexToRemove - lastIndexRemoved - 1);
            Debug.Assert(indexToRemove > lastIndexRemoved); // We require that the input be a sorted set.
            Array.Copy(self._array, copied + removed, newArray, copied, copyLength);
            removed++;
            copied += copyLength;
            lastIndexRemoved = indexToRemove;
        }

        Array.Copy(self._array, copied + removed, newArray, copied, self.Length - (copied + removed));

        return new Array<T>(newArray);
    }
}
