using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using System.Text.Json;
using Yolu.Threading;
using static System.Diagnostics.Stopwatch;
using System;
using System.ComponentModel;

namespace Yolu.DateTimes;


public class TimestampJsonConverter : JsonConverter<Timestamp> {
    public override Timestamp Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        if (reader.TokenType != JsonTokenType.Number) {
            throw new JsonException();
        }

        return new Timestamp(reader.GetInt64());
    }

    public override void Write(Utf8JsonWriter writer, Timestamp value, JsonSerializerOptions options) {
        // Convert the Timestamp to TimeSpan and write as string
        writer.WriteNumberValue(value.Ticks);
    }
}


/// <summary>
/// Represents timestamp.
/// </summary>
/// <remarks>
/// This class can be used as allocation-free alternative to <see cref="System.Diagnostics.Stopwatch"/>.
/// </remarks>
[JsonConverter(typeof(TimestampJsonConverter))]
[Serializable]
[ImmutableObject(true)]
public readonly record struct Timestamp :
    IEquatable<Timestamp>,
    IComparable<Timestamp>,
    IComparisonOperators<Timestamp, Timestamp, bool>,
    IAdditionOperators<Timestamp, TimeSpan, Timestamp>,
    ISubtractionOperators<Timestamp, TimeSpan, Timestamp>,
    IInterlockedOperations<Timestamp> {
    private static readonly double _tickFrequency = (double)TimeSpan.TicksPerSecond / Frequency;
    private readonly long _ticks;

    /// <summary>
    /// 
    /// </summary>
    public long Ticks => _ticks;

    /// <summary>
    /// Gets now.
    /// </summary>
    public static Timestamp Now => new(Math.Max(GetTimestamp(), 1L));

    public Timestamp(long ticks) => this._ticks = ticks;

    /// <summary>
    /// Captures the current point in time.
    /// </summary>
    public Timestamp() {
    }

    /// <summary>
    /// Constructs timestamp from <see cref="TimeSpan"/>.
    /// </summary>
    /// <param name="ts">The point in time.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="ts"/> is negative.</exception>
    public Timestamp(TimeSpan ts)
        : this(ts >= TimeSpan.Zero ? FromTimeSpan(ts) : throw new ArgumentOutOfRangeException(nameof(ts))) {
    }

    /// <summary>
    /// Gets a value indicating that the timestamp is zero.
    /// </summary>
    public bool IsEmpty => _ticks is 0L;

    /// <summary>
    /// Gets a value indcating that the current timestamp represents the future point in time.
    /// </summary>
    public bool IsFuture => _ticks > GetTimestamp();

    /// <summary>
    /// Gets a value indcating that the current timestamp represents the past point in time.
    /// </summary>
    public bool IsPast => _ticks < GetTimestamp();

    private static long ToTicks(double duration)
        => unchecked((long)(_tickFrequency * duration));

    private static long FromTimeSpan(TimeSpan value)
        => unchecked((long)(value.Ticks / _tickFrequency));

    /// <summary>
    /// Gets <see cref="TimeSpan"/> representing this timestamp.
    /// </summary>
    /// <remarks>
    /// This property may return a value with lost precision.
    /// </remarks>
    public TimeSpan Value => new(ToTicks(_ticks));

    /// <summary>
    /// Gets precise difference between the current point in time and this timestamp.
    /// </summary>
    /// <remarks>
    /// This property is always greater than or equal to <see cref="TimeSpan.Zero"/>.
    /// </remarks>
    public TimeSpan Elapsed => new(ToTicks(ElapsedTicks));

    /// <summary>
    /// Gets the total elapsed time measured by the current instance, in timer ticks.
    /// </summary>
    public long ElapsedTicks => Math.Max(0L, GetTimestamp() - _ticks);

    /// <summary>
    /// Gets the total elapsed time measured by the current instance, in milliseconds.
    /// </summary>
    public double ElapsedMilliseconds => (double)ElapsedTicks / Frequency * 1_000D;

    /// <summary>
    /// Gets a difference between two timestamps, in milliseconds.
    /// </summary>
    /// <param name="past">The timestamp in the past.</param>
    /// <returns>The number of milliseconds since <paramref name="past"/>.</returns>
    public double ElapsedSince(Timestamp past)
        => (double)(_ticks - past._ticks) / Frequency * 1_000D;

    /// <summary>
    /// Gets <see cref="TimeSpan"/> representing the given timestamp.
    /// </summary>
    /// <remarks>
    /// This operation leads to loss of precision.
    /// </remarks>
    /// <param name="stamp">The timestamp to convert.</param>
    public static explicit operator TimeSpan(Timestamp stamp) => stamp.Value;

    /// <summary>
    /// Compares this timestamp with the given value.
    /// </summary>
    /// <param name="other">The timestamp to compare.</param>
    /// <returns>The result of comparison.</returns>
    public int CompareTo(Timestamp other) => _ticks.CompareTo(other._ticks);

    /// <summary>
    /// Gets timestamp in the form of the string.
    /// </summary>
    /// <returns>The string representing this timestamp.</returns>
    public override string ToString() => Value.ToString();

    /// <summary>
    /// Determines whether the first timestamp is greater than the second.
    /// </summary>
    /// <param name="first">The first timestamp to compare.</param>
    /// <param name="second">The second timestamp to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="first"/> is greater than <paramref name="second"/>.</returns>
    public static bool operator >(Timestamp first, Timestamp second) => first._ticks > second._ticks;

    /// <summary>
    /// Determines whether the first timestamp is less than the second.
    /// </summary>
    /// <param name="first">The first timestamp to compare.</param>
    /// <param name="second">The second timestamp to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="first"/> is less than <paramref name="second"/>.</returns>
    public static bool operator <(Timestamp first, Timestamp second) => first._ticks < second._ticks;

    /// <summary>
    /// Determines whether the first timestamp is greater than or equal to the second.
    /// </summary>
    /// <param name="first">The first timestamp to compare.</param>
    /// <param name="second">The second timestamp to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="first"/> is greater than or equal to <paramref name="second"/>.</returns>
    public static bool operator >=(Timestamp first, Timestamp second) => first._ticks >= second._ticks;

    /// <summary>
    /// Determines whether the first timestamp is less than or equal to the second.
    /// </summary>
    /// <param name="first">The first timestamp to compare.</param>
    /// <param name="second">The second timestamp to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="first"/> is less than or equal to <paramref name="second"/>.</returns>
    public static bool operator <=(Timestamp first, Timestamp second) => first._ticks <= second._ticks;

    /// <summary>
    /// Adds the specified duration to the timestamp.
    /// </summary>
    /// <param name="x">The timestamp value.</param>
    /// <param name="y">The delta.</param>
    /// <returns>The modified timestamp.</returns>
    /// <exception cref="OverflowException"><paramref name="y"/> is too large.</exception>
    public static Timestamp operator checked +(Timestamp x, TimeSpan y) {
        var ticks = checked(x._ticks + FromTimeSpan(y));
        return ticks >= 0L ? new(ticks) : throw new OverflowException();
    }

    /// <summary>
    /// Adds the specified duration to the timestamp.
    /// </summary>
    /// <param name="x">The timestamp value.</param>
    /// <param name="y">The delta.</param>
    /// <returns>The modified timestamp.</returns>
    public static Timestamp operator +(Timestamp x, TimeSpan y) {
        var ticks = x._ticks + FromTimeSpan(y);
        return ticks > 0L ? new(ticks) : default;
    }

    /// <summary>
    /// Subtracts the specified duration from the timestamp.
    /// </summary>
    /// <param name="x">The timestamp value.</param>
    /// <param name="y">The delta.</param>
    /// <returns>The modified timestamp.</returns>
    /// <exception cref="OverflowException"><paramref name="y"/> is too large.</exception>
    public static Timestamp operator checked -(Timestamp x, TimeSpan y) {
        var ticks = checked(x._ticks - FromTimeSpan(y));
        return ticks >= 0L ? new(ticks) : throw new OverflowException();
    }

    /// <summary>
    /// Subtracts the specified duration from the timestamp.
    /// </summary>
    /// <param name="x">The timestamp value.</param>
    /// <param name="y">The delta.</param>
    /// <returns>The modified timestamp.</returns>
    /// <exception cref="OverflowException"><paramref name="y"/> is too large.</exception>
    public static Timestamp operator -(Timestamp x, TimeSpan y) {
        var ticks = x._ticks - FromTimeSpan(y);
        return ticks > 0L ? new(ticks) : default;
    }

    /// <summary>
    /// Reads the timestamp and prevents the processor from reordering memory operations.
    /// </summary>
    /// <param name="location">The managed pointer to the timestamp.</param>
    /// <returns>The value at the specified location.</returns>
    public static Timestamp VolatileRead(ref readonly Timestamp location)
        => new(Volatile.Read(in location._ticks));

    /// <summary>
    /// Writes the timestamp and prevents the proces from reordering memory operations.
    /// </summary>
    /// <param name="location">The managed pointer to the timestamp.</param>
    /// <param name="newValue">The value to write.</param>
    public static void VolatileWrite(ref Timestamp location, Timestamp newValue)
        => Volatile.Write(ref Unsafe.AsRef(in location._ticks), newValue._ticks);

    /// <summary>
    /// Updates the timestamp to the current point in time and prevents the proces from reordering memory operations.
    /// </summary>
    /// <param name="location">The location of the timestampt to update.</param>
    public static void Refresh(ref Timestamp location)
        => Volatile.Write(ref Unsafe.AsRef(in location._ticks), Math.Max(1L, GetTimestamp()));

    /// <inheritdoc cref="IInterlockedOperations{T}.CompareExchange(ref T, T, T)"/>
    public static Timestamp CompareExchange(ref Timestamp location, Timestamp value, Timestamp comparand)
        => new(Interlocked.CompareExchange(ref Unsafe.AsRef(in location._ticks), value._ticks, comparand._ticks));
}