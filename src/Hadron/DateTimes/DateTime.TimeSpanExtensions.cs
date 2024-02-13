using System.Diagnostics.Contracts;

namespace Hadron.DateTimes;

/// <summary>
/// Static class containing Fluent <see cref="DateTime"/> extension methods.
/// </summary>
public static partial class DateTimeExtensions {
    /// <summary>
    /// Subtracts given <see cref="TimeSpan"/> from current date (<see cref="DateTime.Now"/>) and returns resulting <see cref="DateTime"/> in the past.
    /// </summary>
    [Pure]
    public static DateTime Ago(this TimeSpan from) =>
        from.Before(DateTime.Now);

    /// <summary>
    /// Subtracts given <see cref="TimeSpan"/> from <paramref name="originalValue"/> <see cref="DateTime"/> and returns resulting <see cref="DateTime"/> in the past.
    /// </summary>
    [Pure]
    public static DateTime Ago(this TimeSpan from, DateTime originalValue) =>
        from.Before(originalValue);

    /// <summary>
    /// Subtracts given <see cref="TimeSpan"/> from <paramref name="originalValue"/> <see cref="DateTime"/> and returns resulting <see cref="DateTime"/> in the past.
    /// </summary>
    [Pure]
    public static DateTime Before(this TimeSpan from, DateTime originalValue) =>
        originalValue - from;

    /// <summary>
    /// Adds given <see cref="TimeSpan"/> to current <see cref="DateTime.Now"/> and returns resulting <see cref="DateTime"/> in the future.
    /// </summary>
    [Pure]
    public static DateTime FromNow(this TimeSpan from) =>
        from.From(DateTime.Now);

    /// <summary>
    /// Adds given <see cref="TimeSpan"/> to supplied <paramref name="originalValue"/> <see cref="DateTime"/> and returns resulting <see cref="DateTime"/> in the future.
    /// </summary>
    [Pure]
    public static DateTime From(this TimeSpan from, DateTime originalValue) =>
        originalValue + from;

    /// <summary>
    /// Adds given <see cref="TimeSpan"/> to supplied <paramref name="originalValue"/> <see cref="DateTime"/> and returns resulting <see cref="DateTime"/> in the future.
    /// </summary>
    /// <seealso cref="From(TimeSpan, DateTime)"/>
    /// <remarks>
    /// Synonym of <see cref="From(TimeSpan, DateTime)"/> method.
    /// </remarks>
    [Pure]
    public static DateTime Since(this TimeSpan from, DateTime originalValue) =>
        From(from, originalValue);
}