using Yolu.Threading.Tasks;

namespace Yolu.Test.Threading.Tasks;

public class SerialCancellationTaskManagerTest {
    [Fact]
    public async Task ExecuteAsync_ShouldRunTask() {
        var executor = new SerialCancellationTaskManager();
        var result = 0;

        await executor.ExecuteAsync(async token => {
            await Task.Delay(50, token);
            result = 1;
        });

        Assert.Equal(1, result);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldCancelPreviousTask() {
        var executor = new SerialCancellationTaskManager();
        var result = 0;

        var task1 = executor.ExecuteAsync(async token => {
            await Task.Delay(100, token);
            result = 1;
        });

        var task2 = executor.ExecuteAsync(async token => {
            await Task.Delay(50, token);
            result = 2;
        });

        await task2;

        Assert.Equal(2, result);
        await Assert.ThrowsAsync<TaskCanceledException>(() => task1);
    }
}