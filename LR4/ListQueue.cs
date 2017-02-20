using System.Collections.Generic;

namespace LR4
{
    // Очередь, представленная в виде линейного связного списка.
    public class ListQueue<T> : IQueue<T>
    {
        private LinkedList<T> _list;      // Линейный список элементов

        // Создает очередь на основе связного списка.
        public ListQueue()
        {
            _list = new LinkedList<T>();
        }


        // Получает количество элементов в очереди.
        public int Count => _list.Count;

        // Удаляет все элементы из очереди.
        public void Clear()
        {
            _list.Clear();
        }

        // Удаляет элемент из начала очереди и возвращает его.
        public T Dequeue()
        {
            var item = _list.First.Value;
            _list.RemoveFirst();
            return item;
        }

        // Добавляет элемент в конец очереди.
        public void Enqueue(T item)
        {
            _list.AddLast(item);
        }

        // Возвращает элемент из начала очереди.
        public T Peek() => _list.First.Value;
    }
}
