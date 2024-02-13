using System.Diagnostics.Contracts;
using System.Globalization;

namespace Hadron.DateTimes;

/// <summary>
/// Static class containing Fluent <see cref="DateOnly"/> extension methods.
/// </summary>
public static partial class DateOnlyExtensions {
    /// <summary>
    /// Gets the number of days in the month of the specified <see cref="DateOnly"/>.
    /// </summary>
    /// <param name="date">The <see cref="DateOnly"/> to get the number of days in the month for.</param>
    /// <returns>The number of days in the month of the specified <see cref="DateOnly"/>.</returns>
    public static int DaysInMonth(this DateOnly date) => DateTime.DaysInMonth(date.Year, date.Month);

    /// <summary>
    /// Gets the number of days in the specified month of the specified year.
    /// </summary>
    /// <param name="year">The year.</param>
    /// <param name="month">The month.</param>
    /// <returns>The number of days in the specified month of the specified year.</returns>
    public static int DaysInMonth(int year, int month) => DateTime.DaysInMonth(year, month);

    /// <summary>
    /// Returns the very end of the given day (the last millisecond of the last hour for the given <see cref="DateOnly"/>).
    /// </summary>
    [Pure]
    public static DateTime EndOfDay(this DateOnly date) => date.ToDateTime(TimeOnly.MaxValue);

    /// <summary>
    /// Returns the timezone-adjusted very end of the given day (the last millisecond of the last hour for the given <see cref="DateOnly"/>).
    /// </summary>
    [Pure]
    public static DateTime EndOfDay(this DateOnly date, int timeZoneOffset) =>
        new DateTime(date.Year, date.Month, date.Day, 23, 59, 59, 999)
            .AddHours(timeZoneOffset);

    /// <summary>
    /// Returns the same date (same Day, Month, Hour, Minute, Second etc) in the next calendar year.
    /// If that day does not exist in next year in same month, number of missing days is added to the last day in same month next year.
    /// </summary>
    [Pure]
    public static DateOnly NextYear(this DateOnly start) {
        var nextYear = start.Year + 1;
        var numberOfDaysInSameMonthNextYear = DateTime.DaysInMonth(nextYear, start.Month);

        if (numberOfDaysInSameMonthNextYear < start.Day) {
            var differenceInDays = start.Day - numberOfDaysInSameMonthNextYear;
            var DateOnly = new DateOnly(nextYear, start.Month, numberOfDaysInSameMonthNextYear);
            return DateOnly.AddDays(differenceInDays);
        }

        return new(nextYear, start.Month, start.Day);
    }

    /// <summary>
    /// Returns the same date (same Day, Month, Hour, Minute, Second etc) in the previous calendar year.
    /// If that day does not exist in previous year in same month, number of missing days is added to the last day in same month previous year.
    /// </summary>
    [Pure]
    public static DateOnly PreviousYear(this DateOnly start) {
        var previousYear = start.Year - 1;
        var numberOfDaysInSameMonthPreviousYear = DateTime.DaysInMonth(previousYear, start.Month);

        if (numberOfDaysInSameMonthPreviousYear < start.Day) {
            var differenceInDays = start.Day - numberOfDaysInSameMonthPreviousYear;
            var DateOnly = new DateOnly(previousYear, start.Month, numberOfDaysInSameMonthPreviousYear);
            return DateOnly.AddDays(differenceInDays);
        }
        return new(previousYear, start.Month, start.Day);
    }

    /// <summary>
    /// Returns <see cref="DateOnly"/> increased by 24 hours ie Next Day.
    /// </summary>
    [Pure]
    public static DateOnly NextDay(this DateOnly start) =>
        start.AddDays(1);

    /// <summary>
    /// Returns <see cref="DateOnly"/> decreased by 24h period ie Previous Day.
    /// </summary>
    [Pure]
    public static DateOnly PreviousDay(this DateOnly start) => start.AddDays(-1);

    /// <summary>
    /// Returns first next occurrence of specified <see cref="DayOfWeek"/>.
    /// </summary>
    [Pure]
    public static DateOnly Next(this DateOnly start, DayOfWeek day) {
        do {
            start = start.NextDay();
        } while (start.DayOfWeek != day);

        return start;
    }

    /// <summary>
    /// Returns first next occurrence of specified <see cref="DayOfWeek"/>.
    /// </summary>
    [Pure]
    public static DateOnly Previous(this DateOnly start, DayOfWeek day) {
        do {
            start = start.PreviousDay();
        } while (start.DayOfWeek != day);

        return start;
    }

    /// <summary>
    /// Increases supplied <see cref="DateOnly"/> for 7 days ie returns the Next Week.
    /// </summary>
    [Pure]
    public static DateOnly WeekAfter(this DateOnly start) =>
        start.AddDays(7);

    /// <summary>
    /// Decreases supplied <see cref="DateOnly"/> for 7 days ie returns the Previous Week.
    /// </summary>
    [Pure]
    public static DateOnly WeekEarlier(this DateOnly start) =>
         start.AddDays(-7);

    /// <summary>
    /// Increases the <see cref="DateOnly"/> object with given <see cref="TimeSpan"/> value.
    /// </summary>
    [Pure]
    public static DateTime IncreaseTime(this DateOnly startDate, TimeSpan toAdd) =>
        startDate.ToDateTime(default) + toAdd;

    /// <summary>
    /// Decreases the <see cref="DateOnly"/> object with given <see cref="TimeSpan"/> value.
    /// </summary>
    [Pure]
    public static DateTime DecreaseTime(this DateOnly startDate, TimeSpan toSubtract) =>
               startDate.ToDateTime(default) - toSubtract;

    /// <summary>
    /// Returns the original <see cref="DateOnly"/> with Hour part changed to supplied hour parameter.
    /// </summary>
    [Pure]
    public static DateTime SetTime(this DateOnly originalDate, int hour) =>
        new(originalDate.Year, originalDate.Month, originalDate.Day, hour, 0, 0, 0);

    /// <summary>
    /// Returns the original <see cref="DateOnly"/> with Hour and Minute parts changed to supplied hour and minute parameters.
    /// </summary>
    [Pure]
    public static DateTime SetTime(this DateOnly originalDate, int hour, int minute) =>
        new(originalDate.Year, originalDate.Month, originalDate.Day, hour, minute, 0, 0);

    /// <summary>
    /// Returns the original <see cref="DateOnly"/> with Hour, Minute and Second parts changed to supplied hour, minute and second parameters.
    /// </summary>
    [Pure]
    public static DateTime SetTime(this DateOnly originalDate, int hour, int minute, int second) =>
        new(originalDate.Year, originalDate.Month, originalDate.Day, hour, minute, second, 0);

    /// <summary>
    /// Returns the original <see cref="DateOnly"/> with Hour, Minute, Second and Millisecond parts changed to supplied hour, minute, second and millisecond parameters.
    /// </summary>
    [Pure]
    public static DateTime SetTime(this DateOnly originalDate, int hour, int minute, int second, int millisecond) =>
        new(originalDate.Year, originalDate.Month, originalDate.Day, hour, minute, second, millisecond);

    /// <summary>
    /// Returns the original <see cref="DateOnly"/> with <see cref="TimeOnly"/> parameters.
    /// </summary>
    [Pure]
    public static DateTime SetTime(this DateOnly originalDate, TimeOnly time) =>
        originalDate.ToDateTime(time);

    /// <summary>
    /// Returns original <see cref="DateOnly"/> value with time part set to Noon (12:00:00h).
    /// </summary>
    /// <param name="value">The <see cref="DateOnly"/> find Noon for.</param>
    /// <returns>A <see cref="DateOnly"/> value with time part set to Noon (12:00:00h).</returns>
    [Pure]
    public static DateTime Noon(this DateOnly value) =>
        value.SetTime(12, 0, 0, 0);

    /// <summary>
    /// Returns <see cref="DateOnly"/> with changed Year part.
    /// </summary>
    [Pure]
    public static DateOnly SetDate(this DateOnly value, int year) =>
        new(year, value.Month, value.Day);

    /// <summary>
    /// Returns <see cref="DateOnly"/> with changed Year and Month part.
    /// </summary>
    [Pure]
    public static DateOnly SetDate(this DateOnly value, int year, int month) =>
        new(year, month, value.Day);

    /// <summary>
    /// Returns <see cref="DateOnly"/> with changed Year, Month and Day part.
    /// </summary>
    [Pure]
    public static DateOnly SetDate(this DateOnly value, int year, int month, int day) =>
        new(year, month, day);

    /// <summary>
    /// Returns <see cref="DateOnly"/> with changed Year part.
    /// </summary>
    [Pure]
    public static DateOnly SetYear(this DateOnly value, int year) => new(year, value.Month, value.Day);

    /// <summary>
    /// Returns <see cref="DateOnly"/> with changed Month part.
    /// </summary>
    [Pure]
    public static DateOnly SetMonth(this DateOnly value, int month) => new(value.Year, month, value.Day);

    /// <summary>
    /// Returns <see cref="DateOnly"/> with changed Day part.
    /// </summary>
    [Pure]
    public static DateOnly SetDay(this DateOnly value, int day) => new(value.Year, value.Month, day);

    /// <summary>
    /// Determines whether the specified <see cref="DateOnly"/> is before then current value.
    /// </summary>
    /// <param name="current">The current value.</param>
    /// <param name="toCompareWith">Value to compare with.</param>
    /// <returns>
    /// <c>true</c> if the specified current is before; otherwise, <c>false</c>.
    /// </returns>
    [Pure]
    public static bool IsBefore(this DateOnly current, DateOnly toCompareWith) =>
        current < toCompareWith;

    /// <summary>
    /// Determines whether the specified <see cref="DateOnly"/> value is After then current value.
    /// </summary>
    /// <param name="current">The current value.</param>
    /// <param name="toCompareWith">Value to compare with.</param>
    /// <returns>
    /// <c>true</c> if the specified current is after; otherwise, <c>false</c>.
    /// </returns>
    [Pure]
    public static bool IsAfter(this DateOnly current, DateOnly toCompareWith) =>
        current > toCompareWith;

    /// <summary>
    /// Sets the day of the <see cref="DateOnly"/> to the first day in that calendar quarter.
    /// credit to http://www.devcurry.com/2009/05/find-first-and-last-day-of-current.html
    /// </summary>
    /// <returns>given <see cref="DateOnly"/> with the day part set to the first day in the quarter.</returns>
    [Pure]
    public static DateOnly FirstDayOfQuarter(this DateOnly current) {
        var currentQuarter = (current.Month - 1) / 3 + 1;
        var firstDay = new DateOnly(current.Year, 3 * currentQuarter - 2, 1);

        return current.SetDate(firstDay.Year, firstDay.Month, firstDay.Day);
    }

    /// <summary>
    /// Sets the day of the <see cref="DateOnly"/> to the first day in that month.
    /// </summary>
    /// <param name="current">The current <see cref="DateOnly"/> to be changed.</param>
    /// <returns>given <see cref="DateOnly"/> with the day part set to the first day in that month.</returns>
    [Pure]
    public static DateOnly FirstDayOfMonth(this DateOnly current) =>
        current.SetDay(1);

    /// <summary>
    /// Sets the day of the <see cref="DateOnly"/> to the last day in that calendar quarter.
    /// credit to http://www.devcurry.com/2009/05/find-first-and-last-day-of-current.html
    /// </summary>
    /// <returns>given <see cref="DateOnly"/> with the day part set to the last day in the quarter.</returns>
    [Pure]
    public static DateOnly LastDayOfQuarter(this DateOnly current) {
        var currentQuarter = (current.Month - 1) / 3 + 1;
        var firstDay = current.SetDate(current.Year, 3 * currentQuarter - 2, 1);
        return firstDay.SetMonth(firstDay.Month + 2).LastDayOfMonth();
    }

    /// <summary>
    /// Sets the day of the <see cref="DateOnly"/> to the last day in that month.
    /// </summary>
    /// <param name="current">The current DateOnly to be changed.</param>
    /// <returns>given <see cref="DateOnly"/> with the day part set to the last day in that month.</returns>
    [Pure]
    public static DateOnly LastDayOfMonth(this DateOnly current) =>
        current.SetDay(DateTime.DaysInMonth(current.Year, current.Month));

    /// <summary>
    /// Adds the given number of business days to the <see cref="DateOnly"/>.
    /// </summary>
    /// <param name="current">The date to be changed.</param>
    /// <param name="days">Number of business days to be added.</param>
    /// <returns>A <see cref="DateOnly"/> increased by a given number of business days.</returns>
    [Pure]
    public static DateOnly AddBusinessDays(this DateOnly current, int days) {
        var sign = Math.Sign(days);
        var unsignedDays = Math.Abs(days);
        for (var i = 0; i < unsignedDays; i++) {
            current = current.AddDays(sign);
            if (current.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday) {
                i--;
            }
        }
        return current;
    }

    /// <summary>
    /// Subtracts the given number of business days to the <see cref="DateOnly"/>.
    /// </summary>
    /// <param name="current">The date to be changed.</param>
    /// <param name="days">Number of business days to be subtracted.</param>
    /// <returns>A <see cref="DateOnly"/> increased by a given number of business days.</returns>
    [Pure]
    public static DateOnly SubtractBusinessDays(this DateOnly current, int days) =>
        current.AddBusinessDays(-days);

    /// <summary>
    /// Determine if a <see cref="DateOnly"/> is in the future.
    /// </summary>
    /// <param name="DateOnly">The date to be checked.</param>
    /// <returns><c>true</c> if <paramref name="DateOnly"/> is in the future; otherwise <c>false</c>.</returns>
    [Pure]
    public static bool IsInFuture(this DateOnly DateOnly) =>
        DateOnly > DateTime.Now.GetDateOnly();

    /// <summary>
    /// Determine if a <see cref="DateOnly"/> is in the past.
    /// </summary>
    /// <param name="DateOnly">The date to be checked.</param>
    /// <returns><c>true</c> if <paramref name="DateOnly"/> is in the past; otherwise <c>false</c>.</returns>
    [Pure]
    public static bool IsInPast(this DateOnly DateOnly) =>
        DateOnly < DateTime.Now.GetDateOnly();

    /// <summary>
    /// Returns a DateOnly adjusted to the beginning of the week.
    /// </summary>
    /// <param name="DateOnly">The DateOnly to adjust</param>
    /// <returns>A DateOnly instance adjusted to the beginning of the current week</returns>
    /// <remarks>the beginning of the week is controlled by the current Culture</remarks>
    [Pure]
    public static DateOnly FirstDayOfWeek(this DateOnly DateOnly) {
        var currentCulture = CultureInfo.CurrentCulture;
        var firstDayOfWeek = currentCulture.DateTimeFormat.FirstDayOfWeek;
        var offset = DateOnly.DayOfWeek - firstDayOfWeek < 0 ? 7 : 0;
        var numberOfDaysSinceBeginningOfTheWeek = DateOnly.DayOfWeek + offset - firstDayOfWeek;

        return DateOnly.AddDays(-numberOfDaysSinceBeginningOfTheWeek);
    }

    /// <summary>
    /// Obsolete. This method has been renamed to FirstDayOfWeek to be more consistent with existing conventions.
    /// </summary>
    [Obsolete("This method has been renamed to FirstDayOfWeek to be more consistent with existing conventions.")]
    [Pure]
    public static DateOnly StartOfWeek(this DateOnly DateOnly) =>
        DateOnly.FirstDayOfWeek();

    /// <summary>
    /// Returns the first day of the year keeping the time component intact. Eg, 2011-02-04T06:40:20.005 => 2011-01-01T06:40:20.005
    /// </summary>
    /// <param name="current">The DateOnly to adjust</param>
    [Pure]
    public static DateOnly FirstDayOfYear(this DateOnly current) =>
        current.SetDate(current.Year, 1, 1);

    /// <summary>
    /// Returns the last day of the week keeping the time component intact. Eg, 2011-12-24T06:40:20.005 => 2011-12-25T06:40:20.005
    /// </summary>
    /// <param name="current">The DateOnly to adjust</param>
    [Pure]
    public static DateOnly LastDayOfWeek(this DateOnly current) =>
        current.FirstDayOfWeek().AddDays(6);

    /// <summary>
    /// Returns the last day of the year keeping the time component intact. Eg, 2011-12-24T06:40:20.005 => 2011-12-31T06:40:20.005
    /// </summary>
    /// <param name="current">The DateOnly to adjust</param>
    [Pure]
    public static DateOnly LastDayOfYear(this DateOnly current) =>
        current.SetDate(current.Year, 12, 31);

    /// <summary>
    /// Returns the previous month keeping the time component intact. Eg, 2010-01-20T06:40:20.005 => 2009-12-20T06:40:20.005
    /// If the previous month doesn't have that many days the last day of the previous month is used. Eg, 2009-03-31T06:40:20.005 => 2009-02-28T06:40:20.005
    /// </summary>
    /// <param name="current">The DateOnly to adjust</param>
    [Pure]
    public static DateOnly PreviousMonth(this DateOnly current) {
        var year = current.Month == 1 ? current.Year - 1 : current.Year;

        var month = current.Month == 1 ? 12 : current.Month - 1;

        var firstDayOfPreviousMonth = current.SetDate(year, month, 1);

        var lastDayOfPreviousMonth = firstDayOfPreviousMonth.LastDayOfMonth().Day;

        var day = current.Day > lastDayOfPreviousMonth ? lastDayOfPreviousMonth : current.Day;

        return firstDayOfPreviousMonth.SetDay(day);
    }

    /// <summary>
    /// Returns the next month keeping the time component intact. Eg, 2012-12-05T06:40:20.005 => 2013-01-05T06:40:20.005
    /// If the next month doesn't have that many days the last day of the next month is used. Eg, 2013-01-31T06:40:20.005 => 2013-02-28T06:40:20.005
    /// </summary>
    /// <param name="current">The DateOnly to adjust</param>
    [Pure]
    public static DateOnly NextMonth(this DateOnly current) {

        var year = current.Month == 12 ? current.Year + 1 : current.Year;

        var month = current.Month == 12 ? 1 : current.Month + 1;

        var firstDayOfNextMonth = current.SetDate(year, month, 1);

        var lastDayOfPreviousMonth = firstDayOfNextMonth.LastDayOfMonth().Day;

        var day = current.Day > lastDayOfPreviousMonth ? lastDayOfPreviousMonth : current.Day;

        return firstDayOfNextMonth.SetDay(day);
    }

    /// <summary>
    /// Determines whether the specified <see cref="DateOnly"/> value is exactly the same day (day + month + year) then current
    /// </summary>
    /// <param name="current">The current value</param>
    /// <param name="date">Value to compare with</param>
    /// <returns>
    /// <c>true</c> if the specified date is exactly the same year then current; otherwise, <c>false</c>.
    /// </returns>
    [Pure]
    public static bool SameDay(this DateOnly current, DateOnly date) => current == date;

    /// <summary>
    /// Determines whether the specified <see cref="DateOnly"/> value is exactly the same month (month + year) then current. Eg, 2015-12-01 and 2014-12-01 => False
    /// </summary>
    /// <param name="current">The current value</param>
    /// <param name="date">Value to compare with</param>
    /// <returns>
    /// <c>true</c> if the specified date is exactly the same month and year then current; otherwise, <c>false</c>.
    /// </returns>
    [Pure]
    public static bool SameMonth(this DateOnly current, DateOnly date) =>
        current.Month == date.Month && current.Year == date.Year;

    /// <summary>
    /// Determines whether the specified <see cref="DateOnly"/> value is exactly the same year then current. Eg, 2015-12-01 and 2015-01-01 => True
    /// </summary>
    /// <param name="current">The current value</param>
    /// <param name="date">Value to compare with</param>
    /// <returns>
    /// <c>true</c> if the specified date is exactly the same date then current; otherwise, <c>false</c>.
    /// </returns>
    [Pure]
    public static bool SameYear(this DateOnly current, DateOnly date) =>
        current.Year == date.Year;
}