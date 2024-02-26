namespace Yolu;

/// <summary>
/// Represents Express functional programming-like pipeline operators with method chains
/// </summary>
/// <code>
/// var result = Pipeline.Create(10)
///     .Pipe(x => x * 2)
///     .Pipe(x => x + 10)
///     .Pipe(x => x / 2)
///     .Pipe(x => x - 5)
///     .Execute();
/// Consol.WriteLine(result); // 20
/// </code>
public static class Pipeline {
    public static Pipeline<U> Create<U>(U value) {
        return new Pipeline<U>(() => value);
    }

    public static Pipeline<U> Create<U>(Func<U> func) {
        return new Pipeline<U>(() => func());
    }

    public static AsyncPipeline<U> Create<U>(Func<Task<U>> func) {
        return new AsyncPipeline<U>(() => func());
    }

    public static Pipeline<T> Pipe<T>(Func<T> func) {
        return new Pipeline<T>(() => func());
    }
}
