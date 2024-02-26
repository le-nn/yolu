using System.Diagnostics.Contracts;

namespace Yolu.DateTimes;

public static class IntExtensions {
    /// <summary>
    /// Returns the specified day in January of the given year.
    /// </summary>
    /// <param name="year">
    /// The year (1 through 9999).
    /// </param>
    /// <param name="day">
    /// The day (1 through 31).
    /// </param>
    [Pure]
    public static DateOnly January(this int year, int day) =>
        new(year, 1, day);

    /// <summary>
    /// Returns the specified day in January of the given year.
    /// </summary>
    /// <param name="year">
    /// The year (1 through 9999).
    /// </param>
    /// <param name="day">
    /// The day (1 through 31).
    /// </param>
    [Pure]
    public static DateOnly Jan(this int year, int day) => year.January(day);

    /// <summary>
    /// Returns the specified day in February of the given year.
    /// </summary>
    /// <param name="year">
    /// The year (1 through 9999).
    /// </param>
    /// <param name="day">
    /// The day (1 through 28 in a common year, and 29 in leap years).
    /// </param>
    [Pure]
    public static DateOnly February(this int year, int day) =>
        new(year, 2, day);

    /// <summary>
    /// Returns the specified day in February of the given year.
    /// </summary>
    /// <param name="year">
    /// The year (1 through 9999).
    /// </param>
    /// <param name="day">
    /// The day (1 through 28 in a common year, and 29 in leap years).
    /// </param>
    [Pure]
    public static DateOnly Feb(this int year, int day) => year.February(day);

    /// <summary>
    /// Returns the specified day in March of the given year.
    /// </summary>
    /// <param name="year">
    /// The year (1 through 9999).
    /// </param>
    /// <param name="day">
    /// The day (1 through 31).
    /// </param>
    [Pure]
    public static DateOnly March(this int year, int day) =>
        new(year, 3, day);

    /// <summary>
    /// Returns the specified day in March of the given year.
    /// </summary>
    /// <param name="year">
    /// The year (1 through 9999).
    /// </param>
    /// <param name="day">
    /// The day (1 through 31).
    /// </param>
    [Pure]
    public static DateOnly Mar(this int year, int day) => year.March(day);

    /// <summary>
    /// Returns the specified day in April of the given year.
    /// </summary>
    /// <param name="year">
    /// The year (1 through 9999).
    /// </param>
    /// <param name="day">
    /// The day (1 through 30).
    /// </param>
    [Pure]
    public static DateOnly April(this int year, int day) =>
        new(year, 4, day);

    /// <summary>
    /// Returns the specified day in April of the given year.
    /// </summary>
    /// <param name="year">
    /// The year (1 through 9999).
    /// </param>
    /// <param name="day">
    /// The day (1 through 30).
    /// </param>
    [Pure]
    public static DateOnly Apr(this int year, int day) => year.April(day);

    /// <summary>
    /// Returns the specified day in May of the given year.
    /// </summary>
    /// <param name="year">
    /// The year (1 through 9999).
    /// </param>
    /// <param name="day">
    /// The day (1 through 31).
    /// </param>
    [Pure]
    public static DateOnly May(this int year, int day) =>
        new(year, 5, day);

    /// <summary>
    /// Returns the specified day in June of the given year.
    /// </summary>
    /// <param name="year">
    /// The year (1 through 9999).
    /// </param>
    /// <param name="day">
    /// The day (1 through 30).
    /// </param>
    [Pure]
    public static DateOnly June(this int year, int day) =>
        new(year, 6, day);

    /// <summary>
    /// Returns the specified day in June of the given year.
    /// </summary>
    /// <param name="year">
    /// The year (1 through 9999).
    /// </param>
    /// <param name="day">
    /// The day (1 through 30).
    /// </param>
    [Pure]
    public static DateOnly Jun(this int year, int day) => year.June(day);

    /// <summary>
    /// Returns the specified day in July of the given year.
    /// </summary>
    /// <param name="year">
    /// The year (1 through 9999).
    /// </param>
    /// <param name="day">
    /// The day (1 through 31).
    /// </param>
    [Pure]
    public static DateOnly July(this int year, int day) =>
        new(year, 7, day);

    /// <summary>
    /// Returns the specified day in July of the given year.
    /// </summary>
    /// <param name="year">
    /// The year (1 through 9999).
    /// </param>
    /// <param name="day">
    /// The day (1 through 31).
    /// </param>
    [Pure]
    public static DateOnly Jul(this int year, int day) => year.July(day);

    /// <summary>
    /// Returns the specified day in August of the given year.
    /// </summary>
    /// <param name="year">
    /// The year (1 through 9999).
    /// </param>
    /// <param name="day">
    /// The day (1 through 31).
    /// </param>
    [Pure]
    public static DateOnly August(this int year, int day) =>
        new(year, 8, day);

    /// <summary>
    /// Returns the specified day in August of the given year.
    /// </summary>
    /// <param name="year">
    /// The year (1 through 9999).
    /// </param>
    /// <param name="day">
    /// The day (1 through 31).
    /// </param>
    [Pure]
    public static DateOnly Aug(this int year, int day) => year.August(day);

    /// <summary>
    /// Returns the specified day in September of the given year.
    /// </summary>
    /// <param name="year">
    /// The year (1 through 9999).
    /// </param>
    /// <param name="day">
    /// The day (1 through 30).
    /// </param>
    [Pure]
    public static DateOnly September(this int year, int day) =>
        new(year, 9, day);

    /// <summary>
    /// Returns the specified day in September of the given year.
    /// </summary>
    /// <param name="year">
    /// The year (1 through 9999).
    /// </param>
    /// <param name="day">
    /// The day (1 through 30).
    /// </param>
    [Pure]
    public static DateOnly Sep(this int year, int day) => year.September(day);

    /// <summary>
    /// Returns the specified day in October of the given year.
    /// </summary>
    /// <param name="year">
    /// The year (1 through 9999).
    /// </param>
    /// <param name="day">
    /// The day (1 through 31).
    /// </param>
    [Pure]
    public static DateOnly October(this int year, int day) =>
        new(year, 10, day);

    /// <summary>
    /// Returns the specified day in October of the given year.
    /// </summary>
    /// <param name="year">
    /// The year (1 through 9999).
    /// </param>
    /// <param name="day">
    /// The day (1 through 31).
    /// </param>
    [Pure]
    public static DateOnly Oct(this int year, int day) => year.October(day);

    /// <summary>
    /// Returns the specified day in November of the given year.
    /// </summary>
    /// <param name="year">
    /// The year (1 through 9999).
    /// </param>
    /// <param name="day">
    /// The day (1 through 30).
    /// </param>
    [Pure]
    public static DateOnly November(this int year, int day) =>
        new(year, 11, day);

    /// <summary>
    /// Returns the specified day in November of the given year.
    /// </summary>
    /// <param name="year">
    /// The year (1 through 9999).
    /// </param>
    /// <param name="day">
    /// The day (1 through 30).
    /// </param>
    [Pure]
    public static DateOnly Nov(this int year, int day) => year.November(day);

    /// <summary>
    /// Returns the specified day in December of the given year.
    /// </summary>
    /// <param name="year">
    /// The year (1 through 9999).
    /// </param>
    /// <param name="day">
    /// The day (1 through 31).
    /// </param>
    [Pure]
    public static DateOnly December(this int year, int day) =>
        new(year, 12, day);

    /// <summary>
    /// Returns the specified day in December of the given year.
    /// </summary>
    /// <param name="year">
    /// The year (1 through 9999).
    /// </param>
    /// <param name="day">
    /// The day (1 through 31).
    /// </param>
    [Pure]
    public static DateOnly Dec(this int year, int day) => year.December(day);
}