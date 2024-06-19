using System.Collections.Concurrent;

namespace Yolu;

/// <summary>
/// Represents a subject that both observes and provides notifications.
/// </summary>
/// <typeparam name="T">The type of the elements being observed.</typeparam>
public class Subject<T> : IObservable<T>, IObserver<T> {
    private bool _didComplete;
    private Exception? _exception;
    private readonly ConcurrentDictionary<Guid, IObserver<T>> _observers = [];

    /// <summary>
    /// Subscribes an observer to the subject.
    /// </summary>
    /// <param name="observer">The observer to subscribe.</param>
    /// <returns>A disposable object that can be used to unsubscribe the observer.</returns>
    public IDisposable Subscribe(IObserver<T> observer) {
        if (_exception is not null) {
            observer.OnError(_exception);
        }
        else if (_didComplete) {
            observer.OnCompleted();
        }

        var id = Guid.NewGuid();
        _observers.TryAdd(id, observer);

        return new Subscription(() => _observers.TryRemove(id, out _));
    }

    /// <summary>
    /// Notifies all subscribed observers with a new value.
    /// </summary>
    /// <param name="value">The new value.</param>
    public void OnNext(T value) {
        if (_didComplete) {
            return;
        }

        foreach (var (_, v) in _observers) {
            v.OnNext(value);
        }
    }


    /// <summary>
    /// Notifies all subscribed observers that the provider has finished sending push-based notifications.
    /// </summary>
    public void OnCompleted() {
        if (_didComplete) {
            return;
        }

        foreach (var (_, value) in _observers) {
            value.OnCompleted();
        }


        _didComplete = true;
    }

    /// <summary>
    /// Notifies all subscribed observers that the provider has experienced an error condition.
    /// </summary>
    /// <param name="error">An object that provides additional information about the error.</param>
    public void OnError(Exception error) {
        if (_didComplete) {
            return;
        }


        foreach (var (_, value) in _observers) {
            value.OnError(error);
        }

        _exception = error;
        _didComplete = true;
    }

    /// <summary>
    /// Subscribes an action to the subject.
    /// </summary>
    /// <param name="action">The action to subscribe.</param>
    /// <returns>A disposable object that can be used to unsubscribe the action.</returns>
    public IDisposable Subscribe(Action<T> action) {
        return Subscribe(new Observer<T>(action, _ => { }, () => { }));
    }
}