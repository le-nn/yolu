using Yolu.Executors;

namespace Yolu.Test.Executors;

public class ThrottledExecutorTest {
    [Fact]
    public void Invoke_ShouldExecuteImmediately_WhenLatencyIsZero() {
        var executor = new ThrottledExecutor<int> { LatencyMs = 0 };
        var observer = new TestObserver<int>();
        using var d = executor.Subscribe(observer);

        executor.Invoke(42);

        Assert.Equal(42, observer.LastValue);
    }

    [Fact]
    public async Task Invoke_ShouldThrottleExecution() {
        var executor = new ThrottledExecutor<int> { LatencyMs = 100 };
        var observer = new TestObserver<int>();
        using var d = executor.Subscribe(observer);

        executor.Invoke(1);
        executor.Invoke(2);

        await Task.Delay(150);

        Assert.Equal(2, observer.LastValue);
    }

    [Fact]
    public async Task Invoke_ShouldNotifyAllObservers() {
        var executor = new ThrottledExecutor<int> { LatencyMs = 0 };
        var observer1 = new TestObserver<int>();
        var observer2 = new TestObserver<int>();
        executor.Subscribe(observer1);
        executor.Subscribe(observer2);

        executor.Invoke(42);

        await Task.Delay(50);

        Assert.Equal(42, observer1.LastValue);
        Assert.Equal(42, observer2.LastValue);
    }


    [Fact]
    public async Task Invoke_ShouldCancel() {
        ushort latency = 100;

        var executor = new ThrottledExecutor<int> { LatencyMs = latency };
        var observer = new TestObserver<int>();
        using var d = executor.Subscribe(observer);

        executor.Invoke(1);
        await Task.Delay(10);
        executor.Invoke(2);
        await Task.Delay(10);
        executor.Invoke(3);

        await Task.Delay(latency);
        Assert.Equal(3, observer.LastValue);

        executor.Invoke(4);
        executor.Invoke(5);
        executor.Invoke(6);
        Assert.NotEqual(6, observer.LastValue);
        await Task.Delay(latency + 10);
        Assert.Equal(6, observer.LastValue);
    }

    private class TestObserver<T> : IObserver<T> {
        public T LastValue { get; private set; }

        public void OnCompleted() { }

        public void OnError(Exception error) { }

        public void OnNext(T value) {
            LastValue = value;
        }
    }
}