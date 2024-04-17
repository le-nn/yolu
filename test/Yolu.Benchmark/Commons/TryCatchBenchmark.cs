using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yolu.Benchmark.Commons;
public class TryCatchBenchmark {
    [Benchmark]
    public void TryCatch_Helper() {
        List<int> nums = [];

        for (var i = 0; i < 10_000; i++) {
            var a = TryCatchUtils.Try(() => Throws(1), (NotImplementedException _) => 0);
            nums.Add(a);
        }
    }

    [Benchmark]
    public void TryCatch_Statement_NotImplementedException() {
        List<int> nums = [];

        for (var i = 0; i < 10_000; i++) {
            int a;
            try {
                a = Throws(1);
            }
            catch (NotImplementedException) {
                a = 0;
            }

            nums.Add(a);
        }
    }


    int Throws(int a) {
        throw new NotImplementedException();
    }
}
