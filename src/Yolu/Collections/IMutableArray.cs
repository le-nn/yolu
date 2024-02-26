using System.Runtime.CompilerServices;

namespace Yolu.Collections;

[CollectionBuilder(typeof(MutableArrayClassBuilder), nameof(MutableArrayClassBuilder.CreateIMutableArray))]
public interface IMutableArray<T> : IList<T>, IArray<T>;


