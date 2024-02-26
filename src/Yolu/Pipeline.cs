namespace Yolu;

/// <summary>
/// Represents pipeline operator with async operation.
/// </summary>
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
