namespace Yolu.Collections;

public class FixedSizeQueue<T>(int size) {
    public int Size { get; private set; } = size;

    public IReadOnlyCollection<T> Items => Queue.ToList();

    private Queue<T> Queue { get; } = new Queue<T>(size);

    public void Enqueue(T item) {
        Queue.Enqueue(item);

        while (Queue.Count > Size) {
            Queue.TryDequeue(out _);
        }
    }

    public T Dequeue() {
        var result = Dequeue();
        return result;
    }
}