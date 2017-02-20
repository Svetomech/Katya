using System;

namespace LR4
{
    // Очередь, представленная в виде одномерного массива. Внутри использует
    // кольцевую структуру, следовательно: включение - O(n), исключение - O(1).
    public class ArrayQueue<T> : IQueue<T>
    {
        private T[] _array;      // Одномерный массив элементов
        private int _head;       // Первый элемент в очереди
        private int _tail;       // Последний элемент в очереди
        private int _size;       // Количество элементов

        private const int _GrowFactor = 2;    // Во сколько раз увеличивать размер очереди при её переполнении
        private const int _MinimumGrow = 4;   // Минимальное количество элементов, на которое увеличить

        // Создает очередь с размером по-умолчанию.
        public ArrayQueue() : this(0) { }

        // Создает очередь заданного размера.
        public ArrayQueue(int capacity)
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException(nameof(capacity),
                    capacity, "Отрицательный размер");

            _array = new T[capacity];
            _head = 0;
            _tail = 0;
            _size = 0;
        }


        // Получает количество элементов в очереди.
        public int Count => _size;

        // Удаляет все элементы из очереди.
        public void Clear()
        {
            if (_head < _tail)
                Array.Clear(_array, _head, _size);
            else
            {
                Array.Clear(_array, _head, _array.Length - _head);
                Array.Clear(_array, 0, _tail);
            }

            _head = 0;
            _tail = 0;
            _size = 0;
        }

        // Удаляет элемент из начала очереди и возвращает его.
        public T Dequeue()
        {
            if (_size == 0)
                throw new InvalidOperationException("Пустая очередь");

            T removed = _array[_head];
            _array[_head] = default(T);
            _head = (_head + 1) % _array.Length;
            _size--;
            return removed;
        }

        // Добавляет элемент в конец очереди.
        public void Enqueue(T item)
        {
            if (_size == _array.Length)
            {
                int newcapacity = _array.Length * _GrowFactor;
                if (newcapacity < _array.Length + _MinimumGrow)
                {
                    newcapacity = _array.Length + _MinimumGrow;
                }
                SetCapacity(newcapacity);
            }

            _array[_tail] = item;
            _tail = (_tail + 1) % _array.Length;
            _size++;
        }

        // Возвращает элемент из начала очереди.
        public T Peek()
        {
            if (_size == 0)
                throw new InvalidOperationException("Пустая очередь");

            return _array[_head];
        }


        // Увеличивает или уменьшает внутренний буфер. 
        // Условие: capacity >= _size.
        private void SetCapacity(int capacity)
        {
            T[] newarray = new T[capacity];
            if (_size > 0)
            {
                if (_head < _tail)
                    Array.Copy(_array, _head, newarray, 0, _size);
                else
                {
                    Array.Copy(_array, _head, newarray, 0, _array.Length - _head);
                    Array.Copy(_array, 0, newarray, _array.Length - _head, _tail);
                }
            }

            _array = newarray;
            _head = 0;
            _tail = (_size == capacity) ? 0 : _size;
        }
    }
}