namespace Hadron;

public class DisposableHandler(Action action) : Disposable, IDisposable {
    protected override void Dispose(bool disposing) {
        base.Dispose(disposing);

        action?.Invoke();
    }
}