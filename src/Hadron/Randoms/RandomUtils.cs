using Hadron.DateTimes;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace Hadron.Randoms;

/// <summary>
/// Utility class for generating random values.
/// </summary>
public static partial class RandomUtils {
    /// <summary>
    /// The characters used for generating random strings.
    /// </summary>
    public const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    /// <summary>
    /// Takes a random item from the given list.
    /// </summary>
    /// <typeparam name="T">The type of items in the list.</typeparam>
    /// <param name="random">The random number generator.</param>
    /// <param name="items">The list of items.</param>
    /// <returns>A random item from the list.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T TakeOne<T>(this Random random, IReadOnlyList<T> items) {
        return items[random.Next(items.Count)];
    }

    /// <summary>
    /// Takes a specified number of random items from the given list.
    /// </summary>
    /// <typeparam name="T">The type of items in the list.</typeparam>
    /// <param name="random">The random number generator.</param>
    /// <param name="items">The list of items.</param>
    /// <param name="count">The number of items to take.</param>
    /// <returns>A list of random items from the list.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IReadOnlyList<T> Take<T>(this Random random, IReadOnlyList<T> items, int count) {
        var list = new T[count];
        for (var i = 0; i < list.Length; i++) {
            list[i] = items[random.Next(items.Count)];
        }

        return list;
    }

    /// <summary>
    /// Generates a random boolean value.
    /// </summary>
    /// <param name="random">The random number generator.</param>
    /// <returns>A random boolean value.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool NextBool(this Random random) {
        return random.Next(2) == 0;
    }

    /// <summary>
    /// Generates a random string of the specified length.
    /// </summary>
    /// <param name="random">The random number generator.</param>
    /// <param name="length">The length of the string.</param>
    /// <returns>A random string of the specified length.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string NextString(this Random random, int length) {
        return new string(Enumerable.Repeat(Chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    /// <summary>
    /// Generates a random DateTime value between the specified start and end dates.
    /// </summary>
    /// <param name="random">The random number generator.</param>
    /// <param name="startDate">The start date.</param>
    /// <param name="endDate">The end date.</param>
    /// <returns>A random DateTime value between the start and end dates.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateTime NextDateTime(this Random random, DateTime startDate, DateTime endDate) {
        var timeSpan = endDate - startDate;
        var randomTimeSpan = new TimeSpan((long)(random.NextDouble() * timeSpan.Ticks));
        return startDate + randomTimeSpan;
    }

    /// <summary>
    /// Generates a random DateTime value between the specified start and end dates.
    /// </summary>
    /// <param name="random">The random number generator.</param>
    /// <param name="startDate">The start date.</param>
    /// <param name="endDate">The end date.</param>
    /// <returns>A random DateTime value between the start and end dates.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateOnly NextDateOnly(this Random random, DateOnly startDate, DateOnly endDate) {
        var timeSpan = endDate.ToDateTime(TimeOnly.MinValue) - startDate.ToDateTime(TimeOnly.MaxValue);
        var randomTimeSpan = new TimeSpan((long)(random.NextDouble() * timeSpan.Ticks));
        return (startDate.ToDateTime(TimeOnly.MaxValue) + randomTimeSpan).GetDateOnly();
    }

    /// <param name="minValue">The minimum value.</param>
    /// <param name="maxValue">The maximum value.</param>
    /// <returns>A random TimeOnly value between the minimum and maximum values.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TimeOnly NextTimeOnly(this Random random, TimeOnly minValue, TimeOnly maxValue) {
        var timeSpan = maxValue - minValue;
        var randomTimeSpan = new TimeSpan((long)(random.NextDouble() * timeSpan.Ticks));
        return minValue.Add(randomTimeSpan);
    }

    /// <summary>
    /// Generates a random TimeSpan value between the specified minimum and maximum values.
    /// </summary>
    /// <param name="random">The random number generator.</param>
    /// <param name="minValue">The minimum value.</param>
    /// <param name="maxValue">The maximum value.</param>
    /// <returns>A random TimeSpan value between the minimum and maximum values.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TimeSpan NextTimeSpan(this Random random, TimeSpan minValue, TimeSpan maxValue) {
        var range = maxValue - minValue;
        var randomTicks = (long)(random.NextDouble() * range.Ticks);
        return minValue + TimeSpan.FromTicks(randomTicks);
    }

    /// <summary>
    /// Generates a random double value between the specified minimum and maximum values.
    /// </summary>
    /// <param name="random">The random number generator.</param>
    /// <param name="minValue">The minimum value.</param>
    /// <param name="maxValue">The maximum value.</param>
    /// <returns>A random double value between the minimum and maximum values.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double NextDouble(this Random random, double minValue, double maxValue) {
        return random.NextDouble() * (maxValue - minValue) + minValue;
    }

    /// <summary>
    /// Generates a random integer value between the specified minimum and maximum values.
    /// </summary>
    /// <param name="random">The random number generator.</param>
    /// <param name="minValue">The minimum value.</param>
    /// <param name="maxValue">The maximum value.</param>
    /// <returns>A random integer value between the minimum and maximum values.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int NextInt(this Random random, int minValue, int maxValue) {
        return random.Next(minValue, maxValue);
    }

    /// <summary>
    /// Generates a random long value between the specified minimum and maximum values.
    /// </summary>
    /// <param name="random">The random number generator.</param>
    /// <param name="minValue">The minimum value.</param>
    /// <param name="maxValue">The maximum value.</param>
    /// <returns>A random long value between the minimum and maximum values.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long NextLong(this Random random, long minValue, long maxValue) {
        var range = maxValue - minValue;
        var buffer = new byte[8];
        random.NextBytes(buffer);
        var randomLong = BitConverter.ToInt64(buffer, 0);
        return (Math.Abs(randomLong % range) + minValue);
    }

    /// <summary>
    /// Generates a random float value between the specified minimum and maximum values.
    /// </summary>
    /// <param name="random">The random number generator.</param>
    /// <param name="minValue">The minimum value.</param>
    /// <param name="maxValue">The maximum value.</param>
    /// <returns>A random float value between the minimum and maximum values.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float NextFloat(this Random random, float minValue, float maxValue) {
        return (float)(random.NextDouble() * (maxValue - minValue) + minValue);
    }

    /// <summary>
    /// Generates a random Guid value.
    /// </summary>
    /// <param name="random">The random number generator.</param>
    /// <returns>A random Guid value.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Guid NextGuid(this Random random) {
        var buffer = new byte[16];
        random.NextBytes(buffer);
        return new Guid(buffer);
    }
}
