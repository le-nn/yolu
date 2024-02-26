using System.Runtime.CompilerServices;

namespace Yolu;

public static class SubscriberExtensions {
    /// <summary>
    /// Subscribes an action to the subject.
    /// </summary>
    /// <param name="observable">The observable sequence to subscribe to.</param>
    /// <param name="onNext">The action to invoke for each element in the observable sequence.</param>
    /// <param name="onError">The action to invoke upon exceptional termination of the observable sequence.</param>
    /// <param name="onComplete">The action to invoke upon graceful termination of the observable sequence.</param>
    /// <typeparam name="T">The type of elements in the observable sequence.</typeparam>
    /// <returns>A disposable object that can be used to unsubscribe the action.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IDisposable Subscribe<T>(
        this IObservable<T> observable,
        Action<T> onNext,
        Action<Exception>? onError = null,
        Action? onComplete = null
    ) {
        return observable.Subscribe(new Observer<T>(onNext, onError, onComplete));
    }
}
