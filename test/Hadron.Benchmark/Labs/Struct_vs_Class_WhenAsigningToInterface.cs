using BenchmarkDotNet.Attributes;

namespace Hadron.Benchmark.Labs;

public class Struct_vs_Class_WhenAsigningToInterface {
    [GlobalSetup]
    public void Setup() {

    }

    [Benchmark]
    public void ClassInterface_Test() {
        var classes = new List<IHello>();

        int count = 0;

        for (var i = 0; i < 100000; i++) {
            var mem = i;
            classes.Add(new Class(() => mem));
        }

        foreach (var c in classes) {
            count += c.Call();
        }
    }

    [Benchmark]
    public void Class_Test() {
        var classes = new List<Class>();

        int count = 0;

        for (var i = 0; i < 100000; i++) {
            var mem = i;
            classes.Add(new Class(() => mem));
        }

        foreach (var c in classes) {
            count += c.Call();
        }
    }

    [Benchmark]
    public void StructInterface_Test() {
        var classes = new List<IHello>();

        int count = 0;

        for (var i = 0; i < 100000; i++) {
            var mem = i;
            classes.Add(new Struct(() => mem));
        }

        foreach (var c in classes) {
            count += c.Call();
        }
    }

    [Benchmark]
    public void Struct_Test() {
        var classes = new List<Struct>();

        int count = 0;

        for (var i = 0; i < 100000; i++) {
            var mem = i;
            classes.Add(new Struct(() => mem));
        }

        foreach (var c in classes) {
            count += c.Call();
        }
    }

    private interface IHello {
        int Call();
    }

    public struct Struct(Func<int> count) : IHello {
        public Func<int>? _count = count;

        public int Call() {
            if (_count is null) {
                return 0;
            }

            return _count.Invoke();
        }
    }


    public class Class(Func<int> count) : IHello {
        public Func<int>? _count = count;

        public int Call() {
            if (_count is null) {
                return 0;
            }

            return _count.Invoke();
        }
    }
}

