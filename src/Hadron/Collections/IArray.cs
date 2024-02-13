using System.Runtime.CompilerServices;

namespace Hadron.Collections;

/// <summary>
/// Represents an array-like collection that provides read-only access to its elements.
/// </summary>
/// <typeparam name="T">The type of elements in the array.</typeparam>
[CollectionBuilder(typeof(ArrayClassBuilder), nameof(ArrayClassBuilder.CreateIArray))]
public interface IArray<out T> : IReadOnlyList<T> {

}
