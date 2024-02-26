using Yolu.Collections;

namespace Yolu.Core.Collections.Tests;

public class ArrayTests {
    [Fact]
    public void Array_InitializedWithArray_ShouldHaveCorrectLengthAndElements() {
        // Arrange
        int[] values = [1, 2, 3, 4, 5];

        // Act
        var array = new Array<int>(values);

        // Assert
        Assert.Equal(values.Length, array.Length);
        for (var i = 0; i < values.Length; i++) {
            Assert.Equal(values[i], array[i]);
        }
    }

    [Fact]
    public void Array_InitializedWithReadOnlySpan_ShouldHaveCorrectLengthAndElements() {
        // Arrange
        int[] values = [1, 2, 3, 4, 5];
        ReadOnlySpan<int> span = values.AsSpan();

        // Act
        var array = new Array<int>(span);

        // Assert
        Assert.Equal(values.Length, array.Length);
        for (var i = 0; i < values.Length; i++) {
            Assert.Equal(values[i], array[i]);
        }
    }

    [Fact]
    public void Array_InitializedWithCollection_ShouldHaveCorrectLengthAndElements() {
        // Arrange
        var values = new List<int> { 1, 2, 3, 4, 5 };

        // Act
        var array = new Array<int>(values);

        // Assert
        Assert.Equal(values.Count, array.Length);
        for (var i = 0; i < values.Count; i++) {
            Assert.Equal(values[i], array[i]);
        }
    }

    [Fact]
    public void Array_GetEnumerator_ShouldEnumerateAllElements() {
        // Arrange
        int[] values = [1, 2, 3, 4, 5];
        var array = new Array<int>(values);

        // Act
        var result = array.ToList();

        // Assert
        Assert.Equal(values.Length, result.Count);
        for (var i = 0; i < values.Length; i++) {
            Assert.Equal(values[i], result[i]);
        }
    }

    [Fact]
    public void Array_Equals_ShouldReturnFalseForDifferentArrays() {
        // Arrange
        int[] values1 = [1, 2, 3, 4, 5];
        int[] values2 = [5, 4, 3, 2, 1];
        var array1 = new Array<int>(values1);
        var array2 = new Array<int>(values2);

        // Act
        var result = array1.Equals(array2);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Array_IsEmpty_ShouldReturnTrueForEmptyArray() {
        // Arrange
        var array = Array<int>.Empty;

        // Act
        var result = array.IsEmpty;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Array_IsEmpty_ShouldReturnFalseForNonEmptyArray() {
        // Arrange
        int[] values = [1, 2, 3, 4, 5];
        var array = new Array<int>(values);

        // Act
        var result = array.IsEmpty;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Array_Length_ShouldReturnCorrectLength() {
        // Arrange
        int[] values = [1, 2, 3, 4, 5];
        var array = new Array<int>(values);

        // Act
        var result = array.Length;

        // Assert
        Assert.Equal(values.Length, result);
    }

    [Fact]
    public void Array_Count_ShouldReturnCorrectCount() {
        // Arrange
        int[] values = [1, 2, 3, 4, 5];
        var array = new Array<int>(values);

        // Act
        var result = array.Count;

        // Assert
        Assert.Equal(values.Length, result);
    }

    [Fact]
    public void Array_IsDefault_ShouldReturnTrueForUninitializedArray() {
        // Arrange
        var array = new Array<int>(null);

        // Act
        var result = array.IsDefault;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Array_IsDefault_ShouldReturnFalseForInitializedArray() {
        // Arrange
        int[] values = [1, 2, 3, 4, 5];
        var array = new Array<int>(values);

        // Act
        var result = array.IsDefault;

        // Assert
        Assert.False(result);
    }
}
