namespace Yolu.Test;

public class ForeachTests {
    [Fact]
    public void Foreach_ShouldEnumerateAllElements() {
        // Arrange
        var range = new Range(0, ^1000);
        var expected = Enumerable.Range(0, 1000);

        // Act
        var actual = new List<int>();
        foreach (var i in range) {
            actual.Add(i);
        }

        // Assert
        expected.Should().BeEquivalentTo(actual);
    }

    [Fact]
    public void Foreach_ShouldEnumerateAllElements2() {
        // Arrange
        var range = 0..^1000;
        var expected = Enumerable.Range(0, 1000);

        // Act
        var actual = new List<int>();
        foreach (var i in range) {
            actual.Add(i);
        }

        // Assert
        expected.Should().BeEquivalentTo(actual);
    }

    [Fact]
    public void Foreach_ShouldEnumerateAllElements3() {
        // Act
        var actual = new List<int>();
        foreach (var i in 0..10) {
            actual.Add(i);
        }

        // Assert
        actual.Should().BeEquivalentTo((int[])[0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10]);
    }

    [Fact]
    public void Foreach_ShouldEnumerateAllElements4() {
        // Act
        var actual = new List<int>();
        foreach (var i in 5..10) {
            actual.Add(i);
        }

        // Assert
        actual.Should().BeEquivalentTo((int[])[5, 6, 7, 8, 9, 10]);
    }

    [Fact]
    public void Foreach_ShouldEnumerateAllElements5() {
        // Act
        var actual = new List<int>();
        foreach (var i in 0..^10) {
            actual.Add(i);
        }

        // Assert
        actual.Should().BeEquivalentTo((int[])[0, 1, 2, 3, 4, 5, 6, 7, 8, 9]);
    }

    [Fact]
    public void Foreach_ShouldEnumerateAllElements6() {
        // Act
        var actual = new List<int>();
        foreach (var i in 5..^10) {
            actual.Add(i);
        }

        // Assert
        actual.Should().BeEquivalentTo((int[])[5, 6, 7, 8, 9]);
    }
}
