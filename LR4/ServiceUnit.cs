using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static LR4.Applications;

/*
ref-ки для очереди, LongRunning для задач
*/

namespace LR4
{
    // Обслуживающий аппарат. Использует шаблон проектирования Singleton.
    internal class ServiceUnit
    {
        private static ServiceUnit _instance;

        private List<IQueue<float>> _queues;     // Обслуживаемые очереди
        private List<int> _servedQueuesCounts;   // Кол-во обслуженных очередей
        private List<Task> _tasks;               // Задачи, выполняющие очереди

        private const int _TimeFactor = 10;   // Скорость симуляции, [1;1000] (меньше - лучше)
        private const int _LogRate = 100;     // Частота информации, [1;1000] (больше - лучше)

        private ServiceUnit()
        {
            _queues = new List<IQueue<float>>();
            _servedQueuesCounts = new List<int>();
            _tasks = new List<Task>();
        }

        public static ServiceUnit Instance => _instance ?? (_instance = new ServiceUnit());


        // Общее время простоя.
        public float TimeSpentIdle { get; private set; }

        // Общее время обслуживания.
        public float TimeSpentWorking { get; private set; }

        // Счётчик вышедших заявок.
        public int ServedApplicationsCount { get; private set; }

        // Счётчик вошедших заявок.
        public int ApplicationsCount { get; private set; }

        // Добавляет очередь на обслуживание.
        public void AddQueue(IQueue<float> queue)
        {
            _queues.Add(queue);
        }

        // Начинает обслуживание.
        public void Start()
        {
            if (_queues.Count == 0)
                throw new InvalidOperationException("Нечего обслуживать");

            foreach (var queue in _queues)
            {
                var task = Task.Run(() => ServeQueueForever(queue));
                _tasks.Add(task);
            }

            Task.WaitAll(_tasks.ToArray());
        }


        // Работает с очередью заявок.
        private void ServeQueueForever(IQueue<float> queue)
        {
            for (;;)
            {
                AcceptApplication(queue, NextApplication);
                ServeApplication(queue, Application);
            }
        }

        // Принимает одну заявку.
        private void AcceptApplication(IQueue<float> queue, float timeUnits)
        {
            int time = NormalizeTime(timeUnits);

            Work(time);
            TimeSpentIdle += time;

            queue.Enqueue(Application);
            ApplicationsCount++;
        }

        // Обслуживает одну заявку.
        private void ServeApplication(IQueue<float> queue, float timeUnits)
        {
            int time = NormalizeTime(timeUnits);

            Work(time);
            TimeSpentWorking += time;

            queue.Dequeue();
            _servedQueuesCounts.Add(queue.Count);

            if (++ServedApplicationsCount % _LogRate == 0)
            {
                PrintLog(queue);
            }

            if (!ShouldReapply) return;

            queue.Enqueue(Application);
            ApplicationsCount++;      // Считать ли вернувшуюся заявку?
        }

        // Симулирует работу.
        private void Work(int millisecondsWork)
        {
            Thread.Sleep(millisecondsWork);
        }

        // Преобразует единицы времени во время.
        private int NormalizeTime(float timeUnits)
        {
            float time = timeUnits * _TimeFactor;
            return (int) Math.Round(time);
        }

        // Выводит отладочную информацию.
        private void PrintLog(IQueue<float> queue)
        {
            char filler = '=';

            Console.WriteLine(queue is ArrayQueue<float> ? "Очередь-массив" : "Очередь-список");
            Console.Write(new string(filler, Console.WindowWidth));
            Console.WriteLine($"Текущая длина очереди: {queue.Count}");
            Console.WriteLine($"Средняя длина очереди: {_servedQueuesCounts.Average()}");

            Console.WriteLine($"{nameof(ServedApplicationsCount)}: {ServedApplicationsCount}");
            Console.WriteLine($"{nameof(ApplicationsCount)}: {ApplicationsCount}");
            Console.WriteLine($"{nameof(TimeSpentWorking)}: {TimeSpentWorking}");
            Console.WriteLine($"{nameof(TimeSpentIdle)}: {TimeSpentIdle}");

            Console.WriteLine(new string(filler, Console.WindowWidth));
        }
    }
}
