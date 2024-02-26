using BenchmarkDotNet.Attributes;
using Yolu.Benchmark.Collections.Internal;
using Yolu.Collections;

namespace Yolu.Benchmark.Collections.Arrays;

public class GetEnumerator_Benchmark {
    private Array<int>? _arryClass;
    private ArrayStruct<int>? _arrayStruct;
    private IArray<int>? _array;
    private int[]? _nativeArray;

    [GlobalSetup]
    public void Setup() {
        var arr = new int[10000000];
        for (var i = 0; i < arr.Length; i++) {
            arr[i] = i;
        }

        _arryClass = new(arr);
        _arrayStruct = new(arr);
        _array = new Array<int>(arr);
        _nativeArray = arr;
    }

    [Benchmark]
    public int Array() {
        var count = 0;
        foreach (var i in _array!) {
            if (i % 2 == 0) {
                count++;
            }
        }

        return count;
    }

    [Benchmark]
    public int ArrayClass() {
        var count = 0;
        foreach (var i in _arryClass!) {
            if (i % 2 == 0) {
                count++;
            }
        }

        return count;
    }


    [Benchmark]
    public int ArrayStruct() {
        var count = 0;
        foreach (var i in _arrayStruct!) {
            if (i % 2 == 0) {
                count++;
            }
        }

        return count;
    }

    [Benchmark]
    public int Native() {
        var count = 0;
        foreach (var i in _nativeArray!) {
            if (i % 2 == 0) {
                count++;
            }
        }

        return count;
    }
}
