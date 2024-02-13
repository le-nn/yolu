using Hadron.DateTimes;

public class FluentDateTests {
    private static readonly Dictionary<int, Func<int, int, DateOnly>> _fluentDatesFull = new() {
        [1] = (year, day) => year.January(day),
        [2] = (year, day) => year.February(day),
        [3] = (year, day) => year.March(day),
        [4] = (year, day) => year.April(day),
        [5] = (year, day) => year.May(day),
        [6] = (year, day) => year.June(day),
        [7] = (year, day) => year.July(day),
        [8] = (year, day) => year.August(day),
        [9] = (year, day) => year.September(day),
        [10] = (year, day) => year.October(day),
        [11] = (year, day) => year.November(day),
        [12] = (year, day) => year.December(day)
    };
    private static Dictionary<int, Func<int, int, DateOnly>> _fluentDatesShort = new() {
        [1] = (year, day) => year.Jan(day),
        [2] = (year, day) => year.Feb(day),
        [3] = (year, day) => year.Mar(day),
        [4] = (year, day) => year.Apr(day),
        [5] = (year, day) => year.May(day),
        [6] = (year, day) => year.Jun(day),
        [7] = (year, day) => year.Jul(day),
        [8] = (year, day) => year.Aug(day),
        [9] = (year, day) => year.Sep(day),
        [10] = (year, day) => year.Oct(day),
        [11] = (year, day) => year.Nov(day),
        [12] = (year, day) => year.Dec(day)
    };

    public static IEnumerable<object[]> FluentDates => _fluentDatesFull.Concat(_fluentDatesShort).Select(x => new object[]
    {
        x.Key,
        x.Value
    });

    [Theory]
    [MemberData(nameof(FluentDates))]
    public void Dates(int month, Func<int, int, DateOnly> getFluentDate) {
        var date = GetRandomDateTimeOfMonth(month).DateTime;
        var fluentDate = getFluentDate(date.Year, date.Day);

        Assert.Equal(date.Date.GetDateOnly(), fluentDate);
    }

    private static DateTimeOffset GetRandomDateTimeOfMonth(int month) {
        var random = new Random();

        var year = random.Next(1, 10000);
        return new(
            year,
            month,
            random.Next(1, DateTime.DaysInMonth(year, month) + 1),
            random.Next(0, 24),
            random.Next(0, 60),
            random.Next(0, 60),
            random.Next(0, 1000),
            TimeSpan.FromMinutes(random.Next((int)-12.Hours().TotalMinutes, (int)14.Hours().TotalMinutes + 1))
        );
    }
}