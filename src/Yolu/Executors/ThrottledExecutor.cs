using System.Collections.Concurrent;

namespace Yolu.Executors;

/// <summary>
/// A class that throttles execution of actions to ensure that they are not invoked more frequently than a specified latency.
/// It implements the IObservable<T> interface to allow observers to be notified when the action is invoked.
/// </summary>
/// <typeparam name="T">The type of the value being passed to the observers.</typeparam>
public class ThrottledExecutor<T> : IObservable<T> {
    private volatile int _lockFlag;
    private DateTime _lastInvokeTime;
    private Timer? _throttleTimer;
    private readonly ConcurrentDictionary<Guid, IObserver<T>> _observers = new();

    /// <summary>
    /// The latency in milliseconds between consecutive invokes. 
    /// Defaults to 100ms.
    /// </summary>
    public ushort LatencyMs { get; set; } = 100;

    /// <summary>
    /// Initializes a new instance of the <see cref="ThrottledExecutor{T}"/> class.
    /// </summary>
    public ThrottledExecutor() {
        _lastInvokeTime = DateTime.UtcNow - TimeSpan.FromMilliseconds(ushort.MaxValue);
    }

    /// <summary>
    /// Subscribes an observer to the executor, so they can receive notifications when actions are invoked.
    /// </summary>
    /// <param name="observer">The observer to subscribe.</param>
    /// <returns>A disposable that can be used to unsubscribe the observer.</returns>
    /// <exception cref="InvalidOperationException">Thrown when an observer cannot be added.</exception>
    public IDisposable Subscribe(IObserver<T> observer) {
        var id = Guid.NewGuid();
        if (_observers.TryAdd(id, observer) is false) {
            throw new InvalidOperationException("Failed to add an observer.");
        }

        return new Subscription(() => {
            if (_observers.TryRemove(new(id, observer)) is false) {
                throw new InvalidOperationException("Failed to remove an observer.");
            }
        });
    }

    /// <summary>
    /// Subscribes an action to the executor, so it can receive notifications when actions are invoked.
    /// </summary>
    /// <param name="action">The action to subscribe.</param>
    /// <returns>A disposable that can be used to unsubscribe the action.</returns>
    public IDisposable Subscribe(Action<T> action) {
        var observer = new Observer<T>(action);
        return Subscribe(observer);
    }

    /// <summary>
    /// Cancels the throttled executor and stops any further executions.
    /// </summary>
    public void Cancel() {
        _throttleTimer?.Dispose();
        _throttleTimer = null;
    }

    /// <summary>
    /// Invokes the specified value and ensures that the invocation is throttled according to the specified latency.
    /// If the throttle window has elapsed since the last invocation, the action is executed immediately. Otherwise,
    /// it is queued to execute after the throttle window elapses.
    /// </summary>
    /// <param name="value">The value to pass to the observers.</param>
    public void Invoke(T value) {
        // If no throttle window then bypass throttling
        if (LatencyMs is 0) {
            ExecuteThrottledAction(value);
        }
        else {
            LockAndExecuteOnlyIfNotAlreadyLocked(() => {
                var millisecondsSinceLastInvoke =
                    (int)(DateTime.UtcNow - _lastInvokeTime).TotalMilliseconds;

                // If last execute was outside the throttle window then execute immediately
                if (millisecondsSinceLastInvoke >= LatencyMs) {
                    ExecuteThrottledAction(value);
                }
                else {
                    // Set a timer to execute the action once the throttle window has passed
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

    /// <summary>
    /// Ensures that the provided action is executed only if the lock is not already acquired.
    /// </summary>
    /// <param name="action">The action to execute.</param>
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

    /// <summary>
    /// Executes the throttled action and notifies all the subscribed observers with the provided value.
    /// </summary>
    /// <param name="value">The value to pass to the observers.</param>
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