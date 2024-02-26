using System.Collections.Concurrent;

namespace Yolu.Executors;
public class ThrottledExecutor<T> : IObservable<T> {
    private volatile int _lockFlag;
    private DateTime _lastInvokeTime;
    private Timer? _throttleTimer;
    private readonly ConcurrentDictionary<Guid, IObserver<T>> _observers = new();

    public ushort LatencyMs { get; set; } = 100;

    public ThrottledExecutor() {
        _lastInvokeTime = DateTime.UtcNow - TimeSpan.FromMilliseconds(ushort.MaxValue);
    }

    public IDisposable Subscribe(IObserver<T> observer) {
        var id = Guid.NewGuid();
        if (_observers.TryAdd(id, observer) is false) {
            throw new InvalidOperationException("Failed to add an observer.");
        }

        return new DisposableHandler(() => {
            if (_observers.TryRemove(new(id, observer)) is false) {
                throw new InvalidOperationException("Failed to add an observer.");
            }
        });
    }

    public IDisposable Subscribe(Action<T> action) {
        var observer = new Observer<T>(action);
        return Subscribe(observer);
    }

    public void Invoke(T value) {
        // If no throttle window then bypass throttling
        if (LatencyMs is 0) {
            ExecuteThrottledAction(value);
        }
        else {
            LockAndExecuteOnlyIfNotAlreadyLocked(() => {
                // If waiting for a previously throttled notification to execute
                // then ignore this notification request
                //if (InvokingSuspended)
                //    return;

                var millisecondsSinceLastInvoke =
                    (int)(DateTime.UtcNow - _lastInvokeTime).TotalMilliseconds;

                // If last execute was outside the throttle window then execute immediately
                if (millisecondsSinceLastInvoke >= LatencyMs) {
                    ExecuteThrottledAction(value);
                }
                else {
                    // This is exactly the second invoke within the time window,
                    // so set a timer that will trigger at the start of the next
                    // time window and prevent further invokes until
                    // the timer has triggered
                    _throttleTimer?.Dispose();
                    _throttleTimer = new Timer(
                        callback: _ => ExecuteThrottledAction(value),
                        state: null,
                        dueTime: LatencyMs - millisecondsSinceLastInvoke,
                        period: 0
                    );
                }
            });
        }
    }

    private void LockAndExecuteOnlyIfNotAlreadyLocked(Action action) {
        if (Interlocked.CompareExchange(ref _lockFlag, 1, 0) is 0) {
            try {
                action();
            }
            finally {
                _lockFlag = 0;
            }
        }
    }

    private void ExecuteThrottledAction(T value) {
        try {
            foreach (var (_, observer) in _observers) {
                observer.OnNext(value);
            }
        }
        finally {
            _throttleTimer?.Dispose();
            _throttleTimer = null;
            _lastInvokeTime = DateTime.UtcNow;
        }
    }
}