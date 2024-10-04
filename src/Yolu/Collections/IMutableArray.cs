using System.Runtime.CompilerServices;

namespace Yolu.Collections;

[CollectionBuilder(typeof(MutableArrayBuilder), nameof(MutableArrayBuilder.CreateIMutableArray))]
public interface IMutableArray<T> : IList<T>, IArray<T>;


