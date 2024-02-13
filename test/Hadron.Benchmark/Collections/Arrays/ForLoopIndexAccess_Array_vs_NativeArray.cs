using BenchmarkDotNet.Attributes;

namespace Hadron.Benchmark.Commons;

public class ForLoopIndexAccess_Array_vs_NativeArray {
    private readonly Array<int> _array = new(1000000, i => i);
    private readonly int[] _nativeArray = Enumerable.Range(0, 1000000).Select((x, i) => i).ToArray();
    private readonly List<int> _list = Enumerable.Range(0, 1000000).Select((x, i) => i).ToList();

    [Benchmark]
    public int ArrayBenchmark_with_Length() {
        var count = 0;
        var arr = _array;
        for (var i = 0; i < arr.Length; i++) {
            if (arr[i] % 2 == 0) {
                count++;
            }
        }

        return count;
    }

    [Benchmark]
    public int ArrayBenchmark_with_Count() {
        var count = 0;
        var arr = _array;
        for (var i = 0; i < arr.Count; i++) {
            if (arr[i] % 2 == 0) {
                count++;
            }
        }

        return count;
    }

    [Benchmark]
    public int ListBenchmark() {
        var count = 0;
        var arr = _list;
        for (var i = 0; i < arr.Count; i++) {
            if (arr[i] % 2 == 0) {
                count++;
            }
        }

        return count;
    }

    [Benchmark]
    public int NativeArrayBenchmark() {
        var count = 0;
        var arr = _nativeArray;
        for (var i = 0; i < arr.Length; i++) {
            if (arr[i] % 2 == 0) {
                count++;
            }
        }

        return count;
    }

    [Benchmark]
    public int NativeArray_with_Count_Benchmark() {
        var count = 0;
        var arr = _nativeArray;
        var length = arr.Length;
        for (var i = 0; i < length; i++) {
            if (arr[i] % 2 == 0) {
                count++;
            }
        }

        return count;
    }
}
