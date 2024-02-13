using Hadron.Collections;

namespace Hadron.Core.Collections.Tests;

public partial class ArrayMethodsTests {
    [Fact]
    public void Array_IndexOf_ShouldReturnCorrectIndex() {
        // Arrange
        Array<int> array = [1, 2, 3, 4, 5];

        // Assert
        Assert.Equal(2, array.IndexOf(3));
        Assert.Equal(3, array.IndexOf(4));
        Assert.Equal(4, array.IndexOf(5));
    }

    [Fact]
    public void Array_Contains_ShouldReturnTrueForExistingItem() {
        // Arrange
        Array<int> array = [1, 2, 3, 4, 5];

        // Assert
        Assert.True(array.Contains(3));
        Assert.False(array.Contains(6));
    }

    [Fact]
    public void Array_Add_ShouldIncreaseLengthByOne() {
        // Arrange
        Array<int> array = [1, 2, 3, 4, 5];
        var oldLength = array.Length;

        // Act
        array = array.Add(6);

        // Assert
        (oldLength + 1).Should().Be(array.Length);
    }

    [Fact]
    public void Array_Remove_ShouldDecreaseLengthByOne() {
        // Arrange
        Array<int> array = [1, 2, 3, 4, 5];
        var oldLength = array.Length;

        // Act
        array = array.Remove(3);

        // Assert
        Assert.Equal(oldLength - 1, array.Length);
    }
}
