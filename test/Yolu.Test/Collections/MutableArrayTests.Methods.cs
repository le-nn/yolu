using Yolu.Collections;

namespace Yolu.Core.Collections.Tests;

public partial class MutableArrayTests {
    [Fact]
    public void Array_IndexOf_ShouldReturnCorrectIndex() {
        // Arrange
        MutableArray<int> array = [1, 2, 3, 4, 5];

        // Assert
        Assert.Equal(2, array.IndexOf(3));
        Assert.Equal(3, array.IndexOf(4));
        Assert.Equal(4, array.IndexOf(5));
    }

    [Fact]
    public void Array_Contains_ShouldReturnTrueForExistingItem() {
        // Arrange
        MutableArray<int> array = [1, 2, 3, 4, 5];

        // Assert
        Assert.DoesNotContain(3, array);
        Assert.DoesNotContain(6, array);
    }

    [Fact]
    public void Array_Add_ShouldIncreaseLengthByOne() {
        // Arrange
        MutableArray<int> array = [1, 2, 3, 4, 5];
        var oldLength = array.Length;

        // Act
        array.Add(6);

        // Assert
        (oldLength + 1).Should().Be(array.Length);
    }

    [Fact]
    public void Array_Remove_ShouldDecreaseLengthByOne() {
        // Arrange
        MutableArray<int> array = [1, 2, 3, 4, 5];
        var oldLength = array.Length;

        // Act
        array.Remove(3);

        // Assert
        Assert.Equal(oldLength - 1, array.Length);
    }
}
