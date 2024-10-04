using System.Collections.Immutable;
using Yolu.Collections;

namespace Yolu.Core.Collections.Tests;

public partial class MutableArrayTests {
    [Fact]
    public void MutableArray_Create_Empty_MutableArray() {
        // Arrange
        var array = new MutableArray<int>();

        // Act
        var result = array.IsEmpty;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void MutableArray_CreateEmpty_FromCollectionExpression() {
        // Arrange
        MutableArray<int> array = [];

        // Act
        var result = array.IsEmpty;

        // Assert
        Assert.True(result);
    }


    [Fact]
    public void MutableArray_InitializedWithMutableArray_ShouldHaveCorrectLengthAndElements() {
        // Arrange
        int[] values = [1, 2, 3, 4, 5];

        // Act
        var array = new MutableArray<int>(values);

        // Assert
        Assert.Equal(values.Length, array.Count);
        for (var i = 0; i < values.Length; i++) {
            Assert.Equal(values[i], array[i]);
        }
    }

    [Fact]
    public void MutableArray_InitializedWithReadOnlySpan_ShouldHaveCorrectLengthAndElements() {
        // Arrange
        int[] values = [1, 2, 3, 4, 5];
        ReadOnlySpan<int> span = values.AsSpan();

        // Act
        var array = new MutableArray<int>(span);

        // Assert
        Assert.Equal(values.Length, array.Length);
        for (var i = 0; i < values.Length; i++) {
            Assert.Equal(values[i], array[i]);
        }
    }

    [Fact]
    public void MutableArray_InitializedWithCollection_ShouldHaveCorrectLengthAndElements() {
        // Arrange
        var values = new List<int> { 1, 2, 3, 4, 5 };

        // Act
        var array = new MutableArray<int>(values);

        // Assert
        Assert.Equal(values.Count, array.Length);
        for (var i = 0; i < values.Count; i++) {
            Assert.Equal(values[i], array[i]);
        }
    }

    [Fact]
    public void MutableArray_GetEnumerator_ShouldEnumerateAllElements() {
        // Arrange
        int[] values = [1, 2, 3, 4, 5];
        var array = new MutableArray<int>(values);

        // Act
        var result = array.ToList();

        // Assert
        Assert.Equal(values.Length, result.Count);
        for (var i = 0; i < values.Length; i++) {
            Assert.Equal(values[i], result[i]);
        }
    }

    [Fact]
    public void MutableArray_Equals_ShouldReturnFalseForDifferentArrays() {
        // Arrange
        int[] values1 = [1, 2, 3, 4, 5];
        int[] values2 = [5, 4, 3, 2, 1];
        var array1 = new MutableArray<int>(values1);
        var array2 = new MutableArray<int>(values2);

        // Act
        var result = array1.Equals(array2);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void MutableArray_IsEmpty_ShouldReturnTrueForEmptyArray() {
        // Arrange
        var array = MutableArray<int>.Empty;

        // Act
        var result = array.IsEmpty;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void MutableArray_IsEmpty_ShouldReturnFalseForNonEmptyArray() {
        // Arrange
        int[] values = [1, 2, 3, 4, 5];
        var array = new MutableArray<int>(values);

        // Act
        var result = array.IsEmpty;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void MutableArray_Length_ShouldReturnCorrectLength() {
        // Arrange
        int[] values = [1, 2, 3, 4, 5];
        var array = new MutableArray<int>(values);

        // Act
        var result = array.Length;

        // Assert
        Assert.Equal(values.Length, result);
    }

    [Fact]
    public void MutableArray_Count_ShouldReturnCorrectCount() {
        // Arrange
        int[] values = [1, 2, 3, 4, 5];
        var array = new MutableArray<int>(values);

        // Act
        var result = array.Count;

        // Assert
        Assert.Equal(values.Length, result);
    }

    [Fact]
    public void List_to_MutableArray_Conversion_ShouldReturnCorrectArray() {
        // Arrange
        List<int> values = [1, 2, 3, 4, 5];

        // Act
        var array = new MutableArray<int>(values);

        // Assert
        Assert.Equal(values.Count, array.Length);
        for (var i = 0; i < values.Count; i++) {
            Assert.Equal(values[i], array[i]);
        }
    }
}
