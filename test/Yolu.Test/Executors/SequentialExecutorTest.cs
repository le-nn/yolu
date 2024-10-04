using System;
using System.Collections.Concurrent;
using Yolu.Collections;
using Yolu.Executors;
using Yolu.Randoms;

namespace Yolu.Test.Executors;

public class SequentialExecutorTest {
    [Fact]
    public async Task RunTest1() {
        var executor = new SequentialExecutor();
        var results = new List<int>();

        var task1 = executor.ExecuteAsync(async () => {
            await Task.Delay(50);
            results.Add(1);
        });

        var task2 = executor.ExecuteAsync(async () => {
            await Task.Delay(10);
            results.Add(2);
        });

        var task3 = executor.ExecuteAsync(async () => {
            await Task.Delay(60);
            results.Add(3);
        });

        var task4 = executor.ExecuteAsync(async () => {
            await Task.Delay(340);
            results.Add(4);
        });

        var task5 = executor.ExecuteAsync(async () => {
            await Task.Delay(5);
            results.Add(5);
        });

        var task6 = executor.ExecuteAsync(async () => {
            await Task.Delay(160);
            results.Add(6);
        });

        var task7 = executor.ExecuteAsync(async () => {
            await Task.Delay(70);
            results.Add(7);
        });

        await task1;
        await task2;
        await task3;
        await task4;
        await task5;
        await task6;
        await task7;

        Assert.True(results is [1, 2, 3, 4, 5, 6, 7]);
    }

    [Fact]
    public async Task RunTest2() {
        var executor = new SequentialExecutor();
        var results = new List<int>();
        int? executorThreadId = null;

        var task1 = executor.ExecuteAsync(async () => {
            await Task.Delay(1);
            results.Add(1);
            executorThreadId = Thread.CurrentThread.ManagedThreadId;
        });

        var task2 = executor.ExecuteAsync(async () => {
            await Task.Delay(1);
            results.Add(2);
            Assert.Equal(executorThreadId, Thread.CurrentThread.ManagedThreadId);
        });

        var task3 = executor.ExecuteAsync(async () => {
            await Task.Delay(1);
            results.Add(3);
            Assert.Equal(executorThreadId, Thread.CurrentThread.ManagedThreadId);
        });

        var task4 = executor.ExecuteAsync(async () => {
            await Task.Delay(2);
            results.Add(4);
            Assert.Equal(executorThreadId, Thread.CurrentThread.ManagedThreadId);
        });

        var task5 = executor.ExecuteAsync(async () => {
            await Task.Delay(3);
            results.Add(5);
            Assert.Equal(executorThreadId, Thread.CurrentThread.ManagedThreadId);
        });

        var task6 = executor.ExecuteAsync(async () => {
            await Task.Delay(1);
            results.Add(6);
            Assert.Equal(executorThreadId, Thread.CurrentThread.ManagedThreadId);
        });

        var task7 = executor.ExecuteAsync(async () => {
            await Task.Delay(1);
            results.Add(7);
            Assert.Equal(executorThreadId, Thread.CurrentThread.ManagedThreadId);
        });

        await Task.WhenAll(task1, task2, task3, task4, task5, task6, task7);

        Assert.True(results is [1, 2, 3, 4, 5, 6, 7]);

        // Clean up
        executor.Dispose();
    }

    class OP {
        int cursor = 0;
        Random random = new Random();
        int _callCount;

        public int CallCount => _callCount;

        public async Task DoAsync(int i) {
            if (cursor is not 0) {
                Assert.Fail("cursor is not 0");
            }

            Interlocked.Increment(ref _callCount);

            Interlocked.Increment(ref cursor);
            Thread.Sleep(random.Next(1, 3));

            Interlocked.Decrement(ref cursor);
        }

    }

    [Fact]
    public async Task Ensure_OrderOfExecution_Concurrent() {
        foreach (var i in 0..^100) {
            var executor = new SequentialExecutor();
            var op = new OP();
            var random = new Random();

            await Parallel.ForAsync(0, 10, async (it, t) => {
                var results2 = new List<Task>();
                await executor.ExecuteAsync(async () => {
                    await Task.Run(() => op.DoAsync(i));
                    await Task.Run(() => op.DoAsync(i));
                });
            });
        }
    }

    [Fact]
    public async Task Ensure_OrderOfExecution() {
        var executor = new SequentialExecutor();
        var results = new MutableArray<Task<int>>();
        var random = new Random();

        foreach (var i in 0..1000) {
            var task = executor.ExecuteAsync(async () => {
                await Task.Delay(random.Next(1, 5));
                return i * 2;
            });
            results.Add(task);
        }


        foreach (var i in 0..1000) {
            var result = await results[i];

            Assert.Equal(i * 2, result);
        }
    }

    [Fact]
    public async Task Ensure_OrderOfExecution_With_TaskWhenAll() {
        var executor = new SequentialExecutor();
        var results = new MutableArray<Task<int>>();
        var random = new Random();

        foreach (var i in 0..1000) {
            var task = executor.ExecuteAsync(async () => {
                await Task.Delay(random.Next(1, 5));
                return i * 2;
            });
            results.Add(task);
        }

        await Task.WhenAll(results);

        foreach (var i in 0..1000) {
            Assert.Equal(i * 2, results[i].Result);
        }
    }

    [Fact]
    public async Task Ensure_Exception() {
        var executor = new SequentialExecutor();
        var results = new MutableArray<int>();

        await Assert.ThrowsAsync<Exception>(async () => {
            await executor.ExecuteAsync(() => {
                throw new Exception("Test");
            });
        });

        await Assert.ThrowsAsync<InvalidOperationException>(async () => {
            await executor.ExecuteAsync(() => {
                throw new InvalidOperationException("Test");
            });
        });

    }
}