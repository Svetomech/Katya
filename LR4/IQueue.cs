namespace LR4
{
    public interface IQueue<T>
    {
        int Count { get; }

        void Clear();
        T Dequeue();
        void Enqueue(T item);
        T Peek();
    }
}