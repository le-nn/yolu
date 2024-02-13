namespace Hadron.Test;

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
}
