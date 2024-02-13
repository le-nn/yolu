using System.Diagnostics.Contracts;

namespace Hadron.DateTimes;

/// <summary>
/// Extension methods on <see cref="int"/> to allow for a more fluent way of specifying a <see cref="TimeSpan"/>.
/// </summary>
/// <example>
/// Instead of<br />
/// <br />
/// TimeSpan.FromHours(12)<br />
/// <br />
/// you can write<br />
/// <br />
/// 12.Hours()<br />
/// <br />
/// Or even<br />
/// <br />
/// 12.Hours().And(30.Minutes()).
/// </example>
/// <seealso cref="FluentDateTimeExtensions"/>
public static class NumberExtensions {
    /// <summary>
    /// Generates <see cref="TimeSpan"/> value for given number of Years.
    /// </summary>
    [Pure]
    public static TimeSpan Years(this int years) => TimeSpan.FromDays(years * 365);

    /// <summary>
    /// Returns <see cref="TimeSpan"/> for given number of Weeks (number of weeks * 7).
    /// </summary>
    [Pure]
    public static TimeSpan Weeks(this int weeks) => TimeSpan.FromDays(weeks * 7);

    /// <summary>
    /// Returns <see cref="TimeSpan"/> for given number of Weeks (number of weeks * 7).
    /// </summary>
    [Pure]
    public static TimeSpan Weeks(this double weeks) => TimeSpan.FromDays(weeks * 7);

    /// <summary>
    /// Returns <see cref="TimeSpan"/> for given number of Days (number of days).
    /// </summary>
    [Pure]
    public static TimeSpan Days(this int days) => TimeSpan.FromDays(days);

    /// <summary>
    /// Returns <see cref="TimeSpan"/> for given number of Days (number of days).
    /// </summary>
    [Pure]
    public static TimeSpan Days(this double days) => TimeSpan.FromDays(days);

    /// <summary>
    /// Returns <see cref="TimeSpan"/> for given number of Hours.
    /// </summary>
    [Pure]
    public static TimeSpan Hours(this int hours) => TimeSpan.FromHours(hours);

    /// <summary>
    /// Returns <see cref="TimeSpan"/> for given number of Hours.
    /// </summary>
    [Pure]
    public static TimeSpan Hours(this double hours) => TimeSpan.FromHours(hours);

    /// <summary>
    /// Returns <see cref="TimeSpan"/> for given number of Minutes.
    /// </summary>
    [Pure]
    public static TimeSpan Minutes(this int minutes) => TimeSpan.FromMinutes(minutes);

    /// <summary>
    /// Returns <see cref="TimeSpan"/> for given number of Minutes.
    /// </summary>
    [Pure]
    public static TimeSpan Minutes(this double minutes) => TimeSpan.FromMinutes(minutes);

    /// <summary>
    /// Returns <see cref="TimeSpan"/> for given number of Seconds.
    /// </summary>
    public static TimeSpan Seconds(this int seconds) => TimeSpan.FromSeconds(seconds);

    /// <summary>
    /// Returns <see cref="TimeSpan"/> for given number of Seconds.
    /// </summary>
    [Pure]
    public static TimeSpan Seconds(this double seconds) => TimeSpan.FromSeconds(seconds);

    /// <summary>
    /// Returns <see cref="TimeSpan"/> for given number of Milliseconds.
    /// </summary>
    [Pure]
    public static TimeSpan Milliseconds(this int milliseconds) => TimeSpan.FromMilliseconds(milliseconds);

    /// <summary>
    /// Returns <see cref="TimeSpan"/> for given number of Milliseconds.
    /// </summary>
    [Pure]
    public static TimeSpan Milliseconds(this double milliseconds) => TimeSpan.FromMilliseconds(milliseconds);

    /// <summary>
    /// Returns <see cref="TimeSpan"/> for given number of ticks.
    /// </summary>
    [Pure]
    public static TimeSpan Ticks(this int ticks) => TimeSpan.FromTicks(ticks);

    /// <summary>
    /// Represents the number of ticks that are in 1 microsecond.
    /// </summary>
    public const long TicksPerMicrosecond = TimeSpan.TicksPerMillisecond / 1000;

    /// <summary>
    /// Represents the number of ticks that are in 1 nanosecond.
    /// </summary>
    public const double TicksPerNanosecond = TicksPerMicrosecond / 1000d;

    /// <summary>
    /// Returns a <see cref="TimeSpan" /> based on a number of ticks.
    /// </summary>
    [Pure]
    public static TimeSpan Ticks(this long ticks) {
        return TimeSpan.FromTicks(ticks);
    }

    /// <summary>
    /// Gets the nanoseconds component of the time interval represented by the current <see cref="TimeSpan" /> structure.
    /// </summary>
    [Pure]
    public static int Nanoseconds(this TimeSpan self) {
        return (int)((self.Ticks % TicksPerMicrosecond) * (1d / TicksPerNanosecond));
    }

    /// <summary>
    /// Returns a <see cref="TimeSpan" /> based on a number of nanoseconds.
    /// </summary>
    /// <remarks>
    /// .NET's smallest resolutions is 100 nanoseconds. Any nanoseconds passed in
    /// lower than .NET's resolution will be rounded using the default rounding
    /// algorithm in Math.Round().
    /// </remarks>
    [Pure]
    public static TimeSpan Nanoseconds(this int nanoseconds) {
        return ((long)Math.Round(nanoseconds * TicksPerNanosecond)).Ticks();
    }

    /// <summary>
    /// Returns a <see cref="TimeSpan" /> based on a number of nanoseconds.
    /// </summary>
    /// <remarks>
    /// .NET's smallest resolutions is 100 nanoseconds. Any nanoseconds passed in
    /// lower than .NET's resolution will be rounded using the default rounding
    /// algorithm in Math.Round().
    /// </remarks>
    [Pure]
    public static TimeSpan Nanoseconds(this long nanoseconds) {
        return ((long)Math.Round(nanoseconds * TicksPerNanosecond)).Ticks();
    }

    /// <summary>
    /// Gets the value of the current <see cref="TimeSpan" /> structure expressed in whole and fractional nanoseconds.
    /// </summary>
    [Pure]
    public static double TotalNanoseconds(this TimeSpan self) {
        return self.Ticks * (1d / TicksPerNanosecond);
    }

    /// <summary>
    /// Gets the microseconds component of the time interval represented by the current <see cref="TimeSpan" /> structure.
    /// </summary>
    [Pure]
    public static int Microseconds(this TimeSpan self) {
        return (int)((self.Ticks % TimeSpan.TicksPerMillisecond) * (1d / TicksPerMicrosecond));
    }

    /// <summary>
    /// Returns a <see cref="TimeSpan" /> based on a number of microseconds.
    /// </summary>
    [Pure]
    public static TimeSpan Microseconds(this int microseconds) {
        return (microseconds * TicksPerMicrosecond).Ticks();
    }

    /// <summary>
    /// Returns a <see cref="TimeSpan" /> based on a number of microseconds.
    /// </summary>
    [Pure]
    public static TimeSpan Microseconds(this long microseconds) {
        return (microseconds * TicksPerMicrosecond).Ticks();
    }

    /// <summary>
    /// Gets the value of the current <see cref="TimeSpan" /> structure expressed in whole and fractional microseconds.
    /// </summary>
    [Pure]
    public static double TotalMicroseconds(this TimeSpan self) {
        return self.Ticks * (1d / TicksPerMicrosecond);
    }

    /// <summary>
    /// Returns a <see cref="TimeSpan" /> based on a number of seconds, and add the specified
    /// <paramref name="offset"/>.
    /// </summary>
    [Pure]
    public static TimeSpan Seconds(this int seconds, TimeSpan offset) {
        return TimeSpan.FromSeconds(seconds).Add(offset);
    }


    /// <summary>
    /// Returns a <see cref="TimeSpan" /> based on a number of minutes, and add the specified
    /// <paramref name="offset"/>.
    /// </summary>
    [Pure]
    public static TimeSpan Minutes(this int minutes, TimeSpan offset) {
        return TimeSpan.FromMinutes(minutes).Add(offset);
    }

    /// <summary>
    /// Returns a <see cref="TimeSpan" /> based on a number of hours, and add the specified
    /// <paramref name="offset"/>.
    /// </summary>
    [Pure]
    public static TimeSpan Hours(this int hours, TimeSpan offset) {
        return TimeSpan.FromHours(hours).Add(offset);
    }

    /// <summary>
    /// <summary>
    /// Returns a <see cref="TimeSpan" /> based on a number of days, and add the specified
    /// <paramref name="offset"/>.
    /// </summary>
    [Pure]
    public static TimeSpan Days(this int days, TimeSpan offset) {
        return TimeSpan.FromDays(days).Add(offset);
    }

    /// <summary>
    /// Convenience method for chaining multiple calls to the methods provided by this class.
    /// </summary>
    /// <example>
    /// 23.Hours().And(59.Minutes())
    /// </example>
    [Pure]
    public static TimeSpan And(this TimeSpan sourceTime, TimeSpan offset) {
        return sourceTime.Add(offset);
    }
}
