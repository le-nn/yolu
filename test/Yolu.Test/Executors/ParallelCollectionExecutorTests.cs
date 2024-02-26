using System.Collections.Concurrent;
using Yolu.Executors;

namespace Yolu.Test.Executors;

public class ParallelCollectionExecutorTests {
    [Fact]
    public async Task RunAsync_ShouldExecuteAllTasks() {
        // Arrange
        var sources = Enumerable.Range(0, 1000).Select(i => i).ToList();
        var executor = new ParallelCollectionExecutor<int>(sources, 100);
        var results = new BlockingCollection<int>();
        var random = new Random();

        // Act
        await executor.RunAsync(async i => {
            await Task.Delay(random.Next(100)); // Simulate work
            results.Add(i);
        });

        // Assert
        Assert.Equal(sources.Count, results.Count);
        Assert.Equal(sources.Count, executor.Progress);
        Assert.True(sources.All(t => results.Contains(t)));
    }

    [Fact]
    public async Task RunAsync_ShouldThrowIfInvokedTwice() {
        // Arrange
        var executor = new ParallelCollectionExecutor<int>([.. Enumerable.Range(0, 100)], 10);

        var errors = new BlockingCollection<Exception>();
        await executor.RunAsync(
            async i => {
                await Task.Delay(10);
                throw new Error("Error.");
            },
            error => {
                errors.Add(error);
            }
        );

        // Assert
        errors.Count.Should().Be(100);
        foreach (var error in errors) {
            error.Message.Should().Be("Error.");
        }
    }

    [Fact]
    public void Count_ShouldReturnCorrectNumberOfTasks() {
        // Arrange
        var tasks = Enumerable.Range(0, 100).Select(i => i).ToList();
        var executor = new ParallelCollectionExecutor<int>(tasks, 10);

        // Assert
        Assert.Equal(tasks.Count, executor.Count);
    }
}