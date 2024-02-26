using BenchmarkDotNet.Attributes;
using Yolu.Benchmark.Collections.Internal;
using Yolu.Collections;

namespace Yolu.Benchmark.Collections.Arrays;

public class Forloop_IndexAccess_Benchmark {
    private Array<int>? _arrayClass;
    private ArrayStruct<int>? _arrayStruct;
    private IArray<int>? _array;
    private int[]? _nativeArray;

    [GlobalSetup]
    public void Setup() {
        var arr = new int[10000000];
        for (var i = 0; i < arr.Length; i++) {
            arr[i] = i;
        }

        _arrayClass = new(arr);
        _arrayStruct = new(arr);
        _array = new Array<int>(arr);
        _nativeArray = arr;
    }

    [Benchmark]
    public int Array() {
        var count = 0;
        var arr = _array!;
        for (var i = 0; i < arr.Count; i++) {
            if (arr[i] % 2 == 0) {
                count++;
            }
        }

        return count;
    }

    [Benchmark]
    public int ArrayClass() {
        var count = 0;
        var arr = _arrayClass!;
        for (var i = 0; i < arr.Count; i++) {
            if (arr[i] % 2 == 0) {
                count++;
            }
        }

        return count;
    }

    [Benchmark]
    public int ArrayStruct() {
        var count = 0;
        var arr = _arrayStruct ?? default;
        for (var i = 0; i < arr.Count; i++) {
            if (arr[i] % 2 == 0) {
                count++;
            }
        }

        return count;
    }

    [Benchmark]
    public int Native() {
        var count = 0;
        var arr = _nativeArray!;
        for (var i = 0; i < arr.Length; i++) {
            if (arr[i] % 2 == 0) {
                count++;
            }
        }

        return count;
    }
}
