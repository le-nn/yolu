﻿using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Hadron.Collections;

/// <summary>
/// Represents lazily converted read-only collection.
/// </summary>
/// <typeparam name="TInput">Type of items in the source collection.</typeparam>
/// <typeparam name="TOutput">Type of items in the converted collection.</typeparam>
/// <remarks>
/// Initializes a new lazily converted view.
/// </remarks>
/// <param name="collection">Read-only collection to convert.</param>
/// <param name="mapper">Collection items converter.</param>
[StructLayout(LayoutKind.Auto)]
public readonly struct ReadOnlyCollectionView<TInput, TOutput>(
    IReadOnlyCollection<TInput> collection,
    Func<TInput, TOutput> mapper
) : IReadOnlyCollection<TOutput>, IEquatable<ReadOnlyCollectionView<TInput, TOutput>> {
    private readonly IReadOnlyCollection<TInput>? _source = collection ?? throw new ArgumentNullException(nameof(collection));
    private readonly Func<TInput, TOutput> _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    /// <summary>
    /// Initializes a new lazily converted view.
    /// </summary>
    /// <param name="collection">Read-only collection to convert.</param>
    /// <param name="mapper">Collection items converter.</param>
    public ReadOnlyCollectionView(IReadOnlyCollection<TInput> collection, Converter<TInput, TOutput> mapper)
        : this(collection, Unsafe.As<Func<TInput, TOutput>>(mapper)) {
    }

    /// <summary>
    /// Count of items in the collection.
    /// </summary>
    public int Count => _source?.Count ?? 0;

    /// <summary>
    /// Returns enumerator over converted items.
    /// </summary>
    /// <returns>The enumerator over converted items.</returns>
    public IEnumerator<TOutput> GetEnumerator()
        => _source is null or { Count: 0 } || _mapper is null
            ? Enumerable.Empty<TOutput>().GetEnumerator()
            : _source.Select(_mapper).GetEnumerator();

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    private bool Equals(in ReadOnlyCollectionView<TInput, TOutput> other)
        => ReferenceEquals(_source, other._source) && _mapper == other._mapper;

    /// <summary>
    /// Determines whether two converted collections are same.
    /// </summary>
    /// <param name="other">Other collection to compare.</param>
    /// <returns><see langword="true"/> if this view wraps the same source collection and contains the same converter as other view; otherwise, <see langword="false"/>.</returns>
    public bool Equals(ReadOnlyCollectionView<TInput, TOutput> other) => Equals(in other);

    /// <summary>
    /// Returns hash code for the this view.
    /// </summary>
    /// <returns>The hash code of this view.</returns>
    public override int GetHashCode() => RuntimeHelpers.GetHashCode(_source) ^ _mapper.GetHashCode();

    /// <summary>
    /// Determines whether two converted collections are same.
    /// </summary>
    /// <param name="other">Other collection to compare.</param>
    /// <returns><see langword="true"/> if this view wraps the same source collection and contains the same converter as other view; otherwise, <see langword="false"/>.</returns>
    public override bool Equals(object? other)
        => other is ReadOnlyCollectionView<TInput, TOutput> view ? Equals(in view) : Equals(_source, other);

    /// <summary>
    /// Determines whether two collections are same.
    /// </summary>
    /// <param name="first">The first collection to compare.</param>
    /// <param name="second">The second collection to compare.</param>
    /// <returns><see langword="true"/> if the first view wraps the same source collection and contains the same converter as the second view; otherwise, <see langword="false"/>.</returns>
    public static bool operator ==(in ReadOnlyCollectionView<TInput, TOutput> first, in ReadOnlyCollectionView<TInput, TOutput> second)
        => first.Equals(in second);

    /// <summary>
    /// Determines whether two collections are not same.
    /// </summary>
    /// <param name="first">The first collection to compare.</param>
    /// <param name="second">The second collection to compare.</param>
    /// <returns><see langword="true"/> if the first view wraps the different source collection and contains the different converter as the second view; otherwise, <see langword="false"/>.</returns>
    public static bool operator !=(in ReadOnlyCollectionView<TInput, TOutput> first, in ReadOnlyCollectionView<TInput, TOutput> second)
        => !first.Equals(in second);
}