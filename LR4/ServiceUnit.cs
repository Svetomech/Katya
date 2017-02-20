using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static LR4.Applications;

namespace LR4
{
    // Обслуживающий аппарат. Использует шаблон проектирования Singleton.
    internal class ServiceUnit
    {
        private static ServiceUnit _instance;

        private List<IQueue<float>> _queues;     // Обслуживаемые очереди
        private List<int> _servedQueuesCounts;   // Кол-во обслуженных очередей
        private List<Task> _tasks;               // Задачи, выполняющие очереди

        private const int _TimeFactor = 10;   // Скорость симуляции, [1;1000] (меньше - лучше)                                                !!!value
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
                Task.Run(() => ServeQueueAsync(queue));
            }

            // запустить таймер времени
            throw new NotImplementedException();
        }

        // Заканчивает обслуживание.
        public void Stop()
        {
            // остановить задачи и глобальный таймер
            throw new NotImplementedException();
        }


        // Работает с очередью заявок.
        private async Task ServeQueueAsync(IQueue<float> queue)
        {
            var acceptTask = AcceptApplicationAsync(queue, NextApplication);
            var serveTask = ServeApplicationAsync(queue, Application);

            await acceptTask;
            await serveTask;
        }

        // Принимает одну заявку.
        private async Task AcceptApplicationAsync(IQueue<float> queue, float timeUnits)
        {
            int time = NormalizeTime(timeUnits);

            TimeSpentIdle += time;
            await WorkAsync(time);

            queue.Enqueue(Application);
            ApplicationsCount++;
        }

        // Обслуживает одну заявку.
        private async Task ServeApplicationAsync(IQueue<float> queue, float timeUnits)
        {
            int time = NormalizeTime(timeUnits);

            TimeSpentWorking += time;
            await WorkAsync(time);

            queue.Dequeue();

            if (++ServedApplicationsCount % _LogRate == 0)
            {
                PrintLog(queue);
            }

            if (!ShouldReapply) return;

            queue.Enqueue(Application);
            ApplicationsCount++;      // Считать ли вернувшуюся заявку?
        }

        // Симулирует работу.
        private async Task WorkAsync(int millisecondsWork)
        {
            await Task.Delay(millisecondsWork);
        }

        // Преобразует единицы времени во время.
        private int NormalizeTime(float timeUnits)
        {
            float time = timeUnits * _TimeFactor;
            return (int) Math.Round(time);
        }

        // Выводит отладочную информацию.                                                                                                !!!filler
        private void PrintLog(IQueue<float> queue)
        {
            char filler = '*';

            Console.WriteLine(new string(filler, Console.WindowWidth));
            Console.WriteLine($"Текущая длина очереди: {queue.Count}");
            Console.WriteLine($"Средняя длина очереди: {_servedQueuesCounts.Average()}");
            Console.WriteLine(new string(filler, Console.WindowWidth));
        }
    }
}
