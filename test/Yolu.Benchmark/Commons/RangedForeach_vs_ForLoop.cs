using BenchmarkDotNet.Attributes;
using Yolu.Collections;

namespace Yolu.Benchmark.Commons;

public class RangedForeach_vs_ForLoop {
    private readonly Array<int> _array = new(100_000_000, i => i);

    [Benchmark]
    public int ForLoop() {
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
    public int RangedForeach() {
        var count = 0;
        var arr = _array;
        foreach (var i in 0..^arr.Length) {
            if (arr[i] % 2 == 0) {
                count++;
            }
        }

        return count;
    }
}
