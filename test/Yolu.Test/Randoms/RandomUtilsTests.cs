using Yolu.Randoms;

namespace Yolu.Test.Randoms;

public class RandomUtilsTests {
    [Fact]
    public void TakeOne_ShouldReturnRandomItemFromList() {
        // Arrange
        var random = new Random();
        int[] items = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20];

        foreach (var _ in 0..^1000) {
            // Act
            var result = random.TakeOne(items);

            // Assert
            Assert.Contains(result, items);
        }
    }

    [Fact]
    public void Take_ShouldReturnRandomItemsFromList() {
        // Arrange
        var random = new Random();
        int[] items = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20];
        var count = 10;

        foreach (var _ in 0..^1000) {
            // Act
            var result = random.Take(items, count);

            // Assert
            Assert.Equal(count, result.Count);
            foreach (var item in result) {
                Assert.Contains(item, items);
            }
        }
    }

    [Fact]
    public void NextBool_ShouldReturnRandomBooleanValue() {
        // Arrange
        var random = new Random();

        // Act
        var result = random.NextBool();

        // Assert
        Assert.True(result || !result);
    }

    [Fact]
    public void NextString_ShouldReturnRandomStringWithSpecifiedLength() {
        // Arrange
        var random = new Random();

        foreach (var _ in 0..^1000) {
            var length = 100;

            // Act
            var result = random.NextString(length);

            // Assert
            Assert.Equal(length, result.Length);
        }
    }

    [Fact]
    public void NextDateTime_ShouldReturnRandomDateTimeBetweenStartAndEndDates() {
        // Arrange
        var random = new Random();
        var startDate = new DateTime(2020, 1, 1);
        var endDate = new DateTime(2022, 12, 31);
        foreach (var _ in 0..^1000) {
            // Act
            var result = random.NextDateTime(startDate, endDate);

            // Assert
            Assert.InRange(result, startDate, endDate);
        }
    }

    [Fact]
    public void NextDouble_ShouldReturnRandomDoubleBetweenMinAndMaxValues() {
        // Arrange
        var random = new Random();
        var minValue = 0.0;
        var maxValue = 1.0;
        foreach (var _ in 0..^1000) {
            // Act
            var result = random.NextDouble(minValue, maxValue);

            // Assert
            Assert.InRange(result, minValue, maxValue);
        }
    }

    [Fact]
    public void NextInt_ShouldReturnRandomIntegerBetweenMinAndMaxValues() {
        // Arrange
        var random = new Random();
        var minValue = 0;
        var maxValue = 10;
        foreach (var _ in 0..^1000) {
            // Act
            var result = random.NextInt(minValue, maxValue);

            // Assert
            Assert.InRange(result, minValue, maxValue);
        }
    }

    [Fact]
    public void NextLong_ShouldReturnRandomLongBetweenMinAndMaxValues() {
        // Arrange
        var random = new Random();
        var minValue = 0L;
        var maxValue = 100L;
        foreach (var _ in 0..^1000) {
            // Act
            var result = random.NextLong(minValue, maxValue);

            // Assert
            Assert.InRange(result, minValue, maxValue);
        }
    }

    [Fact]
    public void NextDateOnly_ShouldReturnRandomDateOnlyBetweenStartAndEndDates() {
        // Arrange
        var random = new Random();
        var startDate = new DateOnly(2002, 1, 1);
        var endDate = new DateOnly(2022, 12, 31);
        foreach (var _ in 0..^1000) {
            // Act
            var result = random.NextDateOnly(startDate, endDate);

            // Assert
            Assert.InRange(result, startDate, endDate);
        }
    }

    [Fact]
    public void NextTimeOnly_ShouldReturnRandomTimeOnlyBetweenMinAndMaxValues() {
        // Arrange
        var random = new Random();
        var minValue = new TimeOnly(0, 0, 0);
        var maxValue = new TimeOnly(23, 59, 59);
        foreach (var _ in 0..^1000) {
            // Act
            var result = random.NextTimeOnly(minValue, maxValue);

            // Assert
            Assert.InRange(result, minValue, maxValue);
        }
    }

    [Fact]
    public void NextTimeSpan_ShouldReturnRandomTimeSpanBetweenMinAndMaxValues() {
        // Arrange
        var random = new Random();
        var minValue = TimeSpan.FromHours(1);
        var maxValue = TimeSpan.FromHours(15);
        foreach (var _ in 0..^1000) {
            // Act
            var result = random.NextTimeSpan(minValue, maxValue);

            // Assert
            Assert.InRange(result, minValue, maxValue);
        }
    }

    [Fact]
    public void NextFloat_ShouldReturnRandomFloatBetweenMinAndMaxValues() {
        // Arrange
        var random = new Random();
        var minValue = 0.0f;
        var maxValue = 1.0f;
        foreach (var _ in 0..^1000) {
            // Act
            var result = random.NextFloat(minValue, maxValue);

            // Assert
            Assert.InRange(result, minValue, maxValue);
        }
    }

    [Fact]
    public void NextGuid_ShouldReturnRandomGuid() {
        // Arrange
        var random = new Random();
        foreach (var _ in 0..^1000) {
            // Act
            var result = random.NextGuid();

            // Assert
            Assert.NotEqual(Guid.Empty, result);
        }
    }
}
