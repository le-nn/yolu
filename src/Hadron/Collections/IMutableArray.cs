using System.Runtime.CompilerServices;

namespace Hadron.Collections;

[CollectionBuilder(typeof(MutableArrayClassBuilder), nameof(MutableArrayClassBuilder.CreateIMutableArray))]
public interface IMutableArray<T> : IList<T>, IArray<T>;


