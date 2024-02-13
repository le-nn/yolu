using Hadron.Tasks;
using System.Collections.Concurrent;

namespace Luminus.Tasks.Dispatching;

public class Dispatcher {
    private readonly ConcurrentQueue<Action> _actions = new();
    private CancellationTokenSource? _cancellation = new();
    private bool _isRunning = false;
    private readonly object _locker = new();

    public Thread Thread { get; }

    public Dispatcher() {
        Thread = new Thread(ProcessQueue);
        Thread.Start();

        _isRunning = true;
    }

    ~Dispatcher() {
        _isRunning = false;
        _cancellation?.Cancel();
        Thread.Join();
    }

    public Task InvokeAsync(Action action) {
        var task = TaskUtils.CreateTask((resolve, reject) => {
            _actions.Enqueue(() => {
                try {
                    action();
                    resolve();
                }
                catch (Exception ex) {
                    reject(ex);
                }
            });
        });

        _cancellation?.Cancel();

        return task;
    }

    private void ProcessQueue() {
        while (_isRunning) {
            while (_actions.TryDequeue(out var action)) {
                action();
            }

            lock (_locker) {
                _cancellation ??= new();
            }
            _cancellation.Token.WaitHandle.WaitOne(-1);
            _cancellation.Dispose();

            lock (_locker) {
                _cancellation = null;
            }
        }
    }
}