using System;
using Yolu.Collections;
using Yolu.Executors;
using Yolu.Randoms;

namespace Yolu.Test.Executors;

public class ConcatOperationExecutorTest {
    [Fact]
    public async Task RunTest1() {
        var executor = new ConcatAsyncOperationExecutor();
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
        var executor = new ConcatAsyncOperationExecutor();
        var results = new List<int>();

        var task1 = executor.ExecuteAsync(async () => {
            await Task.Delay(1);
            results.Add(1);
            return 0;
        });

        var task2 = executor.ExecuteAsync(async () => {
            await Task.Delay(1);
            results.Add(2);
            return 0;
        });

        var task3 = executor.ExecuteAsync(async () => {
            await Task.Delay(1);
            results.Add(3);

            return 0;
        });

        var task4 = executor.ExecuteAsync(async () => {
            await Task.Delay(2);
            results.Add(4);

            return 0;
        });

        var task5 = executor.ExecuteAsync(async () => {
            await Task.Delay(3);
            results.Add(5);

            return 0;
        });

        var task6 = executor.ExecuteAsync(async () => {
            await Task.Delay(1);
            results.Add(6);

            return 0;
        });

        var task7 = executor.ExecuteAsync(async () => {
            await Task.Delay(1);
            results.Add(7);

            return 0;
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
    public async Task Ensure_OrderOfExecution() {
        var executor = new ConcatAsyncOperationExecutor();
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
        var executor = new ConcatAsyncOperationExecutor();
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
    public async Task Test2() {
        foreach (var i in 0..1000) {
            await Task.Delay(2);
        }
    }
}